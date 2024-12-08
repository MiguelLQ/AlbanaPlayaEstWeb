// Nuevo controlador: RegistroRecurrenteController
using albanaPlayaEst.Models;
using albanaPlayaEst.Data;
using albanaPlayaEst.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace albanaPlayaEst.Controllers
{
    
    public class RegistroRecurrenteController : Controller
    {
        private readonly AlbanaDBcontext _context;

        public RegistroRecurrenteController(AlbanaDBcontext context)
        {
            _context = context;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor,Empleado")]
        public async Task<IActionResult> Index()
        {
            // Obtener los registros recurrentes activos (Espacios ocupados con Estado == true)
            var registrosRecurrentes = await _context.Registros
                .Include(r => r.CodEspNavigation)
                .Include(r => r.CodPagNavigation)
                .Include(r => r.CodVNavigation.CodCliNavigation)
                .Include(r => r.CodVNavigation)
                .Where(r => r.CodPagNavigation.EsPagoRecurrente == true && r.CodEspNavigation.Estad_esp == true) // Solo pagos recurrentes activos
                .GroupBy(r => r.CodEsp) // Agrupar por espacio
                .Select(g => g.OrderByDescending(r => r.CodPagNavigation.FechaPag) // Ordenar por fecha de pago más reciente
                    .FirstOrDefault()) // Tomar solo el registro más reciente por espacio
                .ToListAsync();

            // Verificar si no hay registros válidos
            if (!registrosRecurrentes.Any())
            {
                TempData["Mensaje"] = "No hay pagos recurrentes activos para mostrar.";
            }
            else
            {
                // Verificar si algún vehículo necesita renovar su plan (FechaHoraSalida pasada y el pago recurrente sigue activo)
                var vehículosNecesitanRenovación = registrosRecurrentes
                    .Where(r => r.FechaHoraSalida != null && r.FechaHoraSalida <= DateTime.Now 
                                                          && r.CodPagNavigation.Estado == true) // Pago activo, pero requiere renovación
                    .Select(r => r.CodVNavigation.PlacaV) // Seleccionar las placas de los vehículos
                    .ToList();

                // Si existen vehículos que necesitan renovar, agregar la alerta
                if (vehículosNecesitanRenovación.Any())
                {
                    TempData["AlertaRenovación"] = $"Los siguientes vehículos necesitan renovar su plan o pago recurrente: {string.Join(", ", vehículosNecesitanRenovación)}";
                }
            }

            // Pasar los registros recurrentes a la vista
            return View(registrosRecurrentes);
        }




        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> CargarParaNuevoPago(int id)
        {
            var registroExistente = await _context.Registros
                .Include(r => r.CodPagNavigation)
                .Include(r => r.CodEspNavigation)
                .Include(r => r.CodVNavigation)
                .FirstOrDefaultAsync(r => r.CodReg == id);

            if (registroExistente == null)
            {
                return NotFound();
            }

            // Crear DTO con datos del registro existente
            var model = new RegistroNuevoPagoDto
            {
                CodRegExistente = registroExistente.CodReg,
                FechaEntrada = registroExistente.FechaEntrada,
                FechaHoraSalida = registroExistente.FechaHoraSalida,
                CodEsp = registroExistente.CodEsp,
                CodV = registroExistente.CodV,
                MontoAnterior = registroExistente.CodPagNavigation.MontPag,
                EsPagoRecurrente = registroExistente.CodPagNavigation.EsPagoRecurrente
            };

            // Cargar métodos de pago
            ViewBag.MetodosPago = new SelectList(await _context.Metodpags.ToListAsync(), "CodMetd", "DescrMetd");

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarNuevoPago(RegistroNuevoPagoDto model)
        {
            if (!ModelState.IsValid)
            {
                // Recargar métodos de pago si hay un error de validación
                ViewBag.MetodosPago = new SelectList(await _context.Metodpags.ToListAsync(), "CodMetd", "DescrMetd");
                return View(model);
            }

            // Buscar el registro existente
            var registroExistente = await _context.Registros
                .Include(r => r.CodEspNavigation)
                .FirstOrDefaultAsync(r => r.CodReg == model.CodRegExistente);

            if (registroExistente == null)
            {
                return NotFound();
            }

            // Crear nuevo pago
            var nuevoPago = new Pago
            {
                MontPag = model.NuevoMonto,
                FechaPag = DateTime.Now, // Usar la fecha actual si no se proporciona
                Estado = true, // Asegurar que el estado sea true
                CodMetd = model.CodMetd,
                EsPagoRecurrente = true
            };

            // Guardar el nuevo pago
            _context.Pagos.Add(nuevoPago);
            await _context.SaveChangesAsync();

            // Crear nuevo registro con las fechas enviadas en el modelo
            var nuevoRegistro = new Registro
            {
                FechaEntrada = model.NuevaFechaEntrada ?? registroExistente.FechaEntrada, // Usar nueva fecha o conservar la anterior
                FechaHoraSalida = model.NuevaFechaHoraSalida ?? registroExistente.FechaHoraSalida, // Usar nueva fecha o conservar la anterior
                CodEsp = model.CodEsp,
                CodV = model.CodV,
                cod_Pag = nuevoPago.cod_Pag // Relacionar con el nuevo pago
            };

            // Guardar el nuevo registro
            _context.Registros.Add(nuevoRegistro);
            await _context.SaveChangesAsync();

            // Actualizar el estado del espacio si el pago es recurrente
            if (model.EsPagoRecurrente)
            {
                var espacio = await _context.Espacios.FindAsync(model.CodEsp);
                if (espacio != null)
                {
                    espacio.Estad_esp = true; // Marcar como ocupado
                    _context.Espacios.Update(espacio);
                    await _context.SaveChangesAsync();
                }
            }

            TempData["Exito"] = "El nuevo registro y el pago se han guardado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor,Empleado")]
        public async Task<IActionResult> ConfirmarCancelacion(int id)
        {
            // Buscar el registro correspondiente por ID
            var registro = await _context.Registros
                .Include(r => r.CodVNavigation)
                .Include(r => r.CodPagNavigation)
                .FirstOrDefaultAsync(r => r.CodReg == id);


            if (registro == null)
            {
                return NotFound();
            }
            
            // Pasar los datos del registro a la vista de confirmación
            return View(registro);
        }

        [HttpPost]
        public async Task<IActionResult> CancelarPlan(int id)
        {
            // Obtener el registro con las relaciones necesarias
            var registro = await _context.Registros
                .Include(r => r.CodEspNavigation) // Incluimos la relación con Espacio
                .Include(r => r.CodPagNavigation)
                .FirstOrDefaultAsync(r => r.CodReg == id);

            if (registro == null)
            {
                TempData["Error"] = "No se encontró el registro.";
                return RedirectToAction(nameof(Index));
            }

            // Cambiar el estado del pago recurrente a false
            if (registro.CodPagNavigation != null)
            {
                registro.CodPagNavigation.EsPagoRecurrente = false;
            }

            // Cambiar el estado del espacio a false
            if (registro.CodEspNavigation != null)
            {
                registro.CodEspNavigation.Estad_esp = false;
            }

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "El plan recurrente ha sido cancelado y el espacio ha sido liberado.";
            return RedirectToAction(nameof(Index));
        }


    }
}

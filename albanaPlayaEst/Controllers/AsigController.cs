using Microsoft.AspNetCore.Mvc;
using albanaPlayaEst.Dto;
using albanaPlayaEst.Models;
using System.Linq;
using System;
using albanaPlayaEst.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using albanaPlayaEst.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using X.PagedList.Extensions;
using X.PagedList.Mvc.Core;
namespace albanaPlayaEst.Controllers
{
    public class AsigController : Controller
    {
        private readonly AlbanaDBcontext _context;
        private readonly VehiculoServices _vehiculoServices;
        private readonly IFiltrarPorPlacaService _filtrarPorPlacaService;
        private readonly MetodoPagoService _metodoPagoService; // Inyectar el servicio
        public AsigController(AlbanaDBcontext context, VehiculoServices vehiculoServices, IFiltrarPorPlacaService filtrarPorPlacaService, MetodoPagoService metodoPagoService)
        {
            _context = context;
            _vehiculoServices = vehiculoServices;
            _filtrarPorPlacaService = filtrarPorPlacaService;
            _metodoPagoService = metodoPagoService;
        }
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor,Empleado")]
        public IActionResult Index(string descripcion, string ubicacion, int? page)
        {
            // Recuperar las descripciones únicas de los espacios para el filtro
            var descripciones = _context.Espacios
                .Where(e => !string.IsNullOrEmpty(e.Descr_esp))
                .Select(e => e.Descr_esp)
                .Distinct()
                .ToList();

            var ubicaciones = _context.Espacios
                .Where(e => !string.IsNullOrEmpty(e.Ubi_esp))
                .Select(e => e.Ubi_esp)
                .Distinct()
                .ToList();

            // Asignar datos al ViewBag para la vista
            ViewBag.Descripciones = new SelectList(descripciones);
            ViewBag.Ubicaciones = new SelectList(ubicaciones);

            ViewBag.SelectedDescripcion = descripcion;
            ViewBag.SelectedUbicacion = ubicacion;

            // Definir la consulta
            var espaciosQuery = _context.Espacios
                .Where(e => (string.IsNullOrEmpty(descripcion) || e.Descr_esp.Contains(descripcion)) &&
                            (string.IsNullOrEmpty(ubicacion) || e.Ubi_esp.Contains(ubicacion)))
                .Select(e => new AsignarVDto
                {
                    Cod_esp = e.Cod_esp,
                    Estad_esp = e.Estad_esp,
                    Ubi_esp = e.Ubi_esp,
                    Descr_esp = e.Descr_esp,
                    Cost_esp = e.Cost_esp,
                    CodReg = _context.Registros
                                .Where(r => r.CodEsp == e.Cod_esp)
                                .OrderByDescending(r => r.FechaEntrada)
                                .Select(r => r.CodReg)
                                .FirstOrDefault(),
                    MarcV = _context.Registros
                                .Where(r => r.CodEsp == e.Cod_esp)
                                .OrderByDescending(r => r.FechaEntrada)
                                .Select(r => r.CodVNavigation.MarcV)
                                .FirstOrDefault() ?? "Sin vehículo",
                    ModelV = _context.Registros
                                .Where(r => r.CodEsp == e.Cod_esp)
                                .OrderByDescending(r => r.FechaEntrada)
                                .Select(r => r.CodVNavigation.ModelV)
                                .FirstOrDefault() ?? "Sin vehículo",
                    PlacaV = _context.Registros
                                .Where(r => r.CodEsp == e.Cod_esp)
                                .OrderByDescending(r => r.FechaEntrada)
                                .Select(r => r.CodVNavigation.PlacaV)
                                .FirstOrDefault() ?? "Sin vehículo"
                });

            // Configurar paginación
            int pageSize = 8; // Número de elementos por página
            int pageNumber = page ?? 1; // Número de página actual

            // Convertir a IPagedList
            var pagedEspacios = espaciosQuery.ToPagedList(pageNumber, pageSize);

            return View(pagedEspacios);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor,Empleado")]
        public IActionResult AsignarV(int id)
        {
            // Busca el espacio por su ID
            var espacio = _context.Espacios
                .FirstOrDefault(e => e.Cod_esp == id);
    
            // Obtener los métodos de pago disponibles
            var metodosPago = _context.Metodpags.Select(m => new SelectListItem
            {
                Value = m.CodMetd.ToString(),
                Text = m.DescrMetd
            }).ToList();

            // Validar si el espacio existe
            if (espacio == null)
            {
                // Si el espacio no existe, retornar NotFound con un mensaje adecuado
                Console.WriteLine("Espacio no encontrado.");
                return NotFound("El espacio solicitado no existe.");
            }

            // Validar si el espacio está ocupado
            Console.WriteLine($"Estado del espacio (0=libre, 1=ocupado): {espacio.Estad_esp}");
            if (espacio.Estad_esp) // 1 significa que el espacio está ocupado
            {
                // Si el espacio ya está ocupado, retornar un mensaje adecuado
                Console.WriteLine("Espacio ya ocupado.");
                return NotFound("Este espacio ya está ocupado.");
            }

            // Crear un DTO para pasar la información a la vista
            var asignarVDto = new AsignarVDto
            {
                Cod_esp = espacio.Cod_esp,
                Estad_esp = espacio.Estad_esp, // Debería ser 0 en este caso, ya que está libre
                Ubi_esp = espacio.Ubi_esp,
                Descr_esp = espacio.Descr_esp,
                Cost_esp = espacio.Cost_esp
            };

            // Pasar los métodos de pago a la vista
            ViewData["MetodosPago"] = metodosPago;

            Console.WriteLine($"Espacio válido: {asignarVDto.Cod_esp}, Estado: {asignarVDto.Estad_esp}");
    
            // Retornar la vista con el DTO, para que el usuario pueda ver y asignar un vehículo al espacio
            return View(asignarVDto);
        }

        
        [HttpPost]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        [Route("AsigController/BuscarVehiculoPorPlacaAjax")]
        public IActionResult BuscarVehiculoPorPlacaAjax(string placa)
        {
            var vehiculo = _context.Vehículos
                .Where(v => v.PlacaV == placa)
                .Include(v => v.CodTipVNavigation)  // Asegúrate de incluir la relación con Tipovehic
                .Include(v => v.CodCliNavigation)    // Asegúrate de incluir la relación con Cliente
                .Select(v => new
                {
                    v.PlacaV,
                    v.MarcV,
                    v.ModelV,
                    v.ColorV,
                    v.CodTipVNavigation.DescrTipV, // Aquí se accede a Tipovehic
                    Cliente = new
                    {
                        v.CodCliNavigation.Nom_cliente,
                        v.CodCliNavigation.Apell_cliente,
                        v.CodCliNavigation.Tel_cliente,
                        v.CodCliNavigation.Dni_cliente
                    }
                })
                .FirstOrDefault();

            if (vehiculo == null)
            {
                return Json(new { success = false, message = "Vehículo no encontrado." });
            }

            return Json(new { success = true, vehiculo });
        }
        [HttpPost]
        [Authorize(Roles = "Administrador,Supervisor,Empleado")]
        [ValidateAntiForgeryToken]
        public IActionResult AsignarV(AsignarVDto asignarVDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Buscar el vehículo por su placa
                    var vehiculo = _context.Vehículos
                        .Include(v => v.CodCliNavigation)
                        .FirstOrDefault(v => v.PlacaV == asignarVDto.PlacaV);

                    if (vehiculo == null)
                    {
                        return Json(new { success = false, error = "Vehículo no encontrado." });
                    }

                    var espacio = _context.Espacios.FirstOrDefault(e => e.Cod_esp == asignarVDto.Cod_esp);
                    if (espacio == null || espacio.Estad_esp == true)
                    {
                        return Json(new { success = false, error = "Este espacio ya está ocupado o no existe." });
                    }

                    // Crear el registro de la asignación
                    var nuevoRegistro = new Registro
                    {
                        CodV = vehiculo.CodV,
                        CodEsp = asignarVDto.Cod_esp,
                        FechaEntrada = asignarVDto.FechaEntrada ?? DateTime.Now,
                        FechaHoraSalida = asignarVDto.FechaHoraSalida,
                        cod_Pag = asignarVDto.cod_Pag // Aún no asignamos el pago aquí
                    };

                    // Agregar el nuevo registro
                    _context.Registros.Add(nuevoRegistro);
                    _context.SaveChanges();

                    // Verificar si el registro fue creado correctamente
                    if (nuevoRegistro.CodReg == 0)
                    {
                        return Json(new { success = false, error = "No se pudo crear el registro correctamente." });
                    }

                    // Crear el pago (si se proporcionan datos para pago recurrente, sino, se maneja sin ellos)
                    var nuevoPago = new Pago
                    {
                        MontPag = asignarVDto.MontPag,
                        CodMetd = asignarVDto.CodMetd,
                        // Asignar DateTime.Now si FechaInicio no se proporciona
                        FechaPag = DateTime.Now, // Siempre se asigna como DateTime.Now
                        // Asignar DateTime.Now si FechaFin no se proporciona
                        // Asignar "false" si no se selecciona EsPagoRecurrente
                        EsPagoRecurrente =  asignarVDto.EsPagoRecurrente,
                        // Asignar true por defecto si no se proporciona el Estado
                        Estado = true
                    };
                    
                    // Guardar el pago
                    _context.Pagos.Add(nuevoPago);
                    _context.SaveChanges();

                    // Asignar el pago al registro
                    nuevoRegistro.cod_Pag = nuevoPago.cod_Pag;
                    _context.SaveChanges();

                    // Actualizar el estado del espacio
                    espacio.Estad_esp = true;
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Asig");
                }
                catch (Exception ex)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    foreach (var error in errors)
                    {
                        Console.WriteLine(error);
                    }
                    return Json(new { success = false, error = "Datos inválidos." });
                    // Log the exception for further debugging
                }
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, error = string.Join(", ", errors) });
            }

            return Json(new { success = false, error = "Los datos del formulario no son válidos." });
        }


        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public IActionResult DetalleAsig(int id)
        {
            Console.WriteLine($"ID de asignación: {id}");

            // Buscar el registro de asignación
            var registro = _context.Registros
                .Include(r => r.CodEspNavigation) // Incluye el espacio
                .Include(r => r.CodVNavigation) // Incluye el vehículo
                .ThenInclude(v => v.CodTipVNavigation) // Incluye el tipo de vehículo
                .Include(r => r.CodVNavigation.CodCliNavigation) // Incluye el cliente
                .Include(r => r.CodPagNavigation) // Asegúrate de incluir la relación con pago
                .FirstOrDefault(r => r.CodReg == id);

            if (registro == null)
            {
                return NotFound("Asignación no encontrada.");
            }

            // Verificación de null para relaciones de navegación
            if (registro.CodEspNavigation == null || registro.CodVNavigation == null ||
                registro.CodVNavigation.CodCliNavigation == null || registro.CodVNavigation.CodTipVNavigation == null)
            {
                return NotFound("Datos incompletos de la asignación.");
            }

            // Manejo de 'CodPagNavigation' y 'FechaHoraSalida' si son null
            decimal? montoPago = registro.CodPagNavigation != null ? registro.CodPagNavigation.MontPag : (decimal?)null;
            DateTime? fechaHoraSalida = registro.FechaHoraSalida ?? null; // Si es null, será null

            // Rellenar el DTO con los datos
            var detalleDto = new DetalleAsignacionDto
            {
                Descr_esp = registro.CodEspNavigation.Descr_esp,
                Ubi_esp = registro.CodEspNavigation.Ubi_esp,
                Estad_esp = registro.CodEspNavigation.Estad_esp ? true : false,
                PlacaV = registro.CodVNavigation.PlacaV,
                MarcV = registro.CodVNavigation.MarcV,
                ModelV = registro.CodVNavigation.ModelV,
                ColorV = registro.CodVNavigation.ColorV,
                Nom_cliente = registro.CodVNavigation.CodCliNavigation?.Nom_cliente, // Usar el operador ? para evitar null
                Apell_cliente = registro.CodVNavigation.CodCliNavigation?.Apell_cliente,
                Tel_cliente = registro.CodVNavigation.CodCliNavigation?.Tel_cliente,
                DescrTipV = registro.CodVNavigation.CodTipVNavigation?.DescrTipV, // Evitar acceso a propiedades de null
                FechaEntrada = registro.FechaEntrada ?? DateTime.Now, // Usar DateTime.Now si FechaEntrada es null
                FechaHoraSalida = registro.FechaHoraSalida, // Asignar fechaHoraSalida que puede ser null
                MontPag = registro.CodPagNavigation?.MontPag ?? 0  // Asignar montoPago que puede ser null
            };

            return View(detalleDto);
        }
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor,Empleado")]
        public IActionResult EspaciosOcupados()
        {
            // Consulta para obtener los registros de espacios ocupados con pagos normales (no recurrentes)
            var registrosOcupados = _context.Registros
                .Include(r => r.CodEspNavigation)
                .Include(r => r.CodVNavigation)
                    .ThenInclude(v => v.CodCliNavigation)
                .Include(r => r.CodVNavigation.CodTipVNavigation)
                .Include(r => r.CodPagNavigation) // Incluir relación con pago
                .Where(r => r.CodEspNavigation.Estad_esp == true && 
                            r.CodPagNavigation.EsPagoRecurrente == false) // Solo pagos normales
                .ToList();

            // Agrupar por espacio y obtener el registro más reciente de cada uno
            var espaciosOcupadosDto = registrosOcupados
                .GroupBy(r => r.CodEsp) // Agrupar por código de espacio
                .Select(g => g.OrderByDescending(r => r.CodReg) // Ordenar por el registro más reciente
                    .FirstOrDefault()) // Seleccionar el registro más reciente por espacio
                .Select(r => new AsignarVDto
                {
                    CodReg = r.CodReg, // Código del registro
                    Cod_esp = r.CodEsp,
                    Descr_esp = r.CodEspNavigation.Descr_esp,
                    Estad_esp = r.CodEspNavigation.Estad_esp,
                    PlacaV = r.CodVNavigation.PlacaV,
                    MarcV = r.CodVNavigation.MarcV,
                    ModelV = r.CodVNavigation.ModelV,
                    ColorV = r.CodVNavigation.ColorV,
                    Nom_cliente = r.CodVNavigation.CodCliNavigation.Nom_cliente,
                    Apell_cliente = r.CodVNavigation.CodCliNavigation.Apell_cliente,
                    Tel_cliente = r.CodVNavigation.CodCliNavigation.Tel_cliente,
                    DescrTipV = r.CodVNavigation.CodTipVNavigation.DescrTipV,
                    FechaEntrada = r.FechaEntrada
                })
                .ToList();

            // Validar si no hay registros válidos
            if (!espaciosOcupadosDto.Any())
            {
                ViewBag.Mensaje = "No hay espacios ocupados actualmente con pagos normales.";
            }

            // Retornar la vista con los datos de los espacios ocupados
            return View(espaciosOcupadosDto);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor,Empleado")]
        public IActionResult FiltrarPorPlaca(string placa)
        {
            if (string.IsNullOrEmpty(placa))
            {
                // Si no se proporciona una placa, redirigir a la vista de espacios ocupados
                return RedirectToAction("EspaciosOcupados");
            }

            // Obtener los registros filtrados por la placa
            var registrosFiltrados = _filtrarPorPlacaService.FiltrarPorPlaca(placa);

            // Validar si algún registro tiene un pago recurrente
            if (registrosFiltrados.Any(r => r.EsPagoRecurrente))
            {
                TempData["Mensaje"] = "El vehículo con esa placa tiene un pago recurrente y no puede ser buscado.";
                return RedirectToAction("EspaciosOcupados");
            }

            // Agrupar por espacio y vehículo, y seleccionar el registro más reciente
            var registrosFiltradosDto = registrosFiltrados
                .GroupBy(r => new { r.Cod_esp, r.CodV }) // Agrupar por espacio y vehículo
                .Select(g => g.OrderByDescending(r => r.CodReg) // Ordenar por el código del registro (más reciente)
                    .FirstOrDefault()) // Tomar solo el más reciente
                .Select(r => new AsignarVDto
                {
                    CodReg = r.CodReg, // Código del registro
                    Cod_esp = r.Cod_esp, // Código del espacio
                    Descr_esp = r.Descr_esp, // Descripción del espacio
                    Estad_esp = r.Estad_esp, // Estado del espacio
                    PlacaV = r.PlacaV, // Placa del vehículo
                    MarcV = r.MarcV, // Marca del vehículo
                    ModelV = r.ModelV, // Modelo del vehículo
                    ColorV = r.ColorV, // Color del vehículo
                    Nom_cliente = r.Nom_cliente, // Nombre del cliente
                    Apell_cliente = r.Apell_cliente, // Apellido del cliente
                    Tel_cliente = r.Tel_cliente, // Teléfono del cliente
                    DescrTipV = r.DescrTipV, // Descripción del tipo de vehículo
                    FechaEntrada = r.FechaEntrada // Fecha de entrada
                })
                .ToList();

            if (!registrosFiltradosDto.Any())
            {
                TempData["Mensaje"] = "No se encontraron vehículos con esa placa.";
            }

            // Retornar la vista de espacios ocupados con los registros filtrados
            return View("EspaciosOcupados", registrosFiltradosDto);
        }


        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor,Empleado")]
        public IActionResult LiberarEspacio(int id)
        {
            var registro = _context.Registros
                .Include(r => r.CodEspNavigation)
                .Include(r => r.CodPagNavigation)
                .Include(r => r.CodPagNavigation.CodMetdNavigation)
                .Include(r => r.CodVNavigation)
                .Include(r => r.CodVNavigation.CodCliNavigation)
                .Include(r => r.CodVNavigation.CodTipVNavigation)
                .FirstOrDefault(r => r.CodEsp == id && r.CodEspNavigation.Estad_esp == true); // Buscar el espacio ocupado

            if (registro == null)
            {
                return NotFound();
            }

            // Obtener los métodos de pago disponibles
            var metodosPago = _context.Metodpags.Select(m => new SelectListItem
            {
                Value = m.CodMetd.ToString(),
                Text = m.DescrMetd
            }).ToList();

            // Transformar en un DTO
            var liberarDto = new AsignarVDto
            {
                CodMetd = registro.CodPagNavigation.CodMetd,
                CodTipV = registro.CodVNavigation.CodTipV,
                CodV = registro.CodVNavigation.CodV,
                Cod_cliente = registro.CodVNavigation.Cod_cliente,
                cod_Pag = registro.cod_Pag,
                CodReg = registro.CodReg,
                Cod_esp = registro.CodEsp,
                Descr_esp = registro.CodEspNavigation.Descr_esp,
                Estad_esp = registro.CodEspNavigation.Estad_esp,
                PlacaV = registro.CodVNavigation.PlacaV,
                MarcV = registro.CodVNavigation.MarcV,
                ModelV = registro.CodVNavigation.ModelV,
                ColorV = registro.CodVNavigation.ColorV,
                Nom_cliente = registro.CodVNavigation.CodCliNavigation.Nom_cliente,
                Apell_cliente = registro.CodVNavigation.CodCliNavigation.Apell_cliente,
                Tel_cliente = registro.CodVNavigation.CodCliNavigation.Tel_cliente,
                Dni_cliente = registro.CodVNavigation.CodCliNavigation.Dni_cliente,
                DescrTipV = registro.CodVNavigation.CodTipVNavigation.DescrTipV,
                FechaEntrada = registro.FechaEntrada,
                FechaHoraSalida = registro.FechaHoraSalida,
                MontPag = registro.CodPagNavigation.MontPag,
                FechaFin = registro.FechaHoraSalida// Si ya existe un pago previo, lo muestra
            };

            // Pasar los métodos de pago a la vista
            ViewData["MetodosPago"] = metodosPago;

            return View(liberarDto);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador,Supervisor,Empleado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LiberarEspacio(int CodReg, int Cod_esp, int cod_Pag, int CodCliente, int CodV, int CodTipV, string PlacaV, DateTime FechaHoraSalida, decimal MontPag, int CodMetd)
        {
            // Verificar que el modelo es válido
            if (!ModelState.IsValid)
            {
                // Imprimir los errores de validación
                foreach (var state in ModelState)
                {
                    Console.WriteLine($"Propiedad: {state.Key}, Errores: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return Json(new { success = false, errorMessage = "Datos del formulario no válidos o incompletos." });
            }

            try
            {
                // Buscar el registro asociado con el CodReg
                var registro = await _context.Registros
                    .Include(r => r.CodEspNavigation)
                    .Include(r => r.CodPagNavigation)
                    .FirstOrDefaultAsync(r => r.CodReg == CodReg);

                if (registro == null)
                {
                    return Json(new { success = false, errorMessage = "Registro no encontrado." });
                }

                // Verificar si el pago ya existe y actualizarlo si es necesario
                if (registro.cod_Pag != null)
                {
                    var pagoExistente = await _context.Pagos.FirstOrDefaultAsync(p => p.cod_Pag == registro.cod_Pag);
                    if (pagoExistente != null)
                    {
                        // Si el monto o el método de pago fueron modificados, actualizamos
                        if (pagoExistente.MontPag != MontPag || pagoExistente.CodMetd != CodMetd)
                        {
                            pagoExistente.MontPag = MontPag;
                            pagoExistente.CodMetd = CodMetd;

                            // Actualizar el pago
                            _context.Pagos.Update(pagoExistente);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    // Si no hay pago existente, creamos uno nuevo solo si MontPag es mayor a 0
                    if (MontPag > 0)
                    {
                        var nuevoPago = new Pago
                        {
                            MontPag = MontPag,
                            CodMetd = CodMetd,
                            FechaPag = DateTime.Now,
                            Estado = false // Inicialmente, el estado está en "false"
                        };

                        // Guardar el pago en la base de datos
                        _context.Pagos.Add(nuevoPago);
                        await _context.SaveChangesAsync();

                        // Asociar el pago al registro
                        registro.cod_Pag = nuevoPago.cod_Pag;
                    }
                }

                // Liberar el espacio
                var espacio = await _context.Espacios.FindAsync(Cod_esp);
                if (espacio == null)
                {
                    return Json(new { success = false, errorMessage = "Espacio no encontrado." });
                }
                espacio.Estad_esp = false;  // O cualquier otro valor que represente "liberado"

                // Actualizar la fecha de salida en el registro
                registro.FechaHoraSalida = FechaHoraSalida;

                // Actualizar el registro y liberar el espacio
                _context.Registros.Update(registro);
                _context.Espacios.Update(espacio);

                // Guardar los cambios
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al liberar el espacio: " + ex.Message);
                return Json(new { success = false, errorMessage = "Error al liberar el espacio. Detalles: " + ex.Message });
            }
        }
        
    }
}

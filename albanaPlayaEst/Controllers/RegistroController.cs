using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using albanaPlayaEst.Data;
using albanaPlayaEst.Models;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;

namespace albanaPlayaEst.Controllers
{
    public class RegistroController : Controller
    {
        private readonly AlbanaDBcontext _context;

        public RegistroController(AlbanaDBcontext context)
        {
            _context = context;
        }

        // GET: Registro
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> Index()
        {
            var albanaDBcontext = _context.Registros.Include(r => r.CodEspNavigation).Include(r => r.CodPagNavigation).Include(r => r.CodVNavigation).Include(r => r.CodVNavigation.CodCliNavigation);
            return View(await albanaDBcontext.ToListAsync());
        }
        // GET: Registro/Details/5
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registro = await _context.Registros
                .Include(r => r.CodEspNavigation)
                .Include(r => r.CodPagNavigation)
                .Include(r => r.CodVNavigation.CodCliNavigation)
                .FirstOrDefaultAsync(m => m.CodReg == id);
            if (registro == null)
            {
                return NotFound();
            }

            return View(registro);
        }

        // GET: Registro/Create
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewData["CodEsp"] = new SelectList(_context.Espacios, "Cod_esp", "Ubi_esp");
            ViewData["cod_Pag"] = new SelectList(_context.Pagos, "cod_Pag", "cod_Pag");
            ViewData["CodV"] = new SelectList(_context.Vehículos, "CodV", "V");
            return View();
        }

        // POST: Registro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodReg,FechaEntrada,FechaHoraSalida,CodV,CodEsp,cod_Pag")] Registro registro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodEsp"] = new SelectList(_context.Espacios, "Cod_esp", "Ubi_esp", registro.CodEsp);
            ViewData["cod_Pag"] = new SelectList(_context.Pagos, "cod_Pag", "cod_Pag", registro.cod_Pag);
            ViewData["CodV"] = new SelectList(_context.Vehículos, "CodV", "PlacaV", registro.CodV);
            return View(registro);
        }

        // GET: Registro/Edit/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registro = await _context.Registros.FindAsync(id);
            if (registro == null)
            {
                return NotFound();
            }
            ViewData["CodEsp"] = new SelectList(_context.Espacios, "Cod_esp", "Ubi_esp", registro.CodEsp);
            ViewData["cod_Pag"] = new SelectList(_context.Pagos, "cod_Pag", "cod_Pag", registro.cod_Pag);
            ViewData["CodV"] = new SelectList(_context.Vehículos, "CodV", "PlacaV", registro.CodV);
            return View(registro);
        }

        // POST: Registro/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("CodReg,FechaEntrada,FechaHoraSalida,CodV,CodEsp,cod_Pag")] Registro registro)
        {
            if (id != registro.CodReg)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroExists(registro.CodReg))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodEsp"] = new SelectList(_context.Espacios, "Cod_esp", "Ubi_esp", registro.CodEsp);
            ViewData["cod_Pag"] = new SelectList(_context.Pagos, "cod_Pag", "cod_Pag", registro.cod_Pag);
            ViewData["CodV"] = new SelectList(_context.Vehículos, "CodV", "PlacaV", registro.CodV);
            return View(registro);
        }

        // GET: Registro/Delete/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registro = await _context.Registros
                .Include(r => r.CodEspNavigation)
                .Include(r => r.CodPagNavigation)
                .Include(r => r.CodVNavigation)
                .FirstOrDefaultAsync(m => m.CodReg == id);
            if (registro == null)
            {
                return NotFound();
            }

            return View(registro);
        }

        // POST: Registro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registro = await _context.Registros.FindAsync(id);
            if (registro != null)
            {
                _context.Registros.Remove(registro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistroExists(int id)
        {
            return _context.Registros.Any(e => e.CodReg == id);
        }
        
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> ExportarPorFechas(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue || !fechaFin.HasValue)
            {
                return BadRequest("Debe proporcionar un rango de fechas válido.");
            }

            // Filtrar los registros por el rango de fechas
            var registros = await _context.Registros
                .Include(r => r.CodEspNavigation)
                .Include(r => r.CodPagNavigation)
                .Include(r => r.CodVNavigation)
                .Where(r => r.FechaEntrada >= fechaInicio && r.FechaEntrada <= fechaFin)
                .Select(r => new
                {
                    FechaEntrada = r.FechaEntrada.HasValue ? r.FechaEntrada.Value.ToString("yyyy-MM-dd HH:mm:ss") : "N/A",
                    FechaHoraSalida = r.FechaHoraSalida.HasValue ? r.FechaHoraSalida.Value.ToString("yyyy-MM-dd HH:mm:ss") : "N/A",
                    Espacio = r.CodEspNavigation != null ? r.CodEspNavigation.Ubi_esp : "Sin ubicación",
                    Vehículo = r.CodVNavigation != null ? r.CodVNavigation.PlacaV : "Sin placa",
                    Cliente = r.CodVNavigation.CodCliNavigation != null ? r.CodVNavigation.CodCliNavigation.Dni_cliente : "Sin DNI",
                    Nombre = r.CodVNavigation.CodCliNavigation != null ? r.CodVNavigation.CodCliNavigation.Nom_cliente : "Sin Nombre",
                    Apellido = r.CodVNavigation.CodCliNavigation != null ? r.CodVNavigation.CodCliNavigation.Apell_cliente : "Sin Apellido",
                    Pago = r.CodPagNavigation != null ? r.CodPagNavigation.MontPag : 0,
                    Metodpag = r.CodPagNavigation.CodMetdNavigation != null ? r.CodPagNavigation.CodMetdNavigation.DescrMetd : "Sin metodpag",
                    Recurrente = r.CodPagNavigation != null ? r.CodPagNavigation.EsPagoRecurrente : false,

                })
                .ToListAsync();

            // Crear el archivo Excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Registros");

                // Encabezados
                worksheet.Cells[1, 1].Value = "Fecha Entrada";
                worksheet.Cells[1, 2].Value = "Fecha Hora Salida";
                worksheet.Cells[1, 3].Value = "Espacio";
                worksheet.Cells[1, 4].Value = "Vehículo";
                worksheet.Cells[1, 5].Value = "Cliente";
                worksheet.Cells[1, 6].Value = "Nombre";
                worksheet.Cells[1, 7].Value = "Apellido";
                worksheet.Cells[1, 8].Value = "Monto Pago";
                worksheet.Cells[1, 9].Value = "Metodo de Pago";
                worksheet.Cells[1, 10].Value = "Pago Recurrente";
                // Datos
                int row = 2;
                foreach (var registro in registros)
                {
                    worksheet.Cells[row, 1].Value = registro.FechaEntrada;
                    worksheet.Cells[row, 2].Value = registro.FechaHoraSalida;
                    worksheet.Cells[row, 3].Value = registro.Espacio;
                    worksheet.Cells[row, 4].Value = registro.Vehículo;
                    worksheet.Cells[row, 5].Value = registro.Cliente;
                    worksheet.Cells[row, 6].Value = registro.Nombre;
                    worksheet.Cells[row, 7].Value = registro.Apellido;
                    worksheet.Cells[row, 8].Value = registro.Pago;
                    worksheet.Cells[row, 9].Value = registro.Metodpag;
                    worksheet.Cells[row, 10].Value = registro.Recurrente;
                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = $"Registros_{fechaInicio:yyyyMMdd}_to_{fechaFin:yyyyMMdd}.xlsx";

                return File(stream, contentType, fileName);
            }
        }

        // [HttpGet]
        // public async Task<IActionResult> ExportarExcel()
        // {
        //     var registros = await _context.Registros
        //         .Include(r => r.CodEspNavigation)
        //         .Include(r => r.CodPagNavigation)
        //         .Include(r => r.CodVNavigation)
        //         .Select(r => new
        //         {
        //             FechaEntrada = r.FechaEntrada.HasValue ? r.FechaEntrada.Value.ToString("yyyy-MM-dd HH:mm:ss") : "N/A",
        //             FechaHoraSalida = r.FechaHoraSalida.HasValue ? r.FechaHoraSalida.Value.ToString("yyyy-MM-dd HH:mm:ss") : "N/A",
        //             Espacio = r.CodEspNavigation != null ? r.CodEspNavigation.Ubi_esp : "Sin ubicación",
        //             Vehículo = r.CodVNavigation != null ? r.CodVNavigation.PlacaV : "Sin placa",
        //             Pago = r.CodPagNavigation != null ? r.CodPagNavigation.MontPag : 0 // Proporciona 0 si es nulo
        //         })
        //         .ToListAsync();
        //
        //     using (var package = new ExcelPackage())
        //     {
        //         var worksheet = package.Workbook.Worksheets.Add("Registros");
        //
        //         // Encabezados
        //         worksheet.Cells[1, 1].Value = "Fecha Entrada";
        //         worksheet.Cells[1, 2].Value = "Fecha Hora Salida";
        //         worksheet.Cells[1, 3].Value = "Espacio";
        //         worksheet.Cells[1, 4].Value = "Vehículo";
        //         worksheet.Cells[1, 5].Value = "Monto Pago";
        //
        //         // Datos
        //         int row = 2;
        //         foreach (var registro in registros)
        //         {
        //             worksheet.Cells[row, 1].Value = registro.FechaEntrada;
        //             worksheet.Cells[row, 2].Value = registro.FechaHoraSalida;
        //             worksheet.Cells[row, 3].Value = registro.Espacio;
        //             worksheet.Cells[row, 4].Value = registro.Vehículo;
        //             worksheet.Cells[row, 5].Value = registro.Pago;
        //             row++;
        //         }
        //
        //         worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        //
        //         var stream = new MemoryStream();
        //         package.SaveAs(stream);
        //         stream.Position = 0;
        //
        //         var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //         var fileName = "Registros.xlsx";
        //
        //         return File(stream, contentType, fileName);
        //     }
        // }

    }
}

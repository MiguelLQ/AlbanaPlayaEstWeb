using System.Diagnostics;
using albanaPlayaEst.Data;
using albanaPlayaEst.Dto;
using Microsoft.AspNetCore.Mvc;
using albanaPlayaEst.Models;
using Microsoft.AspNetCore.Authorization;

namespace albanaPlayaEst.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AlbanaDBcontext _context;

    public HomeController(ILogger<HomeController> logger, AlbanaDBcontext context)
    {
        _logger = logger;
        _context = context;
    }
    [HttpGet]
    [Authorize(Roles = "Administrador,Supervisor, Empleado")]
    public IActionResult Index()
    {
        // Obtener los datos del modelo
        var totalVehiculos = _context.Vehículos.Count();
        var totalEspacios = _context.Espacios.Count();
        var espaciosDisponibles = _context.Espacios.Count(e => e.Estad_esp == false);
        var espaciosOcupados = _context.Espacios.Count(e => e.Estad_esp == true);
    
        // Obtener el total de ingresos, asumiendo que cada registro tiene un pago relacionado
        var totalIngresos = _context.Registros.Sum(r => r.CodPagNavigation.MontPag);
    
        // Obtener el número total de registros
        var totalRegistros = _context.Registros.Count();

        // Crear el DTO con los datos
        var dashboardData = new DashboardDto
        {
            TotalVehiculos = totalVehiculos,
            TotalEspacios = totalEspacios,
            EspaciosDisponibles = espaciosDisponibles,
            EspaciosOcupados = espaciosOcupados,
            TotalIngresos = totalIngresos,
            TotalRegistros = totalRegistros
        };

        // Pasar los datos a la vista
        return View(dashboardData);  // Pasa el DTO a la vista
    }
    [Authorize(Roles = "Administrador,Supervisor, Empleado")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

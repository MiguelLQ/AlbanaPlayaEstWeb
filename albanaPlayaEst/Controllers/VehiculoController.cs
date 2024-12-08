using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using albanaPlayaEst.Data;
using albanaPlayaEst.Dto;
using albanaPlayaEst.Models;
using albanaPlayaEst.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace albanaPlayaEst.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly AlbanaDBcontext _context;
        private readonly IClienteService _clienteService;

        public VehiculoController(AlbanaDBcontext context, IClienteService clienteService)
        {
            _context = context;
            _clienteService = clienteService;
        }

        // GET: Vehiculo
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> Index()
        {
            // Usar un DTO para mostrar los vehículos
            var vehículos = await _context.Vehículos
                .Include(v => v.CodCliNavigation)  // Incluir información relacionada si es necesario
                .Include(v => v.CodTipVNavigation)  // Incluir el cliente si es necesario
                .ToListAsync();

            var vehiculosDto = vehículos.Select(v => new VehiculoDto
            {
                CodV = v.CodV,
                MarcV = v.MarcV,
                ModelV = v.ModelV,
                ColorV = v.ColorV,
                PlacaV = v.PlacaV,
                CodTipV = v.CodTipV,
                Cod_cliente = v.Cod_cliente,
                DescrTipV = v.CodTipVNavigation?.DescrTipV,
                Nom_cliente = v.CodCliNavigation?.Nom_cliente,  // Nombre del cliente relacionado
                Apell_cliente = v.CodCliNavigation?.Apell_cliente, // Apellido del cliente relacionado
                Dni_cliente = v.CodCliNavigation?.Dni_cliente
            }).ToList();

            return View(vehiculosDto);
        }

        // GET: Vehiculo/Details/5
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehículo = await _context.Vehículos
                .Include(v => v.CodTipVNavigation)
                .Include(v => v.CodCliNavigation)
                .FirstOrDefaultAsync(m => m.CodV == id);
            if (vehículo == null)
            {
                return NotFound();
            }

            // Mapear los datos del vehículo al DTO
            var vehiculoDto = new VehiculoDto
            {
                CodV = vehículo.CodV,
                MarcV = vehículo.MarcV,
                ModelV = vehículo.ModelV,
                ColorV = vehículo.ColorV,
                PlacaV = vehículo.PlacaV,
                CodTipV = vehículo.CodTipV,
                DescrTipV = vehículo.CodTipVNavigation.DescrTipV,
                Cod_cliente = vehículo.Cod_cliente,
                Dni_cliente = vehículo.CodCliNavigation.Dni_cliente,
                Nom_cliente = vehículo.CodCliNavigation?.Nom_cliente,
                Apell_cliente = vehículo.CodCliNavigation?.Apell_cliente
            };

            return View(vehiculoDto);
        }

        // GET: Vehiculo/Create
        [HttpGet]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create()
        {
            // Aquí estamos pasando la lista de tipos de vehículos correctamente al ViewBag
            ViewBag.TiposVehiculos = _context.Tipovehics.ToList();
            return View(new VehiculoDto());
        }

        // Acción para buscar el cliente por DNI
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult BuscarClientePorDni(string dni)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Dni_cliente == dni);
            if (cliente == null)
            {
                ViewData["ClienteNoEncontrado"] = "Cliente no encontrado con ese DNI.";
                return View("Create", new VehiculoDto());
            }

            var vehiculoDto = new VehiculoDto
            {
                Cod_cliente = cliente.Cod_cliente,
                Dni_cliente = cliente.Dni_cliente,
                Nom_cliente = cliente.Nom_cliente,
                Apell_cliente = cliente.Apell_cliente,
                Tel_cliente = cliente.Tel_cliente
            };

            ViewBag.TiposVehiculos = _context.Tipovehics.ToList();
            return View("Create", vehiculoDto);
        }

        // Acción para crear el vehículo
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult Create(VehiculoDto vehiculoDto)
        {
            if (ModelState.IsValid)
            {
                var vehiculo = new Vehículo
                {
                    PlacaV = vehiculoDto.PlacaV,
                    MarcV = vehiculoDto.MarcV,
                    ModelV = vehiculoDto.ModelV,
                    ColorV = vehiculoDto.ColorV,
                    CodTipV = vehiculoDto.CodTipV,
                    Cod_cliente = vehiculoDto.Cod_cliente,
                };

                _context.Vehículos.Add(vehiculo);
                _context.SaveChanges();
                return RedirectToAction("Index", "Asig"); // O la vista que quieras redirigir
            }

            // Si no es válido, recargamos la lista de tipos de vehículos
            ViewBag.TiposVehiculos = _context.Tipovehics.ToList();
            return View(vehiculoDto);
        }

        // GET: Vehiculo/Edit/5
        [HttpGet]
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehículo = await _context.Vehículos
                .Include(v => v.CodTipVNavigation)  // Incluimos el tipo de vehículo para facilitar el acceso
                .Include(v => v.CodCliNavigation)  // Incluimos el cliente para facilitar el acceso
                .FirstOrDefaultAsync(v => v.CodV == id); // Usamos FirstOrDefaultAsync para simplificar

            if (vehículo == null)
            {
                return NotFound();
            }

            // Mapear el vehículo a un DTO para su edición
            var vehiculoDto = new VehiculoDto
            {
                CodV = vehículo.CodV,
                MarcV = vehículo.MarcV,
                ModelV = vehículo.ModelV,
                ColorV = vehículo.ColorV,
                PlacaV = vehículo.PlacaV,
                CodTipV = vehículo.CodTipV,
                Cod_cliente = vehículo.Cod_cliente,
                Dni_cliente = vehículo.CodCliNavigation?.Dni_cliente,
                DescrTipV = vehículo.CodTipVNavigation?.DescrTipV,
            };
            ViewBag.Clientes = _context.Clientes.ToList();
            // Cargar los tipos de vehículos
            ViewBag.TiposVehiculos = _context.Tipovehics.ToList();

            return View(vehiculoDto);
        }

        // POST: Vehiculo/Edit/5
        [HttpPost]
        [Authorize(Roles = "Administrador,Empleado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodV,MarcV,ModelV,ColorV,PlacaV,CodTipV,Cod_cliente")] VehiculoDto vehiculoDto)
        {
            if (id != vehiculoDto.CodV)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vehiculo = await _context.Vehículos.FindAsync(id);
                    if (vehiculo == null)
                    {
                        return NotFound();
                    }

                    // Actualizamos los campos del vehículo con los datos del DTO
                    vehiculo.MarcV = vehiculoDto.MarcV;
                    vehiculo.ModelV = vehiculoDto.ModelV;
                    vehiculo.ColorV = vehiculoDto.ColorV;
                    vehiculo.PlacaV = vehiculoDto.PlacaV;
                    vehiculo.CodTipV = vehiculoDto.CodTipV;
                    vehiculo.Cod_cliente = vehiculoDto.Cod_cliente;

                    // Guardamos los cambios
                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();

                    // Redirigir a la lista de vehículos después de la edición
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Si el vehículo no existe, mostramos un mensaje de error
                    if (!VehículoExists(vehiculoDto.CodV))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewBag.Clientes = _context.Clientes.ToList();
            // Si el modelo no es válido, recargar los tipos de vehículos
            ViewBag.TiposVehiculos = _context.Tipovehics.ToList();
            return View(vehiculoDto);
        }

        
        private bool VehículoExists(int id)
        {
            return _context.Vehículos.Any(e => e.CodV == id);
        }
        // GET: Vehículo/AgregarV
        [HttpGet]
        [Authorize(Roles = "Administrador,Empleado")]
        public IActionResult AgregarV()
        {
            ViewData["CodTipV"] = new SelectList(_context.Tipovehics, "CodTipV", "DescrTipV");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Empleado")]
        public async Task<IActionResult> AgregarV(VehiculoDto vehiculoDto)
        {
            if (ModelState.IsValid)
            {
                // Busca el cliente por su teléfono
                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.Dni_cliente == vehiculoDto.Dni_cliente);

                if (cliente == null)
                {
                    // Si el cliente no existe, crea uno nuevo
                    cliente = new Cliente
                    {
                        Nom_cliente = vehiculoDto.Nom_cliente,
                        Apell_cliente = vehiculoDto.Apell_cliente,
                        Tel_cliente = vehiculoDto.Tel_cliente,
                        Dni_cliente = vehiculoDto.Dni_cliente
                    };

                    _context.Clientes.Add(cliente);
                    await _context.SaveChangesAsync(); // Guardar cambios antes de usar el cliente
                }

                // Ahora crea el vehículo
                var vehiculo = new Vehículo
                {
                    Cod_cliente = cliente.Cod_cliente, // Verifica que este valor no sea nulo
                    MarcV = vehiculoDto.MarcV,
                    ModelV = vehiculoDto.ModelV,
                    ColorV = vehiculoDto.ColorV,
                    PlacaV = vehiculoDto.PlacaV,
                    CodTipV = vehiculoDto.CodTipV
                };

                _context.Vehículos.Add(vehiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Vehiculo"); 
            }

            ViewData["CodTipV"] = new SelectList(_context.Tipovehics, "CodTipV", "DescrTipV", vehiculoDto.CodTipV);
            return RedirectToAction("Index", "Vehiculo"); 
        }

    }
}

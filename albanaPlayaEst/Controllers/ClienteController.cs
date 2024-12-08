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

namespace albanaPlayaEst.Controllers
{
    public class ClienteController : Controller
    {
        private readonly AlbanaDBcontext _context;

        public ClienteController(AlbanaDBcontext context)
        {
            _context = context;
        }

        // GET: Cliente
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Cliente/Details/5
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Cod_cliente == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Cliente/Create
        [HttpGet]
        [Authorize(Roles = "Administrador, Empleado")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrador, Empleado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cod_cliente,Nom_cliente,Apell_cliente,Tel_cliente,Dni_cliente")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                // Redirige a la vista Create del controlador 
                return RedirectToAction("Create", "Vehiculo");
            }
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        [HttpGet]
        [Authorize(Roles = "Administrador, Empleado")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Empleado")]
        public async Task<IActionResult> Edit(int id, [Bind("Cod_cliente,Nom_cliente,Apell_cliente,Tel_cliente,Dni_cliente")] Cliente cliente)
        {
            if (id != cliente.Cod_cliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Cod_cliente))
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
            return View(cliente);
        }
        
        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Cod_cliente == id);
        }
    }
}

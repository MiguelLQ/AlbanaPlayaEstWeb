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
    public class EspacioController : Controller
    {
        private readonly AlbanaDBcontext _context;

        public EspacioController(AlbanaDBcontext context)
        {
            _context = context;
        }

        // GET: Espacio
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Espacios.ToListAsync());
        }

        // GET: Espacio/Details/5
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var espacio = await _context.Espacios
                .FirstOrDefaultAsync(m => m.Cod_esp == id);
            if (espacio == null)
            {
                return NotFound();
            }

            return View(espacio);
        }
        // GET: Espacio/Create
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Cod_esp,Estad_esp,Ubi_esp,Descr_esp,Cost_esp")] Espacio espacio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(espacio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(espacio);
        }

        // GET: Espacio/Edit/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var espacio = await _context.Espacios.FindAsync(id);
            if (espacio == null)
            {
                return NotFound();
            }
            return View(espacio);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Cod_esp,Estad_esp,Ubi_esp,Descr_esp,Cost_esp")] Espacio espacio)
        {
            if (id != espacio.Cod_esp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(espacio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspacioExists(espacio.Cod_esp))
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
            return View(espacio);
        }

        private bool EspacioExists(int id)
        {
            return _context.Espacios.Any(e => e.Cod_esp == id);
        }
    }
}

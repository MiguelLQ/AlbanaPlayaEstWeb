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
    public class MetodpagController : Controller
    {
        private readonly AlbanaDBcontext _context;

        public MetodpagController(AlbanaDBcontext context)
        {
            _context = context;
        }

        // GET: Metodpag
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Metodpags.ToListAsync());
        }

        // GET: Metodpag/Details/5
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var metodpag = await _context.Metodpags
                .FirstOrDefaultAsync(m => m.CodMetd == id);
            if (metodpag == null)
            {
                return NotFound();
            }

            return View(metodpag);
        }

        // GET: Metodpag/Create
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Metodpag/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("CodMetd,DescrMetd")] Metodpag metodpag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(metodpag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(metodpag);
        }
        // GET: Metodpag/Edit/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int id)
        {
            var metodpag = _context.Metodpags.Find(id);
            if (metodpag == null)
            {
                return NotFound(); // Devuelve un error 404 si no se encuentra el registro
            }

            return View(metodpag); // Carga la vista con el modelo encontrado
        }

// POST: Metodpag/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("CodMetd,DescrMetd")] Metodpag metodpag)
        {
            if (id != metodpag.CodMetd)
            {
                ModelState.AddModelError(string.Empty, "El ID proporcionado no coincide con el registro.");
                return View(metodpag); // Devuelve la vista con los datos para que el usuario los corrija
            }

            if (!ModelState.IsValid)
            {
                return View(metodpag); // Si hay errores de validación, regresa la vista con el modelo y errores
            }

            try
            {
                _context.Update(metodpag);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Método de pago actualizado correctamente.";
                return RedirectToAction("Index"); // Redirige al listado después de la actualización exitosa
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetodpagExists(metodpag.CodMetd))
                {
                    return NotFound(); // Devuelve un error 404 si el registro ya no existe
                }
                else
                {
                    throw; // Lanza la excepción si ocurre un problema inesperado
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado: " + ex.Message);
                return View(metodpag); // Devuelve la vista con el error
            }
        }

        private bool MetodpagExists(int id)
        {
            return _context.Metodpags.Any(e => e.CodMetd == id);
        }
    }
}

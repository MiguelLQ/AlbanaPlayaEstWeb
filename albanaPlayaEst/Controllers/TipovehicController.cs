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
    public class TipovehicController : Controller
    {
        private readonly AlbanaDBcontext _context;

        public TipovehicController(AlbanaDBcontext context)
        {
            _context = context;
        }

        // GET: Tipovehic
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tipovehics.ToListAsync());
        }

        // GET: Tipovehic/Details/5
        [HttpGet]
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipovehic = await _context.Tipovehics
                .FirstOrDefaultAsync(m => m.CodTipV == id);
            if (tipovehic == null)
            {
                return NotFound();
            }

            return View(tipovehic);
        }

        // GET: Tipovehic/Create
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodTipV,DescrTipV")] Tipovehic tipovehic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipovehic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipovehic);
        }

        // GET: Tipovehic/Edit/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipovehic = await _context.Tipovehics.FindAsync(id);
            if (tipovehic == null)
            {
                return NotFound();
            }
            return View(tipovehic);
        }
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodTipV,DescrTipV")] Tipovehic tipovehic)
        {
            if (id != tipovehic.CodTipV)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipovehic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipovehicExists(tipovehic.CodTipV))
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
            return View(tipovehic);
        }
        
        private bool TipovehicExists(int id)
        {
            return _context.Tipovehics.Any(e => e.CodTipV == id);
        }
    }
}

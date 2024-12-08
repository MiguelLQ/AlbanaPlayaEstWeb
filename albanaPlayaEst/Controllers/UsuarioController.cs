using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using albanaPlayaEst.Data;
using albanaPlayaEst.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
namespace albanaPlayaEst.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AlbanaDBcontext _context;
        private readonly DA_Usuarios _daUsuario;
        public UsuarioController(AlbanaDBcontext context, DA_Usuarios daUsuario)
        {
            _context = context;
            _daUsuario = daUsuario;
        }

        // GET: Usuario
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }
        // GET: Usuario/Details/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id_user == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.AllRoles = new List<string> { "Administrador", "Supervisor", "Empleado" }; // Los roles disponibles
            return View(new Usuario { Roles = new string[0] });
        }


// POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Id_user,Nombre,Correo,Clave,Roles")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Clave = Utilidades.Encriptar(usuario.Clave);
                // Guardar automáticamente RolesJson desde la propiedad Roles
                usuario.RolesJson = JsonSerializer.Serialize(usuario.Roles);

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Definir los roles disponibles
            var rolesDisponibles = new List<string> { "Administrador", "Supervisor", "Empleado" };

            // Pasar los roles disponibles a la vista
            ViewBag.AllRoles = rolesDisponibles;

            return View(usuario);
        }

// POST:// POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id_user,Nombre,Correo,Clave,Roles")] Usuario usuario)
        {
            if (id != usuario.Id_user)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Recuperar el usuario actual desde la base de datos
                    var usuarioActual = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id_user == id);

                    if (usuarioActual == null)
                    {
                        return NotFound();
                    }

                    // Verificar si la clave fue modificada
                    if (usuario.Clave != usuarioActual.Clave)
                    {
                        // Encriptar la nueva clave
                        usuario.Clave = Utilidades.Encriptar(usuario.Clave);
                    }

                    // Actualizar la propiedad RolesJson
                    usuario.RolesJson = JsonSerializer.Serialize(usuario.Roles);

                    // Actualizar el usuario en la base de datos
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id_user))
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
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id_user == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id_user == id);
        }
        // Acción para mostrar el perfil del usuario
        [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> Perfil()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return RedirectToAction("Error", "Home");
            }

            int userId;
            if (int.TryParse(userIdClaim.Value, out userId))
            {
                // Usar la instancia de _daUsuario para llamar al método asincrónico
                var usuario = await _daUsuario.GetUsuarioPorId(userId);
                if (usuario == null)
                {
                    return NotFound();
                }

                return View(usuario);
            }

            return RedirectToAction("Error", "Home");
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
       [Authorize(Roles = "Administrador,Supervisor, Empleado")]
        public async Task<IActionResult> ActualizarFotoPerfil(IFormFile FotoPerfil)
        {
            // Obtener el UserId del usuario autenticado desde los claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                // Buscar al usuario en la base de datos
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id_user == userId);
                if (usuario == null)
                {
                    return NotFound();
                }

                if (FotoPerfil != null && FotoPerfil.Length > 0)
                {
                    // Crear la ruta donde se almacenará la imagen
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Generar un nombre único para la imagen para evitar conflictos
                    var uniqueFileName = $"{Guid.NewGuid()}_{FotoPerfil.FileName}";
                    var filePath = Path.Combine(folderPath, uniqueFileName);

                    // Guardar la imagen en el servidor
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await FotoPerfil.CopyToAsync(stream);
                    }

                    // Actualizar la ruta de la foto en la base de datos
                    usuario.FotoPerfil = "/uploads/" + uniqueFileName;
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();

                    // Actualizar el claim FotoPerfil
                    var identity = (ClaimsIdentity)User.Identity;
                    var fotoPerfilClaim = identity.FindFirst("FotoPerfil");
                    if (fotoPerfilClaim != null)
                    {
                        identity.RemoveClaim(fotoPerfilClaim);
                    }
                    identity.AddClaim(new Claim("FotoPerfil", usuario.FotoPerfil));

                    // Regenerar la cookie de autenticación con los claims actualizados
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity)
                    );
                }

                // Redirigir al perfil después de la actualización
                return RedirectToAction("Perfil");
            }

            // Si algo falla, redirigir a una página de error
            return RedirectToAction("Error", "Home");
        }

    }
}

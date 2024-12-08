using Microsoft.AspNetCore.Mvc;
using albanaPlayaEst.Models;
using albanaPlayaEst.Data;

// Referencias para autenticación
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace albanaPlayaEst.Controllers
{
    public class AccesoController : Controller
    {
        private readonly DA_Usuarios _daUsuario;

        public AccesoController(DA_Usuarios daUsuario)
        {
            _daUsuario = daUsuario;
        }

        // Página de inicio de sesión
        public IActionResult Index()
        {
            return View();
        }

        // Procesar inicio de sesión
        [HttpPost]
        public async Task<IActionResult> Index(Usuario _usuario)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Error"] = "Datos inválidos. Por favor, intente de nuevo.";
                return View();
            }

            try
            {
                // Validar usuario con contraseña hasheada
                var usuario = await _daUsuario.ValidarUsuario(_usuario.Correo, _usuario.Clave);

                if (usuario != null)
                {
                    // Crear Claims si el usuario es válido
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.Nombre),
                        new Claim("Correo", usuario.Correo),
                        new Claim("FotoPerfil", usuario.FotoPerfil ?? "~/lib/bootstrap/dist/img/user.jpg"),
                        new Claim("UserId", usuario.Id_user.ToString())
                    };

                    foreach (string rol in usuario.Roles ?? Array.Empty<string>())
                    {
                        claims.Add(new Claim(ClaimTypes.Role, rol));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewData["Error"] = "Usuario o contraseña incorrectos.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = "Ocurrió un error inesperado. Por favor, intente de nuevo.";
                Console.WriteLine(ex);
                return View();
            }
        }


        // Procesar cierre de sesión
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}

using albanaPlayaEst.Data;
using albanaPlayaEst.Models;
using Microsoft.EntityFrameworkCore;

namespace albanaPlayaEst.Services
{
    public class UsuarioService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ObtenerFotoPerfil()
        {
            var claims = _httpContextAccessor.HttpContext?.User?.Claims;
            var fotoPerfil = claims?.FirstOrDefault(c => c.Type == "FotoPerfil")?.Value;
            return string.IsNullOrEmpty(fotoPerfil) ? "~/lib/bootstrap/dist/img/user.jpg" : fotoPerfil;
        }
    }
}

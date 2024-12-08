using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using albanaPlayaEst.Models;
using Microsoft.EntityFrameworkCore;

namespace albanaPlayaEst.Data
{
    public class DA_Usuarios
    {
        private readonly AlbanaDBcontext _context;

        public DA_Usuarios(AlbanaDBcontext context)
        {
            _context = context;
        }

        public async Task<List<Usuario>> ListaUsuario()
        {
            return await _context.Usuarios.ToListAsync();
        }
        // Este es el método que debes agregar
        public async Task<Usuario> GetUsuarioPorId(int userId)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id_user == userId);
        }

        public async Task<Usuario> ValidarUsuario(string correo, string clave)
        {
            string claveHasheada = Utilidades.Encriptar(clave); // Hashea la contraseña ingresada

            // Busca un usuario que coincida con el correo y la contraseña hasheada
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Clave == claveHasheada);

            return usuario; // Retorna el usuario si coincide, o null si no
        }
    }
}
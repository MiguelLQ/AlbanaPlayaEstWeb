using System.Security.Cryptography;
using System.Text;

namespace albanaPlayaEst.Data;

public class Utilidades
{
    /*========================================================
           Encryptar contraseña
    =========================================================*/
    public static string Encriptar(string clave)
    {
        StringBuilder sb = new StringBuilder();
    
        using (SHA256 hash = SHA256.Create()) // Usar SHA256
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(clave));
        
            foreach (byte b in result)
            {
                sb.Append(b.ToString("x2"));//Convierte el byte a hexadecimal
            }

            return sb.ToString();
        }
    }
    //  PasswUsua = Utilidades.Encriptar(usuarioDto.Contrasenia),
}
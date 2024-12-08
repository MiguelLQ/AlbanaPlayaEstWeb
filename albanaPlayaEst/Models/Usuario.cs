using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace albanaPlayaEst.Models;

public partial class Usuario
{
    [Key]
    public int Id_user { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string FotoPerfil { get; set; }
    public string Clave { get; set; }

    [NotMapped]
    public string[] Roles
    {
        get => string.IsNullOrEmpty(RolesJson) ? Array.Empty<string>() : JsonSerializer.Deserialize<string[]>(RolesJson);
        set => RolesJson = JsonSerializer.Serialize(value);
    }


    // Propiedad que se guarda en la base de datos
    public string RolesJson { get; set; }
}

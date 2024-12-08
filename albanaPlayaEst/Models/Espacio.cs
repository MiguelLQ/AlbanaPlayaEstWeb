using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace albanaPlayaEst.Models
{
    public partial class Espacio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cod_esp { get; set; } // Código único del espacio

        public bool Estad_esp { get; set; } // Estado del espacio

        [Required(ErrorMessage = "La ubicación del espacio es obligatoria.")]
        [StringLength(20, ErrorMessage = "La ubicación no puede tener más de 20 caracteres.")]
        public string Ubi_esp { get; set; } // Ubicación del espacio

        [StringLength(100, ErrorMessage = "La descripción no puede tener más de 100 caracteres.")]
        public string Descr_esp { get; set; } // Descripción del espacio

        [Required(ErrorMessage = "El costo del espacio es obligatorio.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El costo debe ser un número válido con hasta dos decimales.")]
        public decimal Cost_esp { get; set; } // Costo del espacio

        public virtual ICollection<Registro> Registros { get; set; } = new List<Registro>();
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using albanaPlayaEst.Models;

namespace albanaPlayaEst.Models
{
    public partial class Vehículo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodV { get; set; } // Código del vehículo

        [Required(ErrorMessage = "La marca del vehículo es obligatoria.")]
        [StringLength(30, ErrorMessage = "La marca no puede tener más de 30 caracteres.")]
        public string MarcV { get; set; } // Marca del vehículo

        [Required(ErrorMessage = "El modelo del vehículo es obligatorio.")]
        [StringLength(30, ErrorMessage = "El modelo no puede tener más de 30 caracteres.")]
        public string ModelV { get; set; } // Modelo del vehículo

        [Required(ErrorMessage = "El color del vehículo es obligatorio.")]
        [StringLength(30, ErrorMessage = "El color no puede tener más de 30 caracteres.")]
        public string ColorV { get; set; } // Color del vehículo

        [Required(ErrorMessage = "La placa del vehículo es obligatoria.")]
        [StringLength(7, ErrorMessage = "La placa no puede tener más de 7 caracteres.")]
        public string PlacaV { get; set; } // Placa del vehículo

        public int CodTipV { get; set; }  // Clave foránea a Tipovehic

        public int Cod_cliente { get; set; } // Clave foránea a Cliente

        // Propiedad de navegación hacia Cliente
       
        public virtual Cliente CodCliNavigation { get; set; }

        // Propiedad de navegación hacia Tipovehic

        public virtual Tipovehic CodTipVNavigation { get; set; }

        // Propiedad de navegación hacia Registro (un vehículo puede tener varios registros)
        public virtual ICollection<Registro> Registros { get; set; } = new List<Registro>();
    }
}

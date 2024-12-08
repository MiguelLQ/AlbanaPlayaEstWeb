using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace albanaPlayaEst.Models
{
    public partial class Metodpag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CodMetd { get; set; } // Código único del método de pago

        [Required(ErrorMessage = "La descripción del método de pago es obligatoria.")]
        [StringLength(50, ErrorMessage = "La descripción no puede tener más de 100 caracteres.")]
        public string DescrMetd { get; set; } // Descripción del método de pago
        public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}

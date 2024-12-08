using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace albanaPlayaEst.Models;

public class Tipovehic
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CodTipV { get; set; }
    [StringLength(100, ErrorMessage = "La descripción no puede tener más de 100 caracteres.")]
    [Required(ErrorMessage = "La descripción del tipo de vehiculo es obligatoria.")]
    public string DescrTipV { get; set; }
    // Relación de uno a muchos con Vehículo
    public virtual ICollection<Vehículo> Vehículos { get; set; }

}


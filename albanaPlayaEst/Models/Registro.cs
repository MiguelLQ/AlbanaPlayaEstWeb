using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace albanaPlayaEst.Models;

public partial class Registro
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CodReg { get; set; } // Clave primaria del registro

    public DateTime? FechaEntrada { get; set; } // Fecha y hora de entrada

    public DateTime? FechaHoraSalida { get; set; } // Fecha y hora de salida
    public int CodV { get; set; } // Clave foránea del vehículo
    public int CodEsp { get; set; } // Clave foránea del espacio

    public int? cod_Pag { get; set; } // Clave foránea del pago, puede ser nula si aún no se ha pagado

    // Relación de navegación hacia el espacio (cada registro está en un único espacio)
    [NotMapped]
    public virtual Espacio CodEspNavigation { get; set; }

    // Relación de navegación hacia el vehículo (cada registro tiene un único vehículo)
    [NotMapped]
    public virtual Vehículo CodVNavigation { get; set; }
    // Relación de navegación hacia el pago (cada registro tiene un único pago, pero un pago puede estar asociado a varios registros)
    [NotMapped]
    public virtual Pago CodPagNavigation { get; set; }
    
}

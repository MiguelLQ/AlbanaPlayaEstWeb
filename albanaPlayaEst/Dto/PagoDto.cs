namespace albanaPlayaEst.Dto;

public class PagoDto
{
    public int? cod_Pag { get; set; } // Clave primaria del pago

    public decimal MontPag { get; set; } // Monto del pago
    public int CodMetd { get; set; } // Clave del método de pago
    // Nuevo campo: Fecha de inicio del pago mensual
    public DateTime FechaInicio { get; set; } = DateTime.Now;
    // Nuevo campo opcional: Fecha de fin del plan de pagos
    public DateTime? FechaFin { get; set; } 
    // Indica si este pago es recurrente (por ejemplo, mensual)
    public bool EsPagoRecurrente { get; set; } = false;
    // Número de meses pagados en el plan (solo relevante si EsPagoRecurrente = true)
    public string DescrMetd { get; set; } // Descripción del método de pago
    public bool Estado { get; set; } = false;
}
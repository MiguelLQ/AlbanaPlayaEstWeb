namespace albanaPlayaEst.Dto;

public class RegistroNuevoPagoDto
{
    public int CodRegExistente { get; set; }
    public DateTime? FechaEntrada { get; set; }
    public DateTime? FechaHoraSalida { get; set; }
    public DateTime? NuevaFechaEntrada { get; set; }
    public DateTime? NuevaFechaHoraSalida { get; set; }
    public int CodEsp { get; set; }
    public int CodV { get; set; }
    public decimal MontoAnterior { get; set; }
    public bool EsPagoRecurrente { get; set; }

    // Nuevos datos para el pago
    public decimal NuevoMonto { get; set; }
    public DateTime FechaPago { get; set; }
    public bool Estado { get; set; } = false;
    
    // metodo de pago
    public int CodMetd { get; set; } // Código único del método de pago
    
    public string DescrMetd { get; set; } // Descripción del método de pago
}

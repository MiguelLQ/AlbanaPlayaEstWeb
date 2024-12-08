namespace albanaPlayaEst.Dto;

public class DetalleAsignacionDto
{
    public int Cod_esp { get; set; }
    public bool Estad_esp { get; set; }
    public string Ubi_esp { get; set; }
    public string Descr_esp { get; set; } // Descripción del espacio
    public decimal Cost_esp { get; set; } // Costo del espacio
    public int? CodReg { get; set; }
    public DateTime? FechaEntrada { get; set; }
    public DateTime? FechaHoraSalida { get; set; }
    public int CodV { get; set; }
    public string MarcV { get; set; }
    public string ModelV { get; set; }
    public string ColorV { get; set; }
    public string PlacaV { get; set; }
    public int CodTipV { get; set; }
    public string DescrTipV { get; set; }
    public int Cod_cliente { get; set; } // auto_increment
    public string Nom_cliente { get; set; }
    public string Apell_cliente { get; set; }
    public string Tel_cliente { get; set; }
    public string Dni_cliente { get; set; }
    public int CodMetd { get; set; } // Código único del método de pago
        
    public int? cod_Pag { get; set; } 
    public decimal MontPag { get; set; } // Monto del pago
    public bool Estado { get; set; } = false; // Estados: "Pendiente", "Pagado", "Fallido"

}
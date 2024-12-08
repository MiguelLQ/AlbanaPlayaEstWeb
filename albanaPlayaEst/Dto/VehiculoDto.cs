namespace albanaPlayaEst.Dto;

public class VehiculoDto
{
    public int CodV { get; set; } // Código del vehículo
    public string MarcV { get; set; } // Marca del vehículo
    public string ModelV { get; set; } // Modelo del vehículo
    public string ColorV { get; set; } // Color del vehículo
    public string PlacaV { get; set; } // Placa del vehículo
    public int CodTipV { get; set; }  // Clave foránea a Tipovehic
    public int Cod_cliente { get; set; } // Clave foránea a Cliente
    public string DescrTipV { get; set; }
    public string Nom_cliente { get; set; }
    public string Apell_cliente { get; set; }
    public string Tel_cliente { get; set; }
    public string Dni_cliente { get; set; }
    
}
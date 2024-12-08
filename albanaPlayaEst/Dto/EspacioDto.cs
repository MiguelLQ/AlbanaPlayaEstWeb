namespace albanaPlayaEst.Dto;

public class EspacioDto
{
    public int Cod_esp { get; set; } // Código único del espacio

    public bool Estad_esp { get; set; } // Estado del espacio
    
    public string Ubi_esp { get; set; } // Ubicación del espacio
    
    public string Descr_esp { get; set; } // Descripción del espacio

    public decimal Cost_esp { get; set; } // Costo del espacio
}
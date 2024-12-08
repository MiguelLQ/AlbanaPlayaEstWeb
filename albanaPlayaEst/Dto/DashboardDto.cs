namespace albanaPlayaEst.Dto
{
    public class DashboardDto
    {
        // Datos generales
        public int TotalVehiculos { get; set; }
        public int TotalEspacios { get; set; }
        public int EspaciosDisponibles { get; set; }
        public int EspaciosOcupados { get; set; }
    
        // Datos de los pagos
        public decimal TotalIngresos { get; set; }
    
        // Otros datos, como el total de registros
        public int TotalRegistros { get; set; }
    }
    
}

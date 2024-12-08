using albanaPlayaEst.Data;
using albanaPlayaEst.Models;
using Microsoft.EntityFrameworkCore;

namespace albanaPlayaEst.Services
{
    public class VehiculoServices : IVehiculoServices
    {
        private readonly AlbanaDBcontext _context;

        public VehiculoServices(AlbanaDBcontext context)
        {
            _context = context;
        }

        // Implementación del método de la interfaz
        public Vehículo BuscarVehiculoPorPlaca(string placa)
        {
            // Buscamos el vehículo por placa y cargamos las relaciones de Cliente y TipoVehiculo
            var vehiculo = _context.Vehículos
                .Include(v => v.CodCliNavigation) // Cargar los datos de Cliente
                .Include(v => v.CodTipVNavigation) // Cargar los datos de TipoVehiculo
                .FirstOrDefault(v => v.PlacaV == placa); // Buscar por la placa

            // Devolver el vehículo encontrado (puede ser null si no se encuentra)
            return vehiculo;
        }
    }
}
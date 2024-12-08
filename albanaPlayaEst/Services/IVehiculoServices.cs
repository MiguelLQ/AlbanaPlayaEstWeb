using albanaPlayaEst.Models;

namespace albanaPlayaEst.Services;

public interface IVehiculoServices
{
    Vehículo BuscarVehiculoPorPlaca(string placa);
}
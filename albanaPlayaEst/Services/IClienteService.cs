using albanaPlayaEst.Models;
namespace albanaPlayaEst.Services;

public interface IClienteService
{
    Cliente BuscarClientePorDni(string dni);
}
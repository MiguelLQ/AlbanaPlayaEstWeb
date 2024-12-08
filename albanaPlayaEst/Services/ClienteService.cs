using albanaPlayaEst.Models;
namespace albanaPlayaEst.Services;
using albanaPlayaEst.Data;
using Microsoft.AspNetCore.Mvc;

public class ClienteService : IClienteService
{
    private readonly AlbanaDBcontext _context;

    public ClienteService(AlbanaDBcontext context)
    {
        _context = context;
    }

    public Cliente BuscarClientePorDni(string dni)
    {
        return _context.Clientes.FirstOrDefault(c => c.Dni_cliente == dni);
    }
}

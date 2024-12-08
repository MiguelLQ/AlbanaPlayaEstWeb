using albanaPlayaEst.Data;
using albanaPlayaEst.Dto;
using Microsoft.EntityFrameworkCore;

public class FiltrarPorPlacaService : IFiltrarPorPlacaService
{
    private readonly AlbanaDBcontext _context;

    public FiltrarPorPlacaService(AlbanaDBcontext context)
    {
        _context = context;
    }

    public List<AsignarVDto> FiltrarPorPlaca(string placa)
    {
        var registrosFiltrados = _context.Registros
            .Include(r => r.CodEspNavigation)
            .Include(r => r.CodVNavigation)
            .ThenInclude(v => v.CodCliNavigation)
            .Include(r => r.CodVNavigation.CodTipVNavigation)
            .Where(r => r.CodEspNavigation.Estad_esp == true && r.CodVNavigation.PlacaV.Contains(placa))
            .Select(r => new AsignarVDto
            {
                Cod_esp = r.CodEsp,
                Descr_esp = r.CodEspNavigation.Descr_esp,
                Estad_esp = r.CodEspNavigation.Estad_esp,
                PlacaV = r.CodVNavigation.PlacaV,
                MarcV = r.CodVNavigation.MarcV,
                ModelV = r.CodVNavigation.ModelV,
                ColorV = r.CodVNavigation.ColorV,
                Nom_cliente = r.CodVNavigation.CodCliNavigation != null ? r.CodVNavigation.CodCliNavigation.Nom_cliente : null,
                Apell_cliente = r.CodVNavigation.CodCliNavigation != null ? r.CodVNavigation.CodCliNavigation.Apell_cliente : null,
                Tel_cliente = r.CodVNavigation.CodCliNavigation != null ? r.CodVNavigation.CodCliNavigation.Tel_cliente : null,
                DescrTipV = r.CodVNavigation.CodTipVNavigation != null ? r.CodVNavigation.CodTipVNavigation.DescrTipV : null,
                FechaEntrada = r.FechaEntrada
            })
            .ToList();

        return registrosFiltrados;
    }
}
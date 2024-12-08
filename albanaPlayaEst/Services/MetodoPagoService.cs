using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using albanaPlayaEst.Data;

public class MetodoPagoService
{
    private readonly AlbanaDBcontext _context;

    public MetodoPagoService(AlbanaDBcontext context)
    {
        _context = context;
    }

    // Método para obtener los métodos de pago como una lista de SelectListItem
    public IEnumerable<SelectListItem> ObtenerMetodosPago()
    {
        return _context.Metodpags.Select(m => new SelectListItem
        {
            Value = m.CodMetd.ToString(),
            Text = m.DescrMetd
            
        }).ToList();
        
    }
}
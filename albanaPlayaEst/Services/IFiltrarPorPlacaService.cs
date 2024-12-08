using albanaPlayaEst.Dto;

public interface IFiltrarPorPlacaService
{
    List<AsignarVDto> FiltrarPorPlaca(string placa);
}
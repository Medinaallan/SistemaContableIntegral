using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaComunidad.Business.Interfaces;
using SistemaComunidad.Data.Entities;
using SistemaComunidad.Data.Interfaces;

namespace SistemaComunidad.Business.Services
{
    public class AporteService : IAporteService
{
    private readonly IUnitOfWork _unitOfWork;

    public AporteService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Aporte>> ObtenerAportesPorPersonaAsync(int personaId)
    {
        var aportes = await _unitOfWork.Aportes.BuscarAsync(a => a.PersonaId == personaId);
        return aportes;
    }

    public async Task<Aporte?> ObtenerPorIdAsync(int id)
    {
        return await _unitOfWork.Aportes.ObtenerPorIdAsync(id) as Aporte;
    }

    public async Task<Aporte?> ObtenerPorNumeroReciboAsync(string numeroRecibo)
    {
        var todos = await _unitOfWork.Aportes.ObtenerTodosAsync();
        return todos.Cast<Aporte>().FirstOrDefault(a => a.NumeroRecibo == numeroRecibo);
    }

    public async Task<Aporte> RegistrarAporteAsync(Aporte aporte)
    {
        // Generar número de recibo sencillo: YYYYMM-####
        var ultimo = await ObtenerUltimoNumeroReciboAsync();
        var nuevoNumero = ultimo + 1;
        aporte.NumeroRecibo = $"{DateTime.Now:yyyyMM}-{nuevoNumero:0000}";

        await _unitOfWork.Aportes.AgregarAsync(aporte);
        await _unitOfWork.CompletarAsync();
        return aporte;
    }

    public async Task<int> ObtenerUltimoNumeroReciboAsync()
    {
        var todos = await _unitOfWork.Aportes.ObtenerTodosAsync();
        var last = todos.OrderByDescending(a => a.Id).FirstOrDefault();
        if (last == null || string.IsNullOrEmpty(last.NumeroRecibo)) return 0;

        var partes = last.NumeroRecibo.Split('-');
        if (partes.Length >= 2 && int.TryParse(partes[1], out int numero))
        {
            return numero;
        }

        return 0;
    }
}

}

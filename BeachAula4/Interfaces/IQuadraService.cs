using BeachAula4.Entidades;

namespace BeachAula4.Interfaces
{
    public interface IQuadraService
    {
        Task<List<Quadra>> ListarAsync();
        Task<List<Quadra>> ListarAtivasAsync();
        Task<Quadra?> BuscarPorIdAsync(int id);
        Task AdicionarAsync(Quadra quadra);
        Task<bool> AtualizarAsync(Quadra quadra);
        Task<bool> ExcluirAsync(int id);
        Task<List<Quadra>> ListarAtivasPorPrecoMaximoAsync(decimal precoMaximo);
    }
}
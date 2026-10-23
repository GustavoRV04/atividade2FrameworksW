using BeachAula4.Entidades;
using BeachAula4.Interfaces;

namespace BeachAula4.Services
{
    public class QuadraService : IQuadraService
    {
        private readonly IQuadraRepository _repository;

        public QuadraService(IQuadraRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Quadra>> ListarAsync()
        {
            return await _repository.ListarAsync();
        }

        public async Task<List<Quadra>> ListarAtivasAsync()
        {
            return await _repository.ListarAtivasAsync();
        }

        public async Task<Quadra?> BuscarPorIdAsync(int id)
        {
            return await _repository.BuscarPorIdAsync(id);
        }

        public async Task AdicionarAsync(Quadra quadra)
        {
            // Validações de negócio
            if (string.IsNullOrWhiteSpace(quadra.Name))
                throw new ArgumentException("O nome da quadra é obrigatório.");

            if (quadra.ValorHora <= 0)
                throw new ArgumentException("O valor por hora deve ser maior que zero.");

            await _repository.AdicionarAsync(quadra);
        }

        public async Task<bool> AtualizarAsync(Quadra quadra)
        {
            if (string.IsNullOrWhiteSpace(quadra.Name) || quadra.ValorHora <= 0)
                return false;

            return await _repository.AtualizarAsync(quadra);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _repository.ExcluirAsync(id);
        }

        public async Task<List<Quadra>> ListarAtivasPorPrecoMaximoAsync(decimal precoMaximo)
        {
            if (precoMaximo < 0)
                return new List<Quadra>();

            return await _repository.ListarAtivasPorPrecoMaximoAsync(precoMaximo);
        }
    }
}
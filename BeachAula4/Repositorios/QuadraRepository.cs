using BeachAula4.Data;
using BeachAula4.Entidades;
using BeachAula4.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BeachAula4.Repositorios
{
    public class QuadraRepository : IQuadraRepository
    {
        private readonly AppDbContext context;

        public QuadraRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<List<Quadra>> ListarAsync()
        {
            return await context.Quadras.AsNoTracking().ToListAsync();
        }
        public async Task<List<Quadra>> ListarAtivasAsync()
        {
            return await context.Quadras.AsNoTracking().Where(q => q.Ativa).ToListAsync();
        }
        public async Task<Quadra?> BuscarPorIdAsync(int id)
        {
            return await context.Quadras.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task AdicionarAsync(Quadra quadra)
        {
            await context.Quadras.AddAsync(quadra);
            await context.SaveChangesAsync();
        }
        public async Task<bool> AtualizarAsync(Quadra quadra)
        {
            var existente = await context.Quadras.FindAsync(quadra.Id);
            if (existente == null)
            {
                return false;
            }
            existente.Name = quadra.Name;
            existente.ValorHora = quadra.ValorHora;
            existente.Ativa = quadra.Ativa;
            existente.Descricao = quadra.Descricao;

            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var quadra = await context.Quadras.FindAsync(id);

            if (quadra == null)
                return false;

            context.Quadras.Remove(quadra);

            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Quadra>> ListarAtivasPorPrecoMaximoAsync(decimal precoMaximo)
        {
            return await context.Quadras
                .AsNoTracking()
                .Where(q => q.Ativa && q.ValorHora <= precoMaximo)
                .OrderBy(q => q.ValorHora)
                .ToListAsync();
        }
    }
}

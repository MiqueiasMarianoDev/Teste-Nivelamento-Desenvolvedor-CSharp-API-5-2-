using Questao5.Infrastructure.Database.CommandStore.Requests;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public interface IMovimentoRepository
    {
        Task<Guid> AddMovimento(CreateMovimentoRequest request);
    }
}

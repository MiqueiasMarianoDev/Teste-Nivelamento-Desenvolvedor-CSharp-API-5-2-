using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Requests;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public interface IRequisicaoRepository
    {
        Task AddRequisicaoAsync(Idempotencia idempotencia);
    }
}

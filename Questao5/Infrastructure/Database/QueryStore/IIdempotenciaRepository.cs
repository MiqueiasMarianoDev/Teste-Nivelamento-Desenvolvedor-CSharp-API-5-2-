using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public interface IIdempotenciaRepository
    {
        Task<Idempotencia> GetIdempotenciaByIdAsync(Guid IdempotenciaKey);
    }
}

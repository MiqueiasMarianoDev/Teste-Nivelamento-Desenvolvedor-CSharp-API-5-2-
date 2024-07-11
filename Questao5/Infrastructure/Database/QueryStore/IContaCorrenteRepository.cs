using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public interface IContaCorrenteRepository
    {
        Task<ContaCorrente> GetByIdAsync(Guid IdContaCorrente);
    }
}

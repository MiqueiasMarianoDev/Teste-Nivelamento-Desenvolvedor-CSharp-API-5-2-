using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public interface ISaldoRepository
    {
        Task<IEnumerable<Movimento>> GetMovimentos(Guid IdContaCorrente);
    }
}

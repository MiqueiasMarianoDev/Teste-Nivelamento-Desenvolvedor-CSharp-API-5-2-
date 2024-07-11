using Dapper;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using System.Data;

namespace Questao5.Infrastructure.Database.QueryStore.Dapper
{
    public class SaldoRepository : ISaldoRepository
    {
        private readonly IDbConnection _dbConnection;

        public SaldoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Movimento>> GetMovimentos(Guid IdContaCorrente)
        {
            var sql = @"SELECT * 
                    FROM Movimento 
                    WHERE IdContaCorrente = @IdContaCorrente";

            var movimentos = await _dbConnection.QueryAsync<Movimento>(sql, new
            {
                IdContaCorrente
            });

            return movimentos;
        }
    }
}

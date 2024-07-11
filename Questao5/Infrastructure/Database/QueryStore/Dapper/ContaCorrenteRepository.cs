using Dapper;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using System.Data;

namespace Questao5.Infrastructure.Database.QueryStore.Dapper
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly IDbConnection _dbConnection;

        public ContaCorrenteRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ContaCorrente> GetByIdAsync(Guid IdContaCorrente)
        {
            var sql = @"SELECT * 
                    FROM ContaCorrente 
                    WHERE IdContaCorrente = @IdContaCorrente";

            var contaCorrente = await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new
            {
                IdContaCorrente
            });

            return contaCorrente;
        }
    }
}

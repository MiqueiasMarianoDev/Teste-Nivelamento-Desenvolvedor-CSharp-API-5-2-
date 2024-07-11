using Dapper;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using System.Data;

namespace Questao5.Infrastructure.Database.QueryStore.Dapper
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly IDbConnection _dbConnection;

        public IdempotenciaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Idempotencia> GetIdempotenciaByIdAsync(Guid idempotenciaKey)
        {
            var sql = @"SELECT * 
                    FROM Idempotencia 
                    WHERE chave_idempotencia = @IdempotenciaKey";

            var idempotencia = await _dbConnection.QueryFirstOrDefaultAsync<Idempotencia>(sql, new
            {
                IdempotenciaKey = idempotenciaKey
            });

            return idempotencia;
        }
    }
}

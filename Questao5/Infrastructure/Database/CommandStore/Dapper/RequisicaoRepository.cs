using Dapper;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using System.Data;
using System.Drawing;

namespace Questao5.Infrastructure.Database.CommandStore.Dapper
{
    public class RequisicaoRepository : IRequisicaoRepository
    {
        private readonly IDbConnection _dbConnection;

        public RequisicaoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddRequisicaoAsync(Idempotencia idempotencia)
        {
            var sql = "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";
            await _dbConnection.ExecuteAsync(sql, idempotencia);
        }
    }
}

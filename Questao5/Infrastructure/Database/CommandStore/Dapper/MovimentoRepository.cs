using Dapper;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using System.Data;
using System.Drawing;

namespace Questao5.Infrastructure.Database.CommandStore.Dapper
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly IDbConnection _dbConnection;

        public MovimentoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Guid> AddMovimento(CreateMovimentoRequest request)
        {
            var id = Guid.NewGuid();
            var sql = @"INSERT INTO Movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                    VALUES (@Id, @IdContaCorrente,  datetime('now'), @TipoMovimento, @Valor)"
            ;

            await _dbConnection.ExecuteAsync(sql, new
            {
                Id = id,
                request.IdContaCorrente,
                request.TipoMovimento,
                request.Valor,
            });

            return id;
        }
    }
}

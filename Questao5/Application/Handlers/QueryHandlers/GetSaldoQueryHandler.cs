using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Dapper;
using System.Linq;

namespace Questao5.Application.Handlers.QueryHandlers
{
    public class GetSaldoQueryHandler : IRequestHandler<GetSaldoQuery, GetSaldoResponse>
    {
        private readonly ISaldoRepository _repository;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;

        public GetSaldoQueryHandler(ISaldoRepository repository, IContaCorrenteRepository contaCorrenteRepository)
        {
            _repository = repository;
            _contaCorrenteRepository = contaCorrenteRepository;
        }

        public async Task<GetSaldoResponse> Handle(GetSaldoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var contaCorrente = await _contaCorrenteRepository.GetByIdAsync(request.IdContaCorrente);

                if(contaCorrente == null)
                {
                    return new GetSaldoResponse(0, false, $"Conta Corrente não existe");
                }

                if (contaCorrente.Ativo == false)
                {
                    return new GetSaldoResponse(0, false, $"Conta Corrente não está ativa");
                }

                var movimentos = await _repository.GetMovimentos(request.IdContaCorrente);

                var movCreditos = movimentos.Where(c => c.TipoMovimento == MovimentoTipo.Credito).ToList();
                var movDebitos = movimentos.Where(d => d.TipoMovimento == MovimentoTipo.Debito).ToList();


                var saldo = movCreditos.Sum(c => c.Valor) - movDebitos.Sum(d => d.Valor);


                return new GetSaldoResponse(saldo, true, "Saldo obtido com sucesso");
            }
            catch (Exception ex)
            {
                return new GetSaldoResponse(0, false, $"Erro ao obter saldo: {ex.Message}");
            }
        }
    }
}

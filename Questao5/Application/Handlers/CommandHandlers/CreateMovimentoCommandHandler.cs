using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Dapper;


namespace Questao5.Application.Handlers.CommandHandlers {
    public class CreateMovimentoCommandHandler : IRequestHandler<CreateMovimentoCommand, CreateMovimentoResponse>
    {
        private readonly IMovimentoRepository _repository;
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        private readonly IRequisicaoRepository _requisicaoRepository;

        public CreateMovimentoCommandHandler(IMovimentoRepository repository, 
                                             IContaCorrenteRepository contaCorrenteRepository, 
                                             IIdempotenciaRepository idempotenciaRepository, 
                                             IRequisicaoRepository requisicaoRepository)
        {
            _repository = repository;
            _contaCorrenteRepository = contaCorrenteRepository;
            _idempotenciaRepository = idempotenciaRepository;
            _requisicaoRepository = requisicaoRepository;
        }

        public async Task<CreateMovimentoResponse> Handle(CreateMovimentoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contaCorrente = await _contaCorrenteRepository.GetByIdAsync(request.IdContaCorrente);

                if (contaCorrente == null)
                {
                    return new CreateMovimentoResponse(Guid.Empty, false, $"Conta Corrente não existe");
                }

                if (contaCorrente.Ativo == false)
                {
                    return new CreateMovimentoResponse(Guid.Empty, false, $"Conta Corrente não está ativa");
                }

                var idempotencia = await _idempotenciaRepository.GetIdempotenciaByIdAsync(request.IdempotenciaKey);
                if (idempotencia != null)
                {
                    return new CreateMovimentoResponse(Guid.Empty, true, "Operação idempotente já processada.");
                }

                var movimentoId = await _repository.AddMovimento(new CreateMovimentoRequest
                {
                    IdContaCorrente = request.IdContaCorrente,
                    Valor = request.Valor,
                    TipoMovimento = request.TipoMovimento
                });

                idempotencia = new Idempotencia
                {
                    ChaveIdempotencia = request.IdempotenciaKey,
                    Requisicao = request.ToString(),
                    Resultado = "Movimento registrado com sucesso"
                };

                await _requisicaoRepository.AddRequisicaoAsync(idempotencia);

                return new CreateMovimentoResponse(movimentoId, true, "Movimento criado com sucesso");
            }
            catch (Exception ex)
            {
                return new CreateMovimentoResponse(Guid.Empty, false, $"Erro ao criar movimento: {ex.Message}");
            }
        }
    } 
}
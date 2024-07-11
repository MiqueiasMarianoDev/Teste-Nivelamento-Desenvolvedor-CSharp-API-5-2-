using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Handlers.CommandHandlers;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore;
using Xunit;

public class CreateMovimentoCommandHandlerTests
{
    private readonly Mock<IMovimentoRepository> _movimentoRepositoryMock;
    private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepositoryMock;
    private readonly Mock<IIdempotenciaRepository> _idempotenciaRepositoryMock;
    private readonly Mock<IRequisicaoRepository> _requisicaoRepositoryMock;
    private readonly CreateMovimentoCommandHandler _handler;

    public CreateMovimentoCommandHandlerTests()
    {
        _movimentoRepositoryMock = new Mock<IMovimentoRepository>();
        _contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
        _idempotenciaRepositoryMock = new Mock<IIdempotenciaRepository>();
        _requisicaoRepositoryMock = new Mock<IRequisicaoRepository>();
        _handler = new CreateMovimentoCommandHandler(
            _movimentoRepositoryMock.Object,
            _contaCorrenteRepositoryMock.Object,
            _idempotenciaRepositoryMock.Object,
            _requisicaoRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ContaCorrenteNaoExiste_DeveRetornarMensagemErro()
    {
        // Arrange
        var request = new CreateMovimentoCommand
        {
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100,
            TipoMovimento = "Credito",
            IdempotenciaKey = Guid.NewGuid()
        };

        _contaCorrenteRepositoryMock.Setup(repo => repo.GetByIdAsync(request.IdContaCorrente))
                                    .ReturnsAsync((ContaCorrente)null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.Sucesso);
        Assert.Equal("Conta Corrente não existe", result.Mensagem);
    }

    [Fact]
    public async Task Handle_ContaCorrenteInativa_DeveRetornarMensagemErro()
    {
        // Arrange
        var request = new CreateMovimentoCommand
        {
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100,
            TipoMovimento = "Credito",
            IdempotenciaKey = Guid.NewGuid()
        };

        var contaCorrente = new ContaCorrente { IdContaCorrente = request.IdContaCorrente, Ativo = false };

        _contaCorrenteRepositoryMock.Setup(repo => repo.GetByIdAsync(request.IdContaCorrente))
                                    .ReturnsAsync(contaCorrente);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.Sucesso);
        Assert.Equal("Conta Corrente não está ativa", result.Mensagem);
    }

    [Fact]
    public async Task Handle_OperacaoIdempotenteJaProcessada_DeveRetornarMensagemSucesso()
    {
        // Arrange
        var request = new CreateMovimentoCommand
        {
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100,
            TipoMovimento = "Credito",
            IdempotenciaKey = Guid.NewGuid()
        };

        var contaCorrente = new ContaCorrente { IdContaCorrente = request.IdContaCorrente, Ativo = true };
        var idempotencia = new Idempotencia { ChaveIdempotencia = request.IdempotenciaKey };

        _contaCorrenteRepositoryMock.Setup(repo => repo.GetByIdAsync(request.IdContaCorrente))
                                    .ReturnsAsync(contaCorrente);

        _idempotenciaRepositoryMock.Setup(repo => repo.GetIdempotenciaByIdAsync(request.IdempotenciaKey))
                                   .ReturnsAsync(idempotencia);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Sucesso);
        Assert.Equal("Operação idempotente já processada.", result.Mensagem);
    }

    [Fact]
    public async Task Handle_SucessoNaCriacaoDoMovimento_DeveRetornarMensagemSucesso()
    {
        // Arrange
        var request = new CreateMovimentoCommand
        {
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100,
            TipoMovimento = "Credito",
            IdempotenciaKey = Guid.NewGuid()
        };

        var contaCorrente = new ContaCorrente { IdContaCorrente = request.IdContaCorrente, Ativo = true };

        _contaCorrenteRepositoryMock.Setup(repo => repo.GetByIdAsync(request.IdContaCorrente))
                                    .ReturnsAsync(contaCorrente);

        _idempotenciaRepositoryMock.Setup(repo => repo.GetIdempotenciaByIdAsync(request.IdempotenciaKey))
                                   .ReturnsAsync((Idempotencia)null);

        _movimentoRepositoryMock.Setup(repo => repo.AddMovimento(It.IsAny<CreateMovimentoRequest>()))
                                .ReturnsAsync(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Sucesso);
        Assert.Equal("Movimento criado com sucesso", result.Mensagem);
    }

    [Fact]
    public async Task Handle_ErroAoCriarMovimento_DeveRetornarMensagemErro()
    {
        // Arrange
        var request = new CreateMovimentoCommand
        {
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100,
            TipoMovimento = "Credito",
            IdempotenciaKey = Guid.NewGuid()
        };

        var contaCorrente = new ContaCorrente { IdContaCorrente = request.IdContaCorrente, Ativo = true };

        _contaCorrenteRepositoryMock.Setup(repo => repo.GetByIdAsync(request.IdContaCorrente))
                                    .ReturnsAsync(contaCorrente);

        _idempotenciaRepositoryMock.Setup(repo => repo.GetIdempotenciaByIdAsync(request.IdempotenciaKey))
                                   .ReturnsAsync((Idempotencia)null);

        _movimentoRepositoryMock.Setup(repo => repo.AddMovimento(It.IsAny<CreateMovimentoRequest>()))
                                .ThrowsAsync(new Exception("Erro de teste"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.Sucesso);
        Assert.StartsWith("Erro ao criar movimento:", result.Mensagem);
    }
}

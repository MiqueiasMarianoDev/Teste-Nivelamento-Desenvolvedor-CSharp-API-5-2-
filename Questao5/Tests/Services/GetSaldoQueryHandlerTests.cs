using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Questao5.Application.Handlers.QueryHandlers;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Database.QueryStore;
using Xunit;

public class GetSaldoQueryHandlerTests
{
    private readonly Mock<ISaldoRepository> _saldoRepositoryMock;
    private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepositoryMock;
    private readonly GetSaldoQueryHandler _handler;

    public GetSaldoQueryHandlerTests()
    {
        _saldoRepositoryMock = new Mock<ISaldoRepository>();
        _contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
        _handler = new GetSaldoQueryHandler(
            _saldoRepositoryMock.Object,
            _contaCorrenteRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ContaCorrenteNaoExiste_DeveRetornarMensagemErro()
    {
        // Arrange
        var request = new GetSaldoQuery
        {
            IdContaCorrente = Guid.NewGuid()
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
        var request = new GetSaldoQuery
        {
            IdContaCorrente = Guid.NewGuid()
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
    public async Task Handle_SucessoNaObtencaoDoSaldo_DeveRetornarSaldoCorreto()
    {
        // Arrange
        var request = new GetSaldoQuery
        {
            IdContaCorrente = Guid.NewGuid()
        };

        var contaCorrente = new ContaCorrente { IdContaCorrente = request.IdContaCorrente, Ativo = true };

        _contaCorrenteRepositoryMock.Setup(repo => repo.GetByIdAsync(request.IdContaCorrente))
                                    .ReturnsAsync(contaCorrente);

        var movimentos = new List<Movimento>
        {
            new Movimento { TipoMovimento = MovimentoTipo.Credito, Valor = 200 },
            new Movimento { TipoMovimento = MovimentoTipo.Debito, Valor = 50 },
            new Movimento { TipoMovimento = MovimentoTipo.Credito, Valor = 100 },
            new Movimento { TipoMovimento = MovimentoTipo.Debito, Valor = 20 }
        };

        _saldoRepositoryMock.Setup(repo => repo.GetMovimentos(request.IdContaCorrente))
                            .ReturnsAsync(movimentos);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.Sucesso);
        Assert.Equal(230, result.Saldo);
        Assert.Equal("Saldo obtido com sucesso", result.Mensagem);
    }

    [Fact]
    public async Task Handle_ErroAoObterSaldo_DeveRetornarMensagemErro()
    {
        // Arrange
        var request = new GetSaldoQuery
        {
            IdContaCorrente = Guid.NewGuid()
        };

        var contaCorrente = new ContaCorrente { IdContaCorrente = request.IdContaCorrente, Ativo = true };

        _contaCorrenteRepositoryMock.Setup(repo => repo.GetByIdAsync(request.IdContaCorrente))
                                    .ReturnsAsync(contaCorrente);

        _saldoRepositoryMock.Setup(repo => repo.GetMovimentos(request.IdContaCorrente))
                            .ThrowsAsync(new Exception("Erro de teste"));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.Sucesso);
        Assert.StartsWith("Erro ao obter saldo:", result.Mensagem);
    }
}

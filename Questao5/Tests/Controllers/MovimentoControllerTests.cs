using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Services.Controllers;
using Xunit;

public class MovimentoControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly MovimentoController _controller;

    public MovimentoControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new MovimentoController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreateMovimento_ReturnsOkResult_WhenCommandSucceeds()
    {
        // Arrange
        var command = new CreateMovimentoCommand
        {
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100m,
            TipoMovimento = MovimentoTipo.Credito.ToString(),
            IdempotenciaKey = Guid.NewGuid()
        };

        var response = new CreateMovimentoResponse
        (Guid.Empty, true, "Movimento registrado com sucesso."
        );

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateMovimentoCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateMovimento(command);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task CreateMovimento_ReturnsBadRequest_WhenCommandFails()
    {
        // Arrange
        var command = new CreateMovimentoCommand
        {
            IdContaCorrente = Guid.NewGuid(),
            Valor = 100m,
            TipoMovimento = MovimentoTipo.Credito.ToString(),
            IdempotenciaKey = Guid.NewGuid()
        };

        var response = new CreateMovimentoResponse
            (Guid.Empty, false, "Erro ao registrar movimento."
        );

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateMovimentoCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateMovimento(command);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task GetSaldo_ReturnsOkResult_WhenQuerySucceeds()
    {
        // Arrange
        var idContaCorrente = Guid.NewGuid();
        var query = new GetSaldoQuery { IdContaCorrente = idContaCorrente };

        var response = new GetSaldoResponse(1000m, true, "Saldo obtido com sucesso.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetSaldoQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(response);

        // Act
        var result = await _controller.GetSaldo(idContaCorrente);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task GetSaldo_ReturnsBadRequest_WhenQueryFails()
    {
        // Arrange
        var idContaCorrente = Guid.NewGuid();
        var query = new GetSaldoQuery { IdContaCorrente = idContaCorrente };


        var response = new GetSaldoResponse(0, false, "Erro ao obter saldo.");

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetSaldoQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(response);

        // Act
        var result = await _controller.GetSaldo(idContaCorrente);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Value.Should().BeEquivalentTo(response);
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovimentoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("criar")]
        public async Task<IActionResult> CreateMovimento([FromBody] CreateMovimentoCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Sucesso)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet]
        [Route("saldo/{idContaCorrente}")]
        public async Task<IActionResult> GetSaldo(Guid idContaCorrente)
        {
            var query = new GetSaldoQuery { IdContaCorrente = idContaCorrente };
            var result = await _mediator.Send(query);
            if (result.Sucesso)
                return Ok(result);
            return BadRequest(result);
        }
    }
}

using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class GetSaldoQuery : IRequest<GetSaldoResponse>
    {
        public Guid IdContaCorrente { get; set; }
    }
}

namespace Questao5.Application.Commands.Responses
{
    public class CreateMovimentoResponse
    {
        public Guid Id { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }

        public CreateMovimentoResponse(Guid id, bool sucesso, string mensagem)
        {
            Id = id;
            Sucesso = sucesso;
            Mensagem = mensagem;
        }
    }
}

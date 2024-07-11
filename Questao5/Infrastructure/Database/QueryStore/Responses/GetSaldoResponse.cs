namespace Questao5.Infrastructure.Database.QueryStore.Responses
{
    public class GetSaldoResponse
    {
        public decimal Saldo { get; set; }
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }

        public GetSaldoResponse(decimal saldo, bool sucesso, string mensagem)
        {
            Saldo = saldo;
            Sucesso = sucesso;
            Mensagem = mensagem;
        }
    }
}

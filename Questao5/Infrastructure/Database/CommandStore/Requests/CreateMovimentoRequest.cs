namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class CreateMovimentoRequest
    {
        public Guid IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; }
    }
}

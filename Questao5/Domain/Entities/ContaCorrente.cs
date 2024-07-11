using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class ContaCorrente
    {
        public Guid IdContaCorrente { get; set; }
        public int Numero { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

    public List<Movimento> Movimentos { get; set; }

        public ContaCorrente()
        {
            Movimentos = new List<Movimento>();
        }

    }
}

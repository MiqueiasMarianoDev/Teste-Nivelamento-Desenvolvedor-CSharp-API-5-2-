using System;
using System.ComponentModel.DataAnnotations.Schema;
using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public Guid Id { get; set; }

        [Column("idcontacorrente")]
        public Guid IdContaCorrente { get; set; }

        [Column("valor")]
        public decimal Valor { get; set; }

        [Column("tipomovimento")]
        public MovimentoTipo TipoMovimento { get; set; }

        [Column("data")]
        public DateTime Data { get; set; }

        public ContaCorrente ContaCorrente { get; set; }
    }
}

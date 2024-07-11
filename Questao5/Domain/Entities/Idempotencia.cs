using System;
using System.ComponentModel.DataAnnotations.Schema;
using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class Idempotencia
    {
        public Guid ChaveIdempotencia { get; set; }
        public string Requisicao { get; set; }
        public string Resultado { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Impactabank.Models
{
    [Table("ExtratoOperacoes")]
    public class ExtratoOperacoes
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public TipoOperacao Tipo { get; set; }

        [Precision(16, 2)]
        [Required]
        public double Valor { get; set; }

        [Required]
        [ForeignKey("Clientes")]
        public int ClienteId { get; set; }

        public DateTime DataHora { get; set; }
    }
}

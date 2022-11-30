using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Impactabank.Models
{
    [Table("Municipios")]
    public class Municipio
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(60)]
        public string? Nome { get; set; }


        public UF Uf { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Impactabank.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Login { get; set; }


        [MaxLength(1000)]
        public string Senha { get; set; }

        [NotMapped]
        public string ConfirmarSenha { get; set; }
    }
}

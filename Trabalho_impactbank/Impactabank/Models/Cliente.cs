using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Impactabank.Models
{
    [Table("Clientes")]
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(60)]
        [Required]
        public string Nome { get; set; }

        [MaxLength(14)]
        [MinLength(14)]
        [Required]
        public string CPF { get; set; }

        [Required]
        public FaixaSalarial? FaixaSalarial { get; set; }

        [Required]
        public Genero? Genero { get; set; }

        [MaxLength(60)]
        public string? NomeSocial { get; set; }

        [Required]
        [MaxLength(255)]
        public string Logradouro { get; set; }

        [MaxLength(20)]
        [Required]
        public string Numero { get; set; }

        [EmailAddress(ErrorMessage="Preenche um e-mail válido (teste@teste.com)")]
        [Required]
        public string? Email { get; set; }

        [MaxLength(50)]
        public string? Complemento { get; set; }


        /// <summary>
        /// Use https://servicodados.ibge.gov.br/api/v1/localidades/estados/{UF}/municipios para consultar as cidades a partir de uma Unidade Federativa
        /// </summary>
        [Required]
        public Municipio Municipio { get; set; }


        [Required]
        public UF? UF { get; set; }

        [Required]
        public Conta Conta { get; set; }

        [Required]
        public Usuario Usuario { get; set; }

        [NotMapped]
        public IList<Municipio> MunicipiosDaUf { get; set; }

        [NotMapped]
        public bool Alterando { get; set; }
    }
}

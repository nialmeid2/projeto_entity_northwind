using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Impactabank.Models
{
    public enum Genero
    {
        [Display(Name=("Masculino"))]
        M,

        [Display(Name=("Feminino"))]
        F,

        [Display(Name=("Outro"))]
        O
    }
}

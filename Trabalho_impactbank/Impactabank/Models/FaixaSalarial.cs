using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Impactabank.Models
{
    public enum FaixaSalarial
    {
        [Display(Name="De R$0 a R$1.499,99")]
        FAIXA_MENOR,

        [Display(Name="De R$1.500 a R$2.999,99")]
        FAIXA_MEDIA_1,

        [Display(Name="De R$3.000 a R$4.499,99")]
        FAIXA_MEDIA_2,

        [Display(Name="De R$4.500 a R$5.999,99")]
        FAIXA_MEDIA_3,

        [Display(Name="De R$6.000 a R$10.000")]
        FAIXA_ALTA_1,

        [Display(Name="Acima de R$10.000")]
        FAIXA_ALTA_2
    }
}

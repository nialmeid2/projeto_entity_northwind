using System.Globalization;

namespace Impactabank.Models
{
    public static class Extensoes
    {
        public static string FormatoFinanceiro(this double nmr, string cultura = "pt-BR")
        {
            return nmr.ToString("N", new CultureInfo(cultura)); // retornar com formato de dinheiro
        }
    }
}

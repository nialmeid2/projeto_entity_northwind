using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Impactabank.Models
{
    [Table("Contas")]
    public class Conta
    {
        [Key]
        public int Id { get; set; }

        [Precision(16, 2)]
        public double Saldo { get; set; }

        [Precision(16, 2)]
        public double SaldoPoupanca { get; set; }

        [ForeignKey("Clientes")]
        public int ClienteId { get; set; }

        public bool Sacar(double valor)
        {
            if (valor > Saldo)
                return false;
            Saldo -= valor; // subtrai o valor do saldo
            return true;
        }

        public void Depositar(double valor)
        {
            Saldo += valor;
        }


    }
}

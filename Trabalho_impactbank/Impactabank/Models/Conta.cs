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

        public bool Sacar(double valor, TipoOperacao tipoOperacao)
        {
            if (valor > (tipoOperacao == TipoOperacao.SaqueCC ? Saldo : SaldoPoupanca))
                return false;
            Saldo -= tipoOperacao == TipoOperacao.SaqueCC ? valor : 0; // subtrai o valor do saldo apenas qd for saqueCC
            SaldoPoupanca -= tipoOperacao == TipoOperacao.SaquePoupanca ? valor : 0; // subtrai o valor do saldo
            return true;
        }


        public bool Depositar(double valor, TipoOperacao tipoOperacao)
        {
            if (valor <= 0)
                return false;
            Saldo += tipoOperacao == TipoOperacao.DepositoCC ? valor : 0; // só soma qd for depósito de Conta Corrente
            SaldoPoupanca += tipoOperacao == TipoOperacao.DepositoPoupanca ? valor : 0; // só soma qd for depósito de Conta Poupança
            return true;
        }



    }
}

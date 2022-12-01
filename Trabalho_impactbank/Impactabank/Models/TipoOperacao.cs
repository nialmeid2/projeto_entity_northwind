using System.ComponentModel;

namespace Impactabank.Models
{
    public enum TipoOperacao
    {
        [Description("Depósito em Conta Corrente")]
        DepositoCC,

        [Description("Deposito em Conta Poupança")]
        DepositoPoupanca,

        [Description("Saque em Conta Corrente")]
        SaqueCC,

        [Description("saque em Conta Poupança")]
        SaquePoupanca
    }
}
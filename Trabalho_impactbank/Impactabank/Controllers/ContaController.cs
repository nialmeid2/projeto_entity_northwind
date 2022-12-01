using Impactabank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace Impactabank.Controllers
{
    [Authorize]
    public class ContaController : Controller
    {

        private BancoContext BancoContext;
        public ContaController()
        {
            this.BancoContext = new BancoContext();
        }

        public IActionResult Index()
        {
            Cliente contaLogada = getContaLogada();

            if (contaLogada == null)
            {
                return Redirect("../Cliente/Logout"); // desloga
            }

            getExtrato10(contaLogada);

            return View(contaLogada);
        }

        [HttpPost]
        public IActionResult DepositarCC(double valor)
        {
            

            Cliente contaLogada = getContaLogada();
            if (contaLogada == null)
            {
                return Redirect("../Cliente/Logout"); // desloga
            }


            return fazerDeposito(contaLogada, valor, TipoOperacao.DepositoCC);            
        }

        [HttpPost]
        public IActionResult DepositarPoupanca(double valor)
        {


            Cliente contaLogada = getContaLogada();
            if (contaLogada == null)
            {
                return Redirect("../Cliente/Logout"); // desloga
            }


            return fazerDeposito(contaLogada, valor, TipoOperacao.DepositoPoupanca);
        }

        [HttpPost]
        public IActionResult SacarCC(double valor)
        {
            Cliente contaLogada = getContaLogada();
            if (contaLogada == null)
            {
                return Redirect("../Cliente/Logout"); // desloga
            }

            return fazerSaque(contaLogada, valor, TipoOperacao.SaqueCC);
        }

        [HttpPost]
        public IActionResult SacarPoupanca(double valor)
        {
            Cliente contaLogada = getContaLogada();
            if (contaLogada == null)
            {
                return Redirect("../Cliente/Logout"); // desloga
            }

            return fazerSaque(contaLogada, valor, TipoOperacao.SaquePoupanca);
        }

        private IActionResult fazerDeposito(Cliente contaLogada, double valor, TipoOperacao tipoOperacao)
        {
            if (!contaLogada.Conta.Depositar(valor, tipoOperacao))
            {
                ViewBag.MsgErro = "Deposite um valor positivo";
                return View("Index", contaLogada);
            }

            GravaOperacao(contaLogada, valor, tipoOperacao);
            
            ViewBag.Msg = $"Valor de {valor.ToString("C", CultureInfo.CurrentCulture)} depositado com sucesso na " +
                (tipoOperacao == TipoOperacao.DepositoCC ? "Conta Corrente" : "Conta Poupança");

            return View("Index", contaLogada);
        }

        private IActionResult fazerSaque(Cliente contaLogada, double valor, TipoOperacao tipoOperacao)
        {
            if (!contaLogada.Conta.Sacar(valor, tipoOperacao))
            {
                ViewBag.MsgErro = "Você não pode sacar mais do que está disponível em conta";
                return View("Index", contaLogada);
            }

            GravaOperacao(contaLogada, valor, tipoOperacao);

            ViewBag.Msg = $"Valor de {valor.ToString("C", CultureInfo.CurrentCulture)} sacado com sucesso da " +
                (tipoOperacao == TipoOperacao.DepositoCC ? "Conta Corrente" : "Conta Poupança");

            return View("Index", contaLogada);
        }

        public IActionResult ExtratoCompleto()
        {
            Cliente contaLogada = getContaLogada();
            if (contaLogada == null)
            {
                return Redirect("../Cliente/Logout"); // desloga
            }

            getExtratoCompleto(contaLogada);

            return View("Index", contaLogada);
        }

        private void GravaOperacao(Cliente contaLogada, double valor, TipoOperacao tipoOperacao)
        {
            BancoContext.Update(contaLogada.Conta);

            ExtratoOperacoes extrato = criaOperacaoExtrato(contaLogada, valor, tipoOperacao);

            BancoContext.Add(extrato);
            BancoContext.SaveChanges();

            getExtrato10(contaLogada); // mostrar extrato atualizado
        }

        private Cliente getContaLogada()
        {
            Cliente contaLogada = BancoContext.Clientes.Where(c => c.Usuario.Login == User.Identity.Name)
                .Include(c => c.Conta)
                .FirstOrDefault();
                                    
            return contaLogada;
        }

        private void getExtrato10(Cliente contaLogada)
        {
            ViewBag.Ultimas10 = BancoContext.Extrato.Where(e => e.ClienteId == contaLogada.Id)
                .OrderByDescending(c => c.DataHora)
                .Take(10).ToList();            
        }

        private void getExtratoCompleto(Cliente contaLogada)
        {
            var extrato = BancoContext.Extrato.Where(e => e.ClienteId == contaLogada.Id)
                .OrderByDescending(c => c.DataHora).ToList();

            if (extrato != null && extrato.Count > 0)
                ViewBag.ExtratoCompleto = extrato; // só mostra se tiver info
        }


        private ExtratoOperacoes criaOperacaoExtrato(Cliente contaLogada, double valor, TipoOperacao tipoOperacao)
        {
            return new ExtratoOperacoes
            {
                ClienteId = contaLogada.Id,
                Valor = valor,
                Tipo = tipoOperacao,
                DataHora = DateTime.Now
            };
        }
    }
}

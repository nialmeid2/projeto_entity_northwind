using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Impactabank.Models;
using Microsoft.EntityFrameworkCore;
using Impactabank.Attributes;
using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;

namespace Impactabank.Controllers
{
    public class ClienteController : Controller
    {
        private BancoContext BancoContext;
        public ClienteController()
        {
            this.BancoContext = new BancoContext();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Editar()
        {
            // pegar dados do cliente logado
            Cliente logado = BancoContext.Clientes.Where(c => c.Usuario.Login == User.Identity.Name)
                .Include(c => c.Municipio) // trazer dados do município
                .FirstOrDefault();

            if (logado == null)
                return Logout();

            logado.MunicipiosDaUf = listaMunicipios(logado.UF);

            logado.Alterando = true;

            return View("Cadastrar", logado);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Editar(Cliente cliente)
        {
            cliente.Alterando = true; // anti-overposting

            Cliente alterando = BancoContext.Clientes.Where(c => c.Email == User.Identity.Name)
                .Include(c => c.Conta)
                .Include(c => c.Usuario)
                .FirstOrDefault();

            if (alterando == null)
            {
                return Logout();
            }

            BancoContext.Entry(alterando).State = EntityState.Detached;

            cliente.Id = alterando.Id;
            cliente.Email = alterando.Email;
            cliente.Conta = alterando.Conta;

            string senha = cliente.Usuario.Senha;
            cliente.Usuario = alterando.Usuario;

            if (!string.IsNullOrWhiteSpace(senha))
            {
                senha = BCrypt.Net.BCrypt.HashPassword(senha, BCrypt.Net.BCrypt.GenerateSalt());
                cliente.Usuario.Senha = senha; // senha alterada
            }

            cliente.Municipio = BancoContext.Municipios.Where(m => m.Id == cliente.Municipio.Id).FirstOrDefault();
            cliente.MunicipiosDaUf = listaMunicipios(cliente.UF);

            BancoContext.Clientes.Update(cliente);
            BancoContext.SaveChanges();


            ViewBag.Msg = "Dados Alterados com Sucesso!";
            return View("Cadastrar", cliente);
        }

        

        [RedirectAuthenticated("../Home/Index")]
        public IActionResult Login() 
        {
            
            return View("Login");
        }

        [HttpPost]
        [RedirectAuthenticated("../Home/Index")]
        public IActionResult Login(Usuario u) 
        {
            
            Cliente possivelLogado = BancoContext.Clientes.Where(c => c.Usuario.Login == u.Login)
                .Include(c => c.Usuario)
                .FirstOrDefault<Cliente>();

            if (possivelLogado == null || // verificar se o usuário existe e se a senha condiz com a senha do usuário
                BCrypt.Net.BCrypt.Verify(BCrypt.Net.BCrypt.HashPassword(u.Senha, BCrypt.Net.BCrypt.GenerateSalt()), possivelLogado.Usuario.Senha))
            {
                ViewBag.Mensagem = $"O usuário {u.Login} não existe ou a senha está incorreta";
                return View("Login", u);
            }

            FazerLogin(possivelLogado);

            HttpContext.Session.SetString("Mensagem", $"Seja bem vindo de volta, {possivelLogado.Nome}");
            return Redirect("../Home/Index");
        }

        [Authorize] // Não faz sentido logout ser chamado se o usuário já estiver logado
        public IActionResult Logout()
        {
            // Clear the existing external cookie
            HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            ViewBag.RecemAutenticado = false;

            HttpContext.Session.SetString("Mensagem", "Obrigado por utilizar nosso banco, volte sempre :)");
            return Redirect("../Home/Index");
        }

        [HttpGet]
        [RedirectAuthenticated("../Home/Index")]
        public IActionResult Cadastrar()
        {
            return View(new Cliente { Alterando = false }); // Como está cadastrando, é um cliente vazio
        }

        [HttpPost]
        [RedirectAuthenticated("../Home/Index")]
        public IActionResult Cadastrar(Cliente cliente)
        {

            var temUsuario = this.BancoContext.Clientes.Where(x => x.Usuario.Login == cliente.Email).FirstOrDefault();

            cliente.Alterando = false; // anti-overposting
            if (temUsuario != null)
            {
                cliente.MunicipiosDaUf = listaMunicipios(cliente.UF);
                ViewBag.ErroEmail = $"Já existe um cliente cadastrado com o E-mail {cliente.Email}";
                return View(cliente);
            }

            cliente.Conta = new Conta
            {
                Saldo = 0,
                SaldoPoupanca = 0,                
            };

            
            cliente.Usuario.Id = 0;
            cliente.Usuario.Login = cliente.Email;
            cliente.Usuario.Senha = BCrypt.Net.BCrypt.HashPassword(cliente.Usuario.Senha, BCrypt.Net.BCrypt.GenerateSalt());

            // Para entidades relacionadas, eu a seleciono no banco para ele não entender como valor novo
            cliente.Municipio = BancoContext.Municipios.Where(m => m.Id == cliente.Municipio.Id).FirstOrDefault();


            this.BancoContext.Add(cliente);
            this.BancoContext.SaveChanges();

            FazerLogin(cliente); // Ao cadastrar, o usuário´é automaticamente logado


            HttpContext.Session.SetString("Mensagem", "Seja Bem Vindo ao ImpactaBank, " + cliente.Nome);
            return Redirect("../Home/Index");
        }

        [HttpGet]
        public JsonResult BuscarMunicipios(UF? uf)
        {
            var listaRetorno = listaMunicipios(uf);
            if (listaRetorno == null)
            {
                return Json(new
                {
                    Erro = true,
                    Mensagem = "Selecione um estado para selecionar um município"
                });
            }

            return Json(listaRetorno);
            
        }


        private async void FazerLogin(Cliente cliente)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, cliente.Email),
                new Claim("FullName", cliente.Nome),
                new Claim(ClaimTypes.Role, "Cliente"),
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme))).Wait();

            ViewBag.RecemAutenticado = true;
        }

        private IList<Municipio> listaMunicipios(UF? uf)
        {
            if (uf == null)
            {
                return null;
            }

            IList<Municipio> municipios = this.BancoContext.Municipios.Where(m => m.Uf == uf).ToList();

            if (municipios == null || municipios.Count == 0)
            {
                using (HttpClient client = new HttpClient())
                {

                    var taskIBGE = client.GetAsync(String.Format("https://servicodados.ibge.gov.br/api/v1/localidades/estados/{0}/municipios",
                        uf.ToString()));
                    taskIBGE.Wait();

                    var municipiosResult = JsonConvert.DeserializeObject<IList<Municipio>>(taskIBGE.Result.Content.ReadAsStringAsync().Result);

                    foreach (var m in municipiosResult)
                    {
                        m.Id = 0;
                        m.Uf = uf.Value;
                        this.BancoContext.Add(m);
                    }
                    this.BancoContext.SaveChanges(); // salvar no banco para não precisar requisitar novamente depois

                    municipios = municipiosResult;
                }
            }

            return municipios;
        }
    }
}

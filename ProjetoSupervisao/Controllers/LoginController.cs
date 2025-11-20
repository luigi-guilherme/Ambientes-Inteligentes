using Microsoft.AspNetCore.Mvc;
using ProjetoSupervisao.DAO;
using ProjetoSupervisao.Models;

namespace ProjetoSupervisao.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FazLogin(string login, string senha)
        {
            UsuarioDAO dao = new UsuarioDAO();
            UsuarioViewModel usuario = dao.ConsultaLogin(login, senha);

            if (usuario != null)
            {
                HttpContext.Session.SetString("Logado", "true");
                HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Erro = "Usuário ou senha inválidos!";
                return View("Index");
            }
        }

        public IActionResult LogOff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
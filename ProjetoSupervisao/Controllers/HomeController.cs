using Microsoft.AspNetCore.Mvc;
using ProjetoSupervisao.Models;
using System.Diagnostics;

namespace ProjetoSupervisao.Controllers
{
    public class HomeController : PadraoController<PadraoViewModel>
    {
        public HomeController()
        {
            DAO = null;
        }

        public override IActionResult Index()
        {
            return View();
        }

        public IActionResult Sobre()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
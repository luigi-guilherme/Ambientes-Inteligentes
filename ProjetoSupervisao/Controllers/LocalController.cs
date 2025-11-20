using Microsoft.AspNetCore.Mvc;
using ProjetoSupervisao.DAO;
using ProjetoSupervisao.Models;

namespace ProjetoSupervisao.Controllers
{
    public class LocalController : PadraoController<LocalViewModel>
    {
        public LocalController()
        {
            DAO = new LocalDAO();
        }

        protected override void ValidaDados(LocalViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "O nome é obrigatório.");
        }
    }
}
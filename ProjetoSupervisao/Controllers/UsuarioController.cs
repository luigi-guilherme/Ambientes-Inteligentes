using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjetoSupervisao.DAO;
using ProjetoSupervisao.Models;

namespace ProjetoSupervisao.Controllers
{
    public class UsuarioController : PadraoController<UsuarioViewModel>
    {
        public UsuarioController()
        {
            DAO = new UsuarioDAO();
            ExigeAutenticacao = true; // Define o padrão como protegido
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string actionName = context.ActionDescriptor.RouteValues["action"].ToLower();

            // Libera o acesso público apenas para o fluxo de cadastro
            if (actionName == "create" || actionName == "save")
            {
                ExigeAutenticacao = false;
            }

            // Chama a lógica de autenticação do PadraoController
            base.OnActionExecuting(context);
        }

        protected override void PreparaDadosParaView(string Operacao, UsuarioViewModel model)
        {
            base.PreparaDadosParaView(Operacao, model);
            bool isLogged = HelperControllers.VerificaUserLogado(HttpContext.Session);

            // Envia para a View a informação se o botão "Voltar" deve ser exibido
            ViewBag.ShowBackButton = isLogged;
        }

        protected override void ValidaDados(UsuarioViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "O nome é obrigatório.");

            if (string.IsNullOrEmpty(model.Login))
                ModelState.AddModelError("Login", "O login é obrigatório.");

            if (string.IsNullOrEmpty(model.Senha))
                ModelState.AddModelError("Senha", "A senha é obrigatória.");
        }
    }
}
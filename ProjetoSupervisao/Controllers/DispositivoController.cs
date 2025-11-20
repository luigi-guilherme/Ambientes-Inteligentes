using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoSupervisao.DAO;
using ProjetoSupervisao.Models;
using System.Text.RegularExpressions; 


namespace ProjetoSupervisao.Controllers
{
    public class DispositivoController : PadraoController<DispositivoViewModel>
    {
        public DispositivoController()
        {
            DAO = new DispositivoDAO();
        }

        public override IActionResult Edit(int id)
        {
            try
            {
                ViewBag.Operacao = "A";
                DispositivoViewModel model = DAO.Consulta(id) as DispositivoViewModel;
                if (model == null)
                    return RedirectToAction(NomeViewIndex);

                // Pega "urn:ngsi-ld:Vinicola:001" e transforma em "Vinicola001"
                if (!string.IsNullOrEmpty(model.Device_Id_FIWARE))
                {
                    string urn = model.Device_Id_FIWARE.Replace("urn:ngsi-ld:", "");
                    model.ShortFiwareId = urn.Replace(":", "");
                }

                PreparaDadosParaView("A", model);
                return View(NomeViewForm, model);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        protected override void ValidaDados(DispositivoViewModel model, string operacao)
        {
            base.ValidaDados(model, operacao);

            if (string.IsNullOrEmpty(model.Nome))
                ModelState.AddModelError("Nome", "O nome é obrigatório.");

            if (model.LocalId <= 0)
                ModelState.AddModelError("LocalId", "Selecione um local.");

            if (string.IsNullOrEmpty(model.ShortFiwareId))
                ModelState.AddModelError("ShortFiwareId", "O ID FIWARE é obrigatório.");

            if (model.Imagem == null && operacao == "I")
                ModelState.AddModelError("Imagem", "Escolha uma imagem.");

            if (model.Imagem != null && model.Imagem.Length / 1024 / 1024 >= 2)
                ModelState.AddModelError("Imagem", "Imagem limitada a 2 mb.");
        }

        protected override void PreparaDadosParaView(string Operacao, DispositivoViewModel model)
        {
            base.PreparaDadosParaView(Operacao, model);
            CarregaLocais();
        }

        private void CarregaLocais()
        {
            var localDAO = new LocalDAO();
            var locais = localDAO.Listagem();
            var listaLocais = new List<SelectListItem>();

            listaLocais.Add(new SelectListItem("Selecione um local...", "0"));
            foreach (var local in locais)
            {
                listaLocais.Add(new SelectListItem(local.Nome, local.Id.ToString()));
            }
            ViewBag.Locais = listaLocais;
        }

        public override IActionResult Save(DispositivoViewModel model, string Operacao)
        {

            try
            {
                // Lógica do RegEx para quebrar o ID
                if (!string.IsNullOrEmpty(model.ShortFiwareId))
                {
                    var regex = new Regex(@"^([a-zA-Z]+)([0-9].*)$");
                    var match = regex.Match(model.ShortFiwareId);

                    if (match.Success)
                    {
                        string fiwareType = match.Groups[1].Value; // "Vinicola"
                        string fiwareId = match.Groups[2].Value;   // "001"
                        model.Device_Id_FIWARE = $"urn:ngsi-ld:{fiwareType}:{fiwareId}";
                    }
                    else
                    {
                        ModelState.AddModelError("ShortFiwareId", "Formato inválido. Use (Letras)(Números), ex: Vinicola001");
                    }
                }

                ValidaDados(model, Operacao);

                if (ModelState.IsValid == false)
                {
                    ViewBag.Operacao = Operacao;
                    PreparaDadosParaView(Operacao, model);
                    return View(NomeViewForm, model);
                }
                else
                {
                    if (model.Imagem != null)
                        model.ImagemEmByte = ConvertImageToByte(model.Imagem);

                    if (Operacao == "A" && model.Imagem == null)
                    {
                        var oldModel = DAO.Consulta(model.Id);
                        model.ImagemEmByte = oldModel.ImagemEmByte;
                    }

                    if (Operacao == "I")
                    {
                        DAO.Insert(model);

                    }
                    else
                    {
                        DAO.Update(model);
                    }

                    return RedirectToAction(NomeViewIndex);
                }
            }
            catch (Exception erro)
            {
                ViewBag.Operacao = Operacao;
                PreparaDadosParaView(Operacao, model);
                ModelState.AddModelError("", "Erro ao salvar: " + erro.Message);
                return View(NomeViewForm, model);
            }
        }

        public byte[] ConvertImageToByte(IFormFile file)
        {
            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            else
            {
                return null;
            }
        }
    }
}
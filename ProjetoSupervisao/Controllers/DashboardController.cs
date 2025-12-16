using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using ProjetoSupervisao.DAO;
using ProjetoSupervisao.Models;
using System.Globalization;

namespace ProjetoSupervisao.Controllers
{
    public class DashboardController : PadraoController<PadraoViewModel>
    {
        private static readonly TimeZoneInfo BrasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

        public DashboardController()
        {
            DAO = null;
        }

        public override IActionResult Index()
        {
            CarregaDispositivos();
            return View();
        }

        private void CarregaDispositivos()
        {
            var dispositivoDAO = new DispositivoDAO();
            var dispositivos = dispositivoDAO.ListaParaCombo();
            var listaSelect = new List<SelectListItem>();

            listaSelect.Add(new SelectListItem("Selecione um dispositivo...", "0"));
            foreach (var d in dispositivos)
            {
                listaSelect.Add(new SelectListItem(d.Nome, d.Device_Id_FIWARE));
            }
            ViewBag.Dispositivos = listaSelect;
        }

        public async Task<IActionResult> BuscarHistorico(string deviceId, int lastN)
        {
            if (string.IsNullOrEmpty(deviceId) || deviceId == "0")
                return Json(new { erro = "Selecione um dispositivo." });

            if (lastN <= 0)
                return Json(new { erro = "A quantidade deve ser maior que zero." });

            try
            {
                string urlBase = "http://3.92.218.251:8666";
                string entidade = deviceId;

                var dadosTemperatura = await ChamarApiSTH(urlBase, entidade, "temperature", lastN);
                var dadosUmidade = await ChamarApiSTH(urlBase, entidade, "humidity", lastN);
                var dadosLuminosidade = await ChamarApiSTH(urlBase, entidade, "luminosity", lastN);

                var viewModel = ProcessarDadosApi(dadosTemperatura, dadosUmidade, dadosLuminosidade);

                return Json(viewModel);
            }
            catch (Exception erro)
            {
                return Json(new { erro = erro.Message });
            }
        }

        private async Task<JObject> ChamarApiSTH(string urlBase, string entidade, string atributo, int lastN)
        {
            string url = $"{urlBase}/STH/v1/contextEntities/type/Vinicola/id/{entidade}/attributes/{atributo}?lastN={lastN}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("fiware-service", "smart");
                client.DefaultRequestHeaders.Add("fiware-servicepath", "/");

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JObject.Parse(json);
                }
                else
                {
                    throw new Exception($"Erro ao consultar API ({atributo}): {response.StatusCode}");
                }
            }
        }

        private HistoricoSensorViewModel ProcessarDadosApi(JObject tempJson, JObject umidJson, JObject lumJson)
        {
            var viewModel = new HistoricoSensorViewModel();

            var valoresTemp = tempJson["contextResponses"]?[0]?["contextElement"]?["attributes"]?[0]?["values"];
            if (valoresTemp != null)
            {
                foreach (var item in valoresTemp)
                {
                    DateTime dataUTC = Convert.ToDateTime(item["recvTime"]);
                    DateTime dataBrasilia = TimeZoneInfo.ConvertTime(dataUTC, BrasiliaTimeZone);
                    viewModel.Labels.Add(dataBrasilia.ToString("HH:mm:ss"));
                    viewModel.Temperaturas.Add(Convert.ToDouble(item["attrValue"]));
                }
            }

            var valoresUmid = umidJson["contextResponses"]?[0]?["contextElement"]?["attributes"]?[0]?["values"];
            if (valoresUmid != null)
            {
                foreach (var item in valoresUmid)
                {
                    viewModel.Umidades.Add(Convert.ToDouble(item["attrValue"]));
                }
            }

            var valoresLum = lumJson["contextResponses"]?[0]?["contextElement"]?["attributes"]?[0]?["values"];
            if (valoresLum != null)
            {
                foreach (var item in valoresLum)
                {
                    viewModel.Luminosidades.Add(Convert.ToDouble(item["attrValue"]));
                }
            }

            return viewModel;
        }
    }
}
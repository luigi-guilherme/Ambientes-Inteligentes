using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using ProjetoSupervisao.DAO;
using ProjetoSupervisao.Models;
using System.Globalization;
using System.Text;

namespace ProjetoSupervisao.Controllers
{
    public class GerenciadorController : PadraoController<PadraoViewModel>
    {
        private readonly string _orionUrl = "http://3.92.218.251:1026";

        public GerenciadorController()
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

        [HttpGet]
        public async Task<IActionResult> ObterDadosAtuais(string deviceId)
        {
            try
            {
                var viewModel = new TempoRealViewModel();

                var tempTask = ChamarApiOrion(deviceId, "temperature");
                var umidTask = ChamarApiOrion(deviceId, "humidity");
                var lumTask = ChamarApiOrion(deviceId, "luminosity");

                await Task.WhenAll(tempTask, umidTask, lumTask);

                viewModel.Temperatura = tempTask.Result.Valor;
                viewModel.Umidade = umidTask.Result.Valor;
                viewModel.Luminosidade = lumTask.Result.Valor;
                viewModel.Timestamp = tempTask.Result.Timestamp;

                return Json(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnviarComando([FromBody] ComandoRequest model)
        {
            try
            {
                string url = $"{_orionUrl}/v2/entities/{model.DeviceId}/attrs";

                string payload = $@"{{
                    ""{model.Comando}"": {{
                        ""type"": ""command"",
                        ""value"": """"
                    }}
                }}";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("fiware-service", "smart");
                    client.DefaultRequestHeaders.Add("fiware-servicepath", "/");

                    var content = new StringContent(payload, Encoding.UTF8, "application/json");
                    var response = await client.PatchAsync(url, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Falha ao enviar comando: {response.StatusCode}");
                    }
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        private TimeZoneInfo GetBrazilTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            }
        }

        private async Task<(double Valor, string Timestamp)> ChamarApiOrion(string deviceId, string atributo)
        {
            string url = $"{_orionUrl}/v2/entities/{deviceId}/attrs/{atributo}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("fiware-service", "smart");
                client.DefaultRequestHeaders.Add("fiware-servicepath", "/");

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var obj = JObject.Parse(json);

                    double valor = 0.0;
                    DateTime utcTime = DateTime.UtcNow;

                    if (obj["value"] != null && obj["value"].Type != JTokenType.Null)
                    {
                        string valorString = obj["value"].ToString().Replace(",", "."); // força ponto
                        valor = double.Parse(valorString, CultureInfo.InvariantCulture);
                    }

                    if (obj["metadata"]?["TimeInstant"]?["value"] != null)
                    {
                        utcTime = obj["metadata"]["TimeInstant"]["value"].ToObject<DateTime>();
                    }

                    var fusoHorarioBrasil = GetBrazilTimeZone();
                    DateTime brazilTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, fusoHorarioBrasil);
                    string timestampFormatado = brazilTime.ToString("dd/MM/yyyy HH:mm:ss");

                    return (valor, timestampFormatado);
                }
                else
                {
                    throw new Exception($"Erro ao consultar API ({atributo}): {response.StatusCode}");
                }
            }
        }
    }

    public class ComandoRequest
    {
        public string DeviceId { get; set; }
        public string Comando { get; set; }
    }
}
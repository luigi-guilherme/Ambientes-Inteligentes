using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoSupervisao.Models;
using ProjetoSupervisao.Services;

namespace ProjetoSupervisao.Controllers
{
    public class GerenciadorController : Controller
    {
        private readonly OrionApiService _orionService;
        private readonly GerenciadorService _gerenciadorService;

        public GerenciadorController(OrionApiService orionService, GerenciadorService gerenciadorService)
        {
            _orionService = orionService;
            _gerenciadorService = gerenciadorService;
        }

        public IActionResult Index()
        {
            CarregarComboLocais();
            return View();
        }

        private void CarregarComboLocais()
        {
            var locais = _gerenciadorService.ObterTodosLocais();
            var listaSelect = new List<SelectListItem>
            {
                new SelectListItem("Selecione um local...", "0")
            };
            listaSelect.AddRange(locais.Select(l => new SelectListItem(l.Nome, l.Id.ToString())));
            ViewBag.Locais = listaSelect;
        }

        [HttpGet]
        public IActionResult ObterDispositivosPorLocal(int localId)
        {
            try
            {
                var dispositivos = _gerenciadorService.ObterDispositivosPorLocal(localId);

                var listaJson = new List<object> { new { text = "Selecione um dispositivo...", value = "0" } };
                listaJson.AddRange(dispositivos.Select(d => new { text = d.Nome, value = d.Device_Id_FIWARE }));

                return Json(listaJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ObterTriggersPorLocal(int localId)
        {
            try
            {
                var triggers = _gerenciadorService.ObterTriggersDoLocal(localId);
                return Json(triggers);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult SalvarTriggersPorLocal([FromBody] SalvarTriggersLocalRequest request)
        {
            try
            {
                _gerenciadorService.AtualizarTriggersDoLocal(request);
                return Json(new { success = true, message = "Limites atualizados com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObterDadosAtuais(string deviceId)
        {
            try
            {
                var tempTask = _orionService.ObterValorAtributoAsync(deviceId, "temperature");
                var umidTask = _orionService.ObterValorAtributoAsync(deviceId, "humidity");
                var lumTask = _orionService.ObterValorAtributoAsync(deviceId, "luminosity");

                await Task.WhenAll(tempTask, umidTask, lumTask);

                var viewModel = new TempoRealViewModel
                {
                    Temperatura = tempTask.Result.Valor,
                    Umidade = umidTask.Result.Valor,
                    Luminosidade = lumTask.Result.Valor,
                    Timestamp = tempTask.Result.Timestamp
                };

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
                await _orionService.EnviarComandoAsync(model.DeviceId, model.Comando);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}
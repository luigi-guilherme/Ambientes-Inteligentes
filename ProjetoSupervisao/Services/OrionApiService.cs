using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;

namespace ProjetoSupervisao.Services
{
    public class OrionApiService
    {
        private readonly string _orionUrl = "http://3.92.218.251:1026";
        private readonly HttpClient _httpClient;

        public OrionApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("fiware-service", "smart");
            _httpClient.DefaultRequestHeaders.Add("fiware-servicepath", "/");
        }

        public async Task<(double Valor, string Timestamp)> ObterValorAtributoAsync(string deviceId, string atributo)
        {
            string url = $"{_orionUrl}/v2/entities/{deviceId}/attrs/{atributo}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao consultar API ({atributo}): {response.StatusCode}");

            string json = await response.Content.ReadAsStringAsync();
            var obj = JObject.Parse(json);

            double valor = 0.0;
            DateTime utcTime = DateTime.UtcNow;

            if (obj["value"] != null && obj["value"].Type != JTokenType.Null)
            {
                string valorString = obj["value"].ToString().Replace(",", ".");
                valor = double.Parse(valorString, CultureInfo.InvariantCulture);
            }

            if (obj["metadata"]?["TimeInstant"]?["value"] != null)
            {
                utcTime = obj["metadata"]["TimeInstant"]["value"].ToObject<DateTime>();
            }

            return (valor, FormatarParaHorarioBrasilia(utcTime));
        }

        public async Task EnviarComandoAsync(string deviceId, string comando)
        {
            string url = $"{_orionUrl}/v2/entities/{deviceId}/attrs";
            string payload = $@"{{
                ""{comando}"": {{
                    ""type"": ""command"",
                    ""value"": """"
                }}
            }}";

            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await _httpClient.PatchAsync(url, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Falha ao enviar comando: {response.StatusCode}");
        }

        private string FormatarParaHorarioBrasilia(DateTime utcTime)
        {
            TimeZoneInfo fusoHorarioBrasil;
            try
            {
                fusoHorarioBrasil = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
            }
            catch (TimeZoneNotFoundException)
            {
                fusoHorarioBrasil = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            }

            DateTime brazilTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, fusoHorarioBrasil);
            return brazilTime.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
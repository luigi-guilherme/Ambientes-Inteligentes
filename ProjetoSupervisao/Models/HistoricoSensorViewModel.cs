namespace ProjetoSupervisao.Models
{
    public class HistoricoSensorViewModel
    {
        public List<string> Labels { get; set; } = new List<string>();
        public List<double> Temperaturas { get; set; } = new List<double>();
        public List<double> Umidades { get; set; } = new List<double>();
        public List<double> Luminosidades { get; set; } = new List<double>();
    }
}
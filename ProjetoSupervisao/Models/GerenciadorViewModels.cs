namespace ProjetoSupervisao.Models
{

    public class ComandoRequest
    {
        public string DeviceId { get; set; }
        public string Comando { get; set; }
    }


    public class SalvarTriggersLocalRequest
    {
        public int LocalId { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public double UmidadeMin { get; set; }
        public double UmidadeMax { get; set; }
        public double LuminosidadeMin { get; set; }
        public double LuminosidadeMax { get; set; }
    }

    public class TriggersViewModel
    {
        public double? TempMin { get; set; }
        public double? TempMax { get; set; }
        public double? UmidadeMin { get; set; }
        public double? UmidadeMax { get; set; }
        public double? LuminosidadeMin { get; set; }
        public double? LuminosidadeMax { get; set; }
    }
}
using ProjetoSupervisao.Models; 

namespace ProjetoSupervisao.Models
{
    public class LocalViewModel : PadraoViewModel
    {
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        
        public double TempMin { get; set; } = 15.0;
        public double TempMax { get; set; } = 25.0;
        
        public double UmidadeMin { get; set; } = 30.0;
        public double UmidadeMax { get; set; } = 50.0;
        
        public double LuminosidadeMin { get; set; } = 0.0;
        public double LuminosidadeMax { get; set; } = 30.0;
    }
}
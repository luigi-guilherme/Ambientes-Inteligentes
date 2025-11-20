using ProjetoSupervisao.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjetoSupervisao.Models
{
    public class DispositivoViewModel : PadraoViewModel
    {
        public string Nome { get; set; }

        public int LocalId { get; set; }

        public string? Device_Id_FIWARE { get; set; }

        public string? ShortFiwareId { get; set; }

        public IFormFile? Imagem { get; set; }

        public byte[]? ImagemEmByte { get; set; }

        public string ImagemEmBase64
        {
            get
            {
                if (ImagemEmByte != null)
                    return Convert.ToBase64String(ImagemEmByte);
                else
                    return string.Empty;
            }
        }

        public string? NomeLocal { get; set; }
    }
}
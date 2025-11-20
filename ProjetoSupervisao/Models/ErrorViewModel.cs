namespace ProjetoSupervisao.Models
{
    /// <summary>
    /// Modelo para a tela de erro.
    /// Este código foi customizado para aceitar a mensagem de erro no construtor.
    /// Baseado no Capítulo 16 (pg. 36) da apostila.
    /// </summary>
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // --- INÍCIO DA MODIFICAÇÃO (pg. 36) ---

        /// <summary>
        /// Propriedade para guardar a mensagem de exceção.
        /// </summary>
        public string Erro { get; set; }

        /// <summary>
        /// Construtor padrão (necessário para o ASP.NET).
        /// </summary>
        public ErrorViewModel() { }

        /// <summary>
        /// Construtor que recebe a mensagem de erro (usado no PadraoController).
        /// </summary>
        public ErrorViewModel(string erro)
        {
            this.Erro = erro;
        }

        // --- FIM DA MODIFICAÇÃO ---
    }
}
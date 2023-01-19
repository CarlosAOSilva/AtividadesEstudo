using System;

namespace CadastroUsuario.ConsumoApi
{
    public class Request2
    {
        public DateTime DataHora { get; set; }
        public string MensagemErro { get; set; }
        public string RastreioErro { get; set; }
        public string NomeAplicacao { get; set; }
    }
}

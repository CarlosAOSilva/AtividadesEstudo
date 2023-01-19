using System;

namespace CadastroUsuario.Entidades
{
    public class Request
    {   
        public string Nome { get; set; }
        public Char Sexo { get; set; }
        public Int16 Idade { get; set; }
        public string Cep { get; set; }
        public Int32 Identificador { get; set; }

    }
}

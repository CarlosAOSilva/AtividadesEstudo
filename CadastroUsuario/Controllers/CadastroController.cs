using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Cors;
using System.Runtime.ConstrainedExecution;
using Microsoft.AspNetCore.Routing.Constraints;


namespace CadastroUsuario.Controllers
{
    [EnableCors("PermitirTudo")]
    [ApiController]
    public class CadastroController : ControllerBase
    {
        private readonly Conexoes.SqlServer _sql;
        public CadastroController()
        {
            _sql = new Conexoes.SqlServer();
        }

        [HttpGet("ConsultarCep")]
        public IActionResult ConsultarCep(string cep)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8)

                {
                    throw new InvalidOperationException("Cep inválido.");
                }

                var retorno = Conexoes.ConsumoApi.Get<Contratos.ViaCep.Response>("https://viacep.com.br/ws/" + cep + "/json/");
                return StatusCode(200, retorno);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        [HttpPost("CadastrarUsuario")]
        public IActionResult InserirCadastro(Entidades.Request cadastrar)
        {
            var logEntrada = new ConsumoApi.Request2();
            try
            {
                if (cadastrar.Idade <= 16)
                    throw new InvalidOperationException("Idade Inferior a necessária");
                if (string.IsNullOrEmpty(cadastrar.Nome) || cadastrar.Nome.Length < 3 || cadastrar.Nome.Length > 80)
                    throw new InvalidOperationException("Nome Inválido");
                if (cadastrar.Sexo != 'M' && cadastrar.Sexo != 'F')
                    throw new InvalidOperationException("Informação referente ao sexo Inválida!!! Digite M ou F");
                if (string.IsNullOrWhiteSpace(cadastrar.Cep) || cadastrar.Cep.Length != 8)
                    throw new InvalidOperationException("Cep inválido.");
               
                _sql.InserirCadastro(cadastrar);
                return StatusCode(200, cadastrar);

            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                logEntrada.MensagemErro = "ERRO!!! Digite Apenas Dados Válidos Conforme o Solicitado. Repita o Processo!!!";
                logEntrada.RastreioErro = ex.StackTrace;
                logEntrada.DataHora = DateTime.Now;
            }
            return StatusCode(500, "ERRO!!! Identificador Inexistente");

        }

        [HttpDelete("DeletarCadastro")]
        public IActionResult DeletarCadastro(int idCadastrar)
        {
            var logEntrada = new ConsumoApi.Request2();
            logEntrada.NomeAplicacao = "DeletarCadastro";
            try
            {
                if (_sql.VerificarExistenciaCadastro(idCadastrar) == false)
                    throw new InvalidOperationException("Identificador Inexistente");

                _sql.DeletarCadastro(idCadastrar);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                logEntrada.MensagemErro = "ERRO!!! Identificador Inexistente";
                logEntrada.RastreioErro = ex.StackTrace;
                logEntrada.DataHora = DateTime.Now;
            }
            return StatusCode(500, "ERRO!!! Identificador Inexistente");
        }
        
        [HttpGet("VerificarCadastros")]
        public IActionResult ListarCadastrados()
        {
            var lista = _sql.ListarCadastrados();
            return StatusCode(200, lista);
        }
    }
}
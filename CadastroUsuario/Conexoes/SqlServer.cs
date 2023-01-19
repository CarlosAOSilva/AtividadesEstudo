using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Specialized;
using System.Data.Common;

namespace CadastroUsuario.Conexoes
{
    public class SqlServer
    {
        private readonly SqlConnection _conexao;
        public SqlServer()
        {

            string stringConexao = File.ReadAllText(@"C:\Users\carlo\Desktop\ProjetoCarlos\CadastroUsuario\AcessoServidor.txt");
            _conexao = new SqlConnection(stringConexao);

        }

        public void InserirCadastro(Entidades.Request cadastro)
        {
            try
            {

                _conexao.Open();
                string query = @"INSERT INTO Cadastro
                                       (NOME
                                       ,Sexo
                                       ,Idade
                                       ,CEP
                                       ,Identificador)
                                      
                                 VALUES
                                       (@NOME
                                       ,@Sexo
                                       ,@Idade
                                       ,@CEP
                                       ,@Identificador);";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@NOME",cadastro.Nome);
                    cmd.Parameters.AddWithValue("@Idade",cadastro.Idade);
                    cmd.Parameters.AddWithValue("@Sexo", cadastro.Sexo);
                    cmd.Parameters.AddWithValue("@CEP", cadastro.Cep);
                    cmd.Parameters.AddWithValue("@Identificador", cadastro.Identificador);

                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {

                _conexao.Close();
            }
        }

        public bool VerificarExistenciaCadastro(int idCadastro)
        {
            try
            {
                _conexao.Open();
                string query = @"Select Count (Identificador) from Cadastro WHERE Identificador = @Identificador;";
                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("@Identificador", idCadastro);

                    return Convert.ToBoolean(cmd.ExecuteScalar());
                }
            }
            finally
            {
                _conexao.Close();
            }
        }

        public void DeletarCadastro(int idCadastro)
        {
            try
            {
                _conexao.Open();

                string query = @"DELETE FROM Cadastro
                                        Where Identificador = @Identificador";


                using (var cmd = new SqlCommand(query, _conexao))
                {
                    cmd.Parameters.AddWithValue("Identificador", idCadastro);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                _conexao.Close();
            }
        }


        public List<Entidades.Request>ListarCadastrados()
        {
            var cadastros = new List<Entidades.Request>();
            try
            {
                _conexao.Open();

                string query = @"Select * FROM Cadastro";

                using (var cmd = new SqlCommand(query, _conexao))
                {
                    var rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        var cadastrar = new Entidades.Request();

                        cadastrar.Nome = (string)rdr["Nome"];
                        cadastrar.Idade = Convert.ToInt16(rdr["Idade"]);
                        cadastrar.Sexo = Convert.ToChar(rdr["Sexo"]);
                        cadastrar.Cep = (string)rdr["CEP"];
                        cadastrar.Identificador = Convert.ToInt32(rdr["Identificador"]);

                        cadastros.Add(cadastrar);
                    }
                }
            }
            finally
            {
                _conexao.Close();
            }

            return cadastros;
        }
    }
}

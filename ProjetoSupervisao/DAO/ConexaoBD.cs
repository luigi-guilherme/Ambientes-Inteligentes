using Microsoft.AspNetCore.Hosting.Server;
using System.Data.SqlClient;

namespace ProjetoSupervisao.DAO
{
    public static class ConexaoBD
    {
        public static SqlConnection GetConexao()
        {
            string strCon = "Server = tcp:n2server.database.windows.net,1433; Initial Catalog = AMBIENTEDB; Persist Security Info = False; User ID = engenharia; Password = eng@123456; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";

            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
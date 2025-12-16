using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Data.SqlClient;

namespace ProjetoSupervisao.DAO
{
    public static class ConexaoBD
    {
        public static SqlConnection GetConexao()
        {
            string strCon = "Data Source=LOCALHOST; Initial Catalog=AMBIENTEDB; integrated security=true; Encrypt=False;";

            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }
    }
}
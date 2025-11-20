using ProjetoSupervisao.Models;
using System.Data;
using System.Data.SqlClient;

namespace ProjetoSupervisao.DAO
{
    public class UsuarioDAO : PadraoDAO<UsuarioViewModel>
    {
        protected override void SetTabela()
        {
            Tabela = "Usuarios";
            OrdemPadrao = "nome";
            ChaveIdentity = true;
        }

        protected override SqlParameter[] CriaParametros(UsuarioViewModel model)
        {
            return new SqlParameter[]
            {
                new SqlParameter("id", model.Id),
                new SqlParameter("nome", model.Nome),
                new SqlParameter("login", model.Login),
                new SqlParameter("senha", model.Senha)
            };
        }

        protected override UsuarioViewModel MontaModel(DataRow registro)
        {
            return new UsuarioViewModel
            {
                Id = Convert.ToInt32(registro["Id"]),
                Nome = registro["Nome"].ToString(),
                Login = registro["Login"].ToString(),
                Senha = registro["Senha"].ToString()
            };
        }

        public UsuarioViewModel ConsultaLogin(string login, string senha)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("login", login),
                new SqlParameter("senha", senha)
            };

            var tabela = HelperDAO.ExecutaProcSelect("spConsultaLogin", p);

            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }
    }
}
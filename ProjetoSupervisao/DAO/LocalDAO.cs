using ProjetoSupervisao.Models;
using System.Data;
using System.Data.SqlClient;

namespace ProjetoSupervisao.DAO
{
    public class LocalDAO : PadraoDAO<LocalViewModel>
    {
        protected override void SetTabela()
        {
            Tabela = "Locais";
            OrdemPadrao = "nome";
            ChaveIdentity = true;
        }

        protected override SqlParameter[] CriaParametros(LocalViewModel model)
        {
            return new SqlParameter[]
            {
                new SqlParameter("id", model.Id),
                new SqlParameter("nome", model.Nome),
                new SqlParameter("descricao", model.Descricao ?? (object)DBNull.Value)
            };
        }

        protected override LocalViewModel MontaModel(DataRow registro)
        {
            return new LocalViewModel
            {
                Id = Convert.ToInt32(registro["Id"]),
                Nome = registro["Nome"].ToString(),
                Descricao = registro["Descricao"] != DBNull.Value ? registro["Descricao"].ToString() : null
            };
        }
    }
}
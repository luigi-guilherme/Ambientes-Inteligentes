using ProjetoSupervisao.Models;
using System.Data;
using Microsoft.Data.SqlClient;

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
                new SqlParameter("descricao", model.Descricao ?? (object)DBNull.Value),
                new SqlParameter("tempMin", model.TempMin),
                new SqlParameter("tempMax", model.TempMax),
                new SqlParameter("umidadeMin", model.UmidadeMin),
                new SqlParameter("umidadeMax", model.UmidadeMax),
                new SqlParameter("luminosidadeMin", model.LuminosidadeMin),
                new SqlParameter("luminosidadeMax", model.LuminosidadeMax)
            };
        }

        protected override LocalViewModel MontaModel(DataRow registro)
        {
            return new LocalViewModel
            {
                Id = Convert.ToInt32(registro["Id"]),
                Nome = registro["Nome"].ToString(),
                Descricao = registro["Descricao"] != DBNull.Value ? registro["Descricao"].ToString() : null,
                TempMin = Convert.ToDouble(registro["TempMin"]),
                TempMax = Convert.ToDouble(registro["TempMax"]),
                UmidadeMin = Convert.ToDouble(registro["UmidadeMin"]),
                UmidadeMax = Convert.ToDouble(registro["UmidadeMax"]),
                LuminosidadeMin = Convert.ToDouble(registro["LuminosidadeMin"]),
                LuminosidadeMax = Convert.ToDouble(registro["LuminosidadeMax"])
            };
        }
    }
}
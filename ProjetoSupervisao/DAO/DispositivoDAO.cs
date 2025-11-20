using ProjetoSupervisao.Models;
using System.Data;
using System.Data.SqlClient;

namespace ProjetoSupervisao.DAO
{
    public class DispositivoDAO : PadraoDAO<DispositivoViewModel>
    {
        protected override void SetTabela()
        {
            Tabela = "Dispositivos";
            OrdemPadrao = "nome";
            ChaveIdentity = true;
        }

        protected override SqlParameter[] CriaParametros(DispositivoViewModel model)
        {
            object foto = model.ImagemEmByte;
            if (foto == null)
                foto = DBNull.Value;

            return new SqlParameter[]
            {
                new SqlParameter("id", model.Id),
                new SqlParameter("nome", model.Nome),
                new SqlParameter("localId", model.LocalId),
                new SqlParameter("device_id_fiware", model.Device_Id_FIWARE ?? (object)DBNull.Value),
                new SqlParameter("foto", foto)
            };
        }

        protected override DispositivoViewModel MontaModel(DataRow registro)
        {
            var model = new DispositivoViewModel
            {
                Id = Convert.ToInt32(registro["Id"]),
                Nome = registro["Nome"].ToString(),
                LocalId = Convert.ToInt32(registro["LocalId"]),
                Device_Id_FIWARE = registro["Device_Id_FIWARE"] != DBNull.Value ? registro["Device_Id_FIWARE"].ToString() : null
            };

            if (registro["Foto"] != DBNull.Value)
                model.ImagemEmByte = registro["Foto"] as byte[];

            if (registro.Table.Columns.Contains("NomeLocal"))
                model.NomeLocal = registro["NomeLocal"].ToString();

            return model;
        }

        public List<DispositivoViewModel> ListaParaCombo()
        {
            var tabela = HelperDAO.ExecutaProcSelect("spListagemDispositivos", null);
            var lista = new List<DispositivoViewModel>();
            foreach (DataRow dr in tabela.Rows)
            {
                lista.Add(new DispositivoViewModel
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    Nome = dr["Nome"].ToString(),
                    Device_Id_FIWARE = dr["Device_Id_FIWARE"].ToString()
                });
            }
            return lista;
        }
    }
}
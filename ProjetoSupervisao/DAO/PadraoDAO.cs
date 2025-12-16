using ProjetoSupervisao.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ProjetoSupervisao.DAO
{
    public abstract class PadraoDAO<T> where T : PadraoViewModel
    {
        protected string Tabela { get; set; }
        protected string NomeSpListagem { get; set; } = "spListagem";
        protected string OrdemPadrao { get; set; } = "id";
        protected bool ChaveIdentity { get; set; } = false;

        protected abstract SqlParameter[] CriaParametros(T model);

        protected abstract T MontaModel(DataRow registro);

        public PadraoDAO()
        {
            SetTabela();
        }
        protected abstract void SetTabela();


        public virtual int Insert(T model)
        {
            return HelperDAO.ExecutaProc("spInsert_" + Tabela, CriaParametros(model), ChaveIdentity);
        }

        public virtual void Update(T model)
        {
            HelperDAO.ExecutaProc("spUpdate_" + Tabela, CriaParametros(model));
        }

        public virtual void Delete(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("tabela", Tabela)
            };
            HelperDAO.ExecutaProc("spDelete", p);
        }

        public virtual T Consulta(int id)
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("id", id),
                new SqlParameter("tabela", Tabela)
            };
            var tabela = HelperDAO.ExecutaProcSelect("spConsulta", p);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        public virtual List<T> Listagem()
        {
            var p = new SqlParameter[]
            {
                new SqlParameter("tabela", Tabela),
                new SqlParameter("ordem", OrdemPadrao)
            };
            var tabela = HelperDAO.ExecutaProcSelect(NomeSpListagem, p);
            List<T> lista = new List<T>();
            foreach (DataRow registro in tabela.Rows)
                lista.Add(MontaModel(registro));

            return lista;
        }
    }
}
using ProjetoSupervisao.Controllers;
using ProjetoSupervisao.DAO;
using ProjetoSupervisao.Models;

namespace ProjetoSupervisao.Services
{
    public class GerenciadorService
    {
        private readonly LocalDAO _localDAO;
        private readonly DispositivoDAO _dispositivoDAO;

        public GerenciadorService()
        {
            _localDAO = new LocalDAO();
            _dispositivoDAO = new DispositivoDAO();
        }

        public List<LocalViewModel> ObterTodosLocais()
        {
            return _localDAO.Listagem();
        }

        public List<DispositivoViewModel> ObterDispositivosPorLocal(int localId)
        {
            if (localId <= 0) throw new ArgumentException("Local inválido");

            var todos = _dispositivoDAO.Listagem();
            return todos.Where(d => d.LocalId == localId).ToList();
        }

        public TriggersViewModel ObterTriggersDoLocal(int localId)
        {
            if (localId <= 0) throw new ArgumentException("Local inválido");

            var local = _localDAO.Consulta(localId);
            if (local == null) throw new Exception("Local não encontrado");

            return new TriggersViewModel
            {
                TempMin = local.TempMin,
                TempMax = local.TempMax,
                UmidadeMin = local.UmidadeMin,
                UmidadeMax = local.UmidadeMax,
                LuminosidadeMin = local.LuminosidadeMin,
                LuminosidadeMax = local.LuminosidadeMax
            };
        }

        public void AtualizarTriggersDoLocal(SalvarTriggersLocalRequest request)
        {
            if (request.LocalId <= 0) throw new ArgumentException("Local inválido");

            var local = _localDAO.Consulta(request.LocalId);
            if (local == null) throw new Exception("Local não encontrado");

            local.TempMin = request.TempMin;
            local.TempMax = request.TempMax;
            local.UmidadeMin = request.UmidadeMin;
            local.UmidadeMax = request.UmidadeMax;
            local.LuminosidadeMin = request.LuminosidadeMin;
            local.LuminosidadeMax = request.LuminosidadeMax;

            _localDAO.Update(local);
        }
    }
}
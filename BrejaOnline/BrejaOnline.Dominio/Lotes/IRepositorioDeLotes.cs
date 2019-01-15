using System.Collections.Generic;

namespace BrejaOnline.Dominio.Lotes
{
    public interface IRepositorioDeLotes
    {
        Lote ObterPeloId(string identificador);
        void Adiciona(Lote lote);
        List<Lote> ObterLotesPeloNomeDaCerveja(string nomeDaCerveja);
        List<Lote> ObterLotesPeloIdDaCerveja(int id);
        void Excluir(Lote lote);
        void AtualizarQuantidade(string id, int quantidade);
    }
}
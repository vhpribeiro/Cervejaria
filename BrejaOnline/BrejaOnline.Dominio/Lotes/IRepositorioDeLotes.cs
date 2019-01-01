using System.Collections.Generic;
using BrejaOnline.Dominio.Cervejas;

namespace BrejaOnline.Dominio.Lotes
{
    public interface IRepositorioDeLotes
    {
        Lote ObterPeloId(string identificador);
        void Adiciona(Lote lote);
        List<Lote> ObterLotesPeloNomeDaCerveja(string nomeDaCerveja);
        void Excluir(Lote lote);
        void AtualizarQuantidade(string id, int quantidade);
    }
}
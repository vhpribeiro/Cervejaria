using System;

namespace BrejaOnline.Dominio.Lotes
{
    public interface IRepositorioDeLotes
    {
        Lote ObterPeloId(string identificador);
        void Adiciona(Lote lote);
    }
}
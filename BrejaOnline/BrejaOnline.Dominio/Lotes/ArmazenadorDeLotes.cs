using System;
using BrejaOnline.Dominio.Cervejas;

namespace BrejaOnline.Dominio.Lotes
{
    public class ArmazenadorDeLotes
    {
        private readonly IRepositorioDeCerveja _repositorioDeCerveja;
        private readonly IRepositorioDeLotes _repositorioDeLotes;

        public ArmazenadorDeLotes(IRepositorioDeCerveja repositorioDeCerveja, IRepositorioDeLotes repositorioDeLotes)
        {
            _repositorioDeCerveja = repositorioDeCerveja;
            _repositorioDeLotes = repositorioDeLotes;
        }

        public void Armazenar(Lote loteEsperado)
        {
            var lote = _repositorioDeLotes.ObterPeloId(loteEsperado.Identificador);
            if (lote == null)
            {
                _repositorioDeLotes.Adiciona(loteEsperado);
            }
        }
    }
}
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

        public void AdicionaNoLote(string nomeDaCerveja, string identificador, int quantidadeASerAdicionada)
        {
            if (!_repositorioDeCerveja.VerificaSeExistePeloNome(nomeDaCerveja))
                throw new ArgumentException("Cerveja não encontrada");

            var lote = _repositorioDeLotes.ObterPeloId(identificador);
            lote.IncrementarQuantidade(quantidadeASerAdicionada);
        }

        public void ReduzirQuantidadeNoLote(string identificador, int quantidadeASerReduzida)
        {
            var lote = _repositorioDeLotes.ObterPeloId(identificador);
            lote.DecrementarQuantidade(quantidadeASerReduzida);
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
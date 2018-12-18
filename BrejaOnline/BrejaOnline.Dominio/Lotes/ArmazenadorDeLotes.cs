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

        public void AdicionaNoLote(string nomeDaCerveja, string lote, int quantidadeASerAdicionada)
        {
            if (!_repositorioDeCerveja.VerificaSeExistePeloNome(nomeDaCerveja))
                throw new ArgumentException("Cerveja não encontrada");

            var estoque = _repositorioDeLotes.ObterPeloLote(lote);
            estoque.IncrementarQuantidade(quantidadeASerAdicionada);
        }
    }
}
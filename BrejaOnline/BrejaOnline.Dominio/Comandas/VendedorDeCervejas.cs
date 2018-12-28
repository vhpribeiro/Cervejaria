using BrejaOnline.Dominio.Lotes;
using BrejaOnline.Dominio._Base;

namespace BrejaOnline.Dominio.Comandas
{
    public class VendedorDeCervejas
    {
        private readonly IRepositorioDeLotes _repositorioDeLotes;

        public VendedorDeCervejas(IRepositorioDeLotes repositorioDeLotes)
        {
            _repositorioDeLotes = repositorioDeLotes;
        }

        public void Venda(Comanda comanda)
        {
            ValidarVenda(comanda);
            var listaDeLotesComACervejaDesejada = _repositorioDeLotes.ObterPeloNomeDaCerveja(comanda.Cerveja.Nome);

            foreach (var lote in listaDeLotesComACervejaDesejada)
            {
                if (lote.Quantidade > comanda.Quantidade)
                {
                    lote.DecrementarQuantidade(comanda.Quantidade);
                    break;
                }
                else
                {
                    _repositorioDeLotes.Excluir(lote);
                    comanda.AlterarQuantidade(comanda.Quantidade - lote.Quantidade);
                }
            }
        }

        public void ValidarVenda(Comanda comanda)
        {
            var listaDeLotesComACervejaDesejada =_repositorioDeLotes.ObterPeloNomeDaCerveja(comanda.Cerveja.Nome);

            var quantidadeTotalQueSeTemNosLotes = 0;
            listaDeLotesComACervejaDesejada.ForEach(lote => 
                quantidadeTotalQueSeTemNosLotes += lote.Quantidade);

            ValidadorDeRegras.Novo()
                .Quando(quantidadeTotalQueSeTemNosLotes < comanda.Quantidade, "Quantidade indisponível")
                .DispararExcecaoSeExistir();
        }
    }
}
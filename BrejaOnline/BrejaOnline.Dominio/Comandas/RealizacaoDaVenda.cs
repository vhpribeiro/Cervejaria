using BrejaOnline.Dominio.Lotes;
using BrejaOnline.Dominio._Base;

namespace BrejaOnline.Dominio.Comandas
{
    public class RealizacaoDaVenda
    {
        private readonly IRepositorioDeLotes _repositorioDeLotes;

        public RealizacaoDaVenda(IRepositorioDeLotes repositorioDeLotes)
        {
            _repositorioDeLotes = repositorioDeLotes;
        }

        public void Vender(Comanda comanda)
        {
            ValidarVenda(comanda);
            var listaDeLotesComACervejaDesejada = _repositorioDeLotes.ObterPeloNomeDaCerveja(comanda.Cerveja.Nome);

            foreach (var lote in listaDeLotesComACervejaDesejada)
            {
                if (lote.Quantidade > comanda.Quantidade)
                {
                    //TODO verificar se vale a pena o método de adicionar e remover do lote, que estao no armazenadorDeLotes. Talvez eles devam ser chamados aqui?
                    lote.DecrementarQuantidade(comanda.Quantidade);
                    _repositorioDeLotes.Atualizar(lote);
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
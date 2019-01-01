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

        public void Vender(Pedido pedido)
        {
            ValidarVenda(pedido);
            var listaDeLotesComACervejaDesejada = _repositorioDeLotes.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome);

            foreach (var lote in listaDeLotesComACervejaDesejada)
            {
                if (lote.Quantidade > pedido.Quantidade)
                {
                    lote.DecrementarQuantidade(pedido.Quantidade);
                    _repositorioDeLotes.AtualizarQuantidade(lote.Identificador, lote.Quantidade);
                    break;
                }
                else
                {
                    _repositorioDeLotes.Excluir(lote);
                    pedido.AlterarQuantidade(pedido.Quantidade - lote.Quantidade);
                }
            }
        }
        
        public void ValidarVenda(Pedido pedido)
        {
            var lotesComACervejaDesejada =_repositorioDeLotes.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome);

            var quantidadeTotalQueSeTemNosLotes = 0;
            lotesComACervejaDesejada.ForEach(lote => 
                quantidadeTotalQueSeTemNosLotes += lote.Quantidade);

            ValidadorDeRegras.Novo()
                .Quando(quantidadeTotalQueSeTemNosLotes < pedido.Quantidade, Resource.QuantidadeIndisponivel)
                .DispararExcecaoSeExistir();
        }
    }
}
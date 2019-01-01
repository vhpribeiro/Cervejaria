using System.Collections.Generic;
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
            comanda.Pedidos.ForEach(pedido =>
            {
                var lotesComACervejaDesejada = _repositorioDeLotes.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome);
                ValidarVenda(lotesComACervejaDesejada, pedido);
                AlterarQuantidade(lotesComACervejaDesejada, pedido);
            });
        }
        
        public void ValidarVenda(List<Lote> lotesComACervejaDesejada, Pedido pedido)
        {
            var quantidadeTotalQueSeTemNosLotes = 0;
            lotesComACervejaDesejada.ForEach(lote => 
                quantidadeTotalQueSeTemNosLotes += lote.Quantidade);

            ValidadorDeRegras.Novo()
                .Quando(quantidadeTotalQueSeTemNosLotes < pedido.Quantidade, Resource.QuantidadeIndisponivel)
                .DispararExcecaoSeExistir();
        }

        public void AlterarQuantidade(List<Lote> lotesComACervejaDesejada, Pedido pedido)
        {
            foreach (var lote in lotesComACervejaDesejada)
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
    }
}
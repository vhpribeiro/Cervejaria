using System;
using System.Collections.Generic;
using System.Linq;
using BrejaOnline.Dominio.Comandas;
using BrejaOnline.Dominio.Lotes;
using BrejaOnline.Dominio.Test._Builders;
using BrejaOnline.Dominio.Test._Util;
using BrejaOnline.Dominio._Base;
using Moq;
using Xunit;

namespace BrejaOnline.Dominio.Test.Comandas
{
    public class RealizacaoDaVendaTest
    {
        private readonly Mock<IRepositorioDeLotes> _repositorioDeLotes;
        private readonly RealizacaoDaVenda _realizacaoDaVenda;

        public RealizacaoDaVendaTest()
        {
            _repositorioDeLotes = new Mock<IRepositorioDeLotes>();
            _realizacaoDaVenda = new RealizacaoDaVenda(_repositorioDeLotes.Object);
        }
        [Fact]
        public void Deve_realizar_uma_busca_nos_lotes_pelo_nome_da_cerveja()
        {
            var pedido = PedidoBuilder.Novo().Criar();
            var listaDeLotesObtida = new List<Lote>()
            {
                new Lote(pedido.Cerveja, 7),
                new Lote(pedido.Cerveja, 3)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome))
                .Returns(listaDeLotesObtida);

            _realizacaoDaVenda.ValidarVenda(pedido);

            _repositorioDeLotes.Verify(repositorio =>
                repositorio.ObterLotesPeloNomeDaCerveja(It.Is<string>(nomeDaCerveja => nomeDaCerveja == pedido.Cerveja.Nome)));
        }

        [Fact]
        public void Nao_deve_realizar_uma_venda_caso_nao_tenha_quantidade_necessaria()
        {
            var pedido = PedidoBuilder.Novo().ComQuantidade(6).Criar();
            var listaDeLotesObtida = new List<Lote>()
            {
                new Lote(pedido.Cerveja, 2),
                new Lote(pedido.Cerveja, 3)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome))
                .Returns(listaDeLotesObtida);

            Action acao = () => _realizacaoDaVenda.ValidarVenda(pedido);

            Assert.Throws<ExcecaoDeDominio>(acao).ComMensagem(Resource.QuantidadeIndisponivel);
        }

        [Fact]
        public void Deve_diminuir_quantidade_do_lote_caso_o_do_pedido_for_menor()
        {
            const int quantidadeEsperada = 1;
            var pedido = PedidoBuilder.Novo().ComQuantidade(7).Criar();
            var listaDeLotes = new List<Lote>
            {
                new Lote(pedido.Cerveja, 8)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome))
                .Returns(listaDeLotes);

            _realizacaoDaVenda.Vender(pedido);

            Assert.Equal(quantidadeEsperada, listaDeLotes.First().Quantidade);
        }

        [Fact]
        public void Deve_atualizar_o_lote_com_a_nova_quantidade_no_repositorio()
        {
            var pedido = PedidoBuilder.Novo().ComQuantidade(7).Criar();
            var listaDeLotes = new List<Lote>
            {
                new Lote(pedido.Cerveja, 8)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome))
                .Returns(listaDeLotes);

            _realizacaoDaVenda.Vender(pedido);

            _repositorioDeLotes.Verify(repositorio => repositorio.AtualizarQuantidade
                (It.Is<string>(loteId => loteId.Equals(listaDeLotes.First().Identificador)),
                It.Is<int>(loteQuantidade => loteQuantidade.Equals(listaDeLotes.First().Quantidade))));
        }

        [Fact]
        public void Deve_excluir_lote_caso_a_quantidade_do_pedido_for_maior()
        {
            var pedido = PedidoBuilder.Novo().ComQuantidade(10).Criar();
            var listaDeLotes = new List<Lote>
            {
                new Lote(pedido.Cerveja, 5),
                new Lote(pedido.Cerveja, 3),
                new Lote(pedido.Cerveja, 4)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome))
                .Returns(listaDeLotes);

            _realizacaoDaVenda.Vender(pedido);

            _repositorioDeLotes.Verify(repositorio => repositorio.Excluir(It.IsAny<Lote>()), Times.AtLeast(2));
        }
    }
}

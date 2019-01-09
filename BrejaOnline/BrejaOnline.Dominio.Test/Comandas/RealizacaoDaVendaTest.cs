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
        private readonly List<Pedido> _pedidos;
        private readonly List<Lote> _lotes;

        public RealizacaoDaVendaTest()
        {
            _repositorioDeLotes = new Mock<IRepositorioDeLotes>();
            _realizacaoDaVenda = new RealizacaoDaVenda(_repositorioDeLotes.Object);
            _pedidos = new List<Pedido>();
            _lotes = new List<Lote>();
        }
        [Fact]
        public void Deve_realizar_uma_busca_nos_lotes_pelo_nome_da_cerveja()
        {
            var primeiroPedido = PedidoBuilder.Novo().Criar();
            var cervejaDoSegundoPedido = CervejaBuilder.Novo().ComNome("Budweiser").ComPreco(6.75).Criar();
            var segundoPedido = PedidoBuilder.Novo().ComCerveja(cervejaDoSegundoPedido).Criar();
            _pedidos.Add(primeiroPedido);
            _pedidos.Add(segundoPedido);
            var comanda = ComandaBuilder.Novo().ComPedidos(_pedidos).Criar();
            var lotesQueContemPrimeiraCerveja = new List<Lote>()
            {
                new Lote(primeiroPedido.Cerveja, 7),
                new Lote(primeiroPedido.Cerveja, 3)
            };
            var lotesQueContemSegundaCerveja = new List<Lote>()
            {
                new Lote(segundoPedido.Cerveja, 7),
                new Lote(segundoPedido.Cerveja, 3)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterLotesPeloNomeDaCerveja(primeiroPedido.Cerveja.Nome))
                .Returns(lotesQueContemPrimeiraCerveja);
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterLotesPeloNomeDaCerveja(segundoPedido.Cerveja.Nome))
                .Returns(lotesQueContemSegundaCerveja);

            _realizacaoDaVenda.Vender(comanda);

            _repositorioDeLotes.Verify(repositorio =>
                repositorio.ObterLotesPeloNomeDaCerveja(It.Is<string>(nomeDaCerveja => nomeDaCerveja == primeiroPedido.Cerveja.Nome)));
            _repositorioDeLotes.Verify(repositorio =>
                repositorio.ObterLotesPeloNomeDaCerveja(It.Is<string>(nomeDaCerveja => nomeDaCerveja == segundoPedido.Cerveja.Nome)));
        }

        [Fact]
        public void Nao_deve_realizar_uma_venda_caso_nao_tenha_quantidade_necessaria()
        {
            var pedido = PedidoBuilder.Novo().ComQuantidade(6).Criar();
            _lotes.Add(new Lote(pedido.Cerveja, 2));
            _lotes.Add(new Lote(pedido.Cerveja, 3));
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome))
                .Returns(_lotes);

            Action acao = () => _realizacaoDaVenda.ValidarVenda(_lotes, pedido);

            Assert.Throws<ExcecaoDeDominio>(acao).ComMensagem(Resource.QuantidadeIndisponivel);
        }

        [Fact]
        public void Deve_diminuir_quantidade_do_lote_caso_o_do_pedido_for_menor()
        {
            const int quantidadeEsperada = 1;
            var pedido = PedidoBuilder.Novo().ComQuantidade(7).Criar();
            _pedidos.Add(pedido);
            var primeiroLote = new Lote(pedido.Cerveja, 8);
            _lotes.Add(primeiroLote);
            var comanda = ComandaBuilder.Novo().ComPedidos(_pedidos).Criar();
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome))
                .Returns(_lotes);

            _realizacaoDaVenda.Vender(comanda);

            Assert.Equal(quantidadeEsperada, primeiroLote.Quantidade);
        }

        [Fact]
        public void Deve_atualizar_o_lote_com_a_nova_quantidade_no_repositorio()
        {
            var pedido = PedidoBuilder.Novo().ComQuantidade(7).Criar();
            _pedidos.Add(pedido);
            var comanda = ComandaBuilder.Novo().ComPedidos(_pedidos).Criar();
            var primeiroLote = new Lote(pedido.Cerveja, 8);
            _lotes.Add(primeiroLote);
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome))
                .Returns(_lotes);

            _realizacaoDaVenda.Vender(comanda);

            _repositorioDeLotes.Verify(repositorio => repositorio.AtualizarQuantidade
                (It.Is<string>(loteId => loteId == primeiroLote.Identificador),
                It.Is<int>(loteQuantidade => loteQuantidade == primeiroLote.Quantidade)));
        }

        [Fact]
        public void Deve_excluir_lote_caso_a_quantidade_do_pedido_for_maior()
        {
            var pedido = PedidoBuilder.Novo().ComQuantidade(10).Criar();
            _pedidos.Add(pedido);
            var comanda = ComandaBuilder.Novo().ComPedidos(_pedidos).Criar();
            var primeiroLote = new Lote(pedido.Cerveja, 5);
            var segundoLote = new Lote(pedido.Cerveja, 3);
            var terceiroLote = new Lote(pedido.Cerveja, 4);
            _lotes.Add(primeiroLote);
            _lotes.Add(segundoLote);
            _lotes.Add(terceiroLote);
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterLotesPeloNomeDaCerveja(pedido.Cerveja.Nome))
                .Returns(_lotes);

            _realizacaoDaVenda.Vender(comanda);

            _repositorioDeLotes.Verify(repositorio => repositorio.Excluir(It.Is<Lote>(lote => lote == primeiroLote)));
            _repositorioDeLotes.Verify(repositorio => repositorio.Excluir(It.Is<Lote>(lote => lote == segundoLote)));
        }
    }
}

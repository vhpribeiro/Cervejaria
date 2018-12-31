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
            var comanda = ComandaBuilder.Novo().Criar();
            var listaDeLotesObtida = new List<Lote>()
            {
                new Lote(comanda.Cerveja, 7),
                new Lote(comanda.Cerveja, 3)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterPeloNomeDaCerveja(comanda.Cerveja.Nome))
                .Returns(listaDeLotesObtida);

            _realizacaoDaVenda.ValidarVenda(comanda);

            _repositorioDeLotes.Verify(repositorio =>
                repositorio.ObterPeloNomeDaCerveja(It.Is<string>(nomeDaCerveja => nomeDaCerveja == comanda.Cerveja.Nome)));
        }

        [Fact]
        public void Nao_deve_realizar_uma_venda_caso_nao_tenha_quantidade_necessaria()
        {
            var comanda = ComandaBuilder.Novo().ComQuantidade(6).Criar();
            var listaDeLotesObtida = new List<Lote>()
            {
                new Lote(comanda.Cerveja, 2),
                new Lote(comanda.Cerveja, 3)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterPeloNomeDaCerveja(comanda.Cerveja.Nome))
                .Returns(listaDeLotesObtida);

            Action acao = () => _realizacaoDaVenda.ValidarVenda(comanda);

            Assert.Throws<ExcecaoDeDominio>(acao).ComMensagem(Resource.QuantidadeIndisponivel);
        }

        [Fact]
        public void Deve_diminuir_quantidade_do_lote_caso_a_da_comanda_for_menor()
        {
            const int quantidadeEsperada = 1;
            var comanda = ComandaBuilder.Novo().ComQuantidade(7).Criar();
            var listaDeLotes = new List<Lote>
            {
                new Lote(comanda.Cerveja, 8)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterPeloNomeDaCerveja(comanda.Cerveja.Nome))
                .Returns(listaDeLotes);

            _realizacaoDaVenda.Vender(comanda);

            Assert.Equal(quantidadeEsperada, listaDeLotes.First().Quantidade);
        }

        [Fact]
        public void Deve_atualizar_o_lote_com_a_nova_quantidade_no_repositorio()
        {
            const int quantidadeEsperada = 1;
            var comanda = ComandaBuilder.Novo().ComQuantidade(7).Criar();
            var listaDeLotes = new List<Lote>
            {
                new Lote(comanda.Cerveja, 8)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterPeloNomeDaCerveja(comanda.Cerveja.Nome))
                .Returns(listaDeLotes);

            _realizacaoDaVenda.Vender(comanda);

            _repositorioDeLotes.Verify(repositorio => repositorio.Atualizar(It.Is<Lote>(lote => lote.Quantidade == quantidadeEsperada)));
        }

        [Fact]
        public void Deve_excluir_lote_caso_a_quantidade_da_comanda_for_maior()
        {
            var comanda = ComandaBuilder.Novo().ComQuantidade(10).Criar();
            var listaDeLotes = new List<Lote>
            {
                new Lote(comanda.Cerveja, 5),
                new Lote(comanda.Cerveja, 3),
                new Lote(comanda.Cerveja, 4)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterPeloNomeDaCerveja(comanda.Cerveja.Nome))
                .Returns(listaDeLotes);

            _realizacaoDaVenda.Vender(comanda);

            _repositorioDeLotes.Verify(repositorio => repositorio.Excluir(It.IsAny<Lote>()), Times.AtLeast(2));
        }
    }
}

using System;
using BrejaOnline.Dominio.Comandas;
using BrejaOnline.Dominio.Test._Builders;
using BrejaOnline.Dominio.Test._Util;
using BrejaOnline.Dominio._Base;
using ExpectedObjects;
using Xunit;

namespace BrejaOnline.Dominio.Test.Comandas
{
    public class PedidoTest
    {
        [Fact]
        public void Deve_criar_um_pedido()
        {
            var cerveja = CervejaBuilder.Novo().Criar();
            const int quantidade = 4;
            var valor = cerveja.Preco * quantidade;
            var pedidoEsperado = new
            {
                Cerveja = cerveja,
                Quantidade = quantidade,
                Valor = valor
            }.ToExpectedObject();

            var pedidoObtido = new Pedido(cerveja, quantidade);

            pedidoEsperado.ShouldMatch(pedidoObtido);
        }

        [Fact]
        public void Nao_deve_permitir_quantidade_invalida()
        {
            const int quantidadeInvalida = -6;

            Action acao = () => PedidoBuilder.Novo().ComQuantidade(quantidadeInvalida).Criar();

            Assert.Throws<ExcecaoDeDominio>(acao).ComMensagem(Resource.QuantidadeInvalida);
        }

        [Fact]
        public void Deve_alterar_a_quantidade()
        {
            const int quantidadeEsperada = 5;
            var pedido = PedidoBuilder.Novo().Criar();

            pedido.AlterarQuantidade(quantidadeEsperada);

            Assert.Equal(quantidadeEsperada, pedido.Quantidade);
        }

        [Fact]
        public void Nao_deve_alterar_para_uma_quantidade_invalida()
        {
            const int quantidadeInvalida = -6;
            var pedido = PedidoBuilder.Novo().Criar();

            Action acao = () => pedido.AlterarQuantidade(quantidadeInvalida);

            Assert.Throws<ExcecaoDeDominio>(acao).ComMensagem(Resource.QuantidadeInvalida);
        }
    }
}

using System;
using BrejaOnline.Dominio.Comandas;
using BrejaOnline.Dominio.Test.Builders;
using BrejaOnline.Dominio._Base;
using ExpectedObjects;
using Xunit;

namespace BrejaOnline.Dominio.Test.Comandas
{
    public class ComandaTest
    {
        [Fact]
        public void Deve_criar_uma_comanda()
        {
            var cerveja = CervejaBuilder.Novo().Criar();
            const int quantidade = 4;
            double valorTotal = cerveja.Preco * quantidade;
            var comandaEsperada = new
            {
                Cerveja = cerveja,
                Quantidade = quantidade,
                ValorTotal = valorTotal
            }.ToExpectedObject();

            var comandaObtida = new Comanda(cerveja, quantidade);

            comandaEsperada.ShouldMatch(comandaObtida);
        }

        [Fact]
        public void Nao_deve_permitir_quantidade_invalida()
        {
            const int quantidadeInvalida = -6;

            Action acao = () => ComandaBuilder.Novo().ComQuantidade(quantidadeInvalida).Criar();

            Assert.Throws<ExcecaoDeDominio>(acao);
        }

        [Fact]
        public void Deve_alterar_a_quantidade()
        {
            const int quantidadeEsperada = 5;
            var comanda = ComandaBuilder.Novo().Criar();

            comanda.AlterarQuantidade(quantidadeEsperada);

            Assert.Equal(quantidadeEsperada, comanda.Quantidade);
        }

        [Fact]
        public void Nao_deve_alterar_para_uma_quantidade_invalida()
        {
            const int quantidadeInvalida = -6;
            var comanda = ComandaBuilder.Novo().Criar();

            Assert.Throws<ExcecaoDeDominio>(() => comanda.AlterarQuantidade(quantidadeInvalida));
        }
    }
}

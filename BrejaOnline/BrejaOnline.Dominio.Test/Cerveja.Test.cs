using System;
using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Test.Builders;
using BrejaOnline.Dominio._Base;
using ExpectedObjects;
using Xunit;

namespace BrejaOnline.Dominio.Test
{
    public class CervejaTest
    {
        public string _nome;
        public double _preco;
        public string _descricao;
        public TipoDeCerveja _tipo;


        public CervejaTest()
        {
            _nome = "Skoll";
            _preco = 7.50;
            _descricao = "Cerveja barata";
            _tipo = TipoDeCerveja.LAGER;
        }

        [Fact]
        public void Deve_criar_uma_cerveja()
        {
            var cervejaDesejada = new
            {
                Nome = _nome,
                Preco = _preco,
                Descricao = _descricao,
                Tipo = _tipo
            };

            var cervejaCriada = new Cerveja(cervejaDesejada.Nome, cervejaDesejada.Preco, cervejaDesejada.Descricao,
                 cervejaDesejada.Tipo);

            cervejaDesejada.ToExpectedObject().ShouldMatch(cervejaCriada);
        }

        [Fact]
        public void Nao_deve_aceitar_preco_invalido()
        {
            Assert.Throws<ExcecaoDeDominio>(() => CervejaBuilder.Novo().ComPreco(-7.5).Criar());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Nao_deve_aceitar_nome_invalido(string nomeInvalido)
        {
            Assert.Throws<ExcecaoDeDominio>(() => CervejaBuilder.Novo().ComNome(nomeInvalido).Criar());
        }

        [Fact]
        public void Deve_alterar_o_nome()
        {
            const string nomeEsperado = "Budweiser";
            var cerveja = CervejaBuilder.Novo().ComNome("Brahma").Criar();

            cerveja.AlterarNome(nomeEsperado);

            Assert.Equal(nomeEsperado, cerveja.Nome);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Nao_deve_permitir_alterar_nome_por_nome_invalido(string nomeInvalido)
        {
            var cerveja = CervejaBuilder.Novo().Criar();

            Assert.Throws<ExcecaoDeDominio>(() => cerveja.AlterarNome(nomeInvalido));
        }
    }
}

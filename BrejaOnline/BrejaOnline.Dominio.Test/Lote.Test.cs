using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Test.Builders;
using ExpectedObjects;
using System;
using BrejaOnline.Dominio.Lotes;
using Xunit;

namespace BrejaOnline.Dominio.Test
{
    public class LoteTest
    {
        private readonly Cerveja _cerveja;
        private readonly string _lote;

        public LoteTest()
        {
            _cerveja = CervejaBuilder.Novo().Criar();
            _lote = DateTime.Now.ToString("yyyyMMdd");
        }
        
        [Fact]
        public void Deve_criar_um_lote_no_estoque()
        {
            var estoqueEsperado = new
            {
                Cerveja = _cerveja,
                Quantidade = 3,
                Identificador = _lote
            }.ToExpectedObject();

            var estoqueDesejado = new Lote(_cerveja, 3);

            estoqueEsperado.ShouldMatch(estoqueDesejado);
        }

        [Theory]
        [InlineData(-6)]
        [InlineData(-100)]
        public void Nao_deve_aceitar_quantidade_invalida(int quantidadeInvalida)
        {
            Assert.Throws<ArgumentException>(() => new Lote(_cerveja, quantidadeInvalida));
        }

        [Theory]
        [InlineData(50)]
        [InlineData(14)]
        public void Deve_alterar_a_quantidade_do_estoque(int quantidadeASerAdicionada)
        {
            var cerveja = CervejaBuilder.Novo().Criar();
            var estoque = new Lote(cerveja, 5);
            var quantidadeEsperada = estoque.Quantidade + quantidadeASerAdicionada;

            estoque.IncrementarQuantidade(quantidadeASerAdicionada);

            Assert.Equal(quantidadeEsperada, estoque.Quantidade);
        }

        [Fact]
        public void Nao_deve_permitir_alterar_quantidade_para_valor_invalido()
        {
            var cerveja = CervejaBuilder.Novo().Criar();
            var estoque = new Lote(cerveja, 6);

            Assert.Throws<ArgumentException>(() => estoque.IncrementarQuantidade(-90));
        }

        [Fact]
        public void Nao_deve_permitir_cerveja_invalida()
        {
            Assert.Throws<ArgumentException>(() => new Lote(null, 5));
        }

        [Fact]
        public void Deve_criar_estoque_com_lote_valido()
        {
            var loteEsperado = DateTime.Now.ToString("yyyyMMdd");

            var estoque = new Lote(CervejaBuilder.Novo().Criar(), 10);

            Assert.Equal(loteEsperado, estoque.Identificador);
        }

    }
}

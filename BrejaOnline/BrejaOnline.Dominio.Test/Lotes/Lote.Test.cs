using System;
using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Lotes;
using BrejaOnline.Dominio.Test._Builders;
using BrejaOnline.Dominio.Test._Util;
using BrejaOnline.Dominio._Base;
using ExpectedObjects;
using Xunit;

namespace BrejaOnline.Dominio.Test.Lotes
{
    public class LoteTest
    {
        private readonly Cerveja _cerveja;
        private readonly string _identificador;

        public LoteTest()
        {
            _cerveja = CervejaBuilder.Novo().Criar();
            _identificador = DateTime.Now.ToString("yyyyMMdd");
        }
        
        [Fact]
        public void Deve_criar_um_lote_no_estoque()
        {
            var estoqueEsperado = new
            {
                Cerveja = _cerveja,
                Quantidade = 3,
                Identificador = _identificador
            }.ToExpectedObject();

            var estoqueDesejado = new Lote(_cerveja, 3);

            estoqueEsperado.ShouldMatch(estoqueDesejado);
        }

        [Theory]
        [InlineData(-6)]
        [InlineData(-100)]
        public void Nao_deve_aceitar_quantidade_invalida(int quantidadeInvalida)
        {
            Action acao = () => new Lote(_cerveja, quantidadeInvalida);

            Assert.Throws<ExcecaoDeDominio>(acao).ComMensagem(Resource.QuantidadeInvalida);
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

        [Theory]
        [InlineData(-49)]
        [InlineData(-150)]
        public void Nao_deve_permitir_alterar_quantidade_para_valores_invalido_ao_incrementar(int valorInvalido)
        {
            const int quantidadeBase = 6;
            var cerveja = CervejaBuilder.Novo().Criar();
            var estoque = new Lote(cerveja, quantidadeBase);

            Action acao = () => estoque.IncrementarQuantidade(valorInvalido);

            Assert.Throws<ExcecaoDeDominio>(acao).ComMensagem(Resource.QuantidadeAdicionadaEhInvalida);
        }

        [Theory]
        [InlineData(22, 44)]
        [InlineData(22, -44)]
        public void Nao_deve_permitir_alterar_quantidade_para_valores_invalido_ao_decrementar(int quantidadeBase, int valorInvalido)
        {
            var cerveja = CervejaBuilder.Novo().Criar();
            var lote = new Lote(cerveja, quantidadeBase);

            Action acao = () => lote.DecrementarQuantidade(valorInvalido);

            Assert.Throws<ExcecaoDeDominio>(acao).ComMensagem(Resource.QuantidadeReduzidaEhInvalida);
        }

        [Fact]
        public void Nao_deve_permitir_cerveja_invalida()
        {
            Cerveja cervejaInvalida = null;

            Action acao = () => new Lote(cervejaInvalida, 5);

            Assert.Throws<ExcecaoDeDominio>(acao).ComMensagem(Resource.CervejaInvalida);
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

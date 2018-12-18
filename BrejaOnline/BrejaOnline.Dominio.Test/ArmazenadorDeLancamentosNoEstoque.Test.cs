using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Lotes;
using BrejaOnline.Dominio.Test.Builders;
using Moq;
using System;
using Xunit;

namespace BrejaOnline.Dominio.Test
{
    public class ArmazenadorDeLancamentosNoEstoqueTest
    {
        private readonly Mock<IRepositorioDeCerveja> _armazenadorDeCerveja;
        private readonly Mock<IRepositorioDeLotes> _armazenadorDeEstoque;
        private readonly ArmazenadorDeLotes _armazenadorDeLotes;
        private readonly string _lote;

        public ArmazenadorDeLancamentosNoEstoqueTest()
        {
            _armazenadorDeCerveja = new Mock<IRepositorioDeCerveja>();
            _armazenadorDeEstoque = new Mock<IRepositorioDeLotes>();
            _armazenadorDeLotes = new ArmazenadorDeLotes(_armazenadorDeCerveja.Object, _armazenadorDeEstoque.Object);
           _lote = "20181218";
        }
        [Theory]
        [InlineData(2, 8, 10)]
        [InlineData(5, 12, 17)]
        public void Deve_adicionar_cerveja_em_um_lote_ja_existente(int quantidadeBase, int quantidadeASerAdicionada, int quantidadeEsperada)
        {
            var cerveja = CervejaBuilder.Novo().Criar();
            var estoqueEsperado = new Lote(cerveja, quantidadeBase);
            _armazenadorDeEstoque.Setup(metodo => metodo.ObterPeloLote(_lote)).Returns(estoqueEsperado);
            _armazenadorDeCerveja.Setup(metodo => metodo.VerificaSeExistePeloNome(cerveja.Nome)).Returns(true);

            _armazenadorDeLotes.AdicionaNoLote(cerveja.Nome, _lote,
                quantidadeASerAdicionada);

            Assert.Equal(quantidadeEsperada, estoqueEsperado.Quantidade);
        }

        [Fact]
        public void Deve_disparar_excecao_quando_cerveja_nao_existir()
        {
            const string mensagemEsperada = "Cerveja não encontrada";
            var cerveja = CervejaBuilder.Novo().Criar();
            const int quantidadeASerAdicionada = 5;
            _armazenadorDeCerveja.Setup(metodo => metodo.VerificaSeExistePeloNome(cerveja.Nome)).Returns(false);

            var erro = Assert.Throws<ArgumentException>(() =>
                _armazenadorDeLotes.AdicionaNoLote(cerveja.Nome, _lote, quantidadeASerAdicionada));

            Assert.Equal(mensagemEsperada, erro.Message);
        }
    }
}

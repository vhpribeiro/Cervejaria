using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Lotes;
using BrejaOnline.Dominio.Test.Builders;
using Moq;
using System;
using Xunit;

namespace BrejaOnline.Dominio.Test
{
    public class ArmazenadorDeLotes
    {
        private readonly Mock<IRepositorioDeCerveja> _repositorioDeCerveja;
        private readonly Mock<IRepositorioDeLotes> _repositorioDeLote;
        private readonly Lotes.ArmazenadorDeLotes _armazenadorDeLotes;
        private readonly string _identificador;

        public ArmazenadorDeLotes()
        {
            _repositorioDeCerveja = new Mock<IRepositorioDeCerveja>();
            _repositorioDeLote = new Mock<IRepositorioDeLotes>();
            _armazenadorDeLotes = new Lotes.ArmazenadorDeLotes(_repositorioDeCerveja.Object, _repositorioDeLote.Object);
           _identificador = "20181218";
        }

        [Theory]
        [InlineData(2, 8, 10)]
        [InlineData(5, 12, 17)]
        public void Deve_adicionar_cerveja_em_um_lote_ja_existente(int quantidadeBase, int quantidadeASerAdicionada, int quantidadeEsperada)
        {
            var cerveja = CervejaBuilder.Novo().Criar();
            var loteEsperado = new Lote(cerveja, quantidadeBase);
            _repositorioDeLote.Setup(metodo => metodo.ObterPeloId(_identificador)).Returns(loteEsperado);
            _repositorioDeCerveja.Setup(metodo => metodo.VerificaSeExistePeloNome(cerveja.Nome)).Returns(true);

            _armazenadorDeLotes.AdicionaNoLote(cerveja.Nome, _identificador,
                quantidadeASerAdicionada);

            Assert.Equal(quantidadeEsperada, loteEsperado.Quantidade);
        }

        [Fact]
        public void Deve_reduzir_quantidade_de_cerveja_em_um_lote_existente()
        {
            const int quantidadeASerReduzida = 5;
            const int quantidadeBase = 17;
            var cerveja = CervejaBuilder.Novo().Criar();
            var lote = new Lote(cerveja, quantidadeBase);
            var quantidadeEsperada = quantidadeBase - quantidadeASerReduzida;
            _repositorioDeLote.Setup(metodo => metodo.ObterPeloId(lote.Identificador)).Returns(lote);

            _armazenadorDeLotes.ReduzirQuantidadeNoLote(_identificador, quantidadeASerReduzida);

            Assert.Equal(quantidadeEsperada, lote.Quantidade);
        }

        [Fact]
        public void Deve_disparar_excecao_quando_cerveja_nao_existir()
        {
            const string mensagemEsperada = "Cerveja não encontrada";
            var cerveja = CervejaBuilder.Novo().Criar();
            const int quantidadeASerAdicionada = 5;
            _repositorioDeCerveja.Setup(metodo => metodo.VerificaSeExistePeloNome(cerveja.Nome)).Returns(false);

            var erro = Assert.Throws<ArgumentException>(() =>
                _armazenadorDeLotes.AdicionaNoLote(cerveja.Nome, _identificador, quantidadeASerAdicionada));

            Assert.Equal(mensagemEsperada, erro.Message);
        }
    }
}

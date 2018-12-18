using System;
using System.Collections.Generic;
using System.Text;
using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Estoque;
using BrejaOnline.Dominio.Test.Builders;
using Moq;
using Xunit;

namespace BrejaOnline.Dominio.Test
{
    public class ArmazenadorDeLancamentosNoEstoqueTest
    {
        [Fact]
        public void Deve_adicionar_cerveja_em_um_lote_ja_existente()
        {
            const int quantidadeASerAdicionada = 5;
            const int quantidadeEsperada = 10;
            var cervejaEsperada = CervejaBuilder.Novo().Criar();
            const string lote = "20181218";
            var estoqueEsperado = new EstoqueDoPub(cervejaEsperada, 5, lote);
            var armazenadorDeCerveja = new Mock<IArmazenadorDeCerveja>();
            var armazenadorDeEstoque = new Mock<IArmazenadorDeEstoque>();
            armazenadorDeEstoque.Setup(m => m.ObterPeloLote(lote)).Returns(estoqueEsperado);
            var armazenadorDeLancamentosNoEstoque = new ArmazenadorDeLancamentosNoEstoque(armazenadorDeCerveja.Object, armazenadorDeEstoque.Object);

            armazenadorDeLancamentosNoEstoque.AdicionaNoEstoque(cervejaEsperada.Nome, lote,
                quantidadeASerAdicionada);

            Assert.Equal(quantidadeEsperada, estoqueEsperado.Quantidade);
        }
    }

    public interface IArmazenadorDeEstoque
    {
        EstoqueDoPub ObterPeloLote(string lote);
    }

    public class ArmazenadorDeLancamentosNoEstoque
    {
        private IArmazenadorDeCerveja _armazenadorDeCerveja;
        private IArmazenadorDeEstoque _armazenadorDeEstoque;

        public ArmazenadorDeLancamentosNoEstoque(IArmazenadorDeCerveja armazenadorDeCerveja, IArmazenadorDeEstoque armazenadorDeEstoque)
        {
            _armazenadorDeCerveja = armazenadorDeCerveja;
            _armazenadorDeEstoque = armazenadorDeEstoque;
        }

        public void AdicionaNoEstoque(string nomeDaCerveja, string lote, int quantidadeASerAdicionada)
        {
            var estoque = _armazenadorDeEstoque.ObterPeloLote(lote);
            estoque.IncrementarQuantidade(quantidadeASerAdicionada);
        }
    }
}

using System;
using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Test.Builders;
using ExpectedObjects;
using Xunit;

namespace BrejaOnline.Dominio.Test
{
    public class EstoqueTest
    {
        private readonly Cerveja _cerveja;

        public EstoqueTest()
        {
            _cerveja = CervejaBuilder.Novo().Criar();
        }
        
        [Fact]
        public void Deve_criar_uma_lote_no_estoque()
        {
            var estoqueEsperado = new
            {
                Cerveja = _cerveja,
                Quantidade = 3,
                Lote = "20181216"
            }.ToExpectedObject();

            var estoqueDesejado = new EstoqueDoPub(_cerveja, 3, "20181216");

            estoqueEsperado.ShouldMatch(estoqueDesejado);
        }

        [Theory]
        [InlineData(-6)]
        [InlineData(-100)]
        public void Nao_deve_aceitar_quantidade_invalida(int quantidadeInvalida)
        {
            const string lote = "20181216";

            Assert.Throws<ArgumentException>(() => new EstoqueDoPub(_cerveja, quantidadeInvalida, lote));
        }

        [Theory]
        [InlineData(50)]
        [InlineData(14)]
        public void Deve_alterar_a_quantidade_do_estoque(int quantidadeASerAdicionada)
        {
            var cerveja = CervejaBuilder.Novo().Criar();
            var estoque = new EstoqueDoPub(cerveja, 5, "20181218");
            int quantidadeEsperada = estoque.Quantidade + quantidadeASerAdicionada;

            estoque.IncrementarQuantidade(quantidadeASerAdicionada);

            Assert.Equal(quantidadeEsperada, estoque.Quantidade);
        }

        [Fact]
        public void Nao_deve_permitir_alterar_quantidade_para_valor_invalido()
        {
            var cerveja = CervejaBuilder.Novo().Criar();
            var estoque = new EstoqueDoPub(cerveja, 6, "20181218");

            Assert.Throws<ArgumentException>(() => estoque.IncrementarQuantidade(-90));
        }

        [Fact]
        public void Nao_deve_permitir_cerveja_invalida()
        {
            Assert.Throws<ArgumentException>(() => new EstoqueDoPub(null, 5, "20181218"));
        }

        [Theory]
        [InlineData("")]
        public void Nao_deve_permitir_lote_invalido(string loteInvalido)
        {
            var cerveja = CervejaBuilder.Novo().Criar();

            Assert.Throws<ArgumentException>(() => new EstoqueDoPub(cerveja, 5, loteInvalido));
        }

    }

    public class EstoqueDoPub
    {
        public int Quantidade { get; protected set; }
        public Cerveja Cerveja { get; protected set; }
        public string Lote { get; protected set; }

        public EstoqueDoPub(Cerveja cerveja, int quantidade, string lote)
        {
            if (quantidade < 0)
            {
                throw new ArgumentException("Quantidade inválida");
            }

            if (cerveja == null)
            {
                throw new ArgumentException("Cerveja não pode ser nula");
            }

            Cerveja = cerveja;
            Quantidade = quantidade;
            Lote = lote;
        }

        public void IncrementarQuantidade(int quantidadeASerAdicionada)
        {
            Quantidade += quantidadeASerAdicionada;
            if (Quantidade <= 0)
            {
                throw new ArgumentException("Quantidade inválida");
            }
        }
    }
}

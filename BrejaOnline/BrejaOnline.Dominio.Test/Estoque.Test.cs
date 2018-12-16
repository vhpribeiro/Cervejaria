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
                Lote = 20181216
            }.ToExpectedObject();

            var estoqueDesejado = new EstoqueDoPub(_cerveja, 3, 20181216);

            estoqueEsperado.ShouldMatch(estoqueDesejado);
        }

        [Theory]
        [InlineData(-6)]
        [InlineData(-100)]
        public void Nao_deve_aceitar_quantidade_invalida(int quantidadeInvalida)
        {
            const int lote = 20181216;

            Assert.Throws<ArgumentException>(() => new EstoqueDoPub(_cerveja, quantidadeInvalida, lote));
        }
    }

    public class EstoqueDoPub
    {
        public int Quantidade { get; protected set; }
        public Cerveja Cerveja { get; protected set; }
        public int Lote { get; protected set; }

        public EstoqueDoPub(Cerveja cerveja, int quantidade, int lote)
        {
            if (quantidade < 0)
            {
                throw new ArgumentException("Quantidade inválida");
            }

            Cerveja = cerveja;
            Quantidade = quantidade;
            Lote = lote;
        }
    }
}

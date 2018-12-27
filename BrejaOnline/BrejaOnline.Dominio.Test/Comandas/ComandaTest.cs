using BrejaOnline.Dominio.Cervejas;
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
            var cerveja = CervejaBuilder.Novo().Criar();

            Assert.Throws<ExcecaoDeDominio>(() => new Comanda(cerveja, quantidadeInvalida));
        }
    }

    public class Comanda
    {
        public Cerveja Cerveja { get; protected set; }
        public int Quantidade { get; protected set; }
        public double ValorTotal { get; protected set; }

        public Comanda(Cerveja cerveja, int quantidade)
        {
            ValidadorDeRegras.Novo()
                .Quando(quantidade < 0, "Quantidade inválida")
                .DispararExcecaoSeExistir();

            Cerveja = cerveja;
            Quantidade = quantidade;
            ValorTotal = cerveja.Preco * quantidade;
        }

    }
}

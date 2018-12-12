using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Estoque;
using BrejaOnline.Dominio.Test.Builders;
using Moq;
using Xunit;

namespace BrejaOnline.Dominio.Test
{
    public class EstoqueTest
    {
        [Fact]
        public void Deve_verificar_se_o_metodo_adicionar_foi_chamado()
        {
            var cerveja = new Cerveja("Skoll", 5.50, "Cerveja muito boa",
                "Mercearia", TipoDeCerveja.LAGER);
            var cervejaEstoqueMock = new Mock<IEstoqueRepositorio>();
            var estoqueDeCerveja = new Estoque.Estoque(cervejaEstoqueMock.Object);

            estoqueDeCerveja.Armazenar(cerveja);

            cervejaEstoqueMock.Verify(estoque => estoque.Adiciona(It.IsAny<Cerveja>()));
        }

        [Fact]
        public void Não_deve_adicionar_caso_ja_exista_tal_cerveja_no_estoque()
        {
            var cervejaEsperada = CervejaBuilder.Novo().ComNome("Teste").Criar();
            var cervejaEstoqueMock = new Mock<IEstoqueRepositorio>();
            cervejaEstoqueMock.Setup(estoque => estoque.VerificaSeExistePeloNome(cervejaEsperada.Nome)).Returns(true);
            var estoqueDeCerveja = new Estoque.Estoque(cervejaEstoqueMock.Object);

            estoqueDeCerveja.Armazenar(cervejaEsperada);

            cervejaEstoqueMock.Verify(estoque =>
                estoque.Adiciona(It.Is<Cerveja>(cerveja => cerveja.Nome == cervejaEsperada.Nome)),Times.Never());
        }
    }
}

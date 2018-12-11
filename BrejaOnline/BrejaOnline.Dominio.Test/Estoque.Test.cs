using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Estoque;
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
    }
}

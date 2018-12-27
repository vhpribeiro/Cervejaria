using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Test.Builders;
using Moq;
using Xunit;

namespace BrejaOnline.Dominio.Test.Cervejas
{
    public class ArmazenadorDeCervejaTest
    {
        [Fact]
        public void Deve_verificar_se_o_metodo_adicionar_foi_chamado()
        {
            var cerveja = new Cerveja("Skoll", 5.50, "Cerveja muito boa", TipoDeCerveja.LAGER);
            var repositorio = new Mock<IRepositorioDeCerveja>();
            var estoqueDeCerveja = new ArmazenadorDeCerveja(repositorio.Object);

            estoqueDeCerveja.Armazenar(cerveja);

            repositorio.Verify(estoque => estoque.Adiciona(cerveja));
        }

        [Fact]
        public void Não_deve_adicionar_caso_ja_exista_tal_cerveja_no_estoque()
        {
            var cervejaEsperada = CervejaBuilder.Novo().ComNome("Teste").Criar();
            var cervejaEstoqueMock = new Mock<IRepositorioDeCerveja>();
            cervejaEstoqueMock.Setup(estoque => estoque.VerificaSeExistePeloNome(cervejaEsperada.Nome)).Returns(true);
            var estoqueDeCerveja = new ArmazenadorDeCerveja(cervejaEstoqueMock.Object);

            estoqueDeCerveja.Armazenar(cervejaEsperada);

            cervejaEstoqueMock.Verify(estoque =>
                estoque.Adiciona(It.IsAny<Cerveja>()),Times.Never());
        }
    }
}

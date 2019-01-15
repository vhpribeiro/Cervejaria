using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Test._Builders;
using Moq;
using Xunit;

namespace BrejaOnline.Dominio.Test.Cervejas
{
    public class ArmazenadorDeCervejaTest
    {
        private readonly Mock<IRepositorioDeCerveja> _repositorioDeCerveja;
        private readonly ArmazenadorDeCerveja _armazenadorDeCerveja;

        public ArmazenadorDeCervejaTest()
        {
            _repositorioDeCerveja = new Mock<IRepositorioDeCerveja>();
            _armazenadorDeCerveja = new ArmazenadorDeCerveja(_repositorioDeCerveja.Object);
        }
        [Fact]
        public void Deve_verificar_se_o_metodo_adicionar_foi_chamado()
        {
            var cerveja = new Cerveja("Skoll", 5.50, "Cerveja muito boa", TipoDeCerveja.LAGER);

            _armazenadorDeCerveja.Armazenar(cerveja);

            _repositorioDeCerveja.Verify(estoque => estoque.Adiciona(cerveja));
        }

        [Fact]
        public void Não_deve_adicionar_caso_ja_exista_tal_cerveja_no_estoque()
        {
            var cervejaEsperada = CervejaBuilder.Novo().ComNome("Teste").Criar();
            _repositorioDeCerveja.Setup(estoque => estoque.VerificaSeExistePeloNome(cervejaEsperada.Nome)).Returns(true);

            _armazenadorDeCerveja.Armazenar(cervejaEsperada);

            _repositorioDeCerveja.Verify(estoque =>
                estoque.Adiciona(It.IsAny<Cerveja>()),Times.Never());
        }
    }
}

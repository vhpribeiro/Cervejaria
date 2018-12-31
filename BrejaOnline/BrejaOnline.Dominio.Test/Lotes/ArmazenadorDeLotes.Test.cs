using System;
using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Lotes;
using BrejaOnline.Dominio.Test.Builders;
using Moq;
using Xunit;

namespace BrejaOnline.Dominio.Test.Lotes
{
    public class ArmazenadorDeLotesTeste
    {
        private readonly Mock<IRepositorioDeCerveja> _repositorioDeCerveja;
        private readonly Mock<IRepositorioDeLotes> _repositorioDeLotes;
        private readonly ArmazenadorDeLotes _armazenadorDeLotes;
        private readonly string _identificador;

        public ArmazenadorDeLotesTeste()
        {
            _repositorioDeCerveja = new Mock<IRepositorioDeCerveja>();
            _repositorioDeLotes = new Mock<IRepositorioDeLotes>();
            _armazenadorDeLotes = new ArmazenadorDeLotes(_repositorioDeCerveja.Object, _repositorioDeLotes.Object);
           _identificador = DateTime.Now.ToString("yyyyMMdd"); 
        }

        [Fact]
        public void Deve_armazenar_um_lote_no_repositorio()
        {
            var cerveja = CervejaBuilder.Novo().Criar();
            var loteEsperado = new Lote(cerveja, 8);

            _armazenadorDeLotes.Armazenar(loteEsperado);

            _repositorioDeLotes.Verify(estoque => estoque.Adiciona(
                It.Is<Lote>(lote => lote.Identificador == loteEsperado.Identificador)));
        }
    }
}

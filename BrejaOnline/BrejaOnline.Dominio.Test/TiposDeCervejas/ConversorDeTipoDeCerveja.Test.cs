using System;
using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Test._Util;
using BrejaOnline.Dominio.TiposDeCervejas;
using BrejaOnline.Dominio._Base;
using Xunit;

namespace BrejaOnline.Dominio.Test.TiposDeCervejas
{
    public class ConversorDeTipoDeCervejaTeste
    {
        private readonly ConversorDeTipoDeCerveja _conversorDeTipoDeCerveja;

        public ConversorDeTipoDeCervejaTeste()
        {
            _conversorDeTipoDeCerveja = new ConversorDeTipoDeCerveja();
        }
        [Fact]
        public void Deve_converter_string_para_um_tipo_de_cerveja()
        {
            const TipoDeCerveja tipoDeCervejaEsperado = TipoDeCerveja.PILSEN;
            const string tipoDeCerveja = "PILSEN";

            var tipoDeCervejaObtido = _conversorDeTipoDeCerveja.Converter(tipoDeCerveja);

            Assert.Equal(tipoDeCervejaEsperado, tipoDeCervejaObtido);
        }

        [Fact]
        public void Nao_deve_converter_tipo_de_cerveja_invalido()
        {
            var tipoDeCerveja = "BUDWEISER";

            Action acao = () => _conversorDeTipoDeCerveja.Converter(tipoDeCerveja);

            Assert.Throws<ExcecaoDeDominio>(acao).ComMensagem(Resource.TipoDeCervejaInválida);
        }
    }
}

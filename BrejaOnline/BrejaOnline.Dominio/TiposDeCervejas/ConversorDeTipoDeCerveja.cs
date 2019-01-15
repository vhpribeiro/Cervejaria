using System;
using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio._Base;

namespace BrejaOnline.Dominio.TiposDeCervejas
{
    public class ConversorDeTipoDeCerveja : IConversorDeTipoDeCerveja
    {
        public TipoDeCerveja Converter(string tipoDeCerveja)
        {
            ValidadorDeRegras.Novo()
                .Quando(!Enum.TryParse<TipoDeCerveja>(tipoDeCerveja, out var tipoDeCervejaConvertido),
                    Resource.TipoDeCervejaInválida).DispararExcecaoSeExistir();

            return tipoDeCervejaConvertido;
        }
    }
}
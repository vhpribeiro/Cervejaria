using BrejaOnline.Dominio.Cervejas;

namespace BrejaOnline.Dominio.TiposDeCervejas
{
    public interface IConversorDeTipoDeCerveja
    {
        TipoDeCerveja Converter(string tipoDeCerveja);
    }
}
using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Comandas;

namespace BrejaOnline.Dominio.Test._Builders
{
    public class ComandaBuilder
    {
        private Cerveja _cerveja = CervejaBuilder.Novo().Criar();
        private int _quantidade = 10;

        public static ComandaBuilder Novo()
        {
            return new ComandaBuilder();
        }

        public ComandaBuilder ComCerveja(Cerveja cerveja)
        {
            _cerveja = cerveja;
            return this;
        }

        public ComandaBuilder ComQuantidade(int quantidade)
        {
            _quantidade = quantidade;
            return this;
        }

        public Comanda Criar()
        {
            return new Comanda(_cerveja, _quantidade);
        }
    }
}

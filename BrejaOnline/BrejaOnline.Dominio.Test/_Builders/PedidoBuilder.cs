using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Comandas;

namespace BrejaOnline.Dominio.Test._Builders
{
    public class PedidoBuilder
    {
        private Cerveja _cerveja = CervejaBuilder.Novo().Criar();
        private int _quantidade = 10;

        public static PedidoBuilder Novo()
        {
            return new PedidoBuilder();
        }

        public PedidoBuilder ComCerveja(Cerveja cerveja)
        {
            _cerveja = cerveja;
            return this;
        }

        public PedidoBuilder ComQuantidade(int quantidade)
        {
            _quantidade = quantidade;
            return this;
        }

        public Pedido Criar()
        {
            return new Pedido(_cerveja, _quantidade);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using BrejaOnline.Dominio.Comandas;

namespace BrejaOnline.Dominio.Test._Builders
{
    public class ComandaBuilder
    {
        private List<Pedido> _pedidos;

        public static ComandaBuilder Novo()
        {
            return new ComandaBuilder
            {
                _pedidos = new List<Pedido>()
            };
        }

        public ComandaBuilder ComPedidos(List<Pedido> pedidos)
        {
            _pedidos = pedidos;
            return this;
        }

        public void ComNovoPedido(Pedido pedido)
        {
            _pedidos.Add(pedido);
        }

        public Comanda Criar()
        {
            return new Comanda(_pedidos);
        }
    }
}

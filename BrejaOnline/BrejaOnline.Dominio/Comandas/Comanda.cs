using System.Collections.Generic;
using BrejaOnline.Dominio._Base;

namespace BrejaOnline.Dominio.Comandas
{
    public class Comanda
    {
        public List<Pedido> Pedidos { get; private set; }
        public double ValorTotal { get; private set; }

        public Comanda(List<Pedido> pedidos)
        {
            //TODO como fazer o InlineData receber uma lista vazia?
            ValidadorDeRegras.Novo()
                .Quando(pedidos.Count == 0, Resource.ComandaSemPedidos)
                .DispararExcecaoSeExistir();

            Pedidos = pedidos;
            ValorTotal = 0;

            pedidos.ForEach(pedido =>
                ValorTotal += pedido.Valor);
        }
    }
}
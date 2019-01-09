using System.Collections.Generic;
using BrejaOnline.Dominio.Comandas;
using BrejaOnline.Dominio.Test._Builders;
using BrejaOnline.Dominio.Test._Util;
using BrejaOnline.Dominio._Base;
using ExpectedObjects;
using Xunit;

namespace BrejaOnline.Dominio.Test.Comandas
{
    public class ComandaTest
    {
        [Fact]
        public void Deve_criar_uma_comanda()
        {
            var cervejaDoPrimeiroPedido = CervejaBuilder.Novo().Criar();
            var cervejaDoSegundoPedido = CervejaBuilder.Novo().ComNome("Budweiser").ComPreco(6.75).Criar();
            var primeiroPedido = new Pedido(cervejaDoPrimeiroPedido, 5);
            var segundoPedido = new Pedido(cervejaDoSegundoPedido, 4);
            var valorTotal = primeiroPedido.Valor + segundoPedido.Valor;
            var pedidos = new List<Pedido>
            {
                primeiroPedido,
                segundoPedido
            };
            var comandaEsperada = new
            {
                Pedidos = pedidos,
                ValorTotal = valorTotal
            };

            var comandaObtida = new Comanda(pedidos);

            comandaEsperada.ToExpectedObject().ShouldMatch(comandaObtida);
        }

        [Fact]
        public void Nao_deve_criar_uma_comanda_sem_pedidos()
        {
            var pedidosInvalidos = new List<Pedido>();

            Assert.Throws<ExcecaoDeDominio>(() => new Comanda(pedidosInvalidos)).ComMensagem(Resource.ComandaSemPedidos);
        }
    }
}

using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio._Base;

namespace BrejaOnline.Dominio.Comandas
{
    public class Comanda
    {
        public Cerveja Cerveja { get; protected set; }
        public int Quantidade { get; protected set; }
        public double ValorTotal { get; protected set; }

        public Comanda(Cerveja cerveja, int quantidade)
        {
            ValidadorDeRegras.Novo()
                .Quando(quantidade < 0, Resource.QuantidadeInvalida)
                .DispararExcecaoSeExistir();

            Cerveja = cerveja;
            Quantidade = quantidade;
            ValorTotal = cerveja.Preco * quantidade;
        }

        public void AlterarQuantidade(int quantidadeEsperada)
        {
            ValidadorDeRegras.Novo()
                .Quando(quantidadeEsperada < 0, Resource.QuantidadeInvalida)
                .DispararExcecaoSeExistir();

            Quantidade = quantidadeEsperada;
        }
    }
}
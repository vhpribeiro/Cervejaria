using BrejaOnline.Dominio._Base;

namespace BrejaOnline.Dominio.Cervejas
{
    public class Cerveja
    {
        public string Nome { get; private set; }
        public double Preco { get; private set; }
        public string Descricao { get; private set; }
        public TipoDeCerveja Tipo { get; private set; }

        public Cerveja(string nome, double preco, string descricao, TipoDeCerveja tipo)
        {
            ValidadorDeRegras.Novo()
                .Quando(preco <= 0, Resource.PrecoInvalido)
                .Quando(string.IsNullOrEmpty(nome), Resource.NomeInvalido)
                .DispararExcecaoSeExistir();

            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Tipo = tipo;
        }

        public void AlterarNome(string nome)
        {
            ValidadorDeRegras.Novo()
                .Quando(string.IsNullOrEmpty(nome), Resource.NomeInvalido)
                .DispararExcecaoSeExistir();
            Nome = nome;
        }
    }
}
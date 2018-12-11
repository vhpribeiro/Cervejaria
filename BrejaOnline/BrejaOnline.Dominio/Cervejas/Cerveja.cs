using System;

namespace BrejaOnline.Dominio.Cervejas
{
    public class Cerveja
    {
        public string Nome { get; private set; }
        public double Preco { get; private set; }
        public string Descricao { get; private set; }
        public string Cervejaria { get; private set; }
        public TipoDeCerveja Tipo { get; private set; }

        public Cerveja(string nome, double preco, string descricao, string cervejaria, TipoDeCerveja tipo)
        {
            Nome = nome;
            Preco = preco;
            Descricao = descricao;
            Cervejaria = cervejaria;
            Tipo = tipo;

            if (Preco <= 0)
            {
                throw new Exception();
            }

        }
    }
}
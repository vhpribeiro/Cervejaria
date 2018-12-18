using System;
using BrejaOnline.Dominio.Cervejas;

namespace BrejaOnline.Dominio.Lotes
{
    public class Lote
    {
        public int Quantidade { get; protected set; }
        public Cerveja Cerveja { get; protected set; }
        public string Identificador { get; protected set; }

        public Lote(Cerveja cerveja, int quantidade)
        {
            if (quantidade < 0)
            {
                throw new ArgumentException("Quantidade inválida");
            }

            if (cerveja == null)
            {
                throw new ArgumentException("Cerveja não pode ser nula");
            }

            Cerveja = cerveja;
            Quantidade = quantidade;
            Identificador = DateTime.Now.ToString("yyyyMMdd");
        }

        public void IncrementarQuantidade(int quantidadeASerAdicionada)
        {
            Quantidade += quantidadeASerAdicionada;
            if (Quantidade <= 0)
            {
                throw new ArgumentException("Quantidade inválida");
            }
        }
    }
}
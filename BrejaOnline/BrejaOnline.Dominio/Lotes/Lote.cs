using System;
using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio._Base;

namespace BrejaOnline.Dominio.Lotes
{
    public class Lote
    {
        public int Quantidade { get; protected set; }
        public Cerveja Cerveja { get; protected set; }
        public string Identificador { get; protected set; }

        public Lote(Cerveja cerveja, int quantidade)
        {
            ValidadorDeRegras.Novo()
                .Quando(quantidade < 0, Resource.QuantidadeInvalida)
                .Quando(cerveja == null, Resource.CervejaInvalida)
                .DispararExcecaoSeExistir();

            Cerveja = cerveja;
            Quantidade = quantidade;
            Identificador = DateTime.Now.ToString("yyyyMMdd");
        }

        public void IncrementarQuantidade(int quantidadeASerAdicionada)
        {
            ValidadorDeRegras.Novo()
                .Quando(Quantidade > Quantidade + quantidadeASerAdicionada, Resource.QuantidadeInvalida)
                .DispararExcecaoSeExistir();

            Quantidade += quantidadeASerAdicionada;
        }

        public void DecrementarQuantidade(int quantidadeASerReduzida)
        {
            ValidadorDeRegras.Novo()
                .Quando(Quantidade < Quantidade - quantidadeASerReduzida, "Quantidade adicionada inválida")
                .Quando(Quantidade - quantidadeASerReduzida < 0, "Novo valor de quantidade é inferior a zero")
                .DispararExcecaoSeExistir();

            Quantidade -= quantidadeASerReduzida;
        }
    }
}
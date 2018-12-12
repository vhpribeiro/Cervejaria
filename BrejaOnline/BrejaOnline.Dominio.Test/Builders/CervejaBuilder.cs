using System;
using System.Collections.Generic;
using System.Text;
using BrejaOnline.Dominio.Cervejas;

namespace BrejaOnline.Dominio.Test.Builders
{
    class CervejaBuilder
    {
        private string _nome = "Heineken";
        private double _preco = 11.5;
        private string _descricao = "Cerveja Alemã";
        private string _cervejaria = "Bar da esquina";
        private TipoDeCerveja _tipoDeCerveja = TipoDeCerveja.INDIAN_PALE_ALE;

        public static CervejaBuilder Novo()
        {
            return new CervejaBuilder();
        }

        public CervejaBuilder ComNome(string nome)
        {
            _nome = nome;
            return this;
        }

        public CervejaBuilder ComPreco(double preco)
        {
            _preco = preco;
            return this;
        }

        public CervejaBuilder ComDescricao(string descricao)
        {
            _descricao = descricao;
            return this;
        }

        public CervejaBuilder ComCervejaria(string cervejaria)
        {
            _cervejaria = cervejaria;
            return this;
        }

        public Cerveja Criar()
        {
            return new Cerveja(_nome, _preco, _descricao, _cervejaria, _tipoDeCerveja);
        }
    }
}

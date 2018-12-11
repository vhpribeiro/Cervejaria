using System;
using BrejaOnline.Dominio.Cervejas;
using ExpectedObjects;
using Xunit;

namespace BrejaOnline.Dominio.Test
{
    public class CervejaTest
    {
        public string _nome;
        public double _preco;
        public string _descricao;
        public string _cervejaria;
        public TipoDeCerveja _tipo;


        public CervejaTest()
        {
            _nome = "Skoll";
            _preco = 7.50;
            _descricao = "Cerveja barata";
            _cervejaria = "Pub Irlandês";
            _tipo = TipoDeCerveja.LAGER;
        }

        [Fact]
        public void Deve_criar_uma_cerveja()
        {
            var cervejaDesejada = new
            {
                Nome = _nome,
                Preco = _preco,
                Descricao = _descricao,
                Cervejaria = _cervejaria,
                Tipo = _tipo
            };

            var cervejaCriada = new Cerveja(cervejaDesejada.Nome, cervejaDesejada.Preco, cervejaDesejada.Descricao,
                cervejaDesejada.Cervejaria, cervejaDesejada.Tipo);

            cervejaDesejada.ToExpectedObject().ShouldMatch(cervejaCriada);
        }

        [Fact]
        public void Nao_deve_aceitar_preco_invalido()
        {
            const double precoInvalido = -7.5;
            var cervejaDesejada = new
            {
                Nome = _nome,
                Preco = _preco,
                Descricao = _descricao,
                Cervejaria = _cervejaria,
                Tipo = _tipo
            };

            Assert.Throws<Exception>(() => new Cerveja(cervejaDesejada.Nome, precoInvalido, cervejaDesejada.Descricao,
                cervejaDesejada.Cervejaria, cervejaDesejada.Tipo));
        }
    }
}

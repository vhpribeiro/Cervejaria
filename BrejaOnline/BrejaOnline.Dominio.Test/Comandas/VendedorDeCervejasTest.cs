using System.Collections.Generic;
using System.Linq;
using BrejaOnline.Dominio.Cervejas;
using BrejaOnline.Dominio.Lotes;
using BrejaOnline.Dominio.Test.Builders;
using BrejaOnline.Dominio._Base;
using Moq;
using Xunit;

namespace BrejaOnline.Dominio.Test.Comandas
{
    public class VendedorDeCervejasTest
    {
        private readonly Mock<IRepositorioDeLotes> _repositorioDeLotes;
        private readonly VendedorDeCervejas _vendedorDeCervejas;

        public VendedorDeCervejasTest()
        {
            _repositorioDeLotes = new Mock<IRepositorioDeLotes>();
            _vendedorDeCervejas = new VendedorDeCervejas(_repositorioDeLotes.Object);
        }
        [Fact]
        public void Deve_realizar_uma_busca_nos_lotes_pelo_nome_da_cerveja()
        {
            var cervejaVendida = CervejaBuilder.Novo().Criar();
            var comanda = new Comanda(cervejaVendida, 6);
            var listaDeLotesObtida = new List<Lote>()
            {
                new Lote(cervejaVendida, 7),
                new Lote(cervejaVendida, 3)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterPeloNomeDaCerveja(cervejaVendida.Nome))
                .Returns(listaDeLotesObtida);

            _vendedorDeCervejas.ValidarVenda(comanda);

            _repositorioDeLotes.Verify(repositorio =>
                repositorio.ObterPeloNomeDaCerveja(It.Is<string>(nomeDaCerveja => nomeDaCerveja == cervejaVendida.Nome)));
        }

        [Fact]
        public void Nao_deve_realizar_uma_venda_caso_nao_tenha_quantidade_necessaria()
        {
            var cervejaVendida = CervejaBuilder.Novo().Criar();
            var comanda = new Comanda(cervejaVendida, 6);
            var listaDeLotesObtida = new List<Lote>()
            {
                new Lote(cervejaVendida, 2),
                new Lote(cervejaVendida, 3)
            };

            _repositorioDeLotes.Setup(repositorio => repositorio.ObterPeloNomeDaCerveja(cervejaVendida.Nome))
                .Returns(listaDeLotesObtida);

            Assert.Throws<ExcecaoDeDominio>(() => _vendedorDeCervejas.ValidarVenda(comanda));
        }

        [Fact]
        public void Deve_diminuir_quantidade_do_lote_caso_a_da_comanda_for_menor()
        {
            int quantidadeEsperada = 1;
            var cerveja = CervejaBuilder.Novo().Criar();
            var listaDeLotes = new List<Lote>
            {
                new Lote(cerveja, 8)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterPeloNomeDaCerveja(cerveja.Nome))
                .Returns(listaDeLotes);
            var comanda = new Comanda(cerveja, 7);

            _vendedorDeCervejas.Venda(comanda);

            Assert.Equal(quantidadeEsperada, listaDeLotes.First().Quantidade);
        }

        [Fact]
        public void Deve_excluir_lote_caso_a_quantidade_da_comanda_for_maior()
        {
            var cerveja = CervejaBuilder.Novo().Criar();
            var listaDeLotes = new List<Lote>
            {
                new Lote(cerveja, 5),
                new Lote(cerveja, 3),
                new Lote(cerveja, 4)
            };
            _repositorioDeLotes.Setup(repositorio => repositorio.ObterPeloNomeDaCerveja(cerveja.Nome))
                .Returns(listaDeLotes);
            var comanda = new Comanda(cerveja, 10);

            _vendedorDeCervejas.Venda(comanda);

            _repositorioDeLotes.Verify(repositorio => repositorio.Excluir(It.IsAny<Lote>()), Times.AtLeast(2));
        }
    }

    public class VendedorDeCervejas
    {
        private readonly IRepositorioDeLotes _repositorioDeLotes;

        public VendedorDeCervejas(IRepositorioDeLotes repositorioDeLotes)
        {
            _repositorioDeLotes = repositorioDeLotes;
        }

        public void Venda(Comanda comanda)
        {
            ValidarVenda(comanda);
            var listaDeLotesComACervejaDesejada = _repositorioDeLotes.ObterPeloNomeDaCerveja(comanda.Cerveja.Nome);

            //TODO - Criar um método para diminuir a quantidade
            //TODO - Refatorar esse cara para usar ForEach
            //TODO - Arrumar o ID do lote e colocar até segundos
            var cervejasRestantes = comanda.Quantidade;
            for (var i = 0; i < listaDeLotesComACervejaDesejada.Count; i++)
            {
                if (listaDeLotesComACervejaDesejada[i].Quantidade > cervejasRestantes)
                {
                    listaDeLotesComACervejaDesejada[i].DecrementarQuantidade(cervejasRestantes);
                    break;
                }
                else
                {
                    _repositorioDeLotes.Excluir(listaDeLotesComACervejaDesejada[i]);
                    cervejasRestantes = cervejasRestantes - listaDeLotesComACervejaDesejada[i].Quantidade;
                }
            }

        }

        public void ValidarVenda(Comanda comanda)
        {
            var listaDeLotesComACervejaDesejada =_repositorioDeLotes.ObterPeloNomeDaCerveja(comanda.Cerveja.Nome);

            var quantidadeTotalQueSeTemNosLotes = 0;
            listaDeLotesComACervejaDesejada.ForEach(lote => 
                quantidadeTotalQueSeTemNosLotes += lote.Quantidade);

            ValidadorDeRegras.Novo()
                .Quando(quantidadeTotalQueSeTemNosLotes < comanda.Quantidade, "Quantidade indisponível")
                .DispararExcecaoSeExistir();
        }
    }
}

using BrejaOnline.Dominio.Cervejas;

namespace BrejaOnline.Dominio.Estoque
{
    public class Estoque
    {
        private readonly IEstoqueRepositorio _estoqueRepositorio;

        public Estoque(IEstoqueRepositorio estoqueRepositorio)
        {
            _estoqueRepositorio = estoqueRepositorio;
        }

        public void Armazenar(Cerveja cerveja)
        {
            var cervejaJaExistente = _estoqueRepositorio.VerificaSeExistePeloNome(cerveja.Nome);
            if (cervejaJaExistente == false)
            {
                _estoqueRepositorio.Adiciona(cerveja);
            }
        }
    }
}
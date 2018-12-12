using BrejaOnline.Dominio.Cervejas;

namespace BrejaOnline.Dominio.Estoque
{
    public interface IEstoqueRepositorio
    {
        void Adiciona(Cerveja cerveja);
        bool VerificaSeExistePeloNome(string nome);
    }
}
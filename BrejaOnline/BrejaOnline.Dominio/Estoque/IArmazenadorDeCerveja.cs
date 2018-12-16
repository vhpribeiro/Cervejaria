using BrejaOnline.Dominio.Cervejas;

namespace BrejaOnline.Dominio.Estoque
{
    public interface IArmazenadorDeCerveja
    {
        void Adiciona(Cerveja cerveja);
        bool VerificaSeExistePeloNome(string nome);
    }
}
using BrejaOnline.Dominio._Base;

namespace BrejaOnline.Dominio.Cervejas
{
    public interface IRepositorioDeCerveja : IRepositorio<Cerveja>
    {
        void Adiciona(Cerveja cerveja);
        bool VerificaSeExistePeloNome(string nome);
    }
}
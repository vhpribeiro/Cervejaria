namespace BrejaOnline.Dominio.Cervejas
{
    public interface IRepositorioDeCerveja
    {
        void Adiciona(Cerveja cerveja);
        bool VerificaSeExistePeloNome(string nome);
    }
}
namespace BrejaOnline.Dominio.Cervejas
{
    public class ArmazenadorDeCerveja
    {
        private readonly IRepositorioDeCerveja _repositorioDeCerveja;

        public ArmazenadorDeCerveja(IRepositorioDeCerveja repositorioDeCerveja)
        {
            _repositorioDeCerveja = repositorioDeCerveja;
        }

        public void Armazenar(Cerveja cerveja)
        {
            var cervejaJaExistente = _repositorioDeCerveja.VerificaSeExistePeloNome(cerveja.Nome);
            if (cervejaJaExistente == false)
            {
                _repositorioDeCerveja.Adiciona(cerveja);
            }
        }
    }
}
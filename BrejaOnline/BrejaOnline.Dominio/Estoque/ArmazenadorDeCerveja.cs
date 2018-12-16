using BrejaOnline.Dominio.Cervejas;

namespace BrejaOnline.Dominio.Estoque
{
    public class ArmazenadorDeCerveja
    {
        private readonly IArmazenadorDeCerveja _armazenadorDeCerveja;

        public ArmazenadorDeCerveja(IArmazenadorDeCerveja armazenadorDeCerveja)
        {
            _armazenadorDeCerveja = armazenadorDeCerveja;
        }

        public void Armazenar(Cerveja cerveja)
        {
            var cervejaJaExistente = _armazenadorDeCerveja.VerificaSeExistePeloNome(cerveja.Nome);
            if (cervejaJaExistente == false)
            {
                _armazenadorDeCerveja.Adiciona(cerveja);
            }
        }
    }
}
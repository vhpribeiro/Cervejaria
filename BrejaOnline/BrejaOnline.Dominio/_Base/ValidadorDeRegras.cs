using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrejaOnline.Dominio._Base
{
    class ValidadorDeRegras
    {
        private readonly List<string> _mensagensDeErro;

        private ValidadorDeRegras()
        {
            _mensagensDeErro = new List<string>();
        }

        public static ValidadorDeRegras Novo()
        {
            return new ValidadorDeRegras();
        }

        public ValidadorDeRegras Quando(bool condicao, string mensagem)
        {
            if (condicao)
            {
                _mensagensDeErro.Add(mensagem);
            }

            return this;
        }

        public void DispararExcecaoSeExistir()
        {
            if (_mensagensDeErro.Any())
            {
                throw new ExcecaoDeDominio(_mensagensDeErro);
            }
        }

    }
}

using System;
using System.Collections.Generic;

namespace BrejaOnline.Dominio._Base
{
    public class ExcecaoDeDominio : ArgumentException
    {
        public List<string> MensagensDeErro { get; set; }

        public ExcecaoDeDominio(List<string> mensagensDeErro)
        {
            MensagensDeErro = mensagensDeErro;
        }
    }
}
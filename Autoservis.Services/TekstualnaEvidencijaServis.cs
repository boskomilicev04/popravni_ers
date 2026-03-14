using Autoservis.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Autoservis.Services
{
    public class TekstualnaEvidencijaServis : IEvidencijaServis
    {
        private readonly string _putanja = "log.txt";

        public void Zabelezi(string poruka, TipEvidencije tip)
        {
            string zapis = $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] [{tip}] - {poruka}";

            File.AppendAllLines(_putanja, new[] { zapis });
        }
    }
}

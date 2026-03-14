using System;
using System.Collections.Generic;
using System.Text;

namespace Autoservis.Domain
{
    public class Korisnik
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string KorisnickoIme { get; set; } = string.Empty;
        public string Lozinka { get; set; } = string.Empty;
        public string ImePrezime { get; set; } = string.Empty;
        public Uloga Uloga { get; set; }
    }
}

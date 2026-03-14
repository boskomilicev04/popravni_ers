using System;
using System.Collections.Generic;
using System.Text;

namespace Autoservis.Domain
{
    public class Racun
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ImeMehanicara { get; set; } = string.Empty;
        public string Registracija { get; set; } = string.Empty;
        public DateTime DatumVremeIzdavanja { get; set; } = DateTime.Now;
        public decimal UkupanIznos { get; set; }
    }
}

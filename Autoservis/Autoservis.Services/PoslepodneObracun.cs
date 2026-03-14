using System;
using System.Collections.Generic;
using System.Text;

namespace Autoservis.Services
{
    public class PoslepodneObracun : IObracunServis
    {
        public decimal IzracunajCenu(decimal osnovnaCena) => osnovnaCena * 1.10m;
    }
}

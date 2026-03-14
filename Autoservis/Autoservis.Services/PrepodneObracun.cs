using System;
using System.Collections.Generic;
using System.Text;

namespace Autoservis.Services
{
    public class PrepodneObracun : IObracunServis
    {
        public decimal IzracunajCenu(decimal osnovnaCena) => osnovnaCena * 0.85m;
    }
}

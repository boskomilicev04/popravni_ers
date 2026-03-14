using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Autoservis.Domain
{
    public class Vozilo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RegistarskiBroj { get; set; } = string.Empty;
        public string MarkaIModel { get; set; } = string.Empty;
        public TipVozila Tip { get; set; }
        public decimal ProcenjenaCena { get; set; }
        public bool Servisirano { get; set; } = false;
    }
}

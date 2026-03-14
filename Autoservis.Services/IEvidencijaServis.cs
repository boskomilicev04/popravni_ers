using Autoservis.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Autoservis.Services
{
    public interface IEvidencijaServis
    {
        void Zabelezi(string poruka, TipEvidencije tip);
    }
}

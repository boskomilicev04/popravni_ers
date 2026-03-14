using System;
using System.Collections.Generic;
using System.Text;
using Autoservis.Domain;
using System.Collections.Generic;

namespace Autoservis.Infrastructure.Repositories
{
    public interface IVoziloRepository
    {
        List<Vozilo> GetAll();
        void SaveAll(List<Vozilo> vozila);
    }
}

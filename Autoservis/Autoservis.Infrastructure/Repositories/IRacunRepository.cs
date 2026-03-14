using System;
using System.Collections.Generic;
using System.Text;
using Autoservis.Domain;

namespace Autoservis.Infrastructure.Repositories
{
    public interface IRacunRepository
    {
        List<Racun> GetAll();
        void SaveAll(List<Racun> racuni);
    }
}

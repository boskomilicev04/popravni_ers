using System;
using System.Collections.Generic;
using System.Text;
using Autoservis.Domain;
using System.Collections.Generic;

namespace Autoservis.Infrastructure.Repositories
{
    public interface IKorisnikRepository
    {
        List<Korisnik> GetAll();
        void SaveAll(List<Korisnik> korisnici);
    }
}

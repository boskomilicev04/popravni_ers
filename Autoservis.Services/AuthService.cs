using System;
using System.Collections.Generic;
using System.Text;
using Autoservis.Domain;
using Autoservis.Infrastructure.Repositories;

namespace Autoservis.Services
{
    public class AuthService
    {
        private readonly KorisnikRepository _korisnikRepo = new();
        private readonly IEvidencijaServis _logger;

        public AuthService(IEvidencijaServis logger)
        {
            _logger = logger;
        }

        public Korisnik? Login(string username, string password)
        {
            var sviKorisnici = _korisnikRepo.GetAll();
            var korisnik = sviKorisnici.FirstOrDefault(k => k.KorisnickoIme == username && k.Lozinka == password);

            if (korisnik != null)
            {
                _logger.Zabelezi($"Uspesna prijava korisnika: {username} (Uloga: {korisnik.Uloga}", TipEvidencije.INFO);
            }
            else
            {
                _logger.Zabelezi($"Neuspesan pokusaj prijave za korisnicko ime: {username}", TipEvidencije.WARNING);
            }

            return korisnik;
        }

        public void KreirajInicijalnogAdmina()  
        {
            var svi = _korisnikRepo.GetAll();

            if(svi.Count == 0)
            {
                svi.Add(new Korisnik
                {
                    Id = Guid.NewGuid(),
                    KorisnickoIme = "admin",
                    Lozinka = "admin123",
                    ImePrezime = "Glavni Menadzer",
                    Uloga = Uloga.Menadzer
                });

                svi.Add(new Korisnik
                {
                    Id = Guid.NewGuid(),
                    KorisnickoIme = "ivan",
                    Lozinka = "ivan123",
                    ImePrezime = "Ivan Ivanovic",
                    Uloga = Uloga.Mehanicar
                });

                _korisnikRepo.SaveAll(svi);
                _logger.Zabelezi("Kreirani inicijalni korisnicki nalozi u sistemu.", TipEvidencije.INFO);
            }
        }
    }
}

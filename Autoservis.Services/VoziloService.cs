using System;
using System.Collections.Generic;
using System.Text;
using Autoservis.Domain;
using Autoservis.Infrastructure.Repositories;

namespace Autoservis.Services
{
    public class VoziloService
    {
        private readonly VoziloRepository _repo = new();
        private readonly IObracunServis _obracun;
        private readonly RacunRepository _racunRepo = new();
        private readonly IEvidencijaServis _logger;

        public VoziloService(IObracunServis obracun, IEvidencijaServis logger)
        {
            _obracun = obracun;
            _logger = logger;
        }

        public string DodajVozilo(Vozilo v)
        {
            var svaVozila = _repo.GetAll();

            if (svaVozila.Count(x => !x.Servisirano) >= 10)
            {
                _logger.Zabelezi("Pokusaj dodavanja vozila preko kapaciteta", TipEvidencije.WARNING);
                return "Greska: Kapacitet servisa je popunjen (max 10 vozila)!";
            }

            v.ProcenjenaCena = _obracun.IzracunajCenu(v.ProcenjenaCena);

            svaVozila.Add(v);
            _repo.SaveAll(svaVozila);
            _logger.Zabelezi($"Uspesno dodato vozilo: {v.RegistarskiBroj}", TipEvidencije.INFO);
            return "Vozilo je uspesno evidentirano!";
        }

        public List<Vozilo> UzmiSvaVozila()
        {
            _logger.Zabelezi("Pregled svih vozila u sistemu", TipEvidencije.INFO);
            return _repo.GetAll();
        }

        public (int Ukupno, decimal Zarada, int NaServisu) GenerisiIzvestaj()
        {
            var svaVozila = _repo.GetAll();
            int ukupno = svaVozila.Count;
            decimal ukupnaZarada = svaVozila.Sum(v => v.ProcenjenaCena);
            int trenutnoNaServisu = svaVozila.Count(v => !v.Servisirano);

            return (ukupno, ukupnaZarada, trenutnoNaServisu);
        }

        public string ZavrsiServis(string registracija, string imeMehanicara)
        {
            var svaVozila = _repo.GetAll();
            var vozilo = svaVozila.FirstOrDefault(v => v.RegistarskiBroj == registracija && !v.Servisirano);

            if(vozilo == null)
            {
                _logger.Zabelezi($"Neuspesan pokusaj zavrsetka servisa za: {registracija}", TipEvidencije.WARNING);
                return "Greska: Vozilo nije pronadjeno ili je vec servisirano.";
            }

            vozilo.Servisirano = true;
            _repo.SaveAll(svaVozila);

            var noviRacun = new Racun
            {
                ImeMehanicara = imeMehanicara,
                Registracija = vozilo.RegistarskiBroj,
                UkupanIznos = vozilo.ProcenjenaCena,
                DatumVremeIzdavanja = DateTime.Now
            };

            var sviRacuni = _racunRepo.GetAll();
            sviRacuni.Add(noviRacun);
            _racunRepo.SaveAll(sviRacuni);

            _logger.Zabelezi($"Mehanicar {imeMehanicara} je zavrsio servis za vozilo {registracija}", TipEvidencije.INFO);
            _logger.Zabelezi($"Izdat racun za vozilo {registracija} u iznosu od {noviRacun.UkupanIznos}", TipEvidencije.INFO);

            return $"Servis zavrsen za {registracija}. Racun je izdat.";
        }

        public List<Racun> UzmiSveRacune()
        {
            _logger.Zabelezi("Pregled svih izdatih racuna", TipEvidencije.INFO);
            return _racunRepo.GetAll();
        }

        public List<Vozilo> UzmiVozilaNaCekanju()
        {
            _logger.Zabelezi("Pregled vozila koja cekaju servis", TipEvidencije.INFO);
            return _repo.GetAll().Where(v => v.Servisirano == false).ToList();
        }
    }
}

using System;
using Autoservis.Domain;
using NUnit.Framework;

namespace Autoservis.Tests
{
    [TestFixture]
    public class ModelTests
    {
        [Test]
        public void Vozilo_PriKreiranju_ImaGenerisanId_IServisiranoJeFalse()
        {
            var vozilo = new Vozilo();

            Assert.That(vozilo.Id, Is.Not.EqualTo(Guid.Empty), "Vozilo mora imati generisan GUID.");
            Assert.That(vozilo.Servisirano, Is.False, "Novo vozilo po defaultu ne sme biti obelezeno kao servisirano.");
        }


        [Test]
        public void Racun_PriKreiranju_GeneriseTrenutnoVremeIGuid()
        {
            var racun = new Racun();

            Assert.That(racun.Id, Is.Not.EqualTo(Guid.Empty), "Racun mora imati generisan ID.");

            var razlika = DateTime.Now - racun.DatumVremeIzdavanja;
            Assert.That(razlika.TotalSeconds < 2, Is.True, "Datum izdavanja mora biti trenutno vreme.");
        }

        [Test]
        public void Korisnik_Inicijalizacija_PostavljaIspravneVrednosti()
        {
            var korisnik = new Korisnik
            {
                KorisnickoIme = "test_korisnik",
                Uloga = Uloga.Mehanicar
            };

            Assert.That(korisnik.KorisnickoIme, Is.EqualTo("test_korisnik"), "Korisnicko ime se nije dobro upisalo.");
            Assert.That(korisnik.Uloga, Is.EqualTo(Uloga.Mehanicar),  "Uloga se nije dobro upisala.");
            Assert.That(korisnik.Id, Is.Not.EqualTo(Guid.Empty), "Korisnikk mora dobiti unikatan ID pri kreiranju.");
        }
    }
}

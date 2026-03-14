using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Moq;
using Autoservis.Domain;
using Autoservis.Services;
using Autoservis.Infrastructure.Repositories;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;

namespace Autoservis.Tests
{
    [TestFixture]
    public class ServiceTests
    {
        private Mock<IEvidencijaServis> _mockLogger;
        private Mock<IKorisnikRepository> _mockKorisnikRepo;
        private Mock<IVoziloRepository> _mockVoziloRepo;
        private Mock<IObracunServis> _mockObracun;
        private Mock<IRacunRepository> _mockRacunRepo;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<IEvidencijaServis>();
            _mockKorisnikRepo = new Mock<IKorisnikRepository>();
            _mockVoziloRepo = new Mock<IVoziloRepository>();
            _mockObracun = new Mock<IObracunServis>();
            _mockRacunRepo = new Mock<IRacunRepository>();

            _mockRacunRepo.Setup(r => r.GetAll()).Returns(new List<Racun>());
        }

        [Test]
        public void Login_SaIspravnimPodacima_VracaKorisnika()
        {
            var lazniKorisnici = new List<Korisnik>
            {
                new Korisnik { KorisnickoIme = "test", Lozinka = "123", Uloga = Uloga.Mehanicar}
            };

            _mockKorisnikRepo.Setup(repo => repo.GetAll()).Returns(lazniKorisnici);

            var authService = new AuthService(_mockLogger.Object, _mockKorisnikRepo.Object);

            var rezultat = authService.Login("test", "123");

            Assert.That(rezultat, Is.Not.Null);
            Assert.That(rezultat.KorisnickoIme, Is.EqualTo("test"));
            _mockLogger.Verify(l => l.Zabelezi(It.IsAny<string>(), TipEvidencije.INFO), Times.Once);
        }

        [Test]
        public void DodajVozilo_KadaJeKapacitetPun_VracaGresku()
        {
            var listaVozila = new List<Vozilo>();
            for (int i = 0; i < 10; i++)
            {
                listaVozila.Add(new Vozilo { Servisirano = false });
            }

            _mockVoziloRepo.Setup(repo => repo.GetAll()).Returns(listaVozila);

            var service = new VoziloService(_mockObracun.Object, _mockLogger.Object, _mockVoziloRepo.Object, _mockRacunRepo.Object);
            var novoVozilo = new Vozilo { RegistarskiBroj = "BG123" };

            var rezultat = service.DodajVozilo(novoVozilo);

            Assert.That(rezultat, Does.Contain("Greska"));
            _mockLogger.Verify(l => l.Zabelezi(It.IsAny<string>(), TipEvidencije.WARNING), Times.Once);
        }

        [Test]
        public void ZavrsiServis_IspravnoKoristiServisZaObracunCene()
        {
            string reg = "BG-999-ZZ";
            var vozilo = new Vozilo
            {
                RegistarskiBroj = reg,
                ProcenjenaCena = 1000,
                Servisirano = false
            };

            _mockVoziloRepo.Setup(r => r.GetAll()).Returns(new List<Vozilo> { vozilo });

            _mockObracun.Setup(o => o.IzracunajCenu(1000)).Returns(850);

            var service = new VoziloService(_mockObracun.Object, _mockLogger.Object, _mockVoziloRepo.Object, _mockRacunRepo.Object);

            service.ZavrsiServis(reg, "Mehanicar Marko");

            _mockRacunRepo.Verify(r => r.SaveAll(It.IsAny<List<Racun>>()), Times.Once);

            _mockLogger.Verify(l => l.Zabelezi(It.IsAny<string>(), TipEvidencije.INFO), Times.AtLeastOnce);
        }
    }
}

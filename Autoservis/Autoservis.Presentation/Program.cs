using Autoservis.Domain;
using Autoservis.Infrastructure.Repositories;
using Autoservis.Services;


IEvidencijaServis logger = new TekstualnaEvidencijaServis();
IKorisnikRepository korisnikRepo = new KorisnikRepository();
IVoziloRepository voziloRepo = new VoziloRepository();
IRacunRepository racunRepo = new RacunRepository();

InicijalizujPodatke(voziloRepo, racunRepo);

AuthService authService = new AuthService(logger, korisnikRepo);
authService.KreirajInicijalnogAdmina();

var trenutniObracun = ServisFabrika.OdrediSmenu();
VoziloService voziloService = new VoziloService(trenutniObracun, logger, voziloRepo, racunRepo);

Korisnik? ulogovaniKorisnik = null;

// LOGIN PETLJA
Console.WriteLine("=== SISTEM ZA UPRAVLJANJE AUTOSERVISOM ===");
while(ulogovaniKorisnik == null)
{
    Console.WriteLine("\nKorisnicko ime: ");
    string username = Console.ReadLine() ?? "";
    Console.WriteLine("Lozinka: ");
    string password = Console.ReadLine() ?? "";

    ulogovaniKorisnik = authService.Login(username, password);

    if(ulogovaniKorisnik == null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Greska: Pogresno korisnicko ime ili lozinka!");
        Console.ResetColor();
    }
}

Console.Clear();
Console.WriteLine($"Dobrodosli, {ulogovaniKorisnik.ImePrezime}!");
Console.WriteLine($"Trenutni rezim rada: {(trenutniObracun is PrepodneObracun ? "Prepodne (Popust 15%)" : "Poslepodne (Porez 10%)")}");

// GLAVNI MENI
bool programRadi = true;
while (programRadi)
{
    Console.WriteLine($"\n--- GLAVNI MENI ({ulogovaniKorisnik.Uloga}) ---");
    

    if(ulogovaniKorisnik.Uloga == Uloga.Menadzer)
    {
        Console.WriteLine("1. Dodaj novo vozilo na servis");
        Console.WriteLine("2. Pregled svih vozila");
        Console.WriteLine("3. Pregled svih izdatih racuna");
    }
    else if(ulogovaniKorisnik.Uloga == Uloga.Mehanicar)
    {
        Console.WriteLine("1. Pregled vozila koja cekaju na servis");
        Console.WriteLine("2. Zavrsi servis i izdaj racun");
    }

    Console.WriteLine("0. Izlaz");
    Console.Write("Izbor: ");
    string izbor = Console.ReadLine() ?? "";

    if(ulogovaniKorisnik.Uloga == Uloga.Menadzer)
    {
        switch(izbor)
        {
            case "1": DodajVoziloMeni(voziloService); break;
            case "2": PrikaziSvaVozila(voziloService); break;
            case "3": PrikaziSveRacune(voziloService); break;
            case "4": PrikaziIzvestaj(voziloService); break;
            case "0": programRadi = false; break;
            default: Console.WriteLine("Nepoznata opcija!"); break;
        }
    }
    else
    {
        switch(izbor)
        {
            case "1": PrikaziVozilaNaCekanju(voziloService); break;
            case "2": ZavrsiServisMeni(voziloService, ulogovaniKorisnik.ImePrezime); break;
            case "0": programRadi = false; break;
            default: Console.WriteLine("Nepoznata opcija!"); break;
        }
    }
}

void DodajVoziloMeni(VoziloService service)
{
    Console.WriteLine("\n--- UNOS NOVOG VOZILA ---");
    Console.Write("Registracija: ");
    string reg = Console.ReadLine() ?? "";
    Console.Write("Marka i model: ");
    string model = Console.ReadLine() ?? "";
    Console.WriteLine("Tip vozila (0 - Putnicko, 1 - Teretno, 2 - Motocikl): ");
    int.TryParse(Console.ReadLine(), out int tipIndex);
    TipVozila izabraniTip = (TipVozila)tipIndex;

    Console.Write("Procenjena cena usluge: ");
    decimal.TryParse(Console.ReadLine(), out decimal cena);

    Vozilo novo = new Vozilo()
    {
        RegistarskiBroj = reg,
        MarkaIModel = model,
        Tip = izabraniTip,
        ProcenjenaCena = cena,
        Servisirano = false
    };

    string rezultat = service.DodajVozilo(novo);
    Console.WriteLine(rezultat);
}

void PrikaziSvaVozila(VoziloService service)
{
    var lista = service.UzmiSvaVozila();
    IspisiTabeluVozila(lista, "LISTA SVIH VOZILA");
}

void PrikaziVozilaNaCekanju(VoziloService service)
{
    var lista = service.UzmiVozilaNaCekanju();
    IspisiTabeluVozila(lista, "LISTA VOZILA KOJA CEKAJU SERVIS");
}

void IspisiTabeluVozila(List<Vozilo> lista, string naslov)
{
    Console.WriteLine($"\n--- {naslov} ---");
    Console.WriteLine("{0,-15} {1,-20} {2,-15} {3,-10} {4,-10}", "Registracija", "Model", "Tip", "Cena", "Status");
    Console.WriteLine(new string('-', 75));

    foreach (var v in lista)
    {
        string status = v.Servisirano ? "GOTOVO" : "CEKA";
        Console.WriteLine("{0,-15} {1,-20} {2,-15} {3,-10:N2} {4,-10}", v.RegistarskiBroj, v.MarkaIModel, v.Tip, v.ProcenjenaCena, status);
    }
}

void ZavrsiServisMeni(VoziloService service, string imeMehanicara)
{
    Console.WriteLine("\n--- ZAVRSETAK SERVISA ---");
    Console.WriteLine("Unesite registarski broj vozila: ");
    string reg = Console.ReadLine() ?? "";
    string rezultat = service.ZavrsiServis(reg, imeMehanicara);
    Console.WriteLine(rezultat);
}

void PrikaziSveRacune(VoziloService service)
{
    var racuni = service.UzmiSveRacune();
    Console.WriteLine("\n--- LISTA IZDATIH RACUNA ---");
    Console.WriteLine("{0,-10} {1,-20} {2,-15} {3,-10}", "Vozilo", "Mehanicar", "Datum", "Iznos");
    Console.WriteLine(new string('-', 60));

    foreach(var r in racuni)
    {
        Console.WriteLine("{0,-10} {1,-20} {2,-15:dd:MM:yyyy} {3,-10:N2}", r.Registracija, r.ImeMehanicara, r.DatumVremeIzdavanja, r.UkupanIznos);
    }
}

void PrikaziIzvestaj(VoziloService service)
{
    var (ukupno, zarada, naServisu) = service.GenerisiIzvestaj();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("\n========== IZVESTAJ O POSLOVANJU ==========");
    Console.WriteLine($"Ukupan broj primljenih vozila: {ukupno}");
    Console.WriteLine($"Broj vozila trenutno na servisu: {naServisu}");
    Console.WriteLine($"Ukupna procenjena vrednost radova: {zarada:N2} DIN");
    Console.WriteLine("=============================================");
    Console.ResetColor();
}

static void InicijalizujPodatke(IVoziloRepository voziloRepo, IRacunRepository racunRepo)
{
    var vozila = voziloRepo.GetAll();
    if (vozila.Count == 0)
    {
        vozila.Add(new Vozilo
        {
            Id = Guid.NewGuid(),
            RegistarskiBroj = "BG-123-AA",
            MarkaIModel = "Audi A4",
            Tip = TipVozila.Putnicko,
            ProcenjenaCena = 5000,
            Servisirano = false
        });
        vozila.Add(new Vozilo
        {
            Id = Guid.NewGuid(),
            RegistarskiBroj = "NS-456-BB",
            MarkaIModel = "Yamaha R1",
            Tip = TipVozila.Motocikl,
            ProcenjenaCena = 3000,
            Servisirano = true
        });
        voziloRepo.SaveAll(vozila);
    }

    var racuni = racunRepo.GetAll();
    if (racuni.Count == 0)
    {
        racuni.Add(new Racun
        {
            Id = Guid.NewGuid(),
            Registracija = "NS-456-BB",
            ImeMehanicara = "Ivan Ivanovic",
            UkupanIznos = 2550,
            DatumVremeIzdavanja = DateTime.Now.AddDays(-1)
        });
        racunRepo.SaveAll(racuni);
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Autoservis.Domain;
using System.IO;

namespace Autoservis.Infrastructure.Repositories
{
    public class KorisnikRepository : IKorisnikRepository
    {
        private readonly string _putanja = "korisnici.json";

        public List<Korisnik> GetAll()
        {
            if (!File.Exists(_putanja)) return new List<Korisnik>();

            string json = File.ReadAllText(_putanja);

            return JsonSerializer.Deserialize<List<Korisnik>>(json) ?? new List<Korisnik>();
        }

        public void SaveAll(List<Korisnik> korisnici)
        {
            var opcije = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(korisnici, opcije);
            File.WriteAllText(_putanja, json);
        }
    }
}

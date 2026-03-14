using System;
using System.Collections.Generic;
using System.Text;
using Autoservis.Domain;
using System.Text.Json;

namespace Autoservis.Infrastructure.Repositories
{
    public class RacunRepository
    {
        private readonly string _putanja = "racuni.json";

        public List<Racun> GetAll()
        {
            if(!File.Exists(_putanja)) return new List<Racun>();
            string json = File.ReadAllText(_putanja);
            return JsonSerializer.Deserialize<List<Racun>>(json) ?? new List<Racun>();
        }

        public void SaveAll(List<Racun> racuni)
        {
            var opcije = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(racuni, opcije);
            File.WriteAllText(_putanja, json);
        }
    }
}

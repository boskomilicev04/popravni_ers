using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Autoservis.Domain;
using System.IO;

namespace Autoservis.Infrastructure.Repositories
{
    public class VoziloRepository : IVoziloRepository
    {
        private readonly string _putanja = "vozila.json";

        public List<Vozilo> GetAll()
        {
            if (!File.Exists(_putanja)) return new List<Vozilo>();
            string json = File.ReadAllText(_putanja);
            return JsonSerializer.Deserialize<List<Vozilo>>(json) ?? new List<Vozilo>();
        }

        public void Save(Vozilo vozilo)
        {
            var svaVozila = GetAll();
            svaVozila.Add(vozilo);
            string json = JsonSerializer.Serialize(svaVozila, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_putanja, json);
        }
        public void SaveAll(List<Vozilo> vozila)
        {
            string json = JsonSerializer.Serialize(vozila, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_putanja, json);
        }
    }
}

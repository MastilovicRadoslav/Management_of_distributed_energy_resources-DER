using System;

namespace Common.Models
{
    public class ResourceInfo
    {
        public int Id { get; set; } // Primarni ključ
        public string Name { get; set; } // Naziv resursa.
        public double Power { get; set; } // Snaga resursa u kW.
        public bool IsActive { get; set; } // Status resursa.

        // Informacije iz ResourceSchedule
        public DateTime? StartTime { get; set; } // Dozvoljava null vrednosti.
        public DateTime? EndTime { get; set; } // Dozvoljava null vrednosti.
        public double ActiveTime { get; set; } // Ukupno aktivno vreme u sekundama.

        // Zbirni podaci iz Statistics
        public double TotalActivePower { get; set; } // Ukupna aktivna snaga.
        public double TotalProducedEnergy { get; set; } // Ukupna proizvedena energija.
    }
}

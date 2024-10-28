using System;

namespace Common.Models
{
    public class ResourceSchedule
    {
        public int ResourceId { get; set; } // Jedinstveni identifikator resursa kojem raspored pripada.

        public DateTime StartTime { get; set; } // Vreme kada raspored počinje za dati resurs.

        public DateTime EndTime { get; set; } // Vreme kada raspored završava za dati resurs.
    }
}

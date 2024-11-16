using System;

namespace Common.Models
{
    public class ResourceSchedule
    {
        public int ResourceId { get; set; } // Strani ključ
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double ActiveTime { get; set; } // Možda želite da koristite double?
    }
}
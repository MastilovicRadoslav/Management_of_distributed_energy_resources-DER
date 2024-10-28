namespace Common.Models
{
    public class Statistics
    {
        public double TotalActivePower { get; set; } // Ukupna aktivna snaga svih resursa (u vatima ili kilovatima).

        public double TotalProducedEnergy { get; set; } // Ukupna proizvedena energija svih resursa (u kilovat-satima ili drugoj jedinici energije).
    }
}

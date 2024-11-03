namespace Common.Models
{
    public class Statistics
    {
        public int Id { get; set; } // Primarni ključ
        public double TotalActivePower { get; set; }
        public double TotalProducedEnergy { get; set; }
    }
}

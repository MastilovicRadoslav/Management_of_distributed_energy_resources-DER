namespace Common.Models
{
    public class DERResource
    {
        public int Id { get; set; } // Jedinstveni identifikator resursa.

        public string Name { get; set; } // Naziv resursa (može biti ime uređaja ili lokacije).

        public double Power { get; set; } // Snaga resursa u vatima (W) ili kilovatima (kW).

        public bool IsActive { get; set; } // Status resursa; true ako je aktivan, false ako je van funkcije.
    }
}

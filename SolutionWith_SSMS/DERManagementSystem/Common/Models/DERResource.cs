using System;

public class DERResource
{
    public int Id { get; set; } // Primarni ključ
    public string Name { get; set; }
    public double Power { get; set; }
    public bool IsActive { get; set; }
    public DateTime? StartTime { get; set; } // Nullable
    public DateTime? EndTime { get; set; } // Nullable
    public double ActiveTime { get; set; }

}

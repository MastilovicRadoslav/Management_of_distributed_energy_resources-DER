using Common.Models;
using System;
using System.Collections.Generic;

public class DERResource
{
    public int Id { get; set; } // Primarni ključ
    public string Name { get; set; }
    public double Power { get; set; }
    public bool IsActive { get; set; }

    // Nova polja
    public DateTime? StartTime { get; set; } // Nullable
    public DateTime? EndTime { get; set; } // Nullable
    public double ActiveTime { get; set; }

}

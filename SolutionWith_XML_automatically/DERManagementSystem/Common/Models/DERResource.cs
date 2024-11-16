using System;

public class DERResource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Power { get; set; }
    public bool IsActive { get; set; }

    // Nova polja
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double ActiveTime { get; set; }
}

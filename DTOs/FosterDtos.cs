namespace AnimalShelterApi.DTOs;

public class FosterPlacementDto
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public int AdopterId { get; set; }
    public string TermType { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; } = null!;
}

public class CreateFosterPlacementDto
{
    public int AnimalId { get; set; }
    public int AdopterId { get; set; } // Uses the registered adopter ID
    public string TermType { get; set; } = "Monthly";
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime? CustomEndDate { get; set; }
}
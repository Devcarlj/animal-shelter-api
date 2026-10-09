namespace AnimalShelterApi.DTOs;

public class AdoptionApplicationDto
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public int AdopterId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string Status { get; set; } = null!;
}

public class CreateAdoptionApplicationDto
{
    public int AnimalId { get; set; }
    public int AdopterId { get; set; }
}
namespace AnimalShelterApi.DTOs;

// --- Animal DTOs ---
public class AnimalDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Species { get; set; } = null!;
    public string Breed { get; set; } = null!;
    public int AgeMonths { get; set; }
    public string HealthStatus { get; set; } = null!;
    public bool IsAdoptable { get; set; }
}

public class UpsertAnimalDto
{
    public string Name { get; set; } = null!;
    public string Species { get; set; } = null!;
    public string Breed { get; set; } = null!;
    public int AgeMonths { get; set; }
    public string HealthStatus { get; set; } = null!;
    public bool IsAdoptable { get; set; } = true;
}

// --- Adopter DTOs ---
public class AdopterDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public bool IsBlacklisted { get; set; }
}

public class UpsertAdopterDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public bool IsBlacklisted { get; set; } = false;
}
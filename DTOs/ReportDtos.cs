public class ShelterSummaryDto
{

    public int TotalAnimals { get; set; }
    public int AdoptableAnimalsCount { get; set; }
    public int AdoptedOrLockedAnimalsCount { get; set; }
    public int TotalAdopters { get; set; }
    public int BlacklistedAdoptersCount { get; set; }
    public int TotalFosterPlacements { get; set; }
    public int ActiveFosterPlacements { get; set; }


    public Dictionary<string, int> AnimalsBySpecies { get; set; } = new();
    public List<ActiveFosterSummaryItemDto> CurrentActiveFosters { get; set; } = new();
}

public class ActiveFosterSummaryItemDto
{
    public int FosterId { get; set; }
    public string AnimalName { get; set; } = null!;
    public string AdopterName { get; set; } = null!;
    public DateTime EndDate { get; set; }
}
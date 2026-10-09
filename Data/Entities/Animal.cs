using System;
using System.Collections.Generic;

namespace AnimalShelterApi.Data.Entities;

public partial class Animal
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Species { get; set; } = null!;

    public string Breed { get; set; } = null!;

    public int AgeMonths { get; set; }

    public string HealthStatus { get; set; } = null!;

    public bool IsAdoptable { get; set; }

    public virtual ICollection<AdoptionApplication> AdoptionApplications { get; set; } = new List<AdoptionApplication>();

    public virtual ICollection<FosterPlacement> FosterPlacements { get; set; } = new List<FosterPlacement>();
}

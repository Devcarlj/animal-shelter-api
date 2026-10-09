using System;
using System.Collections.Generic;

namespace AnimalShelterApi.Data.Entities;

public partial class FosterPlacement
{
    public int Id { get; set; }

    public int AnimalId { get; set; }

    public int AdopterId { get; set; }

    public string TermType { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Adopter Adopter { get; set; } = null!;

    public virtual Animal Animal { get; set; } = null!;
}

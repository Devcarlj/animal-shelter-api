using AnimalShelterApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalShelterApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly AnimalShelterDbContext _context;

    public ReportsController(AnimalShelterDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<ShelterSummaryDto>> GetShelterSummary()
    {
        // 1. Basic counts
        var totalAnimals = await _context.Animals.CountAsync();
        var adoptableAnimals = await _context.Animals.CountAsync(a => a.IsAdoptable);
        var totalAdopters = await _context.Adopters.CountAsync();
        var blacklistedAdopters = await _context.Adopters.CountAsync(a => a.IsBlacklisted);
        var totalFosters = await _context.FosterPlacements.CountAsync();
        var activeFosters = await _context.FosterPlacements.CountAsync(f => f.Status == "Active");

        // 2. Group animals by species (e.g., Dog: 12, Cat: 8) using LINQ GroupBy
        var animalsBySpecies = await _context.Animals
            .GroupBy(a => a.Species)
            .ToDictionaryAsync(g => g.Key, g => g.Count());

        // 3. Pull actual lightweight objects for active fosters so the UI can display names
        var currentActiveFosters = await _context.FosterPlacements
            .Where(f => f.Status == "Active")
            .Join(_context.Animals, f => f.AnimalId, a => a.Id, (f, a) => new { f, a })
            .Join(_context.Adopters, fa => fa.f.AdopterId, ad => ad.Id, (fa, ad) => new ActiveFosterSummaryItemDto
            {
                FosterId = fa.f.Id,
                AnimalName = fa.a.Name,
                AdopterName = ad.FullName,
                EndDate = fa.f.EndDate
            })
            .ToListAsync();

        var summary = new ShelterSummaryDto
        {
            TotalAnimals = totalAnimals,
            AdoptableAnimalsCount = adoptableAnimals,
            AdoptedOrLockedAnimalsCount = totalAnimals - adoptableAnimals,
            TotalAdopters = totalAdopters,
            BlacklistedAdoptersCount = blacklistedAdopters,
            TotalFosterPlacements = totalFosters,
            ActiveFosterPlacements = activeFosters,
            AnimalsBySpecies = animalsBySpecies,
            CurrentActiveFosters = currentActiveFosters
        };

        return Ok(summary);
    }
}
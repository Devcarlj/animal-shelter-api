using AnimalShelterApi.Data;
using AnimalShelterApi.Data.Entities;
using AnimalShelterApi.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalShelterApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FosterPlacementsController : ControllerBase
{
    private readonly AnimalShelterDbContext _context;
    private readonly IMapper _mapper;

    public FosterPlacementsController(AnimalShelterDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/fosterplacements
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FosterPlacementDto>>> GetAllFosterPlacements()
    {
        var fosters = await _context.FosterPlacements.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<FosterPlacementDto>>(fosters));
    }

    // GET: api/fosterplacements/5
    [HttpGet("{id}")]
    public async Task<ActionResult<FosterPlacementDto>> GetFosterPlacementById(int id)
    {
        var foster = await _context.FosterPlacements.FindAsync(id);
        if (foster == null) return NotFound(new { message = "Foster placement record not found." });

        return Ok(_mapper.Map<FosterPlacementDto>(foster));
    }

    [HttpPost]
    public async Task<ActionResult<FosterPlacementDto>> CreateFosterPlacement([FromBody] CreateFosterPlacementDto dto)
    {
        // 1. Verify Animal exists and is available
        var animal = await _context.Animals.FindAsync(dto.AnimalId);
        if (animal == null) return NotFound(new { message = "Animal not found." });
        if (!animal.IsAdoptable) return BadRequest(new { message = "This animal is currently unavailable for placement." });

        // 2. Verify Adopter exists and check Blacklist rule!
        var adopter = await _context.Adopters.FindAsync(dto.AdopterId);
        if (adopter == null) return NotFound(new { message = "Adopter profile not found." });
        if (adopter.IsBlacklisted) return BadRequest(new { message = "Action denied: This adopter is blacklisted from shelter programs." });

        // 3. Smart Duration Calculation
        DateTime calculatedEndDate = dto.TermType switch
        {
            "Trial" => dto.StartDate.AddDays(14),
            "Monthly" => dto.StartDate.AddMonths(1),
            "Quarterly" => dto.StartDate.AddMonths(3),
            "Custom" => dto.CustomEndDate ?? dto.StartDate.AddDays(30),
            _ => dto.StartDate.AddMonths(1)
        };

        // 4. Save Record
        var fosterPlacement = _mapper.Map<FosterPlacement>(dto);
        fosterPlacement.EndDate = calculatedEndDate;
        fosterPlacement.Status = "Active";

        _context.FosterPlacements.Add(fosterPlacement);
        animal.IsAdoptable = false; // Lock animal availability

        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<FosterPlacementDto>(fosterPlacement);
        return CreatedAtAction(nameof(GetFosterPlacementById), new { id = resultDto.Id }, resultDto);
    }
    // PUT: api/fosterplacements/5/complete (Mark foster as completed/returned)
    [HttpPut("{id}/complete")]
    public async Task<IActionResult> CompleteFosterPlacement(int id)
    {
        var foster = await _context.FosterPlacements.FindAsync(id);
        if (foster == null) return NotFound(new { message = "Foster placement not found." });

        foster.Status = "Completed";

        // Release the animal back to being adoptable
        var animal = await _context.Animals.FindAsync(foster.AnimalId);
        if (animal != null)
        {
            animal.IsAdoptable = true;
        }

        await _context.SaveChangesAsync();
        return Ok(new { message = "Foster placement completed and animal returned to shelter availability." });
    }
}

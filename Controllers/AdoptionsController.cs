using AnimalShelterApi.Data;
using AnimalShelterApi.Data.Entities;
using AnimalShelterApi.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalShelterApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdoptionsController : ControllerBase
{
    private readonly AnimalShelterDbContext _context;
    private readonly IMapper _mapper;

    public AdoptionsController(AnimalShelterDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/adoptions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdoptionApplicationDto>>> GetApplications()
    {
        var applications = await _context.AdoptionApplications.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<AdoptionApplicationDto>>(applications));
    }

    // POST: api/adoptions (Automated Business Rule: Check Blacklist & Availability)
    [HttpPost]
    public async Task<ActionResult<AdoptionApplicationDto>> CreateApplication([FromBody] CreateAdoptionApplicationDto dto)
    {
        // Rule 1: Check if the animal exists and is available
        var animal = await _context.Animals.FindAsync(dto.AnimalId);
        if (animal == null || !animal.IsAdoptable)
        {
            return BadRequest(new { message = "Selected animal is not available for adoption." });
        }

        // Rule 2: Check if the adopter exists and is NOT blacklisted
        var adopter = await _context.Adopters.FindAsync(dto.AdopterId);
        if (adopter == null)
        {
            return BadRequest(new { message = "Adopter profile not found." });
        }
        if (adopter.IsBlacklisted)
        {
            return BadRequest(new { message = "Application rejected: Adopter is flagged under shelter restrictions." });
        }

        // Map DTO to Entity
        var application = _mapper.Map<AdoptionApplication>(dto);
        application.ApplicationDate = DateTime.Now;
        application.Status = "Pending";

        _context.AdoptionApplications.Add(application);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<AdoptionApplicationDto>(application);
        return CreatedAtAction(nameof(GetApplications), new { id = resultDto.Id }, resultDto);
    }

    // PUT: api/adoptions/5/approve (Automated Business Rule: Locks Animal Availability)
    [HttpPut("{id}/approve")]
    public async Task<IActionResult> ApproveApplication(int id)
    {
        var application = await _context.AdoptionApplications.FindAsync(id);
        if (application == null) return NotFound();

        var animal = await _context.Animals.FindAsync(application.AnimalId);
        if (animal != null)
        {
            // Automation: Once approved, animal is no longer adoptable by others
            animal.IsAdoptable = false;
        }

        application.Status = "Approved";
        await _context.SaveChangesAsync();

        return Ok(new { message = "Application approved successfully. Animal availability locked." });
    }
}
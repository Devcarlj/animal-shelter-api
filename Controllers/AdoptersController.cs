using AnimalShelterApi.Data;
using AnimalShelterApi.Data.Entities;
using AnimalShelterApi.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalShelterApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdoptersController : ControllerBase
{
    private readonly AnimalShelterDbContext _context;
    private readonly IMapper _mapper;

    public AdoptersController(AnimalShelterDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/adopters
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdopterDto>>> GetAllAdopters()
    {
        var adopters = await _context.Adopters.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<AdopterDto>>(adopters));
    }

    // GET: api/adopters/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AdopterDto>> GetAdopterById(int id)
    {
        var adopter = await _context.Adopters.FindAsync(id);
        if (adopter == null) return NotFound(new { message = "Adopter not found." });

        return Ok(_mapper.Map<AdopterDto>(adopter));
    }

    // POST: api/adopters (Register a new adopter)
    [HttpPost]
    public async Task<ActionResult<AdopterDto>> CreateAdopter([FromBody] UpsertAdopterDto dto)
    {
        var adopter = _mapper.Map<Adopter>(dto);
        _context.Adopters.Add(adopter);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<AdopterDto>(adopter);
        return CreatedAtAction(nameof(GetAdopterById), new { id = resultDto.Id }, resultDto);
    }

    // PUT: api/adopters/5 (Update adopter or blacklist status)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdopter(int id, [FromBody] UpsertAdopterDto dto)
    {
        var adopter = await _context.Adopters.FindAsync(id);
        if (adopter == null) return NotFound(new { message = "Adopter not found." });

        _mapper.Map(dto, adopter);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Adopter profile updated successfully." });
    }

    // DELETE: api/adopters/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdopter(int id)
    {
        var adopter = await _context.Adopters.FindAsync(id);
        if (adopter == null) return NotFound(new { message = "Adopter not found." });

        _context.Adopters.Remove(adopter);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Adopter deleted successfully." });
    }
}
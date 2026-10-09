using AnimalShelterApi.Data;
using AnimalShelterApi.Data.Entities;
using AnimalShelterApi.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimalShelterApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalsController : ControllerBase
{
    private readonly AnimalShelterDbContext _context;
    private readonly IMapper _mapper;

    public AnimalsController(AnimalShelterDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/animals
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAllAnimals()
    {
        var animals = await _context.Animals.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<AnimalDto>>(animals));
    }

    // GET: api/animals/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AnimalDto>> GetAnimalById(int id)
    {
        var animal = await _context.Animals.FindAsync(id);
        if (animal == null) return NotFound(new { message = "Animal not found." });

        return Ok(_mapper.Map<AnimalDto>(animal));
    }

    // POST: api/animals (Register a new animal)
    [HttpPost]
    public async Task<ActionResult<AnimalDto>> CreateAnimal([FromBody] UpsertAnimalDto dto)
    {
        var animal = _mapper.Map<Animal>(dto);
        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();

        var resultDto = _mapper.Map<AnimalDto>(animal);
        return CreatedAtAction(nameof(GetAnimalById), new { id = resultDto.Id }, resultDto);
    }

    // PUT: api/animals/5 (Update animal details)
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAnimal(int id, [FromBody] UpsertAnimalDto dto)
    {
        var animal = await _context.Animals.FindAsync(id);
        if (animal == null) return NotFound(new { message = "Animal not found." });

        _mapper.Map(dto, animal);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Animal updated successfully." });
    }

    // DELETE: api/animals/5 (Remove an animal)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnimal(int id)
    {
        var animal = await _context.Animals.FindAsync(id);
        if (animal == null) return NotFound(new { message = "Animal not found." });

        _context.Animals.Remove(animal);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Animal deleted successfully." });
    }
}
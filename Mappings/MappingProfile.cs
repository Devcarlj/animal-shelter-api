using AnimalShelterApi.Data.Entities;
using AnimalShelterApi.DTOs;
using AutoMapper;

namespace AnimalShelterApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Adoption Mappings
        CreateMap<AdoptionApplication, AdoptionApplicationDto>().ReverseMap();
        CreateMap<CreateAdoptionApplicationDto, AdoptionApplication>();

        // Animal Mappings
        CreateMap<Animal, AnimalDto>().ReverseMap();
        CreateMap<UpsertAnimalDto, Animal>();

        // Adopter Mappings
        CreateMap<Adopter, AdopterDto>().ReverseMap();
        CreateMap<UpsertAdopterDto, Adopter>();
    }
}
using AnimalShelterApi.Data.Entities;
using AnimalShelterApi.DTOs;
using AutoMapper;

namespace AnimalShelterApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AdoptionApplication, AdoptionApplicationDto>().ReverseMap();
        CreateMap<CreateAdoptionApplicationDto, AdoptionApplication>();
        CreateMap<Animal, AnimalDto>().ReverseMap();
        CreateMap<UpsertAnimalDto, Animal>();
        CreateMap<Adopter, AdopterDto>().ReverseMap();
        CreateMap<UpsertAdopterDto, Adopter>();
        CreateMap<FosterPlacement, FosterPlacementDto>().ReverseMap();
        CreateMap<CreateFosterPlacementDto, FosterPlacement>();
    }
}
using API.DTOs.LandmarkDTOs;
using API.DTOs.StateDTOs;
using API.Models;
using AutoMapper;

namespace API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            // Map State entity to StateSummaryDTO
            CreateMap<State, StateSummaryDTO>();

            // Map State entity to StateDetailDTO
            CreateMap<State, StateDetailDTO>();

            // Map in both directions
            CreateMap<State, StateCreateUpdateDTO>().ReverseMap();

     

            // Map Landmark to LandmarkSummaryDTO (GET all)
            CreateMap<Landmark, LandmarkSummaryDTO>();

            // Map Landmark to LandmarkDetailDTO (GET by Name/ID)
            CreateMap<Landmark, LandmarkDetailDTO>();

            // Map in both directions (POST/PUT)
            CreateMap<Landmark, LandmarkCreateUpdateDTO>().ReverseMap();


        }
    }
}

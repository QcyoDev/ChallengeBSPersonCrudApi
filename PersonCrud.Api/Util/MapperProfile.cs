using AutoMapper;
using PersonCrud.Api.Dtos;
using PersonCrud.Api.Models;

namespace PersonCrud.Api.Util
{
    public class MapperProfile: Profile
    {

        public MapperProfile()
        {
            CreateMap<Person, PersonPostDto>().ReverseMap();

            CreateMap<Person, PersonGetDto>().ReverseMap();

            CreateMap<Person, PersonPutDto>().ReverseMap();
            
            CreateMap<Contact, ContactPostDto>().ReverseMap();

            CreateMap<Contact, ContactGetDto>().ReverseMap();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PRN222.Assignment1.Business.DTOs;
using PRN222.Assignment1.Data.Models;
namespace PRN222.Assignment1.Business.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<Event, EventDto>();
            CreateMap<EventDto, Event>();
            CreateMap<EventCategory, EventCategoryDto>();
            CreateMap<EventCategoryDto, EventCategory>();
            CreateMap<Attendee, AttendeeDto>();
            CreateMap<AttendeeDto, Attendee>();
        }
    }
}

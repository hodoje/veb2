using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;
using WebApp.Models.DomainModels;
using WebApp.Models.DomainModels.Dtos;
using WebApp.Models.Enumerations;

namespace WebApp.App_Start.MappingProfiles
{
    public class ApplicationUserMappingProfile : Profile
    {
        public ApplicationUserMappingProfile()
        {
			CreateMap<ApplicationUser, ApplicationUserDto>()
				.ForMember(destination => destination.Email,
				opts => opts.MapFrom(source => source.Email))
				.ForMember(destination => destination.Name,
				opts => opts.MapFrom(source => source.Name))
				.ForMember(destination => destination.Lastname,
				opts => opts.MapFrom(source => source.LastName))
				.ForMember(destination => destination.Birthday,
				opts => opts.MapFrom(source => source.Birthday))
				.ForMember(destination => destination.Address,
				opts => opts.MapFrom(source => source.Address))
				.ForMember(destination => destination.RegistrationStatus,
				opts => opts.MapFrom(source => (source.RegistrationStatus).ToString()))
				.ForMember(destination => destination.DocumentImage,
				opts => opts.MapFrom(source => source.DocumentImage))
				.ForMember(destination => destination.UserType,
				opts => opts.MapFrom(source => source.UserType))
				.ForMember(destination => destination.Banned,
				opts => opts.MapFrom(source => source.Banned));				
            CreateMap<ApplicationUserDto, ApplicationUser>()
                .ForMember(destination => destination.Email,
                opts => opts.MapFrom(source => source.Email))
                .ForMember(destination => destination.Name,
                opts => opts.MapFrom(source => source.Name))
                .ForMember(destination => destination.LastName,
                opts => opts.MapFrom(source => source.Lastname))
                .ForMember(destination => destination.Birthday,
                opts => opts.MapFrom(source => source.Birthday))
                .ForMember(destination => destination.Address,
                opts => opts.MapFrom(source => source.Address))
                .ForMember(destination => destination.RegistrationStatus,
                opts => opts.MapFrom(source => Enum.Parse(typeof(RegistrationStatus), source.RegistrationStatus)))
                .ForMember(destination => destination.DocumentImage,
                opts => opts.MapFrom(source => source.DocumentImage))
                .ForMember(destination => destination.UserType,
                opts => opts.MapFrom(source => source.UserType))
                .ForMember(destination => destination.Banned,
                opts => opts.MapFrom(source => source.Banned));
        }
    }
}
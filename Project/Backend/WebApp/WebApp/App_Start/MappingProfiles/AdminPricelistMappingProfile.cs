using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;

namespace WebApp.App_Start.MappingProfiles
{
	public class AdminPricelistMappingProfile : Profile
	{
		public AdminPricelistMappingProfile()
		{
			CreateMap<Pricelist, AdminPricelistDto>()
				.ForMember(destination => destination.FromDate,
				opts => opts.MapFrom(source => source.FromDate));
			CreateMap<AdminPricelistDto, Pricelist>()
				.ForMember(destination => destination.FromDate,
				opts => opts.MapFrom(source => source.FromDate));
		}
	}
}
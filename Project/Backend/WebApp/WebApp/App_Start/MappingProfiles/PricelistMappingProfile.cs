using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;

namespace WebApp.App_Start.MappingProfiles
{
	public class PricelistMappingProfile : Profile
	{
		public PricelistMappingProfile()
		{
			CreateMap<Pricelist, PricelistDto>()
				.ForMember(destination => destination.FromDate,
				opts => opts.MapFrom(source => source.FromDate));
			CreateMap<PricelistDto, Pricelist>()
				.ForMember(destination => destination.FromDate,
				opts => opts.MapFrom(source => source.FromDate));
		}
	}
}
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;

namespace WebApp.App_Start.MappingProfiles
{
	public class TransportationLineRouteMappingProfile : Profile
	{
		public TransportationLineRouteMappingProfile()
		{
			CreateMap<RoutePointDto, TransportationLineRoutePoint>()
				.ForMember(destination => destination.SequenceNo,
				opts => opts.MapFrom(source => source.SequenceNumber))
				.ForMember(destination => destination.Station,
				opts => opts.MapFrom(source => source.Station));
		}
	}
}
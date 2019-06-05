using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;

namespace WebApp.App_Start.MappingProfiles
{
	public class ScheduleMappingProfile : Profile
	{
		public ScheduleMappingProfile()
		{
			CreateMap<Schedule, ScheduleDto>()
				.ForMember(destination => destination.DayOfTheWeek,
				opts => opts.MapFrom(source => source.DayOfTheWeek.Name))
				.ForMember(destination => destination.LineNum,
				opts => opts.MapFrom(source => source.TransportationLine.LineNum))
				.ForMember(destination => destination.LineType,
				opts => opts.MapFrom(source => source.TransportationLine.TransportationLineType.Name));
		}
	}
}
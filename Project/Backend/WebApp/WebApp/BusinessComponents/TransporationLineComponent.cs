using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public class TransporationLineComponent : ITransporationLineComponent
    {
        public TransportationLinePlanDto GetTransporationLinePlan(IUnitOfWork unitOfWork, int linueNumber)
        {
            TransportationLine transportationLine = unitOfWork.TransportationLineRepository.Find(x => x.LineNum == linueNumber).FirstOrDefault();

            if (transportationLine == null)
            {
                return null;
            }

            TransportationLinePlanDto planDto = new TransportationLinePlanDto() { LineNumber = transportationLine.LineNum, Routes = new List<RoutePointDto>() };

            List<TransportationLineRoute> routes = unitOfWork.TransportationLineRouteRepository.Find(x => x.TransporationLineId == transportationLine.Id).ToList();
            routes.Sort(TransportationLineRoute.CompareByRoutePoint);

            foreach (TransportationLineRoute route in routes)
            {
                planDto.Routes.Add(new RoutePointDto()
                {
                    SequenceNumber = route.RoutePoint,
                    Station = unitOfWork.StationRepository.Get(route.StationId)
                });
            }

            return planDto;
        }
    }
}
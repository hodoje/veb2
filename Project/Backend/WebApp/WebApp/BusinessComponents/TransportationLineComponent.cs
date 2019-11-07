using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public class TransportationLineComponent : ITransportationLineComponent
    {
        public TransportationLinePlanDto GetTransportationLinePlan(IUnitOfWork unitOfWork, int lineNumber)
        {
			TransportationLine transportationLine = unitOfWork.TransportationLineRepository.Find(x => x.LineNum == lineNumber).FirstOrDefault();

			if (transportationLine == null)
			{
				return null;
			}

			TransportationLinePlanDto planDto = new TransportationLinePlanDto() { LineNumber = transportationLine.LineNum, RoutePoints = new List<RoutePointDto>() };

			List<TransportationLineRoutePoint> routes = unitOfWork.TransportationLineRoutePointsRepository.Find(x => x.TransportationLineId == transportationLine.Id).ToList();
			//routes.Sort(TransportationLineRoute.CompareByRoutePoint);

			foreach (TransportationLineRoutePoint route in routes)
			{
				planDto.RoutePoints.Add(new RoutePointDto()
				{
					SequenceNumber = route.SequenceNo,
					Station = unitOfWork.StationRepository.Get(route.StationId)
				});
			}

			return planDto;
		}

		//private void SortRoutePointsBySequenceNo()
		//{

		//}
    }
}
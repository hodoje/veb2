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

		public bool UpdateTransportationLinePlan(IUnitOfWork unitOfWork, TransportationLinePlanDto updatedLinePlan)
		{
			bool result = false;
			// Current idea to update Many-to-Many relationship is to delete all Many-to-Many table entries and add new ones
			TransportationLine tLineToBeUpdated = unitOfWork.TransportationLineRepository.Find(tl => tl.LineNum == updatedLinePlan.LineNumber).FirstOrDefault();
			if (tLineToBeUpdated != null)
			{
				try
				{
					List<TransportationLineRoutePoint> tLineToBeUpdatedRoutePoints = unitOfWork.TransportationLineRoutePointsRepository.
					Find(rp => rp.TransportationLineId == tLineToBeUpdated.Id).ToList();
					unitOfWork.TransportationLineRoutePointsRepository.RemoveRange(tLineToBeUpdatedRoutePoints);
					unitOfWork.Complete();

					List<TransportationLineRoutePoint> tLToBeUpdatedRoutePointsToAddList = new List<TransportationLineRoutePoint>(updatedLinePlan.RoutePoints.Count);
					foreach (var rp in updatedLinePlan.RoutePoints)
					{
						TransportationLineRoutePoint tlrp = new TransportationLineRoutePoint();
						tlrp.TransportationLineId = tLineToBeUpdated.Id;
						tlrp.StationId = rp.Station.Id;
						tLToBeUpdatedRoutePointsToAddList.Add(tlrp);
					}

					unitOfWork.TransportationLineRoutePointsRepository.AddRange(tLToBeUpdatedRoutePointsToAddList);
					unitOfWork.Complete();
					result = true;
				}
				catch(Exception e)
				{
					throw new Exception("Error while updating transportation line plan.", e);
				}
			}
			return result;
		}
	}
}
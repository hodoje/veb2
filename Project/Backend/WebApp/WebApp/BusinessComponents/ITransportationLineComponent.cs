using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public interface ITransportationLineComponent
    {
        TransportationLinePlanDto GetTransportationLinePlan(IUnitOfWork unitOfWork, int lineNumber);
		bool UpdateTransportationLinePlan(IUnitOfWork unitOfWork, TransportationLinePlanDto updatedLinePlan);
	}
}

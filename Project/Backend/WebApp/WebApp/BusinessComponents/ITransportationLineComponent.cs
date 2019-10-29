using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public interface ITransportationLineComponent
    {
        TransportationLinePlanDto GetTransporationLinePlan(IUnitOfWork unitOfWork, int lineNumber);
    }
}

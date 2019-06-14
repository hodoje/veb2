using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public interface ITransporationLineComponent
    {
        TransportationLinePlanDto GetTransporationLinePlan(IUnitOfWork unitOfWork, int lineNumber);
    }
}

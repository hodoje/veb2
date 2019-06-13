using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public interface ITransporationLineComponent
    {
        TransporationLinePlanDto GetTransporationLinePlan(IUnitOfWork unitOfWork, int linueNumber);
    }
}

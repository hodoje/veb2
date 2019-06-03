using System.Collections.Generic;
using WebApp.Models;
using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public interface ITicketBusinessComponent
    {
        IEnumerable<TicketTypePricelistDto> ListAvailableTicketsWithPrices(IUnitOfWork unitOfWork, double discountCoefficient);

        bool BuyTicket(ApplicationUser user, int ticketTypeId);
    }
}

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using WebApp.Models;
using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public interface ITicketBusinessComponent
    {
        IEnumerable<TicketTypePricelistDto> ListAllTicketPrices(ApplicationUserManager userManager, IUnitOfWork unitOfWork);

		Task<bool> ValidateTicket(ApplicationUserManager userManager, IUnitOfWork unitOfWork, int ticketId);

		Task<IEnumerable<TicketTypePricelistDto>> GetTicketPriceForUser(ApplicationUserManager userManager, IUnitOfWork unitOfWork);

		Task<HttpStatusCode> TicketPurchase(ApplicationUserManager userManager, IUnitOfWork unitOfWork, TicketDto ticketDto, IEmailSender emailSender);
	}
}

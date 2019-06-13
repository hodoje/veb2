﻿using System.Collections.Generic;
using WebApp.Models;
using WebApp.Models.Dtos;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.BusinessComponents
{
    public interface ITicketBusinessComponent
    {
        IEnumerable<TicketTypePricelistDto> ListAllTicketPrices(IUnitOfWork unitOfWork);
        IEnumerable<TicketTypePricelistDto> ListTicketPricesForUser(IUnitOfWork unitOfWork, double discountCoefficient, bool userConfirmed);

        int BuyTicket(IUnitOfWork unitOfWork, ApplicationUser user, int ticketTypeId);

		bool ValidateTicket(IUnitOfWork unitOfWork, int ticketId);
    }
}

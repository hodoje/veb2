using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;

namespace WebApp.BusinessComponents
{
	public interface IPricelistComponent
	{
		void SetPricelistPriceFromTicketType(ref PricelistDto plDto, TicketTypePricelist ttpl);
		void SetTicketTypeBasePriceFromPricelist(TicketType tt, ref TicketTypePricelist ttpl, PricelistDto plDto);
	}
}

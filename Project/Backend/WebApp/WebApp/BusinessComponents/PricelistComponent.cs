using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models.DomainModels;
using WebApp.Models.Dtos;

namespace WebApp.BusinessComponents
{
	public class PricelistComponent : IPricelistComponent
	{
		public void SetPricelistPriceFromTicketType(ref PricelistDto plDto, TicketTypePricelist ttpl)
		{
			if (ttpl.TicketType.Name == "Hourly")
			{
				plDto.Hourly = ttpl.BasePrice;
			}
			else if (ttpl.TicketType.Name == "Daily")
			{
				plDto.Daily = ttpl.BasePrice;
			}
			else if (ttpl.TicketType.Name == "Monthly")
			{
				plDto.Monthly = ttpl.BasePrice;
			}
			else if (ttpl.TicketType.Name == "Yearly")
			{
				plDto.Yearly = ttpl.BasePrice;
			}
		}

		public void SetTicketTypeBasePriceFromPricelist(TicketType tt, ref TicketTypePricelist ttpl, PricelistDto plDto)
		{
			if (tt.Name == "Hourly")
			{
				ttpl.BasePrice = plDto.Hourly;
			}
			else if (tt.Name == "Daily")
			{
				ttpl.BasePrice = plDto.Daily;
			}
			else if (tt.Name == "Monthly")
			{
				ttpl.BasePrice = plDto.Monthly;
			}
			else if (tt.Name == "Yearly")
			{
				ttpl.BasePrice = plDto.Yearly;
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Dtos
{
	public class TransactionTicketWrapper
	{
		public string UserEmail { get; set; }
		public GeneralTransactionDto Transaction { get; set; }
		public TicketDto TicketDto { get; set; }
	}
}
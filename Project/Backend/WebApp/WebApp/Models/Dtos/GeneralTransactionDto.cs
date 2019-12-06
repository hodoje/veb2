using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Dtos
{
	public class GeneralTransactionDto
	{
		public string OrderId { get; set; }
		public DateTime DateCreated { get; set; }
		public string Status { get; set; }
		public string PayerEmail { get; set; }
		public string Price { get; set; }
		public string Currency { get; set; }
	}
}
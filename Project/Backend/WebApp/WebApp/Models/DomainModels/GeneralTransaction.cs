using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.DomainModels
{
	// Using this name because the name 'Transaction' is already taken by BrainTree
	// and I don't want to clump the code with namespace extensions
	public class GeneralTransaction
	{
		public int Id { get; set; }
		public string OrderId { get; set; }
		[Column(TypeName = "datetime2")]
		public DateTime DateCreated { get; set; }
		public string Status { get; set; }
		public string PayerEmail { get; set; }
	    public string Price { get; set; }
		public string Currency { get; set; }
		public string UserId { get; set; }
		[Timestamp]
		public byte[] Timestamp { get; set; }
	}
}
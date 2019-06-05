using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Dtos
{
    public class TransportationLineDto
	{
		public string LineNum { get; set; }
		public TransportationLineTypeDto TransportationLineType { get; set; }
	}
}
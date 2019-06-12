using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.Models.DomainModels.Benefits;

namespace WebApp.Models.DomainModels
{
    public class UserType
	{
		public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        public ICollection<Benefit> Benefits { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
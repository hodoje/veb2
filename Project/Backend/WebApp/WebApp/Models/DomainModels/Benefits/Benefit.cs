using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebApp.Models.DomainModels.Benefits
{
    [KnownType(typeof(TransportDiscountBenefit))]
    public abstract class Benefit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserType> UserTypes { get; set; }
    }
}
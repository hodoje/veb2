using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels.Benefits
{
    public class TransportDiscountBenefit : Benefit
    {
        public TransportDiscountBenefit()
        {

        }

        public TransportDiscountBenefit(double discountValue)
        {
            CoefficientDiscount = discountValue;
        }

        public double CoefficientDiscount { get; set; }
    }
}
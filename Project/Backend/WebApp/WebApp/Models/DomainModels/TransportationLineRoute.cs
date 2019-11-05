using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels
{
    public class TransportationLineRoute
    {
        public int Id { get; set; }
        public int SequenceNo { get; set; }
        public int TransportationLineId { get; set; }
        public virtual TransportationLine TransportationLine { get; set; }
        public int StationId { get; set; }
        public virtual Station Station { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public static int CompareByRoutePoint(TransportationLineRoute x, TransportationLineRoute y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater. 
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }

                return x.SequenceNo > y.SequenceNo ? 1 : -1;
            }

        }
    }
}
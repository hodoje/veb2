using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels
{
    // Checked
    public class Location
    {
        public Location()
        {
            Address = new Address();
        }

        public Location(double longitude, double latitude, Address address)
        {
            Longitude = longitude;
            Latitude = latitude;
            Address = address;
        }

        public int Id { get; set; }
        // X coord
        [Required]
        [Range(-180, 180)]
        public double Longitude { get; set; }
        // Y coord
        [Required]
        [Range(-90, 90)]
        public double Latitude { get; set; }
        public Address Address { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int StationId { get; set; }
        public Station Station { get; set; }
    }
}
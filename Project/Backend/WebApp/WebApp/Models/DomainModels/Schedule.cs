using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models.DomainModels
{
    // Checked
    public class Schedule
    {
        public int Id { get; set; }        
        public int DayOfTheWeekId { get; set; }
        public DayOfTheWeek DayOfTheWeek { get; set; }
        public string Timetable { get; set; }
        public int? TransportationLineId { get; set; }
        public TransportationLine TransportationLine { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

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
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
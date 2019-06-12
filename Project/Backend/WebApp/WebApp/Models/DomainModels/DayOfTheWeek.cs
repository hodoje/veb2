using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.DomainModels
{
    // Checked
    public class DayOfTheWeek
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
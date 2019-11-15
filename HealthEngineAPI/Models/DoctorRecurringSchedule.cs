using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Models
{
    public class DoctorRecurringSchedule
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }
        public string ToTime { get; set; }
        public string FromTime { get; set; }
        public bool IsMon { get; set; }
        public bool IsTues { get; set; }
        public bool IsWednes { get; set; }
        public bool IsThurs { get; set; }
        public bool IsFri { get; set; }
        public bool IsSatur { get; set; }
        public bool IsSun { get; set; }
        public DateTime RecordedAt { get; set; }
    }

    public class RecurringSchedulePostModel
    {
        public int Id { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public Weekdays[] Weekdays { get; set; }
    }

    public class Weekdays
    {
        public bool IsMon { get; set; }
        public bool IsTues { get; set; }
        public bool IsWednes { get; set; }
        public bool IsThurs { get; set; }
        public bool IsFri { get; set; }
        public bool IsSatur { get; set; }
        public bool IsSun { get; set; }
    }
}

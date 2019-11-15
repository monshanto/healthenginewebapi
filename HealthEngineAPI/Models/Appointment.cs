using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public DateTime Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int StatusId { get; set; }
    }

    public class AppointmentReschedule
    {
        public int Id { get; set; }
        public DateTime OldDate { get; set; }
        public string OldFromTime { get; set; }
        public string OldToTime { get; set; }
        public DateTime NewDate { get; set; }
        public string NewFromTime { get; set; }
        public string NewToTime { get; set; }
        public int AppointmentId { get; set; }
    }
}

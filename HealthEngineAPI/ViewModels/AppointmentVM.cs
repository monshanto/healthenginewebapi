using HealthEngineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.ViewModels
{
    public class AppointmentVM
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public DateTime Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int StatusId { get; set; }
    }

    public class PatientAppointmentVM
    {
        public int Age { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public int Gender { get; set; }
        public Appointment Appointment { get; set; }
    }
    public class AppointmentRescheduleVM
    {
        public int AppointmentId { get; set; }
        public string DoctorId { get; set; }
        public DateTime OldDate { get; set; }
        public string OldFromTime { get; set; }
        public int StatusId { get; set; }
        public DateTime NewDate { get; set; }
        public string NewFromTime { get; set; }
        public string NewToTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Models
{
    public class DoctorDetail
    {
        public int Id { get; set; }
        public int SpecialityId { get; set; }
        public int StateId { get; set; }
        public string LicenseNumber { get; set; }
        public string ClinicAddress { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AppointmentFee { get; set; }
        public string UserId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime RecordedAt { get; set; }
    }

    public class AppointmentRating
    {
        public int Id { get; set; }
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public int AppointmentId { get; set; }
        public int Rating { get; set; }
    }
}

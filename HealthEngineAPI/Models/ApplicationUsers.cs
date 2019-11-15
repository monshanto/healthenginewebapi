using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Models
{
    public class ApplicationUsers:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public int OTP { get; set; }
        public int SpecialityId { get; set; }
        public int StateId { get; set; }
        public string LicenseNumber { get; set; }
        public string ClinicAddress { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AppointmentFee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Height { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Weight { get; set; }
        public string BloodGroupId { get; set; }
        public string RefreshToken { get; set; }
        public string GraduatedFrom { get; set; }
        public string MasterIn { get; set; }
        public string MasterFrom { get; set; }
        public int Experience { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ReferralBonus { get; set; }
        public int TotalConsultations { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}

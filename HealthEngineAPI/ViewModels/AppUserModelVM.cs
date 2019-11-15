using HealthEngineAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.ViewModels
{
    public class AppUserModelVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DOB { get; set; }
        public int Gender { get; set; }
        public int StateId { get; set; }
        public string LicenseNumber { get; set; }
        public int SpecialityId { get; set; }
        public string SpecialityName { get; set; }
        public string ClinicAddress { get; set; }
        public decimal AppointmentFee { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string BloodGroupId { get; set; }
        public string GraduatedFrom { get; set; }
        public string MasterIn { get; set; }
        public string MasterFrom { get; set; }
        public int Experience { get; set; }
        public decimal ReferralBonus { get; set; }
        public int TotalConsultations { get; set; }
        public DoctorRecurringSchedule DoctorRecurringSchedule { get; set; }
    }

    public class AppDoctorModelVM
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password  { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string DOB { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        public int StateId { get; set; }

        [Required]
        public string LicenseNumber { get; set; }

        [Required]
        public int SpecialityId { get; set; }
    }

    public class AppPatientModelVM
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string DOB { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        public int StateId { get; set; }
    }
}

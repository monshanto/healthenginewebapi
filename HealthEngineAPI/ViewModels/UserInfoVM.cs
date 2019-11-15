using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.ViewModels
{
    public class UserInfoVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public string Token { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneConfirmed { get; set; }
    }

    public class DoctorSpecialityVM
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SpecialityName { get; set; }
        public int Rating { get; set; }
    }
}

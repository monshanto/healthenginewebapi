using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthEngineAPI.Models;
using HealthEngineAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HealthEngineAPI.Globals;
using HealthEngineAPI.ViewModels;
using Microsoft.Extensions.Options;

namespace HealthEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        #region Constructor & Fields

        private UserManager<ApplicationUsers> _userManager;
        private readonly ApplicationSettings _appSettings;
        private IDoctorService _doctorService;

        public DoctorController(UserManager<ApplicationUsers> userManager, IOptions<ApplicationSettings> appSettings, IDoctorService doctorService)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _doctorService = doctorService;
        }
        #endregion

        #region Api

        #region GetProfile
        [HttpGet]
        [Authorize]
        [Route("FinishDoctorProfile")]
        public async Task<IActionResult> FinishDoctorProfile()
        {
            try
            {
                string currentUserId = GlobalMethods.GetUserID(User);
                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return Unauthorized(new { status = false, message = "Unauthorized Access" });
                }
                var user = await _userManager.FindByIdAsync(currentUserId);
                if (user == null)
                {
                    return BadRequest(new { status = false, message = "User not exist" });
                }

                //Referral Bonus & TotalConsultations Logic to be implemented

                var applicationUser = new AppUserModelVM
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DOB = user.DOB,
                    Email = user.Email,
                    Gender = user.Gender,
                    SpecialityId = user.SpecialityId,
                    PhoneNumber = user.PhoneNumber,
                    StateId = user.StateId,
                    AppointmentFee = user.AppointmentFee,
                    ClinicAddress = user.ClinicAddress,
                    LicenseNumber = user.LicenseNumber,
                    GraduatedFrom = user.GraduatedFrom,
                    MasterIn = user.MasterIn,
                    MasterFrom = user.MasterFrom,
                    Experience = user.Experience,
                    ReferralBonus = user.ReferralBonus,
                    TotalConsultations = user.TotalConsultations
                };

                return Ok(new { status = true, message = "", appUser = applicationUser });
            }
            catch (Exception ae)
            {
                return BadRequest(new { status = false, message = ae.Message.ToString() });
            }
        }
        #endregion

        #region UpdateProfile
        [HttpPost]
        [Authorize]
        [Route("FinishDoctorProfile")]
        public async Task<IActionResult> FinishDoctorProfile(AppUserModelVM model)
        {
            try
            {
                string currentUserId = GlobalMethods.GetUserID(User);
                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return Unauthorized(new { status = false, message = "Unauthorized Access" });
                }
                var user = await _userManager.FindByIdAsync(currentUserId);

                //data will not be updated untill and unless changed phonenumber is verified
                if (user.PhoneNumber != model.PhoneNumber)
                {
                    return Ok(new { status = false, message = "Please verify PhoneNumber",isPhoneVerified = false });
                }
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Gender = model.Gender;
                user.DOB = model.DOB;
                user.SpecialityId = model.SpecialityId;
                user.ClinicAddress = model.ClinicAddress;
                user.PhoneNumber = model.PhoneNumber;
                user.AppointmentFee = model.AppointmentFee;
                user.StateId = model.StateId;
                user.LicenseNumber = model.LicenseNumber;
                user.GraduatedFrom = model.GraduatedFrom;
                user.MasterIn = model.MasterIn;
                user.MasterFrom = model.MasterFrom;
                user.Experience = model.Experience;
                //user.ReferralBonus = model.ReferralBonus;
                //user.TotalConsultations = model.TotalConsultations;

                IdentityResult result = await _userManager.UpdateAsync(user);
                
                if (!result.Succeeded)
                {
                    return Ok(new { status = false, message = result.Errors.First().Description });
                }
                return Ok(new { status = true, message = "Updated successfully" });
            }
            catch (Exception ae)
            {
                return BadRequest(new { status = false, message = ae.Message.ToString() });
            }
        }
        #endregion

        #region Get Doctor Schedule
        [HttpGet]
        [Authorize(Roles = "Doctor")]
        [Route("GetRecurringSchedule")]
        public object GetRecurringSchedule()
        {
            try
            {
                string userId = GlobalMethods.GetUserID(User);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ResponseData { Status = false, Message = "Unauthorized Access" };
                }
                var scheduleList = _doctorService.GetRecurringSchedulesById(userId);
                return scheduleList;
            }
            catch(Exception ae)
            {
                return BadRequest(new { status = false, message = ae.Message.ToString() });
            }
        }
        #endregion

        #region Create Schedule
        [HttpPost]
        [Authorize(Roles = "Doctor")]
        [Route("CreateSchedule")]
        public ResponseData CreateRecurringSchedule(RecurringSchedulePostModel model)
        {
            try
            {
                string userId = GlobalMethods.GetUserID(User);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ResponseData { Status = false, Message = "Unauthorized Access" };
                }
                return _doctorService.CreateRecurringScheduleById(userId, model);
            }
            catch(Exception ae)
            {
                return new ResponseData { Status = false, Message = ae.Message.ToString() };
            }
        }
        #endregion

        #region Delete Schedule
        [HttpDelete]
        //[Authorize]
        [Route("DeleteSchedule")]
        public ResponseData DeleteSchedule(int Id)
        {
            string currentUserId = GlobalMethods.GetUserID(User);
            try
            {
                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return new ResponseData { Status = false, Message = "Unauthorized Access" };
                }
                return _doctorService.DeleteRecurringScheduleById(Id, currentUserId);
            }
            catch (Exception ae)
            {
                return new ResponseData { Status = true, Message = ae.Message.ToString() };
            }
            
        }
        #endregion

        #region DoctorList
        [HttpPost]
        [Route("GetDoctorsSpeciality")]
        public async Task<IActionResult> GetDoctorsSpeciality(QueryParamsModel model)
        {
            try
            {
                IEnumerable<DoctorSpecialityVM> doctorSpecialities = await _doctorService.GetDoctorBySpeciality(model);
                return Ok(doctorSpecialities);
            }
            catch (Exception ae)
            {
                return BadRequest(new { status = false, message = ae.Message.ToString() });
            }
        }
        #endregion

        #region DoctorProfileById
        [HttpPost]
        [Route("DoctorProfileById")]
        public async Task<IActionResult> GetDoctorProfileById(string Id)
        {
            try
            {
                AppUserModelVM doctorDetails = await _doctorService.GetDoctorById(Id);
                return Ok(new { status = true, doctorDetails });
            }
            catch (Exception ae)
            {
                return BadRequest(new { status = false, message = ae.Message.ToString()});
            }
        }
        #endregion

        #endregion
    }
}
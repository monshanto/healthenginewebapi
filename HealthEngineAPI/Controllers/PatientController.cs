using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthEngineAPI.Globals;
using HealthEngineAPI.Models;
using HealthEngineAPI.Services;
using HealthEngineAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HealthEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        #region Constructor & Fields

        private UserManager<ApplicationUsers> _userManager;
        private readonly ApplicationSettings _applicationSettings;
        private IPatientService _patientService;

        public PatientController(UserManager<ApplicationUsers> userManager,IOptions<ApplicationSettings> appSettings,IPatientService patientService)
        {
            _userManager = userManager;
            _applicationSettings = appSettings.Value;
            _patientService = patientService;
        }
        #endregion

        #region Api

        #region BloodGroup
        [HttpGet]
        [Route("GetBloodGroup")]
        public IEnumerable<SelectListModel> GetBloodGroup()
        {
            return _patientService.GetBloodGroup();
        }
        #endregion

        #region GetProfile
        [HttpGet]
        [Authorize]
        [Route("FinishPatientProfile")]
        public async Task<IActionResult> FinishPatientProfile()
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

                var applicationUser = new AppUserModelVM
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DOB = user.DOB,
                    Email = user.Email,
                    Gender = user.Gender,
                    PhoneNumber = user.PhoneNumber,
                    StateId = user.StateId,
                    Height = user.Height,
                    Weight = user.Weight,
                    BloodGroupId = user.BloodGroupId
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
        [Route("FinishPatientProfile")]
        public async Task<IActionResult> FinishPatientProfile(AppUserModelVM model)
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
                    return Ok(new { status = false, message = "Please verify PhoneNumber", isPhoneVerified = false });
                }
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Gender = model.Gender;
                user.DOB = model.DOB;
                user.PhoneNumber = model.PhoneNumber;
                user.StateId = model.StateId;
                user.Height = model.Height;
                user.Weight = model.Weight;
                user.BloodGroupId = model.BloodGroupId;

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

        

        #endregion
    }
}
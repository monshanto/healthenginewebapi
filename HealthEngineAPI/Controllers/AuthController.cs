using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthEngineAPI.Globals;
using HealthEngineAPI.Models;
using HealthEngineAPI.Services;
using HealthEngineAPI.ViewModels;
using HealthEngineAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;


namespace HealthEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Constructor & Fields
        private UserManager<ApplicationUsers> _userManager;
        private readonly ApplicationSettings _appSettings;
        private IDoctorService _doctorService;
        private IPatientService _patientService;

        public AuthController(UserManager<ApplicationUsers> userManager, IOptions<ApplicationSettings> appSettings, IDoctorService doctorService, IPatientService patientService)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _doctorService = doctorService;
            _patientService = patientService;
        }
        #endregion

        #region Api

        #region SignUpDoctor
        [HttpPost]
        [Route("DSignUp")]
        public async Task<IActionResult> RegisterDoctor(AppDoctorModelVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = false, message = "Parameters sent are invalid" });
            }
            try
            {
                UserInfoVM userInfo = new UserInfoVM();
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    userInfo.IsEmailConfirmed = user.EmailConfirmed;
                    userInfo.IsPhoneConfirmed = user.PhoneNumberConfirmed;

                    return BadRequest(new { status = false, message = "You are already registered with this Email", userInfo });
                }

                var applicationUser = new ApplicationUsers
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    PhoneNumber = model.PhoneNumber,
                    DOB = model.DOB,
                    Gender = model.Gender,
                    SpecialityId = model.SpecialityId,
                    StateId = model.StateId,
                    LicenseNumber = model.LicenseNumber,
                    RecordedAt = DateTime.Now
                };

                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                //add role
                await _userManager.AddToRoleAsync(applicationUser, GlobalVariables.isDoctor);
                if (!result.Succeeded)
                {
                    return BadRequest(new
                    {
                        status = false,
                        message = result.Errors.First().Code
                    });
                }
                userInfo.IsEmailConfirmed = false;
                userInfo.IsPhoneConfirmed = false;

                return Ok(new { status = true, message = "Registered Successfully", userInfo });
            }
            catch (Exception ae)
            {
                return BadRequest(new { status = false, message = ae.Message.ToString() });
            }
        }

        #endregion

        #region SignUpPatient
        [HttpPost]
        [Route("PSignUp")]
        public async Task<IActionResult> RegisterPatient(AppPatientModelVM model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new { status = false, message = "The parameters are not correct" });
            }
            try
            {
                UserInfoVM userInfo = new UserInfoVM();

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    userInfo.IsEmailConfirmed = user.EmailConfirmed;
                    userInfo.IsPhoneConfirmed = user.PhoneNumberConfirmed;

                    return BadRequest(new { status = false, message = "You are already registered with this email." });
                }
                int otpCode = GlobalMethods.GenerateOTP();

                var applicationUser = new ApplicationUsers
                {
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    PhoneNumber = model.PhoneNumber,
                    DOB = model.DOB,
                    Gender = model.Gender,
                    OTP = otpCode,
                    StateId = model.StateId
                };
                var result = await _userManager.CreateAsync(applicationUser, model.Password);
                //add role
                await _userManager.AddToRoleAsync(applicationUser, GlobalVariables.isPatient);
                if (!result.Succeeded)
                {
                    return BadRequest(new { status = false, message = result.Errors.First().Code });
                }

                userInfo.IsEmailConfirmed = false;
                userInfo.IsPhoneConfirmed = false;

                return BadRequest(new { status = true, message = "Registered successfully", userInfo });
            }
            catch (Exception ae)
            {
                return BadRequest(new
                {
                    status = false,
                    message = ae.Message.ToString()
                });
            }
        }
        #endregion

        #region LogIn
        [HttpPost]
        [Route("Login")]
        //POST : /api/auth/Login
        public async Task<IActionResult> Login(LoginVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Ok(new { status = false, message = "The parameters are not correct" });
                }
                var user = await _userManager.FindByEmailAsync(model.UserName);
                if (user == null)
                {
                    return NotFound(new { status = false, message = "Sorry,Could not found the account with " + model.UserName });
                }
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    //check email confirmation
                    if ((!_userManager.IsEmailConfirmedAsync(user).Result) || (!_userManager.IsPhoneNumberConfirmedAsync(user).Result))
                    {
                        //changes to be made in responsecode
                        //return Ok(new { status = false, message = "Email is not confirmed" });
                    }

                    // change the security stamp only on correct username/password
                    await _userManager.UpdateSecurityStampAsync(user);
                    //get role assigned to the user
                    var roles = await _userManager.GetRolesAsync(user);
                    var accessToken = GlobalMethods.GenerateAccessToken(user, roles.FirstOrDefault(), _appSettings);

                    //create refresh token
                    user.RefreshToken = GlobalMethods.GenerateRefreshToken();

                    UserInfoVM userInfo = new UserInfoVM()
                    {
                        Id = user.Id,
                        Name = user.FirstName + " " + user.LastName,
                        Email = user.Email,
                        AccessToken = accessToken,
                        UserRole = roles.FirstOrDefault(),
                        IsEmailConfirmed = user.EmailConfirmed,
                        IsPhoneConfirmed = user.PhoneNumberConfirmed
                    };

                    await _userManager.UpdateAsync(user);
                    return Ok(new { status = true, message = "successfull login", userInfo });
                }

                else
                {
                    return Unauthorized(new { status = false, message = "username/password is incorrect" });
                }
            }
            catch (Exception ae)
            {
                return BadRequest(new { status = false, message = ae.Message.ToString() });
            }
        }

        #endregion

        #region ChangePassword

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = false, message = "Parameters are not correct" });
            }
            try
            {
                //get userId from access Token
                string userId = User.Claims.First(x => x.Type == "UserId").Value;
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, changePasswordVM.OldPassword, changePasswordVM.NewPassword);
                    if (!result.Succeeded)
                    {
                        return Ok(new { status = false, message = result.Errors.First().Description });
                    }
                    return Ok(new { status = true, message = "Password changed successfully" });
                }
                else
                {
                    return Ok(new { status = false, message = "Sorry we couldn't found user accociated with this account" });
                }
            }
            catch (Exception ae)
            {
                return BadRequest(new { status = false, message = ae.Message.ToString() });
            }
        }
        #endregion

        #region ForgotPassword

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = false, message = "Parameters are not correct" });
            }
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    //check if associated account is confirmed by linked email/phone number
                    if ((!_userManager.IsEmailConfirmedAsync(user).Result) || (!_userManager.IsPhoneNumberConfirmedAsync(user).Result))
                    {
                        return Ok(new { status = false, message = "Account is not verified." });
                    }
                    int otpCode = GlobalMethods.GenerateOTP();
                    user.OTP = otpCode;
                    user.EmailConfirmed = false;

                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        return BadRequest(new { status = false, message = result.Errors.First().Description });
                    }
                    //send otp via email

                    //----------SendGrid to be implemented----------//

                    ResponseData responseData = new ResponseData();


                    //----------EndSendGridLogic----------//

                    MessageSender messageSender = new MessageSender();

                    return Ok(new { status = true, message = "OTP sent to your registered Email" });
                }
                else
                {
                    return Ok(new { status = false, message = "Sorry,Could not found the account with " + model.Email });
                }
            }
            catch (Exception ae)
            {
                return BadRequest(new { status = false, message = ae.Message.ToString() });
            }
        }
        #endregion

        #region SendOTP

        [HttpPost]
        [Route("SendOTP")]
        public async Task<IActionResult> SendOTP(int id, string email)
        {
            ResponseData response = new ResponseData();
            try
            {
                var currentUser = await _userManager.FindByEmailAsync(email);
                if (currentUser != null)
                {
                    int otpCode = GlobalMethods.GenerateOTP();
                    currentUser.OTP = otpCode;

                    IdentityResult result = await _userManager.UpdateAsync(currentUser);
                    EmailSender emailSender = new EmailSender();
                    MessageSender sender = new MessageSender();
                    if (id == 1) //email
                    {
                        //Send Grid Logic
                        await emailSender.SendEmail(email, "Verify Account", "Your Verification Code is <b>"+ otpCode + "</b>");
                        //End SendGrid
                        //if (!response.Status)
                        //{
                        //    return BadRequest(new { status = false, message = "We couldn't reach you.Please try again!" });
                        //}
                        //return Ok(new { status = true, message = "OTP sent!" });
                    }
                    else if (id == 2) //phone number
                    {
                        response = sender.SendSMS(currentUser.PhoneNumber, otpCode);
                        //if (!response.Status)
                        //{
                        //    return BadRequest(new { status = false, message = "We couldn't reach you.Please try again!" });
                        //}
                        //return Ok(new { status = true, message = "OTP sent!" });
                    }
                }

                return Ok(new { status = false, message = "Sorry could not found the account" });
            }
            catch (Exception ae)
            {
                return BadRequest(new { status = false, message = ae.Message.ToString() });
            }
        }
        #endregion

        #region ResendOTP

        [HttpPost]
        [Route("ResendCode")]
        public async Task<IActionResult> ResendOTP(int id, string email)
        {
            try
            {
                ResponseData response = new ResponseData();
                var currentUser = await _userManager.FindByEmailAsync(email);
                if (currentUser == null)
                {
                    return Ok(new { status = false, message = "Sorry could not found the account" });
                }

                currentUser.OTP = GlobalMethods.GenerateOTP();
                IdentityResult result = await _userManager.UpdateAsync(currentUser);
                if (!result.Succeeded)
                {
                    return Ok(new { status = false, message = result.Errors.First().Description });
                }
                EmailSender emailSender = new EmailSender();
                MessageSender sender = new MessageSender();

                if (id == 1) //email
                {
                    //Send Grid Logic
                    await emailSender.SendEmail(email, "Verify Account", "Your Verification Code is <b>" + currentUser.OTP + "</b>");
                    //End SendGrid
                    //if (!response.Status)
                    //{
                    //    return BadRequest(new { status = false, message = "We couldn't reach you.Please try again!" });
                    //}
                    //return Ok(new { status = true, message = "OTP sent!" });
                }
                else if (id == 2) //phone number
                {
                    response = sender.SendSMS(currentUser.PhoneNumber, currentUser.OTP);
                    //if (!response.Status)
                    //{
                    //    return BadRequest(new { status = false, message = "We couldn't reach you.Please try again!" });
                    //}
                    //return Ok(new { status = true, message = "OTP sent!" });
                }

                return Ok(new { status = true, message = "OTP has been re-sent to your registered Email and Phone Number" });
            }
            catch (Exception ae)
            {
                return Ok(new { status = false, message = ae.Message.ToString() });
            }
        }
        #endregion

        #region Verify Email/Phone

        [HttpPost]
        [Route("Verification")]
        public async Task<IActionResult> Verification(int id, string email, int code)
        {
            try
            {
                IdentityResult result = new IdentityResult();
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return Ok(new { status = false, message = "Sorry,Couldn't found the account with " + email });
                }

                if (id == 1)
                {
                    if (user.OTP == code)
                    {
                        user.EmailConfirmed = true;
                        result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            return Ok(new { status = false, message = "Sorry, email is not verified" });
                        }
                        return Ok(new { status = true, message = "Email has been verified." });
                    }
                    else
                    {
                        return Ok(new { status = false, message = "OTP is incorrect" });
                    }
                }
                else if (id == 2)
                {
                    if (user.OTP == code)
                    {
                        user.PhoneNumberConfirmed = true;
                        result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            return Ok(new { status = false, message = "Sorry, PhoneNumber is not verified" });
                        }
                        return Ok(new { status = true, message = "PhoneNumber has been verified." });
                    }
                    else
                    {
                        return Ok(new { status = false, message = "OTP is incorrect" });
                    }
                }
                else
                {
                    return BadRequest(new { status = false, message = "Something went wrong!" });
                }
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
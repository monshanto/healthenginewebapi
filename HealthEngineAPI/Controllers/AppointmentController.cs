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
    public class AppointmentController : ControllerBase
    {
        #region Constructor & Fields
        private UserManager<ApplicationUsers> _userManager;
        private readonly ApplicationSettings _appSettings;
        private IAppointmentService _appointmentService;

        public AppointmentController(UserManager<ApplicationUsers> userManager, IOptions<ApplicationSettings> appSettings, IAppointmentService appointmentService)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _appointmentService = appointmentService;
        }
        #endregion

        #region Api

        #region BookAppointment
        [HttpPost]
        //[Authorize]
        [Route("BookAppointment")]
        public async Task<IActionResult> BookAppointment(AppointmentVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { status = false, message = "Parameters are not correct." });
            }
            try
            {
                var response=_appointmentService.AppointmentBooking(model);

                return Ok(new { status = response.Status, message = response.Message });
            }
            catch (Exception ae)
            {
                return BadRequest(new { status = false, message = ae.Message.ToString() });
            }
        }
        #endregion

        #region AppointmentRequests
        [HttpGet]
        [Authorize]
        [Route("GetAppointmentRequest")]
        public async Task<IActionResult> GetAppointmentRequest()
        {
            string userId = GlobalMethods.GetUserID(User);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new { status = false, message = "Unauthorized Access" });
            }
            var requests = _appointmentService.GetAppointmentRequestById(userId);
            return Ok(new { status = true, message = "", requests });
        }

        #endregion

        #region PatientProfileById
        [HttpPost]
        [Route("GetPatientById")]
        public async Task<IActionResult> GetPatientById(string patientId)
        {
            try
            {
                var patient = _appointmentService.GetPatientById(patientId);
                return Ok(new { status = true, message = "", patient });
            }
            catch (Exception ae)
            {
                return Ok(new { status = false, message = ae.Message.ToString() });
            }
        }

        #endregion

        #region Confirm/Cancel Appointment
        [HttpPost]
        [Route("AppointmentResponseByDoctor")]
        public ResponseData AppointmentResponseByDoctor(int appId, int statusId)
        {
            try
            {
                if (statusId == GlobalVariables.isConfirmed)
                {
                    return _appointmentService.ConfirmAppointmentById(appId);
                }
                else if (statusId == GlobalVariables.isCancelled)
                {
                    return _appointmentService.CancelAppointmentById(appId);
                }
                return new ResponseData { Status = false, Message = "Something went wrong!" };
            }
            catch (Exception ae)
            {
                return new ResponseData { Status = false, Message = ae.Message.ToString() };
            }
        }
        #endregion

        #region RescheduleAppointment
        [HttpPost]
        [Authorize(Roles = "Patient")]
        [Route("RescheduleAppointment")]
        public ResponseData RescheduleAppointment(AppointmentRescheduleVM model)
        {
            try
            {
                string userId = GlobalMethods.GetUserID(User);
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new ResponseData { Status = false, Message = "Unauthorized Access" };
                }
                return _appointmentService.RescheduleAppointmentById(model);
            }
            catch (Exception ae)
            {
                return new ResponseData { Status = false, Message = ae.Message.ToString() };
            }
        }
        #endregion

        #endregion

    }
}
using HealthEngineAPI.Models;
using HealthEngineAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Services
{
    public interface IAppointmentService
    {
        ResponseData AppointmentBooking(AppointmentVM model);
        List<Appointment> GetAppointmentRequestById(string Id);
        PatientAppointmentVM GetPatientById(string Id);
        ResponseData ConfirmAppointmentById(int appId);
        ResponseData CancelAppointmentById(int appId);
        ResponseData RescheduleAppointmentById(AppointmentRescheduleVM model);
    }
}

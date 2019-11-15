using Dapper;
using Dapper.Contrib.Extensions;
using HealthEngineAPI.Globals;
using HealthEngineAPI.Models;
using HealthEngineAPI.Services;
using HealthEngineAPI.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Repository
{
    public class AppointmentRepository : IAppointmentService
    {
        public IConfiguration Configuration { get; set; }
        public AppointmentRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(Configuration.GetConnectionString("DefaultConnectionString"));
            }
        }

        public ResponseData AppointmentBooking(AppointmentVM model)
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                try
                {
                    var appointment = new Appointment
                    {
                        DoctorId = model.DoctorId,
                        PatientId = model.PatientId,
                        Date = model.Date,
                        FromTime = model.FromTime,
                        ToTime = model.ToTime,
                        StatusId = GlobalVariables.isPending
                    };
                    var id = con.Insert<Appointment>(appointment);
                    return new ResponseData { Status = true, Message = "Appointment booked successfully" };
                }
                catch (Exception ae)
                {
                    return new ResponseData { Status = false, Message = ae.Message.ToString() };
                }
            }
        }

        public List<Appointment> GetAppointmentRequestById(string userId)
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                string sql = @"select * from Appointments where DoctorId = @DoctorId and StatusId IN (3,4,5)";

                try
                {
                    List<Appointment> appointments = con.Query<Appointment>(sql, new { @DoctorId = userId }).ToList();
                    return appointments;
                }
                catch (Exception ae)
                {
                    throw ae;
                }
            }
        }

        public PatientAppointmentVM GetPatientById(string patientId)
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                PatientAppointmentVM model = new PatientAppointmentVM();
                try
                {
                    string sql = @"select * from AspNetUsers as Asp inner join Appointments as a on a.PatientId=Asp.Id where Asp.Id=@Id";
                    var patientDetails = con.Query<ApplicationUsers, Appointment, PatientAppointmentVM>(sql, (p, a) =>
                    {
                        model.Age = p.Age;
                        model.Height = p.Height;
                        model.Weight = p.Weight;
                        model.Gender = p.Gender;
                        model.Appointment = a;
                        return model;
                    }, new
                    {
                        @Id = patientId
                    }, splitOn: "Id").FirstOrDefault();
                    return patientDetails;
                }
                catch (Exception ae)
                {
                    throw ae;
                }
            }
        }

        public ResponseData ConfirmAppointmentById(int appId)
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                try
                {
                    ResponseData response = new ResponseData();
                    var id = con.ExecuteScalar("update Appointments set StatusId = @StatusId where Id = @Id and PatientId = @PatientId", new
                    {
                        @StatusId = GlobalVariables.isConfirmed,
                        @Id = appId
                    });
                    return new ResponseData { Status = true, Message = "Appointment Confirmed" };
                }
                catch (Exception ae)
                {
                    return new ResponseData { Status = false, Message = ae.Message };
                }
            }
        }

        public ResponseData CancelAppointmentById(int appId)
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                try
                {
                    ResponseData response = new ResponseData();
                    var id = con.ExecuteScalar("update Appointments set StatusId = @StatusId where Id = @Id and PatientId = @PatientId", new
                    {
                        @StatusId = GlobalVariables.isCancelled,
                        @Id = appId,
                    });
                    return new ResponseData { Status = true, Message = "Appointment Confirmed" };
                }
                catch (Exception ae)
                {
                    return new ResponseData { Status = false, Message = ae.Message.ToString() };
                }
            }
        }

        public ResponseData RescheduleAppointmentById(AppointmentRescheduleVM model)
        {
            using(IDbConnection con = Connection)
            {
                con.Open();
                try
                {
                    ResponseData response = new ResponseData();
                    return response;
                }
                catch(Exception ae)
                {
                    return new ResponseData { Status = false, Message = ae.Message.ToString() };
                }
            }
        }
    }
}

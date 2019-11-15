using Dapper;
using Dapper.Contrib.Extensions;
using HealthEngineAPI.Models;
using HealthEngineAPI.Services;
using HealthEngineAPI.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HealthEngineAPI.Repository
{
    public class DoctorRepository : IDoctorService
    {
        public IConfiguration Configuration { get; }

        public DoctorRepository(IConfiguration configuration)
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

        public async Task<AppUserModelVM> GetDoctorById(string doctorId)
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                try
                {
                    AppUserModelVM appUserModel = new AppUserModelVM();
                    string sql = "select Asp.FirstName,Asp.LastName,Asp.GraduatedFrom,Asp.MasterIn,Asp.MasterFrom,Asp.Experience,Asp.ReferralBonus,Asp.TotalConsultations,Asp.AppointmentFee,s.*,dsr.* from AspNetUsers as Asp inner join Specialities as s on s.SpecialitieId = Asp.SpecialityId inner join DoctorRecurringSchedules as dsr on dsr.DoctorId = Asp.Id where Asp.Id = @Id";
                    var appUser = con.Query<ApplicationUsers,Specialitie, DoctorRecurringSchedule, AppUserModelVM>(
            sql,
            (user, spec, doctorRecurringDetails) =>
            {
                appUserModel.AppointmentFee = user.AppointmentFee;
                appUserModel.Experience = user.Experience;
                appUserModel.FirstName = user.FirstName;
                appUserModel.LastName = user.LastName;
                appUserModel.MasterIn = user.MasterIn;
                appUserModel.MasterFrom = user.MasterFrom;
                appUserModel.TotalConsultations = user.TotalConsultations;
                appUserModel.GraduatedFrom = user.GraduatedFrom;
                appUserModel.SpecialityName = spec.SpecialityName;
                appUserModel.DoctorRecurringSchedule = doctorRecurringDetails;
                return appUserModel;
            }, new
            {
                @Id = doctorId
            },
            splitOn: "SpecialitieId,Id")
        .FirstOrDefault();
                    return appUser;
                }
                catch (Exception ae)
                {
                    throw ae;
                }
            }
        }

        public DoctorRecurringSchedule GetRecurringSchedulesById(string userId)
        {
            using (IDbConnection con = Connection)
            {
                try
                {
                    con.Open();
                    DoctorRecurringSchedule doctorRecurrings = new DoctorRecurringSchedule();
                    string sql = @"select * from DoctorRecurringSchedules where DoctorId=@DoctorId and
 RecordedAt between DATEADD(MONTH, -1, GETDATE()) and GETDATE()";

                    doctorRecurrings = con.Query<DoctorRecurringSchedule>(sql, new
                    {
                        @DoctorId = userId,
                    }).FirstOrDefault();
                    return doctorRecurrings;
                }
                catch (Exception ae)
                {
                    throw ae;
                }
            }
        }

        public ResponseData CreateRecurringScheduleById(string DoctorId, RecurringSchedulePostModel model)
        {
            using (IDbConnection con = Connection)
            {
                string message = string.Empty;
                try
                {
                    con.Open();
                    string sql = @"select * from DoctorRecurringSchedules where DoctorId = @userId and
   RecordedAt between DATEADD(month, -1, GETDATE()) and GETDATE()";
                    //                    string sql = @"select * from DoctorRecurringSchedules where DoctorId=@userId and FromTime between @From and @To
                    //or ToTime between @From and @To";
                    var check = con.Query<DoctorRecurringSchedule>(sql, new
                    {
                        @From = model.FromTime,
                        @To = model.ToTime,
                        @userId = DoctorId
                    }).ToList();
                    if (check.Count > 0)
                    {
                        List<string> obj = new List<string>();
                        //already scheduled
                        foreach (var item in check)
                        {
                            foreach (PropertyInfo pi in item.GetType().GetProperties())
                            {
                                if (pi.PropertyType == typeof(bool))
                                {
                                    bool value = (bool)pi.GetValue(item);
                                    if (value)
                                    {
                                        obj.Add((pi.Name.Remove(0, 2)) + "day");
                                    }
                                }
                            }
                            message = string.Join(",", obj.ToArray()) + " are already been scheduled at this timing";
                        }
                        return new ResponseData { Status = true, Message = message };
                    }
                    else
                    {
                        //create new schedule
                        string query = @"Insert into DoctorRecurringSchedules Values(@DoctorId,@FromTime,@ToTime,@IsMon,@IsTues,@IsWednes,@IsThurs,@IsFri,@IsSatur,@IsSun,@RecordedAt)SELECT SCOPE_IDENTITY()";

                        for (int i = 0; i < model.Weekdays.Length; i++)
                        {
                            int insertedId = con.Query<int>(query, new { DoctorId, model.FromTime, model.ToTime, model.Weekdays[i].IsMon, model.Weekdays[i].IsTues, model.Weekdays[i].IsWednes, model.Weekdays[i].IsThurs, model.Weekdays[i].IsFri, model.Weekdays[i].IsSatur, model.Weekdays[i].IsSun, @RecordedAt = DateTime.Now }).FirstOrDefault();
                        }

                        return new ResponseData { Status = true, Message = "Schedule has been created." };
                    }
                }
                catch (Exception ae)
                {
                    return new ResponseData { Status = false, Message = ae.Message.ToString() };
                }
            }
        }

        public ResponseData DeleteRecurringScheduleById(int Id, string doctorId)
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                try
                {
                    con.ExecuteScalar(@"Delete from DoctorRecurringSchedules Where Id = @Id AND DoctorId = @DoctorId", new { Id, @DoctorId = doctorId });
                    return new ResponseData { Status = true, Message = "deleted successfully" };
                }
                catch (Exception ae)
                {
                    return new ResponseData { Status = false, Message = ae.Message.ToString() };
                }

            }
        }

        public async Task<IEnumerable<DoctorSpecialityVM>> GetDoctorBySpeciality(QueryParamsModel model)
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                try
                {
                    return await con.QueryAsync<DoctorSpecialityVM>("GetDoctors", new
                    {
                        @SearchValue = model.searchValue,
                        @PageSize = model.pageSize,
                        @PageNo = model.pageNumber,
                        @SortColumn = model.sortField,
                        @SortOrder = model.sortOrder
                    }, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ae)
                {
                    throw ae;
                }
            }
        }

    }
}

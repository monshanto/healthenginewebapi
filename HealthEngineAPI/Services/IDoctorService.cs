using HealthEngineAPI.Models;
using HealthEngineAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Services
{
    public interface IDoctorService
    {
        //ResponseData RegisterDoctor(DoctorDetail model);
        Task<AppUserModelVM> GetDoctorById(string Id);
        DoctorRecurringSchedule GetRecurringSchedulesById(string Id);
        ResponseData CreateRecurringScheduleById(string Id, RecurringSchedulePostModel model);
        ResponseData DeleteRecurringScheduleById(int Id, string userId);
        Task<IEnumerable<DoctorSpecialityVM>> GetDoctorBySpeciality(QueryParamsModel model);

    }
}

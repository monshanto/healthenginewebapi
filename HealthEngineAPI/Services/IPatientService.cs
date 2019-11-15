using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthEngineAPI.Models;
using HealthEngineAPI.Services;
using HealthEngineAPI.Repository;
using HealthEngineAPI.ViewModels;

namespace HealthEngineAPI.Services
{
    public interface IPatientService
    {
        ResponseData RegisterPatient(PatientDetail detail);
        IEnumerable<SelectListModel> GetBloodGroup();
    }
}

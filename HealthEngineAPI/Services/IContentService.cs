using HealthEngineAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Services
{
    public interface IContentService
    {
        Task<IEnumerable<Specialitie>> GetAllSpecialities(QueryParamsModel model);
        IEnumerable<SelectListModel> GetAllStates();
    }
}

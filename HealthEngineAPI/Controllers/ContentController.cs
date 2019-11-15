using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthEngineAPI.Models;
using HealthEngineAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthEngineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private UserManager<ApplicationUsers> _userManager;
        private IContentService _contentService;

        public ContentController(UserManager<ApplicationUsers> userManager,IContentService contentService)
        {
            _userManager = userManager;
            _contentService = contentService;
        }

        #region Specialities

        [HttpPost]
        [Route("GetSpecialities")]
        public async Task<Object> GetSpecialities(QueryParamsModel model)
        {
            IEnumerable<Specialitie> specialities = await _contentService.GetAllSpecialities(model);
            return Ok(specialities);
        }

        #endregion

        #region States
        [HttpGet]
        [Route("GetStates")]
        public IEnumerable<SelectListModel> GetStates()
        {
            return _contentService.GetAllStates();
        }
        #endregion
    }
}
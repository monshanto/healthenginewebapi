using Dapper;
using HealthEngineAPI.Models;
using HealthEngineAPI.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Repository
{
    public class ContentRepository:IContentService
    {
        public IConfiguration Configuration { get; set; }

        public ContentRepository(IConfiguration configuration)
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

        public async Task<IEnumerable<Specialitie>> GetAllSpecialities(QueryParamsModel model)
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                try
                {
                    var specialitieDictionary = new Dictionary<int, Specialitie>();
                    IEnumerable<Specialitie> specialitieObj = con.Query<Specialitie, SubSpecialitie, Specialitie>(
                        "GetDoctorAvailability", (spec, sub) =>
                        {
                            Specialitie specialitie;

                            if (!specialitieDictionary.TryGetValue(spec.SpecialitieId, out specialitie))
                            {
                                specialitie = spec;
                                specialitie.SubSpecialities = new List<SubSpecialitie>();
                                specialitieDictionary.Add(specialitie.SpecialitieId, specialitie);
                            }

                            specialitie.SubSpecialities.Add(sub);
                            return specialitie;
                        }, new {
                            @SearchValue = model.searchValue,
                            @PageNo = model.pageNumber == 0 ? 1 : model.pageNumber,
                            @PageSize = model.pageSize,
                            @SortColumn = model.sortField,
                            @SortOrder = model.sortOrder
                        },
                        splitOn: "Id", commandType: CommandType.StoredProcedure).Distinct().ToList();// split on set | tables id which has many records

                    return specialitieObj;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public IEnumerable<SelectListModel> GetAllStates()
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                return con.Query<SelectListModel>(@"select Id as Id,StateName as [Text] from States");
            }
        }
    }
}

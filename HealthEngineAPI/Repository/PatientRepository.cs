using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using HealthEngineAPI.Models;
using HealthEngineAPI.Services;
using HealthEngineAPI.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HealthEngineAPI.Repository
{
    public class PatientRepository: IPatientService
    {
        public IConfiguration Configuration { get; }
        public PatientRepository (IConfiguration configuration)
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

        //Patient Registration

        public ResponseData RegisterPatient(PatientDetail patient)
        {
            using(IDbConnection con = Connection)
            {
                con.Open();
                try
                {
                    //patient insertion logic
                    var id = con.Insert<PatientDetail>(patient);
                    return new ResponseData { Status = true, Message = "You are registered successffully!" };
                }
                catch(Exception ae)
                {
                    return new ResponseData { Status = false, Message = ae.Message.ToString() };
                }
            }
        }

        public IEnumerable<SelectListModel> GetBloodGroup()
        {
            using (IDbConnection con = Connection)
            {
                con.Open();
                return con.Query<SelectListModel>(@"select Id,Name from BloodGroups");
            }
        }

    }
}

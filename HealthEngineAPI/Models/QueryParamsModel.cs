using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Models
{
    public class QueryParamsModel
    {
        //Query Parameters for listings
        //public FilterParam filter { get; set; }
        public string searchValue { get; set; }
        public string sortOrder { get; set; }
        public string sortField { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}

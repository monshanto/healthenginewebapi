using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Models
{
    public class CommonContent
    {
    }

    public class State
    {
        public int Id { get; set; }
        public string StateName { get; set; }
    }

    public class Specialitie
    {
        public int SpecialitieId { get; set; }
        public string SpecialityName { get; set; }
        public ICollection<SubSpecialitie> SubSpecialities { get; set; }
    }

    public class SubSpecialitie
    {
        public int Id { get; set; }
        public string SubSpecialityName { get; set; }
        public int SpecialitieId { get; set; }
        public Specialitie Specialities { get; set; }
    }

    public class SelectListModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class SpecialitieList
    {
        public int SpecialitieId { get; set; }
        public string SpecialityName { get; set; }
        public IEnumerable<SubSpecialitie> SubSpecialities { get; set; }
    }
}

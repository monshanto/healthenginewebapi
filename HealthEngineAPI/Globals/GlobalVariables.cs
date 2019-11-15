using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthEngineAPI.Globals
{
    public static class GlobalVariables
    {
        public static readonly string isAdmin = "Admin";
        public static readonly string isDoctor = "Doctor";
        public static readonly string isPatient = "Patient";
        public static readonly int isCompleted = 1;
        public static readonly int isCancelled = 2;
        public static readonly int isPending = 3;
        public static readonly int isReschedule = 4;
        public static readonly int isConfirmed = 5;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthEngineAPI.Models
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUsers>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<DoctorDetail> DoctorDetails { get; set; }
        public DbSet<PatientDetail> PatientDetails { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Specialitie> Specialities { get; set; }
        public DbSet<SubSpecialitie> SubSpecialities { get; set; }
        public DbSet<AppointmentRating> AppointmentRatings { get; set; }
        public DbSet<BloodGroup> BloodGroups { get; set; }
        public DbSet<DoctorRecurringSchedule> DoctorRecurringSchedules { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentReschedule> AppointmentReschedules { get; set; }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //builder.Entity<DoctorDetail>().Property(x => x.AppointmentFee).HasPrecision(18, 2);
        //builder.Properties<decimal>().Configure(c => c.HasPrecision(18, 3));

        //base.OnModelCreating(builder);
        //}
    }
}

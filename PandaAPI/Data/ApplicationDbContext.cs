using Microsoft.EntityFrameworkCore;
using PandaAPI.Models;

namespace PandaAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Latin1_General_100_CI_AS");

            modelBuilder.Entity<Patient>()
                .Property(p => p.Name)
                .IsUnicode();
        }
    }
}
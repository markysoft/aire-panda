using Microsoft.EntityFrameworkCore;
using PandaAPI.Data;
using PandaAPI.Models;

namespace PandaAPI.Test.Integration.Repositories
{
    public class DataSeeder
    {
        private List<Patient> patients = new();
        private ApplicationDbContext context;

        public List<Patient> Patients => patients;
        public ApplicationDbContext Context => context;

        public DataSeeder(string contextName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite($"Data Source=panda-{contextName}.db")
                .Options;

            context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.Migrate();
            context.Database.EnsureCreated();
        }

        public void SeedPatients()
        {
            patients = new List<Patient>
                {
                    new Patient
                    {
                        NhsNumber = "1585672939",
                        Name = "Thomas Bray",
                        DateOfBirth = new DateOnly(1962, 9, 12),
                        Postcode = "S68 9DB"
                    },
                    new Patient
                    {
                        NhsNumber = "0206633483",
                        Name = "Carl Bailey",
                        DateOfBirth = new DateOnly(1950, 12, 14),
                        Postcode = "M0U 7EH"
                    },
                    new Patient
                    {
                        NhsNumber = "0759480494",
                        Name = "Graham Reynolds",
                        DateOfBirth = new DateOnly(1958, 9, 19),
                        Postcode = "S4 0JT"
                    }
                };

            context.Patients.AddRange(patients);
            context.SaveChanges();
        }
    }
}

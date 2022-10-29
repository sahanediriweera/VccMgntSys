using Microsoft.EntityFrameworkCore;
using VccMgntSys.Models;

namespace VccMgntSys
{
    public class MainDatabase : DbContext
    {
        public MainDatabase(DbContextOptions options) : base(options)
        {

        }

        public MainDatabase()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Admin> admins { get; set; }

        public DbSet<Citizen> citizens { get; set; }

        public DbSet<Manager> managers { get; set; }

        public DbSet<Message> messages { get; set; }

        public DbSet<Staff> staffs { get; set; }

        public DbSet<Statistics> statistics { get; set; }

        public DbSet<VaccineBatch> vaccineBatches { get; set; }

        public DbSet<VaccineProgram> vaccinePrograms { get; set; }

        public DbSet<Admin> tempAdmins { get; set; }

        public DbSet<Manager> tempManagers { get; set; }

        public DbSet<Staff> tempStaff { get; set; }
    }
}

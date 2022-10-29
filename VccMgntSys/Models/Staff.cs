using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VccMgntSys.Models
{
    public class Staff
    {
        [Key]
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String DateofBirth { get; set; } 

        public String HospitalId { get; set; }

        public String JobDescription { get; set; }

        public String Address { get; set; }

        public String CitizenId { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }

        public long PhoneNumber { get; set; }

        public virtual ICollection<VaccineProgram>? VaccinePrograms { get; set; }

        [NotMapped]
        public virtual ICollection<VaccineProgram>? NextVaccineProgram { get; set; }

        public Staff()
        {

        }

    }
}

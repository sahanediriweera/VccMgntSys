using System.ComponentModel.DataAnnotations;

namespace VccMgntSys.Models
{
    public class VaccineProgram
    {
        [Key]
        public Guid Id { get; set; }

        public String Location { get; set; }

        public String Date { get; set; }

        public virtual ICollection<Citizen> Citizens { get; set; }

        public virtual ICollection<Staff> Staffs { get; set; }

        public virtual ICollection<VaccineBatch> VaccineBatches { get; set; }

        public virtual Manager Manager { get; set; }

        public VaccineProgram()
        {

        }
    }
}

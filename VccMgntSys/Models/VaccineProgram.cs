using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VccMgntSys.Models
{
    public class VaccineProgram
    {
        [Key]
        public Guid Id { get; set; }

        public String Location { get; set; }

        public String Date { get; set; }

        [JsonIgnore]
        public virtual ICollection<Citizen> Citizens { get; set; }

        [JsonIgnore]
        public virtual ICollection<Staff> Staffs { get; set; }

        [JsonIgnore]
        public virtual ICollection<VaccineBatch> VaccineBatches { get; set; }

        public virtual Manager Manager { get; set; }

        public VaccineProgram()
        {

        }
    }
}

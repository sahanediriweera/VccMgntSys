using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VccMgntSys.Models
{
    public class Citizen
    {
        [Key]
        public Guid Id { get; set; }

        public String Name { get; set; }

        public long CitizenID { get; set; }

        public long PhoneNumber { get; set; }

        public String EmailAddress { get; set; }

        public String Password { get; set; }

        public String Address { get; set; }

        public int VaccinationCount { get; set; }

        public String BirthDate { get;set; }

        public String? VaccinationDate { get; set; }

        public String? ReportData { get; set; }

        public String? OtherDiseases { get; set; }

        public String? Status { get; set; }

        public bool? Pending { get; set; }

        public bool? isApproved { get; set; }

        [JsonIgnore]
        public virtual ICollection<VaccineProgram>? VaccineProgram { get; set; }

        public Citizen()
        {
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace VccMgntSys.Models.CreateProgram
{
    public class StaffDetails
    {
        public Guid Id { get; set; }

        public String? Name { get; set; }

        public String? DateofBirth { get; set; }

        public String? HospitalId { get; set; }

        public String? JobDescription { get; set; }

        public StaffDetails()
        {

        }
    }
}

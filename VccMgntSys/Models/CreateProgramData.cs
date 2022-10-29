namespace VccMgntSys.Models
{
    public class CreateProgramData
    {
        public String CitizenIDs { get; set; }

        public String Location { get; set; }

        public String StaffIds { get; set; }

        public String VaccineIDs { get; set; }

        public String Date { get; set; }

        public Guid managerId { get; set; }
    }
}

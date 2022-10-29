namespace VccMgntSys.Models
{
    public class ViewCitizenDetails
    {
        public String Name { get; set; }

        public long CitizenID { get; set; }

        public long PhoneNumber { get; set; }

        public String EmailAddress { get; set; }

        public String Address { get; set; }

        public int VaccinationCount { get; set; }

        public String BirthDate { get; set; }

        public String ReportData { get; set; }

        public String Status { get; set; }

    }
}

namespace VccMgntSys.Models
{
    public class UpdateCitizenData
    {
        public Guid Id { get; set; }
        public int VaccinationCount { get; set; }

        public String? VaccinationDate { get; set; }

        public String? ReportData { get; set; }

        public String? OtherDiseases { get; set; }

        public String? Status { get; set; }

        public bool? Pending { get; set; }
    }
}

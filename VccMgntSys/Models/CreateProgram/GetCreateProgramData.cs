namespace VccMgntSys.Models.CreateProgram
{
    public class GetCreateProgramData
    {
        public List<CitizenDetails>? CitizenDetails { get; set; }

        public List<StaffDetails>? StaffDetails { get;set; }

        public List<VaccinationDetails>? VaccinationDetails { get;set; }

        public GetCreateProgramData() { }
    }
}

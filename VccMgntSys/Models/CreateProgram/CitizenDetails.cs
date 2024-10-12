namespace VccMgntSys.Models.CreateProgram
{
    public class CitizenDetails
    {
        public Guid Id { get; set; }

        public String? Name { get; set; }

        public long CitizenID { get; set; }

        public long PhoneNumber { get; set; }

        public String? EmailAddress { get; set; }

        public CitizenDetails()
        {
        }
    }
}

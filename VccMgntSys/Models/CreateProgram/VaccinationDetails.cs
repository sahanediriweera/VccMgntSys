namespace VccMgntSys.Models.CreateProgram
{
    public class VaccinationDetails
    {
        public Guid Id { get; set; }

        public String? Type { get; set; }

        public String? ExpirationDate { get; set; }

        public String? ProducedDate { get; set; }

        public String? BatchId { get; set; }

        public VaccinationDetails()
        {

        }
    }
}

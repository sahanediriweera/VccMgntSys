namespace VccMgntSys.Models
{
    public class GetVaccineBatches
    {
        public String Type { get; set; }

        public String ExpirationDate { get; set; }

        public String ProducedDate { get; set; }

        public int Count { get; set; }

        public String BatchId { get; set; }
    }
}

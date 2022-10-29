using System.ComponentModel.DataAnnotations;

namespace VccMgntSys.Models
{
    public class Statistics
    {
        [Key]
        public int Id { get; set; }

        public String Datadate { get; set; }

        public long VaccinatedCitizens { get; set; }

        public long TotalVaccinations { get; set; }

        public long TotalPrograms { get; set; }

        public Statistics()
        {

        }

    }
}

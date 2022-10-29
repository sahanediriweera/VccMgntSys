using System.ComponentModel.DataAnnotations;

namespace VccMgntSys.Models
{
    public class VaccineBatch
    {
        [Key]
        public Guid Id { get; set; }

        public String Type { get; set; }

        public String ExpirationDate { get; set; }

        public String ProducedDate { get; set; }

        public int Count { get; set; }

        public virtual ICollection<VaccineProgram> VaccinePrograms { get; set; }
            
        public String BatchId { get; set; }

        public VaccineBatch()
        {

        }


    }
}

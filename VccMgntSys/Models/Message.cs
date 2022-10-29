using System.ComponentModel.DataAnnotations;

namespace VccMgntSys.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }

        public String MessageDate { get; set; }

        public virtual Citizen Citizen { get; set; }

        public virtual VaccineProgram VaccineProgram { get; set; }

        public bool Confirm { get; set; }

        public Message()
        {

        }
    }
}

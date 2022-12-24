using System.ComponentModel.DataAnnotations;

namespace VccMgntSys.Models
{
    public class CreateManager
    {

        public String Name { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }

        public String ConfirmPassword { get; set; }

        public String CitizenID { get; set; }

        public CreateManager()
        {

        }
    }
}

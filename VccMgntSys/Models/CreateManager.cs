using System.ComponentModel.DataAnnotations;

namespace VccMgntSys.Models
{
    public class CreateManager
    {

        public String Name { get; set; }

        public String DateofBirth { get; set; }

        public long PhoneNumber { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }

        public String ConfirmPassword { get; set; }

        public String Address { get; set; }

        public String JobDescription { get; set; }

        public String HospitalID { get; set; }

        public CreateManager()
        {

        }
    }
}

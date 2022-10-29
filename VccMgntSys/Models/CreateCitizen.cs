namespace VccMgntSys.Models
{
    public class CreateCitizen
    {
        public String Name { get; set; }

        public long CitizenID { get; set; }

        public long PhoneNumber { get; set; }

        public String EmailAddress { get; set; }

        public String Password { get; set; }

        public String ConfirmPassword { get; set; }

        public String BirthDay { get; set; }

        public String Address { get; set; }

    }
}

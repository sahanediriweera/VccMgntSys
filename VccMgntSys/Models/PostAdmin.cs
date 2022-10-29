namespace VccMgntSys.Models
{
    public class PostAdmin
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public long PhoneNumber { get; set; }

        public String Email { get; set; }

        public String DateofBirth { get; set; }

        public String Address { get; set; }

        public String JobDescription { get; set; }

        public bool? IsSuperAdmin { get; set; }

        public String StringCitizenID { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VccMgntSys.Models;
using VccMgntSys.Security;

namespace VccMgntSys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SignUpController : Controller
    {
        private readonly MainDatabase mainDatabase;

        public SignUpController(MainDatabase mainDatabase)
        {
            this.mainDatabase = mainDatabase;
        }


        [HttpPost]
        [Route("citizen")]
        public async Task<IActionResult> CreateCitizen(CreateCitizen createCitizen)
        {
            List<Citizen> citizens = await this.mainDatabase.citizens.ToListAsync();

            foreach(Citizen citizen1 in citizens)
            {
                if(citizen1.EmailAddress == createCitizen.EmailAddress)
                {
                    return BadRequest("User Already Exist");
                }
            }

            if (createCitizen == null)
            {
                return BadRequest();
            }

            Citizen citizen = new Citizen()
            {
                CitizenID = createCitizen.CitizenID,
                Name = createCitizen.Name,
                PhoneNumber = createCitizen.PhoneNumber,
                Password = createCitizen.Password,
                EmailAddress = createCitizen.EmailAddress,
                BirthDate = createCitizen.BirthDay,
                Address = createCitizen.Address,
                VaccinationCount = 0,
                Pending = false
            };

            await this.mainDatabase.citizens.AddAsync(citizen);
            await this.mainDatabase.SaveChangesAsync();

            return Ok(citizen.Id);
        }


        [HttpPost]
        [Route("manager")]
        public async Task<IActionResult> SignUpManager(CreateManager createManager)
        {
            List<Manager> managers = await this.mainDatabase.managers.ToListAsync();
            
            foreach(Manager manager1 in managers)
            {
                if(manager1.Email == createManager.Email)
                {
                    return BadRequest("User Already Exist");
                }
            }

            managers = await this.mainDatabase.tempManagers.ToListAsync();

            foreach(Manager manager2 in managers)
            {
                if(manager2.Email == createManager.Email)
                {
                    return BadRequest("Account Activation Pending");
                }
            }

            Manager manager = new Manager()
            {
                Name = createManager.Name,
                Address = createManager.Address,
                PhoneNumber = createManager.PhoneNumber,
                DateofBirth = createManager.DateofBirth,
                Email = createManager.Email,
                Password = createManager.Password,
                JobDescription = createManager.JobDescription,
                HospitalID = createManager.HospitalID,
            };

            await this.mainDatabase.tempManagers.AddAsync(manager);
            await this.mainDatabase.SaveChangesAsync();

            return Ok(manager.Id);
        }

        [HttpPost]
        [Route("staff")]
        public async Task<IActionResult> SignUpStaff(CreateStaff createStaff)
        {
            List<Staff> staffs = await this.mainDatabase.staffs.ToListAsync();

            foreach(Staff staff1 in staffs)
            {
                if(staff1.Email == createStaff.Email)
                {
                    return BadRequest("User Already Exist");
                }
            }

            staffs = await this.mainDatabase.tempStaff.ToListAsync();

            foreach(Staff staff2 in staffs)
            {
                if(staff2.Email == createStaff.Email)
                {
                    return BadRequest("Account Activation Pending");
                }
            }

            Staff staff = new Staff()
            {
                Name = createStaff.Name,
                Address = createStaff.Address,
                PhoneNumber = createStaff.PhoneNumber,
                Password = createStaff.Password,
                CitizenId = createStaff.CitizenId,
                HospitalId = createStaff.HospitalId,
                Email = createStaff.Email,
                DateofBirth = createStaff.DateofBirth,
                JobDescription = createStaff.JobDescription,
            };

            await mainDatabase.tempStaff.AddAsync(staff);
            await this.mainDatabase.SaveChangesAsync();

            return Ok(staff.Id);
        }

        [HttpPost]
        [Route("admin")]
        public async Task<IActionResult> CreateAdmin(CreateAdmin createAdmin)
        {
            List<Admin> admins = await this.mainDatabase.admins.ToListAsync();

            foreach(Admin admin1 in admins)
            {
                if(admin1.Email == createAdmin.Email)
                {
                    return BadRequest("User Already Exist");
                }
            }

            admins = await this.mainDatabase.tempAdmins.ToListAsync();

            foreach(Admin admin2 in admins)
            {
                if(admin2.Email == createAdmin.Email)
                {
                    return BadRequest("Account Activation is Pending");
                }
            }

            Admin admin = new Admin()
            {
                Name = createAdmin.Name,
                DateofBirth = createAdmin.DateofBirth,
                Address = createAdmin.Address,
                PhoneNumber = createAdmin.PhoneNumber,
                Email = createAdmin.Email,
                Password = createAdmin.Password,
                StringCitizenID = createAdmin.CitizenID,
                JobDescription = createAdmin.JobDescription,
            };

            await mainDatabase.tempAdmins.AddAsync(admin);
            await this.mainDatabase.SaveChangesAsync();

            return Ok(admin.Id);
        }
    }
}

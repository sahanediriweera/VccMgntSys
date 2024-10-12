using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VccMgntSys.Models;

namespace VccMgntSys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {

        private readonly MainDatabase mainDatabase;

        public AdminController(MainDatabase mainDatabase)
        {
            this.mainDatabase = mainDatabase;
        }

        [HttpGet]
        [Route("managers")]

        public async Task<IActionResult> GetManager()
        {
            List<Manager> managers = await this.mainDatabase.tempManagers.ToListAsync();

            List<PostManagers> postManagers = new List<PostManagers>();

            foreach(Manager manager in managers)
            {
                PostManagers postManager = new PostManagers()
                {
                    Id = manager.Id,
                    Name = manager.Name,
                    HospitalID = manager.HospitalID,
                    DateofBirth = manager.DateofBirth,
                    JobDescription = manager.JobDescription,
                    Address = manager.Address,
                    Email = manager.Email,
                    PhoneNumber = manager.PhoneNumber
                };

                postManagers.Add(postManager);
            }

            return Ok(postManagers);
        }

        [HttpPost]
        [Route("managers")]
        public async Task<IActionResult> SaveManager(GetDetails getDetails)
        {
            var tempman = await this.mainDatabase.tempManagers.FindAsync(getDetails.id);
            
            if (tempman == null)
            {
                return BadRequest();
            }

            tempman.isApproved = true;

            mainDatabase.Entry(tempman).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await this.mainDatabase.SaveChangesAsync();

            PostManagers postManager = new PostManagers()
            {
                Id = tempman.Id,
                Name = tempman.Name,
                HospitalID = tempman.HospitalID,
                DateofBirth = tempman.DateofBirth,
                JobDescription = tempman.JobDescription,
                Address = tempman.Address,
                Email = tempman.Email,
                PhoneNumber = tempman.PhoneNumber
            };

            return Ok(postManager);
        }

        [HttpGet]
        [Route("staff")]

        public async Task<IActionResult> GetStaff()
        {
            List<Staff> staffs = await this.mainDatabase.tempStaff.ToListAsync();

            List<PostStaff> postStaffs = new List<PostStaff>();

            foreach(Staff staff in staffs)
            {
                PostStaff postStaff = new PostStaff()
                {
                    DateofBirth=staff.DateofBirth,
                    Name = staff.Name,
                    JobDescription=staff.JobDescription,
                    Address = staff.Address,
                    CitizenId = staff.CitizenId,
                    Email = staff.Email,
                    HospitalId = staff.HospitalId,
                    Id = staff.Id,
                    PhoneNumber = staff.PhoneNumber
                };

                postStaffs.Add(postStaff);
            }

            return Ok(postStaffs);
        }

        [HttpPost]
        [Route("staff")]
        public async Task<IActionResult> SaveStaff(GetDetails getDetails)
        {
            var tempsta = await this.mainDatabase.tempStaff.FindAsync(getDetails.id);

            if (tempsta == null)
            {
                return NotFound();
            }

            tempsta.isApproved = true;

            mainDatabase.Entry(tempsta).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await this.mainDatabase.SaveChangesAsync();

            PostStaff postStaff = new PostStaff()
            {
                DateofBirth = tempsta.DateofBirth,
                Name = tempsta.Name,
                JobDescription = tempsta.JobDescription,
                Address = tempsta.Address,
                CitizenId = tempsta.CitizenId,
                Email = tempsta.Email,
                HospitalId = tempsta.HospitalId,
                Id = tempsta.Id,
                PhoneNumber = tempsta.PhoneNumber
            };

            return Ok(postStaff);
        }

        [HttpGet]
        [Route("admin")]

        public async Task<IActionResult> GetAdmins()
        {
            List<Admin> admins = await this.mainDatabase.tempAdmins.ToListAsync();

            List<PostAdmin> postAdmins = new List<PostAdmin>();

            foreach(Admin admin in admins)
            {
                PostAdmin postAdmin = new PostAdmin()
                {
                    StringCitizenID = admin.StringCitizenID,
                    IsSuperAdmin = false,
                    Address = admin.Address,
                    DateofBirth =admin.DateofBirth,
                    Email = admin.Email,
                    Id = admin.Id,
                    JobDescription =admin.JobDescription,
                    Name = admin.Name,
                    PhoneNumber = admin.PhoneNumber
                };

                postAdmins.Add(postAdmin);
            }

            return Ok(postAdmins);
        }

        [HttpPost]
        [Route("saveadmin")]

        public async Task<IActionResult> SaveAdmin(MakeAdmin makeAdmin)
        {
            Admin? superAdmin = await this.mainDatabase.admins.FindAsync(makeAdmin.SuperAdmin);

            if(superAdmin == null)
            {
                return NotFound("Logged in user not found");
            }

            if(superAdmin.IsSuperAdmin == true)
            {
                var tempadmin = await this.mainDatabase.tempAdmins.FindAsync(makeAdmin.UserAdmin);
                if (tempadmin == null)
                {
                    return NotFound();
                }

                tempadmin.isApproved = true;

                mainDatabase.Entry(tempadmin).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await this.mainDatabase.SaveChangesAsync();

                PostAdmin postAdmin = new PostAdmin()
                {
                    StringCitizenID = tempadmin.StringCitizenID,
                    IsSuperAdmin = false,
                    Address = tempadmin.Address,
                    DateofBirth = tempadmin.DateofBirth,
                    Email = tempadmin.Email,
                    Id = tempadmin.Id,
                    JobDescription = tempadmin.JobDescription,
                    Name = tempadmin.Name,
                    PhoneNumber = tempadmin.PhoneNumber
                };

                return Ok(postAdmin);
            }

            else
            {
                return BadRequest("Admin is not super admin");
            }


        }

        [HttpDelete]
        [Route("programid")]

        public async Task<IActionResult> DeleteProgram(GetDetails getDetails)
        {
            var program = await this.mainDatabase.vaccinePrograms.FindAsync(getDetails.id);

            if (program == null) 
            { 
                return NotFound(); 
            }

            this.mainDatabase.vaccinePrograms.Remove(program);

            return Ok("Program Deleted");
        }

        [HttpGet]
        [Route("realAdmins")]

        public async Task<IActionResult> GetActualAdmins()
        {
            List<Admin> admins = await this.mainDatabase.admins.ToListAsync();

            List<PostAdmin> postAdmins = new List<PostAdmin>();

            foreach(Admin admin in admins)
            {
                PostAdmin postAdmin = new PostAdmin()
                {
                    StringCitizenID = admin.StringCitizenID,
                    IsSuperAdmin = false,
                    Address = admin.Address,
                    DateofBirth = admin.DateofBirth,
                    Email = admin.Email,
                    Id = admin.Id,
                    JobDescription = admin.JobDescription,
                    Name = admin.Name,
                    PhoneNumber = admin.PhoneNumber
                };

                postAdmins.Add(postAdmin);
            }

            return Ok(postAdmins);
        }

        [HttpPost]
        [Route("realadminguid")]

        public async Task<IActionResult> MakeSuperAdmin(MainSuperAdmin mainSuperAdmin)
        {
            Admin? superAdmin = await this.mainDatabase.admins.FindAsync(mainSuperAdmin.SuperAdminGuid);
            
            if (superAdmin == null)
            {
                return BadRequest();
            }

            if(superAdmin.IsSuperAdmin == true)
            {
                Admin? admin = await this.mainDatabase.admins.FindAsync(mainSuperAdmin.AdminGuid);

                if (admin == null)
                {
                    return BadRequest();
                }

                admin.IsSuperAdmin = true;
                this.mainDatabase.Entry(admin).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                await this.mainDatabase.SaveChangesAsync();

                PostAdmin postAdmin = new PostAdmin()
                {
                    StringCitizenID = admin.StringCitizenID,
                    IsSuperAdmin = false,
                    Address = admin.Address,
                    DateofBirth = admin.DateofBirth,
                    Email = admin.Email,
                    Id = admin.Id,
                    JobDescription = admin.JobDescription,
                    Name = admin.Name,
                    PhoneNumber = admin.PhoneNumber
                };

                return Ok(postAdmin);
            }

            else
            {
                return BadRequest("Admin is not superadmin");
            }
        }
    }
}

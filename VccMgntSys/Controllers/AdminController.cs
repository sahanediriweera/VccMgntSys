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
        public async Task<IActionResult> SaveManager(Guid managerid)
        {
            var tempman = await this.mainDatabase.tempManagers.FindAsync(managerid);
            
            if (tempman == null)
            {
                return BadRequest();
            }

            Manager manager = new Manager()
            {
                Name = tempman.Name,
                Address = tempman.Address,
                DateofBirth = tempman.DateofBirth,
                Email = tempman.Email,
                HospitalID = tempman.HospitalID,
                JobDescription = tempman.JobDescription,
                Password = tempman.Password,
                PhoneNumber = tempman.PhoneNumber
            };

            await this.mainDatabase.managers.AddAsync(manager);
            await this.mainDatabase.SaveChangesAsync();

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
        public async Task<IActionResult> SaveStaff(Guid staffid)
        {
            var tempsta = await this.mainDatabase.tempStaff.FindAsync(staffid);

            if (tempsta == null)
            {
                return NotFound();
            }

            Staff staff = new Staff()
            {
                Name = tempsta.Name,
                Address = tempsta.Address,
                DateofBirth = tempsta.DateofBirth,
                Email = tempsta.Email,
                HospitalId = tempsta.HospitalId,
                JobDescription = tempsta.JobDescription,
                Password = tempsta.Password,
                PhoneNumber = tempsta.PhoneNumber,
                CitizenId = tempsta.CitizenId,
            };

            await this.mainDatabase.staffs.AddAsync(staff);
            await this.mainDatabase.SaveChangesAsync();

            PostStaff postStaff = new PostStaff()
            {
                DateofBirth = staff.DateofBirth,
                Name = staff.Name,
                JobDescription = staff.JobDescription,
                Address = staff.Address,
                CitizenId = staff.CitizenId,
                Email = staff.Email,
                HospitalId = staff.HospitalId,
                Id = staff.Id,
                PhoneNumber = staff.PhoneNumber
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

                Admin admin = new Admin()
                {
                    Name = tempadmin.Name,
                    Address = tempadmin.Address,
                    DateofBirth = tempadmin.DateofBirth,
                    Email = tempadmin.Email,
                    JobDescription = tempadmin.JobDescription,
                    Password = tempadmin.Password,
                    StringCitizenID = tempadmin.StringCitizenID,
                    PhoneNumber = tempadmin.PhoneNumber,
                };

                await this.mainDatabase.admins.AddAsync(admin);
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
                return BadRequest("Admin is not super admin");
            }


        }

        [HttpDelete]
        [Route("programid")]

        public async Task<IActionResult> DeleteProgram(VaccineProgram programid)
        {
            var program = await this.mainDatabase.vaccinePrograms.FindAsync(programid);

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
        /*
        [HttpGet]
        [Route("makethesuperadmin")]

        public async Task<IActionResult> MaketheFirstSuperAdmin()
        {
            Admin superAdmin = new Admin()
            {
                Name = "The Admin",
                DateofBirth = "01/01/1998",
                Address = "Faculty of Engineering",
                IsSuperAdmin = true,
                Email = "superadmin@gmail.com",
                JobDescription = "The Super Admin",
                Password = "Admin123",
                PhoneNumber = 125478963,
                StringCitizenID = "123654789"
            };

            await mainDatabase.admins.AddAsync(superAdmin);
            await this.mainDatabase.SaveChangesAsync();
            return Ok();
        }

        
        public void ApproveAdmin()
        {

        }

        public void ApproveManager()
        {

        }

        public void ApproveStaff()
        {

        }

        public void ChangeCitizenData()
        {

        }

        public void ChangeAdminData()
        {

        }

        public void ChangeManagerData()
        {

        }

        public void ChangeStaffData()
        {

        }

        public void DeletePrograms()
        {

        }
        */
    }
}

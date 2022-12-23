using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VccMgntSys.Models;
using VccMgntSys.Security;
using Microsoft.AspNetCore.Cors;

namespace VccMgntSys.Controllers
{
    [ApiController]
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly MainDatabase mainDatabase;

        public LoginController(MainDatabase mainDatabase)
        {
            this.mainDatabase = mainDatabase;
        }

        [HttpPost]
        [Route("manager")]

        public async Task<IActionResult> LoginManager(LoginManager loginManager )
        {
            var managers = await this.mainDatabase.managers.ToListAsync();
            Manager? manager = null;

            foreach( Manager manag in managers) {
                if (loginManager.Email == manag.Email) { 
                    manager = manag;
                }
            }

            if (manager == null)
            {
                return NotFound();
            }

            if(manager.Password != loginManager.Password)
            {
                return BadRequest("Invalid credentials");
            }
            return Ok(manager.Id);
        }

        [HttpPost]
        [Route("staff")]

        public async Task<IActionResult> LoginStaff(LoginStaff loginStaff)
        {
            var staffs = await this.mainDatabase.staffs.ToListAsync();

            Staff staff = null;

            foreach (Staff staffer in staffs) 
            {
                if(staffer.Email == loginStaff.Email)
                {
                    staff = staffer;
                }
            }

            if (staff == null)
            {
                return NotFound();
            }
            if (staff.Password !=  loginStaff.Password)
            {
                return BadRequest("Invalid credentials");
            }
            return Ok(staff.Id);
        }
        [HttpPost]
        [Route("admin")]

        public async Task<IActionResult> LoginAdmin(LoginAdmin loginAdmin)
        {
            var admins = await this.mainDatabase.admins.ToListAsync();
            Admin? admin = null;

            foreach(Admin adminer in admins)
            {
                if(adminer.Email == loginAdmin.Email)
                {
                    admin = adminer;
                }
            }

            if (admin == null)
            {
                return NotFound();
            }
            if (admin.Password != loginAdmin.Password)
            {
                return BadRequest("Invalid credentials");
            }
            return Ok(admin.Id);
        }
        [HttpPost]
        [Route("citizen")]

        public async Task<IActionResult> LoginCitizen(LoginCitizen loginCitizen)
        {
            var citizens = await this.mainDatabase.citizens.ToListAsync();

            Citizen? citizen = null;

            foreach(Citizen citizener in citizens)
            {
                if(citizener.EmailAddress == loginCitizen.Email)
                {
                    citizen = citizener;
                }
            }

            if (citizen == null)
            {
                return NotFound();
            }
            if (citizen.Password != loginCitizen.Password)
            {
                return BadRequest("Invalid credentials");
            }
            return Ok(citizen.Id);
        }
    }
}

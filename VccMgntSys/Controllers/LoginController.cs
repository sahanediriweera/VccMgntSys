using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VccMgntSys.Models;
using VccMgntSys.Security;

namespace VccMgntSys.Controllers
{
    [ApiController]
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

            if(manager.Password == loginManager.Password)
            {
                return BadRequest("Invalid credentials");
            }
            return Ok(manager.Id);
        }

        [HttpPost]
        [Route("staff")]

        public async Task<IActionResult> LoginStaff(LoginStaff loginStaff)
        {
            var staff = await this.mainDatabase.staffs.FindAsync(loginStaff.Email);
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
            var admin = await this.mainDatabase.admins.FindAsync(loginAdmin.Email);
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
            var citizen = await this.mainDatabase.citizens.FindAsync(loginCitizen.Email);
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

using Microsoft.AspNetCore.Mvc;
using VccMgntSys.Models;
using System.Data;

namespace VccMgntSys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitizenController : Controller
    {
        
        private readonly MainDatabase mainDatabase;

        public CitizenController(MainDatabase mainDatabase)
        {
            this.mainDatabase = mainDatabase;
        }

        [HttpPost]
        [Route("GetDetails")]
        public async Task<IActionResult> ViewCitizenDetails(GetDetails getDetails)
        {
            ViewCitizenDetails viewCitizenDetails = new ViewCitizenDetails();

            var citizen = await this.mainDatabase.citizens.FindAsync(getDetails.id);

            if(citizen == null) { return NotFound(); }
            viewCitizenDetails.CitizenID = citizen.CitizenID;
            viewCitizenDetails.Name = citizen.Name;
            viewCitizenDetails.Address = citizen.Address;
            viewCitizenDetails.PhoneNumber = citizen.PhoneNumber;
            viewCitizenDetails.BirthDate = citizen.BirthDate;
            viewCitizenDetails.EmailAddress = citizen.EmailAddress;
            viewCitizenDetails.ReportData = (citizen.ReportData != null) ? citizen.ReportData : "";
            viewCitizenDetails.Status = (citizen.Status != null) ? citizen.Status : "";
            viewCitizenDetails.VaccinationCount = citizen.VaccinationCount;

            return Ok(viewCitizenDetails);
        }

        [HttpPost]
        [Route("ChangeVaccineDate")]
        public async Task<IActionResult> ChangeDate(GetDetails getDetails)
        {
            Citizen? citizen = await this.mainDatabase.citizens.FindAsync(getDetails.id);

            if(citizen==null) 
            { 
                return NotFound(); 
            }

            citizen.Pending = false;
            mainDatabase.Entry(citizen).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await mainDatabase.SaveChangesAsync();
            return Ok(citizen);
        }
        [HttpPost]
        [Route("GetVaccineDate")]
        public async Task<IActionResult> VaccineDate(GetDetails getDetails)
        {

            var citizen = await this.mainDatabase.citizens.FindAsync(getDetails.id);

            if(citizen == null)
            {
                return NotFound();
            }

            String? date = citizen.VaccinationDate;

            if (date == null || date == "")
            {
                return NotFound("No vaccination date");
            }

            try
            {
                String[] alldates = date.Split(",");
                return Ok(alldates[alldates.Length - 1]);

            }catch (Exception)
            {
                return BadRequest();
            }
        }
        
    }
}

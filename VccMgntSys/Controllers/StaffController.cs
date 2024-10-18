using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VccMgntSys.Models;

namespace VccMgntSys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : Controller
    {
        private readonly MainDatabase mainDatabase;
        private readonly IMapper mapper;

        public StaffController(MainDatabase mainDatabase,IMapper mapper)
        {
            this.mainDatabase = mainDatabase;
            this.mapper = mapper;
        }
        [HttpGet]
        [Route("GetNextVaccineProgram")]
        public async Task<IActionResult> NextVaccinationProgram(Guid staffID)
        {
            var staff = await this.mainDatabase.staffs.FindAsync(staffID);
            if (staff == null)
            {
                return BadRequest();
            }

            ICollection<VaccineProgram> vaccineProgramsguids = staff.VaccinePrograms;

            List<VaccineProgram> vaccinePrograms = new List<VaccineProgram>();

            foreach (var vaccineProgram in vaccineProgramsguids)
            {
                VaccineProgram? item = await this.mainDatabase.vaccinePrograms.FindAsync(vaccineProgram.Id);
                if (item != null)
                {
                    DateTime programDateTime = Convert.ToDateTime(item.Date);
                    DateTime today = DateTime.Today;

                    if(DateTime.Compare(programDateTime, today) >= 0)
                    {
                        vaccinePrograms.Add(item);
                    }


                }
            }

            return Ok(vaccinePrograms);
        }

        [HttpGet]
        [Route("GetPatientDetails")]

        public async Task<IActionResult> GetCitizenDetails(long citizenID)
        {
            // Use LINQ to find the citizen by CitizenID
            Citizen? citizen = await this.mainDatabase.citizens
                .FirstOrDefaultAsync(c => c.CitizenID == citizenID);

            // Check if the citizen is found
            if (citizen == null)
            {
                return BadRequest("ID Doesn't Exist");
            }

            // Create the PostCitizenDetails object to return
            PostCitizenDetails postCitizenDetails = new PostCitizenDetails()
            {
                Id = citizen.Id,
                CitizenID = citizen.CitizenID,
                Name = citizen.Name,
                PhoneNumber = citizen.PhoneNumber,
                Address = citizen.Address,
                BirthDate = citizen.BirthDate,
                EmailAddress = citizen.EmailAddress,
                OtherDiseases = citizen.OtherDiseases,
                VaccinationCount = citizen.VaccinationCount,
                Pending = citizen.Pending,
                ReportData = citizen.ReportData,
                Status = citizen.Status,
                VaccinationDate = citizen.VaccinationDate,
                VaccineProgram = citizen.VaccineProgram,
            };

            // Return the citizen details
            return Ok(postCitizenDetails);
        }


        [HttpPost]
        [Route("updatecitizendata")]

        public async Task<IActionResult> updateCitizenData(UpdateCitizenData updatecitizendata)
        {
            var citizen = await this.mainDatabase.citizens.FindAsync(updatecitizendata.Id);
            
            if(citizen == null)
            {
                return NotFound();
            }
            citizen.ReportData = updatecitizendata.ReportData;
            citizen.Status = updatecitizendata.Status;
            citizen.Pending = updatecitizendata.Pending;
            citizen.OtherDiseases = updatecitizendata.OtherDiseases;

            this.mainDatabase.Entry(citizen).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await this.mainDatabase.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("UpdatePatientVaccination")]

        public async Task<IActionResult> UpdatePatientVaccination(GetDetails getDetails)
        {
            var citizen = await this.mainDatabase.citizens.FindAsync(getDetails.id);

            if(citizen == null)
            {
                return BadRequest("Check Again");
            }

            citizen.VaccinationCount++;
            this.mainDatabase.Entry(citizen).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await this.mainDatabase.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("CreateVaccineBatch")]

        public async Task<IActionResult> AddVaccineBatches(GetVaccineBatches getVaccineBatches)
        {
            if (getVaccineBatches == null) { return BadRequest(); }

            VaccineBatch vaccineBatch = new VaccineBatch()
            {
                Type = getVaccineBatches.Type,
                BatchId = getVaccineBatches.BatchId,
                Count = getVaccineBatches.Count,
                ProducedDate = getVaccineBatches.ProducedDate,
                ExpirationDate = getVaccineBatches.ExpirationDate
            };

            await this.mainDatabase.vaccineBatches.AddAsync(vaccineBatch);
            await this.mainDatabase.SaveChangesAsync();

            return Ok(vaccineBatch);
        }

        [HttpPost]
        [Route("citizenvaccineadd")]
        public async Task<IActionResult> AddVaccineProgramCitizen(UpdateCitizenProgram updateCitizenProgram)
        {
            // Find the citizen and vaccine program
            Citizen? citizen = await this.mainDatabase.citizens.FirstOrDefaultAsync(c => c.CitizenID == updateCitizenProgram.Id);
            VaccineProgram? vaccineProgram = await this.mainDatabase.vaccinePrograms.FindAsync(updateCitizenProgram.VaccineProgramID);

            // Validate citizen and vaccine program existence
            if (citizen == null || vaccineProgram == null)
            {
                return BadRequest("Citizen or Vaccine Program not found.");
            }

            // Get current date
            DateTime date = DateTime.Today;

            // Initialize vaccine programs if null and add the vaccine program to the citizen
            if (citizen.VaccineProgram == null)
            {
                citizen.VaccineProgram = new List<VaccineProgram>();
            }
            citizen.VaccineProgram.Add(vaccineProgram);

            // Increment the vaccination count
            citizen.VaccinationCount += 1;

            // Update the vaccination date, check if it's null
            if (string.IsNullOrEmpty(citizen.VaccinationDate))
            {
                citizen.VaccinationDate = date.ToString();
            }
            else
            {
                citizen.VaccinationDate += "," + date.ToString();
            }

            // Mark the citizen entity as modified and save changes
            this.mainDatabase.Entry(citizen).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await this.mainDatabase.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("allprograms")]

        public async Task<IActionResult> AllPrograms()
        {
            List<VaccineProgram> programs = await this.mainDatabase.vaccinePrograms.ToListAsync();

            if(programs == null || programs.Count == 0) { return BadRequest(string.Empty); }

            return Ok(programs);
        }


        /*
        [HttpPost]
        [Route("citizenvaccineadd")]
        public async Task<IActionResult> AddVaccineProgramCitizen(UpdateCitizenProgram updateCitizenProgram)
        {
            Citizen? citizen = await this.mainDatabase.citizens.FindAsync(updateCitizenProgram.Id);

            VaccineProgram? vaccineProgram = await this.mainDatabase.vaccinePrograms.FindAsync(updateCitizenProgram.VaccineProgramID);

            if (citizen == null | vaccineProgram == null) { return BadRequest(); }
            DateTime date = DateTime.Today;
            ICollection<VaccineProgram> vaccinePrograms = new List<VaccineProgram>();
            vaccinePrograms.Add(vaccineProgram);
            citizen.VaccineProgram = vaccinePrograms;
            citizen.VaccinationCount += 1;
            citizen.VaccinationDate = citizen.VaccinationDate + "," + date.ToString();
            this.mainDatabase.Entry(citizen).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await this.mainDatabase.SaveChangesAsync();
            return Ok();
        }*/

        /*
        public void NextVaccination()
        {

        }

        public void PatientVaccinationInformation()
        {

        }

        public void UpdateInformation()
        {

        }

        public void ProgramPatients()
        {

        }
        */
    }
}

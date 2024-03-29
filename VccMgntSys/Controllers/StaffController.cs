﻿using AutoMapper;
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

        public async Task<IActionResult> GetCitizenDetails(long patientguid)
        {
            List<Citizen> citizens = await this.mainDatabase.citizens.ToListAsync();

            Citizen? citizen1 = null;

            foreach(Citizen citizen in citizens)
            {
                if(citizen.CitizenID == patientguid)
                {
                    citizen1 = citizen;
                }
            }

            if (citizen1 == null)
            {
                return BadRequest("ID Doesn't Exist");
            }

            PostCitizenDetails postCitizenDetails = new PostCitizenDetails()
            {
                Id = citizen1.Id,
                CitizenID = citizen1.CitizenID,
                Name = citizen1.Name,
                PhoneNumber = citizen1.PhoneNumber,
                Address = citizen1.Address,
                BirthDate = citizen1.BirthDate,
                EmailAddress = citizen1.EmailAddress,
                OtherDiseases = citizen1.OtherDiseases,
                VaccinationCount = citizen1.VaccinationCount,
                Pending = citizen1.Pending,
                ReportData = citizen1.ReportData,
                Status = citizen1.Status,
                VaccinationDate = citizen1.VaccinationDate,
                VaccineProgram = citizen1.VaccineProgram,
            };

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
            var citizen = await this.mainDatabase.citizens.FindAsync(getDetails);

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
        }

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

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VccMgntSys.Mail_System;
using VccMgntSys.Models;

namespace VccMgntSys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerController : Controller
    {
        private readonly MainDatabase mainDatabase;
        private readonly IMailService mailService;
        public ManagerController(MainDatabase mainDatabase,IMailService mailService)
        {
            this.mainDatabase = mainDatabase;
            this.mailService = mailService;
        }

        [HttpPost]
        [Route("createprogram")]

        public async Task<IActionResult> CreateProgram(CreateProgramData createProgramData)
        {
            List<Citizen> citizenguids = new List<Citizen>();

            String[] citizenIDs = createProgramData.CitizenIDs.Split(',');
            foreach(String CitizenId in citizenIDs)
            {
                var item = await this.mainDatabase.citizens.FindAsync(Guid.Parse(CitizenId));
                if(item != null)
                {
                    citizenguids.Add(item);
                    String currentdates = item.VaccinationDate;
                    item.Pending = true;
                    currentdates = currentdates +","+ createProgramData.Date;
                    item.VaccinationDate = currentdates;
                    this.mainDatabase.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

            }

            ICollection <VaccineBatch> vaccineguids = new List<VaccineBatch>();
            String[] vaccineIds = createProgramData.VaccineIDs.Split(',');

            foreach(String VaccineId in vaccineIds)
            {
                var item = await this.mainDatabase.vaccineBatches.FindAsync(Guid.Parse(VaccineId));
                if( item != null)
                {
                    vaccineguids.Add(item);
                }
            }

            ICollection<Staff> staffguids = new List<Staff>();

            String[] staffIds = createProgramData.StaffIds.Split(',');

            foreach (String staffId in staffIds)
            {
                var item = await this.mainDatabase.staffs.FindAsync(staffId);
                if(item != null)
                {
                    staffguids.Add(item);
                }
                
            }

            Manager? manager = await this.mainDatabase.managers.FindAsync(createProgramData.managerId);
            VaccineProgram vaccineProgram = new VaccineProgram()
            {
                Citizens = citizenguids,
                Staffs = staffguids,
                VaccineBatches = vaccineguids,
                Date = createProgramData.Date,
                Location = createProgramData.Location,
                Manager = manager
            };

            foreach(var vaccineBatch in vaccineguids)
            {
                var item = await this.mainDatabase.vaccineBatches.FindAsync(vaccineBatch.Id);

                if(item == null)
                {
                    item.VaccinePrograms.Add(vaccineProgram);
                }
            }

            foreach(var staff in staffguids)
            {
                var item = await this.mainDatabase.staffs.FindAsync(staff.Id);

                if(item != null)
                {
                    item.VaccinePrograms.Add(vaccineProgram);
                }
            }
            await this.mainDatabase.vaccinePrograms.AddAsync(vaccineProgram);
            await this.mainDatabase.SaveChangesAsync();
            
            foreach(Citizen citizen in citizenguids)
            {
                MailRequest mailRequest = new MailRequest()
                {
                    Body = MailContent.getMailContent(citizen.Name, createProgramData.Date, createProgramData.Location),
                    Subject = MailContent.getMailSubject(),
                    ToEmail = citizen.EmailAddress
                };

                try
                {
                    await mailService.SendMailAsync(mailRequest);
                }

                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Ok(vaccineProgram);
        }

        [HttpGet]
        [Route("currentstatistics")]
        public async Task<IActionResult> CurrentStatistics()
        {
            var data = await this.mainDatabase.statistics.ToListAsync();

            return Ok(data);
        }

        [HttpGet]
        [Route("vaccinetypebatches")]

        public async Task<IActionResult> getVaccineTypeBatches(String vaccinetype)
        {
            List<VaccineBatch> vaccineBatchByType = new List<VaccineBatch>();

            List<VaccineBatch> vaccineBatches = await this.mainDatabase.vaccineBatches.ToListAsync();

            foreach(VaccineBatch vaccineBatch in vaccineBatches)
            {
                if(vaccineBatch != null)
                {
                    if(vaccineBatch.Type == vaccinetype)
                    {
                        vaccineBatchByType.Add(vaccineBatch);
                    }
                }
            }

            return Ok(vaccineBatchByType);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest mailRequest)
        {
            try
            {
                await mailService.SendMailAsync(mailRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("citizendetails")]
        public async Task<IActionResult> GetSuitableCitizen()
        {
            List<Citizen> citizens = await this.mainDatabase.citizens.ToListAsync();

            List<Citizen> selectedCitizens = new List<Citizen>();

            foreach(var citizen in citizens)
            {
                if(citizen.Pending == true)
                {

                }

                else
                {
                    if(citizen.VaccinationCount == 0)
                    {
                        selectedCitizens.Add(citizen);
                    }

                    else
                    {
                        String dates = citizen.VaccinationDate;
                        String[] vaccinationDates = dates.Split(",");
                        dates = vaccinationDates[vaccinationDates.Length - 1];

                        DateTime date1 = Convert.ToDateTime(dates);
                        DateTime date2 = DateTime.Today;
                        int result = DateTime.Compare(date2, date1);

                        if(result > 30)
                        {
                            selectedCitizens.Add(citizen);
                        }
                    }
                }

            }

            return Ok(selectedCitizens);
        }

        /*
        public void CreateProgram() 
        {
            
        }

        public void CurrentStatistics()
        {

        }

        public void FutureStatistics()
        {

        }

        public void CurrentPrograms()
        {

        }
        */
    }
}

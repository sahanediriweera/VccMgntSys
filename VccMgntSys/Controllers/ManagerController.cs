using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VccMgntSys.Mail_System;
using VccMgntSys.Models;
using VccMgntSys.Models.CreateProgram;

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

        [HttpGet]
        [Route("createprogram")]
        public async Task<IActionResult> GetCreateProgramDetails()
        {
            List<Citizen> citizens = await mainDatabase.citizens.ToListAsync();

            List<Staff> staffs = await mainDatabase.staffs.ToListAsync();

            List<VaccineBatch> batches = new List<VaccineBatch>();

            List<VaccinationDetails> vaccinationDetails = new List<VaccinationDetails>();

            List<StaffDetails> staffDetails = new List<StaffDetails>();

            List<CitizenDetails> citizenDetails = new List<CitizenDetails>();

            foreach (Citizen citizen in citizens) {
                CitizenDetails c = new CitizenDetails();

                c.Id = citizen.Id;
                c.Name = citizen.Name;
                c.EmailAddress = citizen.EmailAddress;
                c.CitizenID = citizen.CitizenID;
                c.PhoneNumber = citizen.PhoneNumber;

                citizenDetails.Add(c);
            }

            foreach(Staff staff in staffs)
            {
                StaffDetails s = new StaffDetails();

                s.Id = staff.Id;
                s.Name = staff.Name;
                s.DateofBirth = staff.DateofBirth;
                s.HospitalId = staff.HospitalId;
                s.JobDescription = staff.JobDescription;

                staffDetails.Add(s);
            }

            foreach(VaccineBatch batch in batches)
            {
                VaccinationDetails v = new VaccinationDetails();

                v.Id = batch.Id;
                v.Type = batch.Type;
                v.ExpirationDate = batch.ExpirationDate;
                v.ProducedDate = batch.ProducedDate;
                v.BatchId = batch.BatchId;

                vaccinationDetails.Add(v);
            }

            GetCreateProgramData getCreateProgramData = new GetCreateProgramData();
            getCreateProgramData.StaffDetails = staffDetails;
            getCreateProgramData.VaccinationDetails = vaccinationDetails;
            getCreateProgramData.CitizenDetails = citizenDetails;

            return Ok(getCreateProgramData);
        }

        
        [HttpPost]
        [Route("createprogram")]
        
        public async Task<IActionResult> CreateProgram(CreateProgramData createProgramData)
        {
            List<Citizen> citizenguids = new List<Citizen>();

            String[] citizenIDs = createProgramData.CitizenIDs.Split(',');
            foreach(String CitizenId in citizenIDs)
            {
                Citizen? citizen = await this.mainDatabase.citizens.FindAsync(Guid.Parse(CitizenId));
                if(citizen != null)
                {
                    citizenguids.Add(citizen);
                    String currentdates = citizen.VaccinationDate;
                    citizen.Pending = true;
                    currentdates = currentdates +","+ createProgramData.Date;
                    citizen.VaccinationDate = currentdates;
                    this.mainDatabase.Entry(citizen).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

            }

            ICollection <VaccineBatch> vaccineguids = new List<VaccineBatch>();
            String[] vaccineIds = createProgramData.VaccineIDs.Split(',');

            foreach(String VaccineId in vaccineIds)
            {
                VaccineBatch vaccineBatch = await this.mainDatabase.vaccineBatches.FindAsync(Guid.Parse(VaccineId));
                if(vaccineBatch != null)
                {
                    vaccineguids.Add(vaccineBatch);
                }
            }

            ICollection<Staff> staffguids = new List<Staff>();

            String[] staffIds = createProgramData.StaffIds.Split(',');

            foreach (String staffId in staffIds)
            {
                Staff? staff = await this.mainDatabase.staffs.FindAsync(Guid.Parse(staffId));
                if(staff != null)
                {
                    staffguids.Add(staff);
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
                VaccineBatch? vaccineBatch1 = await this.mainDatabase.vaccineBatches.FindAsync(vaccineBatch.Id);

                if(vaccineBatch1 != null)
                {
                    ICollection<VaccineProgram> vaccinePrograms = new List<VaccineProgram>();
                    vaccinePrograms.Add(vaccineProgram);
                    vaccineBatch1.VaccinePrograms = vaccinePrograms;
                }
            }


            foreach(var staff in staffguids)
            {
                Staff? staff1 = await this.mainDatabase.staffs.FindAsync(staff.Id);

                if(staff1 == null)
                {
                    return BadRequest();
                }

                if(staff1.VaccinePrograms == null)
                {
                    staff1.VaccinePrograms = new VaccineProgram[0];
                }

                if(staff1 != null)
                {

                    ICollection<VaccineProgram> vaccinePrograms = new List<VaccineProgram>();
                    vaccinePrograms.Add(vaccineProgram);
                    staff1.VaccinePrograms = vaccinePrograms;
                }
            }
            
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
            await this.mainDatabase.vaccinePrograms.AddAsync(vaccineProgram);
            await this.mainDatabase.SaveChangesAsync();
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

            List<PostCitizenDetails> selectedCitizens = new List<PostCitizenDetails>();

            foreach(var citizen in citizens)
            {
                if(citizen.Pending == true)
                {

                }

                else
                {
                    if(citizen.VaccinationCount == 0)
                    {
                        PostCitizenDetails postCitizenDetails = new PostCitizenDetails()
                        {
                            VaccinationCount = citizen.VaccinationCount,
                            Address = citizen.Address, 
                            BirthDate = citizen.BirthDate,
                            CitizenID = citizen.CitizenID,
                            EmailAddress = citizen.EmailAddress,
                            Id = citizen.Id,
                            Name = citizen.Name,
                            OtherDiseases = citizen.OtherDiseases,
                            Pending = true,
                            PhoneNumber = citizen.PhoneNumber,
                            ReportData = citizen.ReportData,
                            Status = citizen.Status,
                            VaccinationDate = citizen.VaccinationDate,
                            VaccineProgram = citizen.VaccineProgram
                        };
                        selectedCitizens.Add(postCitizenDetails);
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
                            PostCitizenDetails postCitizenDetails = new PostCitizenDetails()
                            {
                                VaccinationCount = citizen.VaccinationCount,
                                Address = citizen.Address,
                                BirthDate = citizen.BirthDate,
                                CitizenID = citizen.CitizenID,
                                EmailAddress = citizen.EmailAddress,
                                Id = citizen.Id,
                                Name = citizen.Name,
                                OtherDiseases = citizen.OtherDiseases,
                                Pending = true,
                                PhoneNumber = citizen.PhoneNumber,
                                ReportData = citizen.ReportData,
                                Status = citizen.Status,
                                VaccinationDate = citizen.VaccinationDate,
                                VaccineProgram = citizen.VaccineProgram
                            };
                            selectedCitizens.Add(postCitizenDetails);
                        }
                    }
                }

            }

            return Ok(selectedCitizens);
        }

        [HttpGet]
        [Route("getStaff")]

        public async Task<IActionResult> GetStaff()
        {
            List<Staff> staffs = await this.mainDatabase.staffs.ToListAsync();

            List<PostStaff> postStaffs = new List<PostStaff>();

            foreach(Staff staff in staffs)
            {
                PostStaff postStaff = new PostStaff()
                {
                     Address = staff.Address,
                     CitizenId = staff.CitizenId,
                     Id = staff.Id,
                     DateofBirth = staff.DateofBirth,
                     Email = staff.Email,
                     HospitalId = staff.HospitalId,
                     JobDescription = staff.JobDescription,
                     Name = staff.Name,
                     PhoneNumber = staff.PhoneNumber
                };

                postStaffs.Add(postStaff);
            }

            return Ok(postStaffs);
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

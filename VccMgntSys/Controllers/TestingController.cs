
using VccMgntSys.Mail_System;
using Microsoft.AspNetCore.Mvc;

namespace VccMgntSys.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestingController : Controller
    {
        private readonly IMailService mailService;

        public TestingController(IMailService mailService) { this.mailService = mailService; }



    }
}

namespace VccMgntSys.Mail_System
{
    public class SendingMail
    {
        private readonly MailService _mailService;

        public SendingMail()
        {
            this._mailService = new MailService();
        }
    }
}

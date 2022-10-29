namespace VccMgntSys.Mail_System
{
    public interface IMailService
    {
        Task SendMailAsync(MailRequest mailRequest);
    }
}

namespace VccMgntSys.Mail_System
{
    public static class MailContent
    {
        public static String getMailContent(String name,String date, String location)
        {
            return "Dear " + name + "\n"+"Your next Vaccination is scheduled on "+ date +" "+location +". If you wish to change the date please log on to the system and change the details" ;
        }

        public static String getMailSubject()
        {
            return "Your Next Vaccination of " ;
        }
    }
}

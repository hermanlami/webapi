using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementSystem.BLL
{
    public static class Mail
    {
        public static async Task DeadlineNotification(string toEmail, double daysLeft)
        {
            string fromEmail = "hermanlami991@gmail.com";
            string password = "dqvp roym ilie xopz";
            var days = daysLeft > 1 ? daysLeft.ToString()+" days" : "today";
            string message =$"Your task is ending in {days}!";
            var email = EmailStructure(fromEmail, toEmail, message, "Task Deadline");

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new NetworkCredential(fromEmail, password);
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(email);
            }
            catch (Exception ex)
            {

            }
        }
        private static MailMessage EmailStructure(string emailFrom, string EmailTo, string emailBody, string subject)
        {
            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.From = new MailAddress(emailFrom);
            mail.To.Add(new MailAddress(EmailTo));
            mail.Body = emailBody;
            return mail;
        }

    }
}

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
        public static void DeadlineNotification(string toEmail, string taskName, double daysLeft)
        {
            var days = daysLeft <= 0.625 ? daysLeft.ToString() + " today" : $"in {Math.Floor(daysLeft)} days";
            string message = $"Your task {taskName} is ending {days}!";
            EmailStructure(toEmail, message, "Task Deadline");
        }

        public static void CredentialsNotification(string toEmail, string password)
        {
            string message = @$"Your login credentials are
Email: {toEmail}
Password: {password}";
            EmailStructure(toEmail, message, "Login Credentials");
        }

        public static void TaskCompletionResponse(string toEmail, string response)
        {
            string message = $"Your task completion was {response}";
            EmailStructure(toEmail, message, "Login Credentials");
        }

        private static void EmailStructure(string toEmail, string emailBody, string subject)
        {
            string fromEmail = "hermanlami991@gmail.com"; 
            string password = "dqvp roym ilie xopz";  

            using (MailMessage mail = new MailMessage())
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                mail.Subject = subject;
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(new MailAddress(toEmail));
                mail.Body = emailBody;

                smtpClient.Credentials = new NetworkCredential(fromEmail, password);
                smtpClient.EnableSsl = true;

                try
                {
                    smtpClient.Send(mail);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }

}

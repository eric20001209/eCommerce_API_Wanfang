using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;

namespace eCommerce_API.Services
{
    public class MailService : iMailService
    {
        [EmailAddress]
        public string email { get; set; }
        public string password { get; set; }

        public void sendEmail(string email, string password)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("gpossuport2176@gmail.com");
                message.To.Add(new MailAddress(email));
                message.Subject = "Test";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = "Hi, this is your new password<br>";
                message.Body += "<b>NEW PASSWORD</b> is (" + password + ") !!";
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential("gpossuport2176@gmail.com", "suocqnxvfxaqvrjd"); 

                smtp.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }
        }
    }
}

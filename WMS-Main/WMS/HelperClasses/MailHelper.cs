using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using WareHouseMVC.Models;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace WareHouseMVC.HelperClasses
{
    public class MailHelper
    {
        private WareHouseMVCContext context = new WareHouseMVCContext();
        public void SendMail(string MailTo, string Subject, string Body, string ccMail = null)
        {

            //HostInformation hostinformation = new HostInformation();
            //hostinformation = context.HostInformations.FirstOrDefault();

            try
            {
                #region SMTP Configuration
                string smtpServer = ConfigurationManager.AppSettings["SmtpServer"].ToString(); // hostinformation.SMTPServer;
                string smtpPort = ConfigurationManager.AppSettings["SmtpPort"].ToString(); // hostinformation.SMPTPost;
                string smtpEmail = ConfigurationManager.AppSettings["SmtpEmail"].ToString(); // hostinformation.SMTPEmail;
                string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"].ToString(); // hostinformation.SmtpPassword;
                string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
                string AdminMail = ConfigurationManager.AppSettings["AdminMailAddress"].ToString(); // hostinformation.AdminMailAddress;
                #endregion

                MailMessage message = new MailMessage();
                message.From = new MailAddress(MailFrom);
                message.To.Add(MailTo);

                if (!string.IsNullOrEmpty(ccMail))
                {
                    message.CC.Add(ccMail);
                }

                message.Subject = Subject;
                message.Body = Body;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = int.Parse(smtpPort),
                    Credentials = new NetworkCredential(smtpEmail, smtpPassword),
                    EnableSsl = true,
                };

                try
                {
                    smtpClient.Send(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception)
            {
            }

        }
    }
}
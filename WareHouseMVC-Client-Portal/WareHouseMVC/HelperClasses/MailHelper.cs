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
using System.Net.Mime;

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

        internal void SendMailAttachment(string MailTo, string Subject, string Body)
        {
            HostInformation hostinformation = new HostInformation();
            hostinformation = context.HostInformations.FirstOrDefault();

            try
            {
                #region SMTP Configuration
                string host = hostinformation.SMTPServer;// ConfigurationManager.AppSettings["SmptServer"].ToString();
                string userName = hostinformation.SMTPEmail;// ConfigurationManager.AppSettings["SmtpEmail"].ToString();
                string password = hostinformation.SmtpPassword;// ConfigurationManager.AppSettings["SmtpPassword"].ToString();
                string port = hostinformation.SMPTPost;// ConfigurationManager.AppSettings["SmtpPort"].ToString();
                string MailFrom = ConfigurationManager.AppSettings["BaseEmail"].ToString();
                string AdminMail = hostinformation.AdminMailAddress;// ConfigurationManager.AppSettings["AdminMailAddress"].ToString();


                #endregion



                var clientWeb = new WebClient();

                // Download the PDF file from external site (pdfUrl) 
                // to your local file system (pdfLocalFileName)

                Guid guid = Guid.NewGuid();
                


                string pdfLocalFileName ="D:\\WebSitePublished\\Content\\Attachment\\UpcomingDestructionBoxes-"+guid.ToString()+".xls";

                clientWeb.DownloadFile("http://localhost:4362/Reports/DestructionPeriodReportAttachment?clientId=47&monthCount=1&format=2", pdfLocalFileName);  


                string address = MailFrom;
                SmtpClient client = new SmtpClient(host)
                {
                    Credentials = new NetworkCredential(userName, password),
                    Port = Convert.ToInt32(port),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                MailMessage message = new MailMessage
                {
                    From = new MailAddress(address)
                };

                ServicePointManager.ServerCertificateValidationCallback =
    delegate(object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };


                message.CC.Add(AdminMail);
                message.To.Add(MailTo);

                message.Subject = Subject;

                message.Body = Body;
                message.IsBodyHtml = true;

             Attachment   attachment = new Attachment(pdfLocalFileName);
              message.Attachments.Add(attachment);
                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
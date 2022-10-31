using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.ServerHealthService.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Net.Http;

namespace Logitude.Customs.CustomsMessaging.Tasks
{
    public class SendReportExel : ICustomsSendReportExel
    {
        public async void StartRun(string taskId, int seedDefaultTenant)
        {

            try
            {
                
                string serviceUrl = string.Format(LogitudeSettings.LogitudeURL+"api/DeclarationCourierStatusWebService/GetExportReport2Excel?tenant={0}&ExportFromDate={1}&ExportToDate={2}", 0, DateTime.Now.AddDays(-30), DateTime.Now);

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(serviceUrl);


                string subject = " דוחות יצוא" + DateTime.Now.ToString();
                string body = "מייל זה נשלח אוטומטי נא לא להשיב למייל זה";
                SendEmails(subject, body, response);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
            protected  void SendEmails(string subject, string body, HttpResponseMessage response)
            {
                try
                {                  
                    Stream data = response.Content.ReadAsStreamAsync().Result;
               
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                      | SecurityProtocolType.Tls11
                                      | SecurityProtocolType.Tls12;
                    System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                    MailMessage mailMessage = new MailMessage();

                    mailMessage.From = new MailAddress("amitarltest123@gmail.com");

                    mailMessage.To.Add("tzuri@AMITAL.CO.IL");
                    mailMessage.CC.Add("sh254256@gmail.com");               
                    mailMessage.Attachments.Add(new Attachment(data, response.Content.Headers.ContentDisposition.FileName));
                    mailMessage.BodyEncoding = Encoding.UTF8;
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    smtpClient.Port = 587;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential("amitarltest123@gmail.com", "xusckhroahgobvxg");
                    smtpClient.Send(mailMessage);


                }
                catch (Exception exception)
                {

                }
            }



        }
}

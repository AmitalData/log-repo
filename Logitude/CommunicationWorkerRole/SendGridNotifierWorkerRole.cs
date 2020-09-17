using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebFreight.Web.WebServices;

namespace CommunicationWorkerRole
{
    public class SendGridNotifierWorkerRole : WorkerEntryPoint
    {
        private string queueName = "sendgridnotifyqueue";
        private int Tenant;

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        this.Tenant = 0;

                        queueservice = QueueServiceManager.GetQueueService(queueName, 0);
                        var response = queueservice.Receive();
                        LastActivity = DateTime.UtcNow;
                        if (response.MessageId != null)
                        {
                            try
                            {
                                int.TryParse(response.MessageValues["Tenant"].ToString(), out Tenant);
                                string EmailDeliveryError = response.MessageValues["EmailDeliveryError"].ToString();
                                string CreatedByUserId = response.MessageValues["CreatedByUserId"].ToString();
                                string EmailSubject = response.MessageValues["EmailSubject"].ToString();
                                string CurrentLogId = response.MessageValues["CurrentLogId"].ToString();
                                this.CreateCommunicationLog(EmailDeliveryError, CreatedByUserId, EmailSubject, CurrentLogId);

                                LogDoneItemInMemory();
                                queueservice.Complete();
                            }

                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "WorkerRole", "", null);
                                queueservice.Complete();
                                Thread.Sleep(10000);
                            }
                        }

                        //continue
                        Thread.Sleep(10000);
                    }

                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, null, "SendGridNotifierWorkerRole Run method", null, null);
                        Thread.Sleep(10000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private void CreateCommunicationLog(string emailDeliveryError, string createdByUserId, string EmailSubject, string CurrentLogId)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(Tenant);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            ContactRepository contactRepository = new ContactRepository(commonContext);
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();

            Contact contact = contactRepository.GetSingleContact(createdByUserId, Tenant);
            string contactEmail = contact!= null ? contact.Email : "";
            string contactName = contact != null ? contact.EnglishName : "";

            string body = BuildAlertEmailHTML(emailDeliveryError, contactName);
            byte[] bytearray = enc.GetBytes(body);
           
            
            string fromemail = "no-reply@" + (LogitudeSettings.WorkEnvironment == "cloud" ? "amital.co.il" : LogitudeSettings.DeploymentStage != null && (LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2") ? "logbox.co.il" : "LogitudeWorld.com");

            CommunicationLog emailCommunication = communicationLogRepository.GetSingleCommunicationLog(CurrentLogId, Tenant);
            if (emailCommunication != null)
            {
                if (!string.IsNullOrEmpty(emailCommunication.From))
                {
                    if (emailCommunication.From.Contains("no-reply@")) fromemail = emailCommunication.From;
                }
            }


           
            string subject = "Email Delivery Failure : " + EmailSubject;

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "html",
                FileSize = bytearray.Length,
                Tenant = Convert.ToInt32(Tenant),
                Id = IdCounter.GetNumber("Document", Tenant),
                HasFile = true,
                Folder = "sendgridalert",
            };
            documentRepository.Add(document);

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = contactEmail,
                InOut = "O",
                From = fromemail,
                Subject = subject,
                Tenant = Tenant,
                CommunicationLogTypeCode = "E",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = createdByUserId,
                DocumentId = document.Id,
                SearchFields = "sendgridnotifier" + "," + subject,
                CreateDateUTC = DateTime.UtcNow,
            };

            communicationLogRepository.Add(commLog);


            if (emailCommunication != null)
            {
                CommunicationAttachment attachment = new CommunicationAttachment()
                {
                    Id = IdCounter.GetNumber("CommunicationAttachment", Tenant).ToString(),
                    CommunicationLogId = commLog.Id,
                    DocumentId = emailCommunication.DocumentId,
                    Tenant = Tenant,
                };
                commonContext.CommunicationAttachments.Add(attachment);
            }

            string myDocumentId = document.Id;
            string myDocumentFolder = document.Folder;
            string myDocumentExtension = document.Extension;
            string myCommunicationLogId = commLog.Id;

            commonContext.SaveChanges();

            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = commLog.Document.Id,
                FolderName = commLog.Document.Folder,
                Extension = commLog.Document.Extension,
                Tenant = commLog.Document.Tenant,
                FileSize = bytearray.Length,
            };
            Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
            storageservice.Write(bytearray, fileInfo);

            try
            {
				//IQueueService queueservice = QueueServiceManager.GetQueueService("emailqueue", Tenant);
				//Dictionary<string, string> message = new Dictionary<string, string>()
				//    {
				//        { "CommunicationLogId", myCommunicationLogId},
				//        { "Tenant", Tenant.ToString() },
				//    };

				//queueservice.Send(message);

				DbQueueService queueservice = new DbQueueService("EmailQueue", Tenant);
				queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", myCommunicationLogId }, { "Tenant", Tenant.ToString() } }, Tenant);
			}

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Send Grid Notifier Communicationlog", null, ip);
            }
        }

        private string BuildAlertEmailHTML(string error, string name)
        {
            StringBuilder HtmlTemplate = new StringBuilder();
            StringBuilder EnvelopeHtmlTemplate = new StringBuilder();
            List<string> emails = error.Split('\n').ToList();
 
            EnvelopeHtmlTemplate.Append("<div style='text-align:left;font-size:16px;'>");
            EnvelopeHtmlTemplate.Append("<p style='text-align:left;color:#000066;font-size:16px;'>");
            EnvelopeHtmlTemplate.Append("Dear " + name + ",");
            EnvelopeHtmlTemplate.Append("</p>");
            EnvelopeHtmlTemplate.Append("<br/>");
            HtmlTemplate.Append("<p style='text-align:left;color:#000066;font-size:16px;font-weight: bold;'> Delivery has failed to these recipients or groups: </p>");
            HtmlTemplate.Append("<br/>");

            emails = emails.Where(s => !string.IsNullOrWhiteSpace(s) && !string.IsNullOrEmpty(s)).ToList();
            foreach (var item in emails)
            {
                HtmlTemplate.Append("<p style='text-align:left;font-size:16px;'>" + item + "</p><br/>");
            }

            EnvelopeHtmlTemplate.Append(HtmlTemplate.ToString());
            EnvelopeHtmlTemplate.Append("<br/><br/>");
            EnvelopeHtmlTemplate.Append("</div>");
            EnvelopeHtmlTemplate.Append("<p style='vertical-align:top;display:table;text-align:center;font-size:16px;'>");
            EnvelopeHtmlTemplate.Append("Created By <b>" + LogitudeSettings.WorkEnvironment + "</b>");
           
            EnvelopeHtmlTemplate.Append("</p>");
            return EnvelopeHtmlTemplate.ToString();
        }

        public override bool OnStart()
        {
            ConnectClient(); // mohammad to try reconnect in case of disconnected client. 23-7-15
            ServicePointManager.DefaultConnectionLimit = 12;
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "SendGridNotifierWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            return base.OnStart();
        }

        IQueueService queueservice;
        public void ConnectClient()
        {
            try
            {
                queueservice = QueueServiceManager.GetQueueService(queueName, 0);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}

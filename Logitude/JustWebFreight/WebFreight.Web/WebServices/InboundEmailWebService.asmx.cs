using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Services;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for InboundEmailWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class InboundEmailWebService : System.Web.Services.WebService
    {
        public int Tenant { get; set; }
        public string LoggedContactId { get; set; }

        [WebMethod]
        public void SendInboundEmail(string toEmails, string subject, string body, int tenant, string emailId)
        {
            this.Tenant = tenant;
            byte[] bytearray = Encoding.ASCII.GetBytes(body);

            this.GetLoggedContactData();

            ICommonDataContext commonContext = CommonDataContext.GetContext(Tenant);
            IWebFreightContext webContext = WebFreightContext.GetContext(Tenant); 
            ICRMContext cemcontext = CRMContext.GetContext(Tenant);

            DocumentRepository documentRepository = new DocumentRepository(commonContext);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            InboundEmail myHeader = null;
            InboundEmailRepository myHeaderRep = new InboundEmailRepository(webContext);
            InboundEmailLineRepository emailLinerepository = new InboundEmailLineRepository(webContext);

            TicketRepository ticketrep = new TicketRepository(cemcontext);
           
            string InboundEmailId=null;
            string InboundEmailLineId = IdCounter.GetNumber("InboundEmailLine", tenant).ToString();

            // Create InboundEmail
            if (string.IsNullOrWhiteSpace(emailId))
            {
                InboundEmailId = IdCounter.GetNumber("InboundEmail", tenant).ToString();
                myHeader = new InboundEmail()
                {
                    Id = InboundEmailId,
                    Tenant = Tenant,
                    EntityId = "1-1",
                    ObjectTableId = "1-1",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    Uniquekey = InboundEmailId,
                };
                myHeaderRep.Add(myHeader);
            }
            // Update on InboundEmail
            else
            {
                InboundEmailId = emailId;
                myHeader = myHeaderRep.webFreightContext.InboundEmails.Where(d => d.Id == InboundEmailId).FirstOrDefault();
            }

            // Note Must change this to guid - Maheera
            string senderEmail = "support+" + InboundEmailId + "@sandboxf630eae2c7034dd681286d71bd2f47bf.mailgun.org";
           
            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = bytearray.Length,
                Tenant = Convert.ToInt32(Tenant),
                Id = IdCounter.GetNumber("Document", Tenant),
                HasFile = true,
                Folder = "mailgun",
            };

            documentRepository.Add(document);

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = toEmails,
                InOut = "O",
                From =senderEmail,
                Subject = subject,
                Tenant = Tenant,
                CommunicationLogTypeCode = "E",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                CommunicationStatusTypeCode = "W",
                CreatedByUserId = LoggedContactId,
                DocumentId = document.Id,
                SearchFields = "mailgun" + "," + "O" + "," + subject,
                CreateDateUTC = DateTime.UtcNow,
            };

            InboundEmailLine email = new InboundEmailLine()
            {
                Id = InboundEmailLineId,
                Tenant = tenant,
                Sender = senderEmail,
                Recepient = toEmails,
                Subject = subject,
                Body = body,
                CCs = "",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                Direction = "O",
                InboundEmailId = myHeader.Id,
                CommunicationLogId = commLog.Id,
                EntityLineId = "1-1",
            };
            emailLinerepository.Add(email);

            communicationLogRepository.Add(commLog);
          
            string myDocumentId = document.Id;
            string myDocumentFolder = document.Folder;
            string myDocumentExtension = document.Extension;
            string myCommunicationLogId = commLog.Id;
                                                 
            commonContext.SaveChanges();
            webContext.SaveChanges();

            BlobFileInfo fileInfo = new BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = Tenant,
                FileSize = bytearray.Length,
                IsEncrypted = true,
            };
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            storageservice.Write(bytearray, fileInfo);

            try
            {
				//using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())//TransactionFactory.GetNewTransaction())
				//{
				//    BrokeredMessage message = new BrokeredMessage();

				//    message.Properties["CommunicationLogId"] = myCommunicationLogId;
				//    message.Properties["Tenant"] = Tenant;

				//    string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment("emailqueue");//"emailqueue"
				//    QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName);
				//    client.Send(message);

				//    scope.Complete();
				//}

			 
				DbQueueService queueservice = new DbQueueService("EmailQueue", tenant);
				queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", myCommunicationLogId }, { "Tenant", tenant.ToString() } });
			}

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "champ web service", null, ip);
            }
        }

        private void GetLoggedContactData()
        {
            ContactRepository myContactRepository = new ContactRepository(Tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.Contact myContact = myContactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), Tenant);
            if (myContact != null)
            {
                this.LoggedContactId = myContact.Id;
                
            }
        }
    }
}

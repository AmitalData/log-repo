using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Helpers;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using WebFreight.Web.Security;

namespace CommunicationWorkerRole
{
    public class InboundEmailWorkerRole : WorkerEntryPoint
    {
        CorrespondenceRepository correspondenceRep;
        ContactRepository contactRep;
        UserRepository userRep;

        InboundEmailLineRepository inboundEmailLineRepository;
        DocumentRepository documentRepository;
        CommunicationLogRepository communicationLogRepository;
        InboundEmailLine newInboundEmailLine;
        ICommonDataContext commonContext;

        public string LoggedContactId { get; set; }
        public string UserName { get; set; }
        public string StageName { get; set; }
        public string contactEmail = "";
        public string GuidId = "";
        public string EntityId = "";
        public string ChildObjectTableId = "";
        public string ObjectTableId = "";

        List<DocumentsFilingPM> DocumentsFilings;
        User TicketUser;

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        int tenant = 0;
                        queueservice = new DbQueueService(queueName, tenant);//QueueServiceManager.GetQueueService(queueName, 0);
                        var response = queueservice.Receive();
                        LastActivity = DateTime.UtcNow;
                        if (response.MessageId != null)
                        {
                            try
                            {
                                int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
                                string lineId = response.MessageValues["InboundEmailLineId"].ToString();
                                string ticketNumber = response.MessageValues["TicketNumber"].ToString();
                                this.EntityId = response.MessageValues["EntityId"].ToString();
                                string contactId = response.MessageValues["ContactId"] != null ? response.MessageValues["ContactId"].ToString() : "";
                                string ownerId = response.MessageValues["OwnerId"] != null ? response.MessageValues["OwnerId"].ToString() : "";
                                GuidId = response.MessageValues["GuidId"].ToString();
                                ChildObjectTableId = response.MessageValues["ChildObjectTableId"].ToString();
                                ObjectTableId = response.MessageValues["ObjectTableId"].ToString();
                                LoggedContactId = response.MessageValues["CurrentLoggedUserId"].ToString();
                                UserName = this.GetUserName(LoggedContactId, tenant);
                                this.CreateCommunicationLog(lineId, tenant, ticketNumber, contactId, ownerId);
                                queueservice.Complete();
                                LogDoneItemInMemory();
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "InboundEmail WorkerRole Run Method", "", null);
                                queueservice.CompleteAsFailed();
                                Thread.Sleep(10000);
                            }
                        }

                        Thread.Sleep(10000);
                    }

                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "InboundEmail WorkerRole Method Run", null, null);
                        Thread.Sleep(10000);
                        throw;
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private void CreateCommunicationLog(string lineId, int tenant, string ticketNumber, string contactId, string ownerId)
        {
            try
            {
                inboundEmailLineRepository = new InboundEmailLineRepository(tenant);
                newInboundEmailLine = inboundEmailLineRepository.GetSingleInboundEmailLine(lineId, tenant);
                if (newInboundEmailLine != null)
                {
                    commonContext = CommonDataContext.GetContext(tenant);
                    documentRepository = new DocumentRepository(commonContext);
                    communicationLogRepository = new CommunicationLogRepository(commonContext);

                    correspondenceRep = new CorrespondenceRepository(tenant);
                    contactRep = new ContactRepository(tenant);
                    userRep = new UserRepository(tenant);
                    List<InboundEmailLine> oldLines = new List<InboundEmailLine>();
                    DocumentsFilings = new List<DocumentsFilingPM>();

                    oldLines = inboundEmailLineRepository.GetInboundEmailLinesByInboudEmailId(newInboundEmailLine.InboundEmailId, newInboundEmailLine.Tenant);
                    Correspondence myCorrespondenceLine = correspondenceRep.GetSingle(newInboundEmailLine.EntityLineId, tenant);
                    StageName = GetTicketStageName(myCorrespondenceLine.EntityId, myCorrespondenceLine.ObjectTableId, tenant);

                    contactEmail = contactRep.GetEmailContactByIdAndTenant(tenant, contactId);
                    string ownerEmail = contactRep.GetEmailContactByIdAndTenant(tenant, ownerId);
                    string sender = newInboundEmailLine.Sender;
                    string loggedUserEmail = contactRep.GetEmailContactByIdAndTenant(tenant, LoggedContactId);

                    TicketUser = userRep.GetSingleUser(LoggedContactId, tenant);

                    string myHTMLBody = null;
                    byte[] bytearray = null;
                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();

                    #region Read Documents
                    DocumentsFilingQuery docQuery = new DocumentsFilingQuery(tenant);
                    InboundEmailRepository myRep = new InboundEmailRepository(tenant);
                    string inboundEmailId = oldLines.FirstOrDefault().InboundEmailId;
                    InboundEmail myInboundEmail = myRep.GetSingleInboundEmail(inboundEmailId, tenant);
                    if (myInboundEmail != null)
                    {
                        DocumentsFilings = docQuery.GetDocumentsFilingPMsByEntityId(tenant, myInboundEmail.EntityId, myInboundEmail.ObjectTableId);
                    }
                    #endregion

                    string cCs_Emails = "";
                    string contacts_Emails = contactEmail;
                    string internals_Emails = "";

                    if (!string.IsNullOrEmpty(newInboundEmailLine.CCs))
                    {
                        List<string> myEmails = newInboundEmailLine.CCs.Split(';').ToList<string>().ConvertAll(d => d == null ? d : d.ToLower());
                        if (myEmails.Contains(contactEmail))
                        {
                            myEmails.RemoveAll(x => x == contactEmail);
                        }

                        if (!string.IsNullOrEmpty(newInboundEmailLine.InternalUsers))
                        {
                            List<string> myEmails_Internal = newInboundEmailLine.InternalUsers.Split(';').ToList<string>().ConvertAll(d => d == null ? d : d.ToLower());
                            if (myEmails_Internal != null && myEmails_Internal.Count() > 0)
                            {
                                myEmails = myEmails.Except(myEmails_Internal).ToList();
                            }
                        }
                        cCs_Emails = string.Join(";", myEmails).TrimEnd(';');
                    }

                    if (!string.IsNullOrEmpty(newInboundEmailLine.InternalUsers))
                    {
                        List<string> myEmails = newInboundEmailLine.InternalUsers.Split(';').ToList<string>().ConvertAll(d => d == null ? d : d.ToLower());
                        if (myEmails.Contains(contactEmail))
                        {
                            myEmails.RemoveAll(x => x == contactEmail);
                        }
                        internals_Emails = string.Join(";", myEmails).TrimEnd(';');
                    }

                    #region Internal Email
                    if (myCorrespondenceLine.IsInternal)
                    {
                        myHTMLBody = this.BuildInternalHTMLBody(oldLines, ticketNumber);
                        bytearray = enc.GetBytes(myHTMLBody);
                        if (!string.IsNullOrEmpty(internals_Emails))
                        {
                            this.SendEmailCc(internals_Emails, tenant, bytearray, true);
                        }

                        if (myCorrespondenceLine.NotifyMe)
                        {
                            this.SendEmailTo(loggedUserEmail, tenant, bytearray, true);
                        }

                        if (myCorrespondenceLine.NotifyOwner)
                        {
                            this.SendEmailTo(ownerEmail, tenant, bytearray, true);
                        }
                    }
                    #endregion 

                    #region External Email 
                    else
                    {
                        myHTMLBody = this.BuildHTMLBody(oldLines, ticketNumber, false, false);
                        bytearray = enc.GetBytes(myHTMLBody);

                        cCs_Emails = this.FilterCCsEmails(cCs_Emails);

                        if (!string.IsNullOrEmpty(cCs_Emails))
                        {
                            this.SendEmailCc(cCs_Emails, tenant, bytearray, false);
                        }

                        if (!string.IsNullOrEmpty(contactEmail) && !string.Equals(contactEmail, newInboundEmailLine.Sender, StringComparison.CurrentCultureIgnoreCase))
                        {
                            this.SendEmailTo(contactEmail, tenant, bytearray, false);
                        }

                        myHTMLBody = this.BuildHTMLBody(oldLines, ticketNumber, true, false);
                        bytearray = enc.GetBytes(myHTMLBody);
                        if (!string.IsNullOrEmpty(internals_Emails))
                        {
                            this.SendEmailCc(internals_Emails, tenant, bytearray, true);
                        }

                        if (myCorrespondenceLine.NotifyMe)
                        {
                            this.SendEmailTo(loggedUserEmail, tenant, bytearray, false);
                        }

                        myHTMLBody = this.BuildHTMLBody(oldLines, ticketNumber, false, true);
                        bytearray = enc.GetBytes(myHTMLBody);
                        if (myCorrespondenceLine.NotifyOwner)
                        {
                            this.SendEmailTo(ownerEmail, tenant, bytearray, false);
                        }
                    }
                    #endregion 
                }
            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "InboundEmailWorkerRole : Run() Method", null);
                Thread.Sleep(5000);
            }
        }

        private string FilterCCsEmails(string emailsText)
        {
            string myResult = null;

            if (!string.IsNullOrEmpty(emailsText))
            {
                string[] emails = emailsText.Split(';');

                foreach (string email in emails)
                {
                    string iEmail = email.Replace(";", "").ToLower();

                    if (!this.IsSupportEmail(iEmail))
                    {
                        if (!string.IsNullOrEmpty(iEmail))
                        {
                            if (this.IsEmail(iEmail))
                            {
                                if (string.IsNullOrEmpty(myResult))
                                {
                                    myResult = iEmail;
                                }

                                else
                                {
                                    myResult += ";" + emails;
                                }
                            }
                        }
                    }
                }
            }

            return myResult;
        }
        private bool IsSupportEmail(string email)
        {
            bool myResult = false;

            if (!string.IsNullOrEmpty(email))
            {
                email = email.ToLower();

                switch (email)
                {
                    case "s@test.unifreight.co.il":
                    case "support@ilcargo.com":
                    case "support@icl.unifreight.co.il":
                        {
                            myResult = true;
                            break;
                        }
                }
            }

            return myResult;
        }
        private bool IsEmail(string email)
        {
            var myResult = true;

            if (!string.IsNullOrEmpty(email))
            {

                Regex isEmail = new Regex(@"^(([^<>()[\]\\.,;:\s@\""]+"
                                        + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                                        + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                        + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                                        + @"[a-zA-Z]{2,}))$");

                if (!isEmail.IsMatch(email))
                {
                    myResult = false;
                }


            }

            return myResult;
        }

        private string GetTicketStageName(string Id, string objectTableId, int tenant)
        {
            string stageName = "";
            ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTableRepository.GetObjectTableByName("Ticket", 0, true);
            if (objectTable.Id == objectTableId)
            {
                TicketRepository ticketRep = new TicketRepository(tenant);
                Ticket ticket = ticketRep.GetSingle(Id, tenant);
                TicketStageRepository stageRep = new TicketStageRepository(tenant);
                TicketStage ticketStage = stageRep.GetSingle(ticket.StageId, ticket.Tenant);
                stageName = ticketStage.Name;
            }

            return stageName;
        }

        private void SendEmailCc(string emails, int Tenant, byte[] bytearray, bool isInternal)
        {
            string sender = newInboundEmailLine.Sender;

            if (!string.IsNullOrEmpty(sender))
            {
                sender = sender.ToLower();
            }

            List<string> myEmails = emails.Split(';').ToList<string>().ConvertAll(d => d == null ? d : d.ToLower());

            if (myEmails.Contains(sender))
            {
                myEmails.RemoveAll(x => myEmails.Contains(sender));
            }
            string myEmail = string.Join(";", myEmails);

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "html",
                FileSize = bytearray.Length,
                Tenant = Convert.ToInt32(Tenant),
                Id = IdCounter.GetNumber("Document", Tenant),
                HasFile = true,
                Folder = "mailgun",
            };

            documentRepository.Add(document);

            string replayToList = "";

            if (newInboundEmailLine.Direction == "I")
            {
                if (isInternal)
                {
                    string replayTo = newInboundEmailLine.Recepient;
                    replayToList = replayTo.Split('@')[0] + "-in" + "@" + replayTo.Split('@')[1];
                }

                else
                {
                    string replayTo = newInboundEmailLine.Recepient;
                    replayToList = replayTo.Split('@')[0] + "-ex" + "@" + replayTo.Split('@')[1];
                }
            }

            else
            {
                if (isInternal)
                {
                    string replayTo = newInboundEmailLine.Sender;
                    replayToList = replayTo.Split('@')[0] + "-in" + "@" + replayTo.Split('@')[1];
                }

                else
                {
                    string replayTo = newInboundEmailLine.Sender;
                    replayToList = replayTo.Split('@')[0] + "-ex" + "@" + replayTo.Split('@')[1];
                }
            }

            if (string.IsNullOrEmpty(UserName))
            {
                UserName = "Support";
            }

            string tenantEmail = GetTenantManagementEmail(Tenant);
            string from = "<" + UserName + "> " + tenantEmail;

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = "",
                CC = myEmail,
                InOut = "O",
                From = from,
                EntityId = EntityId,
                ChildEntityId = newInboundEmailLine.EntityLineId,
                ChildObjectTableId = ChildObjectTableId,
                ObjectTableId = ObjectTableId,
                Subject = newInboundEmailLine.Subject,
                Tenant = Tenant,
                CommunicationLogTypeCode = "E",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                CommunicationStatusTypeCode = "W",
                //CreatedByUserId = TicketUser != null ? TicketUser.Id : null,
                DocumentId = document.Id,
                SearchFields = "mailgun" + "," + "O" + "," + newInboundEmailLine.Subject,
                CreateDateUTC = DateTime.UtcNow,
                ReplyToList = replayToList,
            };

            communicationLogRepository.Add(commLog);

            string myDocumentId = document.Id;
            string myDocumentFolder = document.Folder;
            string myDocumentExtension = document.Extension;
            string myCommunicationLogId = commLog.Id;

            commonContext.SaveChanges();

            // Update New Line 
            newInboundEmailLine.CommunicationLogId = commLog.Id;
            //newLine.Body
            inboundEmailLineRepository.Update(newInboundEmailLine);
            inboundEmailLineRepository.SubmitChanges();

            Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
            {
                FileName = myDocumentId,
                FolderName = myDocumentFolder,
                Extension = myDocumentExtension,
                Tenant = Tenant,
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
				queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId",myCommunicationLogId }, { "Tenant", Tenant.ToString() } });
			}

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Inbound Email SendEmailCc Communicationlog", null, ip);
            }
        }

        private void SendEmailTo(string to, int Tenant, byte[] bytearray, bool isInternal)
        {
            string sender = newInboundEmailLine.Sender;
            if (string.Equals(to, sender, StringComparison.CurrentCultureIgnoreCase))
            {
                to = "";
            }

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "html",
                FileSize = bytearray.Length,
                Tenant = Convert.ToInt32(Tenant),
                Id = IdCounter.GetNumber("Document", Tenant),
                HasFile = true,
                Folder = "mailgun",
            };

            documentRepository.Add(document);

            string replayToList = "";
            if (newInboundEmailLine.Direction == "I")
            {
                string replayTo = newInboundEmailLine.Recepient;

                if (!replayTo.Split('@')[0].Contains("+"))
                {
                    replayTo = newInboundEmailLine.Recepient.Split('@')[0] + "+" + GuidId + "@" + newInboundEmailLine.Recepient.Split('@')[1];
                }

                //replayToList = newInboundEmailLine.Recepient;
                if (isInternal)
                {
                    replayToList = replayTo.Split('@')[0] + "-in" + "@" + replayTo.Split('@')[1];
                }

                else
                {
                    replayToList = replayTo.Split('@')[0] + "-ex" + "@" + replayTo.Split('@')[1];
                }
            }

            else
            {
                string replayTo = newInboundEmailLine.Sender;

                if (!replayTo.Split('@')[0].Contains("+"))
                {
                    replayTo = newInboundEmailLine.Sender.Split('@')[0] + "+" + GuidId + "@" + newInboundEmailLine.Sender.Split('@')[1];
                }

                if (isInternal)
                {
                    replayToList = replayTo.Split('@')[0] + "-in" + "@" + replayTo.Split('@')[1];
                }

                else
                {
                    //replayToList = newInboundEmailLine.Sender;
                    replayToList = replayTo.Split('@')[0] + "-ex" + "@" + replayTo.Split('@')[1];
                }
            }

            if (string.IsNullOrEmpty(UserName))
            {
                UserName = "Support";
            }

            string tenantEmail = GetTenantManagementEmail(Tenant);
            string from = "<" + UserName + "> " + tenantEmail;

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = to,
                InOut = "O",
                From = from,
                EntityId = EntityId,
                ObjectTableId = ObjectTableId,
                ChildEntityId = newInboundEmailLine.EntityLineId,
                ChildObjectTableId =ChildObjectTableId, 
                Subject = newInboundEmailLine.Subject,
                Tenant = Tenant,
                CommunicationLogTypeCode = "E",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                CommunicationStatusTypeCode = "W",
               // CreatedByUserId = TicketUser != null ? TicketUser.Id : null,
                DocumentId = document.Id,
                SearchFields = "mailgun" + "," + "O" + "," + newInboundEmailLine.Subject,
                CreateDateUTC = DateTime.UtcNow,
                ReplyToList = replayToList,
            };

            communicationLogRepository.Add(commLog);

            string myDocumentId = document.Id;
            string myDocumentFolder = document.Folder;
            string myDocumentExtension = document.Extension;
            string myCommunicationLogId = commLog.Id;

            commonContext.SaveChanges();

            // Update New Line 
            newInboundEmailLine.CommunicationLogId = commLog.Id;
            //newLine.Body
            inboundEmailLineRepository.Update(newInboundEmailLine);
            inboundEmailLineRepository.SubmitChanges();

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
				queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", myCommunicationLogId }, { "Tenant", Tenant.ToString() } });
			}

            catch (Exception ex)
            {
                string ip = "";

                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    ip = HttpContext.Current.Request.UserHostAddress;
                }

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Inbound Email SendEmailTo Communicationlog", null, ip);
            }
        }

        private bool CheckIsValidEmails(string mailsList)
        {
            bool isOk = true;
            if (string.IsNullOrEmpty(mailsList))
            {
                Regex isValidEmail = new Regex(@"^(([^<>()[\]\\.,;:\s@\""]+"
                                              + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                                              + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                              + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                                              + @"[a-zA-Z]{2,}))$");

                string[] mails = mailsList.Split(';');
                foreach (string email in mails)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        isOk = isValidEmail.IsMatch(email);
                    }
                }
            }

            return isOk;
        }

        public string HtmlEncode(string text)
        {
            char[] chars = HttpUtility.HtmlEncode(text).ToCharArray();
            StringBuilder result = new StringBuilder(text.Length + (int)(text.Length * 0.1));

            foreach (char c in chars)
            {
                int value = Convert.ToInt32(c);
                if (value > 127)
                    result.AppendFormat("&#{0};", value);
                else
                    result.Append(c);
            }

            return result.ToString();
        }

        // External  Body
        private string BuildHTMLBody(List<InboundEmailLine> oldLines, string ticketNumber, bool isUser, bool isOwner)
        {
            int tenant = oldLines.FirstOrDefault().Tenant;

            string myStageName = " ( " + StageName + " )";

            string myResult = @"<br>";

            string upperPart = @"<html>";

            string body = @"<body>";

            myResult += upperPart;
            myResult += @"<meta http-equiv='Content-Type' content='text/html;charset=UTF-8'/>";
            myResult += body;

            myResult += @"<div style='color:#1B90CB;clear:both;text-align:center;text-decoration:underline;font-family:Lucida Sans Unicode;font-size:16px;'>
                            <p>#Please type your reply above this line#</p> 
                        </div>";

            myResult += @"<div style='min-height:70px;background:#008dbc;border:1px solid #DADADA;border-radius:8px;-moz-border-radius:8px;-webkit-border-radius:8px;margin-bottom:10px;'>";

            myResult += @"<p style='width:100%!important;text-align:center;font-family:Lucida Sans Unicode;color:#FFFFFF;font-size:17px;'>"
                     + " Ticket # " + ticketNumber + myStageName + ": " + oldLines[0].Subject + "</p>";
            myResult += @"</div>";

            if (oldLines.Count > 0)
            {
                List<string> ids = (from d in oldLines
                                    group d by d.EntityLineId into gr
                                    select gr.Key).ToList();

                List<InboundLineContactClass> mySourceData = correspondenceRep.GetCorrespondenceContacts(ids);
                ICRMContext context = CRMContext.GetContext(tenant);
                CorrespondencesAttachmentQueryService attachQuery = new CorrespondencesAttachmentQueryService(context);
                List<CorrespondencesAttachmentPM> myCorrespondencesAttachments = attachQuery.GetCorrespondencesAttachmentsListByTenant(tenant);

                if (isOwner)
                {
                    string otherCcEmails = "", otherInternalUsersEmails = "";
                    if (!string.IsNullOrEmpty(newInboundEmailLine.CCs) || !string.IsNullOrEmpty(newInboundEmailLine.InternalUsers) || !string.IsNullOrEmpty(contactEmail))
                    {
                        otherCcEmails = newInboundEmailLine.CCs;
                        List<string> list = new List<string>();

                        if (!string.IsNullOrEmpty(otherCcEmails))
                        {
                            list = otherCcEmails.Split(';').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                            otherCcEmails = string.Join(" , ", list);
                        }

                        otherInternalUsersEmails = newInboundEmailLine.InternalUsers;
                        if (!string.IsNullOrEmpty(otherInternalUsersEmails))
                        {
                            list = otherInternalUsersEmails.Split(';').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                            otherInternalUsersEmails = string.Join(" , ", list);
                        }

                        myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;word-wrap:break-word;'>"
                                 + "This mail was sent to ";

                        if (!string.IsNullOrEmpty(otherCcEmails) || !string.IsNullOrEmpty(contactEmail))
                        {
                            string myEmailList = contactEmail + "," + otherCcEmails;

                            myResult += @"<br/>";
                            myResult += @"<span style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;word-wrap:break-word;'>"
                                     + "To/CC's        : " + myEmailList
                                     + @"</span>";
                        }

                        if (!string.IsNullOrEmpty(otherInternalUsersEmails))
                        {
                            myResult += @"<br/>";
                            myResult += @"<span style='width:100%!important;margin-bottom:10px;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;word-wrap:break-word;'>"
                                     + "Internal users : " + otherInternalUsersEmails
                                     + @"</span>";
                        }

                        myResult += @"</p>";
                    }

                    string myRef = "", replayTo;

                    if (newInboundEmailLine.Direction == "I")
                    {
                        replayTo = newInboundEmailLine.Recepient;
                    }

                    else
                    {
                        replayTo = newInboundEmailLine.Sender;
                    }

                    if (replayTo != null)
                    {
                        myRef = replayTo.Split('@')[0] + "-in@" + replayTo.Split('@')[1];
                    }

                    myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;margin-bottom:10px;'>"
                             + "Your Reply will be sent to all parties invloved"
                             + @"</p>";

                    myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;font-size:13px;word-wrap:break-word;'><a href='mailto:" + myRef + "?subject=" + oldLines[0].Subject + "'>Click Here to send Internal Note to internal users</a></p>";
                    myResult += @"<hr/>";

                    //myResult += @"</div>";

                    foreach (InboundEmailLine item in oldLines.OrderByDescending(d => d.CreateDate))
                    {
                        string headerLine = null;
                        InboundLineContactClass myItemData = mySourceData.Where(d => d.Id == item.EntityLineId).FirstOrDefault();

                        if (myItemData != null)
                        {
                            headerLine = myItemData.Name;
                            if (myItemData.IsInternal)
                            {
                                myResult += @"<div style='width:100%!important;background:#FFFADC;color:black;text-align:left;padding:5px'>";
                            }

                            else
                            {
                                myResult += @"<div style='width:100%!important;background:white;color:black;text-align:left;padding:5px'>";
                            }
                        }

                        DateTime lineDate = item.CreateDate;
                        if (string.IsNullOrEmpty(headerLine))
                        {
                            headerLine = item.Recepient.Split('@')[0].ToString();
                        }

                        headerLine += lineDate.ToString(", MMM dd, H:mm:");
                        myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;font-size:14px;font-weight:bold;'>" + headerLine + "</p>";
                        string htmlbody = HtmlEncode(item.Body);

                        if (htmlbody.Contains("\r"))
                        {
                            htmlbody = htmlbody.Replace("\r", @"<br/>");
                        }

                        if (htmlbody.Contains("\n"))
                        {
                            htmlbody = htmlbody.Replace("\n", @"<br/>");
                        }

                        string textDirection = "ltr";
                        if (myItemData.RightToLeft)
                        {
                            textDirection = "rtl";
                        }

                        myResult += @"<p style='width:100%!important;direction:" + textDirection + ";font-family:Lucida Sans Unicode;font-size:12px'>" + htmlbody + "</p>";

                        List<string> docIds = myCorrespondencesAttachments.Where(a => a.CorrespondenceId == item.EntityLineId && a.Tenant == item.Tenant).Select(d => d.DocumentFilingId).ToList();
                        myResult += @"<nav>";

                        foreach (string id in docIds)
                        {
                            DocumentsFilingPM myDoc = DocumentsFilings.Where(a => a.Id == id).FirstOrDefault();
                            string uri = LogitudeSettings.LogitudeURL + "/WebPages/CorrespondenceDownloadpage.aspx?id=" + myDoc.SecurityId + "~" + item.Tenant;
                            myResult += @"<a href=" + uri + "><span style=';font-family:Lucida Sans Unicode;font-size:12px;'>" + myDoc.FileName + "</span></a> |";
                        }

                        myResult += @"</nav>";

                        myResult += @"<br/>";
                        myResult += @"</div>";
                        myResult += @"<hr/>";
                    }
                }

                else if (isUser)
                {
                    string otherCcEmails = "", otherInternalUsersEmails = "";
                    if (!string.IsNullOrEmpty(newInboundEmailLine.CCs) || !string.IsNullOrEmpty(newInboundEmailLine.InternalUsers) || !string.IsNullOrEmpty(contactEmail))
                    {
                        otherCcEmails = newInboundEmailLine.CCs;
                        List<string> list = new List<string>();

                        if (!string.IsNullOrEmpty(otherCcEmails))
                        {
                            list = otherCcEmails.Split(';').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                            otherCcEmails = string.Join(" , ", list);
                        }

                        otherInternalUsersEmails = newInboundEmailLine.InternalUsers;
                        if (!string.IsNullOrEmpty(otherInternalUsersEmails))
                        {
                            list = otherInternalUsersEmails.Split(';').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                            otherInternalUsersEmails = string.Join(" , ", list);
                        }

                        myResult += @"<p style='width:100%!important;margin-bottom:5px;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;word-wrap:break-word;'>"
                                 + "This mail was sent to ";

                        if (!string.IsNullOrEmpty(otherCcEmails) || !string.IsNullOrEmpty(contactEmail))
                        {
                            string myEmailList = contactEmail + "," + otherCcEmails;

                            myResult += @"<br/>";
                            myResult += @"<span style='width:100%!important;margin-bottom:10px;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;word-wrap:break-word;'>"
                                     + "To/CC's        : " + myEmailList
                                     + @"</span>";
                        }

                        if (!string.IsNullOrEmpty(otherInternalUsersEmails))
                        {
                            myResult += @"<br/>";
                            myResult += @"<span style='width:100%!important;margin-bottom:10px;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;word-wrap:break-word;'>"
                                     + "Internal users : " + otherInternalUsersEmails
                                     + @"</span>";
                        }

                        myResult += @"</p>";
                    }

                    string myRef = "", replayTo;

                    if (newInboundEmailLine.Direction == "I")
                    {
                        replayTo = newInboundEmailLine.Recepient;
                    }

                    else
                    {
                        replayTo = newInboundEmailLine.Sender;
                    }

                    if (replayTo != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(replayTo.Split('@')[0]);
                        sb.Append("-ex@");
                        sb.Append(replayTo.Split('@')[1]);
                        myRef = sb.ToString();

                    }

                    myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;'>"
                             + "Your Reply will be sent as Internal Note only to internal users"
                             + @"</p>";

                    myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;font-size:13px;word-wrap:break-word;'><a href='mailto:" + myRef + "?subject=" + oldLines[0].Subject + "'>Click here To Reply to all parties</a></p>";
                    myResult += @"<hr/>";
                    //myResult += @"</div>";

                    foreach (InboundEmailLine item in oldLines.OrderByDescending(d => d.CreateDate))
                    {
                        string headerLine = null;
                        InboundLineContactClass myItemData = mySourceData.Where(d => d.Id == item.EntityLineId).FirstOrDefault();

                        if (myItemData != null)
                        {
                            headerLine = myItemData.Name;
                            if (myItemData.IsInternal)
                            {
                                myResult += @"<div style='width:100%!important;background:#FFFADC;color:black;text-align:left;padding:5px'>";
                            }

                            else
                            {
                                myResult += @"<div style='width:100%!important;background:white;color:black;text-align:left;padding:5px'>";
                            }
                        }

                        DateTime lineDate = item.CreateDate;
                        if (string.IsNullOrEmpty(headerLine))
                        {
                            headerLine = item.Recepient.Split('@')[0].ToString();
                        }

                        headerLine += lineDate.ToString(", MMM dd, H:mm:");
                        myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;font-size:14px;font-weight:bold;'>" + headerLine + "</p>";
                        string htmlbody = HtmlEncode(item.Body);

                        if (htmlbody.Contains("\r"))
                        {
                            htmlbody = htmlbody.Replace("\r", @"<br/>");
                        }

                        if (htmlbody.Contains("\n"))
                        {
                            htmlbody = htmlbody.Replace("\n", @"<br/>");
                        }

                        string textDirection = "ltr";
                        if (myItemData.RightToLeft)
                        {
                            textDirection = "rtl";
                        }

                        myResult += @"<p style='width:100%!important;direction:" + textDirection + ";font-family:Lucida Sans Unicode;font-size:12px'>" + htmlbody + "</p>";

                        List<string> docIds = myCorrespondencesAttachments.Where(a => a.CorrespondenceId == item.EntityLineId && a.Tenant == item.Tenant).Select(d => d.DocumentFilingId).ToList();
                        myResult += @"<nav>";
                        foreach (string id in docIds)
                        {
                            DocumentsFilingPM myDoc = DocumentsFilings.Where(a => a.Id == id).FirstOrDefault();
                            string uri = LogitudeSettings.LogitudeURL + "/WebPages/CorrespondenceDownloadpage.aspx?id=" + myDoc.SecurityId + "~" + item.Tenant; 
                            myResult += @"<a href=" + uri + "><span style=';font-family:Lucida Sans Unicode;font-size:12px;'>" + myDoc.FileName + "</span></a> |";
                        }

                        myResult += @"</nav>";
                        myResult += @"<br/>";
                        myResult += @"</div>";
                        myResult += @"<hr/>";
                    }
                }

                else
                {
                    string otherEmails = "";
                    if (!string.IsNullOrEmpty(newInboundEmailLine.CCs))
                    {
                        if (!string.IsNullOrEmpty(contactEmail))
                        {
                            otherEmails = newInboundEmailLine.CCs + ";" + contactEmail;
                        }

                        else
                        {
                            otherEmails = newInboundEmailLine.CCs;
                        }

                        List<string> list = new List<string>();

                        if (!string.IsNullOrEmpty(otherEmails))
                        {
                            list = otherEmails.Split(';').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                            otherEmails = string.Join(" , ", list);
                        }

                        myResult += @"<p style='width:100%!important;margin-bottom:10px;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;word-wrap:break-word;'>"
                                 + "This mail was sent to : " + otherEmails
                                 + @"</p>";
                    }

                    myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;'>"
                              + "Your Reply will be sent to all parties invloved"
                             + @"</p>";
                    myResult += @"<hr/>";
                    //myResult += @"</div>";

                    foreach (InboundEmailLine item in oldLines.OrderByDescending(d => d.CreateDate))
                    {
                        string headerLine = null;
                        InboundLineContactClass myItemData = mySourceData.Where(d => d.Id == item.EntityLineId).FirstOrDefault();
                        if (!myItemData.IsInternal)
                        {
                            if (myItemData != null)
                            {
                                headerLine = myItemData.Name;
                                if (myItemData.IsInternal)
                                {
                                    myResult += @"<div style='width:100%!important;background:#FFFADC;color:black;text-align:left;padding:5px'>";
                                }

                                else
                                {
                                    myResult += @"<div style='width:100%!important;background:white;color:black;text-align:left;padding:5px'>";
                                }
                            }

                            DateTime lineDate = item.CreateDate;
                            if (string.IsNullOrEmpty(headerLine))
                            {
                                headerLine = item.Recepient.Split('@')[0].ToString();
                            }

                            headerLine += lineDate.ToString(", MMM dd, H:mm:");
                            myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;font-size:14px;font-weight:bold;'>" + headerLine + "</p>";
                            string htmlbody = HtmlEncode(item.Body);

                            if (htmlbody.Contains("\r"))
                            {
                                htmlbody = htmlbody.Replace("\r", @"<br/>");
                            }

                            if (htmlbody.Contains("\n"))
                            {
                                htmlbody = htmlbody.Replace("\n", @"<br/>");
                            }

                            string textDirection = "ltr";
                            if (myItemData.RightToLeft)
                            {
                                textDirection = "rtl";
                            }

                            myResult += @"<p style='width:100%!important;direction:" + textDirection + ";font-family:Lucida Sans Unicode;font-size:12px'>" + htmlbody + "</p>";

                            List<string> docIds = myCorrespondencesAttachments.Where(a => a.CorrespondenceId == item.EntityLineId && a.Tenant == item.Tenant).Select(d => d.DocumentFilingId).ToList();
                            myResult += @"<nav>";
                            foreach (string id in docIds)
                            {
                                DocumentsFilingPM myDoc = DocumentsFilings.Where(a => a.Id == id).FirstOrDefault();
                                string uri = LogitudeSettings.LogitudeURL + "/WebPages/CorrespondenceDownloadpage.aspx?id=" + myDoc.SecurityId + "~" + item.Tenant;
                                myResult += @"<a href=" + uri + "><span style=';font-family:Lucida Sans Unicode;font-size:12px;'>" + myDoc.FileName + "</span></a> |";
                            }

                            myResult += @"</nav>";
                            myResult += @"<br/>";
                            myResult += @"<hr/>";
                            myResult += @"</div>";
                        }
                    }
                }
            }

            myResult += @"<div style='text-align:center;width:100%!important;height:50px;background:#008dbc;border:1px solid #DADADA;border-radius:8px;-moz-border-radius:8px;-webkit-border-radius:8px;font-family:Lucida Sans Unicode;font-size:17px;'>"
                   + @"<p style='text-align:center;font-family:Lucida Sans Unicode;color:#FFFFFF;font-size:17px;'>This email is service from Unifreight!</p>"
                   + @"</div>";

            string lowerPart =
                @"</body>
                  </html>";

            myResult += lowerPart;

            return myResult;
        }

        // Internal Body
        private string BuildInternalHTMLBody(List<InboundEmailLine> oldLines, string ticketNumber)
        {
            int tenant = oldLines.FirstOrDefault().Tenant;

            string myStageName = " ( " + StageName + " )";

            string myResult = @"<br/>";

            string upperPart = @"<html>";

            string body = @"<body>";

            myResult += upperPart;
            myResult += @"<meta http-equiv='Content-Type' content='text/html;charset=UTF-8'/>";
            myResult += body;
            myResult += @"<div style='color:#1B90CB;clear:both;text-align:center;text-decoration:underline;font-family:Lucida Sans Unicode;font-size:12px;'>
                            <p>#Please type your reply above this line#</p> 
                        </div>";

            myResult += @"<div style='min-height:70px;background:#008dbc;border:1px solid #DADADA;border-radius:8px;-moz-border-radius:8px;-webkit-border-radius:8px;margin-bottom:10px;'>";

            myResult += @"<p style='width:100%!important;text-align:center;font-family:Lucida Sans Unicode;color:#FFFFFF;font-size:17px;'>"
                     + " Ticket # " + ticketNumber + myStageName + ": " + oldLines[0].Subject + "</p>";
            myResult += @"</div>";

            if (oldLines.Count > 0)
            {
                List<string> ids = (from d in oldLines
                                    group d by d.EntityLineId into gr
                                    select gr.Key).ToList();

                List<InboundLineContactClass> mySourceData = correspondenceRep.GetCorrespondenceContacts(ids);
                ICRMContext context = CRMContext.GetContext(tenant);
                CorrespondencesAttachmentQueryService attachQuery = new CorrespondencesAttachmentQueryService(context);
                List<CorrespondencesAttachmentPM> myCorrespondencesAttachments = attachQuery.GetCorrespondencesAttachmentsListByTenant(tenant);

                string otherInternalUsersEmails = "";
                if (!string.IsNullOrEmpty(newInboundEmailLine.InternalUsers))
                {
                    otherInternalUsersEmails = newInboundEmailLine.InternalUsers;
                    List<string> list = otherInternalUsersEmails.Split(';').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    otherInternalUsersEmails = string.Join(" , ", list);

                    myResult += @"<p style='width:100%!important;margin-bottom:10px;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;word-wrap:break-word;'>"
                               + "This mail was sent to Internal users: " + otherInternalUsersEmails
                               + @"</p>";
                }

                string myRef = "", replayTo;

                if (newInboundEmailLine.Direction == "I")
                {
                    replayTo = newInboundEmailLine.Recepient;
                }

                else
                {
                    replayTo = newInboundEmailLine.Sender;
                }

                if (replayTo != null)
                {
                    myRef = replayTo.Split('@')[0] + "-ex" + "@" + replayTo.Split('@')[1];
                }

                myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;color:#282E30;font-size:13px;margin-bottom:10px;'>"
                         + "Your Reply will be sent as Internal Note only to Internal users"
                         + @"</p>";

                myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;font-size:13px;word-wrap:break-word;'><a href='mailto:" + myRef + "?subject=" + oldLines[0].Subject + "'>Click here To Reply to all parties</a></p>";
                myResult += @"<hr/>";
                //myResult += @"</div>";

                foreach (InboundEmailLine item in oldLines.OrderByDescending(d => d.CreateDate))
                {
                    string headerLine = null;
                    InboundLineContactClass myItemData = mySourceData.Where(d => d.Id == item.EntityLineId).FirstOrDefault();

                    if (myItemData != null)
                    {
                        headerLine = myItemData.Name;
                        if (myItemData.IsInternal)
                        {
                            myResult += @"<div style='width:100%!important;background:#FFFADC;color:black;text-align:left;padding:5px'>";
                        }

                        else
                        {
                            myResult += @"<div style='width:100%!important;background:white;color:black;text-align:left;padding:5px'>";
                        }
                    }

                    DateTime lineDate = item.CreateDate;
                    if (string.IsNullOrEmpty(headerLine))
                    {
                        headerLine = item.Recepient.Split('@')[0].ToString();
                    }

                    headerLine += lineDate.ToString(", MMM dd, H:mm:");
                    myResult += @"<p style='width:100%!important;text-align:left;font-family:Lucida Sans Unicode;font-size:14px;font-weight:bold;'>" + headerLine + "</p>";
                    string htmlbody = HtmlEncode(item.Body);

                    if (htmlbody.Contains("\r"))
                    {
                        htmlbody = htmlbody.Replace("\r", @"<br/>");
                    }

                    if (htmlbody.Contains("\n"))
                    {
                        htmlbody = htmlbody.Replace("\n", @"<br/>");
                    }

                    string textDirection = "ltr";
                    if (myItemData.RightToLeft)
                    {
                        textDirection = "rtl";
                    }

                    myResult += @"<p style='width:100%!important;direction:" + textDirection + ";font-family:Lucida Sans Unicode;font-size:12px'>" + htmlbody + "</p>";

                    List<string> docIds = myCorrespondencesAttachments.Where(a => a.CorrespondenceId == item.EntityLineId && a.Tenant == item.Tenant).Select(d => d.DocumentFilingId).ToList();
                    myResult += @"<nav>";
                    foreach (string id in docIds)
                    {
                        DocumentsFilingPM myDoc = DocumentsFilings.Where(a => a.Id == id).FirstOrDefault();
                        string uri = LogitudeSettings.LogitudeURL + "/WebPages/CorrespondenceDownloadpage.aspx?id=" + myDoc.SecurityId + "~" + item.Tenant;
                        myResult += @"<a href=" + uri + "><span style=';font-family:Lucida Sans Unicode;font-size:12px;'>" + myDoc.FileName + "</span></a> |";
                    }

                    myResult += @"</nav>";
                    myResult += @"<br/>";
                    myResult += @"</div>";
                    myResult += @"<hr/>";
                }
            }

            myResult += @"<div style='text-align:center;min-height:50px;background:#008dbc;border:1px solid #DADADA;border-radius:8px;-moz-border-radius:8px;-webkit-border-radius:8px;font-family:Lucida Sans Unicode;font-size:17px;'>"
                   + @"<p style='width:100%!important;text-align:center;font-family:Lucida Sans Unicode;color:#FFFFFF;font-size:17px;'>This email is service  from Unifreight!</p>"
                   + @"</div>";

            string lowerPart =
                @"</body>
                  </html>";

            myResult += lowerPart;

            return myResult;
        }

        public string GetTenantManagementEmail(int tenant)
        {
            string email = "";
            TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
            TenantManagement myTenant = tenantManagementRepository.GetSingleTenantManagement(tenant);
            if (myTenant != null)
            {
                email = myTenant.SupportEmail;
            }

            return email;
        }

        private string GetUserName(string id, int tenant)
        {
            string name = "";
            ContactRepository contactRep = new ContactRepository(tenant);
            Contact myContact = contactRep.GetSingleContact(id, tenant);
            if (myContact != null)
            {
                name = myContact.EnglishName;
            }
            return name;
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "InboundEmail";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            ConnectClient(); // mohammad to try reconnect in case of disconnected client. 23-7-15
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        DbQueueService queueservice;
        private string queueName = "inboundemailqueue";
        public void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService(queueName, 0); 
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "email worker role start", null, null);
            }
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }
    }
}

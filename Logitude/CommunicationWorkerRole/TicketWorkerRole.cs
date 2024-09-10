using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
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
    public class TicketWorkerRole : WorkerEntryPoint
    {
        private string queueName = "ticketqueue";
        private int Tenant;
        private ICRMContext context;

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        this.Tenant = 0;

                        //queueservice = QueueServiceManager.GetQueueService(queueName, 0);
                        queueservice = new DbQueueService();
                        queueservice.InitializeQueue(queueName, 0);
                        var response = queueservice.Receive();
                        LastActivity = DateTime.UtcNow;
                        if (response.MessageId != null)
                        {
                            try
                            {
                                int.TryParse(response.MessageValues["Tenant"].ToString(), out Tenant);
                                string TicketId = response.MessageValues["TicketId"].ToString();

                                this.Send(TicketId);

                                LogDoneItemInMemory();
                            }

                            catch (Exception ex)
                            {
                                if (response.RetryNumber <= 2)
                                {
                                    queueservice.Delay(new TimeSpan(0, 0, 0, 5));
                                }

                                if (response.RetryNumber > 2 && response.RetryNumber <= 4)
                                {
                                    queueservice.Delay(new TimeSpan(0, 0, 0, 10));
                                }
                                if (response.RetryNumber >= 5)
                                {
                                    queueservice.CompleteAsFailed();
                                    ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "WorkerRole", "", null);
                                }
                                //queueservice.Complete();
                                Thread.Sleep(10000);
                            }
                        }

                        //continue
                        Thread.Sleep(10000);
                    }

                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, null, "Ticket worker role Run method", null, null);
                        Thread.Sleep(10000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        TicketEscalationRepository ticketEscalationRep;
        private void Send(string ticketId)
        {
            //this.context = CRMContext.GetContext(Tenant);
            ticketEscalationRep = new TicketEscalationRepository(Tenant);
            List<TicketEscalation> myEscalations = ticketEscalationRep.GetTicketEscalations(ticketId, Tenant).ToList();
            TicketEscalation myCurrentTicketEscalation;

            TicketQueryService ticketQuery = new TicketQueryService(Tenant);
            TicketPM myTicket = ticketQuery.GetSingle(ticketId, true, false);

            DateTime myCurrentDate = TenantServerConfigration.GetCurrentDateTime(Tenant);

            if (myTicket != null && myTicket.FirstResponseTime == null)
            {
                myCurrentTicketEscalation = myEscalations.Where(a => a.IsClose == false && a.EscalationFor == "FR").OrderBy(a => a.DueDate).FirstOrDefault();
                if (myCurrentTicketEscalation != null)
                {
                    TimeSpan myTimeSpan = new TimeSpan();
                    myTimeSpan = myCurrentDate - myCurrentTicketEscalation.DueDate.Value;

                    if (myTimeSpan.TotalMinutes >= 0)
                    {
                        this.SendEmailAlerts(myCurrentTicketEscalation, myTicket);
                        myCurrentTicketEscalation.IsClose = true;
                        myCurrentTicketEscalation.IsSLAViolated = true;
                        myCurrentTicketEscalation.CloseDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                        ticketEscalationRep = new TicketEscalationRepository(Tenant);
                        ticketEscalationRep.Update(myCurrentTicketEscalation);
                        //context.SaveChanges();
                        ticketEscalationRep.SubmitChanges();
                        // Check if there are another escalations for the same ticket !! 

                    }
                    queueservice.Complete();
                    CheckRemainingEscalations(myTicket, myCurrentTicketEscalation.EscalationFor);
                }

                else
                {
                    queueservice.Complete();
                }
            }

            if (myTicket != null && myTicket.FirstResolveDate == null)
            {
                myCurrentTicketEscalation = myEscalations.Where(a => a.IsClose == false && a.EscalationFor == "RW").OrderBy(a => a.DueDate).FirstOrDefault();
                if (myCurrentTicketEscalation != null)
                {
                    TimeSpan myTimeSpan = new TimeSpan();
                    myTimeSpan = myCurrentDate - myCurrentTicketEscalation.DueDate.Value;

                    if (myTimeSpan.TotalMinutes >= 0)
                    {
                        this.SendEmailAlerts(myCurrentTicketEscalation, myTicket);
                        myCurrentTicketEscalation.IsClose = true;
                        myCurrentTicketEscalation.IsSLAViolated = true;
                        myCurrentTicketEscalation.CloseDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                        ticketEscalationRep = new TicketEscalationRepository(Tenant);
                        ticketEscalationRep.Update(myCurrentTicketEscalation);
                        ticketEscalationRep.SubmitChanges();
                        //context.SaveChanges();
                        // Check if there are another escalations for the same ticket !! 

                    }
                    queueservice.Complete();
                    CheckRemainingEscalations(myTicket, myCurrentTicketEscalation.EscalationFor);

                }

                else
                {
                    queueservice.Complete();
                }
            }

            else
            {
                queueservice.Complete();
            }
        }

        private void CheckRemainingEscalations(TicketPM myTicket, string type)
        {
            // IQueueService queueservice = QueueServiceManager.GetQueueService("ticketqueue", Tenant);

            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("ticketqueue", Tenant);

            ticketEscalationRep = new TicketEscalationRepository(myTicket.Tenant);
            List<TicketEscalation> myEscalations = ticketEscalationRep.GetTicketEscalations(myTicket.Id, myTicket.Tenant).ToList();
            TicketEscalation myTicketEscalation = myEscalations.Where(a => a.IsClose == false && a.EscalationFor == type).OrderBy(a => a.DueDate).FirstOrDefault();

            if (myTicketEscalation != null)
            {
                DateTime myDueDate = myTicketEscalation.DueDate.Value;
                //DateTime myCreateDate = myTicketEscalation.CreateDate.Value;
                DateTime myCreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant);

                TimeSpan myTimeSpan = new TimeSpan();
                myTimeSpan = myDueDate - myCreateDate;
                Dictionary<string, string> param = new Dictionary<string, string>() { { "Tenant", Tenant.ToString() }, { "TicketId", myTicket.Id.ToString() } };

                queueservice.Send(param, Tenant, myTimeSpan);
            }
        }

        private void SendEmailAlerts(TicketEscalation myCurrentTicket, TicketPM myTicket)
        {
            if (!string.IsNullOrEmpty(myCurrentTicket.Recepients))
            {
                this.CreateCommunicationLog(myCurrentTicket, myTicket);
                this.AddEscalationEvent(myCurrentTicket, myTicket);
            }
        }

        private void CreateCommunicationLog(TicketEscalation myCurrentTicket, TicketPM myTicket)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(Tenant);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();

            string body = BuildAlertEmailHTML(myCurrentTicket, myTicket);
            byte[] bytearray = enc.GetBytes(body);

            string fromemail = SettingUtil.Emails.FromNoReply;
            string subject = "Ticket Alert";

            if (myCurrentTicket.EscalationFor == "FR")
            {
                subject = "Ticket First Response Escalation Alert";
            }
            else
            {
                subject = "Ticket Resolve Within Escalation Alert";
            }

            ObjectTableQuery query = new ObjectTableQuery(Tenant);
            ObjectTablePM objectTable = query.GetObjectTableByName("Ticket", Tenant);

            string objectTableId = "";
            if (objectTable != null)
            {
                objectTableId = objectTable.Id;
            }

            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "xml",
                FileSize = bytearray.Length,
                Tenant = Convert.ToInt32(Tenant),
                Id = IdCounter.GetNumber("Document", Tenant),
                HasFile = true,
                Folder = "ticketalert",
            };

            documentRepository.Add(document);

            CommunicationLog commLog = new CommunicationLog()
            {
                Id = IdCounter.GetNumber("CommunicationLog", Tenant),
                LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                LastStatusDateUTC = DateTime.UtcNow,
                To = myCurrentTicket.Recepients,
                InOut = "O",
                From = fromemail,
                Subject = subject,
                Tenant = Tenant,
                CommunicationLogTypeCode = "E",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(Tenant),
                CommunicationStatusTypeCode = "W",
                //CreatedByUserId = myTicket.CreatedByContactId,
                DocumentId = document.Id,
                SearchFields = "ticketalert" + "," + subject,
                CreateDateUTC = DateTime.UtcNow,
                EntityId = myTicket.Id,
                //ChildEntityId = newInboundEmailLine.EntityLineId,
                //ChildObjectTableId = ChildObjectTableId,
                ObjectTableId = objectTableId,
            };

            communicationLogRepository.Add(commLog);

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

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Ticket Alert Communicationlog", null, ip);
            }
        }

        private void AddEscalationEvent(TicketEscalation myCurrentTicket, TicketPM myTicket)
        {
            string myEventNote = "";
            if (myCurrentTicket.EscalationFor == "FR")
            {
                myEventNote = "First Response Escalation " + myCurrentTicket.LineNumber;
            }

            else
            {
                myEventNote = "Resolve Within Escalation " + myCurrentTicket.LineNumber;
            }

            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = myTicket.Tenant,
                EventTypeCode = "ESTK",
                //UserId = contact.Id,
                EntityId = myTicket.Id,
                ObjectTableName = "Ticket",
                Notes = myEventNote,
            });
        }

        private string BuildAlertEmailHTML(TicketEscalation myCurrentTicket, TicketPM myTicket)
        {
            StringBuilder HtmlTemplate = new StringBuilder();

            StringBuilder EnvelopeHtmlTemplate = new StringBuilder();

            EnvelopeHtmlTemplate.Append(//font-size:11px;
                "<p style='border-style:solid;border-radius:7px;border-color:#385D8A;background-color:#4F81BD;font-family:Century;text-align:center;color:white;vertical-align: middle;padding:5px'>"
                + "Automatic e<span style='font-family:Arial'>-</span>mail Notification" + "<br />" + "Ticket # " + myTicket.TicketNumber
                + "</p>"
             );

            EnvelopeHtmlTemplate.Append("<div style='text-align:left;font-family:Century;'>");//font-size:11px;
            EnvelopeHtmlTemplate.Append("<p style='text-align:left'>");

            EnvelopeHtmlTemplate.Append("Hello,");
            EnvelopeHtmlTemplate.Append("<br /><br />");

            if (myCurrentTicket.EscalationFor == "FR")
            {
                HtmlTemplate.Append("Please note that Ticket: " + myTicket.TicketNumber + " Due for first Response is " + myCurrentTicket.DueDate);
            }

            else
            {
                HtmlTemplate.Append("Please note that Ticket: " + myTicket.TicketNumber + " Due for Resolve is " + myCurrentTicket.DueDate);
            }

            HtmlTemplate.Append("<br/><br/>");

            HtmlTemplate.Append("Subject: " + myTicket.Subject + "<br/>");

            if (!string.IsNullOrEmpty(myTicket.CompanyName))
                HtmlTemplate.Append("Company : " + myTicket.CompanyName + "<br/>");

            if (!string.IsNullOrEmpty(myTicket.ContactName))
                HtmlTemplate.Append("Contact: " + myTicket.ContactName + "<br/>");

            EnvelopeHtmlTemplate.Append(HtmlTemplate.ToString());

            EnvelopeHtmlTemplate.Append("<br/><br/>");

            EnvelopeHtmlTemplate.Append("</p>");
            EnvelopeHtmlTemplate.Append("</div>");
            EnvelopeHtmlTemplate.Append("<br/>");
            EnvelopeHtmlTemplate.Append("<div style='vertical-align:top;display:table;text-align:center'>");

            EnvelopeHtmlTemplate.Append("Created By <b>Unifreight</b>");

            EnvelopeHtmlTemplate.Append("</div>");

            return EnvelopeHtmlTemplate.ToString();
        }

        public override bool OnStart()
        {

            ConnectClient(); // mohammad to try reconnect in case of disconnected client. 23-7-15
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            //ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "TicketWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        IQueueService queueservice;
        public void ConnectClient()
        {
            try
            {

                //queueservice = QueueServiceManager.GetQueueService(queueName, 0);
                queueservice = new DbQueueService(queueName, 0);
                //queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", log.Id }, { "Tenant", tenant.ToString() } });
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

using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Logitude.BL.Security;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class InboundEmailService
    {
        bool isNewEntity;
        private int tenant;
        public InboundEmail Poco { get; set; }

        public IWebFreightContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        //private InboundEmailPM entityPM;
        private IWebFreightContext objectContext;
        private InboundEmailRepository entityRepository;
        private InboundEmailLineRepository inboundEmailLineRepository;
        private List<InboundEmailLinePM> inboundEmailLineChangeSet;

        public void SetChangeSet(List<InboundEmailLinePM> inboundEmailLineChangeSet)
        {
            this.inboundEmailLineChangeSet = inboundEmailLineChangeSet;
        }

        public InboundEmailService(IWebFreightContext objectContext, int tenant)
        {
            //this.entityPM = entityPM;
            this.tenant =tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new InboundEmailRepository(objectContext);
            this.inboundEmailLineRepository = new InboundEmailLineRepository(objectContext);
        }

        public InboundEmailService(IWebFreightContext objectContext)
        {
            this.inboundEmailLineRepository = new InboundEmailLineRepository(objectContext);
            this.entityRepository = new InboundEmailRepository(objectContext);
        }

        public void Create(InboundEmailPM entityPM)
        {
            this.isNewEntity = true;
            string Id = IdCounter.GetNumber("InboundEmail", tenant).ToString();
            entityPM.Id = Id;
            entityPM.Uniquekey = Id;
            this.Poco = new InboundEmail();

            this.Poco.Id = entityPM.Id;
            this.Poco.Uniquekey = entityPM.Uniquekey;

            foreach (InboundEmailLinePM itemPM in entityPM.InboundEmailLines)
            {
                this.CreateInboundEmailLine(itemPM, entityPM.Id);
            }

            InboundEmailMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);

            entityRepository.SubmitChanges();
        }

        public void Update(InboundEmailPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.Poco = entityRepository.GetSingleInboundEmail(entityPM.Id, tenant);

            if (mapComposition)
            { 
                this.inboundEmailLineChangeSet = entityPM.InboundEmailLines;
            }

            //For Composition 
            this.UpdateInboundEmailLineCollection(entityPM.Id);

            InboundEmailMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        private void UpdateInboundEmailLineCollection(string Id)
        {
            if (inboundEmailLineChangeSet != null)
            {
                foreach (InboundEmailLinePM itemPM in inboundEmailLineChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateInboundEmailLine(itemPM, Id);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateInboundEmailLine(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteInboundEmailLine(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }

        private void CreateInboundEmailLine(InboundEmailLinePM itemPM, string inboundEmailId)
        {
            itemPM.Id = IdCounter.GetNumber("InboundEmailLine", tenant).ToString();
            itemPM.Tenant = tenant;
            itemPM.InboundEmailId = inboundEmailId;

            InboundEmailLine itemPoco = new InboundEmailLine()
            {
                Id = itemPM.Id,
                Tenant = itemPM.Tenant,
                Sender = itemPM.Sender,
                Recepient = itemPM.Recepient,
                Subject = itemPM.Subject,
                Body = itemPM.Body,
                FullBody = itemPM.FullBody,
                HTMLFullBody = itemPM.HTMLFullBody,
                CreateDate = itemPM.CreateDate,
                CCs = itemPM.CCs,
                Bcc = itemPM.Bcc,
                Direction = itemPM.Direction,
                InboundEmailId = itemPM.InboundEmailId,
                EntityLineId = itemPM.EntityLineId,
            };

            InboundEmailMapping.MapInboundEmailLineEntity(itemPM, itemPoco, true);
            inboundEmailLineRepository.Add(itemPoco);
        }

        private void UpdateInboundEmailLine(InboundEmailLinePM itemPM)
        {
            InboundEmailLine itemPoco = inboundEmailLineRepository.GetSingleInboundEmailLine(itemPM.Id, tenant);
            InboundEmailMapping.MapInboundEmailLineEntity(itemPM, itemPoco, false);
            inboundEmailLineRepository.Update(itemPoco);
        }

        private void DeleteInboundEmailLine(InboundEmailLinePM itemPM)
        {
            InboundEmailLine itemPoco = inboundEmailLineRepository.GetSingleInboundEmailLine(itemPM.Id, tenant);
            inboundEmailLineRepository.Remove(itemPoco);
        }

        public void ApplyEmailSending(InboundEmailLinePM entityLinePM, string ticketid, string ticketTableId, string guidId, bool notifyMe, string contactId, string childObjetctTableId)
        {
            this.tenant = entityLinePM.Tenant;

            // Get Ticket Number
            TicketRepository ticketRep = new TicketRepository(tenant);
            Ticket ticket = ticketRep.GetSingle(ticketid, tenant);

            entityLinePM.Id = IdCounter.GetNumber("InboundEmailLine", tenant).ToString();
            entityLinePM.Tenant = tenant;

            InboundEmailLine itemPoco = new InboundEmailLine();
            InboundEmailMapping.MapInboundEmailLineEntity(entityLinePM, itemPoco, true);
            inboundEmailLineRepository.Add(itemPoco);
            inboundEmailLineRepository.SubmitChanges();


            this.SendEmail(itemPoco, ticket.TicketNumber, ticket.ContactId, ticket.OwnerId, contactId, "", ticket.Id, ticketTableId, childObjetctTableId);
        }

        public void SendEmail(InboundEmailLine entity, string ticketNumber, string contactId, string ownerId, string currentUserId, string guidId, string ticketId, string objectTableId, string childObjectTableId)
        {
            // Azure Cash //
            string queueName = "inboundemailqueue";
            int tenant = entity.Tenant;

            try
            {
                DbQueueService queueservice = new DbQueueService(queueName, tenant);//QueueServiceManager.GetQueueService(queueName, tenant);
                Dictionary<string, string> message = new Dictionary<string, string>() 
                    {
                        { "Tenant", tenant.ToString() }, 
                        { "InboundEmailLineId",  entity.Id },
                        { "TicketNumber", ticketNumber },
                        { "ContactId", contactId }, 
                        { "OwnerId", ownerId},
                        { "CurrentLoggedUserId", currentUserId} ,
                        { "GuidId", guidId} ,
                        { "EntityId", ticketId} ,
                        { "ObjectTableId", objectTableId} ,
                        { "ChildObjectTableId", childObjectTableId} ,
                    };

                queueservice.Send(message);
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

                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Inbound Email Service Send Email Method", null, ip);
                throw;
            }
        }

        private string GetLoggedContactData(int tenant)
        {
            string contactId = "";
            ContactRepository myContactRepository = new ContactRepository(tenant);
            Simplog.Data.CommonDataModel.EntityPOCOs.Contact myContact = myContactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), tenant);

            if (myContact != null)
            {
                contactId = myContact.Id;
            }

            return contactId;
        }
    }
}

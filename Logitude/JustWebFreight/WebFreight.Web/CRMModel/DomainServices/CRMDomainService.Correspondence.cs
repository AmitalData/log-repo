using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.BL.EntityUpdateServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Security;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public CorrespondencePM GetSingleCorrespondencePM(string id, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            correspondenceQuery = new CorrespondenceQueryService(crmContext);
            CorrespondencePM Correspondence = correspondenceQuery.GetSingle(id, false, false);
            return Correspondence;
        }

        public CorrespondenceList GetSingleCorrespondenceList(string id, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            CorrespondenceListQueryService listService = new CorrespondenceListQueryService(crmContext);
            return listService.GetSingle(id);
        }

        public void UpdateCorrespondenceList(CorrespondenceList list)
        {

        }

        public List<CorrespondenceList> GetCorrespondenceLists(int tenant)
        {

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            CorrespondenceListQueryService listService = new CorrespondenceListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<CorrespondenceList> GetCorrespondencesFilters(byte[] xmlFilters, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            CorrespondenceListQueryService listService = new CorrespondenceListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetCorrespondenceFiltersCount(byte[] xmlFilters, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            CorrespondenceListQueryService queryService = new CorrespondenceListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

        public void InsertCorrespondence(CorrespondencePM entityPm)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            CorrespondenceUpdateService service = new CorrespondenceUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);

            IWebFreightContext objectContext = WebFreightContext.GetContext(entityPm.Tenant);

            InboundEmailService inboundEmailService = new InboundEmailService(objectContext);

            ObjectTableRepository myRepository = new ObjectTableRepository(entityPm.Tenant);
            ObjectTable myTable = myRepository.GetObjectTableByName("Ticket", entityPm.Tenant, true);
            TicketQueryService myQuery = new TicketQueryService(entityPm.Tenant);
            TicketPM ticket = myQuery.GetSingle(entityPm.EntityId,true,false);

            InboundEmailRepository repositoryInboundEmail = new InboundEmailRepository(entityPm.Tenant);
            InboundEmail myInboundEmail = repositoryInboundEmail.GetInboundEmail(entityPm.EntityId, myTable.Id, entityPm.Tenant); // Get PM from Query 

            ContactRepository contactRep = new ContactRepository(entityPm.Tenant);
            string contactEmail = contactRep.GetEmailContactByIdAndTenant(entityPm.Tenant, entityPm.CreatedByContactId);

            string senderEmail = this.GetSenderEmail(entityPm.Tenant, ticket.GuidId, ticket.SupportMailboxId);
            InboundEmailLinePM line = new InboundEmailLinePM()
            {
                Tenant = entityPm.Tenant,
                Sender = senderEmail,
                Recepient = contactEmail,
                Subject = ticket.Subject,
                Body = entityPm.Description,
                FullBody = entityPm.Description,
                CCs = entityPm.CCs,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPm.Tenant),
                Direction = "O",
                InboundEmailId = myInboundEmail.Id,
                EntityLineId = entityPm.Id,
                Bcc = entityPm.Bcc,
                InternalUsers = entityPm.InternalUsers,
            };

            inboundEmailService.ApplyEmailSending(line, entityPm.EntityId, myTable.Id, ticket.GuidId, entityPm.NotifyMe, entityPm.CreatedByContactId, myInboundEmail.ObjectTableId);
        }

        public string GetSenderEmail(int tenant,string guidId, string supportMailboxId)
        {
            string email = "";
            var mailBox = this.GetDefaultSupportMailBox(supportMailboxId, tenant);
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement myTenant = tenantManagementRepository.GetSingleTenantManagement(tenant);
                if (myTenant != null)
                {
                    string supportDomain = myTenant.SupportDomain;
                    if (!string.IsNullOrEmpty(supportDomain))
                        email = mailBox + "+"+ guidId + "@" + supportDomain;
                }
                scope.Complete();
            }
            return email; 
        }

        private string GetDefaultSupportMailBox(string supportMailboxId, int tenant)
        {
            string mailBox = null;
            SupportMailboxRepository mailboxRepository = new SupportMailboxRepository(tenant);
            SupportMailbox supportMailbox = mailboxRepository.GetSingle(supportMailboxId, tenant);
            mailBox = supportMailbox != null ? supportMailbox.Mailbox : null;
            return mailBox;
        }


        private int CalculateLinesForCurrentTicket(CorrespondencePM entityPm)
        {
            int count = (from a in crmContext.Correspondences
                         where (a.EntityId == entityPm.EntityId && a.ObjectTableId == entityPm.ObjectTableId)
                         select a).ToList().Count();

            return count;
        }

        public void UpdateCorrespondence(CorrespondencePM entityPm)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            CorrespondenceUpdateService service = new CorrespondenceUpdateService(crmContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            service.Update(entityPm, true);
        }

        public List<CorrespondencePM> GetCorrespondencesList(string ticketId, int tenant)
        {
            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }

            CorrespondenceQueryService listService = new CorrespondenceQueryService(crmContext);
            List<CorrespondencePM> myResult = listService.GetAllCorrespondencesByEntityIdAndTenant(ticketId, tenant);

            //#region docs
            //ObjectTabelRepository tableRepository = new ObjectTabelRepository(0);
            //ObjectTable table = tableRepository.GetObjectTableByName("Ticket", 0, false);

            //DocumentsFilingRepository rep = new DocumentsFilingRepository(tenant);
            //DocumentsFilingQuery query = new DocumentsFilingQuery(tenant);

            //List<DocumentsFilingPM> docsIn = query.GetDocumentsFilingPMsByEntityId(tenant, ticketId, table.Id);
            //List<string> correspondencesListIds = myResult.Select(a => a.Id).ToList();

            //CorrespondencesAttachmentRepository attachmentrep = new CorrespondencesAttachmentRepository(tenant);
            //List<CorrespondencesAttachment> attachmentsCorrespondencesList = attachmentrep.GetAllCorrespondencesAttachmentByListOfIds(correspondencesListIds, tenant);

            //foreach (CorrespondencePM item in myResult)
            //{
            //    List<CorrespondencesAttachment> myCorrespondence = attachmentsCorrespondencesList.Where(a => a.CorrespondenceId == item.Id).ToList();

            //    foreach (CorrespondencesAttachment attach in myCorrespondence)
            //    {
            //        DocumentsFilingPM doc = docsIn.Where(a => a.Id == attach.DocumentFilingId).FirstOrDefault();
            //        DocumentDataPM datapm = new DocumentDataPM()
            //        {
            //            Id = doc.Id,
            //            EntityId = doc.EntityId,
            //            DocumentTypeId = doc.DocumentTypeId,
            //            Code = doc.Code,
            //            ReceivedDate = doc.CreateDate,
            //            Tenant = doc.Tenant,
            //            DocumentId = doc.DocumentId,
            //            DocumentTypeName = doc.CustomsDocumentTypeName,
            //            FileName = doc.FileName != null && doc.FileExtension != null ? doc.FileName + "." + doc.FileExtension : null,
            //            SecurityId = doc.SecurityId,
            //            CorrespondenceId = attach.CorrespondenceId,
            //            FileExtension = doc.FileExtension != null ? doc.FileExtension : null,
            //            FileSize = doc.FileSize,
            //            CreateDate = doc.CreateDate,
            //            UpdateDate = doc.UpdateDate,
            //        };

            //        item.CorrespondenceDocumentData.Add(datapm);
            //    }
            //}
            //#endregion 

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Correspondence", tenant, myResult.Cast<object>().ToList());
            return myResult.ToList();
        }

        [Invoke]
        public void UpdateCorrespondenceInvokeOperation(string id,int tenant, bool rightToLeft)
        {
            CorrespondencePM entityPM = this.GetSingleCorrespondencePM(id, tenant);
            entityPM.RightToLeft = rightToLeft;
            this.UpdateCorrespondence(entityPM);
        }
    }
}
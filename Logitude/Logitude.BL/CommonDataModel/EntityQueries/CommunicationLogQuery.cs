using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class CommunicationLogQuery
    {
        CommunicationLogRepository repository;

        public CommunicationLogQuery()
        {
            repository = new CommunicationLogRepository(); 
        }

        public CommunicationLogQuery(int tenant)
        {
            repository = new CommunicationLogRepository(tenant);
        }

        public CommunicationLogQuery(CommunicationLogRepository communicationLogRepository)
        {
            repository = communicationLogRepository;
        }

        public CommunicationLogPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.CommunicationLogs.Include("CommunicationLogType").Include("CommunicationStatusType").Include("CreatedByUser.Contact").Include("ObjectTable").Include("CurrentTenant").Include("ExternalDocument")
                    where a.Id == id
                    select new CommunicationLogPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CC = a.CC,
                        CreateDate = a.CreateDate,
                        DoneDate = a.DoneDate,
                        EntityId = a.EntityId,
                        InOut = a.InOut == "I" ? "In" : "Out",
                        CommunicationLogTypeCode = a.CommunicationLogTypeCode,
                        CommunicationStatusTypeCode = a.CommunicationStatusTypeCode,
                        Subject = a.Subject,
                        To = a.To,
                        From = a.From,
                        CommunicationLogTypeName = a.CommunicationLogType.Name,
                        CommunicationStatusTypeName = a.CommunicationStatusType.Name,
                        ObjectTableId = a.ObjectTableId,
                        DocumentId = a.DocumentId,
                        DocumentInId = a.DocumentsFilingId,
                        DocumentOutId = a.DocumentOutId,
                        CreatedByUserId = a.CreatedByUserId,
                        Retries = a.Retries,
                        BCC = a.BCC,
                        LastStatusDate = a.LastStatusDate,
                        CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                        ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                        EntityReference = a.EntityReference,
                        SearchFields = a.SearchFields,
                        ExceptionMessage = a.ExceptionMessage,
                        CorrelationID = a.CorrelationID,
                        Logs = a.Logs,
                        CreateDateUTC = a.CreateDateUTC,
                        DoneDateUTC = a.DoneDateUTC,
                        LastStatusDateUTC = a.LastStatusDateUTC,
                        NextTryDateTimeUTC = a.NextTryDateTimeUTC,
                        Priority = a.Priority,
                        QueueName = a.QueueName,
                        MessageLockId = a.MessageLockId,
                        TenantName = a.CurrentTenant == null ? null : a.CurrentTenant.Company,
                        AWBNumber = a.AWBNumber,
                        ChildEntityId = a.ChildEntityId,
                        ChildObjectTableId = a.ChildObjectTableId,
                        SecurityId = a.ExternalDocument.SecurityId,
                        
                        IsBodySecured = a.IsSecured,
                        EmailDeliveryError = a.EmailDeliveryError,
                        ResponseDocumentId = a.ResponseDocumentId,
                        UniqueNumber = a.UniqueNumber,
                        WasAnalyzed = a.WasAnalyzed,
                    }).FirstOrDefault();
        }

        public List<CommunicationLogPM> GetCommunicationLogPMsByEntityId(string entityId, int tenant)
        {
            List<CommunicationLogPM> commlogs = (from a in repository.context.CommunicationLogs.Include("CommunicationLogType").Include("CommunicationStatusType").Include("ObjectTable")
                                                       where a.Tenant == tenant && a.EntityId == entityId
                    select new CommunicationLogPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CC = a.CC,
                        CreateDate = a.CreateDate,
                        DoneDate = a.DoneDate,
                        EntityId = a.EntityId,
                        InOut = a.InOut == "I" ? "In" : "Out",
                        CommunicationLogTypeCode = a.CommunicationLogTypeCode,
                        CommunicationStatusTypeCode = a.CommunicationStatusTypeCode,
                        Subject = a.Subject,
                        To = a.To,
                        From = a.From,
                        CommunicationLogTypeName = a.CommunicationLogType.Name,
                        CommunicationStatusTypeName = a.CommunicationStatusType.Name,
                        ObjectTableId = a.ObjectTableId,
                        DocumentId = a.DocumentId,
                        DocumentInId = a.DocumentsFilingId,
                        DocumentOutId = a.DocumentOutId,
                        CreatedByUserId = a.CreatedByUserId,
                        Retries = a.Retries,
                        BCC = a.BCC,
                        LastStatusDate = a.LastStatusDate,
                       // CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                        ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                        EntityReference = a.EntityReference,
                        SearchFields = a.SearchFields,
                        ExceptionMessage = a.ExceptionMessage,
                        CorrelationID = a.CorrelationID,
                        Logs = a.Logs,
                        CreateDateUTC = a.CreateDateUTC,
                        DoneDateUTC = a.DoneDateUTC,
                        LastStatusDateUTC = a.LastStatusDateUTC,
                        NextTryDateTimeUTC = a.NextTryDateTimeUTC,
                        Priority = a.Priority,
                        QueueName = a.QueueName,
                        MessageLockId = a.MessageLockId,
                        //TenantName = a.CurrentTenant == null ? null : a.CurrentTenant.Company,
                        AWBNumber = a.AWBNumber,
                        ReplyToList = a.ReplyToList,
                        ChildEntityId = a.ChildEntityId,
                        ChildObjectTableId = a.ChildObjectTableId,
                      
                        IsBodySecured = a.IsSecured,
                        EmailDeliveryError = a.EmailDeliveryError,
                        ResponseDocumentId = a.ResponseDocumentId,
                        UniqueNumber = a.UniqueNumber,
                        WasAnalyzed = a.WasAnalyzed,
                    }).ToList();

            List<string> contactIds = new List<string>();
            foreach (CommunicationLogPM item in commlogs)
            { 
                if (!string.IsNullOrEmpty(item.CreatedByUserId))
                {
                    var contactId = contactIds.Where(d => d == item.CreatedByUserId).FirstOrDefault();
                    if (string.IsNullOrEmpty(contactId)) contactIds.Add(item.CreatedByUserId);

                }
            }

            List<ContactList> contactLists = null;
            if (contactIds.Count > 0)
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                contactLists = contactQuery.GetContactListsByListIds(contactIds, tenant).ToList();
            }

        
            TenantQuery tenantQuery = new TenantQuery(tenant);
            string company = tenantQuery.GetCompanyNameById(tenant);


            foreach (CommunicationLogPM item in commlogs)
            {
                item.TenantName = company;
                if (!string.IsNullOrEmpty(item.CreatedByUserId) && contactLists != null)
                {
                    ContactList contactList = contactLists.Where(d => d.Id == item.CreatedByUserId).FirstOrDefault();
                    if (contactList != null) item.CreatedByUserName = contactList.EnglishName;

                }
            }


            return commlogs;
        }

        public IQueryable<CommunicationLogPM> GetCommunicationLogPMsByTenant(int tenant)
        {
            return (from a in repository.context.CommunicationLogs.Include("CommunicationLogType").Include("CommunicationStatusType").Include("ObjectTable").Include("CreatedByUser.Contact").Include("CurrentTenant")
                    where a.Tenant == tenant
                    select new CommunicationLogPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CC = a.CC,
                        CreateDate = a.CreateDate,
                        DoneDate = a.DoneDate,
                        EntityId = a.EntityId,
                        InOut = a.InOut,
                        CommunicationLogTypeCode = a.CommunicationLogTypeCode,
                        CommunicationStatusTypeCode = a.CommunicationStatusTypeCode,
                        Subject = a.Subject,
                        To = a.To,
                        From = a.From,
                        CommunicationLogTypeName = a.CommunicationLogType.Name,
                        CommunicationStatusTypeName = a.CommunicationStatusType.Name,
                        ObjectTableId = a.ObjectTableId,
                        DocumentId = a.DocumentId,
                        DocumentInId = a.DocumentsFilingId,
                        DocumentOutId = a.DocumentOutId,
                        CreatedByUserId = a.CreatedByUserId,
                        Retries = a.Retries,
                        BCC = a.BCC,
                        LastStatusDate = a.LastStatusDate,
                        CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                        ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                        EntityReference = a.EntityReference,
                        SearchFields = a.SearchFields,
                        ExceptionMessage = a.ExceptionMessage,
                        CorrelationID = a.CorrelationID,
                        Logs = a.Logs,
                        CreateDateUTC = a.CreateDateUTC,
                        DoneDateUTC = a.DoneDateUTC,
                        LastStatusDateUTC = a.LastStatusDateUTC,
                        NextTryDateTimeUTC = a.NextTryDateTimeUTC,
                        Priority = a.Priority,
                        QueueName = a.QueueName,
                        MessageLockId = a.MessageLockId,
                        TenantName = a.CurrentTenant == null ? null : a.CurrentTenant.Company,
                        AWBNumber = a.AWBNumber,
                        ReplyToList=a.ReplyToList,
                        ChildEntityId = a.ChildEntityId,
                        ChildObjectTableId = a.ChildObjectTableId,
                        
                        IsBodySecured = a.IsSecured,
                        EmailDeliveryError = a.EmailDeliveryError,
                        ResponseDocumentId = a.ResponseDocumentId,
                        UniqueNumber = a.UniqueNumber,
                        WasAnalyzed = a.WasAnalyzed,
                    });
        }

        public IQueryable<CommunicationLogList> GetIQueryableEntityList(IQueryable<CommunicationLog> iQueryable)
        {
            IQueryable<CommunicationLogList> result = from f in iQueryable.Include("CommunicationLogType").Include("CommunicationStatusType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("ObjectTable").Include("CurrentTenant")
                                                      select new CommunicationLogList()
                                                      {

                                                          Id = f.Id,
                                                          Tenant = f.Tenant,
                                                          CC = f.CC,
                                                          CreateDate = f.CreateDate,
                                                          DoneDate = f.DoneDate,
                                                          EntityId = f.EntityId,
                                                          InOut = f.InOut == "I" ? "In" : "Out",
                                                          CommunicationLogTypeCode = f.CommunicationLogTypeCode,
                                                          CommunicationStatusTypeCode = f.CommunicationStatusTypeCode,
                                                          Subject = f.Subject,
                                                          To = f.To,
                                                          From = f.From,
                                                          CommunicationLogTypeName = f.CommunicationLogType.Name,
                                                          CommunicationStatusTypeName = f.CommunicationStatusType.Name,
                                                          ObjectTableId = f.ObjectTableId,
                                                          DocumentId = f.DocumentId,
                                                          DocumentInId = f.DocumentsFilingId,
                                                          DocumentOutId = f.DocumentOutId,
                                                          CreatedByUserId = f.CreatedByUserId,
                                                          Retries = f.Retries,
                                                          BCC = f.BCC,
                                                          LastStatusDate = f.LastStatusDate,
                                                          CreatedByUserName = f.CreatedByUser != null ? (f.CreatedByUser.Contact != null ? f.CreatedByUser.Contact.EnglishName : null) : null,
                                                          ObjectTableName = f.ObjectTable != null ? f.ObjectTable.Name : null,
                                                          SearchFields = f.SearchFields,
                                                          EntityReference = f.EntityReference,
                                                          ExceptionMessage = f.ExceptionMessage,
                                                          CorrelationID = f.CorrelationID,
                                                          Logs = f.Logs,
                                                          CreateDateUTC = f.CreateDateUTC,
                                                          DoneDateUTC = f.DoneDateUTC,
                                                          LastStatusDateUTC = f.LastStatusDateUTC,
                                                          NextTryDateTime = f.NextTryDateTime,
                                                          NextTryDateTimeUTC = f.NextTryDateTimeUTC,
                                                          Priority = f.Priority,
                                                          QueueName = f.QueueName,
                                                          MessageLockId = f.MessageLockId,
                                                          TenantName = f.CurrentTenant == null ? null : f.CurrentTenant.Company,
                                                          AWBNumber = f.AWBNumber,
                                                          ReplyToList= f.ReplyToList,
                                                          ChildEntityId = f.ChildEntityId,
                                                          ChildObjectTableId = f.ChildObjectTableId,
                                                        
                                                          IsBodySecured = f.IsSecured,
                                                          EmailDeliveryError = f.EmailDeliveryError,
                                                          ResponseDocumentId = f.ResponseDocumentId,
                                                          UniqueNumber = f.UniqueNumber,
                                                          WasAnalyzed = f.WasAnalyzed,
                                                      };
            return result;
        }

        private CommunicationLogList GetMyEL(CommunicationLog f)
        {
            var my = new CommunicationLogList()
            {

                Id = f.Id,
                Tenant = f.Tenant,
                CC = f.CC,
                CreateDate = f.CreateDate,
                DoneDate = f.DoneDate,
                EntityId = f.EntityId,
                InOut = f.InOut == "I" ? "In" : "Out",
                CommunicationLogTypeCode = f.CommunicationLogTypeCode,
                CommunicationStatusTypeCode = f.CommunicationStatusTypeCode,
                Subject = f.Subject,
                To = f.To,
                From = f.From,
                CommunicationLogTypeName = f.CommunicationLogType.Name,
                CommunicationStatusTypeName = f.CommunicationStatusType.Name,
                ObjectTableId = f.ObjectTableId,
                DocumentId = f.DocumentId,
                DocumentInId = f.DocumentsFilingId,
                DocumentOutId = f.DocumentOutId,
                CreatedByUserId = f.CreatedByUserId,
                Retries = f.Retries,
                BCC = f.BCC,
                LastStatusDate = f.LastStatusDate,
                CreatedByUserName = f.CreatedByUser != null ? (f.CreatedByUser.Contact != null ? f.CreatedByUser.Contact.EnglishName : null) : null,
                ObjectTableName = f.ObjectTable != null ? f.ObjectTable.Name : null,
                SearchFields = f.SearchFields,
                EntityReference = f.EntityReference,
                ExceptionMessage = f.ExceptionMessage,
                CorrelationID = f.CorrelationID,
                Logs = f.Logs,
                CreateDateUTC = f.CreateDateUTC,
                DoneDateUTC = f.DoneDateUTC,
                LastStatusDateUTC = f.LastStatusDateUTC,
                NextTryDateTime = f.NextTryDateTime,
                NextTryDateTimeUTC = f.NextTryDateTimeUTC,
                Priority = f.Priority,
                QueueName = f.QueueName,
                MessageLockId = f.MessageLockId,
                TenantName = f.CurrentTenant == null ? null : f.CurrentTenant.Company,
                AWBNumber = f.AWBNumber,
                ReplyToList = f.ReplyToList,
                ChildEntityId = f.ChildEntityId,
                ChildObjectTableId = f.ChildObjectTableId,

                IsBodySecured = f.IsSecured,
                EmailDeliveryError = f.EmailDeliveryError,
                ResponseDocumentId = f.ResponseDocumentId,
                UniqueNumber = f.UniqueNumber,
                WasAnalyzed = f.WasAnalyzed,
            };

            if (f.CreatedByUser != null && f.CreatedByUser.Contact != null)
            {
                my.CreatedByUserName = f.CreatedByUser.Contact.EnglishName;
            }
            if (f.ObjectTable != null)
            {
                my.ObjectTableName = f.ObjectTable.Name;
            }
            return my;
        }

        // This method added by maheera. we need to re-implement this method since there are some Includes we don't need (in our wrok -Ticket).  
        public IQueryable<CommunicationLogPM> GetCommunicationLogPMsByEntityIdForTicket(string entityId, int tenant)
        {
            IQueryable<CommunicationLogPM> commlogs = (from a in repository.context.CommunicationLogs.Include("CommunicationLogType").Include("CommunicationStatusType")
                                                       where a.Tenant == tenant && a.EntityId == entityId
                                                       select new CommunicationLogPM()
                                                       {
                                                           Id = a.Id,
                                                           Tenant = a.Tenant,
                                                           CC = a.CC,
                                                           CreateDate = a.CreateDate,
                                                           DoneDate = a.DoneDate,
                                                           EntityId = a.EntityId,
                                                           InOut = a.InOut == "I" ? "In" : "Out",
                                                           CommunicationLogTypeCode = a.CommunicationLogTypeCode,
                                                           CommunicationStatusTypeCode = a.CommunicationStatusTypeCode,
                                                           Subject = a.Subject,
                                                           To = a.To,
                                                           From = a.From,
                                                           CommunicationLogTypeName = a.CommunicationLogType.Name,
                                                           CommunicationStatusTypeName = a.CommunicationStatusType.Name,
                                                           ObjectTableId = a.ObjectTableId,
                                                           DocumentId = a.DocumentId,
                                                           DocumentInId = a.DocumentsFilingId,
                                                           DocumentOutId = a.DocumentOutId,
                                                           CreatedByUserId = a.CreatedByUserId,
                                                           Retries = a.Retries,
                                                           BCC = a.BCC,
                                                           LastStatusDate = a.LastStatusDate,
                                                           EntityReference = a.EntityReference,
                                                           SearchFields = a.SearchFields,
                                                           ExceptionMessage = a.ExceptionMessage,
                                                           CorrelationID = a.CorrelationID,
                                                           Logs = a.Logs,
                                                           CreateDateUTC = a.CreateDateUTC,
                                                           DoneDateUTC = a.DoneDateUTC,
                                                           LastStatusDateUTC = a.LastStatusDateUTC,
                                                           NextTryDateTimeUTC = a.NextTryDateTimeUTC,
                                                           Priority = a.Priority,
                                                           QueueName = a.QueueName,
                                                           MessageLockId = a.MessageLockId,
                                                           AWBNumber = a.AWBNumber,
                                                           ReplyToList = a.ReplyToList,
                                                           ChildEntityId = a.ChildEntityId,
                                                           ChildObjectTableId = a.ChildObjectTableId,
                                                          
                                                           IsBodySecured = a.IsSecured,
                                                           EmailDeliveryError = a.EmailDeliveryError,
                                                           ResponseDocumentId = a.ResponseDocumentId,
                                                           UniqueNumber = a.UniqueNumber,
                                                           WasAnalyzed = a.WasAnalyzed,
                                                       });
            return commlogs;
        }


        public IQueryable<CommunicationLogPM> GetCommunicationLogPMsByEntityIdAndDocumentOutId(string entityId, string documentOutId, int tenant)
        {
            IQueryable<CommunicationLogPM> commlogs = (from a in repository.context.CommunicationLogs.Include("CommunicationLogType").Include("CommunicationStatusType").Include("CreatedByUser.Contact").Include("ObjectTable").Include("CurrentTenant")
                                                       where a.Tenant == tenant && a.EntityId == entityId && a.DocumentOutId == documentOutId
                                                       select new CommunicationLogPM()
                                                       {
                                                           Id = a.Id,
                                                           Tenant = a.Tenant,
                                                           CC = a.CC,
                                                           CreateDate = a.CreateDate,
                                                           DoneDate = a.DoneDate,
                                                           EntityId = a.EntityId,
                                                           InOut = a.InOut == "I" ? "In" : "Out",
                                                           CommunicationLogTypeCode = a.CommunicationLogTypeCode,
                                                           CommunicationStatusTypeCode = a.CommunicationStatusTypeCode,
                                                           Subject = a.Subject,
                                                           To = a.To,
                                                           From = a.From,
                                                           CommunicationLogTypeName = a.CommunicationLogType.Name,
                                                           CommunicationStatusTypeName = a.CommunicationStatusType.Name,
                                                           ObjectTableId = a.ObjectTableId,
                                                           DocumentId = a.DocumentId,
                                                           DocumentInId = a.DocumentsFilingId,
                                                           DocumentOutId = a.DocumentOutId,
                                                           CreatedByUserId = a.CreatedByUserId,
                                                           Retries = a.Retries,
                                                           BCC = a.BCC,
                                                           LastStatusDate = a.LastStatusDate,
                                                           CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                           ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                                           EntityReference = a.EntityReference,
                                                           SearchFields = a.SearchFields,
                                                           ExceptionMessage = a.ExceptionMessage,
                                                           CorrelationID = a.CorrelationID,
                                                           Logs = a.Logs,
                                                           CreateDateUTC = a.CreateDateUTC,
                                                           DoneDateUTC = a.DoneDateUTC,
                                                           LastStatusDateUTC = a.LastStatusDateUTC,
                                                           NextTryDateTimeUTC = a.NextTryDateTimeUTC,
                                                           Priority = a.Priority,
                                                           QueueName = a.QueueName,
                                                           MessageLockId = a.MessageLockId,
                                                           TenantName = a.CurrentTenant == null ? null : a.CurrentTenant.Company,
                                                           AWBNumber = a.AWBNumber,
                                                           ReplyToList = a.ReplyToList,
                                                           ChildEntityId = a.ChildEntityId,
                                                           ChildObjectTableId = a.ChildObjectTableId,
                                                          
                                                           IsBodySecured = a.IsSecured,
                                                           EmailDeliveryError = a.EmailDeliveryError,
                                                           ResponseDocumentId = a.ResponseDocumentId,
                                                           UniqueNumber = a.UniqueNumber,
                                                           WasAnalyzed = a.WasAnalyzed,
                                                       });
            return commlogs;
        }


        public string GetCommunicationLogsDocumentIdByEntityId(string entityId, int tenant)
        {
            return (from a in repository.context.CommunicationLogs
                    where a.EntityId == entityId && a.Subject == "Shared Manifest"
                    select a.DocumentId).FirstOrDefault();
        }


    }
}
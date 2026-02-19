

using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentsExecutionLogQuery
    {
        DocumentsExecutionLogRepository repository;

        public DocumentsExecutionLogQuery()
        {
            repository = new DocumentsExecutionLogRepository();
        }

        public DocumentsExecutionLogQuery(int tenant)
        {
            repository = new DocumentsExecutionLogRepository(tenant);
        }

        public DocumentsExecutionLogQuery(DocumentsExecutionLogRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<DocumentsExecutionLogList> GetIQueryableEntityList(IQueryable<DocumentsExecutionLog> iQueryable)
        {
            IQueryable<DocumentsExecutionLogList> result = from a in iQueryable
                                                         select new DocumentsExecutionLogList()
                                                         {

                                                             Tenant = a.Tenant,
                                                             Id = a.Id,
                                                             CreatedByUserId = a.CreatedByUserId,
                                                             CreateDate = a.CreateDate,
                                                             StartDate = a.StartDate,
                                                             Subject = a.Subject,
                                                             StatusCode = a.StatusCode,
                                                             DocumentTypeTemplateId = a.DocumentTypeTemplateId,
                                                             DocumentTypeId = a.DocumentTypeId,
                                                             DoneDate = a.DoneDate,
                                                             ExceptionMessage = a.ExceptionMessage,
                                                             Logs = a.Logs,
                                                             RequestXML = a.RequestXML,
                                                             RetryNumber = a.RetryNumber,
                                                             ExecutedByServerName = a.ExecutedByServerName,

                                                         };


            return result;
        }


        public DocumentsExecutionLogPM GetSinglePM(string id, int tenant)
        {
            DocumentsExecutionLogPM entity = (from a in repository.context.DocumentsExecutionLogs
                                            where a.Tenant == tenant
                                            && a.Id == id
                                            select new DocumentsExecutionLogPM()
                                            {
                                                Tenant = a.Tenant,
                                                Id = a.Id,
                                                CreatedByUserId = a.CreatedByUserId,
                                                CreateDate = a.CreateDate,
                                                StartDate = a.StartDate,
                                                Subject = a.Subject,
                                                StatusCode = a.StatusCode,
                                                DocumentTypeTemplateId = a.DocumentTypeTemplateId,
                                                DocumentTypeId = a.DocumentTypeId,
                                                DoneDate = a.DoneDate,
                                                ExceptionMessage = a.ExceptionMessage,
                                                Logs = a.Logs,
                                                RequestXML = a.RequestXML,
                                                RetryNumber = a.RetryNumber,
                                                ExecutedByServerName = a.ExecutedByServerName,

                                            }).FirstOrDefault();
            return entity;
        }

        public IQueryable<DocumentsExecutionLogPM> GetDocumentsExecutionLogPMsByTenant(int tenant)
        {
            IQueryable<DocumentsExecutionLogPM> DocumentsExecutionLogPMs = from a in repository.context.DocumentsExecutionLogs
                                                                       where a.Tenant == tenant
                                                                       select new DocumentsExecutionLogPM()
                                                                       {
                                                                           Tenant = a.Tenant,
                                                                           Id = a.Id,
                                                                           CreatedByUserId = a.CreatedByUserId,
                                                                           CreateDate = a.CreateDate,
                                                                           StartDate = a.StartDate,
                                                                           Subject = a.Subject,
                                                                           StatusCode = a.StatusCode,
                                                                           DocumentTypeTemplateId = a.DocumentTypeTemplateId,
                                                                           DocumentTypeId = a.DocumentTypeId,
                                                                           DoneDate = a.DoneDate,
                                                                           ExceptionMessage = a.ExceptionMessage,
                                                                           Logs = a.Logs,
                                                                           RequestXML = a.RequestXML,
                                                                           RetryNumber = a.RetryNumber,
                                                                           ExecutedByServerName = a.ExecutedByServerName,

                                                                       };
            return DocumentsExecutionLogPMs;
        }

        public IQueryable<DocumentsExecutionLogList> GetDocumentsExecutionLogListsByTenant(int tenant)
        {
            IQueryable<DocumentsExecutionLogList> DocumentsExecutionLogLists = from a in repository.context.DocumentsExecutionLogs
                                                                           where a.Tenant == tenant
                                                                           select new DocumentsExecutionLogList()
                                                                           {
                                                                               Tenant = a.Tenant,
                                                                               Id = a.Id,
                                                                               CreatedByUserId = a.CreatedByUserId,
                                                                               CreateDate = a.CreateDate,
                                                                               StartDate = a.StartDate,
                                                                               Subject = a.Subject,
                                                                               StatusCode = a.StatusCode,
                                                                               DocumentTypeTemplateId = a.DocumentTypeTemplateId,
                                                                               DocumentTypeId = a.DocumentTypeId,
                                                                               DoneDate = a.DoneDate,
                                                                               ExceptionMessage = a.ExceptionMessage,
                                                                               Logs = a.Logs,
                                                                               RequestXML = a.RequestXML,
                                                                               RetryNumber = a.RetryNumber,
                                                                               ExecutedByServerName = a.ExecutedByServerName,


                                                                           };
            return DocumentsExecutionLogLists;
        }

        public DocumentsExecutionLogList GetDocumentsExecutionLogList(string id, int tenant)
        {
            DocumentsExecutionLogList documentsExecutionLogList = (from a in repository.context.DocumentsExecutionLogs
                                              where a.Tenant == tenant
                                              && a.Id == id
                                              select new DocumentsExecutionLogList()
                                              {
                                                  Tenant = a.Tenant,
                                                  Id = a.Id,
                                                  Subject = a.Subject,
                                                  StatusCode = a.StatusCode,
                                                  ExceptionMessage = a.ExceptionMessage,
                                                  CreateDate = a.CreateDate,
                                                  ExecutedByServerName = a.ExecutedByServerName,

                                              }).FirstOrDefault();

            if (documentsExecutionLogList != null) documentsExecutionLogList.ExceptionMessage = GetUnderStandableMessageFromMessageException(documentsExecutionLogList.ExceptionMessage);

            return documentsExecutionLogList;
        }


        private string GetUnderStandableMessageFromMessageException(string exceptionMessage)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                string[] lines = exceptionMessage.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                result = lines[0];
            }
            return result;
        }
    }
}
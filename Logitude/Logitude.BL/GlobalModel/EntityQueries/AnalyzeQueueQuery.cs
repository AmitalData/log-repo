using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Security;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityLists;
using System.Transactions;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.GlobalModel
{
    public class AnalyzeQueueQuery
    {
        AnalyzeQueueRepository repository;

        public AnalyzeQueueQuery()
        {
            repository = new AnalyzeQueueRepository();
        }

        public AnalyzeQueueQuery(int tenant)
        {
            repository = new AnalyzeQueueRepository();
        }

        public AnalyzeQueueQuery(AnalyzeQueueRepository analyzeQueueRepository)
        {
            repository = analyzeQueueRepository;
        }

        public AnalyzeQueuePM GetSinglePM(string id, int tenant)
        {
            string entityName = "AnalyzeQueuePM" + id;
            AnalyzeQueuePM securedPm = new AnalyzeQueuePM();

            if (HttpContext.Current != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null)
                {
                    AnalyzeQueuePM analyzeQueue = (from a in repository.context.AnalyzeQueues.Include("AnalyzeQueueStatus").Include("TenantManagement")
                                                   where a.Id == id
                                                   select new AnalyzeQueuePM()
                                                   {
                                                       From = a.From,
                                                       FileSize = a.FileSize,
                                                       CommunicationLogId = a.CommunicationLogId,
                                                       ConnectedToEntity = a.ConnectedToEntity,
                                                       ConnectedToTenant = a.ConnectedToTenant,
                                                       CreateDate = a.CreateDate,
                                                       ErrorMessage = a.ErrorMessage,
                                                       Log=a.Log,
                                                       MessageBody = a.MessageBody,
                                                       Retries = a.Retries,
                                                       Status = a.AnalyzeQueueStatus.Name,
                                                       Subject = a.Subject,
                                                       Tenant = a.Tenant,
                                                       SearchFields = a.SearchFields,
                                                       Id = a.Id,
                                                       StackTrace = a.StackTrace,
                                                       AWBNumber = a.AWBNumber,
                                                       TenantName = a.TenantManagement == null ? null : a.TenantManagement.Name,
                                                       AckReason = a.AckReason,
                                                       DoneDate = a.DoneDate,
                                                       EntityReference = a.EntityReference,
                                                       FileName = a.FileName,
                                                   }).FirstOrDefault();
                    double Byte = 1024;
                    analyzeQueue.FileSize = MethodHelper.Roundd((analyzeQueue.FileSize / Byte), 2);

                    if (analyzeQueue.CommunicationLogId != null)
                    {
                        CommunicationLog commLog;
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            CommunicationLogRepository commLogRep = new CommunicationLogRepository(analyzeQueue.Tenant);
                            commLog = commLogRep.GetSingleCommunicationLog(analyzeQueue.CommunicationLogId, analyzeQueue.Tenant);
                        }
                        analyzeQueue.EntityReference = commLog.EntityReference;
                        analyzeQueue.ObjectTableName = commLog.ObjectTable != null ? commLog.ObjectTable.Name : null;
                    }

                    string str = System.Text.Encoding.UTF8.GetString(analyzeQueue.MessageBody);
                    analyzeQueue.MessageBodyString = str;
                    SecuredMapping.GetMappedPM(analyzeQueue, securedPm, "AnalyzeQueue", analyzeQueue.Tenant);

                    return securedPm;
                }
                else
                {
                    securedPm = (AnalyzeQueuePM)CacheManager.CacheWrapper.Get(entityName);
                }
            }

            else
            {
                AnalyzeQueuePM analyzeQueuepm = (from a in repository.context.AnalyzeQueues.Include("AnalyzeQueueStatus").Include("TenantManagement")
                                                 where a.Id == id
                                                 select new AnalyzeQueuePM()
                                                 {
                                                     Id = a.Id,
                                                     From = a.From,
                                                     FileSize = a.FileSize,
                                                     CommunicationLogId = a.CommunicationLogId,
                                                     ConnectedToEntity = a.ConnectedToEntity,
                                                     ConnectedToTenant = a.ConnectedToTenant,
                                                     CreateDate = a.CreateDate,
                                                     ErrorMessage = a.ErrorMessage,
                                                     Retries = a.Retries,
                                                     Status = a.AnalyzeQueueStatus.Name,
                                                     Subject = a.Subject,
                                                     Tenant = a.Tenant,
                                                     SearchFields = a.SearchFields,
                                                     StackTrace = a.StackTrace,
                                                     AWBNumber = a.AWBNumber,
                                                     TenantName = a.TenantManagement == null ? null : a.TenantManagement.Name,
                                                     AckReason = a.AckReason,
                                                     DoneDate = a.DoneDate,
                                                     EntityReference = a.EntityReference,
                                                     FileName = a.FileName,
                                                     Log = a.Log,
                                                 }).FirstOrDefault();

                double Byte = 1024;
                analyzeQueuepm.FileSize = MethodHelper.Roundd((analyzeQueuepm.FileSize / Byte), 2);
                if (analyzeQueuepm.CommunicationLogId != null)
                {
                    CommunicationLog commLog;
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        CommunicationLogRepository commLogRep = new CommunicationLogRepository(analyzeQueuepm.Tenant);
                        commLog = commLogRep.GetSingleCommunicationLog(analyzeQueuepm.CommunicationLogId, analyzeQueuepm.Tenant);
                    }
                    analyzeQueuepm.EntityReference = commLog.EntityReference;
                    analyzeQueuepm.ObjectTableName = commLog.ObjectTable != null ? commLog.ObjectTable.Name : null;
                }

                SecuredMapping.GetMappedPM(analyzeQueuepm, securedPm, "AnalyzeQueue", analyzeQueuepm.Tenant);
            }

            return securedPm;
        }

        public IQueryable<AnalyzeQueuePM> GetAnalyzeQueuePMsByTenant(int tenant)
        {
            IQueryable<AnalyzeQueuePM> analyzeQueues = from a in repository.context.AnalyzeQueues.Include("AnalyzeQueueStatus").Include("TenantManagement")
                                                       where a.Tenant == tenant
                                                       select new AnalyzeQueuePM()
                                                       {
                                                           Id = a.Id,
                                                           From = a.From,
                                                           FileSize = a.FileSize,
                                                           CommunicationLogId = a.CommunicationLogId,
                                                           ConnectedToEntity = a.ConnectedToEntity,
                                                           ConnectedToTenant = a.ConnectedToTenant,
                                                           CreateDate = a.CreateDate,
                                                           ErrorMessage = a.ErrorMessage,
                                                           Retries = a.Retries,
                                                           Status = a.AnalyzeQueueStatus.Name,
                                                           Subject = a.Subject,
                                                           Tenant = a.Tenant,
                                                           EntityReference = a.EntityReference,
                                                           SearchFields = a.SearchFields,
                                                           StackTrace = a.StackTrace,
                                                           AWBNumber = a.AWBNumber,
                                                           TenantName = a.TenantManagement == null ? null : a.TenantManagement.Name,
                                                           AckReason = a.AckReason,
                                                           DoneDate = a.DoneDate,
                                                           FileName = a.FileName,
                                                           Log = a.Log,
                                                       };
            return analyzeQueues;
        }

        public IQueryable<AnalyzeQueueList> GetIQueryableEntityList(IQueryable<AnalyzeQueue> iQueryable)
        {
            IQueryable<AnalyzeQueueList> result = from analyzeQueue in iQueryable.Include("AnalyzeQueueStatus").Include("TenantManagement")
                                                  select new AnalyzeQueueList()
                                                  {
                                                      Id = analyzeQueue.Id,
                                                      From = analyzeQueue.From,
                                                      FileSize = analyzeQueue.FileSize,
                                                      CommunicationLogId = analyzeQueue.CommunicationLogId,
                                                      ConnectedToEntity = analyzeQueue.ConnectedToEntity,
                                                      ConnectedToTenant = analyzeQueue.ConnectedToTenant,
                                                      CreateDate = analyzeQueue.CreateDate,
                                                      ErrorMessage = analyzeQueue.ErrorMessage,
                                                      Retries = analyzeQueue.Retries,
                                                      Status = analyzeQueue.AnalyzeQueueStatus.Name,
                                                      Subject = analyzeQueue.Subject,
                                                      Tenant = analyzeQueue.Tenant,
                                                      SearchFields = analyzeQueue.SearchFields,
                                                      StackTrace = analyzeQueue.StackTrace,
                                                      AWBNumber = analyzeQueue.AWBNumber,
                                                      TenantName = analyzeQueue.TenantManagement == null ? null : analyzeQueue.TenantManagement.Name,
                                                      AckReason = analyzeQueue.AckReason,
                                                      DoneDate = analyzeQueue.DoneDate,
                                                      EntityReference = analyzeQueue.EntityReference,
                                                      ObjectTableName = analyzeQueue.ObjectTableName
                                                      
                                                  };
            return result;
        }
    }
}

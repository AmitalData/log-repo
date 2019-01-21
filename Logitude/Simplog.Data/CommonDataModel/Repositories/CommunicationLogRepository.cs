using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Data.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CommunicationLogRepository:IRepository<CommunicationLog>
    {
        ICommonDataContext commonDataContext;

        public CommunicationLogRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CommunicationLogRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CommunicationLogRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public CommunicationLog GetSingleCommunicationLog(string id,int tenant)
        {
            var q = (from a in context.CommunicationLogs.Include("CommunicationLogType").Include("CommunicationStatusType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("ObjectTable").Include("InternalDocument").Include("ExternalDocument").Include("Document").Include("CurrentTenant")
                     where a.Id == id
                     select a);
            var log= q.FirstOrDefault();
            return log;

            return (from a in context.CommunicationLogs.Include("CommunicationLogType").Include("CommunicationStatusType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("ObjectTable").Include("InternalDocument").Include("ExternalDocument").Include("Document").Include("CurrentTenant")
                    where a.Id == id
                    select a).FirstOrDefault();

        }

        public int GetCommunicationLogCountForTenantInLasthour(int tenant)
        {

            DateTime datetime = TenantServerConfigration.GetCurrentDateTime(tenant).AddHours(-1);
            return (from a in context.CommunicationLogs
                    where a.Tenant == tenant && a.CreateDate > datetime 
                    select a).Count();
        }



        public CommunicationLog GetSingleCommunicationLog(string id, int tenant, DateTime? createDate)
        {
            return (from a in context.CommunicationLogs.Include("CommunicationLogType").Include("CommunicationStatusType").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("ObjectTable").Include("InternalDocument").Include("ExternalDocument").Include("Document").Include("CurrentTenant")
                    where a.Id == id && a.Tenant == tenant && a.CreateDate == createDate
                    select a).FirstOrDefault();
        }

        public IQueryable<CommunicationLog> GetCommunicationLogs(int tenant)
        {
            if (tenant != 0)
            {
                return (from a in context.CommunicationLogs.Include("Document").Include("CreatedByUser").Include("CommunicationLogType").Include("CommunicationStatusType").Include("CreatedByUser.Contact").Include("ObjectTable").Include("CurrentTenant")
                        where a.Tenant == tenant
                        select a);
            }
            else
            {
                // for now we consider that there is only one database ,in future this code must consider multi database
                return (from a in context.CommunicationLogs.Include("Document").Include("CreatedByUser").Include("CommunicationLogType").Include("CommunicationStatusType").Include("CreatedByUser.Contact").Include("ObjectTable").Include("CurrentTenant")
                        select a);
                // for now it should get all Record // Zaki asked // 14-6-2016
                //where a.CommunicationLogTypeCode != "Q"
            }
        }
        public IQueryable<CommunicationLog> GetCommunicationLogsByTenant(int tenant)
        {
            if (tenant != 0)
            {
                return (from a in context.CommunicationLogs.Include("Document").Include("CreatedByUser").Include("CommunicationLogType").Include("CommunicationStatusType").Include("CreatedByUser.Contact").Include("ObjectTable").Include("CurrentTenant")
                        where a.Tenant == tenant
                        select a);
            }
            else
            {
                // for now we consider that there is only one database ,in future this code must consider multi database
                return (from a in context.CommunicationLogs.Include("Document").Include("CreatedByUser").Include("CommunicationLogType").Include("CommunicationStatusType").Include("CreatedByUser.Contact").Include("ObjectTable").Include("CurrentTenant")
                        
                        select a);
                //where a.CommunicationLogTypeCode != "Q"
            }
        }

        public CommunicationLog GetSpecificCommunicationLogForEntity(string entityId, int tenant, DateTime date)
        {
            CommunicationLog log = (from a in context.CommunicationLogs.Include("Document").Include("CreatedByUser").Include("CommunicationLogType").Include("CommunicationStatusType").Include("CreatedByUser.Contact").Include("ObjectTable").Include("CurrentTenant")
                                    where a.EntityId == entityId && a.Tenant == tenant && a.CreateDate > date&&a.InOut=="I"
                                    select a).FirstOrDefault();

            return log;
        }


        public CommunicationLog GetCommunicationLogByEntityIdAndQueueNameAndSubject(string entityId, string queueName, string subject, int tenant)
        {
            return (from a in context.CommunicationLogs.Include("Document")
                    where a.EntityId == entityId && a.QueueName == queueName && a.Tenant == tenant && a.Subject == subject
                    select a).OrderByDescending(d => d.CreateDate).FirstOrDefault();

        }

        public List<CommunicationLog> GetShareManifestCommunicationLogByEntityIdAndQueueNameAndSubject(string entityId, string queueName, string subject , string subject2)
        {
            return (from a in context.CommunicationLogs.Include("Document")
                    where a.EntityId == entityId && a.QueueName == queueName && (a.Subject == subject  ||  a.Subject == subject2)
                    select a).OrderByDescending(d => d.CreateDate).ToList();

        }

        public void Add(CommunicationLog entity)
        {
            context.CommunicationLogs.Add(entity);
        }

        public void Remove(CommunicationLog entity)
        {
            context.CommunicationLogs.Attach(entity);
            context.CommunicationLogs.Remove(entity);
        }

        public void Update(CommunicationLog entity)
        {
            try
            {
                context.CommunicationLogs.Attach(entity);
               
            }
            catch
            { }

            context.SetAsModified(entity);
        }

        public List<CommunicationLog> All()
        {
            return context.CommunicationLogs.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CommunicationLog> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public CommunicationLog GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}

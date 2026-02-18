using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class LogitudeMessagesTransmissionLogRepository: IRepository<LogitudeMessagesTransmissionLog>
    {
        ICommonDataContext commonDataContext;

        public LogitudeMessagesTransmissionLogRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public LogitudeMessagesTransmissionLogRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public LogitudeMessagesTransmissionLogRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<LogitudeMessagesTransmissionLog> GetLogitudeMessagesTransmissionLogs(int tenant)
        {
            return (from record in context.LogitudeMessagesTransmissionLogs where record.Tenant == tenant select record);
        }

        public IQueryable<LogitudeMessagesTransmissionLog> GetLogitudeMessagesTransmissionLogsByAirlineCode(string airlineCode)
        {
            return (from record in context.LogitudeMessagesTransmissionLogs where record.AirlineCode == airlineCode select record);
        }

        public LogitudeMessagesTransmissionLog GetSingleLogitudeMessagesTransmissionLog(int tenant, string id)
        {
            return (from record in context.LogitudeMessagesTransmissionLogs where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public LogitudeMessagesTransmissionLog GetSingleLogitudeMessagesTransmissionLog(string id, int tenant)
        {
            return (from record in context.LogitudeMessagesTransmissionLogs where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(LogitudeMessagesTransmissionLog entity)
        {
            context.LogitudeMessagesTransmissionLogs.Add(entity);
        }

        public void Remove(LogitudeMessagesTransmissionLog entity)
        {
            try
            {
                context.LogitudeMessagesTransmissionLogs.Attach(entity);
            }
            catch { }
            context.LogitudeMessagesTransmissionLogs.Remove(entity);
        }

        public void Update(LogitudeMessagesTransmissionLog entity)
        {
            try
            {
                context.LogitudeMessagesTransmissionLogs.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<LogitudeMessagesTransmissionLog> All()
        {
            return context.LogitudeMessagesTransmissionLogs.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<LogitudeMessagesTransmissionLog> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public LogitudeMessagesTransmissionLog GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
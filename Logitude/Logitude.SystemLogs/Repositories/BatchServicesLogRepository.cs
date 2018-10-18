using Logitude.SystemLogs.POCOs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.SystemLogs.Repositories
{
    public class BatchServicesLogRepository : IErrorLogRepository<BatchServicesLog>
    {


        ISystemLogContext systemLogContext;
        public BatchServicesLogRepository()
        {
            systemLogContext = SystemLogContext.GetContext();
        }

        public BatchServicesLogRepository(ISystemLogContext context)
        {
            systemLogContext = context;
        }


        public BatchServicesLog GetSingleBatchServicesLog(string id)
        {
            return (from a in context.BatchServicesLogs
                    where a.Id == id 
                    select a).FirstOrDefault();
        }

        public BatchServicesLog GetSingleBatchServicesLogByCode(string batchServiceCode)
        {
            return (from a in context.BatchServicesLogs
                    where a.BatchServiceCode == batchServiceCode
                    select a).FirstOrDefault();
        }

        public List<BatchServicesLog> GetBatchServicesLogsByCode(string batchServiceCode,DateTime? LastActivity)
        {
            if (true)
            {
                
            }
            using (TransactionScope scope = TransactionFactory.GetNewTransactionWithDefaultIsolationLevel())//TransactionFactory.GetNewTransaction())
            {
                return (from a in context.BatchServicesLogs
                        where a.BatchServiceCode == batchServiceCode && (a.LastActivity > LastActivity && a.LastActivity < DateTime.UtcNow)
                        select a).ToList();
            }
        }

        public IQueryable<BatchServicesLog> GetBatchServicesLogsByServiceCode(string batchServiceCode, DateTime? LastActivity)
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransactionWithDefaultIsolationLevel())
            {
                return (from a in context.BatchServicesLogs
                        where a.BatchServiceCode == batchServiceCode && (a.LastActivity > LastActivity && a.LastActivity < DateTime.UtcNow)
                        select a);
            }
        }


        public IQueryable<BatchServicesLog> GetBatchServicesLogs()
        {
            return from a in context.BatchServicesLogs
                  
                   select a;
        }



        public void Add(BatchServicesLog entity)
        {
            context.BatchServicesLogs.Add(entity);
        }

        public void Remove(BatchServicesLog entity)
        {
            context.BatchServicesLogs.Attach(entity);
            context.BatchServicesLogs.Remove(entity);
        }

        public void Update(BatchServicesLog entity)
        {
            context.BatchServicesLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BatchServicesLog> All()
        {
            return context.BatchServicesLogs.ToList();
        }

        public ISystemLogContext context
        {
            get { return systemLogContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.SystemLogs.POCOs;

namespace Logitude.SystemLogs.Repositories
{
    public class FailedTokenLogRepository : IErrorLogRepository<FailedTokenLog>
    {
        ISystemLogContext systemLogContext;
        public FailedTokenLogRepository()
        {
            systemLogContext = SystemLogContext.GetContext();
        }

        public FailedTokenLogRepository(ISystemLogContext context)
        {
            systemLogContext = context;
        }


        public FailedTokenLog GetSingleFailedTokenLog(string id)
        {
            return (from a in context.FailedTokenLogs
                    where a.Id == id
                    select a).FirstOrDefault();
        }



        public IQueryable<FailedTokenLog> GetAllFailedTokenLogs()
        {
            return from a in context.FailedTokenLogs
                   select a;
        }


        public void Add(FailedTokenLog entity)
        {
            context.FailedTokenLogs.Add(entity);
        }

        public void Remove(FailedTokenLog entity)
        {
            context.FailedTokenLogs.Attach(entity);
            context.FailedTokenLogs.Remove(entity);
        }

        public void Update(FailedTokenLog entity)
        {
            context.FailedTokenLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FailedTokenLog> All()
        {
            return context.FailedTokenLogs.ToList();
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

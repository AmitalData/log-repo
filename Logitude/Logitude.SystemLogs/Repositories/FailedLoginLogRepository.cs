using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.SystemLogs.POCOs;

namespace Logitude.SystemLogs.Repositories
{
    public class FailedLoginLogRepository : IErrorLogRepository<FailedLoginLog>
    {
        ISystemLogContext systemLogContext;
        public FailedLoginLogRepository()
        {
            systemLogContext = SystemLogContext.GetContext();
        }

        public FailedLoginLogRepository(ISystemLogContext context)
        {
            systemLogContext = context;
        }


        public FailedLoginLog GetSingleFailedLoginLog(string id)
        {
            return (from a in context.FailedLoginLogs
                    where a.Id == id
                    select a).FirstOrDefault();
        }



        public IQueryable<FailedLoginLog> GetAllFailedLoginLogs()
        {
            return from a in context.FailedLoginLogs
                   select a;
        }


        public void Add(FailedLoginLog entity)
        {
            context.FailedLoginLogs.Add(entity);
        }

        public void Remove(FailedLoginLog entity)
        {
            context.FailedLoginLogs.Attach(entity);
            context.FailedLoginLogs.Remove(entity);
        }

        public void Update(FailedLoginLog entity)
        {
            context.FailedLoginLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FailedLoginLog> All()
        {
            return context.FailedLoginLogs.ToList();
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

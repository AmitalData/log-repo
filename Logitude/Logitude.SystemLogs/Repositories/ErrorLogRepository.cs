using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.SystemLogs.POCOs;

namespace Logitude.SystemLogs.Repositories
{
   public class ErrorLogRepository : IErrorLogRepository<ErrorLog>
    {
       ISystemLogContext systemLogContext;
        public ErrorLogRepository()
        {
            systemLogContext = SystemLogContext.GetContext();
        }

        public ErrorLogRepository(ISystemLogContext context)
        {
            systemLogContext = context;
        }


        public  ErrorLog GetSingleErrorLog(string id)
        {
            return (from a in context.ErrorLogs
                    where a.Id == id
                    select a).FirstOrDefault();
        }



        public IQueryable<ErrorLog> GetAllErrorLogs()
        {
            return from a in context.ErrorLogs
                   select a;
        }


        public void Add(ErrorLog entity)
        {
            context.ErrorLogs.Add(entity);
        }

        public void Remove(ErrorLog entity)
        {
            context.ErrorLogs.Attach(entity);
            context.ErrorLogs.Remove(entity);
        }

        public void Update(ErrorLog entity)
        {
            context.ErrorLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ErrorLog> All()
        {
            return context.ErrorLogs.ToList();
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

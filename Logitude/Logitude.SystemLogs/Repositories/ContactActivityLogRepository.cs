using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.SystemLogs.POCOs;

namespace Logitude.SystemLogs.Repositories
{
    public class ContactActivityLogRepository:IErrorLogRepository<ContactActivityLog>
    {
       ISystemLogContext systemLogContext;
        public ContactActivityLogRepository()
        {
            systemLogContext = SystemLogContext.GetContext();
        }

        public ContactActivityLogRepository(ISystemLogContext context)
        {
            systemLogContext = context;
        }


        public ContactActivityLog GetSingleContactActivityLog(string id,int tenant)
        {
            return (from a in context.ContactActivityLogs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }



        public IQueryable<ContactActivityLog> GetContactActivityLogs(int tenant)
        {
            return from a in context.ContactActivityLogs
                   where a.Tenant == tenant
                   select a;
        }


        public IQueryable<ContactActivityLog> GetContactActivityLogsInLogin(int tenant)
        {
            return from a in context.ContactActivityLogs
                   where a.Tenant == tenant && a.Module == "System Login" && a.IsSharedLogisticsContact == true 
                   select a;
        }



        public IQueryable<ContactActivityLog> GetSharedLogisticsContactLogs(int tenant)
        {
            return from a in context.ContactActivityLogs
                   where a.Tenant == tenant && a.IsSharedLogisticsContact
                   select a;
        }


        public void Add(ContactActivityLog entity)
        {
            context.ContactActivityLogs.Add(entity);
        }

        public void Remove(ContactActivityLog entity)
        {
            context.ContactActivityLogs.Attach(entity);
            context.ContactActivityLogs.Remove(entity);
        }

        public void Update(ContactActivityLog entity)
        {
            context.ContactActivityLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContactActivityLog> All()
        {
            return context.ContactActivityLogs.ToList();
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

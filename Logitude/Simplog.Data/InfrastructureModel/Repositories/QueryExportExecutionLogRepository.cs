using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class QueryExportExecutionLogRepository : IRepository<QueryExportExecutionLog>
    {

        private IWebFreightContext currentContext;
        public QueryExportExecutionLogRepository(int tenant)
        {
            currentContext = WebFreightContext.GetContext(tenant);
        }

        public QueryExportExecutionLogRepository(IWebFreightContext context)
        {
            currentContext = context;
        }



        public QueryExportExecutionLog GetSingle(string id, int tenant)
        {
            return (from a in context.QueryExportExecutionLogs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QueryExportExecutionLog> GetAll(int tenant)
        {
            return from a in context.QueryExportExecutionLogs
                   where a.Tenant == tenant
                   select a;
        }

       
      
        public void Add(QueryExportExecutionLog entity)
        {
            
            context.QueryExportExecutionLogs.Add(entity);
        }

        public void Remove(QueryExportExecutionLog entity)
        {
            context.QueryExportExecutionLogs.Attach(entity);
            context.QueryExportExecutionLogs.Remove(entity);
        }

        
        public void Update(QueryExportExecutionLog entity)
        {
             
            context.QueryExportExecutionLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QueryExportExecutionLog> All()
        {
            return context.QueryExportExecutionLogs.ToList();
        }

        private IWebFreightContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<QueryExportExecutionLog> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QueryExportExecutionLog GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}

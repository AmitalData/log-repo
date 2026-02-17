using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class APILogsDataRepository : IRepository<APILogsData>
    {
        public IWebFreightContext webFreightContext;

        public APILogsDataRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public APILogsDataRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public APILogsDataRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public void Add(APILogsData entity)
        {
            webFreightContext.APILogsData.Add(entity);
        }

        public void Remove(APILogsData entity)
        {
            webFreightContext.APILogsData.Attach(entity);
            webFreightContext.APILogsData.Remove(entity);
        }

        public void Update(APILogsData entity)
        {
            webFreightContext.APILogsData.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<APILogsData> All()
        {
            return webFreightContext.APILogsData.ToList();
        }

        public APILogsData GetSingleAPILogsData(string Id,int Tenant)
        {
            return webFreightContext.APILogsData.Where(a => a.Id == Id && a.Tenant == Tenant).FirstOrDefault();
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<APILogsData> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public APILogsData GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<APILogsData> GetAPILogsData(int tenant)
        {
            return webFreightContext.APILogsData.Where(a => a.Tenant == tenant);
        }
    }
}

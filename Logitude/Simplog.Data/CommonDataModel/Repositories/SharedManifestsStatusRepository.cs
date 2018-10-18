using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class SharedManifestsStatusRepository : IRepository<SharedManifestsStatus>
    {
        ICommonDataContext commonDataContext;

        public SharedManifestsStatusRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public SharedManifestsStatusRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public SharedManifestsStatusRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<SharedManifestsStatus> GetSharedManifestsStatuses()
        {
            return context.SharedManifestsStatuses;
        }

        public SharedManifestsStatus GetSingleSharedManifestsStatus(string code)
        {
            return (from record in context.SharedManifestsStatuses where record.StatusCode == code select record).FirstOrDefault();
        }


        public void Add(SharedManifestsStatus entity)
        {
            context.SharedManifestsStatuses.Add(entity);
        }

        public void Remove(SharedManifestsStatus entity)
        {
            context.SharedManifestsStatuses.Attach(entity);
            context.SharedManifestsStatuses.Remove(entity);
        }

        public void Update(SharedManifestsStatus entity)
        {
            context.SharedManifestsStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SharedManifestsStatus> All()
        {
            return context.SharedManifestsStatuses.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<SharedManifestsStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public SharedManifestsStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}

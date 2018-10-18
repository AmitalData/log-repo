using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class ManifestStatusRepository : IRepository<ManifestStatus>
    {
        private IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public ManifestStatusRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public ManifestStatusRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public ManifestStatus GetSingleManifestStatus(string code)
        {
            return (from a in Context.ManifestStatus where a.Code == code select a).FirstOrDefault();
        }
        public IQueryable<ManifestStatus> GetAll()
        {
            return (from a in Context.ManifestStatus select a);
        }
        public IQueryable<ManifestStatus> GetManifestStatus()
        {
            return (from a in Context.ManifestStatus select a);
        }

        public List<ManifestStatus> All()
        {
            return Context.ManifestStatus.ToList();
        }

        public void Add(ManifestStatus entity)
        {
            Context.ManifestStatus.Add(entity);
        }

        public void Remove(ManifestStatus entity)
        {
            Context.ManifestStatus.Attach(entity);
            Context.ManifestStatus.Remove(entity);
        }

        public void Update(ManifestStatus entity)
        {
            Context.ManifestStatus.Attach(entity);
            Context.SetAsModified(entity);
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

        public List<ManifestStatus> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ManifestStatus GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}

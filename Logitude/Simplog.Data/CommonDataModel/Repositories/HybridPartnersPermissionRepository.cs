using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class HybridPartnersPermissionRepository : IRepository<HybridPartnersPermission>
    {
         ICommonDataContext commonDataContext;

        public HybridPartnersPermissionRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public HybridPartnersPermissionRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public HybridPartnersPermission GetSingleHybridPartnersPermission(string hybridPartnerId, string allowedByHybridPartnerId)
        {
            return (from record in context.HybridPartnersPermissions where record.HybridPartnerId == hybridPartnerId && record.AllowedByHybridPartnerId == allowedByHybridPartnerId  select record).FirstOrDefault();
        }

        public List<HybridPartnersPermission> GetHybridPartnersPermissionByPartnerId(string hybridPartnerId)
        {
            return (from record in context.HybridPartnersPermissions where record.HybridPartnerId == hybridPartnerId select record).ToList();
        }

        public HybridPartnersPermissionRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void Add(HybridPartnersPermission entity)
        {
            context.HybridPartnersPermissions.Add(entity);
        }

        public void Remove(HybridPartnersPermission entity)
        {
            context.HybridPartnersPermissions.Attach(entity);
            context.HybridPartnersPermissions.Remove(entity);
        }

        public void Update(HybridPartnersPermission entity)
        {
            context.HybridPartnersPermissions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<HybridPartnersPermission> All()
        {
            return context.HybridPartnersPermissions.ToList();

        }


        public List<HybridPartnersPermission> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public HybridPartnersPermission GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
    }
}

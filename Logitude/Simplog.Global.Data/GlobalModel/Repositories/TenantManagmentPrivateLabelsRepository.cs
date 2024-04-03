using System.Collections.Generic;
using System.Linq;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Global.Data.GlobalModel.Mapping;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class TenantManagmentPrivateLabelsRepository : IRepository<TenantManagmentPrivateLabels>
    {
        IGlobalContext globalContext;

        public TenantManagmentPrivateLabelsRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public TenantManagmentPrivateLabelsRepository()
        {
            globalContext = GlobalContext.GetContext();
        }


        public IQueryable<TenantManagmentPrivateLabels> GetTenantManagmentPrivateLabels()
        {
            return this.context.TenantManagmentPrivateLabels;
        }


        public IQueryable<TenantManagmentPrivateLabels> GetTenantManagmentPrivateLabels(int tenant)
        {
            return this.context.TenantManagmentPrivateLabels;
        }


        public TenantManagmentPrivateLabels GetSingleTenantManagmentPrivateLabels(string id)
        {
            return (from a in context.TenantManagmentPrivateLabels
                   where a.Id == id
                   select a).FirstOrDefault();
        }

        public TenantManagmentPrivateLabels GetSingleTenantManagmentPrivateLabelsByHybridPartnerId(string hybridPartnerId)
        {
            return (from a in context.TenantManagmentPrivateLabels
                    where a.HybridPartnerId == hybridPartnerId && !a.InActive
                    select a).FirstOrDefault();
        }

        public TenantManagmentPrivateLabels GetSingleTenantManagmentPrivateLabels(string id,int tenant)
        {
            return (from a in context.TenantManagmentPrivateLabels
                    where a.Id == id
            select a).FirstOrDefault();
        }

        public TenantManagmentPrivateLabels GetSingleTenantManagmentPrivateLabelByURL_Cache(string url)
        {
            string entityKeyString = $"GetSingleTenantManagmentPrivateLabelByURL_Cache({url})";
            var res = CacheManager
                .GetOrInsertNewObject<TenantManagmentPrivateLabels>(entityKeyString,
                () => { return this.GetSingleTenantManagmentPrivateLabelByURL(url); });
            return res;

        }

        public TenantManagmentPrivateLabels GetSingleTenantManagmentPrivateLabelByURL(string url)
        {
            return (from a in context.TenantManagmentPrivateLabels
                    where a.PrivateLabelUrl == url && a.InActive == false
                    select a).FirstOrDefault();
        }
      
        public void Add(TenantManagmentPrivateLabels entity)
        {
            context.TenantManagmentPrivateLabels.Add(entity);
        }

        public void Remove(TenantManagmentPrivateLabels entity)
        {
            context.TenantManagmentPrivateLabels.Attach(entity);
            context.TenantManagmentPrivateLabels.Remove(entity);
        }

        public void Update(TenantManagmentPrivateLabels entity)
        {
            context.TenantManagmentPrivateLabels.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TenantManagmentPrivateLabels> All()
        {
            return context.TenantManagmentPrivateLabels.ToList();
        }

        public IGlobalContext context
        {
            get {return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<TenantManagmentPrivateLabels> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TenantManagmentPrivateLabels GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        } 
    }
}
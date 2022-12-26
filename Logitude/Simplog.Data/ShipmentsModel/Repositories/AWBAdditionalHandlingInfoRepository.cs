using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class AWBAdditionalHandlingInfoRepository : IRepository<AWBAdditionalHandlingInfo>
    {
        private IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public AWBAdditionalHandlingInfoRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public AWBAdditionalHandlingInfoRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public AWBAdditionalHandlingInfo GetSingleAWBAdditionalHandlingInfo(string id, int tenant)
        {
            return (from a in Context.AWBAdditionalHandlingInfos where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public AWBAdditionalHandlingInfo GetSingleAWBAdditionalHandlingInfoByCode(string code, int tenant)
        {
            return (from a in Context.AWBAdditionalHandlingInfos where a.Code == code && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<AWBAdditionalHandlingInfo> GetAWBAdditionalHandlingInfos(int tenant)
        {
            return (from a in Context.AWBAdditionalHandlingInfos where a.Tenant == tenant select a);
        }

        public void Add(AWBAdditionalHandlingInfo entity)
        {
            Context.AWBAdditionalHandlingInfos.Add(entity);
        }

        public void Remove(AWBAdditionalHandlingInfo entity)
        {
            Context.AWBAdditionalHandlingInfos.Attach(entity);
            Context.AWBAdditionalHandlingInfos.Remove(entity);
        }

        public void Update(AWBAdditionalHandlingInfo entity)
        {
            Context.AWBAdditionalHandlingInfos.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<AWBAdditionalHandlingInfo> All()
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

        public List<AWBAdditionalHandlingInfo> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AWBAdditionalHandlingInfo GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}

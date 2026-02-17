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
    public class HybridPartnerRepository : IRepository<HybridPartner>
    {
         ICommonDataContext commonDataContext;

        public HybridPartnerRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public HybridPartnerRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public HybridPartnerRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        public HybridPartner GetSingleHybridPartner(string id,int Tenant)
        {
            return (from a in context.HybridPartners where a.Id == id select a).FirstOrDefault();
        }
        public HybridPartner GetSingleHybridPartner(string id)
        {
            return (from a in context.HybridPartners where a.Id == id select a).FirstOrDefault();         
        }
        public IQueryable<HybridPartner> GetHybridPartners(int Tenant)
        {
            return from a in context.HybridPartners      
                   select a;
        }
        public IQueryable<HybridPartner> GetHybridPartnersByTenant()
        {
            return from a in context.HybridPartners      
                   select a;
        }

         public IQueryable<HybridPartner> GetHybridPartnersByPartnerTenant(int partnerTenant)
        {
            return (from record in context.HybridPartners where record.PartnerTenant == partnerTenant select record);
        }
    
        public void Add(HybridPartner entity)
        {
            context.HybridPartners.Add(entity);
        }

        public void Remove(HybridPartner entity)
        {
            context.HybridPartners.Attach(entity);
            context.HybridPartners.Remove(entity);
        }

        public void Update(HybridPartner entity)
        {
            context.HybridPartners.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<HybridPartner> All()
        {
            return context.HybridPartners.ToList();
        }

        public ICommonDataContext context
        {
            get {return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<HybridPartner> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public HybridPartner GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}

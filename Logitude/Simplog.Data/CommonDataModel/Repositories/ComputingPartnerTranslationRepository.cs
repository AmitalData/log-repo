using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ComputingPartnerTranslationRepository : IRepository<ComputingPartnerTranslation>
    {
        ICommonDataContext context;
        public ICommonDataContext Context
        {
            get { return context; }
        }

        public ComputingPartnerTranslationRepository()
        {
            this.context = new CommonDataContext();
        }

        public ComputingPartnerTranslationRepository(int tenant)
        {
            this.context = CommonDataContext.GetContext(tenant);
        }

        public ComputingPartnerTranslationRepository(ICommonDataContext context)
        {
            this.context = context;
        }

        public IQueryable<ComputingPartnerTranslation> GetComputingPartnerTranslations(int tenant)
        {
            return (from d in Context.ComputingPartnerTranslations where d.Tenant == tenant select d);
        }

        public ComputingPartnerTranslation GetSingleComputingPartnerTranslation(string id)
        {
            return (from d in Context.ComputingPartnerTranslations where d.Id == id select d).FirstOrDefault();
        }

        public ComputingPartnerTranslation GetSingleTranslationByOurCode(string partnerId, string tableId, string ourCode, int tenant)
        {
            return (from d in Context.ComputingPartnerTranslations 
                    where d.ComputingPartnerId == partnerId 
                    && d.ObjectTableId == tableId
                    && d.OurCode == ourCode
                    &&d.Tenant == tenant
                    select d).FirstOrDefault();
        }

        public List<ComputingPartnerTranslation> All()
        {
            return Context.ComputingPartnerTranslations.ToList();
        }

        public List<ComputingPartnerTranslation> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ComputingPartnerTranslation GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void Add(ComputingPartnerTranslation entity)
        {
            Context.ComputingPartnerTranslations.Add(entity);
        }

        public void Remove(ComputingPartnerTranslation entity)
        {
            Context.ComputingPartnerTranslations.Attach(entity);
            Context.ComputingPartnerTranslations.Remove(entity);
        }

        public void Update(ComputingPartnerTranslation entity)
        {
            Context.ComputingPartnerTranslations.Attach(entity);
            Context.SetAsModified(entity);
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }
    }
}

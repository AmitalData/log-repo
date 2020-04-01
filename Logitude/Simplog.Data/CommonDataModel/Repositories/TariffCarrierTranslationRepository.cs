using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TariffCarrierTranslationRepository : IRepository<TariffCarrierTranslation>
    {
        ICommonDataContext context;
        public ICommonDataContext Context
        {
            get { return context; }
        }
        
        public TariffCarrierTranslationRepository(int tenant)
        {
            this.context = CommonDataContext.GetContext(tenant);
        }

        public TariffCarrierTranslationRepository(ICommonDataContext context)
        {
            this.context = context;
        }

        public IQueryable<TariffCarrierTranslation> GetTariffCarrierTranslations(int tenant)
        {
            return (from d in Context.TariffCarrierTranslations where d.Tenant == tenant select d);
        }

        public TariffCarrierTranslation GetSingleTariffCarrierTranslation(string id)
        {
            return (from d in Context.TariffCarrierTranslations where d.Id == id select d).FirstOrDefault();
        }

        public List<TariffCarrierTranslation> All()
        {
            return Context.TariffCarrierTranslations.ToList();
        }

        public List<TariffCarrierTranslation> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TariffCarrierTranslation GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void Add(TariffCarrierTranslation entity)
        {
            Context.TariffCarrierTranslations.Add(entity);
        }

        public void Remove(TariffCarrierTranslation entity)
        {
            Context.TariffCarrierTranslations.Attach(entity);
            Context.TariffCarrierTranslations.Remove(entity);
        }

        public void Update(TariffCarrierTranslation entity)
        {
            Context.TariffCarrierTranslations.Attach(entity);
            Context.SetAsModified(entity);
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }
    }
}

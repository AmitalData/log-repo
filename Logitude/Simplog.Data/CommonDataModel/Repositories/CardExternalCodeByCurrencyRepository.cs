using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
  public  class CardExternalCodeByCurrencyRepository: IRepository<CardExternalCodeByCurrency>
    {

         ICommonDataContext commonDataContext;
        public CardExternalCodeByCurrencyRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CardExternalCodeByCurrencyRepository()
        {
            commonDataContext = new CommonDataContext();

        }

        public CardExternalCodeByCurrencyRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }


        public IQueryable<CardExternalCodeByCurrency> GetCardExternalCodeByCurrencies()
        {
            return context.CardExternalCodeByCurrencies;
        }

     

        public IQueryable<CardExternalCodeByCurrency> GetCardExternalCodeByCurrenciesByTenant(int tenant)
        {
            return (from record in context.CardExternalCodeByCurrencies.Include("Currency") where record.Tenant == tenant select record);
        }

        public IQueryable<CardExternalCodeByCurrency> GetCardExternalCodeByCurrencyforCustomer(int tenant, string customerId)
        {
            return (from record in context.CardExternalCodeByCurrencies.Include("Currency") where record.Tenant == tenant && record.CardId == customerId select record);
        }


     

        public CardExternalCodeByCurrency GetSingleCardExternalCodeByCurrency(string id, int tenant)
        {
            return (from record in context.CardExternalCodeByCurrencies.Include("Currency") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();          
        }

   
      
        public void Add(CardExternalCodeByCurrency entity)
        {
            context.CardExternalCodeByCurrencies.Add(entity);
        }

        public void Remove(CardExternalCodeByCurrency entity)
        {
            context.CardExternalCodeByCurrencies.Attach(entity);
            context.CardExternalCodeByCurrencies.Remove(entity);
        }

        public void Update(CardExternalCodeByCurrency entity)
        {
            context.CardExternalCodeByCurrencies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CardExternalCodeByCurrency> All()
        {
            return context.CardExternalCodeByCurrencies.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<CardExternalCodeByCurrency> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CardExternalCodeByCurrency GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }


     
    }
}

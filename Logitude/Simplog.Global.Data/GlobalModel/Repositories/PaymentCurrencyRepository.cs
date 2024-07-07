using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class PaymentCurrencyRepository: IRepository<PaymentCurrency>
    {
        IGlobalContext globalContext;

        public PaymentCurrencyRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public PaymentCurrencyRepository()
        {
            globalContext = new GlobalContext();
        }

        public PaymentCurrencyRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext();
        }

        public PaymentCurrency GetSinglePaymentCurrency(string code)
        {
            PaymentCurrency instance = (from i in context.PaymentCurrencies
                                 where i.Code == code                                 
                                 select i).FirstOrDefault();

            string entityName = "WebhookKey" + code;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && instance != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, instance, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    instance = (PaymentCurrency)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return instance;
        }

        public IQueryable<PaymentCurrency> GetPaymentCurrencies()
        {
            IQueryable<PaymentCurrency> items = context.PaymentCurrencies;
            string entityName = "AllPaymentCurrencies";
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && items != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, items, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    items = (IQueryable<PaymentCurrency>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return items;

        }

        public IQueryable<PaymentCurrency> GetAll()
        {
            return context.PaymentCurrencies;
        }

        public void Add(PaymentCurrency entity)
        {
            context.PaymentCurrencies.Add(entity);
        }

        public void Remove(PaymentCurrency entity)
        {
            context.PaymentCurrencies.Attach(entity);
            context.PaymentCurrencies.Remove(entity);
        }

        public void Update(PaymentCurrency entity)
        {
            context.PaymentCurrencies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentCurrency> All()
        {
            return context.PaymentCurrencies.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
        
        public List<PaymentCurrency> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PaymentCurrency GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
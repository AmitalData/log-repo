using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class PaymentMethodRepository : IRepository<PaymentMethod>
    {
        IGlobalContext globalContext;

        public PaymentMethodRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public PaymentMethodRepository()
        {
            globalContext = new GlobalContext();
        }

        public PaymentMethodRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext();
        }

        public PaymentMethod GetSinglePaymentMethod(string code)
        {
            PaymentMethod instance = (from i in context.PaymentMethods
                                 where i.Code == code                                 
                                 select i).FirstOrDefault();
            string entityName = "PaymentMethod" + code;
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && instance != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, instance, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    instance = (PaymentMethod)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return instance;
        }

        public IQueryable<PaymentMethod> GetPaymentMethods()
        {
            IQueryable<PaymentMethod> items = context.PaymentMethods;
            string entityName = "AllPaymentMethods";
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && items != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, items, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    items = (IQueryable<PaymentMethod>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return items;
        }

        public IQueryable<PaymentMethod> GetAll()
        {
            return context.PaymentMethods;
        }

        public void Add(PaymentMethod entity)
        {
            context.PaymentMethods.Add(entity);
        }

        public void Remove(PaymentMethod entity)
        {
            context.PaymentMethods.Attach(entity);
            context.PaymentMethods.Remove(entity);
        }

        public void Update(PaymentMethod entity)
        {
            context.PaymentMethods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentMethod> All()
        {
            List<PaymentMethod> items = context.PaymentMethods.ToList();
            string entityName = "AllPaymentMethodsList";
            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && items != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, items, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    items = (List<PaymentMethod>)CacheManager.CacheWrapper.Get(entityName);
                }
            }
            return items;
            //return context.PaymentMethods.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PaymentMethod> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PaymentMethod GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}

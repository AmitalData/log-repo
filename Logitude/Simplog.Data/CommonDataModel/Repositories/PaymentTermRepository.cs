using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PaymentTermRepository:IRepository<PaymentTerm>
    {
        ICommonDataContext commonDataContext;

        public PaymentTermRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public PaymentTermRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public PaymentTermRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }


        public IQueryable<PaymentTerm> GetPaymentTerms(int tenant)
        {
            return (from record in context.PaymentTerms where record.Tenant == tenant select record);
        }
        
        public IQueryable<PaymentTerm> GetPaymenTermsByTenant(int tenant)
        {
            return (from record in context.PaymentTerms where record.Tenant == tenant select record);
        }
        
        public PaymentTerm GetSinglePaymentTerm(string id, int tenant)
        {
            return (from record in context.PaymentTerms where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();           
        }

        public void Add(PaymentTerm entity)
        {
            context.PaymentTerms.Add(entity);
        }

        public void Remove(PaymentTerm entity)
        {
            context.PaymentTerms.Remove(entity);
        }

        public void Update(PaymentTerm entity)
        {
            context.PaymentTerms.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentTerm> All()
        {
            return context.PaymentTerms.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PaymentTerm> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PaymentTerm GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
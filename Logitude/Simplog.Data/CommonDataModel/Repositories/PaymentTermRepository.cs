using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

        public PaymentTerm GetSinglePaymentTerm(string id)
        {

            return (from record in context.PaymentTerms where record.Id == id select record).FirstOrDefault();
             
        }



        public IQueryable<PaymentTerm> GetPaymenTermsByTenant(int tenant)
        {
            return (from record in context.PaymentTerms where record.Tenant == tenant select record);
        }
        
        public PaymentTerm GetSinglePaymentTerm(string id, int tenant)
        {
            return (from record in context.PaymentTerms where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();           
        }

        public PaymentTerm GetSinglePaymentTermByCode(string code, int tenant)
        {
            return (from record in context.PaymentTerms where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public PaymentTerm GetSinglePaymentTermByExternalId(string externalId, int tenant)
        {
            return (from record in context.PaymentTerms where record.ExternalId == externalId && record.Tenant == tenant select record).FirstOrDefault();
        }
        public PaymentTerm GetSingleByDaysDifference(int daysDifference, int tenant)
        {
            PaymentTerm query = (from a in context.PaymentTerms
                         where a.Days == daysDifference && a.Tenant == tenant && a.ExternalId != "MS"
                         select a).FirstOrDefault();


            return query;
        }

        public PaymentTerm GetSinglemanuallySetPaymentTerm(int tenant)
        {
            return (from record in context.PaymentTerms where record.IsManuallySet && record.Tenant == tenant select record).FirstOrDefault();
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
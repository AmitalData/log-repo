using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityLists;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class PaymentMethodQuery
    {
        PaymentMethodRepository repository;

        public PaymentMethodQuery()
        {
            repository = new PaymentMethodRepository(); 
        }

        public PaymentMethodQuery(int tenant)
        {
            repository = new PaymentMethodRepository(tenant);
        }

        public PaymentMethodQuery(PaymentMethodRepository paymentMethodRepository)
        {
            repository = paymentMethodRepository;
        }

        public PaymentMethodPM GetSinglePaymentMethodPM(string code)
        {
            return (from a in repository.context.PaymentMethods
                    where a.Code == code
                    select new PaymentMethodPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<PaymentMethodList> GetIQueryableEntityList(IQueryable<PaymentMethod> iQueryable)
        {
            IQueryable<PaymentMethodList> result = from entity in iQueryable
                                                   select new PaymentMethodList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
}
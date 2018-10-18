using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class PaymentCurrencyQuery
    {
        PaymentCurrencyRepository repository;

        public PaymentCurrencyQuery()
        {
            repository = new PaymentCurrencyRepository(); 
        }

        public PaymentCurrencyQuery(int tenant)
        {
            repository = new PaymentCurrencyRepository(tenant);
        }

        public PaymentCurrencyQuery(PaymentCurrencyRepository PaymentCurrencyRepository)
        {
            repository = PaymentCurrencyRepository;
        }

        public PaymentCurrencyPM GetSinglePaymentCurrencyPM(string code)
        {
            return (from a in repository.context.PaymentCurrencies
                    where a.Code == code
                    select new PaymentCurrencyPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<PaymentCurrencyList> GetIQueryableEntityList(IQueryable<PaymentCurrency> iQueryable)
        {
            IQueryable<PaymentCurrencyList> result = from entity in iQueryable
                                                   select new PaymentCurrencyList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
}
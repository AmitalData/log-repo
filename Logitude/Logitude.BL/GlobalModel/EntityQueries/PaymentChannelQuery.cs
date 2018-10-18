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
    public class PaymentChannelQuery
    {
        PaymentChannelRepository repository;

        public PaymentChannelQuery()
        {
            repository = new PaymentChannelRepository(); 
        }

        public PaymentChannelQuery(int tenant)
        {
            repository = new PaymentChannelRepository(tenant);
        }

        public PaymentChannelQuery(PaymentChannelRepository paymentChannelRepository)
        {
            repository = paymentChannelRepository;
        }

        public PaymentChannelPM GetSinglePaymentChannelPM(string code)
        {
            return (from a in repository.context.PaymentChannels
                    where a.Code == code
                    select new PaymentChannelPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<PaymentChannelList> GetIQueryableEntityList(IQueryable<PaymentChannel> iQueryable)
        {
            IQueryable<PaymentChannelList> result = from entity in iQueryable
                                                    select new PaymentChannelList()
                                                     {
                                                         Name = entity.Name,
                                                         Code = entity.Code,
                                                         SearchFields = entity.SearchFields,
                                                     };
            return result;
        }
    }
}
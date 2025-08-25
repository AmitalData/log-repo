using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class PaymentGatewayPartnerQuery
    {

        PaymentGatewayPartnerRepository repository;


        public PaymentGatewayPartnerQuery(int tenant)
        {
            repository = new PaymentGatewayPartnerRepository(tenant);
        }

        public PaymentGatewayPartnerQuery(PaymentGatewayPartnerRepository PaymentGatewayPartnersRepository)
        {
            repository = PaymentGatewayPartnersRepository;
        }


        public PaymentGatewayPartnerPM GetSinglePaymentGatewayPartnersPM(string Code)
        {
            return (from a in repository.context.PaymentGatewayPartners
                    where a.Code == Code
                    select new PaymentGatewayPartnerPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        
                    }).FirstOrDefault();
        }



        public List<PaymentGatewayPartnerPM> GetPaymentGatewayPartnersPMs(string Code)
        {
            return (from a in repository.context.PaymentGatewayPartners
                    where a.Code == Code
                    select new PaymentGatewayPartnerPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                        

                    }).ToList();
        }



        public IQueryable<PaymentGatewayPartnerList> GetIQueryableEntityList(IQueryable<PaymentGatewayPartner> iQueryable)
        {
            IQueryable<PaymentGatewayPartnerList> result = from a in iQueryable
                                                       
                                                        select new PaymentGatewayPartnerList()
                                                        {
                                                            Code = a.Code,
                                                            Name = a.Name,
                                                            SearchFields = a.SearchFields,
                                                           
                                                        };
            return result;
        }



    }
}

using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class TenantAdditionalDataQuery
    {
        private TenantAdditionalDataRepository repository;
        public TenantAdditionalDataQuery()
        {
            repository = new TenantAdditionalDataRepository();
        }
        public TenantAdditionalDataQuery(int tenant)
        {
            repository = new TenantAdditionalDataRepository();
        }
        public TenantAdditionalDataQuery(TenantAdditionalDataRepository repository)
        {
            this.repository = repository;
        }

        public TenantAdditionalDataPM GetSinglePM(int id)
        {
            return (from a in repository.context.TenantAdditionalDatas
                    where a.Id == id
                    select new TenantAdditionalDataPM()
                    {
                        Tenant = a.Tenant,
                        DropBoxAccessToken = a.DropBoxAccessToken,
                        DropBoxState = a.DropBoxState,
                        DropBoxUID = a.DropBoxUID,
                        DropBoxUEmail = a.DropBoxUEmail,
                        PaymentGatewayPartnerCode = a.PaymentGatewayPartnerCode,
                        PaymentGatewayConnectionString = a.PaymentGatewayConnectionString,
                    }).FirstOrDefault();
        }

        public TenantAdditionalDataPM GetSinglePM(int id, int tenant)
        {
            return (from a in repository.context.TenantAdditionalDatas
                    where a.Id == id & a.Tenant == tenant
                    select new TenantAdditionalDataPM()
                    {
                        
                        DropBoxAccessToken = a.DropBoxAccessToken,
                        DropBoxState = a.DropBoxState,
                        DropBoxUID = a.DropBoxUID,
                        DropBoxUEmail = a.DropBoxUEmail,
                        PaymentGatewayPartnerCode = a.PaymentGatewayPartnerCode,
                        PaymentGatewayConnectionString = a.PaymentGatewayConnectionString,
                    }).FirstOrDefault();
        }



        public IQueryable<TenantAdditionalDataPM> GetTenantAdditionalDataPMs()
        {



            return from a in repository.context.TenantAdditionalDatas
                   select new TenantAdditionalDataPM()
                   {
                       Tenant = a.Tenant,
                       DropBoxAccessToken = a.DropBoxAccessToken,
                       DropBoxState = a.DropBoxState,
                       DropBoxUID = a.DropBoxUID,
                       DropBoxUEmail = a.DropBoxUEmail,
                       PaymentGatewayPartnerCode = a.PaymentGatewayPartnerCode,
                       PaymentGatewayConnectionString = a.PaymentGatewayConnectionString,
                   };


        }

        public IQueryable<TenantAdditionalDataList> GetIQueryableEntityList(IQueryable<TenantAdditionalData> iQueryable)
        {

            IQueryable<TenantAdditionalDataList> result = from entity in iQueryable
                                                    select new TenantAdditionalDataList()
                                                    {
                                                        Tenant = entity.Tenant,
                                                        DropBoxAccessToken = entity.DropBoxAccessToken,
                                                        DropBoxState = entity.DropBoxState,
                                                        DropBoxUID = entity.DropBoxUID,
                                                        DropBoxUEmail = entity.DropBoxUEmail,
                                                        PaymentGatewayPartnerCode = entity.PaymentGatewayPartnerCode,
                                                        PaymentGatewayConnectionString = entity.PaymentGatewayConnectionString,
                                                    };
            return result;


        }
    }
}
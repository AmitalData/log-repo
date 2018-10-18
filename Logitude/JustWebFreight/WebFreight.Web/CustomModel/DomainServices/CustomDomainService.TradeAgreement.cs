using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public TradeAgreementPM GetSingleTradeAgreementPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            tradeAgreementQuery = new TradeAgreementQueryService(customContext);
            TradeAgreementPM TradeAgreement = tradeAgreementQuery.GetSingle(code, false, false);
            return TradeAgreement;
        }

        public TradeAgreementList GetSingleTradeAgreementList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);


            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            TradeAgreementListQueryService listService = new TradeAgreementListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<TradeAgreementList> GetTradeAgreementLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.TradeAgreement", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            TradeAgreementListQueryService listService = new TradeAgreementListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<TradeAgreementList> GetTradeAgreementFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.TradeAgreement", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            TradeAgreementListQueryService listService = new TradeAgreementListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetTradeAgreementFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.TradeAgreement", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            TradeAgreementListQueryService queryService = new TradeAgreementListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}
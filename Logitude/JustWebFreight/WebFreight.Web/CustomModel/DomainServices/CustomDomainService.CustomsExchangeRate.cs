using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomsExchangeRatePM GetSingleCustomsExchangeRatePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsExchangeRateQuery = new CustomsExchangeRateQueryService(customContext);
            CustomsExchangeRatePM CustomsExchangeRate = customsExchangeRateQuery.GetSingle(id, true, false);
            return CustomsExchangeRate;
        }

        public CustomsExchangeRateList GetSingleCustomsExchangeRateList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.CustomsExchangeRate", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsExchangeRateListQueryService listService = new CustomsExchangeRateListQueryService(customContext);
            return listService.GetSingle(id);
        }



        public List<CustomsExchangeRateList> GetCustomsExchangeRateLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.CustomsExchangeRate", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsExchangeRateListQueryService listService = new CustomsExchangeRateListQueryService(customContext);
            return listService.GetList(tenant);
         //   return new List<CustomsExchangeRateList>();
        }

        public List<CustomsExchangeRateList> GetCustomsExchangeRateByDateLists(DateTime rateDate, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.CustomsExchangeRate", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            customsExchangeRateQuery  = new CustomsExchangeRateQueryService(customContext);
            return customsExchangeRateQuery.GetRatesByDate(rateDate,tenant);
           
        }


        public List<CustomsExchangeRateList> GetCustomsExchangeRateFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.CustomsExchangeRate", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsExchangeRateListQueryService listService = new CustomsExchangeRateListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }



        public int GetCustomsExchangeRateFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.CustomsExchangeRate", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsExchangeRateListQueryService queryService = new CustomsExchangeRateListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertCustomsExchangeRate(CustomsExchangeRatePM entityPm)
        {
           //  SecurityUtility.CheckContactFeature("Customs.CustomsExchangeRate", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            CustomsExchangeRateUpdateService service = new CustomsExchangeRateUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);


            service.Update(entityPm, true);

        }

        public void UpdateCustomsExchangeRate(CustomsExchangeRatePM currententityPm)
        {
            // SecurityUtility.CheckContactFeature("Customs.CustomsExchangeRate", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            CustomsExchangeRateUpdateService service = new CustomsExchangeRateUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(currententityPm, true);

        }


        public void UpdateCustomsExchangeRateList(CustomsExchangeRateList list)
        {

        }

        public List<CustomsExchangeRatePM> GetCustomsExchangeRateForCurrencyAndDate(string currencyTypeCodes, DateTime? date, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            customsExchangeRateQuery = new CustomsExchangeRateQueryService(customContext);
            List<CustomsExchangeRatePM> CustomsExchangeRates = customsExchangeRateQuery.GetExchangeRateByCurrencyAndDate(currencyTypeCodes, date, tenant);
            return CustomsExchangeRates;
        }

        //<--- Yuval Chalup 14.12.2014 TASK-4238
        public List<CustomsExchangeRatePM> GetCustomsExchangeRateForDate(DateTime? rateDate, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            customsExchangeRateQuery = new CustomsExchangeRateQueryService(customContext);
            List<CustomsExchangeRatePM> CustomsExchangeRates = customsExchangeRateQuery.GetCustomsExchangeRateForDate(rateDate, tenant);
            return CustomsExchangeRates;
        }
        //Yuval Chalup 14.12.2014 TASK-4238 --->

    }
}
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

        public CustomsPaymentTermPM GetSingleCustomsPaymentTermPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsPaymentTermQuery = new CustomsPaymentTermQueryService(customContext);
            CustomsPaymentTermPM CustomsPaymentTerm = customsPaymentTermQuery.GetSingle(code, false, false);
            return CustomsPaymentTerm;
        }

        public CustomsPaymentTermList GetSingleCustomsPaymentTermList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("Customs.CustomsPaymentTerm", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsPaymentTermListQueryService listService = new CustomsPaymentTermListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<CustomsPaymentTermList> GetCustomsPaymentTermLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsPaymentTerm", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsPaymentTermListQueryService listService = new CustomsPaymentTermListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsPaymentTermList> GetCustomsPaymentTermFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsPaymentTerm", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsPaymentTermListQueryService listService = new CustomsPaymentTermListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsPaymentTermFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.CustomsPaymentTerm", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsPaymentTermListQueryService queryService = new CustomsPaymentTermListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}
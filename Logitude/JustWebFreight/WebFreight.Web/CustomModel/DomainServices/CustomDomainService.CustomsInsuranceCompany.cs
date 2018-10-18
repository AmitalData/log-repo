using System.Collections.Generic;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomsInsuranceCompanyPM GetSingleCustomsInsuranceCompanyPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsInsuranceCompanyQuery = new CustomsInsuranceCompanyQueryService(customContext);
            CustomsInsuranceCompanyPM CustomsInsuranceCompany = customsInsuranceCompanyQuery.GetSingle(id, false, false);
            return CustomsInsuranceCompany;
        }

        public CustomsInsuranceCompanyList GetSingleCustomsInsuranceCompanyList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsInsuranceCompanyListQueryService listService = new CustomsInsuranceCompanyListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsInsuranceCompanyList> GetCustomsInsuranceCompanyLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsInsuranceCompanyListQueryService listService = new CustomsInsuranceCompanyListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsInsuranceCompanyList> GetCustomsInsuranceCompanyFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsInsuranceCompanyListQueryService listService = new CustomsInsuranceCompanyListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsInsuranceCompanyFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsInsuranceCompanyListQueryService queryService = new CustomsInsuranceCompanyListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}
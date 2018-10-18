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
        public BankPM GetSingleBankPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            bankQuery = new BankQueryService(customContext);
            BankPM Bank = bankQuery.GetSingle(id, false, false);
            return Bank;
        }

        public BankList GetSingleBankList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           
            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            BankListQueryService listService = new BankListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<BankList> GetBankLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            BankListQueryService listService = new BankListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<BankList> GetBankFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            BankListQueryService listService = new BankListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetBankFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            BankListQueryService queryService = new BankListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}
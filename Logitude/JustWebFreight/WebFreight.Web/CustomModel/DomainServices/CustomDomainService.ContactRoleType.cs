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

        public ContactRoleTypePM GetSingleContactRoleTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            contactRoleTypeQuery = new ContactRoleTypeQueryService(customContext);
            ContactRoleTypePM ContactRoleType = contactRoleTypeQuery.GetSingle(code, false, false);
            return ContactRoleType;
        }

        public ContactRoleTypeList GetSingleContactRoleTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ContactRoleType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ContactRoleTypeListQueryService listService = new ContactRoleTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<ContactRoleTypeList> GetContactRoleTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ContactRoleType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ContactRoleTypeListQueryService listService = new ContactRoleTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ContactRoleTypeList> GetContactRoleTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ContactRoleType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ContactRoleTypeListQueryService listService = new ContactRoleTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetContactRoleTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.ContactRoleType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ContactRoleTypeListQueryService queryService = new ContactRoleTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}
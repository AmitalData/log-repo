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


        public MorningMessageTypePM GetSingleMorningMessageTypePM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            morningMessageTypeQuery = new MorningMessageTypeQueryService(customContext);
            MorningMessageTypePM MorningMessageType = morningMessageTypeQuery.GetSingle(code, false, false);
            return MorningMessageType;
        }

        public MorningMessageTypeList GetSingleMorningMessageTypeList(string code, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.MorningMessageType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            MorningMessageTypeListQueryService listService = new MorningMessageTypeListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<MorningMessageTypeList> GetMorningMessageTypeLists(int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.MorningMessageType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            MorningMessageTypeListQueryService listService = new MorningMessageTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<MorningMessageTypeList> GetMorningMessageTypeFilters(byte[] xmlFilters, int tenant)
        {
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.MorningMessageType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            MorningMessageTypeListQueryService listService = new MorningMessageTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetMorningMessageTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
           // SecurityUtility.AuthenticationOnTenant(tenant);
          //  SecurityUtility.CheckContactFeature("Customs.MorningMessageType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            MorningMessageTypeListQueryService queryService = new MorningMessageTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public CustomBanksCardPM GetSingleCustomBanksCardPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customBanksCardQuery = new CustomBanksCardQueryService(customContext);
            CustomBanksCardPM CustomBanksCard = customBanksCardQuery.GetSingle(id, true, false);
            return CustomBanksCard;
        }
                           
        public CustomBanksCardList GetSingleCustomBanksCardList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomBanksCardListQueryService listService = new CustomBanksCardListQueryService(customContext);
            return listService.GetSingle(id);
        }
                   

        public List<CustomBanksCardList> GetCustomBanksCardLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomBanksCardListQueryService listService = new CustomBanksCardListQueryService(customContext);
            return listService.GetList(tenant);
            return new List<CustomBanksCardList>();
        }


        public List<CustomBanksCardList> GetCustomBanksCardFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomBanksCardListQueryService listService = new CustomBanksCardListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }


        public int GetCustomBanksCardFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomBanksCardListQueryService queryService = new CustomBanksCardListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }



        public void UpdateCustomBanksCardList(CustomBanksCardList list)
        {

        }

    }
}
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
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public DecCargoSplitConsPackDetPM GetSingleDecCargoSplitConsPackDetPM(string DeclarationCargoSplitId, int deccargosplitconslineno, int deccargosplitconsitemline, int packageline, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            decCargoSplitConsPackDetQueryService = new DecCargoSplitConsPackDetQueryService(customContext);
            DecCargoSplitConsPackDetPM DecCargoSplitConsPackDet = decCargoSplitConsPackDetQueryService.GetSingle(DeclarationCargoSplitId, deccargosplitconslineno, deccargosplitconsitemline, packageline, false, false);
            return DecCargoSplitConsPackDet;
        }

        public DecCargoSplitConsPackDetList GetSingleDecCargoSplitConsPackDetList(string DeclarationCargoSplitId, int deccargosplitconslineno, int deccargosplitconsitemline, int packageline, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.DecCargoSplitConsPackDet", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitConsPackDetListQueryService listService = new DecCargoSplitConsPackDetListQueryService(customContext);
            return listService.GetSingle(DeclarationCargoSplitId, deccargosplitconslineno, deccargosplitconsitemline, packageline);
        }

        public List<DecCargoSplitConsPackDetList> GetDecCargoSplitConsPackDetLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitConsPackDetListQueryService listService = new DecCargoSplitConsPackDetListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<DecCargoSplitConsPackDetList> GetDecCargoSplitConsPackDetFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            DecCargoSplitConsPackDetListQueryService listService = new DecCargoSplitConsPackDetListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        //public int GetDecCargoSplitConsPackDetFiltersCount(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    customContext = CustomContext.GetContext(tenant);
        //    DecCargoSplitConsPackDetListQueryService queryService = new DecCargoSplitConsPackDetListQueryService(customContext);
        //    QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
        //    return queryService.GetListCount(queryOperations);

        //}
    }
}
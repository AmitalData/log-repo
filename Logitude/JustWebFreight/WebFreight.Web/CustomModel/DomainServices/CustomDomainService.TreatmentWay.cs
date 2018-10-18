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
        public TreatmentWayPM GetSingleTreatmentWayPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            treatmentWayQueryService = new TreatmentWayQueryService(customContext);
            TreatmentWayPM TreatmentWay = treatmentWayQueryService.GetSingle(code, false, false);
            return TreatmentWay;
        }

        public TreatmentWayList GetSingleTreatmentWayList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.TreatmentWay", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            TreatmentWayListQueryService listService = new TreatmentWayListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<TreatmentWayList> GetTreatmentWayLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            TreatmentWayListQueryService listService = new TreatmentWayListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<TreatmentWayList> GetTreatmentWayFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            customContext = CustomContext.GetContext(tenant);
            TreatmentWayListQueryService listService = new TreatmentWayListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        //public int GetTreatmentWayFiltersCount(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    customContext = CustomContext.GetContext(tenant);
        //    TreatmentWayListQueryService queryService = new TreatmentWayListQueryService(customContext);
        //    QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
        //    return queryService.GetListCount(queryOperations);

        //}
    }
}
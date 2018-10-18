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
        public MeasurmentUnitPM GetSingleMeasurmentUnitPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            measurmentUnitQuery = new MeasurmentUnitQueryService(customContext);
            MeasurmentUnitPM MeasurmentUnit = measurmentUnitQuery.GetSingle(code, false, false);
            return MeasurmentUnit;
        }

        

        public MeasurmentUnitList GetSingleMeasurmentUnitList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            MeasurmentUnitListQueryService listService = new MeasurmentUnitListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<MeasurmentUnitList> GetMeasurmentUnitLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.MeasurmentUnit", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            MeasurmentUnitListQueryService listService = new MeasurmentUnitListQueryService(customContext);
            return listService.GetList(tenant);
        }

        //public MeasurmentUnitPM GetMeasurmantUnitByMalamId(string malamId, int tenant)
        //{
        //    customContext = CustomContext.GetContext(tenant);
        //    measurmentUnitQuery = new MeasurmentUnitQueryService(customContext);
        //    MeasurmentUnitPM MeasurmentUnit = measurmentUnitQuery.GetMeasurmentUnitByMalamId(malamId);
        //    return MeasurmentUnit;
        //}


        public List<MeasurmentUnitList> GetMeasurmentUnitFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.MeasurmentUnit", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            MeasurmentUnitListQueryService listService = new MeasurmentUnitListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetMeasurmentUnitFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
           // SecurityUtility.CheckContactFeature("Customs.MeasurmentUnit", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            MeasurmentUnitListQueryService queryService = new MeasurmentUnitListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}
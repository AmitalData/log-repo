using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Server.Tools.Helpers;


namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

         public ConverterTypePM GetSingleConverterTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            converterTypeQuery = new ConverterTypeQueryService(customContext);
            ConverterTypePM ConverterType = converterTypeQuery.GetSingle(id, false, false);
            return ConverterType;
        }

        public ConverterTypeList GetSingleConverterTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.ConverterType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ConverterTypeListQueryService listService = new ConverterTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<ConverterTypeList> GetConverterTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.ConverterType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConverterTypeListQueryService listService = new ConverterTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<ConverterTypeList> GetConverterTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.ConverterType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            ConverterTypeListQueryService listService = new ConverterTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetConverterTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
      //      SecurityUtility.CheckContactFeature("Customs.ConverterType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ConverterTypeListQueryService queryService = new ConverterTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomDocumentTypeMetaDataPM GetSingleCustomDocumentTypeMetaDataPM(string metadataTypeCode,string documentTypeCode, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customDocumentTypeMetaDataQuery = new CustomDocumentTypeMetaDataQueryService(customContext);
            CustomDocumentTypeMetaDataPM CustomDocumentTypeMetaData = customDocumentTypeMetaDataQuery.GetSingle(metadataTypeCode,documentTypeCode, true, false);
            return CustomDocumentTypeMetaData;
        }

        public CustomDocumentTypeMetaDataList GetSingleCustomDocumentTypeMetaDataList(string metadataTypeCode, string documentTypeCode, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
               //SecurityUtility.CheckContactFeature("Customs.CustomDocumentTypeMetaData", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomDocumentTypeMetaDataListQueryService listService = new CustomDocumentTypeMetaDataListQueryService(customContext);
            return listService.GetSingle(metadataTypeCode, documentTypeCode);
        }

        public List<CustomDocumentTypeMetaDataList> GetCustomDocumentTypeMetaDataLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
               //SecurityUtility.CheckContactFeature("Customs.CustomDocumentTypeMetaData", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomDocumentTypeMetaDataListQueryService listService = new CustomDocumentTypeMetaDataListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomDocumentTypeMetaDataList> GetCustomDocumentTypeMetaDataFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
                    //SecurityUtility.CheckContactFeature("Customs.CustomDocumentTypeMetaData", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomDocumentTypeMetaDataListQueryService listService = new CustomDocumentTypeMetaDataListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomDocumentTypeMetaDataFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
              //SecurityUtility.CheckContactFeature("Customs.CustomDocumentTypeMetaData", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomDocumentTypeMetaDataListQueryService queryService = new CustomDocumentTypeMetaDataListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

        public List<CustomDocumentTypeMetaDataPM> GetCustomDocumentTypeMetaDataByType(string customDocumentTypeCode, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customDocumentTypeMetaDataQuery = new CustomDocumentTypeMetaDataQueryService(customContext);
            List<CustomDocumentTypeMetaDataPM> CustomDocumentTypeMetaData = customDocumentTypeMetaDataQuery.GetCustomDocumentTypeMetaDataByType(customDocumentTypeCode);
            return CustomDocumentTypeMetaData;
        }

     


    }
}
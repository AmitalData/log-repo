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
        public CustomsDocumentUploadPM GetSingleCustomsDocumentUploadPM(string id,int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsDocumentUploadQuery = new CustomsDocumentUploadQueryService(customContext);
            CustomsDocumentUploadPM CustomsDocumentUpload = customsDocumentUploadQuery.GetSingle(id,  true, false);
            return CustomsDocumentUpload;
        }

        public CustomsDocumentUploadList GetSingleCustomsDocumentUploadList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CustomsDocumentUpload", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentUploadListQueryService listService = new CustomsDocumentUploadListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<CustomsDocumentUploadList> GetCustomsDocumentUploadLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //   SecurityUtility.CheckContactFeature("Customs.CustomsDocumentUpload", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentUploadListQueryService listService = new CustomsDocumentUploadListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsDocumentUploadList> GetCustomsDocumentUploadFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            //        SecurityUtility.CheckContactFeature("Customs.CustomsDocumentUpload", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentUploadListQueryService listService = new CustomsDocumentUploadListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsDocumentUploadFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.CustomsDocumentUpload", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsDocumentUploadListQueryService queryService = new CustomsDocumentUploadListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    

    }
}
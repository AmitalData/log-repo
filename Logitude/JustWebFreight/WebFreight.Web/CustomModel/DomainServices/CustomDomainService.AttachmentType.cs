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
        public AttachmentTypePM GetSingleAttachmentTypePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            attachmentTypeQuery = new AttachmentTypeQueryService(customContext);
            AttachmentTypePM AttachmentType = attachmentTypeQuery.GetSingle(id, false, false);
            return AttachmentType;
        }

        public AttachmentTypeList GetSingleAttachmentTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  SecurityUtility.CheckContactFeature("Customs.AttachmentType", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            AttachmentTypeListQueryService listService = new AttachmentTypeListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<AttachmentTypeList> GetAttachmentTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // SecurityUtility.CheckContactFeature("Customs.AttachmentType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AttachmentTypeListQueryService listService = new AttachmentTypeListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<AttachmentTypeList> GetAttachmentTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.AttachmentType", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            AttachmentTypeListQueryService listService = new AttachmentTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetAttachmentTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //     SecurityUtility.CheckContactFeature("Customs.AttachmentType", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            AttachmentTypeListQueryService queryService = new AttachmentTypeListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}
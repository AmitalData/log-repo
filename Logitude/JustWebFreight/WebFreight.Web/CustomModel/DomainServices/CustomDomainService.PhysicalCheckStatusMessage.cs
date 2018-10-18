using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
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
        public PhysicalCheckStatusMessagePM GetSinglePhysicalCheckStatusMessagePM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            physicalCheckStatusMessageQuery = new PhysicalCheckStatusMessageQueryService(customContext);
            PhysicalCheckStatusMessagePM PhysicalCheckStatusMessage = physicalCheckStatusMessageQuery.GetSingle(id, false, false);
            return PhysicalCheckStatusMessage;
        }

        public PhysicalCheckStatusMessageList GetSinglePhysicalCheckStatusMessageList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.PhysicalCheckStatusMessage", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            PhysicalCheckStatusMessageListQueryService listService = new PhysicalCheckStatusMessageListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public List<PhysicalCheckStatusMessageList> GetPhysicalCheckStatusMessageLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("Customs.PhysicalCheckStatusMessage", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PhysicalCheckStatusMessageListQueryService listService = new PhysicalCheckStatusMessageListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<PhysicalCheckStatusMessageList> GetPhysicalCheckStatusMessageFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.PhysicalCheckStatusMessage", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            PhysicalCheckStatusMessageListQueryService listService = new PhysicalCheckStatusMessageListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPhysicalCheckStatusMessageFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
         //   SecurityUtility.CheckContactFeature("Customs.PhysicalCheckStatusMessage", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            PhysicalCheckStatusMessageListQueryService queryService = new PhysicalCheckStatusMessageListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}
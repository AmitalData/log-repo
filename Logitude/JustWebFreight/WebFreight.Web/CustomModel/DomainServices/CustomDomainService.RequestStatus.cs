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

        public RequestStatusPM GetSingleRequestStatusPM(string code, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            requestStatusQuery = new RequestStatusQueryService(customContext);
            RequestStatusPM RequestStatus = requestStatusQuery.GetSingle(code, false, false);
            return RequestStatus;
        }

        public RequestStatusList GetSingleRequestStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RequestStatus", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            RequestStatusListQueryService listService = new RequestStatusListQueryService(customContext);
            return listService.GetSingle(code);
        }

        public List<RequestStatusList> GetRequestStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RequestStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            RequestStatusListQueryService listService = new RequestStatusListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<RequestStatusList> GetRequestStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RequestStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            RequestStatusListQueryService listService = new RequestStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetRequestStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.RequestStatus", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            RequestStatusListQueryService queryService = new RequestStatusListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }

    }
}
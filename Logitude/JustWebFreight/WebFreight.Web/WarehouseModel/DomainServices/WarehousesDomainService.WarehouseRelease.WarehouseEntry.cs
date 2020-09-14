using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.WarehouseLib.BL.EntityQueryServices;
using Logitude.WarehouseLib.Data;
using Logitude.WarehouseLib.Data.EntityListQueryServices;
using Logitude.WarehouseLib.Data.EntityLists;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.WarehouseModel.DomainServices
{
    public partial class WarehousesDomainService : LogitudeDomainService
    {
        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<WarehouseEntryList> GetWarehouseEntryFilters(byte[] xmlFilters, int tenant)
        {

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("WarehouseEntry", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = WarehouseContext.GetContext(tenant);
            }

            WarehouseEntryListQueryService listService = new WarehouseEntryListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);

            List<WarehouseEntryList> myResult = listService.GetList(queryOperations, tenant);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("WarehouseEntry", tenant, myResult.Cast<object>().ToList());

            return myResult;

        }

        public int GetWarehouseEntryFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("WarehouseEntry", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = WarehouseContext.GetContext(tenant);
            }

            WarehouseEntryListQueryService queryService = new WarehouseEntryListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);
        }

    }
}
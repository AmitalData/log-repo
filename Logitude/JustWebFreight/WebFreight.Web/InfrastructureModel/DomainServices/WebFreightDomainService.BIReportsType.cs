
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityListQueryServices;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {

        public List<BIReportsTypeList> GetBIReportsTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IInfrastructureContext objectContext = InfrastructureContext.GetContext(0);
            BIReportsTypeListQueryService listService = new BIReportsTypeListQueryService(objectContext);
            return listService.GetList(tenant);
        }

        public List<BIReportsTypeList> GetBIReportsTypesFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IInfrastructureContext objectContext = InfrastructureContext.GetContext(0);
            BIReportsTypeListQueryService listService = new BIReportsTypeListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetBIReportsTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            IInfrastructureContext objectContext = InfrastructureContext.GetContext(0);

            BIReportsTypeListQueryService queryService = new BIReportsTypeListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);

        }
    }
}
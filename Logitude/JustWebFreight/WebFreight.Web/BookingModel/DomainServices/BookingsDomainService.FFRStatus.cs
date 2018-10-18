using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityQueryServices;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityListQueryServices;
using Logitude.BookingLib.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.BookingModel.DomainServices
{
    public partial class BookingsDomainService
    {
        public FFRStatusPM GetSingleFFRStatusPM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            ffrStatusQuery = new FFRStatusQueryService(objectContext);
            FFRStatusPM FFRStatus = ffrStatusQuery.GetSingle(code, false, false);
            return FFRStatus;
        }

        public FFRStatusList GetSingleFFRStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            FFRStatusListQueryService listService = new FFRStatusListQueryService(objectContext);
            return listService.GetSingle(code);
        }

        public List<FFRStatusList> GetFFRStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            FFRStatusListQueryService listService = new FFRStatusListQueryService(objectContext);
            return listService.GetList(tenant);
        }

        public void UpdateFFRStatusList(FFRStatusList list)
        {

        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<FFRStatusList> GetFFRStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            FFRStatusListQueryService listService = new FFRStatusListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetFFRStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            FFRStatusListQueryService queryService = new FFRStatusListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }
    }
}
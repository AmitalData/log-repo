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
        public BookingStatusPM GetSingleBookingStatusPM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            bookingStatusQuery = new BookingStatusQueryService(objectContext);
            BookingStatusPM BookingStatus = bookingStatusQuery.GetSingle(code, false, false);
            return BookingStatus;
        }

        public BookingStatusList GetSingleBookingStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingStatusListQueryService listService = new BookingStatusListQueryService(objectContext);
            return listService.GetSingle(code);
        }

        public List<BookingStatusList> GetBookingStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingStatusListQueryService listService = new BookingStatusListQueryService(objectContext);
            return listService.GetList(tenant);
        }

        public void UpdateBookingStatusList(BookingStatusList list)
        {

        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<BookingStatusList> GetBookingStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingStatusListQueryService listService = new BookingStatusListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetBookingStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingStatusListQueryService queryService = new BookingStatusListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }
    }
}
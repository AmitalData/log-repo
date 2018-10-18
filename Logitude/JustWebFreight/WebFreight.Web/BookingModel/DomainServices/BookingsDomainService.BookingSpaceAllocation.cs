using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.Helpers;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityQueryServices;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityListQueryServices;
using Logitude.BookingLib.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.BookingModel.DomainServices
{
    public partial class BookingsDomainService
    {
        public BookingSpaceAllocationPM GetSingleBookingSpaceAllocationPM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            bookingSpaceAllocationQuery = new BookingSpaceAllocationQueryService(objectContext);
            BookingSpaceAllocationPM BookingSpaceAllocation = bookingSpaceAllocationQuery.GetSingle(code, false, false);
            return BookingSpaceAllocation;
        }

        public BookingSpaceAllocationList GetSingleBookingSpaceAllocationList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingSpaceAllocationListQueryService listService = new BookingSpaceAllocationListQueryService(objectContext);
            return listService.GetSingle(code);
        }

        public List<BookingSpaceAllocationList> GetBookingSpaceAllocationLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingSpaceAllocationListQueryService listService = new BookingSpaceAllocationListQueryService(objectContext);
            return listService.GetList(tenant);
        }

        public void UpdateBookingSpaceAllocationList(BookingSpaceAllocationList list)
        {

        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<BookingSpaceAllocationList> GetBookingSpaceAllocationFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingSpaceAllocationListQueryService listService = new BookingSpaceAllocationListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetBookingSpaceAllocationFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingSpaceAllocationListQueryService queryService = new BookingSpaceAllocationListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }
    }
}
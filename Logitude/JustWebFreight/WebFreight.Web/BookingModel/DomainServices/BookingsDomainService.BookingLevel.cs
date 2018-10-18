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
        public BookingLevelPM GetSingleBookingLevelPM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            bookingLevelQuery = new BookingLevelQueryService(objectContext);
            BookingLevelPM BookingLevel = bookingLevelQuery.GetSingle(code, false, false);
            return BookingLevel;
        }

        public BookingLevelList GetSingleBookingLevelList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingLevelListQueryService listService = new BookingLevelListQueryService(objectContext);
            return listService.GetSingle(code);
        }

        public List<BookingLevelList> GetBookingLevelLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingLevelListQueryService listService = new BookingLevelListQueryService(objectContext);
            return listService.GetList(tenant);
        }

        public void UpdateBookingLevelList(BookingLevelList list)
        {

        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<BookingLevelList> GetBookingLevelFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingLevelListQueryService listService = new BookingLevelListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetBookingLevelFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingLevelListQueryService queryService = new BookingLevelListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }
    }
}
using Logitude.BL.Helpers;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityQueryServices;
using Logitude.BookingLib.BL.EntityUpdateServices;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityListQueryServices;
using Logitude.BookingLib.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
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
        private BookingProductQueryService myBookingProductQueryService;

        public BookingProductPM GetSingleBookingProductPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            myBookingProductQueryService = new BookingProductQueryService(objectContext);
            BookingProductPM entityPM = myBookingProductQueryService.GetSingle(id, false, false);
            return entityPM;
        }

        public BookingProductList GetSingleBookingProductList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingProductListQueryService listService = new BookingProductListQueryService(objectContext);
            return listService.GetSingle(id);
        }

        public List<BookingProductList> GetBookingProductLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingProductListQueryService listService = new BookingProductListQueryService(objectContext);
            return listService.GetList(tenant);
        }

        public void UpdateBookingProductList(BookingProductList list)
        {

        }

        [System.ServiceModel.DomainServices.Server.Query(HasSideEffects = true)]
        public List<BookingProductList> GetBookingProductFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingProductListQueryService listService = new BookingProductListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetBookingProductFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            BookingProductListQueryService queryService = new BookingProductListQueryService(objectContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }

        public void InsertBookingProduct(BookingProductPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(0);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(0);
            }

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            BookingProductUpdateService service = new BookingProductUpdateService(objectContext, new Dictionary<string, IContext>(), 0);
            service.Update(entityPM, true);

            TableLastUpdateClass.UpdateTableHistory(0, "BookingProduct");
        }

        public void UpdateBookingProduct(BookingProductPM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(0);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(0);
            }

            entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            BookingProductUpdateService service = new BookingProductUpdateService(objectContext, new Dictionary<string, IContext>(), 0);
            service.Update(entityPM, true);

            TableLastUpdateClass.UpdateTableHistory(0, "BookingProduct");
        }
    }
}
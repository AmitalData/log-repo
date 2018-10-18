using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public CustomsPartnersItemPM GetSingleCustomsPartnersItemPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsPartnersItemQuery = new CustomsPartnersItemQueryService(customContext);
            CustomsPartnersItemPM CustomsPartnersItem = customsPartnersItemQuery.GetSingle(id, true, false);
            return CustomsPartnersItem;
        }

        public CustomsPartnersItemList GetSingleCustomsPartnersItemList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsPartnersItem", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            CustomsPartnersItemListQueryService listService = new CustomsPartnersItemListQueryService(customContext);
            return listService.GetSingle(id);
        }


        public List<CustomsPartnersItemList> GeCustomsPartnersItemsForSelection(string vendorId, string CustomerId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsPartnersItemQuery = new CustomsPartnersItemQueryService(customContext);
            List<CustomsPartnersItemList> customsPartnersItems = customsPartnersItemQuery.GetItemsByVendorOrCustomer(vendorId, CustomerId, tenant);
            return customsPartnersItems;
        }

        public CustomsPartnersItemPM GetSingleCustomsPartnersItemByCode(string itemCode, string vendorId, string CustomerId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            customsPartnersItemQuery = new CustomsPartnersItemQueryService(customContext);
            CustomsPartnersItemPM customsPartnersItem = customsPartnersItemQuery.GetItemsByCode(itemCode,vendorId, CustomerId, tenant);
            return customsPartnersItem;
        }

        public List<CustomsPartnersItemList> GetCustomsPartnersItemLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsPartnersItem", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsPartnersItemListQueryService listService = new CustomsPartnersItemListQueryService(customContext);
            return listService.GetList(tenant);
        }


        public List<CustomsPartnersItemList> GetCustomsPartnersItemFilters(byte[] xmlFilters, int tenant)
        {


            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsPartnersItem", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            CustomsPartnersItemListQueryService listService = new CustomsPartnersItemListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetCustomsPartnersItemFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Customs.CustomsPartnersItem", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            CustomsPartnersItemListQueryService queryService = new CustomsPartnersItemListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertCustomsPartnersItem(CustomsPartnersItemPM entityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.CustomsPartnersItem", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

            CustomsPartnersItemUpdateService service = new CustomsPartnersItemUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);


            service.Update(entityPm, true);

        }

        public void UpdateCustomsPartnersItem(CustomsPartnersItemPM currententityPm)
        {
            SecurityUtility.CheckContactFeature("Customs.CustomsPartnersItem", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            CustomsPartnersItemUpdateService service = new CustomsPartnersItemUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

            service.Update(currententityPm, true);

        }


        public void UpdateCustomsPartnersItemList(CustomsPartnersItemList list)
        {

        }


    }
}
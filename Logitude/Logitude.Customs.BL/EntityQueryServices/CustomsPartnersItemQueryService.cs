using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.EntityQueryServices
{
   public partial class CustomsPartnersItemQueryService
    {
       public List<CustomsPartnersItemList> GetItemsByVendorOrCustomer(string vendorId, string customerId, int tenant)
       {
           List<CustomsPartnersItem> customsPartnersItems = null;
           List<CustomsPartnersItemList> customsPartnersItemsList = new List<CustomsPartnersItemList>();
           CustomsPartnersItemList customsPartnersItemList = null;
           var setting = CustomsSettingQueryService.GetSettingByTenant(tenant);
           if (setting != null)
           {
               customsPartnersItems = repository.GetItemsForCustomerOrVendor(vendorId, customerId, tenant);
               if (!setting.IsConnectedToUniFreight)
               {


                   if (customsPartnersItems.Count > 0)
                   {
                       foreach (CustomsPartnersItem item in customsPartnersItems)
                       {
                           customsPartnersItemList = new CustomsPartnersItemList()
                           {
                               Id = item.Id,
                               ClassificationCode = item.ClassificationCode,
                               ItemCode = item.ItemCode,
                               CustomerId = item.CustomerId,
                               CustomerName = item.Client != null ? item.Client.FullName : null,
                               Name = item.Name,
                               VendorId = item.VendorId,
                               VendorName = item.CustomsVendor != null ? item.CustomsVendor.VendorName : null,
                               Tenant = item.Tenant,
                               SearchFields = item.SearchFields,
                           };
                           customsPartnersItemsList.Add(customsPartnersItemList);
                       }

                   }

               }
               else
               {
                   var cntxt=AmitalContext.GetContext(tenant);
                   var myGTBITEMQueryService = new GTBITEMQueryService(cntxt);
                   //myGTBITEMQueryService
               }
               
           }
           return customsPartnersItemsList;
       }

       public CustomsPartnersItemPM GetItemsByCode(string itemCode, string vendorId, string customerId, int tenant)
       {
           CustomsPartnersItem item = repository.GetSingleItemByCode(itemCode,vendorId, customerId, tenant);
           CustomsPartnersItemPM customsPartnersItemPM = null;

           if (item != null)
           {
               customsPartnersItemPM = new CustomsPartnersItemPM()
                               {
                                   Id = item.Id,
                                   ClassificationCode = item.ClassificationCode,
                                   ItemCode = item.ItemCode,
                                   CustomerId = item.CustomerId,
                                   CustomerName = item.Client != null ? item.Client.FullName : null,
                                   Name = item.Name,
                                   VendorId = item.VendorId,
                                   VendorName = item.CustomsVendor != null ? item.CustomsVendor.VendorName : null,
                                   Tenant = item.Tenant,
                                   SearchFields = item.SearchFields,
                               };
           }
          
       
             
           return customsPartnersItemPM;
       }
    }
}

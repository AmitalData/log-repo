using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
  public partial class CustomerTenantAccessMapping
    {
      public static void MapEntity(CustomerTenantAccessPM entityPM, CustomerTenantAccess entityPOCO, bool isNewState,string loggedContactId,Tenant loggedTenant)
        {
            if (isNewState)
            {
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Id = entityPM.Id;
                entityPOCO.CustomerTenant = entityPM.CustomerTenant;
                entityPOCO.UpdatedByUserId = loggedContactId;
                entityPOCO.RequestDateTime = TenantServerConfigration.GetCurrentDateTime(loggedTenant.Id);

            }

            else
            {
                entityPOCO.RequestDateTime = entityPM.RequestDateTime;
            }
            entityPOCO.LastShipmentDate = entityPM.LastShipmentDate;
            entityPOCO.ContactName = entityPM.ContactName;
            entityPOCO.CompanyVat = entityPM.CompanyVat;
            entityPOCO.CompanyName = entityPM.CompanyName;
            entityPOCO.CompanyEmail = entityPM.CompanyEmail;
            entityPOCO.ContactPhone = entityPM.ContactPhone;
            entityPOCO.ContactMobile = entityPM.ContactMobile;
            //entityPOCO.RequestDateTime = entityPM.RequestDateTime;
            entityPOCO.Status = entityPM.Status;
            entityPOCO.LastUpdateDate = TenantServerConfigration.GetCurrentDateTime(loggedTenant.Id);
            entityPOCO.UpdatedByUserId = loggedContactId;
            entityPOCO.IsPrivateLabelCustomer = entityPM.IsPrivateLabelCustomer;
            entityPOCO.StockTypeCode = entityPM.StockTypeCode;

            BuildSearchFields(entityPM, entityPOCO);
        }

          private static void BuildSearchFields(CustomerTenantAccessPM entityPM, CustomerTenantAccess entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CompanyName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CompanyEmail);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ContactName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CompanyVat);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerTenant.ToString());
            foreach (var item in entityPM.CustomerTenantAccessCards)
            {
                if (!string.IsNullOrEmpty(item.CustomerCode))
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, item.CustomerCode); 
                }
                MethodHelper.AddToSearchFields(ref mySearchFields, item.Tenant.ToString()); 

            }
            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}

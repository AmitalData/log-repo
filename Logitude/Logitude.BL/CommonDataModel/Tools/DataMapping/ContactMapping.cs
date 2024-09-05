using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Server.Infrastructure;
using Simplog.Data.Helpers;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class ContactMapping
    {
        public static void MapEntity(ContactPM entityPM, Contact entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPOCO.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                TenantPM tenant = GetCurrentTenant(entityPM.Tenant);
                entityPOCO.DontShowLocalLabels = tenant.LayoutDirection == "rtl" ? false : true; //bug 44449
            }

            entityPOCO.ComputedKey = (!string.IsNullOrEmpty(entityPM.Email) ? entityPM.Email : entityPM.Id);
            entityPOCO.Anniversary = entityPM.Anniversary;
            entityPOCO.Birthday = entityPM.Birthday;
            entityPOCO.BusinessPhone = entityPM.BusinessPhone;
            entityPOCO.Email = entityPM.Email;
            entityPOCO.EnglishName = entityPM.EnglishName;
            entityPOCO.FacebookId = entityPM.FacebookId;
            entityPOCO.Fax = entityPM.Fax;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.LocalName = entityPM.LocalName;
            entityPOCO.Mobile = entityPM.Mobile;
            entityPOCO.DisplayGettingStarted = entityPM.DisplayGettingStarted;
            entityPOCO.Notes = entityPM.Notes;
            entityPOCO.Signature = entityPM.Signature;
            entityPOCO.SignatureHtml = entityPM.SignatureHtml;
            entityPOCO.ExternalId = entityPM.ExternalId;
            entityPOCO.BirthdayReminder = entityPM.BirthdayReminder;
            entityPOCO.AnniversaryReminder = entityPM.AnniversaryReminder;
            entityPOCO.ImageDetailId = entityPM.ImageDetailId;
            entityPOCO.DoneDate = entityPM.DoneDate;
            entityPOCO.BirthDayOfYear = entityPM.Birthday != null ? entityPM.Birthday.Value.DayOfYear : 0;
            entityPOCO.ContactDoneMethodCode = entityPM.ContactDoneMethodCode;
            entityPOCO.Position = entityPM.Position;
            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.DigitalPortalLanguage = entityPM.DigitalPortalLanguage;
           
            
                entityPOCO.DontShowLocalLabels = !isNewState ? entityPM.DontShowLocalLabels : entityPOCO.DontShowLocalLabels;

           

            //entityPOCO.DontShowLocalLabels = LogitudeSettings.WorkEnvironment == "customs" ? false : true; //bug 44449
            if (entityPM.CompanyName != null)
            {
                if (entityPM.CompanyName.Length > 1000)
                {
                    entityPM.CompanyName = entityPM.CompanyName.Substring(0, 1000);
                }
            }

            entityPOCO.CompanyName = entityPM.CompanyName;
            entityPOCO.ContactForAccounting = entityPM.ContactForAccounting;
            BuildSearchFields(entityPM, entityPOCO);
        }

        public static TenantPM GetCurrentTenant(int id)
        {
            TenantQuery tenantQuery = new TenantQuery(id);
            TenantPM tenant = tenantQuery.GetTenantFromDB(id);
            return tenant;
        }

        public static void BuildSearchFields(ContactPM entityPM, Contact entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Email);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.BusinessPhone);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Mobile);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Fax);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CompanyName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
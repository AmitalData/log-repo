using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class AddressMapping
    {
        public static void MapEntity(AddressPM entityPM, Address entityPOCO, bool isNewEntity)
        {
            if (isNewEntity)
            {
                entityPOCO.Tenant = entityPM.Tenant;
            }
            //if (!string.IsNullOrEmpty(entityPM.ExternalId) && string.IsNullOrEmpty(entityPOCO.ExternalId))
            //{
            //    entityPOCO.ExternalId = entityPOCO.Id;
            //}
            entityPOCO.Address1 = entityPM.Address1;
            entityPOCO.Address2 = entityPM.Address2;
            entityPOCO.AddressTypeId = entityPM.AddressTypeId;
            entityPOCO.ATTN = entityPM.ATTN;
            entityPOCO.CardId = entityPM.CardId;
            entityPOCO.City = entityPM.City;
            entityPOCO.CountryId = entityPM.CountryId;
            entityPOCO.Description = entityPM.Description;
            entityPOCO.FaxNumber = entityPM.FaxNumber;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.PhoneNumber = entityPM.PhoneNumber;
            entityPOCO.StateId = entityPM.StateId;            
            entityPOCO.ZipCode = entityPM.ZipCode;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.IsLocalLanguage = entityPM.IsLocalLanguage;
            entityPOCO.ExternalId = entityPM.ExternalId;

            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void BuildSearchFields(AddressPM entityPM, Address entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Address1);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Address2);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ZipCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ATTN);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.City);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CountryEnglishName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
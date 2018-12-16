


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
    public class CustomsShipperMapping
    {
        public static void MapEntity(CustomsShipperPM entityPM, CustomsShipper entityPOCO, bool isNewState, Card entityCard)
        {
            if (isNewState)
            {
                entityCard.Id = entityPOCO.Id = entityPM.Id;
                entityCard.Tenant = entityPOCO.Tenant = entityPM.Tenant;

                entityCard.Code = entityPM.CustomsShipperCode;
            }

            // Map To Card
            entityCard.Code = entityPM.CustomsShipperCode;
            entityCard.EnglishName = entityPM.EnglishName;
            entityCard.VatNumber = entityPM.ShipperVAT;
            entityCard.CountryId = entityPM.CountryId;
            entityCard.CountryCode = entityPM.CountryCode;
            entityCard.CountryName = entityPM.CountryName;


            // Map To CustomsShipperPM
            entityPOCO.ValidDepositionNumber = entityPM.ValidDepositionNumber;
            entityPOCO.CustomsShipperCode = entityPM.CustomsShipperCode;
            entityPOCO.FutureDepositionExist = entityPM.FutureDepositionExist;
            entityPOCO.ValidityStartDate = entityPM.ValidityStartDate;
            entityPOCO.ValidityEndDate = entityPM.ValidityEndDate;




            BuildSearchFields(entityPM, entityCard);
        }

        private static void BuildSearchFields(CustomsShipperPM entityPM, Card entityCard)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomsShipperCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.CityName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.CountryName);


            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityCard.SearchFields = mySearchFields;
        }



    }
}


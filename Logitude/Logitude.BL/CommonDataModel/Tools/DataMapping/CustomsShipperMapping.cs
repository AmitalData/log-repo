


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
                entityCard.Code = entityPM.Code;
                entityCard.Id = entityPOCO.Id = entityPM.Id;
                entityCard.Tenant = entityPOCO.Tenant = entityPM.Tenant;
                entityCard.CreateDate = entityPM.CreateDate;
                entityCard.CreatedByUserId = entityPM.CreatedByUserId;
            }
            

            // Map To Card
            entityCard.UpdateDate = entityPM.UpdateDate;
            entityCard.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityCard.EnglishName = entityPM.EnglishName;
            entityCard.LocalName = entityPM.LocalName;
            entityCard.VatNumber = entityPM.ShipperVAT;
            
            
            // Map To CustomsShipperPM
            entityPOCO.ValidDepositionNumber = entityPM.ValidDepositionNumber;
            entityPOCO.CustomsShipperCode = entityPM.CustomsShipperCode;
            entityPOCO.ValidityStartDate = entityPM.ValidityStartDate;
            entityPOCO.ValidityEndDate = entityPM.ValidityEndDate;
            BuildSearchFields(entityPM, entityCard);

            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        private static void BuildSearchFields(CustomsShipperPM entityPM, Card entityCard)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomsShipperCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ValidDepositionNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityCard.SearchFields = mySearchFields;
        }



    }
}


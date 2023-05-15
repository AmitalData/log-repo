using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
   public class HybridPartnerMapping
    {
       public static void MapEntity(HybridPartnerPM entityPM, HybridPartner entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            entityPOCO.Name = entityPM.Name;
            entityPOCO.LocalName = entityPM.LocalName;
            entityPOCO.LogoId = entityPM.LogoId;
            entityPOCO.PartnerTenant = entityPM.PartnerTenant.Value;
            entityPOCO.SmallLogoId = entityPM.SmallLogoId;
            entityPOCO.IsMislakaActivated = entityPM.IsMislakaActivated;
            entityPOCO.IsExternalPartner = entityPM.IsExternalPartner;
            entityPOCO.ReceiveAllStatuses = entityPM.ReceiveAllStatuses;
            entityPOCO.AllowSendingDocsToAgent = entityPM.AllowSendingDocsToAgent;
            entityPOCO.InActive = entityPM.InActive;
            BuildSearchFields(entityPM, entityPOCO);
        }

       private static void BuildSearchFields(HybridPartnerPM entityPM, HybridPartner entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
         


            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}

using Logitude.AmitalMessaging.Infrastructure.SystemTable;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsHouseTypeUpdateService : ICanUpdateClosedTable<CustomsHouseTypePM>
    {
        protected override void OnUpdating(CustomsHouseTypePM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            CustomsHouseTypeAdditionalRepository additionalRep = new CustomsHouseTypeAdditionalRepository(context);
            CustomsHouseTypeAdditional additional = additionalRep.GetSingleAdditionalByCode(entityPM.Code, entityPM.Tenant);
            if (additional != null)
            {
                
                additional.TransportModeId = entityPM.TransportModeId;
                additional.UnloadPortCode = entityPM.UnloadPortCode;
                additionalRep.Update(additional);
            }
            else
            {
                additional = new CustomsHouseTypeAdditional();
                additional.TransportModeId = entityPM.TransportModeId;
                additional.UnloadPortCode = entityPM.UnloadPortCode;
                additional.Id = IdCounter.GetNumber("Customs.CustomsHouseTypeAdditional", entityPM.Tenant);
                additional.Code = entityPM.Code;
                additional.Tenant = entityPM.Tenant;
                ForceDefault(entityPM, additional);

                additional.SearchFields = entityPM.Code + ','+ entityPM.EnglishName + ',' + entityPM.LocalName;

                additionalRep.Add(additional);
            }
            var setting = CustomsSettingQueryService.GetSettingByTenant(entityPM.Tenant);
            if (setting != null)
            {
                if (setting.IsConnectedToUniFreight)
                {
                    if (HttpContextUtil.IsCustomDomainService())
                    {
                        var newTABLEDATA = new TABLEDATA()
                        {
                            TABLEDATA_ID = entityPM.Code,
                            TABLEDATA_NAME_ENG = entityPM.EnglishName,
                            TABLEDATA_NAME_HEB = entityPM.LocalName,
                            TABLEDATA_ADDITIONALCODE1 = entityPM.TransportModeId,
                            TABLEDATA_BLOCKED = entityPM.Inactive == true ? "T": "F",
                            //TABLEDATA_REMARKS = (recMehes.updateDate.HasValue ? recMehes.updateDate.Value.ToString() : "")
                        };

                        var myCUSTOMS_TABLE = new CUSTOMS_TABLE();
                        myCUSTOMS_TABLE.TABLECODE = new TABLECODE[] { new TABLECODE { TABLECODE_ID = "2011" } }; ;
                        var myTABLEDATAList = new List<TABLEDATA>();
                        myTABLEDATAList.Add(newTABLEDATA);
                        myCUSTOMS_TABLE.TABLECODE[0].TABLEDATA = myTABLEDATAList.ToArray();
               
                        UServerCommunication.SendUpdateTableToUnifreight(entityPM.Tenant, "2011", myCUSTOMS_TABLE);
                    }
                }
            }
            
        }

        private static void ForceDefault(CustomsHouseTypePM entityPM, CustomsHouseTypeAdditional additional)
        {
            ///<<<Task 12655:שיפור בניתוח טבלה - CustomsHouseTypes - 2011
            switch (entityPM.Code)
            {
                case "1"://חיפה
                    additional.TransportModeId = additional.TransportModeId ?? "O";
                    additional.UnloadPortCode = additional.UnloadPortCode ?? "ILHFA";
                    break;
                case "2"://אשדוד
                    additional.TransportModeId = additional.TransportModeId ?? "O";
                    additional.UnloadPortCode = additional.UnloadPortCode ?? "ILASH";
                    break;
                case "4"://עבור קוד 4 - נתב"ג
                    additional.TransportModeId = additional.TransportModeId ?? "A";
                    additional.UnloadPortCode = additional.UnloadPortCode ?? null;
                    break;
                case "9"://עבור קוד 9 - אילת
                    additional.TransportModeId = additional.TransportModeId ?? "A";
                    additional.UnloadPortCode = additional.UnloadPortCode ?? "ILETH"; // "ILELT";
                    break;
                default:
                    break;
            }
            ///<<<Task 12655:שיפור בניתוח טבלה - CustomsHouseTypes - 2011
        }
    }
}

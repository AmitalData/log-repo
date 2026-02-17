using Logitude.Customs.Def.ClosedTable;
using Logitude.Customs.BL.EntityQueryServices;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Utils
{
    public class CustomsSettingUtil
    {
        public static string GetSufix(int tenant)
        {
            CustomsDeploymentStage customsDeploymentStage = GetCustomsDeploymentStage(tenant);
            switch (customsDeploymentStage)
            {

                //case CustomsDeploymentStage.Test:
                //    return ".TST.xml";
                case CustomsDeploymentStage.Pilot:
                case CustomsDeploymentStage.PrePilot:
                    return ".PLT.xml";
                    break;
                case CustomsDeploymentStage.Production:
                    return ".PRD.xml";
                    break;

                case CustomsDeploymentStage.Test:
                    /*
    INSERT INTO "CUSTOMSENVOIRMENTTYPES" (CODE, ENGLISHNAME, LOCALNAME, SEARCHFIELDS, INACTIVE) VALUES ('4', 'Test', 'בדיקות4', 'בדיקות,TSTתTEST', '0')
    Commit Successful
    UPDATE "CUSTOMSSETTINGS" SET CUSTOMSENVOIRMENTTYPECODE = '4' 
    Commit Successful
                     */
                    return ".TST.xml";
                default:
                    return ".KHL.xml";
                    //    case CustomsDeploymentStage.None:
                    //    return ".KHL.xml";
                    //    break;
                    

                    break;
            }

        }

        public static CustomsDeploymentStage GetCustomsDeploymentStage(int tenant)
        {
            return CustomsSettingQueryService.GetSettingByTenant(tenant).MyCustomsDeploymentStage;
        }
        public static bool ForceDownloadXapFromIIS()
        {
            var customsSettingQueryService = new CustomsSettingQueryService(0);
            var def = customsSettingQueryService.GetAll().FirstOrDefault(rec => !String.IsNullOrWhiteSpace(rec.CustomsAgentId));
            if (def == null)
            {
                return false;
            }
            return true;
        }
    }
}

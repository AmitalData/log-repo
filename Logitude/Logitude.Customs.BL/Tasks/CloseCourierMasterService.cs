using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Tasks
{
    public class CloseCourierMasterService: ICustomsCloseCourierMasterService
    {
        public void StartRun(string taskId, int seedDefaultTenant)
        {

            //seedDefaultTenant==0
            var customsSettingQueryService = new CustomsSettingQueryService(seedDefaultTenant);
            var allCustomsSetting = customsSettingQueryService.GetAll();
            allCustomsSetting.ForEach(t => RunPerTenant(t));
            //for (int i = 0; i <= 3; i++)
            //{
            //    LogMessagingUtil.Instance.AppendLine("Log warning # " + i + " , Be careful !!");
            //}


        }

        private void RunPerTenant(CustomsSettingPM t)
        {
            LogMessagingUtil.Instance.AppendLine($"RunPerTenant({t.Tenant})");

            CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(t.Tenant);
            List<CourierMasterPM> courierMasterPMList = courierMasterQueryService.GetAllCourierMastersForClosing(t.Tenant);
            if(courierMasterPMList != null)
            {
                LogMessagingUtil.Instance.AppendLine($"נמצאו " + courierMasterPMList.Count() + " טיסות פתוחות לסגירה " + "\n");
                ICustomContext dbContext = CustomContext.GetContext(t.Tenant);
                CourierMasterUpdateService CourierMasterUpdateService = new CourierMasterUpdateService(dbContext, new Dictionary<string, IContext>(), t.Tenant);
                foreach (CourierMasterPM courierMasterPMItem in courierMasterPMList)
                {
                    try
                    {
                        courierMasterPMItem.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        courierMasterPMItem.IsOpen = false;
                        CourierMasterUpdateService.Update(courierMasterPMItem, true);
                        LogMessagingUtil.Instance.AppendLine($"נסגרה טיסה " + courierMasterPMItem.AirlinePrefix + "-" + courierMasterPMItem.MAWB + "\n");
                    }
                    catch
                    {
                        LogMessagingUtil.Instance.AppendLine($"לא נסגרה טיסה " + courierMasterPMItem.AirlinePrefix + "-" + courierMasterPMItem.MAWB + "\n");
                    }
                }
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine($"לא נמצאו טיסות פתוחות לסגירה");
            }
        }
    }
    
}

using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.CustomsMessaging.Tasks
{
    public class SendManifestService : ICustomsSendManifestService
    {
        public void StartRun(string taskId, int seedDefaultTenant)
        {

            var customsSettingQueryService = new CustomsSettingQueryService(seedDefaultTenant);
            var allCustomsSetting = customsSettingQueryService.GetAll();
            allCustomsSetting.ForEach(t => RunPerTenant(t));


        }

        private void RunPerTenant(CustomsSettingPM t)
        {
            FeatureQuery featureQuery = new FeatureQuery();
            var usrid = AuthenticationUtil.ResolveUserId(t.Tenant);
            var features = featureQuery.GetAllowedFeaturesForLoggedUser(usrid, t.Tenant);
            var feature = features.Features.FirstOrDefault(x => x.Code == "SendManifest");
            if (feature != null)
            {
                LogMessagingUtil.Instance.AppendLine($"RunPerTenant({t.Tenant})");

                CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(t.Tenant);
                List<CourierMaster> courierMasters = courierMasterQueryService.GetAllCourierMastersToSendAutoManifest(t.Tenant);

                foreach (var courierMaster in courierMasters)
                {

                    var messagingService = new DCAInUCB1170_MsgMessagingService();
                    var sts = messagingService.CreateCRS(t.Tenant, null,
                        new SendALLCorrectRequestParams()
                        {
                            CourierMasterId = courierMaster.Id,
                            HAWB = courierMaster.HAWB,
                            // CourierDeclarationStatusCode = courierMaster.
                        }

                        );
                }
            } else
            {
                LogMessagingUtil.Instance.AppendLine("אין הרשאות למתזמן");
            }


            //CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(t.Tenant);
            //List<CourierMasterPM> courierMasterPMList = courierMasterQueryService.GetAllCourierMastersForClosing(t.Tenant);
            //if(courierMasterPMList != null)
            //{
            //    LogMessagingUtil.Instance.AppendLine($"נמצאו " + courierMasterPMList.Count() + " טיסות פתוחות לסגירה " + "\n");
            //    ICustomContext dbContext = CustomContext.GetContext(t.Tenant);
            //    CourierMasterUpdateService CourierMasterUpdateService = new CourierMasterUpdateService(dbContext, new Dictionary<string, IContext>(), t.Tenant);
            //    foreach (CourierMasterPM courierMasterPMItem in courierMasterPMList)
            //    {
            //        try
            //        {
            //            courierMasterPMItem.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            //            courierMasterPMItem.IsOpen = false;
            //            CourierMasterUpdateService.Update(courierMasterPMItem, true);
            //            LogMessagingUtil.Instance.AppendLine($"נסגרה טיסה " + courierMasterPMItem.AirlinePrefix + "-" + courierMasterPMItem.MAWB + "\n");
            //        }
            //        catch
            //        {
            //            LogMessagingUtil.Instance.AppendLine($"לא נסגרה טיסה " + courierMasterPMItem.AirlinePrefix + "-" + courierMasterPMItem.MAWB + "\n");
            //        }
            //    }
            //}
            //else
            //{
            //    LogMessagingUtil.Instance.AppendLine($"לא נמצאו טיסות פתוחות לסגירה");
            //}
        }
    }

}

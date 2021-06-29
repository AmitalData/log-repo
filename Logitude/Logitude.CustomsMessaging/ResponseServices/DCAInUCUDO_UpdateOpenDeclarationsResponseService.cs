
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Utils;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.BL.Messaging.U2L.DeclarationDocuments;
using Logitude.AmitalMessaging.Utils;
using Logitude.Server.Tools.Utils;
using System.Configuration;
using Logitude.CustomsMessaging.U2L.CommDec;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Server.Tools.Models;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.CustomsMessaging.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DCAInUCUDO_UpdateOpenDeclarationsResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, UpdateOpenDeclarationsResponseContentHeader, UpdateOpenDeclarationsRequestParams>
    {
        public override INF_MSG_GenericResponseData GetResponse(UpdateOpenDeclarationsResponseContentHeader customResponse, UpdateOpenDeclarationsRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(UpdateOpenDeclarationsResponseContentHeader customResponse, UpdateOpenDeclarationsRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();


            bool lockit = !string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("Singleton.CRS:UCUDO"));
            // string key = ProcessLockTableUtil.Instance.GetKey4DocumentsFilingId(customResponse.CustomFileNo, requestParams.Tenant);

            // using (var processLockTableDisposable = ProcessLockTableUtil.Instance.GetProcessLockTableDisposable(requestParams.Tenant, lockit, key, "CRS:UCUDO"))
            // {
            string customFileNo = "";

            CommDecService CommDecService = new CommDecService();
                try {
                    string error = "";
                string decId = "";
                var context = CustomContext.GetContext(requestParams.Tenant);


                DeclarationCourierStatusRepository rep = new DeclarationCourierStatusRepository(context);

                   List< DeclarationCourierStatus> decCouriers = rep.GetByMasterIDDeclarationCourierStatus(requestParams.Tenant, requestParams.LoggingEntityId);
                CourierMasterRepository courierMasterRepository = new CourierMasterRepository(context);
                CourierMasterQueryService courierMasterQueryService = new CourierMasterQueryService(courierMasterRepository);
                CourierMasterPM courierMasterPM = courierMasterQueryService.GetSingle(customResponse.EntityId, false, false);

                if (decCouriers != null )

                    {

                        DateTime stopLogAt = new DateTime(2021, 06, 01);

                        string logData = "";

                    var loggedUser = requestParams.LoggingUserId;

                    //  logData = $"_CourierDeclarationPM.DeclarationId={_CourierDeclarationPM.DeclarationId}, ChangeSetOp={_CourierDeclarationPM.ChangeSetOp}, decCourier.IsClosedForFollowUp={decCourier.IsClosedForFollowUp},OpenDeclarations ={courierMaster.OpenDeclarations}before update1";

                    //    LogitudeSettings.HandleLogMe("OpenDeclarations " + logData, false, "time", stopLogAt);


                    courierMasterPM.OpenDeclarations = decCouriers.Count(x => x.IsClosedForFollowUp == false);

                    CourierMasterUpdateService courierMasterUpdateService = new CourierMasterUpdateService(context);


                    courierMasterUpdateService.Update(courierMasterPM, true);

                       // courierMasterRepository.SubmitChanges();
                    }

              

                this.MyResponseData.ApplicationID = customFileNo;
                     this.MyResponseData.HasException = false;
                    this.MyResponseData.Succeeded = true;


                this.MyRequestSheetParam = new RequestSheetParam();
                this.MyRequestSheetParam.CustomFileNo = courierMasterPM.MAWB;
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");
                 this.MyRequestSheetParam.EntityId1 = decId;
             //   this.MyRequestSheetParam.RequestDescription = "הצהרה נפתחה בהצלחה :" + customFileNo + "_" + decId;


            }
            catch (Exception ex)
                {
                    this.MyResponseData.ApplicationID = customFileNo;
                    this.MyResponseData.HasException = true;
                    this.MyResponseData.Succeeded = false;
                    this.MyResponseData.UserMessage = ex.Message;
                throw new Exception(ex.Message);
                }
           // }
        }

        
    }
 }

using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.Repsitories;
using Microsoft.Practices.Unity;
using Logitude.CustomsMessaging.Utils;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class MultiUpdateBatchUCBCMSS_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBMultiUpdateWithResponseContentHeader, GenericRequestParams>
    {

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            var item = "Hello";
          /*  var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();

            var objectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var objectTableIdCourierMaster = ObjectTableRepository.GetObjectTableByName("Customs.CourierMaster");

            var qs = new DeclarationCourierStatusQueryService(context);
            List<DeclarationPM> lockedDeclarations = new List<DeclarationPM>();
            List<DeclarationCourierStatusPM> listPM = new List<DeclarationCourierStatusPM>();
            //listPM = qs.GetByMasterIDDeclarationCourierStatus(requestParams.Tenant, requestParams.AppicationId);
            if (customResponse.ServerSplitDeclarationsList == null || (customResponse.ServerSplitDeclarationsList != null && customResponse.ServerSplitDeclarationsList.Count == 0))
            {
                LogMessagingUtil.Instance.AppendLine("Splitter to 50 - Create new CRS");
                mess.AppendLine($"Splitter to 50 - Create new CRS master {requestParams.AppicationId} ");


                var DeclarationIdList = qs.GetByMasterID_DeclarationIdList(requestParams.Tenant, requestParams.AppicationId);
                DeclarationIdList.ChunkBy(50).ForEach(list50 =>
                {
                    //CreateCRS(customResponse, requestParams,list50);
                    customResponse.ServerSplitDeclarationsList = list50;
                    customResponse.LoggingUserId = requestParams.LoggingUserId;
                    var myCRSUtil = new CRSUtil();
                    myCRSUtil
                    .CreateCRS_DCAIn<DCAInUCBCMSSWithResponseContentHeader>(customResponse, (requestParams as RequestParamsBase));

                });
                this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
                this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
                this.MyResponseData.UserMessage = mess.ToString();
                this.MyResponseData.Succeeded = true;

                return;
            }

            LogMessagingUtil.Instance.AppendLine("Handle 50 DeclarationIdList");

            listPM = qs.GetByDeclarationIdList(requestParams.Tenant, customResponse.ServerSplitDeclarationsList);

            if (listPM.Count == 0)
            {
                mess.AppendLine($"There ARE  NOT any Declarations 'R'eady to (Manifest) send for master {requestParams.AppicationId} ");
            }



            foreach (DeclarationCourierStatusPM itemPM in listPM)
            {

                using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
                {
                    var context1 = CustomContext.GetContext(requestParams.Tenant);//context each CRS TRANS
                    LogMessagingUtil.Instance.AppendLine("ChangeStorgeSite for declaration: " + itemPM.DeclarationId + "\n");
                    var changeStorgeSiteService = new ChangeStorgeSiteService(context1);
                    if (customResponse.StorageSiteCode != null)
                    {
                        changeStorgeSiteService.ChangeSite(customResponse.StorageSiteCode, requestParams, mess, objectTableId, objectTableIdCourierMaster, lockedDeclarations, itemPM,false);
                    }
                    else
                    {
                        if(customResponse.UnLoadPortCode != null)
                        {
                            changeStorgeSiteService.ChangeSite(customResponse.UnLoadPortCode, requestParams, mess, objectTableId, objectTableIdCourierMaster, lockedDeclarations, itemPM,true);
                        }
                    }

                    scopeNewCRS.Complete();
                }

            }



            if (lockedDeclarations != null && lockedDeclarations.Count() > 0)
            {
                List<string> declarationsList = new List<string>();
                string message = string.Concat("אתר אחסון בטיסה השתנה ל ", customResponse.StorageSiteCode, ", אך ההצהרה לא ניתנת לעידכון. נא לעדכן ידנית");
                if (customResponse.UnLoadPortCode != null)
                {
                     message = string.Concat("אתר פריקה בטיסה השתנה ל ", customResponse.UnLoadPortCode, ", אך ההצהרה לא ניתנת לעידכון. נא לעדכן ידנית");
                }
                mess.AppendLine("\n" + "Locked Declarations: " + "\n");
                foreach (DeclarationPM itemDeclaration in lockedDeclarations)
                {
                    declarationsList.Add(itemDeclaration.CustomFileNo);
                    mess.AppendLine($" ( {itemDeclaration.Id} ),");
                }
                if (customResponse.StorageSiteCode != null)
                {
                    RaiseEvent(lockedDeclarations.FirstOrDefault(), declarationsList, "U-FSE", message);
                }
                if(customResponse.UnLoadPortCode != null)
                {
                    RaiseEvent(lockedDeclarations.FirstOrDefault(), declarationsList, "FSE", message);
                }
            }

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            this.MyResponseData.UserMessage = mess.ToString();
            this.MyResponseData.Succeeded = true;*/
        }


    }

}

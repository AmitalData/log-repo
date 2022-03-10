using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
///using Logitude.CustomsMessaging.Utils;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.Utils;
using System.Configuration;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.Messaging.Maman;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityPMs.UGenerated;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using UnifreightIIG.Common.MessageLib.ExportStorage.MN2791;
using Logitude.Customs.BL.TraceEvents;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class MN_MSG2791_ExportDeliveryAnswerMessageResponseService
        : ResponseServiceBase<INF_MSG_GenericResponseData, MN_MSG2791_ExportDeliveryAnswerMessage, GenericRequestParams>
    {

        public override INF_MSG_GenericResponseData GetResponse(MN_MSG2791_ExportDeliveryAnswerMessage customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }
        
        public override void Update(MN_MSG2791_ExportDeliveryAnswerMessage customResponse, GenericRequestParams requestParams)
        {
            if (customResponse.ResponseContentHeader.Exception != null)
            {
                LogMessagingUtil.Instance.AppendLine(customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription);
                this.MyResponseData = new INF_MSG_GenericResponseData();
                this.MyResponseData.Succeeded = true;
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception.FirstOrDefault().ExeptionDescription;
                return;
            }
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            ExportStorageQueryService service = new ExportStorageQueryService(dbContext);
            var entity = service.GetByCargoKeys(
                customResponse.CargoIdentifier.cargoIdentifierKey1,
                customResponse.CargoIdentifier.cargoIdentifierKey2,
                customResponse.CargoIdentifier.cargoIdentifierKey3,
                customResponse.CargoIdentifier.cargoIdentifierType, requestParams.Tenant);
            
            if (MyRequestSheetParam == null)
                MyRequestSheetParam = new RequestSheetParam();

            if (entity != null)
            {
                var ER1TaskStatus =new int?[] { 2, 9, 8 };
                if (ER1TaskStatus.Contains(customResponse.CargoDetails?.CargoStatusID))
                {
                    RaiseExportStorageStatus("ER1", "ER1", entity, "");
                }
                LogMessagingUtil.Instance.AppendLine("entity found, id: " + entity.Id);
                if (customResponse.Exception != null)
                {
                    LogMessagingUtil.Instance.AppendLine("customResponse.Exception");
                    string xml = "";
                    foreach (var item in customResponse.Exception)
                    {
                        xml += XmlGenericUtil<UnifreightIIG.Common.MessageLib.ExportStorage.MN2791.Exception>.MySerializeObject(item);
                        if(item.ExceptionLevel == 1)
                        {
                            RaiseExportStorageStatus("ER2", "ER2", entity, item.ExeptionDescription);
                        }
                        if (item.ExceptionLevel == 2)
                        {
                            RaiseExportStorageStatus("ALT", "ALT", entity, item.ExeptionDescription);
                        }
                    }
                    entity.StorErrorXML = xml;
                }
                
                entity.CustomsStatus = customResponse.CargoDetails?.CargoStatusID?.ToString();
                entity.ChangeSetOp = ChangeSetOperation.Update;
                var updateService = new ExportStorageUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                updateService.Update(entity, true);
                MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                MyRequestSheetParam.EntityId2 = entity.DeclarationId;
                MyRequestSheetParam.EntityId1 = entity.Id;

            }

            MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.ExportStorage");

            
            MyResponseData = new INF_MSG_GenericResponseData();
            MyResponseData.Succeeded = true;
            MyResponseData.UserMessage = "";

        }

        public static void RaiseExportStorageStatus(string statusId, string unifrieghtStatus, ExportStoragePM dirtyEntityPM, string FUStatusRemarks)
        {
            try
            {
                string loggingUserId = "";
                loggingUserId = AuthenticationUtil.ResolveUserId(dirtyEntityPM.Tenant);

                var eventContextTagModel = dirtyEntityPM.CurrentContextTag as EventContextTagModel;
                var myAmitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dirtyEntityPM.Tenant,
                    objectTableName = "Customs.ExportStorage",
                    EventCode = statusId,
                    notes = "DO_NOT_RAISE_EVENT",
                    CommunicationLoggingEntityReference = dirtyEntityPM.Id.ToString(),
                    EntityId = dirtyEntityPM.Id,
                    UserId = loggingUserId,
                    CommunicationSubject = "FU Status from logitude ",
                };

                if (!string.IsNullOrWhiteSpace(dirtyEntityPM.StorageNo))
                {
                    myAmitalEventTracerModel.MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "MSCSTORAGE",
                        primary_number = dirtyEntityPM.StorageNo,
                        status = "new",
                        xml_status = "new",
                        status_id = unifrieghtStatus,
                        status_DateTime = DateTime.Now,
                        //status_save = "no_fail",
                        comments = FUStatusRemarks,
                    };
                }
                AmitalEventTracer.CreateTraceEvent(myAmitalEventTracerModel);
            }
            catch (System.Exception)
            {
                // TODO: BL Stop Execute or Cuntinue - Ask IHAB
                throw;
            }
        }
    }
}

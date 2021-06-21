using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Models;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Linq;
using System.Collections.Generic;
using UnifreightIIG.Common.DeclarationStatusQueryRequestServiceReference;
using Logitude.Customs.BL.TraceEvents;
using System.Configuration;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityPMs.UGenerated;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.AmitalMessaging.Customs.CustomFile;
using System.Globalization;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using UnifreightIIG.Common.ContainerizationMessageServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{

    public class SaveCC_MSG2450_ContainerizationMessageResponseService
        : ResponseServiceBase<INF_MSG_GenericResponseData,
        INF_MSG_Generic,
        GenericRequestParams>
    {
        public override void OnRequestFail(INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        {
            base.OnRequestFail(customResponse, requestParams);
        }
        public override INF_MSG_GenericResponseData GetResponse(
            INF_MSG_Generic customResponse,
            GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        {
            if (customResponse.ResponseContentHeader.Exception == null)
            {
                ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
                var myContainerizationQueryService = new ContainerizationQueryService(dbContext);
                ContainerizationPM _ContainerizationPM;
                string containerizationID = requestParams.LoggingEntityId;

                _ContainerizationPM = myContainerizationQueryService.GetSingle(containerizationID, true, false);
                if (_ContainerizationPM.OperationMode == "3")
                {
                    _ContainerizationPM.ContainerizationStatus = "3";
                    _ContainerizationPM.ChangeSetOp = ChangeSetOperation.Update;
                     var ContainerizationUpdateService = new ContainerizationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                    ContainerizationUpdateService.Update(_ContainerizationPM, true);
                    
                    var myDeclarationQueryService = new DeclarationQueryService(dbContext);
                    var declarationPMs = myDeclarationQueryService.GetDeclarationsByExportContainerizationId(containerizationID);
                    foreach (var item in declarationPMs)
                    {
                        item.ExportContainerizationID = null;
                        item.ChangeSetOp = ChangeSetOperation.Update;
                        var DeclarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                        DeclarationUpdateService.Update(item, true);
                    }
                }
                this.MyResponseData = new INF_MSG_GenericResponseData()
                {
                    ApplicationID = requestParams.AppicationId,
                    Succeeded = true,
                    HasException = false,
                    UserMessage = "המכלה נשלחה בהצלחה"
                };
            }
            else
            {
                var ExeptionDescription = "";
                foreach (var rec in customResponse.ResponseContentHeader.Exception)
                {
                    if (!String.IsNullOrWhiteSpace(ExeptionDescription))
                        ExeptionDescription += Environment.NewLine;
                    ExeptionDescription += rec.ExeptionDescription;
                }
                this.MyResponseData = new INF_MSG_GenericResponseData()
                {
                    Succeeded = false,
                    ApplicationID = requestParams.AppicationId,
                    HasException = true,
                    UserMessage = ExeptionDescription,
                };
            }

        }
    }
}

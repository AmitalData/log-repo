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
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);
            var myContainerizationQueryService = new ContainerizationQueryService(dbContext);
            ContainerizationPM _ContainerizationPM;
            string containerizationID = requestParams.LoggingEntityId;

            _ContainerizationPM = myContainerizationQueryService.GetSingle(containerizationID, true, false);
            if (customResponse.ResponseContentHeader.Exception == null || !customResponse.ResponseContentHeader.Exception.Any(x=>x.ExceptionLevel == 3))
            { 
                if (_ContainerizationPM.OperationMode == "3")
                {
                    _ContainerizationPM.IsMultiCustomers = null;
                    _ContainerizationPM.ExportFile = null;
                    _ContainerizationPM.ContainerizationStatus = "3";
                    _ContainerizationPM.ConnectedDeclarations = null;
                    _ContainerizationPM.CargoTypeCode = null;
                    _ContainerizationPM.ManifestNumber = null;
                    _ContainerizationPM.SecondCargoID = null;
                    _ContainerizationPM.ThirdCargoID = null;
                    _ContainerizationPM.ExistInCustoms = null;
                    var myConsigmentQueryService = new ConsignmentQueryService(dbContext);
                    var consigmentPMs = myConsigmentQueryService.GetConsigmentByExportContainerizationID(containerizationID, requestParams.Tenant);
                    foreach (var item in consigmentPMs)
                    {
                        item.ExportContainerizationID = null;
                        item.ChangeSetOp = ChangeSetOperation.Update;
                        var ConsigmentUpdateService = new ConsignmentUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
                        ConsigmentUpdateService.Update(item, true);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(_ContainerizationPM.ExistInCustoms)) { 
                       var containerizationCargoID = (!string.IsNullOrEmpty(_ContainerizationPM.CargoTypeCode) ? (_ContainerizationPM.CargoTypeCode) : "")+ "-" + 
                           (!string.IsNullOrEmpty(_ContainerizationPM.ManifestNumber) ? (_ContainerizationPM.ManifestNumber ) : "")+ "-" + 
                           (!string.IsNullOrEmpty(_ContainerizationPM.SecondCargoID) ? (_ContainerizationPM.SecondCargoID ) : "")+ "-" + 
                           (!string.IsNullOrEmpty(_ContainerizationPM.ThirdCargoID) ? (_ContainerizationPM.ThirdCargoID ) : "");
                       
                       _ContainerizationPM.ExistInCustoms = containerizationCargoID;
                    }
                    _ContainerizationPM.ContainerizationStatus = "1";
                }

                this.MyResponseData = new INF_MSG_GenericResponseData()
                {
                    ApplicationID = requestParams.AppicationId,
                    Succeeded = true,
                    HasException = false,
                    UserMessage = "המכלה נשלחה בהצלחה" + Environment.NewLine + customResponse.ResponseContentHeader.Remark
                };
            }
            else
            {
                if (string.IsNullOrEmpty(_ContainerizationPM.ContainerizationStatus))
                    _ContainerizationPM.ContainerizationStatus = "4";
                
                if (_ContainerizationPM.ContainerizationStatus == "1")
                    _ContainerizationPM.ContainerizationStatus = "2";
                
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

            _ContainerizationPM.ChangeSetOp = ChangeSetOperation.Update;
            var ContainerizationUpdateService = new ContainerizationUpdateService(dbContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), requestParams.Tenant);
            ContainerizationUpdateService.Update(_ContainerizationPM, true);
        }
    }
}

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

        public override INF_MSG_GenericResponseData GetResponse(
            INF_MSG_Generic customResponse,
            GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(INF_MSG_Generic customResponse,
            GenericRequestParams requestParams)
        {
            
            
            


            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.ApplicationID = requestParams.AppicationId;

            MyResponseData.Succeeded = true;
            MyResponseData.HasException = false;
            MyResponseData.UserMessage ="המכלה נשלחה בהצלחה";

        }

  
 
 
     }
}

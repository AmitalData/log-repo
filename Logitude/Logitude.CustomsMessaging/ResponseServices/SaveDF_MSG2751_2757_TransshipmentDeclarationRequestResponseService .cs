using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Logitude.Customs.BL.BL;
using UnifreightIIG.Common.TransshipmentDeclarationRequestServiceReference;
using Logitude.Customs.BL.Utils;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class SaveDF_MSG2751_2757_TransshipmentDeclarationRequestResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_2757_MSG10004_ExportDeclarationResponse, GenericRequestParams>
    {
        public override void OnRequestFail(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }
            base.OnRequestFail(customResponse, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(
            DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
            UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse castCustomResponse =
               Serializer.CastXML<UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse, DF_NG_2757_MSG10004_ExportDeclarationResponse>(customResponse);

            new DF_NG_2757_MSG10004_ExportDeclarationResponseService().Update(castCustomResponse, requestParams);
        }
    }
}

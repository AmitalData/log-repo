using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.Gen;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
 using UnifreightIIG.Common.MessageLib.Collateral;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO;
using Logitude.Customs.BL.BL;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using UnifreightIIG.Common.ExportDeclarationServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class DF_NG_2757_MSG10004_ExportDeclarationResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, DF_NG_2757_MSG10004_ExportDeclarationResponse, GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;
        private bool _FastDelete;
        //private List<SupplierInvoiceItemsTaxesModPM> _SupplierInvoiceItemsTaxesModificationPMList;
        //public UnifreightIIG.Common.CommonIIGInterface.IResponseHeaderOrFault _ResponseHeaderExeption;
        public bool _IsSubmitDeclarationResponse { get; set; }
        public bool _IsRetrieveDeclarationResponse { get; set; }
        decimal? totGeneralTaxCalc = 0;
        decimal? totPurchaseCalc = 0;
        decimal? totVatCalc = 0;
        decimal? generalTax = 0;
        decimal? purchase = 0;
        decimal? vat = 0;

        DeclarationError _MyDeclarationError;
        decimal? _TotalBtlCoverageNISSum = 0;

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

            /// itzik test     TestTrans(requestParams);
            return this.MyResponseData;
        }

        private void TestTrans(GenericRequestParams requestParams) /// itzik test 
        {
            DeclarationStatusRequestParams searchParams = new DeclarationStatusRequestParams()
            {
                LoggingEnabled = true,

                CustomFileNo = _MyDeclarationPM.CustomFileNo,
                DeclarationNumber = _MyDeclarationPM.DeclarationNumber,
                Tenant = requestParams.Tenant,
                RequestName = "Declaration Status Search",
                ResponseName = "Declaration Status Search",
                SuppressSplitWR = true
            };

            searchParams.RequestVIA = SendRequestVIA.WebServiceInteractive;
            var resData = Logitude.CustomsMessaging.MessagingServices.DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService.SendInteractive(searchParams);
            if (!resData.Succeeded)
            {
                LogMessagingUtil.Instance.AppendLine("Sending Declaration Status Request Failed " + resData.CustomsRequestsSheetId + ", Message: " + resData.UserMessage);
                ///return;
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("Request Succeeded " + resData.CustomsRequestsSheetId);
            }

        }

        public override void Update(DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse, GenericRequestParams requestParams)
        {
    
            this.MyResponseData.UserMessage = "בקשה נשלחה בהצלחה";
          

            
        }


  

    }
}

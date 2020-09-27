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
using System.Xml.Serialization;
using System.Xml.Linq;
using UnifreightIIG.Common.DeclarationCancellationRequestMsgServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class SaveDF_MSG5002_DeclarationCancellationRequestMsgResponseService :
        ResponseServiceBase<INF_MSG_GenericResponseData, INF_MSG_Generic, GenericRequestParams>
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

        public override void OnRequestFail(INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }
            base.OnRequestFail(customResponse, requestParams);
        }

        public override INF_MSG_GenericResponseData GetResponse(
            INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        {

            /// itzik test     TestTrans(requestParams);
            return this.MyResponseData;
        }


        public override void Update(INF_MSG_Generic customResponse, GenericRequestParams requestParams)
        {
            ICustomContext dbContext = CustomContext.GetContext(requestParams.Tenant);

            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyResponseData.Succeeded = true;
            this.MyResponseData.HasException = false;

            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParams.Tenant);
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);

            var dec = declarationQueryService.GetSingle(requestParams.AppicationId, false, false);

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                this.MyResponseData.HasException = true;
                this.MyResponseData.UserMessage = customResponse.ResponseContentHeader.Exception[0].ExeptionDescription;
            }
            else
            {
                dec.CancelRequestStatusCode = "2";
                dec.ChangeSetOp = ChangeSetOperation.Update;
                var amitalEventTracerModel = new Logitude.Customs.BL.TraceEvents.AmitalEventTracerModel()
                {
                    Tenant = dec.Tenant,
                    objectTableName = "Customs.Declaration",
                    EventCode = "CWR",
                    notes = null,
                    CommunicationLoggingEntityReference = dec.DeclarationNumber,
                    EntityId = dec.Id,
                    UserId = requestParams.LoggingUserId,

                    CommunicationSubject = "FU Status CWR from logitude ",
                    MyFUStatus = new AmitalEventTracerModel.FUStatus()
                    {
                        entname = "CFIFILEM",
                        primary_number = dec.CustomFileNo,
                        status = "new",
                        xml_status = "new",
                        status_id = "CWR",
                        status_DateTime = DateTime.Now,
                        comments = null,
                    }
                };
                AmitalEventTracer.CreateTraceEvent(amitalEventTracerModel);

                declarationUpdateService.Update(dec, true);
                this.MyResponseData.UserMessage = "בקשת ביטול נשלחה בהצלחה.";
            }

            //DF_NG_5117_ImportDeclerationAmendmentReplyResponseService dF_NG_5117_ImportDeclerationAmendmentReplyResponseService = new DF_NG_5117_ImportDeclerationAmendmentReplyResponseService();
            //this.MyResponseData=  dF_NG_5117_ImportDeclerationAmendmentReplyResponseService.Update5117(Cast5117Msg(customResponse), requestParams);
        }







    }
}

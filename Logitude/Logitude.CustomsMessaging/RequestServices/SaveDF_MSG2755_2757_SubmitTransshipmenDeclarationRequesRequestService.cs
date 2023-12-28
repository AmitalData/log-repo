using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.Repsitories;
using Logitude.Customs.BL.BL;
using UnifreightIIG.Common.SubmitTransshipmenDeclarationRequestServiceReference;
using Logitude.Customs.BL.Utils;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class SaveDF_MSG2755_2757_SubmitTransshipmenDeclarationRequesRequestService
        : RequestServiceBase<DF_NG_2755_MSG12001_SubmitDeclaration, GenericRequestParams>
    {
        private ICustomContext dbContext;
        public override void OnRequestFail(GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }

            base.OnRequestFail(requestParams);
        }
        public override void ManipulateRequestParams(GenericRequestParams requestParams)
        {

            this.dbContext = CustomContext.GetContext(requestParams.Tenant);
            var DeclarationPaymentQueryService = new DeclarationPaymentQueryService(this.dbContext);
            var declarationPaymentsPM = DeclarationPaymentQueryService.GetSingle(requestParams.AppicationId, true, false);
            if (requestParams.RequestVIA == SendRequestVIA.Default)
            {
                requestParams.RequestVIA = DefaultMessageController.Via(requestParams.Tenant, requestParams.MainInterfaceCode, requestParams.RequestVIA);
            }

            var srverTime = DateTime.Now;
            if (
                declarationPaymentsPM.FuturePaymentDateTime > srverTime &&
                declarationPaymentsPM.FuturePaymentDateTime.GetValueOrDefault().Subtract(srverTime) > TimeSpan.FromMinutes(1)
                )
            {

                switch (requestParams.RequestVIA)
                {
                    case SendRequestVIA.WebServiceInteractive:
                        requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                        break;
                    case SendRequestVIA.WebServiceBatch:
                        break;
                    case SendRequestVIA.DCABatch:
                        break;
                    case SendRequestVIA.Default:
                    default:
                        throw new System.Exception("should not be SendRequestVIA.Default !!!!");
                        break;
                }
                requestParams.RequestVIAChangeDue = string.Concat("נרשמה בקשה מתוזמנת לתאריך ", declarationPaymentsPM.FuturePaymentDateTime.GetValueOrDefault().ToShortDateString(), " שעה ", declarationPaymentsPM.FuturePaymentDateTime.GetValueOrDefault().ToShortTimeString());// "הבקשה תשלח בעתיד";
                requestParams.FutureSendDateTime = declarationPaymentsPM.FuturePaymentDateTime;
            }
            if (!requestParams.FutureSendDateTime.HasValue)
            {
                switch (requestParams.RequestVIA)
                {
                    case SendRequestVIA.WebServiceInteractive:
                        var myDF_MSG10000_ImportDeclarationRequestService = new DF_MSG10000_ImportDeclarationRequestService();
                        myDF_MSG10000_ImportDeclarationRequestService.ManipulateRequestParams(requestParams);
                        break;
                    case SendRequestVIA.WebServiceBatch:
                    case SendRequestVIA.DCABatch:
                        break;
                    case SendRequestVIA.Default:
                    default:
                        throw new System.Exception("should not be SendRequestVIA.Default !!!!");
                        break;
                }
            }

            base.ManipulateRequestParams(requestParams);
        }

        public override DF_NG_2755_MSG12001_SubmitDeclaration GetRequest(GenericRequestParams requestParams)
        {            
            UnifreightIIG.Common.SubmitExportDeclarationRequestServiceReference.DF_NG_2755_MSG12001_SubmitDeclaration dF_NG_2755_MSG12001_SubmitDeclaration =  new DF_NG_2755_MSG12001_SubmitExportDeclarationRequestService().GetRequest(requestParams);
            DF_NG_2755_MSG12001_SubmitDeclaration thisDF_NG_2755_MSG12001_SubmitDeclaration =
                Serializer.CastXML<DF_NG_2755_MSG12001_SubmitDeclaration, UnifreightIIG.Common.SubmitExportDeclarationRequestServiceReference.DF_NG_2755_MSG12001_SubmitDeclaration>(dF_NG_2755_MSG12001_SubmitDeclaration);

            return thisDF_NG_2755_MSG12001_SubmitDeclaration;
        }

        public override void PostGetRequest(DF_NG_2755_MSG12001_SubmitDeclaration customRequest, GenericRequestParams requestParams)
        {
            this.dbContext = CustomContext.GetContext(requestParams.Tenant);
            var DeclarationPaymentQueryService = new DeclarationPaymentQueryService(this.dbContext);
            var declarationPaymentsPM = DeclarationPaymentQueryService.GetSingle(requestParams.AppicationId, true, false);
            if (declarationPaymentsPM != null && !string.IsNullOrWhiteSpace(declarationPaymentsPM.DeclarationId))
            {
                var declarationQueryService = new DeclarationQueryService(this.dbContext);
                var declarationPM = declarationQueryService.GetSingle(declarationPaymentsPM.DeclarationId, false, false);
                if (declarationPM != null && declarationPM.IsCourierDeclaration)
                {
                    DeclarationCourierStatusUpdateService declarationCourierStatusUpdateService = new DeclarationCourierStatusUpdateService(this.dbContext, new Dictionary<string, IContext>(), requestParams.Tenant);
                    DeclarationCourierStatusQueryService declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(this.dbContext);
                    DeclarationCourierStatusPM currentDeclarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationPM.Id, true, false);
                    if (currentDeclarationCourierStatusPM == null)
                    {
                        currentDeclarationCourierStatusPM = new DeclarationCourierStatusPM()
                        {
                            DeclarationId = declarationPM.Id,
                            Tenant = declarationPM.Tenant,
                            IsClosedForFollowUp = false,
                            IsCourierMissingClassification = false,
                        };
                        currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Insert;
                    }
                    else
                    {
                        currentDeclarationCourierStatusPM.ChangeSetOp = ChangeSetOperation.Update;
                    }
                    currentDeclarationCourierStatusPM.CourierPaymentStatusCode = "I";
                    declarationCourierStatusUpdateService.Update(currentDeclarationCourierStatusPM, true);
                }
            }
        }
    }
}

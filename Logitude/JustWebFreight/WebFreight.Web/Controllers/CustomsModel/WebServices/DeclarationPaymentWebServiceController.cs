
using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.Customs.BL.BL;
using Logitude.Customs.BL.DataContracts;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.Server.Tools;
using NPOI.SS.Formula.Functions;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class DeclarationPaymentWebServiceController : ApiController
    {

    
        public HttpResponseMessage PostResponseCheckFileCredit([FromBody] string  response)
		{
			try
			{
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
				int tenant = authToken.Tenant;
				ICustomContext dbContext = CustomContext.GetContext(tenant);

				if (String.IsNullOrWhiteSpace(response))
				{
					throw new Exception("ImmediatelyResponse is null");
				}
				var GenericResponse = XmlGenericUtil<GenericResponse>.DeSerializeObject(response);
				var genericResponseObj = GenericResponse.GenericResponseObj.FirstOrDefault();
				if (genericResponseObj == null)
				{
					throw new Exception("GenericResponse.GenericResponseObj is null");
				}

				if (genericResponseObj.ResponseXml == null && genericResponseObj.ResponseXml == "")
				{
					throw new Exception("genericResponseObj.ResponseXml is null");
				}

				var creditResponseData = XmlGenericUtil<CUSTOMCREDIT_UL>.DeSerializeObject(genericResponseObj.ResponseXml);
				var comunicationLog = Communications.GetCommunicationLog(tenant, genericResponseObj.CorrelationId);
				if(comunicationLog == null || string.IsNullOrEmpty(comunicationLog.AdditionalFields))
				{
					throw new Exception("comunicationLog is null or comunicationLog.AdditionalFields is null");
				}
				var checkFileCrediteReq = JsonConvert.DeserializeObject<CheckFileCrediteReq>(comunicationLog.AdditionalFields);

				bool isCheckFileCredit = true;
				if (!string.IsNullOrEmpty(creditResponseData.CustomFileCredit[0].ErrorMessage))
				{
					isCheckFileCredit = false;
				}

				var declarationQueryService = new DeclarationQueryService(dbContext);
				var myDeclarationPaymentQueryService = new DeclarationPaymentQueryService(dbContext);
				var declarationPM = declarationQueryService.GetSingle(checkFileCrediteReq.AppicationId, true, false);
				var declarationPaymentPM = myDeclarationPaymentQueryService.GetSingle(declarationPM.Id, true, false);

				switch (checkFileCrediteReq.ClassName)
				{
					case "AutoPaymentService":
						AutoPaymentService autoPaymentService = new AutoPaymentService(declarationPM);
						autoPaymentService.SendPaymentIsCheckFileCredit(isCheckFileCredit, declarationPM, declarationPaymentPM, checkFileCrediteReq.LoggingUserId, true);
						break;
					case "DF_NG_8251_Web02_DeclarationStatus_ResponseService":
						DF_NG_8251_Web02_DeclarationStatus_ResponseService dF_NG_8251_Web02_DeclarationStatus_ResponseService = new DF_NG_8251_Web02_DeclarationStatus_ResponseService();
						dF_NG_8251_Web02_DeclarationStatus_ResponseService.SendPaymentIsCheckFileCredit(isCheckFileCredit, declarationPM, declarationPaymentPM, dbContext, checkFileCrediteReq.LoggingUserId, checkFileCrediteReq.LoggingObjectTableId, checkFileCrediteReq.LoggingEntityReference,true);
						break;
					case "MN_NG_8241_CargoResponseService":
						MN_NG_8241_CargoResponseService mN_NG_8241_CargoResponseService = new MN_NG_8241_CargoResponseService();
						mN_NG_8241_CargoResponseService.SendPaymentIsCheckFileCredit(isCheckFileCredit, declarationPM, declarationPaymentPM, dbContext, checkFileCrediteReq.LoggingUserId, checkFileCrediteReq.LoggingObjectTableId, checkFileCrediteReq.LoggingEntityReference);
						break;
					case "DF_NG_2754_MSG10004_ImportDeclarationResponseService":
						DF_NG_2754_MSG10004_ImportDeclarationResponseService dF_NG_2754_MSG10004_ImportDeclarationResponseService = new DF_NG_2754_MSG10004_ImportDeclarationResponseService();
						dF_NG_2754_MSG10004_ImportDeclarationResponseService.SendPaymentIsCheckFileCredit(isCheckFileCredit, declarationPM, declarationPaymentPM, dbContext, checkFileCrediteReq.LoggingUserId, checkFileCrediteReq.LoggingObjectTableId);
						break;
				}
				Communications.UpdateCommunicationLogStatus(comunicationLog.Id, comunicationLog.Tenant, null, "D", response, null);


				return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

	
	}
}

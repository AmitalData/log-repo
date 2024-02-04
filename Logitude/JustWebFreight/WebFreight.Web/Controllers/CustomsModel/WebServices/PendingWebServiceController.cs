using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using WebFreight.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Controllers.CustomsModel.Extended;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using static WebFreight.Web.Controllers.CustomsModel.Extended.CourierMasterController;

namespace WebFreight.Web.Controllers.WebServices
{
    public class PendingWebServiceController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage DeclarationsforBulkFeed(string courierMasterId, string goodsDescription, string weightFrom, string weightTo, string incotermCode, string searchFilter, string totalInvoice, string fastIndividualProcess, int? skip = null, int? take = null, string sortingCol = null, string sortingDir = null)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                var declarations = new DeclarationRepository(tenant).GetforPendingBulkFeed(courierMasterId, goodsDescription, weightFrom, weightTo, incotermCode, searchFilter, totalInvoice, fastIndividualProcess, skip, take, sortingCol, sortingDir);
                //ServiceResponse response = new ServiceResponse();
                //response.Count = declarations.Count();
                //response.Result = declarations;
                //return Request.CreateResponse(HttpStatusCode.OK, response);
                return Request.CreateResponse(HttpStatusCode.OK, declarations);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        [HttpPost]
        public HttpResponseMessage BulkFeeding([FromBody] AddMultiPendingsRequestParams requestParamsData, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                QueryOperations queryOperations = CourierDeclarationPendingListExtendedController.CreateQueryOperations(filters, authToken.Tenant, "Customs.DeclarationCourierStatus");
                string res = new DCAInUCBUCADPE_MsgMessagingService().CreateCRS(authToken.Tenant, requestParamsData, queryOperations);
                

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpPost]
        public HttpResponseMessage PostSendMultiUpdate(MultiUpdateRequestParams requestParamsData, [FromUri] ApiQueryFilters filters)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                QueryOperations queryOperations = CourierDeclarationPendingListExtendedController.CreateQueryOperations(filters, authToken.Tenant, "Customs.DeclarationCourierStatus");

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new DCAInUCBMultiUpdate_MsgMessagingService();
                string RequestInProgressList;
                var sts = messagingService.CreateCRS(tenant, null, requestParamsData, out RequestInProgressList, queryOperations);
                DataResult result = new DataResult();
                result.RequestInProgressList = RequestInProgressList;
                result.Message = sts;
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private void TestPending(int tenant, AddMultiPendingsRequestParams requestParamsData, QueryOperations queryOperations)
        {
            var customResponse = new DCAInUCBUCADPEResponseContentHeader()
            {
                tenant = tenant,
                requestParamsData = requestParamsData,
                queryOperations = queryOperations,
            };

            new UniCourierBatchSendUCADPE_MsgResponseService().UpdateDeclarationPendings(customResponse);
        }
    }
}
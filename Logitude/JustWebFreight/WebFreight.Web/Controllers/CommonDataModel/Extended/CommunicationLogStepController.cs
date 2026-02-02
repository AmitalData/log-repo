//This controller is not generated, you can add your methods there.
//All controllers under App_Code  should be moved to the Controllers Folder, we moved some of it, but still there is many old ones there.

//Regards,
//Islam.

using Logitude.AmitalMessaging.Utils;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Xml;
using WebFreight.Web.CustomWebServices.BL.XLSExport;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class CommunicationLogStepController : ApiController
    {
        public HttpResponseMessage GetCommunicationLogStepsListsByLogId(string logId, int tenant)
        {
            Authentication(tenant);

            CommunicationLogStepQuery communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
            List<CommunicationLogStepList> myResult = communicationLogStepQuery.GetCommunicationLogStepListsByLogId(logId, tenant);

            return Request.CreateResponse(HttpStatusCode.OK, myResult);
        }

        public HttpResponseMessage GetExportExcelByLogId(
            string mainInterfaceCode, string logId, int tenant)
        {

            Authentication(tenant);
            var myXLSExportService = new XLSExportService();

            var result =
            myXLSExportService
            //.Start("8347","1-1370596", 1);
            .Start(mainInterfaceCode, logId, tenant, new GuaranteeRequestProvider());


            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            response.Content = new StreamContent(new MemoryStream(result));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName =
                mainInterfaceCode + "_" + logId + ".xls";
            return response;
        }

        public HttpResponseMessage GetExportExcelByRequestId(
            string mainInterfaceCode, string requestId, int tenant)
        {
            Authentication(tenant);
            try
            {
                var qs = new CustomsRequestsSheetQueryService(tenant);
                var crsPM = qs.GetSingle(requestId, false, false);
                return GetExportExcelByLogId(
                mainInterfaceCode, crsPM.RequestComminicationId, tenant);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private static void Authentication(int tenant)
        {
            string token = HttpContext.Current?.Request?.Headers["Token"];
            if (token != null)
            {
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
            }
        }


        public HttpResponseMessage GetCommunicationLogStepsRequestParamResponseData(string mainInterfaceCode, string logId, int tenant)
        {
            try
            {
                Authentication(tenant);



                var stepFilter = new CustomsStepEnum[] { CustomsStepEnum.StartRequestParams, CustomsStepEnum.AnalyzeResponseData };
                var myFilter = new int[] { 0, 30 };
                List<CommunicationLogStepList> myResult = GetCommunicationLogStepsDocumentData(mainInterfaceCode, logId, tenant, myFilter, false);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public List<CommunicationLogStepList> GetCommunicationLogStepsDocumentDataBystringStepFilter(string mainInterfaceCode, string communicationLogId, int tenant, string stringStepFilter, bool suppressHugeData)
        {
            var list = stringStepFilter.Split(',').Select(r => int.Parse(r)).ToArray();
            return GetCommunicationLogStepsDocumentData(mainInterfaceCode, communicationLogId, tenant, list, suppressHugeData);

        }







        /// <summary>


        public HttpResponseMessage GetRequestComminicationIdByEntityId2(int tenant, string InterfaceTypeCode, string RequestStatusCode, string ObjectTableId2, string EntityId2)
        {
            try
            {
                Authentication(tenant);



                var listService = new Logitude.Customs.BL.EntityQueryServices.CustomsRequestsSheetQueryService(tenant);
                var CustomsRequestsSheet = listService.GetByEntityId2(tenant, InterfaceTypeCode, RequestStatusCode, ObjectTableId2, EntityId2);
                //if (CustomsRequestsSheet == null)
                //{
                //    return null;
                //}
                //var logid = CustomsRequestsSheet.RequestComminicationId;

                return Request.CreateResponse(HttpStatusCode.OK, CustomsRequestsSheet);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        /// <returns></returns>



        public List<CommunicationLogStepList> GetCommunicationLogStepsDocumentData(string mainInterfaceCode, string communicationLogId, int tenant, int[] stepFilter, bool suppressHugeData)
        {
            var communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
            IMessagingServiceInterfaceType messagingService = null;
            if (!String.IsNullOrWhiteSpace(mainInterfaceCode))
            {
                var swSvc = Stopwatch.StartNew();
                messagingService = MessagingServiceFactoryHelper.GetMessagingService(mainInterfaceCode);
                swSvc.Stop();
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[CommLog] GetMessagingService({mainInterfaceCode}) took {swSvc.ElapsedMilliseconds} ms. logId={communicationLogId}");
            }
            var swQuery = Stopwatch.StartNew();
            List<CommunicationLogStepList> stepLIstOut = communicationLogStepQuery.GetCommunicationLogStepsDocumentData(communicationLogId, tenant, stepFilter,false,false);
            swQuery.Stop();
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[CommLog] Query.GetCommunicationLogStepsDocumentData took {swQuery.ElapsedMilliseconds} ms. steps={(stepLIstOut?.Count ?? 0)} filter=[{string.Join(",", stepFilter ?? new int[0])}] logId={communicationLogId}");
            foreach (var item in stepLIstOut)
            {
                if (!String.IsNullOrWhiteSpace(item.DocumentData) && messagingService != null)
                {
                    int sizeOf200KB = 200000;
                    if (suppressHugeData && item.DocumentData != null && (item.DocumentData.Length * sizeof(Char) > sizeOf200KB))
                    {
                        item.DocumentData = "(item.DocumentData.Length * sizeof(Char) > sizeOf250KB)";
                    }
                    else
                    {
                        var swConv = Stopwatch.StartNew();
                        item.DocumentData = messagingService.ConvertStepDataToJSON(item.StepNumber, item.DocumentData);
                        swConv.Stop();
                        NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"[CommLog] ConvertStepDataToJSON step={item.StepNumber} took {swConv.ElapsedMilliseconds} ms. logId={communicationLogId}");
                    }
                }
            }

            return stepLIstOut;
        }

    }
}
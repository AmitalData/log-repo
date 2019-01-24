using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Customs.Def.ClosedTable;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Utils;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Configuration;
using Logitude.Customs.BL.Messaging.Customs;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CustomsRequestSheetExtendedController : ApiController
    {


        public HttpResponseMessage GetRequestInProgress(int Tenant,
            string InterfaceTypeCode,
            string ObjectTableId1, string EntityId1,
            string ObjectTableId2, string EntityId2,
            string CustomFileNo,
            bool displayOnlyMode)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                CustomsRequestsSheetQueryService customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
                List<CustomsRequestsSheetPM> requestSheets = customsRequestsSheetQuery.GetRequestInProgress(Tenant, InterfaceTypeCode, ObjectTableId1, EntityId1,
                    ObjectTableId2, EntityId2,
                    CustomFileNo, displayOnlyMode);
                var payRequest = requestSheets.FirstOrDefault(r => r.InterfaceTypeCode == "2755");
                if (payRequest != null)
                {
                    var communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
                    var stepList = communicationLogStepQuery
                        .GetCommunicationLogStepsDocumentData(payRequest.RequestComminicationId, tenant, new int[] { 0 }, true);
                    var xdoc = XDocument.Parse(stepList.First().DocumentData);
                    var FutureSendDateTime = xdoc.Descendants("FutureSendDateTime").FirstOrDefault();
                    if (FutureSendDateTime != null)
                    {
                        try
                        {
                            payRequest.FutureSendDateTime = XmlConvert.ToDateTime(FutureSendDateTime.Value);
                        }
                        catch (Exception e)
                        {
                            try
                            {
                                //  <FutureSendDateTime xsi:nil="true" />Logger.LogMe("FutureSendDateTime : " + stepList.First().DocumentData, true);
                            }
                            catch (Exception)
                            {

                                ///throw;
                            }
                                                         
                        }
                        
                    }
                }
                //From Declaration EditComponent Return //Fast as posibble 
                if (InterfaceTypeCode.Trim() == "2750" && !string.IsNullOrWhiteSpace(CustomFileNo))
                {
                    requestSheets = requestSheets ?? new List<CustomsRequestsSheetPM>();
                    var firstReq = requestSheets.FirstOrDefault();
                    //var repo = new DeclarationRepository(customContext); // stopped the concurrency code and returned only a dummy invoice 4 me 
                    //var ConcurrencyGUID = repo.GetConcurrencyGUIDByCustomFileNo(CustomFileNo, tenant);
                    //if (!String.IsNullOrWhiteSpace(ConcurrencyGUID))
                    //{
                        if (firstReq == null)
                        {
                            firstReq = new CustomsRequestsSheetPM();// DUMMY 4 MOHAMMAD 
                            requestSheets.Add(firstReq);
                        }
                        //firstReq.MainEntityConcurrencyGUID = ConcurrencyGUID;
                    //}
                }



                

                return Request.CreateResponse(HttpStatusCode.OK, requestSheets);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetAnyRequest(int Tenant,
            string InterfaceTypeCode,
            string CustomFileNo
            )
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                CustomsRequestsSheetQueryService customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
                List<CustomsRequestsSheetPM> requestSheets = customsRequestsSheetQuery.GetCustomsRequestsSheetByCustomFileNumberPM(CustomFileNo, Tenant);
                requestSheets  =requestSheets.Where(r => r.InterfaceTypeCode == InterfaceTypeCode).ToList();





                return Request.CreateResponse(HttpStatusCode.OK, requestSheets);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetRequestByInterfaceTypeCode(int Tenant,
            string InterfaceTypeCode,
            string ObjectTableId1, string EntityId1,
            string CustomFileNo)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                CustomsRequestsSheetQueryService customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
                List<CustomsRequestsSheetPM> requestSheets = customsRequestsSheetQuery.GetRequestByInterfaceTypeCode(Tenant, InterfaceTypeCode, 
                    ObjectTableId1, EntityId1,
                    CustomFileNo);
                return Request.CreateResponse(HttpStatusCode.OK, requestSheets);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSetCustomsRequestSheetStatus(CustomsRequestsSheetPM dummyPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                string mess = null;
                try
                {

                    

                    //string key = ProcessLockTableUtil.Instance.GetKey4InProggressCustomsRequestsSheet(dummyPM.Id);
                    //using (var disposableToken = ProcessLockTableUtil.Instance.LockItAndGetReleaseToken(key, "Try Cancell"))
                    {
                        {
                            using (var scope = TransactionFactory.GetTransaction())
                            {
                                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                                var customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);

                                CustomsRequestsSheetPM currententityPm = customsRequestsSheetQuery.GetSingle(dummyPM.Id, false, false);

                                bool tryConcurrentKiller = true;// ConfigurationManager.AppSettings["20180718.ConcurrentKiller"] == "1";
                                if (tryConcurrentKiller)
                                {
                                    if (CustomsRequestsSheetQueryService.GetintrefaceTypeListDisplayOnly().ToList().Contains(currententityPm.InterfaceTypeCode))
                                    {
                                        if (DateTime.Now.Subtract(currententityPm.RequestCreateDate.GetValueOrDefault()) < TimeSpan.FromMinutes(10)) //CALL#321639         
                                        {
                                            string CRSKey = CustomsRequestsSheetDomainModelUtil.GetCRSKey(currententityPm.Id);
                                            using (var scope1 = TransactionFactory.GetNewTransaction())
                                            {
                                                var concurrentKiller = new ConcurrentKiller();
                                                concurrentKiller.LockOrCrashOnCommitDueUnique(CRSKey, currententityPm.Tenant);
                                                scope1.Complete();
                                            }
                                        }
                                    }
                                }

                                ///CommLogStepCanCancell(currententityPm);
                                currententityPm.RequestStatusCode = dummyPM.RequestStatusCode;
                                var us = new CustomsRequestsSheetUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
                                us.CommLogStepCanCancelledAction = CommLogStepCanCancelled;
                                currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                                us.Update(currententityPm, true);
                                mess = null;
                                scope.Complete();
                            }

                        }
                    }
                }
                catch   (ProcessLockException myProcessLockException)
                {
                    mess = "המסר באמצע שליחה ";
                }
                catch (Exception e)
                {
                    mess = e.Message;
                }


                return Request.CreateResponse(HttpStatusCode.OK, mess);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public static bool Isinteractive(int tenant, string RequestComminicationId)
        {
            SendRequestVIA? curSendRequestVIA = null;
            var communicationLogStepQuery = new CommunicationLogStepQuery();
            var requestParamXml = communicationLogStepQuery.GetStartRequestParams(tenant, RequestComminicationId);
            if (!string.IsNullOrWhiteSpace(requestParamXml))
            {
                var xdoc = XDocument.Parse(requestParamXml);
                var eleRequestVIA = xdoc.Descendants("RequestVIA").FirstOrDefault();
                var eleRequestVIAValue = eleRequestVIA.Value;
                if (!String.IsNullOrWhiteSpace(eleRequestVIAValue))
                {
                    SendRequestVIA SendRequestVIA;
                    if (Enum.TryParse<SendRequestVIA>(eleRequestVIAValue, out SendRequestVIA))
                    {
                        curSendRequestVIA = SendRequestVIA;
                    }
                    //var RequestVIAChangeDueEle = xdoc.Descendants("RequestVIAChangeDue").FirstOrDefault();
                    //if (RequestVIAChangeDueEle != null)
                    //{
                    //    RequestVIAChangeDue = RequestVIAChangeDueEle.Value;
                    //}
                }
            }
            if (!curSendRequestVIA.HasValue)
            {
                return true;//default 
            }
            switch (curSendRequestVIA.Value)
            {
                case SendRequestVIA.WebServiceInteractive:
                    return true;//default 
                    break;
                case SendRequestVIA.Default:
                case SendRequestVIA.WebServiceBatch:
                case SendRequestVIA.DCABatch:
                default:
                    return false;//default 
                    break;
            }
        }


         
        public static void CommLogStepCanCancelled(CustomsRequestsSheet entityPOCO, 
            CustomsRequestsSheetPM entityPM,DateTime? nowIs
            )
        {
            if (entityPOCO.RequestStatusCode == "30")
            {
                throw new Exception("Request already analyzed");
            }
            nowIs = nowIs ?? DateTime.Now;
            bool isinteractive = false;
            var communicationLogStepQuery = new CommunicationLogStepQuery(entityPM.Tenant);
            var stepList = communicationLogStepQuery
                .GetCommunicationLogStepListsByLogId(entityPM.RequestComminicationId, entityPM.Tenant);
            DateTime? canCancellAtTime = null;

            if (!entityPM.IsDCA)
            {
                var stepReq = stepList.FirstOrDefault(rec => rec.StepNumber == (int)CustomsStepEnum.StartRequestParams);
                if (stepReq != null)
                {
                    var requestParamXml = stepReq.DocumentData;
                    isinteractive = Isinteractive(entityPM.Tenant, entityPM.RequestComminicationId);
                    if (isinteractive)
                    {
                        var startAt = entityPM.RequestCreateDate.GetValueOrDefault();
                        if (entityPM.FutureSendDateTime.HasValue)
                        {
                            if (entityPM.FutureSendDateTime.Value > entityPM.RequestCreateDate)
                            {
                                startAt = entityPM.FutureSendDateTime.Value;
                            }
                        }
                        if (nowIs.GetValueOrDefault().Subtract(startAt) > TimeSpan.FromMinutes(10))
                        {
                            return;//can cancell
                        }
                        canCancellAtTime = startAt.AddMinutes(10);
                    }
                }
            }
            if (!ResponseDataBase.RequestSheetCanCancelled(entityPM.RequestStatusCode, entityPM.IsDCA))
            {
                if (!isinteractive)//whill exec later !!
                {
                    ///throw new Exception("Unable to cancel request. It has already been sent (Batch proccess)");
                }

                var currStep = stepList.OrderBy(r => r.StepNumber).FirstOrDefault(r => r.Status != CommStatusEnum.D.ToString());
                if (currStep != null)
                {
                    if (currStep.StepNumber == (int)CustomsStepEnum.ReceivedCustomResponseCorrelation)
                    {
                        if (currStep.Status != CommStatusEnum.W.ToString())
                        {
                            if (canCancellAtTime != null)
                            {
                                throw new Exception(GetMessage(canCancellAtTime));
                            }
                            throw new Exception("Unable to cancel request. It has already been sent");
                        }
                    }
                    else if (currStep.StepNumber > (int)CustomsStepEnum.ReceivedCustomResponseCorrelation)
                    {
                        if (canCancellAtTime != null)
                        {
                            throw new Exception(GetMessage(canCancellAtTime));
                        }
                        throw new Exception("Unable to cancel request. It has already been sent");

                    }
                }
            }
        }

        private static string GetMessage(DateTime? canCancellMoreTime)
        {
            return $"  נשלח בתהליך אינטראקטיבי יתאפשר ביטול החל מהשעה {canCancellMoreTime.GetValueOrDefault().ToShortTimeString()}";
        }

        public HttpResponseMessage PostCustomsRequestSheetReQueue(CustomsRequestsSheetPM dummyPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                string mess = null;
                try
                {

                    MessagingServiceFactoryHelper.ResolveAndReQueue(dummyPM.InterfaceTypeCode, dummyPM.Tenant, dummyPM.Id);
                    mess = null;
                }
                catch (Exception e)
                {
                    mess = e.Message;
                }


                return Request.CreateResponse(HttpStatusCode.OK, mess);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetGeneralRequestInProgress(int Tenant,
            string InterfaceTypeCode,
            string ObjectTableId1, string EntityId1,
            string ObjectTableId2, string EntityId2,
            string CustomFileNo)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                CustomsRequestsSheetQueryService customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
                List<CustomsRequestsSheetPM> requestSheets = customsRequestsSheetQuery.GetGeneralRequestInProgress(Tenant, InterfaceTypeCode, ObjectTableId1, EntityId1, ObjectTableId2, EntityId2, CustomFileNo);

                return Request.CreateResponse(HttpStatusCode.OK, requestSheets);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}
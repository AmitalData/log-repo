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
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using Logitude.Server.Tools.Helpers;
using WebFreight.Web.DataContracts;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using System.Web.Script.Serialization;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Reflection;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Transactions;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CustomsRequestSheetExtendedController : ApiController
    {


        public HttpResponseMessage GetRequestInProgress(int Tenant,
            string InterfaceTypeCode,
            string ObjectTableId1, string EntityId1,
            string ObjectTableId2, string EntityId2,
            string CustomFileNo,
            bool displayOnlyMode,bool isWorkSheetFromExcel = false,string userId = null)
        {
            try
            {
                InterfaceTypeCode = InterfaceTypeCode.Trim();//why " 2750"
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                Boolean include8250IsShaam = false; 
                if (InterfaceTypeCode == "2750" && !string.IsNullOrWhiteSpace( CustomFileNo ))
                {
                    var decQS = new DeclarationQueryService(customContext);

                    var declaration =decQS.GetSingleByCustomFileNoFromCache(CustomFileNo, authToken.Tenant);
                    if (declaration!=null)
                    {
                        include8250IsShaam = declaration.ProcedureCurrentCode == "4070001"; //"ProcedureCurrentCode":"4070001","ProcedureCurrentName":"יבוא מסחרי-שח\"מ
                    }
                    

                }
                CustomsRequestsSheetQueryService customsRequestsSheetQuery = new CustomsRequestsSheetQueryService(customContext);
                //List<CustomsRequestsSheetPM> requestSheets = customsRequestsSheetQuery.GetRequestInProgress(Tenant, InterfaceTypeCode, ObjectTableId1, EntityId1,
                //    ObjectTableId2, EntityId2,
                //    CustomFileNo, displayOnlyMode);

                List<CustomsRequestsSheetPM> requestSheets = customsRequestsSheetQuery.GetRequestInProgress(new RequestInProgressParams()
                {
                    Tenant = Tenant,
                    InterfaceTypeCode = InterfaceTypeCode,
                    ObjectTableId1 = ObjectTableId1,
                    EntityId1 = EntityId1,
                    ObjectTableId2 = ObjectTableId2,
                    EntityId2 = EntityId2,
                    CustomFileNo = CustomFileNo,
                    DisplayOnlyMode = displayOnlyMode,
                    Include8250IsShaam= include8250IsShaam,
                    IsWorkSheetFromExcel= isWorkSheetFromExcel,
                    UserId=userId,
                });
                var payRequest = requestSheets.FirstOrDefault(r => r.InterfaceTypeCode == "2755");
                if (payRequest != null)
                {
                    var communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
                    var stepList = communicationLogStepQuery
                        .GetCommunicationLogStepsDocumentData(payRequest.RequestComminicationId, tenant, new int[] { 0 }, true,false);
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
                requestSheets  =requestSheets.Where(r => r.InterfaceTypeCode == InterfaceTypeCode && r.RequestStatusCode !="99").ToList();





                return Request.CreateResponse(HttpStatusCode.OK, requestSheets);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage CancelByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                int tenant = authToken.Tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Customs.CustomsRequestsSheet",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Customs.CustomsRequestsSheets",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };


                List<ObjectField> CustomsRequestsSheetObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CustomsRequestsSheet", tenant);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();
                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
                        //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
                        //{
                        //string[] values = filterValue1.ToString().Split(',');
                        //if (values.Count() > 1)
                        //{
                        //filterValue1 = values[0];
                        //filterValue2 = values[1];
                        //}
                        //}
                        //ToDo: Get object field by name and set the remained filter properties
                        ObjectField field = CustomsRequestsSheetObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            //queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);

                        }
                        else
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }



                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = CustomsRequestsSheetObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {


                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            //queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);

                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                ICustomContext MyContext = CustomContext.GetContext(tenant);
                CustomsRequestsSheetListQueryService customsRequestsSheetQuery = new CustomsRequestsSheetListQueryService(MyContext);

                List<CustomsRequestsSheetList> entityLists = customsRequestsSheetQuery.GetList(queryOperations, tenant);


       
                if (entityLists.FirstOrDefault(x => x.RequestStatusCode != "15" && x.RequestStatusCode != "21") != null)
                {
                    throw new Exception("אין אפשרות לבטל בקשות בסטטוס ניתוח נכשל");

                }

                CustomsRequestsSheetUpdateService customsRequestsSheetUpdate = new CustomsRequestsSheetUpdateService(MyContext);


                customsRequestsSheetUpdate.CancelRequests(entityLists, tenant, MyContext);

                ServiceResponse response = new ServiceResponse();
                HttpResponseMessage reponseMessage;
                response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = customsRequestsSheetQuery.GetListCount(queryOperations, tenant);
                    response.Count = count;
                }

                response.Result = entityLists;
                  reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        [HttpGet]
        public HttpResponseMessage ReAnalysisByFilters([FromUri] ApiQueryFilters filters)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                int tenant = authToken.Tenant;

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "Customs.CustomsRequestsSheet",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "Customs.CustomsRequestsSheets",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };


                List<ObjectField> CustomsRequestsSheetObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.CustomsRequestsSheet", tenant);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();
                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";
                        //if (filterValue1 != null && filterValue1.GetType() == typeof(string))
                        //{
                        //string[] values = filterValue1.ToString().Split(',');
                        //if (values.Count() > 1)
                        //{
                        //filterValue1 = values[0];
                        //filterValue2 = values[1];
                        //}
                        //}
                        //ToDo: Get object field by name and set the remained filter properties
                        ObjectField field = CustomsRequestsSheetObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            //queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);

                        }
                        else
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                    }



                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = CustomsRequestsSheetObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {


                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            //queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList, field.IsCustom, field.DataTypeCode);

                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }

                ICustomContext MyContext = CustomContext.GetContext(tenant);
                CustomsRequestsSheetListQueryService customsRequestsSheetQuery = new CustomsRequestsSheetListQueryService(MyContext);

                List<CustomsRequestsSheetList> entityLists = customsRequestsSheetQuery.GetList(queryOperations, tenant);

                ServiceResponse response = new ServiceResponse();
                HttpResponseMessage reponseMessage;
                if (entityLists.FirstOrDefault(x => x.RequestStatusCode != "21" && x.RequestStatusCode != "25") != null)
                {
                    throw new Exception("אין אפשרות לנתח מחדש בקשות בסטטוס שליחה נכשלה");

                }


                var messagingService = new DCAInUCB9999ReAnAnalysis_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null, entityLists.Select(x=>  x.Id).ToList());




                  response = new ServiceResponse();
                if (filters.GetCount)
                {
                    int count = customsRequestsSheetQuery.GetListCount(queryOperations, tenant);
                    response.Count = count;
                }

                response.Result = entityLists;
                  reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
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
                                            var TS = DateTime.Now.Subtract(currententityPm.RequestCreateDate.GetValueOrDefault());
                                            try
                                            {

                                            
                                            string CRSKey = CustomsRequestsSheetDomainModelUtil.GetCRSKey(currententityPm.Id);
                                            using (var scope1 = TransactionFactory.GetNewTransaction())
                                            {
                                                var concurrentKiller = new ConcurrentKiller();
                                                concurrentKiller.LockOrCrashOnCommitDueUnique(CRSKey, currententityPm.Tenant);
                                                scope1.Complete();
                                            }
                                            }
                                            catch (Exception e)
                                            {
                                                var wait = 10 - TS.TotalMinutes;
                                                throw new Exception( $"  דקות {wait} נסה עוד",e);
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

                                if (CustomsRequestsSheetQueryService.GetintrefaceTypeListDisplayOnly().ToList().Contains(currententityPm.InterfaceTypeCode))
                                {
                                    var req = new CD_NG_8347_Web01_CurrencyRateSearchRequestParams()
                                    {
                                        InterfaceTypeCode = currententityPm.InterfaceTypeCode,
                                        LoggingObjectTableId = currententityPm.ObjectTableId1,
                                        LoggingEntityId = currententityPm.EntityId1,

                                    };
                                    CustomsRequestsSheetDomainModelUtil
                                        .ReleaseConcurrentVirtualKey(req);

                                }
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
            var communicationLogStepQuery = new CommunicationLogStepQuery(tenant);
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
                var beforeFiveHours= DateTime.Now.AddHours(-5);
                if (currStep != null  && entityPM.AnswerCreateDate > beforeFiveHours)
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

        public HttpResponseMessage GetGeneralRequestInProgressByEntity2(int Tenant,
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
                List<CustomsRequestsSheetPM> requestSheets = customsRequestsSheetQuery.GetGeneralRequestInProgressByEntity2(Tenant, InterfaceTypeCode, ObjectTableId1, EntityId1, ObjectTableId2, EntityId2, CustomFileNo);

                return Request.CreateResponse(HttpStatusCode.OK, requestSheets);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetRequestDescription(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;
                        ICustomContext MyContext = CustomContext.GetContext(tenant);
                        CustomsRequestsSheetRepository customsRequestsSheetRepository = new CustomsRequestsSheetRepository(MyContext);
                        string desc = customsRequestsSheetRepository.GetRequestDescription(id);
                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, desc);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
    }
}
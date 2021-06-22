using Logitude.AmitalMessaging.Customs.CustomFile;
//using Logitude.BL.Security;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WebFreight.Web.CustomModel;
using WebFreight.Web.CustomWebServices;
using WebFreight.Web.Helpers;
using WebFreight.Web.DataContracts;
using System.Linq.Expressions;
using System.Text;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{

    public class IIGGeneralMessagesController : ApiController
    {

        public IIGGeneralMessagesController()
        {

        }


        public HttpResponseMessage PostMorningMessages(MorningMessageRequestParams requestParams)
        {
            try
            {



                // use messageing service

                var service = new MM_Web01_MorningMessagesListMessagingService();
                var responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostCourierBOLRequest(CourierBOLQueryRequestParams requestParams)
        {
            try
            {


                var service = new MN_NG_9022_CourierBOLQueryMessagingService();
                var responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostMasterBOLRequest(MasterBOLQueryRequestParams requestParams)
        {
            try
            {
                var service = new MN_NG_9020_MasterBOLQueryMessagingService();
                var responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostCreditQueryRequest(CreditQueryRequestParams requestParams)
        {
            try
            {
                var service = new TSH_NG_8289_Web05_CreditQueryMessagingService();
                var responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostGoldCreditQueryRequest(CreditQueryRequestParams requestParams)
        {
            try
            {
                var service = new TSH_WEB8289_9060_RTGSInfoQueryMessagingService();
                var responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetClientProgressBarIndicatorCurrentStage(int tenant, string CustomsRequestsSheetId, bool BasicResponse)
        {
            var responseData = new ResultClientProgressBar();
            try
            {

                if (BasicResponse)
                {
                    responseData.responseDataXml = ClientProgressBarIndicatorService.GetClientProgressBarIndicatorCurrentStage(CustomsRequestsSheetId);
                }
                else
                {
                    var myServerStateWebService = new ServerStateWebService();
                    bool stopMeNow = false;
                    bool continueInBackground = false;
                    string responseDataXml = "";
                    myServerStateWebService.ClientIsAngular = true;
                    int progressStage = myServerStateWebService.CalcClientProgressBarIndicatorCurrentStage(tenant, CustomsRequestsSheetId,
                        out stopMeNow, out continueInBackground, out responseDataXml);
                    responseData.continueInBackground = continueInBackground;
                    responseData.responseDataXml = responseDataXml;
                    responseData.stopMeNow = stopMeNow;
                    responseData.ProgressStage = progressStage;

                }



                return Request.CreateResponse(HttpStatusCode.OK, responseData);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
            finally
            {
                //if (responseData.stopMeNow && !responseData.continueInBackground && string.IsNullOrWhiteSpace(responseData.responseDataXml))
                //{
                //    LogitudeSettings.HandleLogMe(CustomsRequestsSheetId, false, "ProgressIndicator", new DateTime(2018, 01, 14));
                //}
            }
        }

        public HttpResponseMessage PostExchangeRatesQuery(CD_NG_8347_Web01_CurrencyRateSearchRequestParams requestParams)
        {
            try
            {



                // use messageing service

                var service = new CD_NG_8347_Web01_CurrencyRateSearchParamMessagingService();
                var responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostCustomItemLegalDemandsQuery(CustomItemLegalDemandsRequestParams requestParams)
        {
            try
            {
                // use messageing service
                var service = new CB_NG_8316_CustomItemLegalDemandsMessagingService();
                var responseData = service.Send(requestParams);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostImporterDeclarationRequest(ImporterDeclarationRequestParams requestParams)
        {
            try
            {
                var service = new VE_8326_ImporterDeclarationMessagingService();
                var responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostSpecialActivityRequest(SpecialActivityRequestParams requestParams)
        {
            try
            {
                var service = new ST_NG_40_MSG7_SpecialActivityRequestMessagingService();
                var responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage PostUpdateClosedTables(SystemTableRequestParams systemTableRequestParams)
        {


            try
            {
                if (systemTableRequestParams.UpdateAllTables)
                {

                    LoadCustomClosedTables.UpdateAllClosedTables(systemTableRequestParams.Tenant, systemTableRequestParams);


                }
                else
                {
                    //var messagingService = new SYSTBL_NG_9000_MSG_SystemTableRequestMessageService();
                    //responseData = messagingService.Send(requestParams);
                    SYSTBL_NG_9000_MSG_SystemTableRequestMessageService.SendIt(systemTableRequestParams.TableId, systemTableRequestParams.Tenant);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "OK-(Not Crash)");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage PostFillNotExistedClosedTables(SystemTableRequestParams systemTableRequestParams)
        {

            try
            {
                var myMehesSystemTables = new SystemTables();

                var entitySystemTables = myMehesSystemTables.GetTableData(systemTableRequestParams.TableId, systemTableRequestParams.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, entitySystemTables);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

#if false


            object[] GetAllParamArray = { };

            CustomClosedTableList addedEntitiesList = new CustomClosedTableList() { ClosedTableData = new List<CustomClosedTableData>() };
            CustomClosedTableData data = null;
            foreach (var systemrecord in entitySystemTables)
            {
                bool recordExists = false;
                object existedRecord = null;

                foreach (object o in addedEntitiesList.ClosedTableData)
                {
                    Type objectType = o.GetType();
                    PropertyInfo codeInfo = objectType.GetProperty("Code");
                    string code = codeInfo.GetValue(o).ToString();
                    if (systemrecord.id == code)
                    {
                        recordExists = true;
                        break;
                    }
                }

                if (!recordExists)
                {
                    data = new CustomClosedTableData();
                    Type newPocoType = data.GetType();

                    PropertyInfo codeInfo = newPocoType.GetProperty("Code");
                    PropertyInfo localNameInfo = newPocoType.GetProperty("LocalName");
                    PropertyInfo searchFieldsInfo = newPocoType.GetProperty("SearchFields");

                    codeInfo.SetValue(data, systemrecord.id);
                    localNameInfo.SetValue(data, systemrecord.name);
                    searchFieldsInfo.SetValue(data, systemrecord.id + "," + systemrecord.name);


                    object[] addParams = { data };

                    addedEntitiesList.ClosedTableData.Add(data);

                }
                else
                {
                    if (existedRecord != null)
                    {
                        Type exitedRecordType = existedRecord.GetType();
                        PropertyInfo localNameInfo = exitedRecordType.GetProperty("LocalName");
                        PropertyInfo searchFieldsInfo = exitedRecordType.GetProperty("SearchFields");

                        localNameInfo.SetValue(existedRecord, systemrecord.name);
                        searchFieldsInfo.SetValue(existedRecord, systemrecord.id + "," + systemrecord.name);



                        object[] addParams = { existedRecord };

                    }

                }
            }


            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CustomClosedTableList));
            ser.Serialize(memstream, addedEntitiesList);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;

#endif

        }
        public HttpResponseMessage PostVehicleRequest(UpdateDeleteVehicleRequestParams vehicleParams)
        {


            try
            {
                INF_MSG_GenericResponseData responseData;
                var messageService = new VP_NG_2690_VehicleInMessagingService();
                responseData = messageService.Send(vehicleParams);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage PostCustomFileCredit(CustomFileCreditRequestParams requestParamsCredit)
        {

            try
            {

                CustomFileCreditResponseData responseData = new CustomFileCreditResponseData();
                DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsCredit.Tenant);
                DeclarationPM declarationPM = declarationQueryService.GetSingle(requestParamsCredit.AppicationId, false, false);
                if (declarationPM != null && declarationPM.IsConnectedToUnifreight)
                {
                    try
                    {
                        ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "שליחת בקשת העברה לגובה");
                        var myCustomFileCreditService = new CustomFileCreditService(requestParamsCredit);
                        CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit();
                        ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "ניתוח תשובה העברה לגובה");
                        responseData.CreditStatus = creditResponseData.CustomFileCredit[0].CreditStatus;
                        if (!string.IsNullOrEmpty(creditResponseData.CustomFileCredit[0].ErrorMessage))
                        {
                            responseData.UserMessage = creditResponseData.CustomFileCredit[0].ErrorMessage;
                            responseData.HasException = true;
                        }
                        responseData.PaymentDateTime = null;
                        responseData.BankCode = creditResponseData.CustomFileCredit[0].BankCode;
                        responseData.PaymentDate = creditResponseData.CustomFileCredit[0].PaymentDate;
                        if (!String.IsNullOrWhiteSpace(responseData.PaymentTime) && creditResponseData.CustomFileCredit[0].PaymentTime.Length >= 12)
                        {
                            responseData.PaymentTime = creditResponseData.CustomFileCredit[0].PaymentTime != null ? creditResponseData.CustomFileCredit[0].PaymentTime.Substring(8, 4) : null;
                        }
                        if (!String.IsNullOrWhiteSpace(responseData.PaymentDate))
                        {
                            responseData.PaymentDateTime = GetUnifreightFormatedDate(responseData.PaymentDate, responseData.PaymentTime, "").GetValueOrDefault();
                        }
                        responseData.BillingTaxAmount = creditResponseData.CustomFileCredit[0].BillingTaxAmount;
                        responseData.IsTRansGove = false;
                        responseData.Succeeded = true;
                    }
                    catch (Exception e)
                    {
                        responseData.CreditStatus = "0";
                        responseData.Succeeded = false;
                        responseData.HasException = true;
                        responseData.UserMessage = e.ToString();
                    }
                }
                else
                {
                    responseData.CreditStatus = "5";
                    responseData.IsTRansGove = false;
                }






                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public static DateTime? GetUnifreightFormatedDate(string txt, string time, string dtdField)
        {
            DateTime date;
            if (string.IsNullOrWhiteSpace(txt)) return null;

            //if (!string.IsNullOrWhiteSpace(time))
            //{
            //    txt = string.Concat(txt, time);
            //}
            if (DateTime.TryParseExact(txt, "yyyyMMddHHmm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date;
            }
            if (DateTime.TryParseExact(txt, "dd'.'MM'.'yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date;
            }
            if (DateTime.TryParseExact(txt, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date;
            }
            if (DateTime.TryParseExact(txt, "yyyyMMddHHmmffff", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date;
            }
            //throw new Exception("AmitalConvertUtil:GetShortDate:value=" + txt + " Field=" + dtdField);
            return null;
        }

        public HttpResponseMessage GetDefBankForCustomer(string customerCode, int tenant)
        {
            try
            {
                string bank = "";

                if (!string.IsNullOrWhiteSpace(customerCode))
                {


                    var declarationQS = new DeclarationQueryService(tenant);
                    bank = declarationQS.GetDefault("ISRAEL", "CIM_AGENT_BANK", "NON", customerCode, tenant);
                }

                return Request.CreateResponse(HttpStatusCode.OK, bank);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage PostChangingTimeRequestParams
            (CH_NG_191_MSG2_ChangingTimeRequestParams checkParams)
        {

            try
            {
                CH_NG_192_MSG3_ApproveChangeTimeResponseData responseData;

                var messagingService = new CH_NG_191_MSG2_ChangingTimeRequestMessagingService();
                responseData = messagingService.Send(checkParams);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage GetResetDeclarationNumber(string declarationId, int tenant)
        {

            try
            {
                var mess = DeclarationUpdateService.ResetDeclarationNumber(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, mess);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostCargoQueryRequestParams
           (CargoQueryRequestParams requestParams)
        {

            try
            {
                CargoQueryResponseData responseData;

                var messagingService = new MN_NG_8240_CargoQueryMessagingService();
                responseData = messagingService.Send(requestParams);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        public HttpResponseMessage PostMessageRestoreRequestParams
           (MessageRestoreRequestParams requestParams)
        {

            try
            {

                var service = new SYSTBL_NG_9010_MSG_MessageRestoreRequestMessagingService();
                var responseData = service.Send(requestParams);


                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        public HttpResponseMessage PostBlockListInWarehouseRequestParams
           (BlockListInWarehouseRequestParams requestParams)
        {

            try
            {

                var service = new ST_8330_BlockListInWarehouseFilterMessagingService();
                var responseData = service.Send(requestParams);

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage GetLOGISUPPACC(string declarationId, int InvoiceCounterKey, int LineNumber, int tenant)
        {


            try
            {

                var customContext = Logitude.Customs.Data.CustomContext.GetContext(tenant);
                var supplierInvioceItemCertificatQuery = new SupplierInvioceItemCertificatQueryService(customContext);

                var supplierInvoiceQuery = new
                    SupplierInvoiceQueryService(customContext);
                var supplierInvoicePM = supplierInvoiceQuery.GetSingle(declarationId, InvoiceCounterKey, false, false);
                var supplierInvoiceItemQuery = new
                    SupplierInvoiceItemQueryService(customContext);
                var mySelectedItem = supplierInvoiceItemQuery.GetSingle(declarationId, InvoiceCounterKey, LineNumber, false, false);
                List<SupplierInvioceItemCertificatPM> certificates = supplierInvioceItemCertificatQuery.GetSupplierInvioceItemCertificatesForSupplierInvoice(declarationId, InvoiceCounterKey, tenant);
                var certificatesList = certificates.Where(d => d.LineNumber == LineNumber).ToList(); ;
                var SISequenceNumeric = supplierInvoicePM.SequenceNumeric.GetValueOrDefault().ToString();
                var xml = LOGISUPPACC.GetXML(mySelectedItem, certificatesList, SISequenceNumeric);
                var myClass = new { MyXML = xml };
                return Request.CreateResponse(HttpStatusCode.OK, myClass);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostCustomsBookInRequestParams
       (CustomsBookInRequestParams requestParams)
        {

            try
            {

                var messageService = new CBC_NG_8361_MSG01_CustomsBookInMessagingService();
                var responseData = messageService.Send(requestParams);

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }


        public HttpResponseMessage PostDeficitFileFilterRequestParams
     (DeficitFileFilterRequestParams requestParams)
        {

            try
            {

                var messageService = new TPG_8304_DeficitFileFilterParamMessagingService();
                var responseData = messageService.Send(requestParams);

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage PostTPG_NG_8244_ClaimFileFilterRequestParams
   (TPG_NG_8244_ClaimFileFilterRequestParams requestParams)
        {

            try
            {

                var messageService = new TPG_NG_8244_ClaimFileFilterParamMessagingService();
                var responseData = messageService.Send(requestParams);

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }

        public HttpResponseMessage PostCargoSplitRequestParams
           (CargoSplitRequestParams requestParams)
        {

            try
            {
                INF_MSG_GenericResponseData responseData;

                var messagingService = new MN_MSG8370_CargoSplitMessagingService();
                responseData = messagingService.Send(requestParams);
                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        public HttpResponseMessage PostVirtualDeclarationCourierStatus
                   (AmitalLazyLoadEvent amitalLazyLoadEvent)
        {
            try
            {
               string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                var authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                int tenant = authToken.Tenant;

                ICustomContext MyContext = CustomContext.GetContext(tenant);
                DeclarationCourierStatusListQueryService declarationCourierStatusQuery = new DeclarationCourierStatusListQueryService(MyContext);
                var q =declarationCourierStatusQuery.GetVirtual(tenant);
                //var myLazyLoadEvent = amitalLazyLoadEvent.MyLazyLoadEvent as LazyLoadEvent;
                q=q.LazyFilters(amitalLazyLoadEvent , 
                    ()=> { return declarationCourierStatusQuery.GetVirtual(tenant); } 
                    );

                ServiceResponse response = new ServiceResponse();
                if (amitalLazyLoadEvent.GetCount)
                {
                    int count = q.Count();
                    response.Count = count;
                }

                amitalLazyLoadEvent.sortField = amitalLazyLoadEvent.sortField ?? "DeclarationId";
                q = q.LazyOrderBy(amitalLazyLoadEvent);
                
                q = q.LazySkipTake(amitalLazyLoadEvent);

                response.Result = q.ToList(); ;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        static Car[] _Cars = null;
        public HttpResponseMessage GetVirtualCar
           (bool GetCount, int first, int rows,  string sortField/*: "CreatAt"*/, int  sortOrder/*: 1*/)
        {
            int max = 105;  

            try
            {
                if (_Cars == null)
                {

                    var carService = new CarService();
                    var cars = new List<Car>();
                    ///if (first + i < 500)

                    for (int i = 0; i < max; i++)
                    {


                        cars.Add(carService.generateCar(first + i));

                    }
                    _Cars = cars.ToArray();


                }
                

                ServiceResponse response = new ServiceResponse();
                if (GetCount)
                {
                    int count = max;
                    response.Count = count;
                }
                var lazyLoadEvent = new AmitalLazyLoadEvent() {
                    first = first,
                    rows = rows,
                    sortField = sortField,
                    sortOrder = sortOrder
                };
                var q = _Cars.ToList().AsQueryable<Car>();
                if (!string.IsNullOrWhiteSpace(sortField)  && sortField!="undefined")
                {
                    //sortField
                    q=q.LazyOrderBy(lazyLoadEvent);


                }
                q = q.LazySkipTake(lazyLoadEvent);
                response.Result = q.ToArray();

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }


    }
    class ResultClientProgressBar
    {
        public bool stopMeNow { get; set; }
        public bool continueInBackground { get; set; }
        public string responseDataXml { get; set; }
        public int ProgressStage { get; set; }

    }


    class CarService
    {
        static string[] brands = { "Vapid", "Carson", "Kitano", "Dabver", "Ibex", "Morello", "Akira", "Titan", "Dover", "Norma" };
        static string[] colors = { "Black", "White", "Red", "Blue", "Silver", "Green", "Yellow" };

        public Car generateCar(int id)
        {
            return new Car()
            {
                vin =id.ToString(), //this.generateVin(),
                brand = this.generateBrand(id),
                color = this.generateColor(id),
                CreatAt = this.generateYear(id)
            };
        }


        string generateVin()
        {


            return RandomString(5);
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";//"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@!#$%^&*";//"ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        string generateBrand(int id)
        {
            return brands[id % 10];
            return brands[random.Next(10)];
        }

        string generateColor(int id)
        {
            return colors[id % 5];
            //return colors[random.Next(7)];
        }

        DateTime generateYear(int id)
        {
            return (DateTime.Now.Date.AddDays(-1*id));
            //return 2000 + random.Next(21);
        }
    }

    public class Car
    {
        public string vin { get; set; }
        public DateTime CreatAt { get; set; }
        public string brand { get; set; }

        public string color { get; set; }

        public int price { get; set; }

        public int saleDate { get; set; }

    }


    /// <summary>
    /// The <see cref="NSG.PrimeNG.LazyLoading"/> namespace contains a class
    /// used by lazy loading feature and filter features.
    /// 
    /// The lazy loading feature allows one to return a page of data
    /// and combined with the filtering and sorting features gives
    /// a rich feature of transferring large set of data efficiently.
    /// </summary>
    /// <example>
    /// A full example as follows:
    /// <code>
    /// string _jsonString =
    ///     "{\"first\":0,\"rows\":3," +
    ///     "\"sortOrder\":-1,\"sortField\":\"NoteTypeSortOrder\"," +
    ///     "\"filters\":{\"NoteTypeDesc\":{\"value\":\"SO\",\"matchMode\":\"StartsWith\"}}}";
    /// JavaScriptSerializer _js_slzr = new JavaScriptSerializer();
    /// LazyLoadEvent _loadEvent = (LazyLoadEvent)_js_slzr.Deserialize(_jsonString, typeof(LazyLoadEvent));
    /// List&lt;NoteType&gt; _rows = NoteTypes.AsQueryable()
    ///     .LazyOrderBy(_loadEvent)
    ///     .LazyFilters(_loadEvent)
    ///     .LazySkipTake(_loadEvent).ToList();
    /// </code>
    /// </example>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }
    //
    /// <summary>
    /// Set of static helper methods, meant to be used as extension methods.
    /// </summary>
    public static partial class Helpers
    {
        //
        /// <summary>
        /// Sort this IQueryable, with:
        ///  sortField and
        ///  sortOrder 1=ascending
        ///           -1=descending
        /// <example> 
        /// This sample shows how to call this method, where _incidentQuery
        /// is IQueryable of Incident:
        /// <code>
        ///   JavaScriptSerializer _jsSlzr = new JavaScriptSerializer();
        ///   _loadEvent = (LazyLoadEvent) _jsSlzr.Deserialize( jsonString, typeof(LazyLoadEvent) );
        ///   _incidentQuery = _incidentQuery.LazyOrderBy( _loadEvent );
        /// </code>
        /// </example>
        /// <note type="note">
        ///  'OrderBy' must be called before the method 'Skip'.
        /// </note>
        /// </summary>
        /// <typeparam name="T">Some class (database)</typeparam>
        /// <param name="qry">IQueryable query of T (above class)</param>
        /// <param name="lle">PrimeNG lazy loading event (LazyLoadEvent) structure</param>
        /// <returns>IQueryable query of T (with ascending or descending sort applied)</returns>
        public static IQueryable<T> LazyOrderBy<T>(
                this IQueryable<T> qry, AmitalLazyLoadEvent lle)
        {
            if (string.IsNullOrWhiteSpace(lle.sortField) || lle.sortField == "undefined")
            {
                return qry;
            }
                ParameterExpression _parm = Expression.Parameter(typeof(T));
            MemberExpression memberAccess = Expression.PropertyOrField(_parm, lle.sortField);
            LambdaExpression keySelector = Expression.Lambda(memberAccess, _parm);
            //
            MethodCallExpression orderBy = Expression.Call(
                typeof(Queryable),
                (lle.sortOrder == 1 ? "OrderBy" : "OrderByDescending"),
                new Type[] { typeof(T), memberAccess.Type },
                qry.Expression,
                Expression.Quote(keySelector));
            //
            return qry.Provider.CreateQuery<T>(orderBy);
        }
        //
        /// <summary>
        ///  Skip forward in the database and take n # of rows
        /// <example> 
        /// This sample shows how to call this method, where _incidentQuery
        /// is IQueryable of Incident:
        /// <code>
        ///   JavaScriptSerializer _jsSlzr = new JavaScriptSerializer();
        ///   _loadEvent = (LazyLoadEvent) _jsSlzr.Deserialize( jsonString, typeof(LazyLoadEvent) );
        ///   _incidentQuery = _incidentQuery.LazySkipTake( _loadEvent );
        /// </code>
        /// </example>
        /// <note type="note">
        ///  'OrderBy' must be called before the method 'Skip'.
        /// </note>
        /// </summary>
        /// <typeparam name="T">Some class (database)</typeparam>
        /// <param name="qry">IQueryable query of T (above class)</param>
        /// <param name="lle">PrimeNG lazy loading event (LazyLoadEvent) structure</param>
        /// <returns>IQueryable query of T (with skip/take applied)</returns>
        public static IQueryable<T> LazySkipTake<T>(
                this IQueryable<T> qry, AmitalLazyLoadEvent lle)
        {
            int first = (int)lle.first;
            int rows = (int)lle.rows;

            if (rows > 0)
            {
                qry = qry.Skip(first);
                qry = qry.Take(rows);
            }
            return qry;
        }
        //
        /// <summary>
        ///  Apply filter to an IQueryable from PrimeNG request.
        ///  Filter:
        ///   key of the dictionary is the field name,
        ///   object is value(s) and match mode
        /// <example> 
        /// This sample shows how to call this method, where _incidentQuery
        /// is IQueryable of Incident:
        /// <code>
        ///   JavaScriptSerializer _jsSlzr = new JavaScriptSerializer();
        ///   _loadEvent = (LazyLoadEvent) _jsSlzr.Deserialize( jsonString, typeof(LazyLoadEvent) );
        ///   _incidentQuery = _incidentQuery.LazyFilters( _loadEvent );
        /// </code>
        /// </example>
        /// </summary>
        /// <typeparam name="T">Some class (database)</typeparam>
        /// <param name="qry">IQueryable query of T (above class)</param>
        /// <param name="lle">PrimeNG lazy loading event (LazyLoadEvent) structure</param>
        /// <returns>IQueryable query of T (with where filters applied)</returns>
        public static IQueryable<T> LazyFilters<T>(
                this IQueryable<T> qry, AmitalLazyLoadEvent lle , Func<IQueryable<T>> getBasic)
        {
            if (lle.filters != null)
            {
                foreach (var filterField in lle.filters)
                {
                    PropertyInfo _propertyInfo = typeof(T).GetProperty(filterField.key);
                    Type _type = _propertyInfo.PropertyType;
                    IQueryable<T> qry1 = getBasic();
                    bool useQ = false;
                    Expression<Func<T, bool>> expressionPerField = null;
                    foreach (var item in filterField.value)
                    {



                        dynamic filterMetadata = (item as dynamic) ;
                        string matchMode = (string)filterMetadata.matchMode;
                        string @operator = (string)filterMetadata.@operator;

                        string filtervalue = filterMetadata.value.ToString();
                        if (!string.IsNullOrWhiteSpace(filtervalue))
                        {
                            useQ = true;
                            var whereClause1 = LazyDynamicFilterExpression<T>(
                                filterField.key, matchMode, filtervalue, _type
                                );
                            if (expressionPerField == null)
                            {
                                expressionPerField = whereClause1;
                            }
                            else
                            {
                                if (@operator == "or")
                                {

                                    expressionPerField = expressionPerField.Or(whereClause1);

                                }
                                else
                                {
                                    expressionPerField = expressionPerField.And(whereClause1);

                                }

                            }

                            //qry1 =qry1.Where(whereClause1);


                           

                        }

                       
                    }
                    if (useQ)
                    {
                        //qry = qry.Intersect(qry1);
                        qry = qry.Where(expressionPerField);
                    }

                    //Dictionary<string, Object> _value =
                    //        ((Dictionary<string, Object>)filterField.value);
                    //var whereClause = LazyDynamicFilterExpression<T>(filterField.key,
                    //        (string)_value["matchMode"], _value["value"].ToString(), _type);


                }
            }
            return qry;
        }
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {

            ParameterExpression p = a.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[b.Parameters[0]] = p;

            Expression body = Expression.AndAlso(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> a, Expression<Func<T, bool>> b)
        {

            ParameterExpression p = a.Parameters[0];

            SubstExpressionVisitor visitor = new SubstExpressionVisitor();
            visitor.subst[b.Parameters[0]] = p;

            Expression body = Expression.OrElse(a.Body, visitor.Visit(b.Body));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }

        //
        // PrimeNG:
        //  "contains", "startsWith", "endsWith", "equals", "notEquals", "in", "lt", "lte", "gt" and "gte".
        /// <summary>
        ///  A method to create an expression dynamically given a generic entity,
        ///  and a propertyName, operator and value.
        ///  <list type="bullet">
        ///   <listheader><description>Operators</description></listheader>
        ///   <item><description>contains</description></item>
        ///   <item><description>startsWith</description></item>
        ///   <item><description>endsWith</description></item>
        ///   <item><description>equals</description></item>
        ///   <item><description>notEquals</description></item>
        ///   <item><description>lt</description></item>
        ///   <item><description>lte</description></item>
        ///   <item><description>gt</description></item>
        ///   <item><description>gte</description></item>
        /// </list>
        ///  <note type="note">
        ///   The following code mostly come from:
        ///   https://stackoverflow.com/questions/2497303/how-to-specify-dynamic-field-names-in-a-linq-where-clause
        ///  </note>
        ///  <note type="note">
        ///   The 'in' operator is not handled and will throw an exception:
        ///   <exception cref="ArgumentOutOfRangeException">Unhandled or invalid operators</exception>
        ///  </note>
        /// </summary>
        /// <typeparam name="TEntity">
        ///  The class to create the expression for. Most commonly an entity framework
        ///  entity that is used for a DbSet.
        /// </typeparam>
        /// <param name="propertyName">A string value of the property name.</param>
        /// <param name="op">
        ///  A string representing an operator (see above list of operators).
        /// </param>
        /// <param name="value">A string representation of the value.</param>
        /// <param name="valueType">The underlying type of the value</param>
        /// <returns>
        ///  An expression that can be used for querying data sets
        ///  (Expression&lt;Func&lt;TEntity, bool&gt;&gt;)
        /// </returns>
        private static Expression<Func<TEntity, bool>>
            LazyDynamicFilterExpression<TEntity>(
                string propertyName, string op, string value, Type valueType)
        {
            Type type = typeof(TEntity);
            object asType = AsType(value, valueType);
            ParameterExpression p = Expression.Parameter(type, "x");
            MemberExpression member = Expression.Property(p, propertyName);
            string _stringValue = asType.ToString();
            ConstantExpression valueExpression = Expression.Constant(asType);
            //
            MethodInfo method;
            Expression q;
            //
            switch (op.ToLower())
            {
                case "gt":
                    q = Expression.GreaterThan(member, valueExpression);
                    break;
                case "lt":
                    q = Expression.LessThan(member, valueExpression);
                    break;
                case "equals":
                    q = Expression.Equal(member, valueExpression);
                    break;
                case "lte":
                    q = Expression.LessThanOrEqual(member, valueExpression);
                    break;
                case "gte":
                    q = Expression.GreaterThanOrEqual(member, valueExpression);
                    break;
                case "notequals":
                    q = Expression.NotEqual(member, valueExpression);
                    break;
                case "contains":
                    method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    q = Expression.Call(member, method ?? throw new InvalidOperationException(),
                        Expression.Constant(_stringValue, typeof(string)));
                    break;
                case "startswith":
                    method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                    q = Expression.Call(
                        member,
                        method ?? throw new InvalidOperationException(),
                        Expression.Constant(_stringValue, typeof(string)));
                    break;
                case "endswith":
                    method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    q = Expression.Call(member, method ?? throw new InvalidOperationException(),
                        Expression.Constant(_stringValue, typeof(string)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(op), $"filter matchMode of: '{op}', not gt/lt/equals/lte/gte/notequals/contains/startswith/endswith");
            }
            //
            return Expression.Lambda<Func<TEntity, bool>>(q, p);
        }
        //
        /// <summary>
        ///  Extract this string value as the passed in object type (convert/cast).
        /// </summary>
        /// <param name="value">The value, as a string</param>
        /// <param name="type">The desired type</param>
        /// <returns>The value, as the specified type</returns>
        private static object AsType(string value, Type type)
        {
            //TODO: This method needs to be expanded to include all appropriate use cases
            string v = value;
            if (value.StartsWith("'") && value.EndsWith("'"))
                v = value.Substring(1, value.Length - 2);
            if (value.StartsWith("\"") && value.EndsWith("\""))
                v = value.Substring(1, value.Length - 2);
            //
            if (type == typeof(string)) return v;
            if (type == typeof(DateTime)) return DateTime.Parse(v);
            if (type == typeof(DateTime?)) return DateTime.Parse(v);
            if (type == typeof(int)) return int.Parse(v);
            if (type == typeof(int?)) return int.Parse(v);
            if (type == typeof(long) || type == typeof(long?)) return long.Parse(v);
            if (type == typeof(short) || type == typeof(short?)) return short.Parse(v);
            if (type == typeof(byte) || type == typeof(byte?)) return byte.Parse(v);
            if (type == typeof(bool) || type == typeof(bool?)) return bool.Parse(v);
            //
            throw new ArgumentException("NSG.PrimeNG.LazyLoading.Helpers.AsType: " +
                "A filter was attempted for a field with value '" + value + "' and type '" +
                type + "' however this type is not currently supported");
        }
    }
    //
    /// <summary>
    /// PrimeNG structure, used by lazy loading feature.
    /// Class LazyLoadEvent ported from PrimeNG to this library.
    /// Generally, populated by the PrimeNG filter feature.
    /// <example>
    /// An example of the JSON:
    /// {"first":0,"rows":3,"sortOrder":1,
    ///   "filters":{"ServerId":{"value":1,"matchMode":"eq"},
    ///     "Mailed":{"value":"false","matchMode":"eq"},
    ///     "Closed":{"value":"false","matchMode":"eq"},
    ///     "Special":{"value":"false","matchMode":"eq"}},
    ///    "globalFilter":null}
    /// </example>
    /// </summary>
    //public class AmitalLazyLoadEvent
    //{
    //    public bool GetCount;
    //    public LazyLoadEvent MyLazyLoadEvent;

    //}
    public  class AmitaFilterMetadata
    {
        public string key;
        public Object[] value;

}
    public class FilterMetadata
    {
        public string @value;
        public string @matchMode;
        public string @operator;
    }

    public class AmitalLazyLoadEvent
    {
        public bool GetCount;
        // {"first":0,"rows":3,"sortOrder":1,
        // "filters":{"ServerId":{"value":1,"matchMode":"eq"},"Mailed":{"value":"false","matchMode":"eq"},"Closed":{"value":"false","matchMode":"eq"},"Special":{"value":"false","matchMode":"eq"}},
        // "globalFilter":null}
        /// <summary>
        /// First record #.
        /// </summary>
        public long first;
        /// <summary>
        /// # of rows to return (page size).
        /// </summary>
        public long rows;
        /// <summary>
        /// Sort field.
        /// </summary>
        public string sortField;
        /// <summary>
        /// Ascending or desending sort order.
        /// </summary>
        /// <value> 1 = asc, -1 = desc</value>
        public int sortOrder;
        /// <summary>
        /// multiSortMeta, not implemented.
        /// </summary>
        public object multiSortMeta;
        /// <summary>
        /// A dictionary of filters.
        /// Key of the dictionary is the field name, object is value(s)
        /// and match mode.
        /// </summary>
        /// <example>
        /// "filters":{"ServerId":{"value":1,"matchMode":"eq"},
        ///     "Mailed":{"value":"false","matchMode":"eq"},
        ///     "Closed":{"value":"false","matchMode":"eq"},
        ///     "Special":{"value":"false","matchMode":"eq"}},
        /// </example>
        public Dictionary<string, Dictionary<string, Object>> filters_old;
        public AmitaFilterMetadata[] filters;
        /// <summary>
        /// globalFilter, not implemented.
        /// </summary>
        public object globalFilter;
        //
        /// <summary>
        /// Returns a string that represents of the current object.
        /// This method overrides the default 'to string' method.
        /// </summary>
        /// <returns>
        /// A formatted string of the object's values.
        /// </returns>
        public override string ToString()
        {
            StringBuilder _return = new StringBuilder("record:[");
            _return.AppendFormat("first: {0}, rows: {1}, ", first, rows);
            _return.AppendFormat("sortField: {0}, sortOrder: {1}, ", sortField, sortOrder);
            _return.AppendFormat("multiSortMeta: {0}, ", multiSortMeta.ToString());
            _return.AppendFormat("filters: {0}, ", filters.ToString());
            _return.AppendFormat("globalFilter: {0}]", globalFilter.ToString());
            return _return.ToString();
        }
    }

    internal class SubstExpressionVisitor : System.Linq.Expressions.ExpressionVisitor
    {
        public Dictionary<Expression, Expression> subst = new Dictionary<Expression, Expression>();

        protected override Expression VisitParameter(ParameterExpression node)
        {
            Expression newValue;
            if (subst.TryGetValue(node, out newValue))
            {
                return newValue;
            }
            return node;
        }
    }
    public enum SortingEnumeration
    {
        OrderByAsc = 1,
        OrderByDesc = -1
    }

    public enum OperatorEnumeration
    {
        And = 1,
        Or = 2,
        None = 3
    }

    public static class OperatorConstant
    {
        private const string And = "and";
        private const string Or = "or";

        public static OperatorEnumeration ConvertOperatorEnumeration(string value)
        {
            switch (value.ToLower())
            {
                case And:
                    return OperatorEnumeration.And;
                case Or:
                    return OperatorEnumeration.Or;
                default:
                    return OperatorEnumeration.None;
            }
        }
    }
}
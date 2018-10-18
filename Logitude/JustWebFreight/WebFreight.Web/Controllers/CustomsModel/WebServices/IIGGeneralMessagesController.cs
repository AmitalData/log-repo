using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.BL.Security;
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
                        responseData.PaymentTime = creditResponseData.CustomFileCredit[0].PaymentTime != null ? creditResponseData.CustomFileCredit[0].PaymentTime.Substring(8, 4) : null;
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

            if (!string.IsNullOrWhiteSpace(time))
            {
                txt = string.Concat(txt, time);
            }
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
    }
    class ResultClientProgressBar
    {
        public bool stopMeNow { get; set; }
        public bool continueInBackground { get; set; }
        public string responseDataXml { get; set; }
        public int ProgressStage { get; set; }

    }

}
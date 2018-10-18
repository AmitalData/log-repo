using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Helpers;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Unifreight.Data.AmitalModel.EntityKeys;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.ResponseServices;
using UnifreightIIG.Common.AgentPaymentRequestServiceReference;
using UnifreightIIG.Common.AgentPaymentReplyServiceReference;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Counters;

namespace Logitude.CustomsMessaging.UnifreightGateway.Testres
{
    public class MritTestService : UnifreightGatewayProxy
    {
       
        //public enum MethodsEnum
        //{
        //    //None,
        //    InsertImportDeclaration
        //}


        public MritTestService()
            :base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, // "ImportDeclarationService";
            true 
            )

        {
            //base.UniVersion = "1.000.000001";
            //base.UniDescription = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;// "ImportDeclarationService";
            //UniProduction = false;
          
        }

   
        
        public override void ProccessRequest(
            string DataIn1,
            string DataIn2,
            out string DataOut1,
            out string DataOut2,
            out string SUCCESS,
            ref string MoreParams,
            out string MessageOut )
        
        {
            DataOut1 = DataOut2 = MessageOut = "";
            SUCCESS = false.ToString();

            string result = TranslateCustomer("10010055");
            int[] AuthorizedSignerPermit = null;

            AuthorizedSignerPermit = new int[3];
            AuthorizedSignerPermit[0] = 1;
            AuthorizedSignerPermit[1] = 1;
            AuthorizedSignerPermit[2] = 1;


            if (AuthorizedSignerPermit != null)
            {
                int arraySize = AuthorizedSignerPermit.Count();
                if (arraySize > 0)
                {
                    string AuthorizedSignerPermit1 = AuthorizedSignerPermit[0].ToString();
                }
                if (arraySize > 1)
                {
                    string AuthorizedSignerPermit2 = AuthorizedSignerPermit[1].ToString();
                }
                if (arraySize > 2)
                {
                    string AuthorizedSignerPermit3 = AuthorizedSignerPermit[2].ToString();
                }
            }
            return;

            D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam requestParams2715 = new D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityRequestParam();
            requestParams2715.DeclaretionId = "1-1047";
            requestParams2715.DocumentsFilingId = "eiys5j9tz0a2iqmj5li7ra00000000";
            requestParams2715.DocumentsTicketId = "1-975";
            requestParams2715.Tenant = 1;
            requestParams2715.LoggingUserId = "1-6032";
            requestParams2715.RequestVIA = SendRequestVIA.WebServiceInteractive;
            AddAttachmentResponseData responseData2715;

            var messagingService = new D_NG_2715_MSG22002_AddAGlobalScannedAttachmentToEntityMessagingService();
            responseData2715 = messagingService.Send(requestParams2715);

            return;


            ICustomContext customContext = CustomContext.GetContext(208);
            var myCustomsDocumentQueryService = new CustomsDocumentQueryService(customContext);
            GetTicketsParams GetTicketsParam = new GetTicketsParams();
            GetTicketsParam.ParentEntityCode = "Declaration";
            GetTicketsParam.ParentEntityId = "1-707";
            GetTicketsParam.Child1EntityCode = "SupplierInvoice";
            GetTicketsParam.Child1EntityId = "1";

            //List<CustomsDocumentPM> list = myCustomsDocumentQueryService.GetCustomsDocumentPMList(GetTicketsParam, 208);
            //List<CustomsDocumentPM> list = myCustomsDocumentQueryService.GetCustomsDocumentPMList("1-707", "Declaration", 208);


            return;


            ICustomContext _Context = CustomContext.GetContext(1);
            var customsDocumentQueryService = new CustomsDocumentQueryService(_Context);
            var customsDocumentsTicketQueryService = new CustomsDocumentsTicketQueryService(_Context);

            CustomsDocumentsTicketPM _CustomsDocumentsTicketPM = customsDocumentsTicketQueryService.GetSingle("1-315", true, false);
            var MyRequestSheetParam = new RequestSheetParam();
            MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            MyRequestSheetParam.EntityId1 = _CustomsDocumentsTicketPM.CustomsDocumentPointers.FirstOrDefault().ParentEntityId;
            MyRequestSheetParam.ObjectTableId2 = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument");
            MyRequestSheetParam.EntityId2 = "1-315";

            return;

            var temp = CheckCustomFileCredit();
            return;

            var service = new TSH_MSG7_AgentPaymentReplyResponseService();
            GenericRequestParams requestparams = new GenericRequestParams();
            requestparams.AppicationId = "1-76";
            requestparams.Tenant = 1;
            TSH_MSG7_AgentPaymentReply response = new TSH_MSG7_AgentPaymentReply();
            response.AgentPaymentReply = new TSH_MSG7_AgentPaymentReplyAgentPaymentReply();
            response.AgentPaymentReply.status = 3;
            response.AgentPaymentMethods = new TSH_MSG7_AgentPaymentReplyAgentPaymentMethods[2];
            response.AgentPaymentMethods[0] = new TSH_MSG7_AgentPaymentReplyAgentPaymentMethods();
            response.AgentPaymentMethods[0].type = 1;
            response.AgentPaymentMethods[0].amount = 400;
            response.AgentPaymentMethods[0].paymentMethodStatus = 2;

            response.AgentPaymentMethods[1] = new TSH_MSG7_AgentPaymentReplyAgentPaymentMethods();
            response.AgentPaymentMethods[1].type = 2;
            response.AgentPaymentMethods[1].amount = 1250;
            response.AgentPaymentMethods[1].paymentMethodStatus = 2;

            service.Update(response,requestparams);


            return;

            var messageService = new CL_3630_AddUpdateDeleteAddressContactMassagingService();
            AddAddressContactForClient clientParams = new AddAddressContactForClient();
            clientParams.ExternalId = "335";
            clientParams.OperationType = AddAddressContactForClient.OperationTypes.Delete;
            clientParams.AddressCode = new AddAddressContactForClient.ClientAddress();
            clientParams.AddressCode.AddressId = "1-55";
            clientParams.Tenant = 1;
            INF_MSG_GenericResponseData responseData = null;
            responseData = messageService.Send(clientParams);

            return;

               var customResponse = new TSH_MSG2_PaymentOrderReply();
                var reData = new Logitude.CustomsMessaging.Common.ResponseData.INF_MSG_GenericResponseData();

                var requestParams = new NewPaymentRequestParams();
                requestParams.Tenant = 1;

                /*var context = CustomContext.GetContext(requestParams.Tenant); // to check what to do? how can i know witch record in CustomsDocumentPointers ???
                var myCustomsDocumentPointerQueryService = new CustomsDocumentPointerQueryService(context);
                var customsDocumentPM = myCustomsDocumentPointerQueryService.GetSingle(addAttachmentMessage.RequiredDocumentDetails.DocumentID.ToString(), true, false);
                requestParams.AppicationId = customsDocumentPM.DocumentInId;*/
                TSH_MSG2_3050_PaymentOrderReplyResponseService customResponseService = new TSH_MSG2_3050_PaymentOrderReplyResponseService();
                customResponseService.Update(customResponse, requestParams);
        
        }

        private string TranslateCustomer(string amitalCustomerCode)
        {
            if (String.IsNullOrWhiteSpace(amitalCustomerCode))
            {
                AppendLogLine("AmitalCustomerCode is null");
                return null;
            }
            var repository = new CardRepository(208);
            var myCard = repository.GetSingleCardByCode(amitalCustomerCode, 208, false);
            if (myCard == null)
            {
                //Check if Customer exists using CODE , if not create a new customer
                myCard = new Card();
                myCard.Id = IdCounter.GetNumber("Card", 208).ToString();
                myCard.CreateDate = DateTime.Now;
                myCard.Code = "10010055";
                myCard.Tenant = 208;
                myCard.EnglishName = "test";
                myCard.LocalName = "בדיקה";
                myCard.PartnerTypeId = "CS";
                repository.Add(myCard);
                repository.SubmitChanges();
                AppendLogLine("Create new Customer = " + amitalCustomerCode + " because could not translate to Logitude Id");
            }
            var cardId = myCard.Id;
            AppendLogLine("AmitalCustomerCode = " + amitalCustomerCode + " Translated to " + cardId);
            return cardId;
        }


        //public byte[] CheckCustomFileCredit(byte[] requestParamsData)
        public byte[] CheckCustomFileCredit()
        {

            /*                var setting = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);
                if (setting.IsConnectedToUniFreight)
                {
                    var customFileCreditModel = new Logitude.Customs.BL.Messaging.L2U.CustomFile.CustomFileCreditService.CustomFileCreditModel();
                    customFileCreditModel.AppicationId = requestParams.AppicationId;
                    customFileCreditModel.Tenant = requestParams.Tenant;
                    customFileCreditModel.Mode = "Check";
                    //ClientProgressBarIndicatorService.UpdateStage(requestParams.PBId, "check CustomFileCreditService");
                    CustomFileCreditService customFileCreditService = new CustomFileCreditService(customFileCreditModel);
                    var res = customFileCreditService.CheckFileCredit();

                }
             
             
             
                         var setting = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);
            if (setting.IsConnectedToUniFreight )
            {
                if (!requestParams.Trans2Govee)
                {
                    var customFileCreditModel = new Logitude.Customs.BL.Messaging.L2U.CustomFile.CustomFileCreditService.CustomFileCreditModel();
                    customFileCreditModel.AppicationId = requestParams.AppicationId;
                    customFileCreditModel.Tenant = requestParams.Tenant;
                    customFileCreditModel.Mode = "Check";
                    ClientProgressBarIndicatorService.UpdateStage(requestParams.PBId, "check CustomFileCreditService");
                    CustomFileCreditService customFileCreditService = new CustomFileCreditService(customFileCreditModel);
                    var res = customFileCreditService.CheckFileCredit();

                    bool transGove = true;

                    if (transGove)
                    {
                        BusinessErrorException businessErrorException = new BusinessErrorException("");

                        var response = new Logitude.CustomsMessaging.Common.ResponseData.SubmitDeclarationResponseData();
                        response.IsTRansGove = true;
                        response.Succeeded = true;
                        response.HasException = false;
                        businessErrorException.CurrentContextTag = response;

                        throw businessErrorException;

                    }
                    else
                    {
                        ///BusinessErrorException businessErrorException = new BusinessErrorException("אין כסף !!!");
                        BusinessErrorException businessErrorException = new BusinessErrorException("");
                        var response = new Logitude.CustomsMessaging.Common.ResponseData.SubmitDeclarationResponseData();
                        response.Succeeded = false;
                        response.HasException = true;
                        response.ExceptionMessage = "אין כסף !!!";
                        businessErrorException.CurrentContextTag = response;

                        throw businessErrorException;

                    }
                               
                }
                else
                {
                    ///trans Govve
                }
            }
           
             */
            //MemoryStream memorystream = new MemoryStream(requestParamsData);
            //XmlSerializer serializer = new XmlSerializer(typeof(GenericRequestParams));
            //GenericRequestParams declarationParams = (GenericRequestParams)serializer.Deserialize(memorystream);

            CustomFileCreditResponseData responseData = new CustomFileCreditResponseData();
            CustomFileCreditRequestParams requestParamsCredit = new CustomFileCreditRequestParams();
            requestParamsCredit.AppicationId = "1-10";
            requestParamsCredit.LoggingUserId = "MIRIT";
            requestParamsCredit.Tenant = 1;
            requestParamsCredit.Mode = "Check";


            var setting = CustomsSettingQueryService.GetSettingByTenant(requestParamsCredit.Tenant);
            if (setting.IsConnectedToUniFreight)
            {
                var myCustomFileCreditService = new CustomFileCreditService(requestParamsCredit);
                CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit();

                responseData.CreditStatus = creditResponseData.CustomFileCredit[0].CreditStatus;
                if (creditResponseData.CustomFileCredit[0].ErrorMessage != null || creditResponseData.CustomFileCredit[0].ErrorMessage != "")
                {
                    responseData.UserMessage = creditResponseData.CustomFileCredit[0].ErrorMessage;
                    responseData.HasException = true;
                }
                responseData.BankCode = creditResponseData.CustomFileCredit[0].BankCode;
                responseData.PaymentDate = creditResponseData.CustomFileCredit[0].PaymentDate;
                responseData.BillingTaxAmount = creditResponseData.CustomFileCredit[0].BillingTaxAmount;
                responseData.IsTRansGove = false;
            }
            else
            {
                responseData.CreditStatus = "5";
            }

            if (responseData.CreditStatus == "5" | responseData.CreditStatus == "3")
            {
                GenericRequestParams submitRequestParams = new GenericRequestParams();
                submitRequestParams = requestParamsCredit;
                var messagingService = new DF_NG_2755_MSG12001_SubmitDeclarationMessagingService();
                INF_MSG_GenericResponseData submitResponseData = messagingService.Send(submitRequestParams);
                responseData.Succeeded = submitResponseData.Succeeded;
                responseData.HasException = submitResponseData.HasException;
                responseData.UserMessage = submitResponseData.UserMessage;
            }
            else if (responseData.CreditStatus == "1")
            {
                responseData.IsTRansGove = true;
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(responseData.GetType());
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
        
 
        public byte[] SearchCurrencyRateRequest(byte[] searchCurrencyRateParams)
        {
            MemoryStream memorystream = new MemoryStream(searchCurrencyRateParams);
            XmlSerializer serializer = new XmlSerializer(typeof(CD_NG_8347_Web01_CurrencyRateSearchRequestParams));
            //CD_NG_8347_Web01_CurrencyRateSearchRequestParams searchRateParams = (CD_NG_8347_Web01_CurrencyRateSearchRequestParams)serializer.Deserialize(memorystream);
            CD_NG_8347_Web01_CurrencyRateSearchRequestParams searchRateParams = new CD_NG_8347_Web01_CurrencyRateSearchRequestParams();
            searchRateParams.CurrencyTypeId = "NIS";
            searchRateParams.Tenant = 1;
            searchRateParams.FromDate = DateTime.Today;
            searchRateParams.ToDate = DateTime.Today;

            CD_NG_8348_Web02_CurrencyRateDetailResponseData responseData;
            if (searchRateParams.TestCase != null && searchRateParams.TestCase.Type == "webservice" && searchRateParams.TestCase.Code != "Real Logic")
            {
                responseData = new CD_NG_8348_Web02_CurrencyRateDetailResponseData();
                switch (searchRateParams.TestCase.Code)
                {

                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            //responseData.CurrencyRateList = GetTestCurrencyRateResultList(searchRateParams);
                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for search custom currency rate message!";
                            break;
                        }
                }
            }
            else
            {
                var messageService = new CD_NG_8347_Web01_CurrencyRateSearchParamMessagingService();
                responseData = messageService.Send(searchRateParams);

            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CD_NG_8348_Web02_CurrencyRateDetailResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
 

        public override string GetAssemblyQualifiedName()
        {
            return this.GetType().Name;
            //return this.GetType().AssemblyQualifiedName;
            //"UnifreightGatewayServer.BL.TaskYam.LogIn.TYLoginService, UnifreightGatewayServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }

        public override string GetExampleDataIn1()
        {
            
            return "Ask mirit ..."; 
        }

        public override string GetExampleDataIn2()
        {

            

            
            return "";
        }

        public override string GetExampleDataout1()
        {

            return ""; //new entity  ...import dec.
        }
        public override string GetExampleDataout2()
        {

            return "";
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Members

        //public   void Dispose()
        //{


        //}
        public override void Dispose()
        {
            
        }
        #endregion
    }
  
 
}

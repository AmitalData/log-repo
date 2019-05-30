using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.BL.Validators;
using System.Data.Entity.Infrastructure;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Transactions;
using System.Data.Common;
using Simplog.Data.InfrastructureModel;


namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for DeclarationWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeclarationWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public byte [] SendDeclaration(byte[] declarationParamsData, string declarationId, int tenant)
        {
            MemoryStream memorystream = new MemoryStream(declarationParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(GenericRequestParams));
            GenericRequestParams declarationParams = (GenericRequestParams)serializer.Deserialize(memorystream);
        
            // for test.
            INF_MSG_GenericResponseData responseData;
            if (declarationParams.TestCase != null && declarationParams.TestCase.Type == "webservice" && declarationParams.TestCase.Code!="Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (declarationParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            ICustomContext customContext = CustomContext.GetContext(tenant);
                            DeclarationQueryService declarationQuery = new DeclarationQueryService(tenant);
                            DeclarationPM declaration = declarationQuery.GetSingle(declarationId, false, false);
                            declaration.IsChanged = false;

                            DeclarationConstraintQueryService constraintQuery = new DeclarationConstraintQueryService(tenant);
                            DeclarationConstraintUpdateService constraintService = new DeclarationConstraintUpdateService(customContext, new Dictionary<string, IContext>(), tenant);

                            //List<DeclarationConstraintPM> declarationconstraints = constraintQuery.GetDeclarationConstraintsByDeclrationId(declarationId, tenant);
                            //foreach (DeclarationConstraintPM item in declaration.DeclarationConstraints)
                            //{

                            //    item.ChangeSetOp = ChangeSetOperation.Update;
                            //    constraintService.Update(item, true);
                            //}

                           // declaration.DeclarationStatusTypeCode = "12";
                            declaration.ChangeSetOp = ChangeSetOperation.Update;
                            DeclarationUpdateService updateService = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
                            updateService.Update(declaration, true);
                           
                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send declaration message!";
                            break;
                        }
                    case "In Background":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.ContinueProcessInBackground = true;
                            ICustomContext customContext = CustomContext.GetContext(tenant);
                            DeclarationQueryService declarationQuery = new DeclarationQueryService(tenant);
                            DeclarationPM declaration = declarationQuery.GetSingle(declarationId, false, false);
                            declaration.IsChanged = false;
                            declaration.ChangeSetOp = ChangeSetOperation.Update;
                            DeclarationUpdateService updateService = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
                            updateService.Update(declaration, true);
                            break;
                        }
                }

            }
            else
            {
                var messagingService = new DF_MSG10000_ImportDeclarationMessagingService();
                responseData = messagingService.Send(declarationParams);
            }
#if false
                ICustomContext customContext = CustomContext.GetContext(tenant);
                DeclarationQueryService declarationQuery = new DeclarationQueryService(tenant);
                DeclarationPM declaration =   declarationQuery.GetSingle(declarationId, false, false);
                declaration.IsChanged = false;
                declaration.ChangeSetOp = ChangeSetOperation.Update;
                DeclarationUpdateService updateService = new DeclarationUpdateService(customContext,new Dictionary<string,IContext>(),tenant);
                updateService.Update(declaration, true);
   
#endif

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(responseData.GetType());
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendPayment(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(GenericRequestParams));
            GenericRequestParams requestParams = (GenericRequestParams)serializer.Deserialize(memorystream);
            INF_MSG_GenericResponseData responseData;
            //DeclarationQueryService query = new DeclarationQueryService(requestParams.Tenant);
            //DeclarationPM declaration = query.GetSingle(requestParams.AppicationId, false, true);
            //ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);
            //DeclarationUpdateService updateService = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), requestParams.Tenant);
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            //declaration.PaymentDate = DateTime.Today.Date;
                            //declaration.ChangeSetOp = ChangeSetOperation.Update;
                            //updateService.Update(declaration, true);
                           
                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send declaration payment message!";
                            break;
                        }

                }

            }
            else
            {
                var messagingService = new DF_NG_2755_MSG12001_SubmitDeclarationMessagingService();
                responseData = messagingService.Send(requestParams);
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

        [WebMethod]
        public byte[] SendPaymentWithCheckCustomFileCredit(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(CustomFileCreditRequestParams));
            CustomFileCreditRequestParams requestParamsCredit = (CustomFileCreditRequestParams)serializer.Deserialize(memorystream);
            CustomFileCreditResponseData responseData = new CustomFileCreditResponseData();
            //var setting = CustomsSettingQueryService.GetSettingByTenant(requestParamsCredit.Tenant);
            //if (setting.IsConnectedToUniFreight)
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(requestParamsCredit.Tenant);
            DeclarationPM declarationPM = declarationQueryService.GetSingleDeclarationById(requestParamsCredit.AppicationId, requestParamsCredit.Tenant);
            if(declarationPM != null && declarationPM.IsConnectedToUnifreight)
            {
                try
                {
                    ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "שליחת בקשה לבדיקת אשראי");
                    var myCustomFileCreditService = new CustomFileCreditService(requestParamsCredit);
                    CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit();
                    ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "ניתוח תשובה בקרת אשראי");
                    responseData.CreditStatus = creditResponseData.CustomFileCredit[0].CreditStatus;
                    if (creditResponseData.CustomFileCredit[0].ErrorMessage != null || creditResponseData.CustomFileCredit[0].ErrorMessage != "")
                    {
                        responseData.UserMessage = creditResponseData.CustomFileCredit[0].ErrorMessage;
                        responseData.HasException = true;
                    }
                    responseData.Succeeded = true;
                    responseData.BankCode = creditResponseData.CustomFileCredit[0].BankCode;
                    responseData.PaymentDate = creditResponseData.CustomFileCredit[0].PaymentDate;
                    responseData.BillingTaxAmount = creditResponseData.CustomFileCredit[0].BillingTaxAmount;
                    responseData.IsTRansGove = false;
                }
                catch (Exception e)
                {
                    responseData.CreditStatus = "0";
                    responseData.Succeeded = true ;
                    responseData.HasException = true;
                    responseData.UserMessage = e.ToString();
                }
            }
            else
            {
                responseData.CreditStatus = "5";
                responseData.IsTRansGove = false;
            }

            if (responseData.CreditStatus == "5" | responseData.CreditStatus == "3")
            {
                ClientProgressBarIndicatorService.UpsertClientProgressBarIndicatorCurrentStage(requestParamsCredit.PBId, "תחילת שליחה למכס- הגשת תשלום");
                GenericRequestParams submitRequestParams = new GenericRequestParams();
                submitRequestParams.AppicationId = requestParamsCredit.AppicationId;
                submitRequestParams.InterfaceTypeCode = requestParamsCredit.InterfaceTypeCode;
                submitRequestParams.PBId = requestParamsCredit.PBId;
                submitRequestParams.CustomsRequestsSheetId = requestParamsCredit.CustomsRequestsSheetId;
                submitRequestParams.Tenant = requestParamsCredit.Tenant;
                submitRequestParams.RequestVIA = requestParamsCredit.RequestVIA;
                submitRequestParams.LoggingUserId = requestParamsCredit.LoggingUserId;
                submitRequestParams.ForcePersonalSign = requestParamsCredit.ForcePersonalSign;

                submitRequestParams.LoggingEntityId = requestParamsCredit.LoggingEntityId;
                submitRequestParams.LoggingEntityId2 = requestParamsCredit.LoggingEntityId2;
                submitRequestParams.LoggingObjectTableId = requestParamsCredit.LoggingObjectTableId;
                submitRequestParams.LoggingObjectTableId2 = requestParamsCredit.LoggingObjectTableId2;
                
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

        [WebMethod]
        public byte[] SendTransferRequest(byte[] requestParamsData) // Mirit 07/04/15 Task-12283 
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(CustomFileCreditRequestParams));
            CustomFileCreditRequestParams requestParamsCredit = (CustomFileCreditRequestParams)serializer.Deserialize(memorystream);
            CustomFileCreditResponseData responseData = new CustomFileCreditResponseData();
            //var setting = CustomsSettingQueryService.GetSettingByTenant(requestParamsCredit.Tenant);
            //if (setting.IsConnectedToUniFreight)
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
                catch (Exception e)
                {
                    responseData.CreditStatus = "0";
                    responseData.Succeeded = false; ;
                    responseData.HasException = true;
                    responseData.UserMessage = e.ToString();
                }
            }
            else
            {
                responseData.CreditStatus = "5";
                responseData.IsTRansGove = false;
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

        [WebMethod]
        public byte[] SearchDeclarationStatusRequest(byte[] searchDeclarationStatusParams)
        {
            MemoryStream memorystream = new MemoryStream(searchDeclarationStatusParams);
            XmlSerializer serializer = new XmlSerializer(typeof(DeclarationStatusRequestParams));
            DeclarationStatusRequestParams newSearchDeclarationStatusRequestParams = (DeclarationStatusRequestParams)serializer.Deserialize(memorystream);
        
            DeclarationStatusResponseData responseData = null;
            if (newSearchDeclarationStatusRequestParams.TestCase != null && newSearchDeclarationStatusRequestParams.TestCase.Type == "webservice" && newSearchDeclarationStatusRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new DeclarationStatusResponseData();
                switch (newSearchDeclarationStatusRequestParams.TestCase.Code)
                {
                    case "Search Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            responseData.ResponseStatusXML = "Test Status String to be xml";

                            break;
                        }
                    case "Search Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for search Declaration Status message!";
                            break;
                        }

                }

            }
            else
            {

                 // use messageing service
                var service = new DF_NG_8250_Web01_DeclarationStatus_RequestMessagingService();
                responseData = service.Send(newSearchDeclarationStatusRequestParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(DeclarationStatusResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendDeclarationConstraint(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(ConstraintApprovalRequestParams));
            ConstraintApprovalRequestParams requestParams = (ConstraintApprovalRequestParams)serializer.Deserialize(memorystream);
            INF_MSG_GenericResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send declaration constraint!";
                            break;
                        }

                }

            }
            else
            {
                // use messageing service
                var messagingService = new EV_NG_8214_MSG23001_ConstraintApprovalRequestMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(INF_MSG_GenericResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendCollateralAnswers(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(CollateralRequestParams));
            CollateralRequestParams requestParams = (CollateralRequestParams)serializer.Deserialize(memorystream);
            INF_MSG_GenericResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send collateral answers!";
                            break;
                        }

                }

            }
            else
            {
                // use messageing service
                var messagingService = new COLT_NG_8212_MSG10041_CollateralAnswerMsgMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(INF_MSG_GenericResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendMorningMessageRequest(byte[] morningMessageRequestParams)
        {
            MemoryStream memorystream = new MemoryStream(morningMessageRequestParams);
            XmlSerializer serializer = new XmlSerializer(typeof(MorningMessageRequestParams));
            MorningMessageRequestParams newMorningMessageRequestParams = (MorningMessageRequestParams)serializer.Deserialize(memorystream);

            MorningMessageResponseData responseData = null;
            if (newMorningMessageRequestParams.TestCase != null && newMorningMessageRequestParams.TestCase.Type == "webservice" && newMorningMessageRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new MorningMessageResponseData();
                switch (newMorningMessageRequestParams.TestCase.Code)
                {
                    case "Search Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            //responseData.ResponseStatusXML = "Test Status String to be xml";

                            break;
                        }
                    case "Search Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for search Declaration Status message!";
                            break;
                        }
                }
            }
            else
            {
                // use messageing service
                var service = new MM_Web01_MorningMessagesListMessagingService();
                responseData = service.Send(newMorningMessageRequestParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(MorningMessageResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }


        [WebMethod]
        public byte[] SendDeclarationConstraintAgentObjection(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(ConstraintAgentObjectionRequestParams));
            ConstraintAgentObjectionRequestParams requestParams = (ConstraintAgentObjectionRequestParams)serializer.Deserialize(memorystream);

            ConstraintAgentAnswerResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new ConstraintAgentAnswerResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send declaration constraint!";
                            break;
                        }

                }

            }
            else
            {
                // use messageing service
                var messagingService = new EV_NG_8216_MSG23003_ConstraintAgentAnswerMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(ConstraintAgentAnswerResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendCargoQuery(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(CargoQueryRequestParams));
            CargoQueryRequestParams requestParams = (CargoQueryRequestParams)serializer.Deserialize(memorystream);
            //CargoQueryRequestParams, CargoQueryResponseData
            CargoQueryResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new CargoQueryResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send Cargo Query!";
                            break;
                        }

                }

            }
            else
            {
                // use messageing service
                var messagingService = new MN_NG_8240_CargoQueryMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CargoQueryResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        //<--- Yuval Chalup 24.12.2014 TASK-9972
        [WebMethod]
        public byte[] SendDeclarationRestore(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(DeclarationRestoreRequestParams));
            DeclarationRestoreRequestParams requestParams = (DeclarationRestoreRequestParams)serializer.Deserialize(memorystream);
            DeclarationRestoreResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new DeclarationRestoreResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send declaration restore!";
                            break;
                        }
                }
            }
            else
            {
              
                // use messageing service
                var messagingService = new DF_NG_8373_Web05_RetrieveImportDeclarationMessagingService();
                responseData = messagingService.Send(requestParams);
              
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(DeclarationRestoreResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
        //Yuval Chalup 24.12.2014 TASK-9972 --->

        //<--- Yuval Chalup 24.12.2014 TASK-9760
        [WebMethod]
        public byte[] SendDeclarationFaultProcedural(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(FaultProceduralRequestParams));
            FaultProceduralRequestParams requestParams = (FaultProceduralRequestParams)serializer.Deserialize(memorystream);
            FaultProceduralResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new FaultProceduralResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send declaration Faults!";
                            break;
                        }
                }
            }
            else
            {
                
                // use messageing service
                var messagingService = new DF_NG_Web8332_FaultProceduralParamMessagingService();
                responseData = messagingService.Send(requestParams);
               
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(FaultProceduralResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
        //Yuval Chalup 24.12.2014 TASK-9760 --->

        // moran 3.11.14 - Task 7933 -->
        [WebMethod]
        public byte[] SendWarehouseBlockBalanceQuery(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(ST_8328_Web01_WarehouseBlockBalanceRequestParams));
            ST_8328_Web01_WarehouseBlockBalanceRequestParams requestParams = (ST_8328_Web01_WarehouseBlockBalanceRequestParams)serializer.Deserialize(memorystream);
            ST_8328_Web01_WarehouseBlockBalanceResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new ST_8328_Web01_WarehouseBlockBalanceResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send declaration constraint!";
                            break;
                        }

                }

            }
            else
            {
                // use messageing service
                var messagingService = new ST_8328_Web01_WarehouseBlockBalanceMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(ST_8328_Web01_WarehouseBlockBalanceResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
        // moran 3.11.14 - Task 7933 <--


        [WebMethod]
        public byte[] SendCollateralAnswerMsgRequest(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(CollateralRequestParams));
            CollateralRequestParams requestParams = (CollateralRequestParams)serializer.Deserialize(memorystream);

            INF_MSG_GenericResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send request file!";
                            break;
                        }
                }
            }
            else
            {
                // use messageing service
                var messagingService = new COLT_NG_8212_MSG10041_CollateralAnswerMsgMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(INF_MSG_GenericResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] RefreshConsignmentRequest(byte[] cargoQueryParams, int tenant)
        {
            MemoryStream memorystream = new MemoryStream(cargoQueryParams);
            XmlSerializer serializer = new XmlSerializer(typeof(CargoQueryRequestParams));
            CargoQueryRequestParams requestParams = (CargoQueryRequestParams)serializer.Deserialize(memorystream);
            //INF_MSG_GenericResponseData responseData;

            ICustomContext customContext = CustomContext.GetContext(tenant);
            DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);
            CargoQueryResponseData responseData;

            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new CargoQueryResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            Random rand = new Random();
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            responseData.ApplicationID = rand.Next(1000000).ToString();

                            DeclarationPM declaration = declarationQuery.GetSingle(requestParams.DeclarationId, true, false);

                            declaration.ChangeSetOp = ChangeSetOperation.Update;
                            //declaration.DeclarationDocumentId = "55";
                            ////consignment.ThirdCargoID = "111";
                            //declaration.Consignments.Add(new ConsignmentPM() { DeclarationId = declaration.Id, ConsignmentNumber = 11, Tenant = declaration.Tenant, ChangeSetOp= ChangeSetOperation.Insert, CargoTypeCode = "11", ManifestNumber = "1122" });
                            DeclarationUpdateService updateService = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
                            updateService.Update(declaration, true);

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for refresh consignment!";
                            break;
                        }
                }
            }
            else
            {
                var messagingService = new MN_NG_8240_CargoQueryMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CargoQueryResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendImporterDeclarationRequest(byte[] importerDeclarationRequestParams)
        {
            MemoryStream memorystream = new MemoryStream(importerDeclarationRequestParams);
            XmlSerializer serializer = new XmlSerializer(typeof(ImporterDeclarationRequestParams));
            ImporterDeclarationRequestParams newImporterDeclarationRequestParams = (ImporterDeclarationRequestParams)serializer.Deserialize(memorystream);

            ImporterDeclarationResponseData responseData = null;
            if (newImporterDeclarationRequestParams.TestCase != null && newImporterDeclarationRequestParams.TestCase.Type == "webservice" && newImporterDeclarationRequestParams.TestCase.Code != "Real Logic")
            {
                responseData = new ImporterDeclarationResponseData();
                switch (newImporterDeclarationRequestParams.TestCase.Code)
                {
                    case "Search Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            //responseData.ResponseStatusXML = "Test Status String to be xml";

                            break;
                        }
                    case "Search Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for search Declaration Status message!";
                            break;
                        }
                }
            }
            else
            {
                // use messageing service
                var service = new VE_8326_ImporterDeclarationMessagingService();
                responseData = service.Send(newImporterDeclarationRequestParams);
            }
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(ImporterDeclarationResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }

        [WebMethod]
        public byte[] SendDeclarationPrintRequest(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(DF_NG_8302_Web03_DeclarationPrintRequestParams));
            DF_NG_8302_Web03_DeclarationPrintRequestParams requestParams = (DF_NG_8302_Web03_DeclarationPrintRequestParams)serializer.Deserialize(memorystream);
            DeclarationPrintResponseData responseData;

            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new DeclarationPrintResponseData();
                switch (requestParams.TestCase.Code)
                {
                case "Send Succeeded":
                    {
                        responseData.HasException = false;
                        responseData.Succeeded = true;
                        responseData.UserMessage = null;
                        break;
                    }
                    case "Send Failed":
                    {
                        responseData.HasException = true;
                        responseData.Succeeded = false;
                        responseData.UserMessage = "this is a test fail exception for send declaration payment message!";
                        break;
                    }
                }
            }
            else
            {
                var messagingService = new DF_NG_8302_Web03_DeclarationPrintMessagingService();
                responseData = messagingService.Send(requestParams);
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

        [WebMethod]
        public byte[] SendMasterBOLQueryRequest(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(MasterBOLQueryRequestParams));
            MasterBOLQueryRequestParams requestParams = (MasterBOLQueryRequestParams)serializer.Deserialize(memorystream);
            MasterBOLFeedBackResponseData responseData;

            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new MasterBOLFeedBackResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;
                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send declaration payment message!";
                            break;
                        }
                }
            }
            else
            {
                var messagingService = new MN_NG_9020_MasterBOLQueryMessagingService();
                responseData = messagingService.Send(requestParams);
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

        // moran 18.6.15 - Task 13281 -->
        [WebMethod]
        public byte[] SendPaymentQuery(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(TSH_NG_8285_Web01_PaymentRequestParams));
            TSH_NG_8285_Web01_PaymentRequestParams requestParams = (TSH_NG_8285_Web01_PaymentRequestParams)serializer.Deserialize(memorystream);
            TSH_NG_8285_Web01_PaymentResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new TSH_NG_8285_Web01_PaymentResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send Payment Query!";
                            break;
                        }

                }

            }
            else
            {
                // use messageing service
                var messagingService = new TSH_NG_8285_Web01_PaymentFilterParamMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(TSH_NG_8285_Web01_PaymentResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
        // moran 18.6.15 - Task 13281 <--

        // moran 7.7.15 - Task 13442 -->
        [WebMethod]
        public byte[] SendGuaranteeFileFilterQuery(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(GuaranteeRequestParams));
            GuaranteeRequestParams requestParams = (GuaranteeRequestParams)serializer.Deserialize(memorystream);
            GuaranteeResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new GuaranteeResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send Guarantee Query!";
                            break;
                        }

                }

            }
            else
            {
                // use messageing service
                var messagingService = new TPG_NG_8305_Web05_GuaranteeFileFilterParamMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(GuaranteeResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
        // moran 7.7.15 - Task 13442 <--

        //<--- Yuval Chalup 07.09.2015 TASK-15037
        [WebMethod]
        public byte[] SendDeclarationFilter(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(DeclarationFilterRequestParams));
            DeclarationFilterRequestParams requestParams = (DeclarationFilterRequestParams)serializer.Deserialize(memorystream);
            DeclarationFilterResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new DeclarationFilterResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send declaration filter!";
                            break;
                        }
                }
            }
            else
            {

                // use messageing service
                var messagingService = new TPG_NG_8307_Web09_DeclarationFilterMessagingService();
                responseData = messagingService.Send(requestParams);

            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(DeclarationFilterResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }
        //Yuval Chalup 07.09.2015 TASK-15037--->







        [WebMethod]
        public byte[] CheckCertificateStatus(string declarationId, int tenant)
        {


            ICustomContext customContext = CustomContext.GetContext(tenant);
            CustomContext activeContext = customContext.GetActiveDbContext() as CustomContext;
            object[] parameters = new object[] { };
            string dbms = System.Configuration.ConfigurationManager.AppSettings.Get("DBMS");

            DbRawSqlQuery<SupplierInvoiceItem> items = null;

            if (dbms == "oracle")
            {
                items = activeContext.Database.SqlQuery<SupplierInvoiceItem>("select * from supplierInvoiceItems where CertificatesStatusCode ='2' and declarationid ='" +declarationId +"' and tenant = " + tenant, parameters);
            }
            else
            {
                items = activeContext.Database.SqlQuery<SupplierInvoiceItem>("select * from customs.SupplierInvoiceItems  where CertificatesStatusCode = '2' and DeclarationId='" + declarationId + "' and tenant = " + tenant, parameters);
            }


            CustomsRequiredFieldErrors errors = new CustomsRequiredFieldErrors() { RequiredFields = new List<CustomsRequiredFieldsErrorItem>() };

            foreach (SupplierInvoiceItem item in items)
            {
                errors.RequiredFields.Add(new CustomsRequiredFieldsErrorItem() { CustomMessageError = "Certificate fields are not valid", EntityReference = item.CounterKey.ToString(), EntityReference2 = item.SequenceNumeric.ToString(), FieldName = "CertificateStatusCode", TableName = "SupplierInvoiceItem" });
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(CustomsRequiredFieldErrors));
            ser.Serialize(memstream, errors);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;



        }


        [WebMethod]
        public byte[] SendCargoSplit(byte[] requestParamsData)
        {
            MemoryStream memorystream = new MemoryStream(requestParamsData);
            XmlSerializer serializer = new XmlSerializer(typeof(CargoSplitRequestParams));
            CargoSplitRequestParams requestParams = (CargoSplitRequestParams)serializer.Deserialize(memorystream);
            INF_MSG_GenericResponseData responseData = null;
            if (requestParams.TestCase != null && requestParams.TestCase.Type == "webservice" && requestParams.TestCase.Code != "Real Logic")
            {
                responseData = new INF_MSG_GenericResponseData();
                switch (requestParams.TestCase.Code)
                {
                    case "Send Succeeded":
                        {
                            responseData.HasException = false;
                            responseData.Succeeded = true;
                            responseData.UserMessage = null;

                            break;
                        }
                    case "Send Failed":
                        {
                            responseData.HasException = true;
                            responseData.Succeeded = false;
                            responseData.UserMessage = "this is a test fail exception for send Cargo Split!";
                            break;
                        }

                }

            }
            else
            {
                // use messageing service
                var messagingService = new MN_MSG8370_CargoSplitMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(INF_MSG_GenericResponseData));
            ser.Serialize(memstream, responseData);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;
        }


        private static string GetConnection(int tenant)
        {
            GlobalDB currentDb;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
            {
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);
                scope.Complete();
            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;
        }
    }
}

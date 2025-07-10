using Logitude.AmitalMessaging.Customs.CustomFile.ReleaseFile;
using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IdentityModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Serialization;
using Unifreight.BL.EntityPMs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.Data.EntityListQueryServices;
using Unifreight.BL.EntityPMs.UGenerated;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.AmitalMessaging.Customs.CustomFile;

namespace Logitude.Customs.BL.Messaging.U2L.PayHand
{
    public class PayHandService : UnifreightGenericService
    {
        private LOGIPAYHAND _LOGIPAYHAND;
        private LogitudePayHand _LogitudePayHand;
        private ICustomContext _context;

        public const string UpsertActionConst = "Logitude.Customs.BL.Messaging.U2L.PayHand.PayHandService.Upsert()";
        private DeclarationPM _MyDeclarationPM;
        private Stopwatch _Stopwatch;
        private DeclarationPaymentPM declarationPaymentPM;
        private DbContextBase _AmitalContext;

        public PayHandService()
            : base(
            "1.000.000001",
            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
            true
            )
        {

        }

        public override void ProccessGenericRequest(
              string xmlLOGIPAYHAND,
              ref string MoreParams,
              out string MessageOut)
        {
            MessageOut = "";
            _Stopwatch = Stopwatch.StartNew();
            MyCommunicationsParams.Subject = "PayHandService ";
            string user = this.MyCommunicationsParams.LoggingUserId;
            if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(ResolvedTenant());

            if (MoreParams == "PAYHAND_QUEUE")
            {
                if (String.IsNullOrWhiteSpace(xmlLOGIPAYHAND))
                {
                    throw new BusinessErrorException("LOGITUDE FILE is missing");
                }
                MyGenericResponseObj.Stage = "GetContext";
                _context = CustomContext.GetContext(ResolvedTenant());
                var myQueryService = new DeclarationQueryService(_context);

                ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());

                AppendLogLine("GetContext:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                MyGenericResponseObj.Stage = "GetSingle";
                this._MyDeclarationPM = myQueryService.GetSingle(xmlLOGIPAYHAND, true, false);
                if (this._MyDeclarationPM == null)
                {
                    throw new BusinessErrorException("LOGITUDE FILE is " + xmlLOGIPAYHAND + " but not found");
                }
                AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                MyGenericResponseObj.Stage = "GetSinglePayment";
                var mydeclarationPaymentQueryService = new DeclarationPaymentQueryService(_context);
                this.declarationPaymentPM = mydeclarationPaymentQueryService.GetSingle(this._MyDeclarationPM.Id, true, false);

                AppendLogLine("GetSinglePayment:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            }
            else
            {
                DeserilazeObject(xmlLOGIPAYHAND);
                AppendLogLine("DeserilazeObject:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                //CheckIntegrity();
                //AppendLogLine("CheckIntegrity:Took:" + _Stopwatch.Elapsed.ToString());_Stopwatch.Restart(); 
                MyGenericResponseObj.Stage = "GetContext";
                _context = CustomContext.GetContext(ResolvedTenant());
                var myQueryService = new DeclarationQueryService(_context);

                ICustomContext dbContext = CustomContext.GetContext(ResolvedTenant());

                if (String.IsNullOrWhiteSpace(_LogitudePayHand.logitude_file))
                {
                    throw new BusinessErrorException("LOGITUDE FILE is missing");
                }
                AppendLogLine("GetContext:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                MyGenericResponseObj.Stage = "GetSingle";
                this._MyDeclarationPM = myQueryService.GetSingle(this._LogitudePayHand.logitude_file, true, false);
                if (this._MyDeclarationPM == null)
                {
                    throw new BusinessErrorException("LOGITUDE FILE is " + this._LogitudePayHand.logitude_file + " but not found");
                }
                AppendLogLine("GetSingle:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                MyGenericResponseObj.Stage = "GetSinglePayment";
                var mydeclarationPaymentQueryService = new DeclarationPaymentQueryService(_context);
                this.declarationPaymentPM = mydeclarationPaymentQueryService.GetSingle(this._MyDeclarationPM.Id, true, false);

                AppendLogLine("GetSinglePayment:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();

                MyGenericResponseObj.Stage = "DeclarationPaymentUpsert";

                if (declarationPaymentPM == null)
                {
                    declarationPaymentPM = new DeclarationPaymentPM()
                    {
                        DeclarationId = _MyDeclarationPM.Id,
                        Tenant = _MyDeclarationPM.Tenant,
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    };

                }

                declarationPaymentPM.PaymentDate = DateTime.Now;
                declarationPaymentPM.CreatedByUserId = user;
                declarationPaymentPM.SignatoryIdentification = _MyDeclarationPM.SignerPersonalId;
                if (declarationPaymentPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Insert) declarationPaymentPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                if (declarationPaymentPM.DeclarationPaymentMethods == null || declarationPaymentPM.DeclarationPaymentMethods.Count() < 1)
                {
                    DeclarationPaymentMethodPM declarationPaymentMethod = new DeclarationPaymentMethodPM()
                    {
                        Tenant = _MyDeclarationPM.Tenant,
                        DeclarationId = _MyDeclarationPM.Id,
                        Line = 1,
                        SequenceNumeric = 1,
                        MethodTypeCode = "1",
                        Amount = _MyDeclarationPM.TotalTax,
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    };

                    CustomBankList customBankList = new CustomBankList();
                    customBankList = GetBank();
                    if (customBankList != null)
                    {
                        declarationPaymentMethod.BankCode = customBankList.BankCode;
                        declarationPaymentMethod.BranchCode = customBankList.BranchCode;
                        declarationPaymentMethod.PayerActivityTypeCode = customBankList.PayerTypeCode;
                        declarationPaymentMethod.AccountNumber = customBankList.AccountNumber;
                        declarationPaymentMethod.CustomsBranchId = customBankList.CustomsBranchId;
                    }

                    declarationPaymentPM.DeclarationPaymentMethods.Add(declarationPaymentMethod);
                }
                else
                {
                    declarationPaymentPM.DeclarationPaymentMethods.FirstOrDefault().Amount = _MyDeclarationPM.TotalTax;
                    declarationPaymentPM.DeclarationPaymentMethods.FirstOrDefault().ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                }

                DeclarationPaymentUpdateService declarationPaymentUpdateService = new DeclarationPaymentUpdateService(_context, new Dictionary<string, IContext>(), _MyDeclarationPM.Tenant);
                declarationPaymentUpdateService.Update(declarationPaymentPM, true);

                AppendLogLine("DeclarationPaymentUpsert:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            }
            if (this._MyDeclarationPM != null && !String.IsNullOrWhiteSpace(this._MyDeclarationPM.CustomFileNo))
            {
                MyGenericResponseObj.Stage = "CCUFILEMUpdate (Clear MEHESDRAFTSTATUS) ";
                CCUFILEMPM _CCUFILEMPM;
                long lCUSTOMFILENO;
                AmitalContext _AmitalContext;
                if (!long.TryParse(this._MyDeclarationPM.CustomFileNo, out lCUSTOMFILENO))
                {
                    throw new BusinessErrorException("dirtyDeclarationPM.CustomFileNo could not convert to long ");
                }
                TransactionScope scope = null;

                if (!DbContextBaseUtil.UnifreightDataIncludedInMain_FeatureOn)
                {
                    scope = TransactionFactory.GetNewOracleReadCommittedTransaction();
                }
                try
                {
                    using (_AmitalContext = AmitalContext.GetContext(this._MyDeclarationPM.Tenant))
                    {

                        var myCCUFILEMQueryService = new CCUFILEMQueryService(_AmitalContext);
                        var myCCUFILEMUpdateService = new CCUFILEMUpdateService(_AmitalContext);
                        myCCUFILEMUpdateService.DontAddTransaction = true;//we cant add a transaction with isolation level snap shot inside a read committed one so you have to assign this prop to true mohammad.

                        int? FILENO = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);
                        if (FILENO.HasValue)
                        {
                            int? FILENO1 = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO_forUpdateNOWAIT(lCUSTOMFILENO, this._MyDeclarationPM.Tenant);

                            //do not need the composite due we delete all down entities !!!_CCUFILEMPM = myCCUFILEMQueryService.GetSingle(FILENO.Value, true, false);
                            _CCUFILEMPM = myCCUFILEMQueryService.GetSingle(FILENO.Value, false, false);
                            _CCUFILEMPM.ChangeSetOp = ChangeSetOperation.Update;
                            _CCUFILEMPM.MEHESDRAFTSTATUS = null;
                            using (var logger = (_AmitalContext as DbContextBase).CreateLogger())
                            {
                                try
                                {
                                    myCCUFILEMUpdateService.Update(_CCUFILEMPM, true);
                                }
                                catch (Exception eUpdate)
                                {
                                    LogMessagingUtil.Instance.Append("CCUFILEMUpdateService.Update:");
                                    LogMessagingUtil.Instance.AppendLine(logger.ToString(2040));
                                    throw;
                                }
                                AppendLogLine("CCUFILEMUpdateService.Update:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
                            }
                        }
                    }
                }
                finally
                {
                    if (scope != null)
                    {
                        scope.Dispose();
                    }
                }
            }

            MyGenericResponseObj.Stage = "Check integrity ";
            CustomsRequiredFieldErrors errors = CustomsRequiredFieldsValidator.GetRequiredFieldErrorsForDeclarationPayment(_MyDeclarationPM.Id, _MyDeclarationPM.Tenant);
            //CustomsRequiredFieldErrors
            List<CustomsRequiredFieldsErrorItem> errorsList = errors.RequiredFields;
            if (errorsList.Count() > 0)
            {
                List<string> RequiredFieldsList = new List<string>();
                string RequiredField;
                
                foreach (CustomsRequiredFieldsErrorItem error in errorsList)
                {
                    if (error.EntityReference2 == "OTHER")
                    {
                        RequiredField = error.EntityReference;
                    }
                    else
                    {
                        RequiredField = TranslateTextsClass.GetRequiredFieldForTableMessageTranslation("Customs.General.O.FieldForTableIsRequired", error.FieldName, error.TableName, error.EntityReference, _MyDeclarationPM.Tenant);
                    }
                    //RequiredField = TranslateTextsClass.Translate("Customs.General.O.FieldForTableIsRequired", _MyDeclarationPM.Tenant) + ", Field " + error.FieldName + ", Table " + error.TableName + ", " + error.EntityReference;
                    RequiredFieldsList.Add(RequiredField);
                    MyGenericResponseObj.ErrorDescription = String.Join(Environment.NewLine, RequiredFieldsList.ToArray());
                    MyGenericResponseObj.Message = "Errors while checking Declaration payment";
                    MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.BusinessError;
                    return;
                }

            }

            string mode = "";
            if (MoreParams == "PAYHAND_QUEUE")
            {
            MyGenericResponseObj.Stage = "Send DeclarationPayment Hand Request";
                CustomFileCreditRequestParams searchParams = new CustomFileCreditRequestParams() 
                {
                    Tenant = _MyDeclarationPM.Tenant,
                    AppicationId = declarationPaymentPM.DeclarationId,
                    LoggingEnabled = true,
                    LoggingEntityId = _MyDeclarationPM.Id,
                    InterfaceTypeCode = "2755",
                    LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                    LoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                    LoggingUserId = user,
                    RequestName = "send declaration payment request",
                    ResponseName = "send declaration payment response",
                    Mode = mode,
                    RequestVIA = SendRequestVIA.WebServiceBatch,
                };

                MemoryStream memstream = new MemoryStream();
                XmlSerializer ser = new XmlSerializer(typeof(CustomFileCreditRequestParams));
                ser.Serialize(memstream, searchParams);
                memstream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memstream);
                string content = reader.ReadToEnd();
                byte[] bytearray = memstream.ToArray();
                byte[] bytearrayRes = null;
                /*
                string uri = Simplog.Infrastructure.App.Current.Host.Source.AbsoluteUri;
                uri = uri.Replace("/ClientBin/Simplog.Infrastructure.xap", "/CustomWebServices/DeclarationWebService.asmx");
                BasicHttpBinding binding = BindingInfo.GetBindingInfo();
                Simplog.Infrastructure.DeclarationServiceReference.DeclarationWebServiceSoapClient declarationServiceReference = new Simplog.Infrastructure.DeclarationServiceReference.DeclarationWebServiceSoapClient();
                declarationServiceReference.Endpoint.Address = new System.ServiceModel.EndpointAddress(uri);
                if (declarationServiceReference.Endpoint.Address.Uri.Scheme == "https")
                {
                    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                }
                else
                {
                    binding.Security.Mode = BasicHttpSecurityMode.None;
                }
                declarationServiceReference.Endpoint.Binding = binding;
                */
                //bytearrayRes = declarationServiceReference.SendPaymentWithCheckCustomFileCredit(bytearray);
                AnalyzeResponseMessageSendPaymentWithCheckCustomFileCredit(bytearrayRes);

            }
            else
            {
                MyGenericResponseObj.Stage = "Send DeclarationPayment Hand Request";
                GenericRequestParams requestParams = new GenericRequestParams()
                {
                    Tenant = _MyDeclarationPM.Tenant,
                    AppicationId = declarationPaymentPM.DeclarationId,
                    LoggingEnabled = true,
                    LoggingEntityId = _MyDeclarationPM.Id,
                    InterfaceTypeCode = "2755",
                    LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                    LoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                    LoggingUserId = user,
                    RequestName = "send declaration payment request",
                    ResponseName = "send declaration payment response",
                    RequestVIA = SendRequestVIA.WebServiceBatch,
                    //ForcePersonalSign = _ForcePersonalSign,
                };
                SBQMessageService.CreateSheetSBQMessage<GenericRequestParams>(requestParams, false);

            }

            AppendLogLine("send declaration payment request:Took:" + _Stopwatch.Elapsed.ToString()); _Stopwatch.Restart();
            MyGenericResponseObj.Stage = "Done All ";
            MyGenericResponseObj.StatusType = GenericResponseObj.StatusEnum.Success;

        }


        private string AnalyzeResponseMessageSendPaymentWithCheckCustomFileCredit(byte[] bytearrayRes)
        {
            string message = "";
            byte[] data = bytearrayRes;

            if (data != null)
            {
                MemoryStream memorystream = new MemoryStream(data);
                XmlSerializer serializer = new XmlSerializer(typeof(CustomFileCreditResponseData));
                CustomFileCreditResponseData responseData = (CustomFileCreditResponseData)serializer.Deserialize(memorystream);
                if (responseData != null)
                {
                    if (responseData.IsTRansGove)
                    {
                        
                    }
                    else
                    {
                        message = responseData.UserMessage;
                        if (string.IsNullOrWhiteSpace(responseData.UserMessage))
                        {
                            if (!responseData.HasException && responseData.Succeeded)
                            {
                                message = "Send Payment Succeeded";
                            }
                            else
                            {
                                message = "Send Payment Failed";
                            }
                        }
                        if (!responseData.HasException && responseData.Succeeded)
                        {
                            
        }
                    }
                }
                else
                {
                    message = "Service returned a null response!";
                }
            }
            return message;
        }



        private CustomBankList GetBank()
        {
            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(ResolvedTenant());

            Boolean BlockAgentBankForMasabDefaultValue = false;
            string defValue = defaultValueQueryService.GetDefault("ISRAEL", "CGG_BLOCK_BANK", "NON", "NON", ResolvedTenant());
            if (defValue == "Y")
            {
                BlockAgentBankForMasabDefaultValue = true;
            }
            CustomBankQueryService customBankQueryService = new CustomBankQueryService(_context);
            CustomBankListQueryService customBankListQueryService = new CustomBankListQueryService(_context);
            List<CustomBankList> customBanksList = new List<CustomBankList>();
            CustomBankList customBankList = null;
            customBanksList = customBankQueryService.GetCustomBanksByCard(_MyDeclarationPM.CustomerId, _MyDeclarationPM.Tenant);
            if (customBanksList != null && customBanksList.Count() == 1) customBankList = customBanksList.Where(d => !d.InActive).FirstOrDefault();
            if (customBankList == null || string.IsNullOrWhiteSpace(customBankList.BankCode))
            {
                if(!BlockAgentBankForMasabDefaultValue)
                {
                    customBanksList = customBankListQueryService.GetList(_MyDeclarationPM.Tenant).Where(r => r.PayerTypeCode == "3" && !r.InActive).ToList();
                    if (customBanksList != null && customBanksList.Count() == 1)
                    {
                        customBankList = customBanksList.FirstOrDefault();
                    }
                }
            }
            if (customBankList == null || string.IsNullOrWhiteSpace(customBankList.BankCode))
            {
                Boolean credit = false;
                if(credit)customBankList = GetBankFromCreditCheck(customBankListQueryService, customBanksList);
                if (customBankList == null || string.IsNullOrWhiteSpace(customBankList.BankCode))
                {
                    if (!string.IsNullOrWhiteSpace(_MyDeclarationPM.CustomerCode))
                    {
                        string bank = defaultValueQueryService.GetDefault("ISRAEL", "CIM_AGENT_BANK", "NON", _MyDeclarationPM.CustomerCode, _MyDeclarationPM.Tenant);
                        if (!String.IsNullOrWhiteSpace(bank))
                        {
                            customBanksList = customBankListQueryService.GetList(_MyDeclarationPM.Tenant).Where(r => r.InternalCode == bank && !r.InActive).ToList();
                            customBankList = customBanksList.FirstOrDefault();
                        }
                    }
                }
            }
                //customBanksList = customBanksList.Where(r => !r.InActive && r.PayerTypeCode == "3");
            return customBankList;
        }

        private CustomBankList GetBankFromCreditCheck(CustomBankListQueryService customBankListQueryService, List<CustomBankList> customBanksList)
        {
            using (_AmitalContext = AmitalContext.GetContext(this._MyDeclarationPM.Tenant))
            {
                CustomBankList customBankList = new CustomBankList();
                using (var logger = (_AmitalContext as DbContextBase).CreateLogger())
                {
                    try
                    {
                        string user = this.MyCommunicationsParams.LoggingUserId;
                        if (String.IsNullOrWhiteSpace(user)) user = AuthenticationUtil.ResolveUserId(ResolvedTenant());
                        CustomFileCreditRequestParams requestParamsCredit = new CustomFileCreditRequestParams()
                        {
                            Tenant = _MyDeclarationPM.Tenant,
                            AppicationId = declarationPaymentPM.DeclarationId,
                            LoggingEnabled = true,
                            LoggingEntityId = _MyDeclarationPM.Id,
                            InterfaceTypeCode = "2755",
                            LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration"),
                            LoggingEntityReference = _MyDeclarationPM.DeclarationNumber,
                            LoggingUserId = user,
                            RequestName = "Send Credit to Get Bank Request",
                            ResponseName = "Get Credit to Get Bank Response",
                            Mode = "GetBank",
                            RequestVIA = SendRequestVIA.WebServiceBatch,
                        };
                        var myCustomFileCreditService = new CustomFileCreditService(requestParamsCredit);
                        CUSTOMCREDIT_UL creditResponseData = myCustomFileCreditService.CheckFileCredit();
                        if (creditResponseData.CustomFileCredit != null && !String.IsNullOrWhiteSpace(creditResponseData.CustomFileCredit[0].BankCode))
                        {
                            customBanksList = customBankListQueryService.GetList(_MyDeclarationPM.Tenant).Where(r => r.InternalCode == creditResponseData.CustomFileCredit[0].BankCode && !r.InActive).ToList();
                            customBankList = customBanksList.FirstOrDefault();
                        }
                    }
                    catch (Exception e)
                    {
                        LogMessagingUtil.Instance.Append("CCUFILEMUpdateService.Update: " + e.ToString());
                        LogMessagingUtil.Instance.AppendLine(logger.ToString(2040));
                        // throw;
                    }
                }
                return customBankList;
            }
        }

       

        void DeserilazeObject(string xmlLOGIPAYHAND)
        {

            MyGenericResponseObj.Stage = "Initalize ProccessRequest";
            AppendLogLine("PayHandService.ProccessRequest");

            AppendLogLine("Deserialize(DataIn1) ..");


            if (string.IsNullOrWhiteSpace(xmlLOGIPAYHAND))
            {
                throw new BusinessErrorException("DataIn1 is missing");
            }
            if (xmlLOGIPAYHAND.Length > 1000)
            {
                AppendLogLine("XmlIn=" + xmlLOGIPAYHAND.Substring(0, 1000));
                AppendLogLine(".Substring(0, 1000)");
            }
            else
            {
                AppendLogLine("XmlIn=" + xmlLOGIPAYHAND);
            }


            AppendLogLine("Tring DeserilazeObject");
            MyGenericResponseObj.Stage = "Trying DeserilazeObject";
            this._LOGIPAYHAND = XmlGenericUtil<LOGIPAYHAND>.DeSerializeObject(xmlLOGIPAYHAND);

            if (_LOGIPAYHAND.LogitudePayHand == null || _LOGIPAYHAND.LogitudePayHand.Length != 1)
            {
                throw new BusinessErrorException("_LOGIPAYHAND.PayHand.Length != 1");
            }
            this._LogitudePayHand = _LOGIPAYHAND.LogitudePayHand[0];
        }

        private void CheckIntegrity()
        {
            MyGenericResponseObj.Stage = "Check integrity ";

            if (String.IsNullOrWhiteSpace(this._LogitudePayHand.logitude_file))
            {
                throw new BusinessErrorException("LOGITUDEFILE is missing");
            }
            AppendLogLine("LOGITUDE FILE = " + this._LogitudePayHand.logitude_file);
        }

        

        public override string GetAssemblyQualifiedName()
        {
            throw new NotImplementedException();
        }

        public override string GetExampleDataIn1()
        {
            return "";
        }

        public override string GetExampleDataIn2()
        {
            return "";
        }

        public override string GetExampleDataout1()
        {
            return "";
        }

        public override string GetExampleDataout2()
        {
            return "";
        }

        public override void ProccessRequest(string DataIn1, string DataIn2, out string DataOut1, out string DataOut2, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }

        public override void ProccessBASE64Request(string BASE64DataIn1, string BASE64DataIn2, string BASE64DataIn3, out string BASE64DataOut1, out string BASE64DataOut2, out string BASE64DataOut3, out string SUCCESS, ref string MoreParams, out string MessageOut)
        {
            throw new NotImplementedException();
        }


    }
}


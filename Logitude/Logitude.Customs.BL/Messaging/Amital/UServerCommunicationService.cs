using Logitude.AmitalMessaging.Infrastructure;
using Logitude.AmitalMessaging.Infrastructure.SystemTable;
using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using UnifreightIIG.UServer;

namespace Logitude.Customs.BL.Messaging.Amital
{
    public class UServerCommunicationService<CommunicationModelType, TransmissionBodyType>
        where TransmissionBodyType : class
        where CommunicationModelType : AmitalCommunicationModelBase
    {

        const string _From = "AMITAL"; //please note THE STARDART ANALYZE WANT AMITAL PARTNAER  "Logitude";
        protected CommunicationModelType _CommunicationModel = null;
        protected TransmissionBodyType _TransmissionBodyModel = null;
        private CommunicationsParams _CommunicationsParams;


        public UServerCommunicationService(CommunicationModelType communicationModel, TransmissionBodyType transmissionModel)
        {
            _CommunicationModel = communicationModel;
            _TransmissionBodyModel = transmissionModel;
        }


        public UServerCommunicationServiceInfoM Send(bool? pImmediately = null, bool SuppressBuildCom = false)
        {
            return Send(
                new UServerCommunicationServiceParam()
                {
                    SendImmediately = pImmediately,
                    SuppressBuildCom = SuppressBuildCom
                });
        }

        public UServerCommunicationServiceInfoM Send(UServerCommunicationServiceParam uServerCommunicationServiceParam)
        {
            //if (UnifreightIIGCommonUtil.GetTenantSetting(GetTenant()) == null)
            //{
            //    throw new Exception("SendFileToAmitalService():no setting for tenant");
            //}

            bool testDelay = false;
            if (testDelay)
            {
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }

            bool Immediately;
            if (uServerCommunicationServiceParam.SendImmediately == null)
            {
                Immediately = false;//Environment.UserDomainName.Equals("ntdomain", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                Immediately = uServerCommunicationServiceParam.SendImmediately.Value;
            }
            //if (!(Environment.UserDomainName.Equals("NTDOMAIN", StringComparison.OrdinalIgnoreCase) ||
            //    Environment.MachineName.Equals("IIGTest", StringComparison.OrdinalIgnoreCase) ||
            //    AuthenticationUtil.SystemIdentityName(GetTenant()) == AuthenticationUtil.ResolveUserIdentityName(GetTenant())
            //    ))
            //{

            //    Immediately = false;//Ramalah can not access ntdomain (amital network ) 
            //}

            var myInfo = new UServerCommunicationServiceInfoM();
            string validationErrors = GetValidationErrors();
            if (!String.IsNullOrWhiteSpace(validationErrors))
            {
                throw new Exception("validationErrors :" + validationErrors);
            }
            Build();




            byte[] myByteData = null;
            _CommunicationsParams = new CommunicationsParams()
            {

                Tenant = GetTenant(),//_PhysicalCheckPM.Tenant,

                LoggingObjectTableId = GetLoggingObjectTableId(),// objectTable.Id,
                LoggingEntityId = GetLoggingEntityId(),//_PhysicalCheckPM.Id,

                Subject = GetSubject(),// "FU Status",
                LoggingEntityReference = GetLoggingEntityReference(),//_PhysicalCheckPM.CheckId,
                LoggingUserId = GetLoggingUserId(),//loggingUserId,


                Status = "W",
                To = "Unifreight",
                CommunicationLogTypeCode = "T",
                FolderName = "Amital",
                From = _From, //"Logitude",
                InOut = "O",

                //XMLData=some xml data string 
            };
            var myMainObject = "";
            if (_CommunicationModel.UnifaceMethodType == Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.AnalyzeStandard)
            {
                var mytransmission = GetTransmission();
                myMainObject = XmlGenericUtil<transmission>.SerializeObject(mytransmission, true);
            }
            else
            {
                var dec = this._TransmissionBodyModel as Dictionary<string, string>;
                if (dec != null)
                {
                    myMainObject = UnifreightListsUtil.Serialize(dec);
                }
                else
                {
                    myMainObject = XmlGenericUtil<TransmissionBodyType>.SerializeObject(this._TransmissionBodyModel, true);
                }

                
            }
            //Debug.WriteLine(mySerilazeObject);
            var myUrouterParam = GetUrouterParams(myMainObject);
            var unifaceTester = TestIt(myUrouterParam, "");

            //LogMessagingUtil.Instance.AppendLine(unifaceTester);
            myByteData = Encoding.UTF8.GetBytes(myUrouterParam);
            _CommunicationsParams.ByteData = myByteData;
            if (Immediately)
            {
                string P_MESSAGE = "";
                string uniTester = "";
                var response = SendMessageToUServerUtil.SendMessageToUServer(GetTenant(), myUrouterParam, out P_MESSAGE, out uniTester);

                myInfo.GenericResponseObj = TryGetGenericResponseObj(response);
                myInfo.ImmediatelyResponse = response;
                myInfo.ImmediatelyMessage = P_MESSAGE;
                _CommunicationsParams.Logs = response;
                _CommunicationsParams.Status = "D";
            }
            if (Immediately && uServerCommunicationServiceParam.SuppressBuildCom)
            {

            }
            else
            {
                ///using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    myInfo.CommunicationLogId = Communications.AddCommunicationLog(_CommunicationsParams);
                    ///scope.Compl
                }
            }


            if (!Immediately)
            {
                var onTransactionCompleted = " Build  SendDataToExternalServicesBQ ";
                

                if (LogitudeSettings.QueueServiceMode != "db" && Transaction.Current != null)
                {
                    Transaction.Current.TransactionCompleted += (sender, e) =>
                    {
                        if (e.Transaction.TransactionInformation.Status == TransactionStatus.Committed)
                        {
                            onTransactionCompleted += " onTransactionCompleted";
                            Communications.
                                SendCommunicationLogMessageToQueue(
                                SBQueueNames.SendDataToExternalServicesBQ.ToString(),
                                myInfo.CommunicationLogId, _CommunicationsParams.Tenant,
                                null,
                                uServerCommunicationServiceParam.UServerDelayTime);
                        }
                    };
                }
                else
                {
                    Communications.
                            SendCommunicationLogMessageToQueue(
                            SBQueueNames.SendDataToExternalServicesBQ.ToString(),
                            myInfo.CommunicationLogId, _CommunicationsParams.Tenant,
                            null,
                            uServerCommunicationServiceParam.UServerDelayTime);
                }
                LogMessagingUtil.Instance.AppendLine("SendCommunicationLogMessageToQueue()")
                        .Append(onTransactionCompleted)
                        .Append(",CommunicationLogId:").Append(myInfo.CommunicationLogId)
                        .Append(",Tenant").Append(_CommunicationsParams.Tenant);
            }
            return myInfo;
        }

        private GenericResponseObj TryGetGenericResponseObj(string response)
        {
            if (String.IsNullOrWhiteSpace(response))
            {
                return null;
            }
            try
            {
                var GenericResponse = XmlGenericUtil<GenericResponse>.DeSerializeObject(response);
                
                var genericResponseObj = GenericResponse.GenericResponseObj.FirstOrDefault();
                if (!String.IsNullOrWhiteSpace(genericResponseObj.ErrorDescription))
                {
                    LogMessagingUtil.Instance.AppendLine("Urouter Response ErrorDescription:" + genericResponseObj.ErrorDescription);
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("Urouter Response Message:" + genericResponseObj.Message);
                }
                if (!string.IsNullOrWhiteSpace(genericResponseObj.CorrelationId))
                {
                    LogMessagingUtil.Instance.AppendLine("commId=genericResponseObj.CorrelationId=" + genericResponseObj.CorrelationId);    
                }
                return genericResponseObj ;
                 
            }
            catch (Exception )
            {
                
                
            }
            return null;
            
        }


 
        string GetUrouterParams(string mainXml)
        {
            var myParams = new Hashtable();
            myParams.Add("componentname", "GWSFLOGITUDE");
            myParams.Add("Operation", "AnalyzeStandard");
            myParams.Add("Subject", GetSubject());
            string unfreightUserId = ResolveUnfreightUserId();
            if (!String.IsNullOrWhiteSpace(unfreightUserId))
            {
                myParams.Add("$$GSC_USER_ID", unfreightUserId);
            }
            if (_CommunicationModel.UnifaceMethodType == AmitalStandardCommunicationModel.OperationMethod.DataAccess)
            {
                myParams["Operation"] = "DataAccess";
                myParams["GWSFLOGITUDE:componentname"] = _CommunicationModel.UnifaceComponentName;
                myParams["GWSFLOGITUDE:operation"] = _CommunicationModel.UnifaceOperation;
            }
            myParams["GWSFLOGITUDE:Xml"] = mainXml;
            string xmlIn = UnifreightListsUtil.Serialize(myParams);
            return xmlIn;

        }

        private string ResolveUnfreightUserId()
        {
            //return AuthenticationUtil.ResolveUnifreightUserId(GetTenant());
            string unifreightUser = null;
            if (RequestSheetContext.Current != null)
            {
                var loggingUserIdFromRS = RequestSheetContext.Current.GetContextOrDefault().GetUserFromRequestParam();
                if (!string.IsNullOrWhiteSpace(loggingUserIdFromRS))
                {
                    UserRepository userRep = new UserRepository(GetTenant());
                    User user = userRep.GetSingleUser(loggingUserIdFromRS, GetTenant(), true);
                    if (user != null)
                    {
                        if (!String.IsNullOrWhiteSpace(user.Code))
                        {
                            unifreightUser = user.Code;
                        }
                    }
                }
            }
            if (String.IsNullOrWhiteSpace(unifreightUser))
            {
                unifreightUser = AuthenticationUtil.ResolveUnifreightUserId(GetTenant());
            }
            return unifreightUser;
            ///throw new NotImplementedException();
            //return null;
        }

        public static string TestIt(string xmlIn, string P_MOREPARAMS)
        {
            string out1 =
            @"
 variables
	string p_xml_in 
	string p_xml_Data 
	string p_MoreParams
	string p_Message 
endvariables
DEBUG
p_xml_in =$MyXml
p_MoreParams =$MoreParams
activate ""GWSFINSRVEXE"".DoIt(p_xml_in ,p_xml_Data ,p_MoreParams ,p_Message )
MyXml:blockdata ~{0}~
MoreParams:blockdata ~{1}~
 ";
            return string.Format(out1, xmlIn, P_MOREPARAMS);
        }

        protected virtual string GetValidationErrors() { return null; }
        protected virtual void Build() { }
        ///protected abstract List<data> GetDataList();


        //protected abstract string GetSubject();
        //protected abstract string GetLoggingUserId();
        //protected abstract string GetLoggingEntityReference();
        //protected abstract string GetLoggingEntityId();
        //protected abstract int GetTenant();
        //protected abstract string GetLoggingObjectTableId();


        protected virtual string GetSubject()
        {
            return _CommunicationModel.CommunicationSubject;
        }

        protected virtual string GetLoggingUserId()
        {
            return _CommunicationModel.UserId;
        }

        protected virtual string GetLoggingEntityReference()
        {
            return _CommunicationModel.CommunicationLoggingEntityReference;
        }

        protected virtual string GetLoggingEntityId()
        {
            return _CommunicationModel.EntityId;
        }

        protected virtual int GetTenant()
        {
            return _CommunicationModel.Tenant;
        }

        protected virtual string GetLoggingObjectTableId()
        {
            if (String.IsNullOrWhiteSpace(_CommunicationModel.objectTableName)) return "";//not must 
            var objectTableRepository = new ObjectTableRepository(0); // ObjectTabelRepository tenant must be zero !!
            var objectTable = objectTableRepository.GetObjectTableByName(_CommunicationModel.objectTableName,// "Customs.PhysicalCheck", 
                0, true);

            return objectTable.Id;
        }

        transmission GetTransmission()
        {

            var CommunicationsParamsSubject = _CommunicationsParams.Subject;
            var mytransmission = new transmission();
            var mytransmission_details = new List<transmission_details>();
            var mytransmission_detail1 = new transmission_details()
            {
                sender = new sender() { Value = _From },
                subject = new subject() { Value = CommunicationsParamsSubject },    
            };
            if (_CommunicationModel.special_instruction != null)
            {
                mytransmission_detail1
                    .special_instructions = new special_instructions[] { _CommunicationModel.special_instruction };
            }

            var mySerilazeObject = XmlGenericUtil<TransmissionBodyType>.SerializeObject(this._TransmissionBodyModel, true);

           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(mySerilazeObject);



            var myListdata = new List<data>() { new data() { entity = mySerilazeObject } };

            mytransmission.data = myListdata.ToArray();// GetDataList().ToArray();
            if (mytransmission.data.Count() < 1)
            {
                throw new Exception("(mytransmission.data.Count < 1)");
            }

            mytransmission_details.Add(mytransmission_detail1);
            mytransmission.transmission_details = mytransmission_details.ToArray();
            return mytransmission;

        }

    }
    public class UServerCommunication
    {
        public static UServerCommunicationServiceInfoM? SendUpdateTableToUnifreight(
            int curTenant, string TableID, CUSTOMS_TABLE myCUSTOMS_TABLE,
            bool? pImmediately = null,
            bool pForceSendUnfConnection = false
            )
        {
            var mySetting = Logitude.Customs.BL.EntityQueryServices.CustomsSettingQueryService.GetSettingByTenant(curTenant);
            if (pForceSendUnfConnection)//יתקים רק בעדכון טבלאות מכס מתוך הרשימה
            {
                if (String.IsNullOrWhiteSpace(mySetting.UnfConnectionString)) return null;
            }
          

            bool immediately = false;
            var myAmitalCommunicationModel = new AmitalCommunicationModelBase(
                Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess, "YTBFLOGITABLE", "UpdateTable")
            {
                Tenant = curTenant,
                objectTableName = null,


                //CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                //EntityId = dirtyDeclarationPM.Id,
                //UserId = loggingUserId,
                CommunicationSubject = "IIG Table ( " + TableID + " ) arrived from Logitude ",

            };

            if (pImmediately.HasValue)
            {
                immediately = pImmediately.Value;
            }
            var myUServerCommunicationService = new UServerCommunicationService<AmitalCommunicationModelBase, CUSTOMS_TABLE>(myAmitalCommunicationModel, myCUSTOMS_TABLE);
            var res=myUServerCommunicationService.Send(immediately);
            return res;
        }

    }


    public struct UServerCommunicationServiceInfoM
    {
        public string CommunicationLogId;

        public string ImmediatelyMessage { get; set; }

        public string ImmediatelyResponse { get; set; }

        public GenericResponseObj GenericResponseObj { get; set; }
    }

    public class UServerCommunicationServiceParam
    {
        public bool? SendImmediately { get; set; }
        public bool SuppressBuildCom { get; internal set; }
        public TimeSpan UServerDelayTime { get; set; }
    }

}

using Logitude.AmitalMessaging.Infrastructure.Transmission;
using Logitude.AmitalMessaging.Utils;

using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Logitude.Customs.BL.Messaging.Amital
{
    
    public class UServerQService<CommunicationModelType, TransmissionBodyType>
        where TransmissionBodyType : class
        where CommunicationModelType : AmitalCommunicationModelBase
    {

        const string _From = //"AMITAL"; 
            "IIGC";
        protected CommunicationModelType _CommunicationModel = null;
        protected TransmissionBodyType _TransmissionBodyModel = null;
        private CommunicationsParams _CommunicationsParams;


        public UServerQService(CommunicationModelType communicationModel, TransmissionBodyType transmissionModel)
        {
            _CommunicationModel = communicationModel;
            _TransmissionBodyModel = transmissionModel;
        }

        public struct Info
        {
            public string CommunicationLogId;

            public string ImmediatelyMessage { get; set; }

            public string ImmediatelyResponse { get; set; }
        }


        public Info Send(bool Immediately = false)
        {
            var myInfo = new Info();
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
                myMainObject = XmlGenericUtil<TransmissionBodyType>.SerializeObject(this._TransmissionBodyModel, true);
            }
            //Debug.WriteLine(mySerilazeObject);
            var myUrouterParam = GetUrouterParams(myMainObject);
            var unifaceTester = TestIt(myUrouterParam, "");
            //LogMessagingUtil.Instance.AppendLine(unifaceTester);
            myByteData = Encoding.UTF8.GetBytes(myUrouterParam);
            _CommunicationsParams.ByteData = myByteData;
            string communicationLogId = Communications.AddCommunicationLog(_CommunicationsParams);
            if (Immediately)
            {
                string xmlResponse = "";
                var response = SendMessageToUServer(myUrouterParam, communicationLogId, GetTenant(), out xmlResponse);
                myInfo.ImmediatelyResponse = response;
                myInfo.ImmediatelyMessage = xmlResponse;

                return myInfo;
            }

            Communications.SendCommunicationLogMessageToQueue(SBQueueNames.SendDataToExternalServicesBQ.ToString(), communicationLogId, _CommunicationsParams.Tenant
                ///,(AmitalStandardCommunicationModel)this._CommunicationModel
                );
            myInfo.CommunicationLogId = communicationLogId;
            return myInfo;
        }
        private string SendMessageToUServer(string urouterRequest, string communicationLogId, int tenant, out string xmlResponse)
        {
            //implement the code to send the xml file to amital;

            
            xmlResponse = "";
            if (String.IsNullOrWhiteSpace(urouterRequest))
            {
                throw new ArgumentNullException("SendFileToAmitalService():xmlfile is null");
            }
            string responseCommunicationLogId = "";
            var myUServerQueueManager = new UServerQueueManager();
            myUServerQueueManager.SendSync(communicationLogId, tenant, out responseCommunicationLogId);
            var responseComm = Communications.GetCommunicationLog(tenant, responseCommunicationLogId);
            xmlResponse = Communications.GetData(responseComm); ;

            //var myUServerUtil = new UServerUtil(myUServerDNS, myUServerPort); ;
            //myUServerUtil.DoIt(urouterRequest, ref P_MOREPARAMS, out P_XML_DATA, out P_MESSAGE);
            return xmlResponse;
        }
        string GetUrouterParams(string mainXml)
        {
            var myParams = new Hashtable();
            myParams.Add("componentname", "GWSFLOGITUDE");
            myParams.Add("Operation", "AnalyzeStandard");

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
                subject = new subject() { Value = CommunicationsParamsSubject }
            };


            var mySerilazeObject = XmlGenericUtil<TransmissionBodyType>.SerializeObject(this._TransmissionBodyModel, true);

            Debug.WriteLine(mySerilazeObject);



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
}

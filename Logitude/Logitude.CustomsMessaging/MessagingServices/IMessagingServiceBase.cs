using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.Messaging.Customs;
using System;
using UnifreightIIG.Common.Utils;
namespace Logitude.CustomsMessaging.MessagingServices
{
    public interface IMessagingServiceBase<TRequestParams, TResponseData, TCustomsRequest, TCustomsResponse, TRequestHeader>
        :IMessagingServiceInterfaceType
        where TCustomsRequest : class ,new()
        where TRequestParams : Logitude.CustomsMessaging.Common.RequestParams.RequestParamsBase
        where TResponseData : Logitude.CustomsMessaging.Common.ResponseData.ResponseDataBase, new()
    {
        TResponseData Send(TRequestParams requestParams, TCustomsRequest customsRequestCalc = default(TCustomsRequest) );

        //string CreateSBQMessage(TRequestParams requestParams);
        //TResponseData Send(TRequestParams requestParams);
        //void Update(TCustomsResponse customsResponse, TRequestParams requestParams);
        ///string TestSendXml(string XmlIn, out string exceptionMessage);
    }
    public interface  IMessagingBatchService
    {

        object CustomsRequestCalc { get; set; } //TCustomsRequest

        bool ViaDCA { get; set; }

        string CustomsRequestXml { get; set; }

        byte[] CustomRequestSignedByteArry { get; set; }

        string CustomsResponseXml { get; set; }

        

        string LogMessaging { get; set; }

        string ResponseDataXml { get; set; }

        void Save(bool success);
        bool IsSuccess();
    }
    enum CustomStepEnum
    {
        
    }
    public class MessagingBatchService : IMessagingBatchService
    {
        string _ref;
        bool _success;
        private MessagingBatchService() { }
        private MessagingBatchService(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new Exception("reference is missing !!"); 
            }
            _ref = reference;
        }
        public object CustomsRequestCalc { get; set; } //TCustomsRequest

        public bool ViaDCA { get; set; }

        public string CustomsRequestXml { get; set; }

        public byte[] CustomRequestSignedByteArry { get; set; }

        public string CustomsResponseXml { get; set; }
        
        public string ResponseDataXml { get; set; }

        public string LogMessaging { get; set; }

        
        public void Save(bool success)
        {
            _success = success;
            var xml = XmlGenericUtil<MessagingBatchService>.SerializeObject(this);
            var p = System.IO.Path.Combine( @"E:\Temp\CustomQueue",_ref);
            System.IO.File.WriteAllText(p, xml);
        }
        public bool IsSuccess()
        {
            return _success;
        }
        public static MessagingBatchService TryGet(string reference)
        {
            var p = System.IO.Path.Combine(@"E:\Temp\CustomQueue", reference);
            if (System.IO.File.Exists(p))
            {
                var xml = System.IO.File.ReadAllText(p);
                var messagingBatchService = XmlGenericUtil<MessagingBatchService>.DeSerializeObject(xml);
                return messagingBatchService;
            }
            return new MessagingBatchService(reference); 
        }



       
    }
    #if false
    public interface IMessagingServiceInterfaceType
    {

#if false
        void SendBatchStateMachine(int tenant, string correlationId, ref CustomsStepEnum processState); ///,CancellationToken ct) 
#endif
        Nullable<CustomsCommandEnum> CurrentCustomsCommandWR { get; set; }

        string TestSendXml(string customXmlRequest, out string exceptionMessage);


        object SendSheet(int tenant, string customsRequestsSheetId);

        //string DcaReceivedCustomResponseCorrelationCrashIfNotValid(Logitude.Customs.BL.EntityPMs.InterfaceManagementPM messageDCA, int tenant, string selectedFile, byte[] messageBytes, bool pseudo = false);
        string DcaReceivedCustomResponseCorrelation(Logitude.Customs.BL.EntityPMs.InterfaceManagementPM messageDCA, int tenant,
            string selectedFile, string fileContents //byte[] messageBytesbyte[] messageBytes
            , bool pseudo=false);
    }    
#endif
}

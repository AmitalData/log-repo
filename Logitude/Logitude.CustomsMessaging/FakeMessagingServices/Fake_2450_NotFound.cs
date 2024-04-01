using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.Customs.Data.Repsitories;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using UnifreightIIG.Common.ContainerizationMessageServiceReference;
using UnifreightIIG.Common.MessageLib.Unifreight.Customs;
using static System.Net.WebRequestMethods;
using Exception = UnifreightIIG.Common.ContainerizationMessageServiceReference.Exception;
namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_2450_NotFound
    {
        public ResponseHeader _ResponseHeader;
        private GenericRequestParams _requestParams;
        public INF_MSG_Generic fakeRespond;

        //private DF_NG_2450_Web02_ContainerizationStatus_ResponseContainerizationStatusAnswer[] _ContainerizationStatusAnswer;
        public Fake_2450_NotFound(GenericRequestParams requestParams)
        {
            _requestParams = requestParams;
            fakeRespond = new INF_MSG_Generic();
            _ResponseHeader = new ResponseHeader();
        }
        public ResponseHeader CallWS(GenericRequestParams requestParams, out INF_MSG_Generic response)
        {
            fakeRespond.ResponseContentHeader = new ResponseContentHeader();
            fakeRespond.ResponseContentHeader.TransmitionDateTime = DateTime.Now;
            fakeRespond.ResponseContentHeader.ApplicationID = 0;
            fakeRespond.ResponseContentHeader.Exception = new Exception[1];
            fakeRespond.ResponseContentHeader.Exception[0] = new Exception();
            fakeRespond.ResponseContentHeader.Exception[0].ExceptionLevel = 3;
            fakeRespond.ResponseContentHeader.Exception[0].ExeptionType = 14059;
            fakeRespond.ResponseContentHeader.Exception[0].ExceptionParms = new string[] {"DeclarationID: 23042671903868"};
            fakeRespond.ResponseContentHeader.Exception[0].ExeptionDescription = "הצהרה DeclarationID: 23042671903868 ששודרה לא קיימת או לא שייכת למטען הממכיל";
            fakeRespond.ResponseContentHeader.Exception[0].EnglishDescription = "DeclarationNotExistsOrNotInTheMainCargo the parameters in 'ExceptionParms' section";
         
            
            response = fakeRespond; 
            return _ResponseHeader;
        }
    }
}

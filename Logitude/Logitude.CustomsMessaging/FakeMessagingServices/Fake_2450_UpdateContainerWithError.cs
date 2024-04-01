using Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Web.UI.WebControls;
using UnifreightIIG.Common.ContainerizationMessageServiceReference;
using Exception = UnifreightIIG.Common.ContainerizationMessageServiceReference.Exception;
namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_2450_UpdateContainerWithError
    {
        public ResponseHeader _ResponseHeader;
        private GenericRequestParams _requestParams;
        public INF_MSG_Generic fakeRespond;

        //private DF_NG_2450_Web02_ContainerizationStatus_ResponseContainerizationStatusAnswer[] _ContainerizationStatusAnswer;
        public Fake_2450_UpdateContainerWithError(GenericRequestParams requestParams)
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
            response = fakeRespond; 
            return _ResponseHeader;
        }
    }
}

using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.ResponseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.DeclarationStatusQueryRequestServiceReference;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_2450_Containerization_RequestMessagingService
    {
        private ResponseContentHeader _responseContentHeader;
        private DF_NG_2450_Web02_ContainerizationStatus_ResponseContainerizationStatusAnswer[] _ContainerizationStatusAnswer;
        
        internal SaveCC_MSG2450_ContainerizationMessageResponseService GetFakeCustomsResponse(GenericRequestParams requestParamsData)
        {
            SetResponseContentHeader();
            return new SaveCC_MSG2450_ContainerizationMessageResponseService()
            {
                ResponseContentHeader = _responseContentHeader
                
            };
        }

            
        private void SetResponseContentHeader()
        {
            _responseContentHeader = new ResponseContentHeader()
            {
                TransmitionDateTime=DateTime.Now,
                ApplicationID = 1234,
                Remark=""
            };


            if (param1 == true)
            {
                _responseContentHeader.Exception = new Exception[0];
                _responseContentHeader.Exception[0].ExceptionLevel="5",



            }
        }
    }
}

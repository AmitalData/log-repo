using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.RequestServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImportDeclarationServiceReference;
using Exception = UnifreightIIG.Common.ImportDeclarationServiceReference.Exception;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake_2754_MSG10004_ImportDeclarationResponse : Fake_ImportDeclaration_Response
    {
        public Fake_2754_MSG10004_ImportDeclarationResponse(GenericRequestParams requestParams) : base(requestParams) { }
      
        public ResponseHeader CallWS(GenericRequestParams requestParams,out DF_NG_2754_MSG10004_ImportDeclarationResponse response)
        {
            UpdateDeclaration(requestParams);
            UpdateStatus("13");
            response = fakeRespond;
            UpdateFakeResponseContentHeader();
            AddSign();
            AddResponseHeader();
            return _ResponseHeader;

        }



    }
}

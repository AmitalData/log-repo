using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImportDeclarationServiceReference;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    class Fake_2754_MSG10004_ImportDeclarationResponseWithConstraint : Fake_ImportDeclaration_Response
    {
        public Fake_2754_MSG10004_ImportDeclarationResponseWithConstraint(GenericRequestParams requestParams) : base(requestParams) { }

        public ResponseHeader CallWS(out DF_NG_2754_MSG10004_ImportDeclarationResponse response)
        {
            UpdateDeclaration();
            UpdateStatus("12");
            UpdateFakeResponseContentHeader();
            AddSign();
            AddResponseHeader();
            AddConstraints();
            response = fakeRespond;
            return _ResponseHeader;

        }

    }
}

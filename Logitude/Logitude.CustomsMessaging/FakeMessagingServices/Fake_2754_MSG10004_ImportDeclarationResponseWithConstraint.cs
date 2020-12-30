using Logitude.CustomsMessaging.Common.RequestParams;
using Newtonsoft.Json.Linq;
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

        public ResponseHeader CallWS(GenericRequestParams requestParams,out DF_NG_2754_MSG10004_ImportDeclarationResponse response)
        {
            UpdateDeclaration(requestParams);
            UpdateStatus("12");
            UpdateFakeResponseContentHeader();
            AddSign();
            AddResponseHeader();
            dynamic params1 = JObject.Parse(requestParams.TestCase.Param1);
            string code = Convert.ToString(params1.code);
            string documentSectionCode = Convert.ToString(params1.documentSectionCode);
            string tagId = Convert.ToString(params1.tagId);

            if (code != null) {
                AddErrors(code, tagId, documentSectionCode);
            } else
            {
                AddConstraints();
            }
            response = fakeRespond;
            return _ResponseHeader;

        }

    }
}

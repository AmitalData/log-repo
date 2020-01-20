using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ImportDeclarationServiceReference;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    public class Fake1DF_NG_2754_MSG10004_ImportDeclarationResponse :FakeResponseBase
    {
        public DF_NG_2754_MSG10004_ImportDeclarationResponse CallWS(
            DF_MSG10000_ImportDeclaration customRequest,
            GenericRequestParams requestParams,
            out string exceptionMessage)
        {
            exceptionMessage = "";
            var response = new DF_NG_2754_MSG10004_ImportDeclarationResponse()
            {
                Response = new Response()
                {
                }
            };
            switch (requestParams.TestCase.Code)
            {
                case "ConstraintA":
                    {

                    }
                    break;
                default:
                    break;
            }
            return response;

        }

    }
}

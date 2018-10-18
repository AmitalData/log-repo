using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.RequestServices
{
    public  class DCAInCustomRequestService
        : RequestServiceBase<DCAInCustomRequest, GenericRequestParams>
    {
        public override DCAInCustomRequest GetRequest(GenericRequestParams requestParams)
        {
            throw new NotImplementedException();
        }
    }
    public class DCAInRequiredDocumentCustomRequestService
        : RequestServiceBase<DCAInCustomRequest, RequiredDocumentRequestParams>
    {
        public override DCAInCustomRequest GetRequest(RequiredDocumentRequestParams requestParams)
        {
            throw new NotImplementedException();
        }
    }
    [XmlRoot(Namespace = "http://amital.com/customs/Prod/LOGISIVUGWithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/LOGISIVUGWithResponseContentHeader")]

    public class DCAInCustomRequest
    {
    }

    public class DCAInCustomReturnNullRequestService : RequestServiceBase<SYSTBL_NG_9000_MSG_SystemTableRequest, GenericRequestParams>
    {
        public override SYSTBL_NG_9000_MSG_SystemTableRequest GetRequest(GenericRequestParams requestParams)
        {

            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam()
            {
                RequestDescription = requestParams.RequestName
            };
            return new SYSTBL_NG_9000_MSG_SystemTableRequest() { };
        }
    }
}

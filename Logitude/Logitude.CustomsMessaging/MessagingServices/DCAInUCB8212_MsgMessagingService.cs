using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.SystemTableServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    class DCAInUCB8212_MsgMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCAInUCB8212WithResponseContentHeader,
        DCAInCustomReturnNullRequestService,
        UniCourierBatchSend2755_MsgResponseService, RequestHeader>
    {
    }

    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCB2755WithResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCB2755WithResponseContentHeader")]
    public class DCAInUCB8212WithResponseContentHeader : IINF_MSG_Generic
    {
    }
    }

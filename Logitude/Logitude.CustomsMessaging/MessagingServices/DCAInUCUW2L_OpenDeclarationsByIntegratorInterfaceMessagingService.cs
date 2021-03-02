using Logitude.AmitalMessaging.Customs.CustomFile.CommDecFile;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.MessageLib.Ransom;
 using UnifreightIIG.Common.SystemTableServiceReference;


namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceMessagingService : MessagingServiceBase<
        GenericRequestParams,
        INF_MSG_GenericResponseData,
        SYSTBL_NG_9000_MSG_SystemTableRequest,
        DCAInUCUW2LResponseContentHeader,
        DCAInCustomReturnNullRequestService,
        DCAInUCUW2L_OpenDeclarationsByIntegratorInterfaceResponseService, RequestHeader>
    {

        protected override GenericRequestParams CreateDefaultRequestParamsFromCustomsResponse(DCAInUCUW2LResponseContentHeader customsResponse)
        {
            var myGenericRequestParams = new GenericRequestParams()
            {
                LoggingObjectTableId = ObjectTableRepository.GetObjectTableByName("Customs.CustomsDocument"),
                  LoggingEntityId = customsResponse.CustomFileNo
            };
            return myGenericRequestParams;

        }

        protected override DCAInUCUW2LResponseContentHeader CallWS(SYSTBL_NG_9000_MSG_SystemTableRequest customRequest, GenericRequestParams requestParams, out string exceptionMessage)
        {
            throw new NotImplementedException();
        }

        public override string MainInterfaceCode
        {
            get { return "UCUW2L"; }
        }



    

    }

    [XmlRoot(Namespace = "http://amital.com/customs/Prod/DCAInUCUW2LResponseContentHeader", IsNullable = false)]
    [XmlType(AnonymousType = true, Namespace = "http://amital.com/customs/Prod/DCAInUCUW2LResponseContentHeader")]
    public class DCAInUCUW2LResponseContentHeader : IINF_MSG_Generic
    {

        public IResponseContentHeader GetResponseContentHeader()
        {

            return ResponseContentHeader;
        }
        public Logitude.CustomsMessaging.Testers.Messages.DefaultResponseContentHeader ResponseContentHeader { get; set; }

        public int tenant { get; set; }
        public string LoggingUserId { get; set; }
         public string CustomFileNo { get; set; }
        public string LOGICOMMDEC { get; set; }
        public string MoreParams { get; set; }
        //public string master { get; set; }

        //public LOGIDOCS MyLOGIDOCS { get; set; }

        //public string MyMoreParams { get; set; }
        //public string DocumentsFilingCode { get; set; }
        //public string DocumentsFilingId { get; set; }
        //public string DOCUMENTTYPEID { get; set; }
        //public string DocumentTypeCode { get; set; }

        //public string DocumentTypeId { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }

}

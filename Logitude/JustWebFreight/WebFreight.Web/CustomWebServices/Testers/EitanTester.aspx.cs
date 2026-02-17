using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.RequestServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UnifreightIIG.Common.ImportDeclarationServiceReference;

namespace WebFreight.Web.CustomWebServices.Testers
{
    public partial class EitanTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool todo = false;

            if (todo)
            {
                var myDeclarationQueryService = new DeclarationQueryService(1);
                var myDeclaration = myDeclarationQueryService.GetSingle("1-204546", true, false);
                if(myDeclaration != null && myDeclaration.Consignments != null && myDeclaration.Consignments.Count() > 0)
                {
                    var myCustomsDocumentsDefinitionQueryService = new CustomsDocumentsDefinitionQueryService(1);
                    var myCustomsDocumentsDefinition = myCustomsDocumentsDefinitionQueryService.GetCustomsDocumentsDefinitionsForDeclaration(myDeclaration.Consignments[0].CargoTypeCode, myDeclaration.ProcedureCurrentCode, myDeclaration.TransportModeId,1);
                }
                
            }

            if (todo)
            {
                var mySupplierInvoiceQueryService = new SupplierInvoiceQueryService(1);
                var si = mySupplierInvoiceQueryService.GetSingle("1-104145", 11, true, false);
                var siI = si.SupplierInvoiceItems.FirstOrDefault(rec => rec.LineNumber == 2);
                var xml = XmlGenericUtil<SupplierInvoiceItemPM>.SerializeObject(siI);
                var siNew = XmlGenericUtil<SupplierInvoiceItemPM>.DeSerializeObject(xml);
            }

            if (todo)
            {
                //var req = new DF_MSG10000_ImportDeclaration();
                string id = "1-104350"; // "1 -105979"; //"1-104212"
                string dec = "17041014839656"; // "17021020824601"; //"17021010920203"
                id = "1-106723";
                dec = "17021022155988";
                id = "1-107582";
                dec = "17021022197253";
                
                GenericRequestParams requestParams = new GenericRequestParams { AppicationId = id, Tenant = 1, LoggingEnabled = true, LoggingEntityReference = dec, LoggingObjectTableId = "1-343",
                    LoggingEntityId = id, LoggingUserId = "1-27", IsFakeResponse = false, RequestName = "Declaration Request", ResponseName = "Declaration Response",
                    InterfaceTypeCode = "2750", SuppressSplitWR = false, ForcePersonalSign = false, IsAngularClient = true, 
                };
                var myDF_MSG10000_ImportDeclarationRequestService = new DF_MSG10000_ImportDeclarationRequestService();
                //req = myDF_MSG10000_ImportDeclarationRequestService.GetRequest(requestParams);


                INF_MSG_GenericResponseData responseData = new INF_MSG_GenericResponseData();
                var messagingService = new DF_MSG10000_ImportDeclarationMessagingService();
                responseData = messagingService.Send(requestParams);
            }

            if (todo)
            {
                CargoQueryRequestParams requestParams = new CargoQueryRequestParams
                {
                    Tenant = 1,
                    LoggingEnabled = true,
                    LoggingObjectTableId = "1-343",
                    CustomsFile = "172900818",
                    DeclarationNumber = "17021021041551",
                    LoggingUserId = "1-27",
                    IsFakeResponse = false,
                    RequestName = "Manifest Status Query",
                    ResponseName = "Manifest Status Query",
                    InterfaceTypeCode = "8240",
                    SuppressSplitWR = false,
                    ForcePersonalSign = false,
                    IsAngularClient = true,
                    CargoTypeCode = "11",
                    ManifestNumber = "174957",
                    SecondCargoID = "I064985A03",
                    DeclarationId = "1-106067",
                };

                CargoQueryResponseData responseData = new CargoQueryResponseData();
                var messagingService = new MN_NG_8240_CargoQueryMessagingService();
                responseData = messagingService.Send(requestParams);
            }
        }
    }
}
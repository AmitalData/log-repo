
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.RequestServices;
using Logitude.CustomsMessaging.ResponseServices;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.TheGateway;
using Logitude.CustomsMessaging.FakeMessagingServices;
using System.Xml.Serialization;
//using UnifreightIIG.Common.ExportDeclarationServiceReference;
using UnifreightIIG.Common.RetrieveExportOrTransshipmentDeclarationServiceReference;

namespace Logitude.CustomsMessaging.MessagingServices
{
    public class DF_NG_9079_Web05_RetrieveExportOrTransshipmentDeclarationMessagingService
        : MessagingServiceBase<
        DeclarationRestoreRequestParams, DeclarationRestoreResponseData,
        DF_NG_9079_Web05_RetrieveExportOrTransshipmentDeclaration_Request, DF_NG_2757_MSG10004_ExportDeclarationResponse,
        DF_NG_9079_Web05_RetrieveExportOrTransshipmentDeclarationRequestService, DF_NG_2757_MSG10004_RetrieveExportDeclarationResponseService,
        RequestHeader>
    {
        public override string MainInterfaceCode
        {
            get { return "9079"; }
        }

        protected override DeclarationRestoreResponseData GetIIGBLExceptionFromReponseHeader(DF_NG_2757_MSG10004_ExportDeclarationResponse customsResponse)
        {
            try
            {
                var responseContentHeader = customsResponse.ResponseContentHeader as IResponseContentHeader;
                ThrowIIGBLException(_ResponseHeader, responseContentHeader);

            }
            catch (System.ServiceModel.FaultException<UnifreightIIGFault> myUnifreightIIGFault)
            {
                var defaultMessage = "Sending request to IIG Server Failed ";
                var FormattedMessage = UnifreightIIG.Common.Utils.ErrorHandlerUtil.CreateNew().ToFormattedMessage(myUnifreightIIGFault);
                if (String.IsNullOrWhiteSpace(FormattedMessage))
                {
                    FormattedMessage = defaultMessage;
                }
                LogMessagingUtil.Instance.AppendLine(FormattedMessage);

                switch (myUnifreightIIGFault.Detail.PlaceFault)
                {

                    case UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError:
                    case UnifreightIIGFault.PlaceFaultEnum.IIGFatalException:
                    case UnifreightIIGFault.PlaceFaultEnum.IIGTechnicalError:
                        {
                            LogMessagingUtil.Instance.AppendLine("DF_NG_9079_Web05_RetrieveExportDeclarationMessagingService: " + myUnifreightIIGFault.Detail.PlaceFault);
                            if (myUnifreightIIGFault.Detail.PlaceFault == UnifreightIIGFault.PlaceFaultEnum.IIGBusinessError)
                            {
                                //_ResponseHeader.ErrorDescription = FormattedMessage;
                                //(_ResponseService as DF_NG_2754_MSG10004_RetrieveExportDeclarationResponseService)._ResponseHeaderExeption = _ResponseHeader;
                            }
                        }
                        break;
                    default:
                        return new DeclarationRestoreResponseData() { UserMessage = FormattedMessage, HasException = true, ApplicationID = customsResponse.ResponseContentHeader.ApplicationID.ToString() };
                }
            }

            return null;
        }
        public UnifreightIIG.Common.RetrieveExportOrTransshipmentDeclarationServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse CastMsg(UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse customResponse)
        {


            string DeclarationString;
            using (var stringwriter = new System.IO.StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

              //  ns.Add("q", "http://malam.com/customs/DealFile/Declaration/DF_MSG10000_ExportDeclaration");
                var serializer = new XmlSerializer(customResponse.GetType());
                serializer.Serialize(stringwriter, customResponse, namespaces: ns);
                DeclarationString = stringwriter.ToString();
            }
            //DeclarationString = DeclarationString.Replace("xmlns:q", "xmlns");
            //DeclarationString = DeclarationString.Replace("<q:", "<");
            //DeclarationString = DeclarationString.Replace("</q:", "</");

            using (var stringReader = new System.IO.StringReader(DeclarationString))
            {
                var serializer = new XmlSerializer(typeof(UnifreightIIG.Common.RetrieveExportOrTransshipmentDeclarationServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse));

                try
                {
                    return serializer.Deserialize(stringReader) as UnifreightIIG.Common.RetrieveExportOrTransshipmentDeclarationServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse;


                }
                catch (System.Exception ex)
                {
                    return null;
                }

            }


        }

        protected override DF_NG_2757_MSG10004_ExportDeclarationResponse CallWS(DF_NG_9079_Web05_RetrieveExportOrTransshipmentDeclaration_Request customRequest, DeclarationRestoreRequestParams requestParams, out string exceptionMessage)
        {
            exceptionMessage = null;
            var response = new DF_NG_2757_MSG10004_ExportDeclarationResponse();
            var response1 = new UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2757_MSG10004_ExportDeclarationResponse();

            //if (requestParams.TestCase != null)
            //{

            //    var Fake  = new Fake_2754_MSG10004_ExportDeclarationResponse(requestParams);
            //    _ResponseHeader = Fake.CallWS(requestParams, out response1);
            //    response = CastMsg(response1);
            //    return response;
            //}

            // var mP = new UnifreightIIG.Common.TheGateway.MoreParams() { MyOption = UnifreightIIG.Common.TheGateway.MoreParams.Options.None };

            using (var uifreightSdkGateway = new UnifreightSdkGateway(base.CustomsSetting.IIGServiceAddress))
            {
                _ResponseHeader = uifreightSdkGateway.GetChannel<IRetrieveExportOrTransshipmentDeclarationOperation>()
                    .RetrieveExportOrTransshipmentDeclaration(
                    this.RequestsSheetExternalId,
                    base.CustomsSetting.CustomsAgentId,
                    customRequest,
                    ref this._IIGGatewayMoreParams,
                    out response);
            }

            return response;
        }
    }
}

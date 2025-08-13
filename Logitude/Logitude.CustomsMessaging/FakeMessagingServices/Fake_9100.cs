using System;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Customs.BL.EntityQueryServices;
using Newtonsoft.Json.Linq;
using UnifreightIIG.Common.CommonIIGInterface;
using UnifreightIIG.Common.OutgoingMessageRequestServiceReference;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Logitude.CustomsMessaging.FakeMessagingServices
{
    internal sealed class Fake_9100
    {
        private readonly MessageWaitingRequestParams _rp;

        public Fake_9100(MessageWaitingRequestParams rp) => _rp = rp;

        public (NG_9101_MSG_OutgoingMessageResponse response, IResponseHeaderOrFault header, string exceptionMessage) CallWS()
        {
            string serviceName =null;
            string fileName = null;
            string innerXml = null;

            var raw = _rp?.TestCase?.Param1;
            if (!string.IsNullOrWhiteSpace(raw))
            {
                var parts = ParseParam1(raw);
                if (parts.msg != null)
                {
                    innerXml = parts.msg;       
                    serviceName = parts.svc ?? serviceName;
                    fileName = parts.file;
                }
            }

            string externalId;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var nameNoExt = System.IO.Path.GetFileNameWithoutExtension(fileName);   // strip .xml
                nameNoExt = System.IO.Path.GetFileNameWithoutExtension(nameNoExt);  // strip .PRD
                var lastDot = nameNoExt.LastIndexOf('.');
                var candidate = lastDot >= 0 ? nameNoExt.Substring(lastDot + 1) : null;

                externalId = Guid.TryParse(candidate, out var g) ? g.ToString() : Guid.NewGuid().ToString();
            }
            else
            {
                externalId = Guid.NewGuid().ToString();
                fileName = $@"\\gen-prd-cg31\Out\Tehila\CustomsB_2_Amerford-Cargo\{serviceName}_Out.{externalId}.xml";
            }

            var tenant = _rp.Tenant;
            var setting = CustomsSettingQueryService.GetSettingByTenant(tenant);
            var consumerId = setting?.CustomsAgentId?.Length <= 9 ? setting.CustomsAgentId : null;

            var hdr = new ResponseHeader
            {
                CorrelationId = Guid.NewGuid().ToString(),
                ExternalId = externalId,
                Status = "Success",
                ErrorCode = "None",
                ErrorDescription = ""
            };

            string msgRaw =
                $@"<ns0:ESBResponse xmlns:ns0=""http://MalamTeam.Inf.ESB.Schemas.ESBResponse"">
<ns1:ResponseHeader xmlns:ns1=""http://MalamTeam.Inf.ESB.Schemas.ResponseHeader"">
<ns1:CorrelationId>{hdr.CorrelationId}</ns1:CorrelationId>
<ns1:Status>{hdr.Status}</ns1:Status>
<ns1:ErrorDescription>{hdr.ErrorDescription}</ns1:ErrorDescription>
<ns1:ErrorCode>{hdr.ErrorCode}</ns1:ErrorCode>
<ns1:ExternalId>{hdr.ExternalId}</ns1:ExternalId>
</ns1:ResponseHeader>
<Body>{innerXml}</Body>
</ns0:ESBResponse>";

            var first = new NG_9101_MSG_OutgoingMessageResponseOutgoingMessage
            {
                ConsumerID = consumerId,
                CorrelationId = _rp.CorrelationID ?? Guid.NewGuid().ToString(),
                Filename = fileName,
                ServiceName = serviceName,
                Senddate = DateTime.Now,
                RoutedConsumerID = string.Empty,
                MSG = msgRaw
            };

            var rsp = new NG_9101_MSG_OutgoingMessageResponse
            {
                ResponseContentHeader = new ResponseContentHeader
                {
                    TransmitionDateTime = DateTime.Now,
                    ApplicationID = 0,
                    Exception = null
                },
                OutgoingMessage = new[] { first },
                Result = new NG_9101_MSG_OutgoingMessageResponseResult
                {
                    HowManyOtherWaitingMessages = 1,
                }
            };

            return (rsp, hdr, null);
        }
        (string msg, string svc, string file) ParseParam1(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return (null, null, null);

            const string pattern =@"(?s)'msg'\s*:\s*'(?<xml>.*?)'\s*,\s*'servicename'\s*:\s*'(?<svc>[^']*)'\s*,\s*'filename'\s*:\s*'(?<fn>[^']*)'";

            var m = Regex.Match(raw, pattern);
            return m.Success
                ? (m.Groups["xml"].Value, m.Groups["svc"].Value, m.Groups["fn"].Value)
                : (null, null, null);
        }
    }
}

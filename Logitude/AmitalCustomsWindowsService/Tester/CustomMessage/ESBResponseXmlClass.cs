#if true

#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCustomsWindowsService.Tester.CustomMessage
{
    public class ESBResponseXmlClass
    {
        public string Get(string CorrelationId,string ExternalId,string body)
        {
            var xml=
@"
<ns0:ESBResponse xmlns:ns0=""http://MalamTeam.Inf.ESB.Schemas.ESBResponse"">
  <ns1:ResponseHeader xmlns:ns1=""http://MalamTeam.Inf.ESB.Schemas.ResponseHeader"">
    <ns1:CorrelationId>@CorrelationId@</ns1:CorrelationId>
    <ns1:Status>Success</ns1:Status>
    <ns1:ErrorDescription></ns1:ErrorDescription>
    <ns1:ErrorCode>None</ns1:ErrorCode>
    <ns1:ExternalId>@ExternalId@</ns1:ExternalId>
  </ns1:ResponseHeader>
  <Body>
@Body@
  </Body>
</ns0:ESBResponse>";
            return xml
                .Replace("@CorrelationId@", CorrelationId)
                .Replace("@ExternalId@", ExternalId)
                .Replace("@Body@", body);
            
        }
        public bool IsValid()
        {
            return true;
        }

    }
}

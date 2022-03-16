using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class PaymentQueryProvider : IRequestProvider
    {
        public string GetRequest()
        {
            return @"<?xml version=""1.0""?>
<TSH_NG_8285_Web01_PaymentRequestParams xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <LoggingEnabled>true</LoggingEnabled>
    <LoggingUserId>1-11471</LoggingUserId>
    <IsFakeResponse>false</IsFakeResponse>
    <Tenant>1</Tenant>
    <RequestVIA>WebServiceInteractive</RequestVIA>
    <InterfaceTypeCode>8285</InterfaceTypeCode>
    <CustomsRequestsSheetId>9302a2ce-ceed-49da-b142-ae7f24a9c2e9</CustomsRequestsSheetId>
    <TransmitionDateTime xsi:nil=""true"" />
    <FutureSendDateTime xsi:nil=""true"" />
    <SuppressSplitWR>false</SuppressSplitWR>
    <PBId>9302a2ce-ceed-49da-b142-ae7f24a9c2e9</PBId>
    <ForcePersonalSign>false</ForcePersonalSign>
    <IsAngularClient>true</IsAngularClient>
    <DeleteStatus>false</DeleteStatus>
    <SplitterModeLetCreateMyType>false</SplitterModeLetCreateMyType>
    <AvoidSign>false</AvoidSign>
    <AgentExternalId>510120041</AgentExternalId>
    <PaymentID>460381479</PaymentID>
    <paymentDateFrom xsi:nil=""true"" />
    <paymentDateTo xsi:nil=""true"" />
    <EffectiveDateFrom xsi:nil=""true"" />
    <EffectiveDateTo xsi:nil=""true"" />
</TSH_NG_8285_Web01_PaymentRequestParams>".Trim();
        }

        public string GetResponse()
        {
            return @"<?xml version=""1.0""?>
<TSH_NG_8285_Web01_PaymentResponseData xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <HasException>false</HasException>
    <UserMessage>nne5Nflobunnuolm7Nn7wwlopmlom</UserMessage>
    <Succeeded>true</Succeeded>
    <ContinueProcessInBackground>false</ContinueProcessInBackground>
    <PaymentsDetailsList>
        <PaymentsDetailsResult>
            <PaymentlD>460381479</PaymentlD>
            <PaymentType>הכנסה</PaymentType>
            <PaymentAmount>14534</PaymentAmount>
            <Importer>55024335625משירי תנועה ומשאיות-שותפות מוגבל-</Importer>
            <Agent>510120041-עמילות מכס ותחבורה בעמ</Agent>
                <PaymentMethodType>קופה</PaymentMethodType>
                <PaymentStatus>הוכן</PaymentStatus>
                <EntityExternallD>0002302841206</EntityExternallD>
                <EntityType>תיק גרעון מוביל</EntityType>
        </PaymentsDetailsResult>
    </PaymentsDetailsList>
</TSH_NG_8285_Web01_PaymentResponseData>".Trim();
        }
    }
}
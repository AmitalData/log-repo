using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class ImporterDeclarationDetailProvider : IRequestProvider
    {
        public string GetRequest()
        {
            return @"<?xml version=""1.0""?>
<ImporterDeclarationRequestParams xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
<LoggingEnabled>true</LoggingEnabled>
<LoggingUserId>1-5720</LoggingUserId>
<IsFakeResponse>false</IsFakeResponse>
<Tenant>1</Tenant>
<RequestVIA>WebServiceInteractive</RequestVIA>
<InterfaceTypeCode>8326</InterfaceTypeCode>
<CustomsRequestsSheetId>75be8cc4-ff5a-4343-b34b-d1aeae28ef19</CustomsRequestsSheetId>
<TransmitionDateTime xsi:nil=""true""/>
<FutureSendDateTime xsi:nil=""true""/>
<SuppressSplitWR>false</SuppressSplitWR>
<PBId>75be8cc4-ff5a-4343-b34b-d1aeae28ef19</PBId>
<ForcePersonalSign>false</ForcePersonalSign>
<IsAngularClient>true</IsAngularClient>
<DeleteStatus>false</DeleteStatus>
<SplitterModeLetCreateMyType>false</SplitterModeLetCreateMyType>
<ImporterNumber>570048975</ImporterNumber>
<IsByExpireDate>false</IsByExpireDate>
<IsByType>true</IsByType>
<DeclarationConect>1</DeclarationConect>
<DeclarationExpire>2020-08-25T00:00:00.478Z</DeclarationExpire>
<Code>12222</Code>
<FromDate>2019-08-15T00:00:00Z</FromDate>
<ToDate>2019-08-20T00:00:00Z</ToDate>
<JoinCustomsVendors>true</JoinCustomsVendors>
</ImporterDeclarationRequestParams>
";
        }

        public string GetResponse()
        {
            return @"<?xml version=""1.0""?>
<ImporterDeclarationResponseData xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <HasException>false</HasException>
  <UserMessage>התקבלה רשימת תצהירים ליבואן 570048975</UserMessage>
  <Succeeded>true</Succeeded>
  <ContinueProcessInBackground>false</ContinueProcessInBackground>
  <PeriodDeclarationList>
    <PeriodDeclarationResult>
      <PeriodDeclarationID>621498162</PeriodDeclarationID>
      <VendorID>3016315</VendorID>
      <VendorName>YIWU FASHION PACK FACTORY</VendorName>
      <CreateDate>07.11.2018</CreateDate>
      <ValidityFrom>07.11.2018</ValidityFrom>
      <ExpirationDate>07.11.2019</ExpirationDate>
      <Status>1</Status>
      <StatusName>טרם נבדק</StatusName>
      <DocumentID>621498162</DocumentID>
    </PeriodDeclarationResult>
    <PeriodDeclarationResult>
      <PeriodDeclarationID>621508472</PeriodDeclarationID>
      <VendorID>3016536</VendorID>
      <VendorName>DEQI JEWELRY PACKAGING COMPANY</VendorName>
      <CreateDate>07.11.2018</CreateDate>
      <ValidityFrom>07.11.2018</ValidityFrom>
      <ExpirationDate>07.11.2019</ExpirationDate>
      <Status>1</Status>
      <StatusName>טרם נבדק</StatusName>
      <DocumentID>621508472</DocumentID>
    </PeriodDeclarationResult>
    <PeriodDeclarationResult>
      <PeriodDeclarationID>621511944</PeriodDeclarationID>
      <VendorID>3016614</VendorID>
      <VendorName>GUANGZHOU MEILI PACKAGE CO.</VendorName>
      <CreateDate>07.11.2018</CreateDate>
      <ValidityFrom>07.11.2018</ValidityFrom>
      <ExpirationDate>07.11.2019</ExpirationDate>
      <Status>1</Status>
      <StatusName>טרם נבדק</StatusName>
      <DocumentID>621511944</DocumentID>
    </PeriodDeclarationResult>
    <PeriodDeclarationResult>
      <PeriodDeclarationID>621713475</PeriodDeclarationID>
      <VendorID>3018456</VendorID>
      <VendorName>ORIENTAL SKY PACKAGING CO. LTD</VendorName>
      <CreateDate>11.11.2018</CreateDate>
      <ValidityFrom>11.11.2018</ValidityFrom>
      <ExpirationDate>11.11.2019</ExpirationDate>
      <Status>1</Status>
      <StatusName>טרם נבדק</StatusName>
      <DocumentID>621713475</DocumentID>
    </PeriodDeclarationResult>
    <PeriodDeclarationResult>
      <PeriodDeclarationID>621714286</PeriodDeclarationID>
      <VendorID>3018463</VendorID>
      <VendorName>FUZHOU SUNNY PACKAGING CO.,LTD</VendorName>
      <CreateDate>11.11.2018</CreateDate>
      <ValidityFrom>11.11.2018</ValidityFrom>
      <ExpirationDate>11.11.2019</ExpirationDate>
      <Status>1</Status>
      <StatusName>טרם נבדק</StatusName>
      <DocumentID>621714286</DocumentID>
    </PeriodDeclarationResult>
    <PeriodDeclarationResult>
      <PeriodDeclarationID>621843184</PeriodDeclarationID>
      <VendorID>3020174</VendorID>
      <VendorName>SHENZHEN ODEAR FASHION TECHNOLOGY CO. LTD</VendorName>
      <CreateDate>13.11.2018</CreateDate>
      <ValidityFrom>13.11.2018</ValidityFrom>
      <ExpirationDate>13.11.2019</ExpirationDate>
      <Status>1</Status>
      <StatusName>טרם נבדק</StatusName>
      <DocumentID>621843184</DocumentID>
    </PeriodDeclarationResult>
    <PeriodDeclarationResult>
      <PeriodDeclarationID>633588730</PeriodDeclarationID>
      <VendorID>2748933</VendorID>
      <VendorName>CHINA GENERAL PLASTICS CORPORATION</VendorName>
      <CreateDate>10.07.2019</CreateDate>
      <ValidityFrom>10.07.2019</ValidityFrom>
      <ExpirationDate>10.07.2020</ExpirationDate>
      <Status>1</Status>
      <StatusName>טרם נבדק</StatusName>
      <DocumentID>633588730</DocumentID>
    </PeriodDeclarationResult>
    <PeriodDeclarationResult>
      <PeriodDeclarationID>636061837</PeriodDeclarationID>
      <VendorID>3120645</VendorID>
      <VendorName>REGLOPLAS  A.G</VendorName>
      <CreateDate>22.08.2019</CreateDate>
      <ValidityFrom>22.08.2019</ValidityFrom>
      <ExpirationDate>22.08.2020</ExpirationDate>
      <Status>1</Status>
      <StatusName>טרם נבדק</StatusName>
      <DocumentID>636061837</DocumentID>
    </PeriodDeclarationResult>
  </PeriodDeclarationList>
</ImporterDeclarationResponseData>";
        }
    }
}
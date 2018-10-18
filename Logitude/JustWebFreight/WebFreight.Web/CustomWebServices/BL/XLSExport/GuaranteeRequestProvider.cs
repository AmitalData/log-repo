using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class GuaranteeRequestProvider : IRequestProvider
    {
        public string GetRequest()
        {
            return @"<?xml version=""1.0""?>
<GuaranteeRequestParams xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <LoggingEnabled>true</LoggingEnabled>
  <LoggingUserId>1-10920</LoggingUserId>
  <IsFakeResponse>false</IsFakeResponse>
  <Tenant>1</Tenant>
  <RequestVIA>WebServiceInteractive</RequestVIA>
  <InterfaceTypeCode>8305</InterfaceTypeCode>
  <CustomsRequestsSheetId>a87196af-037f-4b83-b804-6f7154e928b9</CustomsRequestsSheetId>
  <TransmitionDateTime xsi:nil=""true"" />
  <FutureSendDateTime xsi:nil=""true"" />
  <SuppressSplitWR>false</SuppressSplitWR>
  <PBId>a87196af-037f-4b83-b804-6f7154e928b9</PBId>
  <ForcePersonalSign>false</ForcePersonalSign>
  <IsAngularClient>true</IsAngularClient>
  <DeleteStatus>false</DeleteStatus>
  <GuranteeType>4</GuranteeType>
  <FileNumber>73000222</FileNumber>
  <Numeral>1</Numeral>
</GuaranteeRequestParams>";
        }
        public string GetResponse()
        {
            return @"<?xml version=""1.0""?>
<GuaranteeResponseData xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <HasException>false</HasException>
  <UserMessage>שליחת מסר שאילתא לערבויות הצליחה</UserMessage>
  <Succeeded>true</Succeeded>
  <ContinueProcessInBackground>false</ContinueProcessInBackground>
  <DisplayFileNumber>73000222/1</DisplayFileNumber>
  <StatusName>פתוח</StatusName>
  <CreditLimit>1,700,000.00</CreditLimit>
  <CustomOfficeNumber>3</CustomOfficeNumber>
  <CustomOfficeName>מרכז</CustomOfficeName>
  <CreditBalance>1,231,821.00</CreditBalance>
  <GuaranteedName>סבון של פעם תעשיות בע""מ</GuaranteedName>
  <AgentExternalID />
  <GuaranteeExecutedAmountAdjusted>0.00</GuaranteeExecutedAmountAdjusted>
  <Validity>30.07.2018</Validity>
  <GuaranteeLettersList>
    <ExternalGuaranteeLettersResult>
      <GuaranteeTypeName>ערבות בנקאית</GuaranteeTypeName>
      <CertificateID>73070047</CertificateID>
      <GuaranteeExternalCertificateNumebr>1272017023</GuaranteeExternalCertificateNumebr>
      <GuaranatorName>הבנק הבינלאומי הראשון לישראל בע""מ</GuaranatorName>
      <GuaranteeValidityDate>30.07.2018</GuaranteeValidityDate>
      <CertificateAmount>850,000.00</CertificateAmount>
      <CertificateAllocation>850,000.00</CertificateAllocation>
      <AvaliableCertificateAmount>0</AvaliableCertificateAmount>
      <GuaranteeStatusName>מאושר</GuaranteeStatusName>
    </ExternalGuaranteeLettersResult>
  </GuaranteeLettersList>
  <CreditTransactionsList>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>14.03.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>75579764</EntityNumber>
      <CreditTransactionAmount>-25,556.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>13.03.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>75142712</EntityNumber>
      <CreditTransactionAmount>-3,550.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>12.03.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>74944705</EntityNumber>
      <CreditTransactionAmount>-31,603.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>12.03.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>74942084</EntityNumber>
      <CreditTransactionAmount>-37,175.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>08.03.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>74159080</EntityNumber>
      <CreditTransactionAmount>-38,785.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>07.03.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>74044173</EntityNumber>
      <CreditTransactionAmount>-16,805.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>05.03.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>73375186</EntityNumber>
      <CreditTransactionAmount>-23,976.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>05.03.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>73226004</EntityNumber>
      <CreditTransactionAmount>-19,601.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>04.03.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>73226006</EntityNumber>
      <CreditTransactionAmount>-18,121.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>04.03.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>73197443</EntityNumber>
      <CreditTransactionAmount>-13,897.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>04.03.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>73031697</EntityNumber>
      <CreditTransactionAmount>-5,608.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>27.02.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>72312410</EntityNumber>
      <CreditTransactionAmount>-15,858.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>20.02.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>70862225</EntityNumber>
      <CreditTransactionAmount>-4,194.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>19.02.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>70450280</EntityNumber>
      <CreditTransactionAmount>-17,978.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>15.02.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>69790950</EntityNumber>
      <CreditTransactionAmount>-19,505.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>15.02.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>69790948</EntityNumber>
      <CreditTransactionAmount>-19,505.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>13.02.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>68307121</EntityNumber>
      <CreditTransactionAmount>-948.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>07.02.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>67168103</EntityNumber>
      <CreditTransactionAmount>-14,864.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>06.02.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>67614161</EntityNumber>
      <CreditTransactionAmount>-21,466.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>04.02.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>66982684</EntityNumber>
      <CreditTransactionAmount>-19,940.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>04.02.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>66982686</EntityNumber>
      <CreditTransactionAmount>-17,616.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>28.01.2018</CreditTransactionDate>
      <CreditTransactionName>הוכחת קיום תנאי</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>64198619</EntityNumber>
      <CreditTransactionAmount>2,881.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>28.01.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>64198619</EntityNumber>
      <CreditTransactionAmount>-2,881.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>25.01.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>65237418</EntityNumber>
      <CreditTransactionAmount>-1,573.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>24.01.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>64236672</EntityNumber>
      <CreditTransactionAmount>-2,881.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>18.01.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>63297570</EntityNumber>
      <CreditTransactionAmount>-18,941.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>17.01.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>63297572</EntityNumber>
      <CreditTransactionAmount>-17,757.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>15.01.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>62564842</EntityNumber>
      <CreditTransactionAmount>-4,417.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
    <ExternalCreditTransactionsResult>
      <CreditTransactionDate>15.01.2018</CreditTransactionDate>
      <CreditTransactionName>הגשת הצהרה</CreditTransactionName>
      <EntityTypeName>סחורה</EntityTypeName>
      <EntityNumber>62544631</EntityNumber>
      <CreditTransactionAmount>-36,059.00</CreditTransactionAmount>
    </ExternalCreditTransactionsResult>
  </CreditTransactionsList>
</GuaranteeResponseData>";
        }
    }
}
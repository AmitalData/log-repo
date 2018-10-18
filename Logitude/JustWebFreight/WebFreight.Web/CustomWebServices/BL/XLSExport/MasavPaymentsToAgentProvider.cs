using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class MasavPaymentsToAgentProvider : IRequestProvider
    {
        public string GetRequest()
        {
         return @"<?xml version=""1.0""?>
<MasavPaymentsToAgentRequestParams xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <LoggingEnabled>true</LoggingEnabled>
  <LoggingUserId>1-12818</LoggingUserId>
  <IsFakeResponse>false</IsFakeResponse>
  <Tenant>1</Tenant>
  <RequestVIA>WebServiceInteractive</RequestVIA>
  <InterfaceTypeCode>8368</InterfaceTypeCode>
  <CustomsRequestsSheetId>83c2e142-1725-422d-b51a-bff84184f39c</CustomsRequestsSheetId>
  <TransmitionDateTime xsi:nil=""true"" />
  <FutureSendDateTime xsi:nil=""true"" />
  <SuppressSplitWR>false</SuppressSplitWR>
  <PBId>83c2e142-1725-422d-b51a-bff84184f39c</PBId>
  <ForcePersonalSign>false</ForcePersonalSign>
  <IsAngularClient>true</IsAngularClient>
  <DeleteStatus>false</DeleteStatus>
  <PaymentDate>2018-04-01T00:00:00Z</PaymentDate>
</MasavPaymentsToAgentRequestParams>";
        }

        public string GetResponse()
        {
            return
                 @"<?xml version=""1.0""?>
<MasavPaymentsToAgentResponseData xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <HasException>false</HasException>
  <UserMessage>ניתוח דוח קופה לתאריך 01.04.2018</UserMessage>
  <Succeeded>true</Succeeded>
  <ContinueProcessInBackground>false</ContinueProcessInBackground>
  <AgentMasavPaymentResultList>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816184</PaymentID>
      <Amount>27,028.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511140055</ExternalID>
      <ExternalName>ק ל א - טנכור קורפוריישן (ישראל)</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>27,028.00</PaymentMethodAmount>
      <Bank>Citibank N.A</Bank>
      <Branch>1</Branch>
      <AccountNumber>500351004</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407751</EntityIdExternalReferenceID>
      <BankCode>22</BankCode>
      <AgentMasavPaymentResultHeader>Citibank N.A, סניף: 1, חשבון: 500351004  -  סה""כ סכום ששולם במס""ב: 27,028.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407751</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017860022</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816603</PaymentID>
      <Amount>15,412.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513243238</ExternalID>
      <ExternalName>קלא-טנכור אינטגרייטד מטרולוג'י (ישר</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>15,412.00</PaymentMethodAmount>
      <Bank>Citibank N.A</Bank>
      <Branch>1</Branch>
      <AccountNumber>500361018</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408051</EntityIdExternalReferenceID>
      <BankCode>22</BankCode>
      <AgentMasavPaymentResultHeader>Citibank N.A, סניף: 1, חשבון: 500361018  -  סה""כ סכום ששולם במס""ב: 15,412.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408051</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018535946</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812427</PaymentID>
      <Amount>119,186.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510523889</ExternalID>
      <ExternalName>טכנולוגית להבים בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>119,186.00</PaymentMethodAmount>
      <Bank>Citibank N.A</Bank>
      <Branch>1</Branch>
      <AccountNumber>500986018</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180102142</EntityIdExternalReferenceID>
      <BankCode>22</BankCode>
      <AgentMasavPaymentResultHeader>Citibank N.A, סניף: 1, חשבון: 500986018  -  סה""כ סכום ששולם במס""ב: 119,186.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180102142</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018137800</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811580</PaymentID>
      <Amount>17,371.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512396144</ExternalID>
      <ExternalName>סיסקו סיסטמס ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>17,371.00</PaymentMethodAmount>
      <Bank>Citibank N.A</Bank>
      <Branch>1</Branch>
      <AccountNumber>501035009</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407657</EntityIdExternalReferenceID>
      <BankCode>22</BankCode>
      <AgentMasavPaymentResultHeader>Citibank N.A, סניף: 1, חשבון: 501035009  -  סה""כ סכום ששולם במס""ב: 79,859.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407657</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017538735</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811604</PaymentID>
      <Amount>3,083.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512396144</ExternalID>
      <ExternalName>סיסקו סיסטמס ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>3,083.00</PaymentMethodAmount>
      <Bank>Citibank N.A</Bank>
      <Branch>1</Branch>
      <AccountNumber>501035009</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407826</EntityIdExternalReferenceID>
      <BankCode>22</BankCode>
      <AgentMasavPaymentResultHeader>Citibank N.A, סניף: 1, חשבון: 501035009  -  סה""כ סכום ששולם במס""ב: 79,859.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407826</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017891241</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812786</PaymentID>
      <Amount>11,366.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512396144</ExternalID>
      <ExternalName>סיסקו סיסטמס ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>11,366.00</PaymentMethodAmount>
      <Bank>Citibank N.A</Bank>
      <Branch>1</Branch>
      <AccountNumber>501035009</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407827</EntityIdExternalReferenceID>
      <BankCode>22</BankCode>
      <AgentMasavPaymentResultHeader>Citibank N.A, סניף: 1, חשבון: 501035009  -  סה""כ סכום ששולם במס""ב: 79,859.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407827</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018500692</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815616</PaymentID>
      <Amount>5,501.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512396144</ExternalID>
      <ExternalName>סיסקו סיסטמס ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>5,501.00</PaymentMethodAmount>
      <Bank>Citibank N.A</Bank>
      <Branch>1</Branch>
      <AccountNumber>501035009</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408075</EntityIdExternalReferenceID>
      <BankCode>22</BankCode>
      <AgentMasavPaymentResultHeader>Citibank N.A, סניף: 1, חשבון: 501035009  -  סה""כ סכום ששולם במס""ב: 79,859.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408075</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018537256</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815654</PaymentID>
      <Amount>30.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512396144</ExternalID>
      <ExternalName>סיסקו סיסטמס ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>30.00</PaymentMethodAmount>
      <Bank>Citibank N.A</Bank>
      <Branch>1</Branch>
      <AccountNumber>501035009</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408043</EntityIdExternalReferenceID>
      <BankCode>22</BankCode>
      <AgentMasavPaymentResultHeader>Citibank N.A, סניף: 1, חשבון: 501035009  -  סה""כ סכום ששולם במס""ב: 79,859.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408043</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018539153</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815768</PaymentID>
      <Amount>42,508.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512396144</ExternalID>
      <ExternalName>סיסקו סיסטמס ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>42,508.00</PaymentMethodAmount>
      <Bank>Citibank N.A</Bank>
      <Branch>1</Branch>
      <AccountNumber>501035009</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408089</EntityIdExternalReferenceID>
      <BankCode>22</BankCode>
      <AgentMasavPaymentResultHeader>Citibank N.A, סניף: 1, חשבון: 501035009  -  סה""כ סכום ששולם במס""ב: 79,859.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408089</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018540359</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812807</PaymentID>
      <Amount>44,024.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510057607</ExternalID>
      <ExternalName>פיברו בריאות בעלי חיים בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>44,024.00</PaymentMethodAmount>
      <Bank>בנק אגוד לישראל בע""מ</Bank>
      <Branch>63</Branch>
      <AccountNumber>82740093</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4186900652</EntityIdExternalReferenceID>
      <BankCode>13</BankCode>
      <AgentMasavPaymentResultHeader>בנק אגוד לישראל בע""מ, סניף: 63, חשבון: 82740093  -  סה""כ סכום ששולם במס""ב: 44,024.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4186900652</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018493674</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813717</PaymentID>
      <Amount>90,581.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>90,581.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015314</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015314</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522191</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813718</PaymentID>
      <Amount>21,611.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>21,611.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015315</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015315</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522266</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813719</PaymentID>
      <Amount>178,668.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>178,668.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015316</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015316</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522308</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813720</PaymentID>
      <Amount>43,165.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>43,165.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015317</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015317</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522340</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813722</PaymentID>
      <Amount>178,738.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>178,738.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015318</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015318</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522365</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813725</PaymentID>
      <Amount>45,359.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>45,359.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015319</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015319</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522407</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813727</PaymentID>
      <Amount>101,553.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>101,553.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015320</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015320</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522423</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813730</PaymentID>
      <Amount>1,200,969.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>1,200,969.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015321</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015321</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522472</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813731</PaymentID>
      <Amount>399,254.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>399,254.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015308</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015308</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522050</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813732</PaymentID>
      <Amount>69,290.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>69,290.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015309</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015309</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522068</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813733</PaymentID>
      <Amount>76,589.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>76,589.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015312</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015312</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522159</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813734</PaymentID>
      <Amount>229,604.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>229,604.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015311</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015311</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522118</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813735</PaymentID>
      <Amount>84,635.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>84,635.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015313</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015313</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522167</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813736</PaymentID>
      <Amount>45,331.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>45,331.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015310</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015310</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522076</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814082</PaymentID>
      <Amount>129,421.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>129,421.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015354</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015354</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018525707</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814085</PaymentID>
      <Amount>83,326.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>83,326.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015357</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015357</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018525798</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814086</PaymentID>
      <Amount>579,867.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>579,867.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015355</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015355</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018525756</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814088</PaymentID>
      <Amount>187,417.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>187,417.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015352</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015352</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018525657</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814090</PaymentID>
      <Amount>116,029.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>116,029.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015356</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015356</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018525780</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814093</PaymentID>
      <Amount>42,131.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>42,131.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015353</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015353</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018525681</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815397</PaymentID>
      <Amount>3,088,733.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>3,088,733.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015410</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015410</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018543940</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815968</PaymentID>
      <Amount>106,310.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>106,310.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015411</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015411</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018566552</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815994</PaymentID>
      <Amount>177,989.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>177,989.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015412</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015412</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018566560</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816703</PaymentID>
      <Amount>1,475,174.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657430</ExternalID>
      <ExternalName>טלקאר חברה בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>1,475,174.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>171360</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015418</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 171360  -  סה""כ סכום ששולם במס""ב: 8,751,744.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015418</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018573517</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811859</PaymentID>
      <Amount>3,189.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510742190</ExternalID>
      <ExternalName>מולטילוק טכנולוגיות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>3,189.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>186961</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407839</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 186961  -  סה""כ סכום ששולם במס""ב: 20,566.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407839</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018117992</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812025</PaymentID>
      <Amount>4,584.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510742190</ExternalID>
      <ExternalName>מולטילוק טכנולוגיות בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>4,584.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>186961</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188501017</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 186961  -  סה""כ סכום ששולם במס""ב: 20,566.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188501017</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018058741</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812370</PaymentID>
      <Amount>8,900.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510742190</ExternalID>
      <ExternalName>מולטילוק טכנולוגיות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>8,900.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>186961</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407835</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 186961  -  סה""כ סכום ששולם במס""ב: 20,566.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407835</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018118743</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812650</PaymentID>
      <Amount>3,893.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510742190</ExternalID>
      <ExternalName>מולטילוק טכנולוגיות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>3,893.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>186961</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407759</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 186961  -  סה""כ סכום ששולם במס""ב: 20,566.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407759</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018142107</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813441</PaymentID>
      <Amount>43,132.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520035213</ExternalID>
      <ExternalName>אורבוטק בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>43,132.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>21109</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185103939</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 21109  -  סה""כ סכום ששולם במס""ב: 118,823.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185103939</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017089739</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816222</PaymentID>
      <Amount>75,691.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520035213</ExternalID>
      <ExternalName>אורבוטק בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>75,691.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>21109</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104240</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 21109  -  סה""כ סכום ששולם במס""ב: 118,823.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104240</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018567022</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816610</PaymentID>
      <Amount>49,903.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513533653</ExternalID>
      <ExternalName>סנפיר-ים יבוא ושיווק דגים ומעדני ים</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>49,903.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>45032</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403631</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 45032  -  סה""כ סכום ששולם במס""ב: 55,810.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403631</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018487486</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816687</PaymentID>
      <Amount>5,907.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513533653</ExternalID>
      <ExternalName>סנפיר-ים יבוא ושיווק דגים ומעדני ים</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>5,907.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>45032</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403641</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 45032  -  סה""כ סכום ששולם במס""ב: 55,810.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403641</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018489102</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812095</PaymentID>
      <Amount>25,311.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512041468</ExternalID>
      <ExternalName>ייל מוצרי בטחון בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>25,311.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>10</Branch>
      <AccountNumber>7858</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188500375</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 10, חשבון: 7858  -  סה""כ סכום ששולם במס""ב: 25,311.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188500375</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018056125</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812468</PaymentID>
      <Amount>13,802.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511247322</ExternalID>
      <ExternalName>מודי יבוא ושיווק קרמיקה בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>13,802.00</PaymentMethodAmount>
      <Bank>בנק דיסקונט לישראל בע""מ</Bank>
      <Branch>34</Branch>
      <AccountNumber>522600</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403485</EntityIdExternalReferenceID>
      <BankCode>11</BankCode>
      <AgentMasavPaymentResultHeader>בנק דיסקונט לישראל בע""מ, סניף: 34, חשבון: 522600  -  סה""כ סכום ששולם במס""ב: 13,802.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403485</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018135119</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812057</PaymentID>
      <Amount>14,599.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520040775</ExternalID>
      <ExternalName>גדות מסופים לכימקלים (1985) בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>14,599.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>167</Branch>
      <AccountNumber>55666</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181401796</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 167, חשבון: 55666  -  סה""כ סכום ששולם במס""ב: 94,233.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181401796</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018168300</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813461</PaymentID>
      <Amount>79,634.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520040775</ExternalID>
      <ExternalName>גדות מסופים לכימקלים (1985) בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>79,634.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>167</Branch>
      <AccountNumber>55666</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402860</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 167, חשבון: 55666  -  סה""כ סכום ששולם במס""ב: 94,233.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402860</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018502003</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816061</PaymentID>
      <Amount>2,871.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512607698</ExternalID>
      <ExternalName>סטרטסיס בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>2,871.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>412</Branch>
      <AccountNumber>90902</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187406397</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 412, חשבון: 90902  -  סה""כ סכום ששולם במס""ב: 2,871.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187406397</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018168789</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816837</PaymentID>
      <Amount>135,944.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510515752</ExternalID>
      <ExternalName>דוד לובינסקי בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>135,944.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>508</Branch>
      <AccountNumber>10411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015167</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 508, חשבון: 10411  -  סה""כ סכום ששולם במס""ב: 135,944.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015167</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018574226</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811209</PaymentID>
      <Amount>371.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>560013609</ExternalID>
      <ExternalName>אינטרנשיונל מיינטננס פרטס לוגיסטיקס</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>371.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407927</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407927</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018273688</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811219</PaymentID>
      <Amount>412.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>560013609</ExternalID>
      <ExternalName>אינטרנשיונל מיינטננס פרטס לוגיסטיקס</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>412.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407926</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407926</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018262921</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811226</PaymentID>
      <Amount>462.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>560013609</ExternalID>
      <ExternalName>אינטרנשיונל מיינטננס פרטס לוגיסטיקס</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>462.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407925</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407925</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018262715</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811678</PaymentID>
      <Amount>67,148.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>560013609</ExternalID>
      <ExternalName>אינטרנשיונל מיינטננס פרטס לוגיסטיקס</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>67,148.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407928</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407928</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018492254</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811790</PaymentID>
      <Amount>4,687.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510067333</ExternalID>
      <ExternalName>י ב מ ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>4,687.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407481</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407481</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017506922</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811821</PaymentID>
      <Amount>4,972.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510067333</ExternalID>
      <ExternalName>י ב מ ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>4,972.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407477</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407477</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017507391</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811867</PaymentID>
      <Amount>23,181.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510067333</ExternalID>
      <ExternalName>י ב מ ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>23,181.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407482</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407482</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017504828</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813251</PaymentID>
      <Amount>8,746.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510067333</ExternalID>
      <ExternalName>י ב מ ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>8,746.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407814</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407814</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018498095</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813257</PaymentID>
      <Amount>101,082.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510067333</ExternalID>
      <ExternalName>י ב מ ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>101,082.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407815</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407815</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018499754</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813271</PaymentID>
      <Amount>8,581.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510067333</ExternalID>
      <ExternalName>י ב מ ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>8,581.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407816</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407816</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018496263</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815846</PaymentID>
      <Amount>733.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>560013609</ExternalID>
      <ExternalName>אינטרנשיונל מיינטננס פרטס לוגיסטיקס</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>733.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408057</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408057</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018525178</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815865</PaymentID>
      <Amount>2,827.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>560013609</ExternalID>
      <ExternalName>אינטרנשיונל מיינטננס פרטס לוגיסטיקס</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,827.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>567</Branch>
      <AccountNumber>19411</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407746</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 567, חשבון: 19411  -  סה""כ סכום ששולם במס""ב: 223,202.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407746</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017810613</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812108</PaymentID>
      <Amount>47,159.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511243545</ExternalID>
      <ExternalName>אגנטק (1987) בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>47,159.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>584</Branch>
      <AccountNumber>40393</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106996</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 584, חשבון: 40393  -  סה""כ סכום ששולם במס""ב: 47,159.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106996</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017599539</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813107</PaymentID>
      <Amount>44,379.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>570000745</ExternalID>
      <ExternalName>תנובה מרכז שיתופי לשיווק תוצרת חקלא</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>44,379.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>600</Branch>
      <AccountNumber>651630</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403541</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 600, חשבון: 651630  -  סה""כ סכום ששולם במס""ב: 61,526.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403541</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018097657</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816983</PaymentID>
      <Amount>17,147.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>570000745</ExternalID>
      <ExternalName>תנובה מרכז שיתופי לשיווק תוצרת חקלא</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>17,147.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>600</Branch>
      <AccountNumber>651630</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403171</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 600, חשבון: 651630  -  סה""כ סכום ששולם במס""ב: 61,526.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403171</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018514677</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811646</PaymentID>
      <Amount>6,691.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512074428</ExternalID>
      <ExternalName>תדיראן סוללות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>6,691.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>600</Branch>
      <AccountNumber>657304</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104306</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 600, חשבון: 657304  -  סה""כ סכום ששולם במס""ב: 11,651.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104306</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018118297</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811704</PaymentID>
      <Amount>4,658.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512074428</ExternalID>
      <ExternalName>תדיראן סוללות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>4,658.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>600</Branch>
      <AccountNumber>657304</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104305</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 600, חשבון: 657304  -  סה""כ סכום ששולם במס""ב: 11,651.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104305</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018104149</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813231</PaymentID>
      <Amount>302.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512074428</ExternalID>
      <ExternalName>תדיראן סוללות בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>302.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>600</Branch>
      <AccountNumber>657304</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185103841</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 600, חשבון: 657304  -  סה""כ סכום ששולם במס""ב: 11,651.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185103841</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017522317</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814224</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511487761</ExternalID>
      <ExternalName>יוניון מוטורס בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>600</Branch>
      <AccountNumber>663965</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188501026</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 600, חשבון: 663965  -  סה""כ סכום ששולם במס""ב: 154.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188501026</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017858273</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814293</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511487761</ExternalID>
      <ExternalName>יוניון מוטורס בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>600</Branch>
      <AccountNumber>663965</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188501055</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 600, חשבון: 663965  -  סה""כ סכום ששולם במס""ב: 154.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188501055</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018141927</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816765</PaymentID>
      <Amount>2,768.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>570000968</ExternalID>
      <ExternalName>גת - גבעת חיים אגודה שיתופית לשימור</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,768.00</PaymentMethodAmount>
      <Bank>בנק הפועלים בע""מ</Bank>
      <Branch>620</Branch>
      <AccountNumber>29889</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187107255</EntityIdExternalReferenceID>
      <BankCode>12</BankCode>
      <AgentMasavPaymentResultHeader>בנק הפועלים בע""מ, סניף: 620, חשבון: 29889  -  סה""כ סכום ששולם במס""ב: 2,768.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187107255</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018570190</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814317</PaymentID>
      <Amount>9,692.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515546224</ExternalID>
      <ExternalName>אייס קפיטל קמעונאות (2016) בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>9,692.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>67502095</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407519</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 67502095  -  סה""כ סכום ששולם במס""ב: 9,692.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407519</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018509883</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813999</PaymentID>
      <Amount>143,585.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>143,585.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015345</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015345</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018524726</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814001</PaymentID>
      <Amount>88,117.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>88,117.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015347</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015347</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018524742</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814002</PaymentID>
      <Amount>164,654.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>164,654.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015346</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015346</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018524734</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814003</PaymentID>
      <Amount>164,666.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>164,666.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015348</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015348</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018524775</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814006</PaymentID>
      <Amount>164,666.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>164,666.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015350</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015350</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018524890</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814007</PaymentID>
      <Amount>211,625.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>211,625.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015349</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015349</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018524858</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814235</PaymentID>
      <Amount>92,619.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>92,619.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015329</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015329</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522704</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814241</PaymentID>
      <Amount>40,444.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>40,444.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015328</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015328</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522662</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814242</PaymentID>
      <Amount>86,054.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>86,054.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015327</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015327</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522647</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814243</PaymentID>
      <Amount>121,126.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>121,126.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015336</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015336</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522977</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814247</PaymentID>
      <Amount>52,814.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>52,814.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015323</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015323</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522548</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814248</PaymentID>
      <Amount>88,796.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>88,796.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015339</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015339</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018523058</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814249</PaymentID>
      <Amount>161,937.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>161,937.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015338</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015338</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018523033</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814253</PaymentID>
      <Amount>61,945.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>61,945.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015331</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015331</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522761</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814254</PaymentID>
      <Amount>67,494.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>67,494.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015340</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015340</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018523066</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814257</PaymentID>
      <Amount>543,543.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>543,543.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015341</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015341</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018523108</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814259</PaymentID>
      <Amount>202,032.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>202,032.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015342</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015342</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018523132</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814264</PaymentID>
      <Amount>64,946.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>64,946.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015344</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015344</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018523207</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814265</PaymentID>
      <Amount>120,053.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>120,053.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015343</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015343</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018523173</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814276</PaymentID>
      <Amount>56,574.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>56,574.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015330</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015330</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522738</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814277</PaymentID>
      <Amount>130,003.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>130,003.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015326</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015326</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522639</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814283</PaymentID>
      <Amount>121,369.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>121,369.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015334</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015334</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522894</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814284</PaymentID>
      <Amount>159,545.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>159,545.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015335</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015335</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522928</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814287</PaymentID>
      <Amount>117,273.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>117,273.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015332</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015332</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522779</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814288</PaymentID>
      <Amount>55,174.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>55,174.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015333</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015333</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522803</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814290</PaymentID>
      <Amount>228,092.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>228,092.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015322</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015322</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522514</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814291</PaymentID>
      <Amount>205,371.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>205,371.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015337</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015337</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018523009</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814292</PaymentID>
      <Amount>42,842.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>42,842.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015325</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015325</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018522563</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814536</PaymentID>
      <Amount>207,971.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>207,971.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015360</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015360</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018530202</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814537</PaymentID>
      <Amount>179,685.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>179,685.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015359</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015359</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018530111</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814589</PaymentID>
      <Amount>60,404.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>60,404.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015361</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015361</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531465</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814592</PaymentID>
      <Amount>188,883.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>188,883.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015366</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015366</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531614</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814593</PaymentID>
      <Amount>93,991.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>93,991.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015362</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015362</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531515</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814596</PaymentID>
      <Amount>118,300.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>118,300.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015364</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015364</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531556</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814598</PaymentID>
      <Amount>97,228.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>97,228.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015365</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015365</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531606</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814601</PaymentID>
      <Amount>201,422.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>201,422.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015363</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015363</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531549</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814602</PaymentID>
      <Amount>137,893.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>137,893.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015367</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015367</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531630</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814604</PaymentID>
      <Amount>341,231.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>341,231.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015368</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015368</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531739</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814605</PaymentID>
      <Amount>103,958.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>103,958.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015371</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015371</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531804</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814606</PaymentID>
      <Amount>208,735.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>208,735.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015372</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015372</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531846</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814608</PaymentID>
      <Amount>109,254.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>109,254.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015370</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015370</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531812</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814610</PaymentID>
      <Amount>249,840.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>249,840.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015369</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015369</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531770</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814631</PaymentID>
      <Amount>178,063.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>178,063.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015373</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015373</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018531879</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815254</PaymentID>
      <Amount>176,219.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>176,219.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015378</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015378</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018541472</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815256</PaymentID>
      <Amount>88,153.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>88,153.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015377</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015377</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018541449</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815257</PaymentID>
      <Amount>45,374.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>45,374.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015380</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015380</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018541613</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815258</PaymentID>
      <Amount>170,975.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>170,975.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015382</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015382</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018541662</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815259</PaymentID>
      <Amount>236,801.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>236,801.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015379</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015379</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018541498</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815260</PaymentID>
      <Amount>196,420.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>196,420.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015385</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015385</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018541761</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815262</PaymentID>
      <Amount>30,045.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>30,045.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015383</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015383</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018541712</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815263</PaymentID>
      <Amount>309,117.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>309,117.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015386</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015386</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018541811</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815264</PaymentID>
      <Amount>91,380.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>91,380.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015389</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015389</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542074</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815268</PaymentID>
      <Amount>166,289.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>166,289.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015392</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015392</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542132</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815269</PaymentID>
      <Amount>136,479.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>136,479.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015388</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015388</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542033</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815271</PaymentID>
      <Amount>101,574.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>101,574.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015398</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015398</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542371</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815277</PaymentID>
      <Amount>50,600.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>50,600.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015395</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015395</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542249</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815278</PaymentID>
      <Amount>406,407.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>406,407.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015397</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015397</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542330</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815280</PaymentID>
      <Amount>62,045.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>62,045.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015390</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015390</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542090</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815281</PaymentID>
      <Amount>41,359.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>41,359.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015387</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015387</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542009</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815282</PaymentID>
      <Amount>137,356.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>137,356.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015405</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015405</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542850</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815283</PaymentID>
      <Amount>411,699.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>411,699.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015384</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015384</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018541746</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815308</PaymentID>
      <Amount>35,839.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>35,839.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015401</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015401</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542629</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815310</PaymentID>
      <Amount>2,221,852.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>2,221,852.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015396</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015396</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542298</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815311</PaymentID>
      <Amount>411,866.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>411,866.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015409</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015409</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018543429</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815313</PaymentID>
      <Amount>576,458.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>576,458.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015406</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015406</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018543155</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815314</PaymentID>
      <Amount>1,354,551.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>1,354,551.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015403</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015403</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542744</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815315</PaymentID>
      <Amount>492,118.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>492,118.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015408</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015408</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018543205</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815317</PaymentID>
      <Amount>88,145.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>88,145.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015376</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015376</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018541415</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815318</PaymentID>
      <Amount>1,299,827.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>1,299,827.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015404</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015404</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542793</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815319</PaymentID>
      <Amount>46,737.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>46,737.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015381</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015381</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018541639</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815320</PaymentID>
      <Amount>106,443.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>106,443.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015393</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015393</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542157</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815327</PaymentID>
      <Amount>53,258.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>53,258.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015400</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015400</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542520</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815328</PaymentID>
      <Amount>95,489.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>95,489.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015391</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015391</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542116</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815332</PaymentID>
      <Amount>104,125.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>104,125.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015402</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015402</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542702</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815333</PaymentID>
      <Amount>138,654.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>138,654.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015407</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015407</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018543163</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815334</PaymentID>
      <Amount>342,226.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>342,226.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015394</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015394</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018542231</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815939</PaymentID>
      <Amount>301,788.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>301,788.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015374</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015374</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018540839</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815976</PaymentID>
      <Amount>920,688.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>920,688.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015375</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015375</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018540904</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816659</PaymentID>
      <Amount>177,205.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>177,205.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015415</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015415</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018572691</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816660</PaymentID>
      <Amount>97,791.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>97,791.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015416</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015416</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018572725</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816661</PaymentID>
      <Amount>244,463.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>244,463.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015417</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015417</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018572766</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816662</PaymentID>
      <Amount>188,520.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>188,520.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015413</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015413</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018572576</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816664</PaymentID>
      <Amount>55,938.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513973255</ExternalID>
      <ExternalName>צ'מפיון מוטורס בע""מ</ExternalName>
      <CustomsUnit>3</CustomsUnit>
      <PaymentMethodAmount>55,938.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>70750006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185015414</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 70750006  -  סה""כ סכום ששולם במס""ב: 18,446,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185015414</EntityIdExternalReferenceID>
          <EntityIdKey1>18031018572618</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813274</PaymentID>
      <Amount>395.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520033200</ExternalID>
      <ExternalName>מנועי בית שמש בעמ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>395.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>77660065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407674</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 77660065  -  סה""כ סכום ששולם במס""ב: 395.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407674</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017597764</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814448</PaymentID>
      <Amount>2,930.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511386401</ExternalID>
      <ExternalName>פריגו ישראל סוכנויות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,930.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>83412006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403556</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 83412006  -  סה""כ סכום ששולם במס""ב: 22,041.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403556</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018128924</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815483</PaymentID>
      <Amount>10,465.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511386401</ExternalID>
      <ExternalName>פריגו ישראל סוכנויות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>10,465.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>83412006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403570</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 83412006  -  סה""כ סכום ששולם במס""ב: 22,041.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403570</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018500858</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815613</PaymentID>
      <Amount>8,646.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511386401</ExternalID>
      <ExternalName>פריגו ישראל סוכנויות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>8,646.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>800</Branch>
      <AccountNumber>83412006</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403571</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 800, חשבון: 83412006  -  סה""כ סכום ששולם במס""ב: 22,041.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403571</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018499028</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811440</PaymentID>
      <Amount>18,732.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513829614</ExternalID>
      <ExternalName>ישקר בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>18,732.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>876</Branch>
      <AccountNumber>78720059</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001305</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 876, חשבון: 78720059  -  סה""כ סכום ששולם במס""ב: 274,837.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001305</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018112084</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811494</PaymentID>
      <Amount>253,967.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513829614</ExternalID>
      <ExternalName>ישקר בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>253,967.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>876</Branch>
      <AccountNumber>78720059</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001017</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 876, חשבון: 78720059  -  סה""כ סכום ששולם במס""ב: 274,837.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001017</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018135416</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816357</PaymentID>
      <Amount>2,138.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513829614</ExternalID>
      <ExternalName>ישקר בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,138.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>876</Branch>
      <AccountNumber>78720059</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408056</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 876, חשבון: 78720059  -  סה""כ סכום ששולם במס""ב: 274,837.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408056</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018533834</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812320</PaymentID>
      <Amount>5,955.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520044165</ExternalID>
      <ExternalName>סי.אם.טי. טכנולוגיות רפואיות בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>5,955.00</PaymentMethodAmount>
      <Bank>בנק לאומי לישראל בע""מ</Bank>
      <Branch>876</Branch>
      <AccountNumber>7930000</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180402781</EntityIdExternalReferenceID>
      <BankCode>10</BankCode>
      <AgentMasavPaymentResultHeader>בנק לאומי לישראל בע""מ, סניף: 876, חשבון: 7930000  -  סה""כ סכום ששולם במס""ב: 5,955.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180402781</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017604461</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816122</PaymentID>
      <Amount>19,816.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510481468</ExternalID>
      <ExternalName>סוגת תעשיות בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>19,816.00</PaymentMethodAmount>
      <Bank>בנק מזרחי טפחות בע""מ</Bank>
      <Branch>410</Branch>
      <AccountNumber>523944</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188501033</EntityIdExternalReferenceID>
      <BankCode>20</BankCode>
      <AgentMasavPaymentResultHeader>בנק מזרחי טפחות בע""מ, סניף: 410, חשבון: 523944  -  סה""כ סכום ששולם במס""ב: 19,816.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188501033</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018544997</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812656</PaymentID>
      <Amount>101,156.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511088593</ExternalID>
      <ExternalName>צח שרפון בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>101,156.00</PaymentMethodAmount>
      <Bank>בנק מזרחי טפחות בע""מ</Bank>
      <Branch>427</Branch>
      <AccountNumber>424111</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402574</EntityIdExternalReferenceID>
      <BankCode>20</BankCode>
      <AgentMasavPaymentResultHeader>בנק מזרחי טפחות בע""מ, סניף: 427, חשבון: 424111  -  סה""כ סכום ששולם במס""ב: 101,156.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402574</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018490779</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815569</PaymentID>
      <Amount>8,670.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510495484</ExternalID>
      <ExternalName>טולגל דגניה יהלומי תעשיה בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>8,670.00</PaymentMethodAmount>
      <Bank>בנק מזרחי טפחות בע""מ</Bank>
      <Branch>439</Branch>
      <AccountNumber>282058</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407584</EntityIdExternalReferenceID>
      <BankCode>20</BankCode>
      <AgentMasavPaymentResultHeader>בנק מזרחי טפחות בע""מ, סניף: 439, חשבון: 282058  -  סה""כ סכום ששולם במס""ב: 8,670.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407584</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018099745</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450810983</PaymentID>
      <Amount>7,055.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510852643</ExternalID>
      <ExternalName>טלרד נטוורקס בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>7,055.00</PaymentMethodAmount>
      <Bank>בנק מזרחי טפחות בע""מ</Bank>
      <Branch>461</Branch>
      <AccountNumber>443739</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106603</EntityIdExternalReferenceID>
      <BankCode>20</BankCode>
      <AgentMasavPaymentResultHeader>בנק מזרחי טפחות בע""מ, סניף: 461, חשבון: 443739  -  סה""כ סכום ששולם במס""ב: 7,055.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106603</EntityIdExternalReferenceID>
          <EntityIdKey1>18041016555078</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814393</PaymentID>
      <Amount>2,275.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>514171917</ExternalID>
      <ExternalName>אי.אל. מדי-מרקט בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,275.00</PaymentMethodAmount>
      <Bank>בנק מרכנתיל דיסקונט בע""מ</Bank>
      <Branch>652</Branch>
      <AccountNumber>16067</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407854</EntityIdExternalReferenceID>
      <BankCode>17</BankCode>
      <AgentMasavPaymentResultHeader>בנק מרכנתיל דיסקונט בע""מ, סניף: 652, חשבון: 16067  -  סה""כ סכום ששולם במס""ב: 2,275.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407854</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018524130</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811017</PaymentID>
      <Amount>747.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510039746</ExternalID>
      <ExternalName>אי. בי. בי. טכנולוגיות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>747.00</PaymentMethodAmount>
      <Bank>בנק מרכנתיל דיסקונט בע""מ</Bank>
      <Branch>679</Branch>
      <AccountNumber>63711</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106572</EntityIdExternalReferenceID>
      <BankCode>17</BankCode>
      <AgentMasavPaymentResultHeader>בנק מרכנתיל דיסקונט בע""מ, סניף: 679, חשבון: 63711  -  סה""כ סכום ששולם במס""ב: 747.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106572</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017872092</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450810792</PaymentID>
      <Amount>105,947.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515548907</ExternalID>
      <ExternalName>אייברי דניסון ישראל בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>105,947.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001581</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001581</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018128163</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450810793</PaymentID>
      <Amount>64,027.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520036658</ExternalID>
      <ExternalName>בתי זקוק לנפט בעמ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>64,027.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180101927</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180101927</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018121739</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450810799</PaymentID>
      <Amount>36,894.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>570039271</ExternalID>
      <ExternalName>ניר עותק ניר דוד - אגודה חקלאית שית</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>36,894.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402450</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402450</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018162295</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450810816</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510078017</ExternalID>
      <ExternalName>לים בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180102145</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180102145</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018119287</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450810956</PaymentID>
      <Amount>37,582.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510815335</ExternalID>
      <ExternalName>רמט טרום בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>37,582.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403383</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403383</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017331941</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811018</PaymentID>
      <Amount>19,711.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511455685</ExternalID>
      <ExternalName>יפאורה - תבורי בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>19,711.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180102055</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180102055</EntityIdExternalReferenceID>
          <EntityIdKey1>18011017314574</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811020</PaymentID>
      <Amount>57,709.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511455685</ExternalID>
      <ExternalName>יפאורה - תבורי בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>57,709.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180102054</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180102054</EntityIdExternalReferenceID>
          <EntityIdKey1>18011017317346</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811021</PaymentID>
      <Amount>122,483.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510657612</ExternalID>
      <ExternalName>קבוצת סידב בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>122,483.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106797</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106797</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017334895</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811063</PaymentID>
      <Amount>18,461.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512982257</ExternalID>
      <ExternalName>גליל כימיקלים בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>18,461.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180102045</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180102045</EntityIdExternalReferenceID>
          <EntityIdKey1>18011017835545</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811228</PaymentID>
      <Amount>1,503.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513646430</ExternalID>
      <ExternalName>לנובו (ישראל) בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,503.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407728</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407728</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017857382</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811372</PaymentID>
      <Amount>5,035.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520042326</ExternalID>
      <ExternalName>גטר גרופ בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>5,035.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106568</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106568</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017780212</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811387</PaymentID>
      <Amount>31,327.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511058984</ExternalID>
      <ExternalName>גולמט בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>31,327.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001713</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001713</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018122489</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811484</PaymentID>
      <Amount>1,744.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520036658</ExternalID>
      <ExternalName>בתי זקוק לנפט בעמ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,744.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403520</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403520</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018105849</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811491</PaymentID>
      <Amount>120,726.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510457666</ExternalID>
      <ExternalName>אוניפארם בעמ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>120,726.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407806</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407806</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017882240</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811501</PaymentID>
      <Amount>1,451.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520036658</ExternalID>
      <ExternalName>בתי זקוק לנפט בעמ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,451.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403497</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403497</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018091866</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811525</PaymentID>
      <Amount>7,983.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520036658</ExternalID>
      <ExternalName>בתי זקוק לנפט בעמ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>7,983.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403498</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403498</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018092856</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811563</PaymentID>
      <Amount>833.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520030883</ExternalID>
      <ExternalName>רותם אמפרט נגב בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>833.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187107043</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187107043</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018113256</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811564</PaymentID>
      <Amount>13,174.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513777813</ExternalID>
      <ExternalName>ישפאר מוצרי צריכה בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>13,174.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106613</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106613</EntityIdExternalReferenceID>
          <EntityIdKey1>18041016565226</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811568</PaymentID>
      <Amount>902.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520036658</ExternalID>
      <ExternalName>בתי זקוק לנפט בעמ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>902.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403280</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403280</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017530146</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811570</PaymentID>
      <Amount>11,412.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>851276568</ExternalID>
      <ExternalName>851276568</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>11,412.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180301182</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180301182</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017856749</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811576</PaymentID>
      <Amount>902.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520036658</ExternalID>
      <ExternalName>בתי זקוק לנפט בעמ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>902.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403350</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403350</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017528140</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811579</PaymentID>
      <Amount>1,228.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520030883</ExternalID>
      <ExternalName>רותם אמפרט נגב בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,228.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106780</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106780</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018100014</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811783</PaymentID>
      <Amount>7,197.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520036658</ExternalID>
      <ExternalName>בתי זקוק לנפט בעמ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>7,197.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403504</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403504</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018096071</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811872</PaymentID>
      <Amount>5,049.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510855380</ExternalID>
      <ExternalName>אמי ייצור ושווק בגוד מגן בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>5,049.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001717</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001717</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018136737</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811875</PaymentID>
      <Amount>3,439.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511058984</ExternalID>
      <ExternalName>גולמט בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>3,439.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001736</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001736</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018137651</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811929</PaymentID>
      <Amount>3,340.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512865312</ExternalID>
      <ExternalName>ארד טכנולוגיות מדידה בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>3,340.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106570</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106570</EntityIdExternalReferenceID>
          <EntityIdKey1>18041016510586</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811934</PaymentID>
      <Amount>12,410.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>563466937</ExternalID>
      <ExternalName>563466937 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>12,410.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180201947</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180201947</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017110063</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811956</PaymentID>
      <Amount>637.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513240051</ExternalID>
      <ExternalName>פרומתאוס פוטשניק בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>637.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407858</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407858</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018068054</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811964</PaymentID>
      <Amount>68,295.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511575607</ExternalID>
      <ExternalName>511575607</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>68,295.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001827</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001827</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018127116</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812026</PaymentID>
      <Amount>16,277.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512713629</ExternalID>
      <ExternalName>אופק סוכנויות מערכות איחסון בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>16,277.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180101350</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180101350</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018157691</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812031</PaymentID>
      <Amount>1,015.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512927807</ExternalID>
      <ExternalName>512927807</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,015.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407763</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407763</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017831163</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812041</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511113631</ExternalID>
      <ExternalName>י. ד. עסקים (1986) בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106855</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106855</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017501063</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812097</PaymentID>
      <Amount>5,226.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515548907</ExternalID>
      <ExternalName>אייברי דניסון ישראל בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>5,226.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001389</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001389</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018116994</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812134</PaymentID>
      <Amount>39,549.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511058984</ExternalID>
      <ExternalName>גולמט בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>39,549.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001671</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001671</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018121184</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812209</PaymentID>
      <Amount>2,102.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>514046283</ExternalID>
      <ExternalName>אלביט מערכות יבשה ותקשוב בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>2,102.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187406187</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187406187</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017819432</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812233</PaymentID>
      <Amount>1,530.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511325326</ExternalID>
      <ExternalName>ג'י.אי.אס. גלובל אנוירומנטל סולושנס</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,530.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403505</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403505</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017859370</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812244</PaymentID>
      <Amount>3,644.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>570039271</ExternalID>
      <ExternalName>ניר עותק ניר דוד - אגודה חקלאית שית</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>3,644.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403496</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403496</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017864263</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812293</PaymentID>
      <Amount>40,298.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>570041509</ExternalID>
      <ExternalName>מגו אפק אגודה שיתופית חקלאית בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>40,298.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180101760</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180101760</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018148203</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812300</PaymentID>
      <Amount>2,208.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512133323</ExternalID>
      <ExternalName>גלובל ישראל - טקניון, צרעה, מגנלי ב</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,208.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403510</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403510</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017880905</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812318</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104154</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104154</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018136075</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812330</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104220</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104220</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018494243</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812435</PaymentID>
      <Amount>1,992.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513912287</ExternalID>
      <ExternalName>נ. ראובני סופלאי בע""מ</ExternalName>
      <CustomsUnit>505</CustomsUnit>
      <PaymentMethodAmount>1,992.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187107249</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187107249</EntityIdExternalReferenceID>
          <EntityIdKey1>18381018164788</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812551</PaymentID>
      <Amount>19,651.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515686004</ExternalID>
      <ExternalName>רמפה מעליות ונגישות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>19,651.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187900556</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187900556</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018080356</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812585</PaymentID>
      <Amount>34,959.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513777813</ExternalID>
      <ExternalName>ישפאר מוצרי צריכה בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>34,959.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187107173</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187107173</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018126480</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812592</PaymentID>
      <Amount>31,600.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513768358</ExternalID>
      <ExternalName>א.ר.י. אבזרים להולכת נוזלים בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>31,600.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187104901</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187104901</EntityIdExternalReferenceID>
          <EntityIdKey1>18011017501501</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812599</PaymentID>
      <Amount>53,185.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510787930</ExternalID>
      <ExternalName>פקר מתכות איכות בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>53,185.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188501013</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188501013</EntityIdExternalReferenceID>
          <EntityIdKey1>18011017895861</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812626</PaymentID>
      <Amount>56,934.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511237554</ExternalID>
      <ExternalName>סמיט טולס הנטר בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>56,934.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402418</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402418</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018157048</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812674</PaymentID>
      <Amount>2,384.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510976921</ExternalID>
      <ExternalName>דיאמנט צעצועים בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,384.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403509</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403509</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018123180</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812844</PaymentID>
      <Amount>11,131.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515602241</ExternalID>
      <ExternalName>515602241 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>11,131.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187900553</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187900553</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018128478</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812906</PaymentID>
      <Amount>364.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562472100</ExternalID>
      <ExternalName>562472100 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>364.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180202010</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180202010</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018133684</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812913</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520035874</ExternalID>
      <ExternalName>אלביט מערכות ל""א וסיגינט- אלישרא בע</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407737</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407737</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018048742</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812926</PaymentID>
      <Amount>19,874.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512865312</ExternalID>
      <ExternalName>ארד טכנולוגיות מדידה בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>19,874.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187107018</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187107018</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017557891</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812931</PaymentID>
      <Amount>261.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562132829</ExternalID>
      <ExternalName>562132829 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>261.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180201925</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180201925</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018137321</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812942</PaymentID>
      <Amount>261.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562149369</ExternalID>
      <ExternalName>562149369 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>261.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180201904</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180201904</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018151546</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812999</PaymentID>
      <Amount>7,782.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513646430</ExternalID>
      <ExternalName>לנובו (ישראל) בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>7,782.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407731</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407731</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017862424</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813072</PaymentID>
      <Amount>5,778.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>907606081</ExternalID>
      <ExternalName>907606081</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>5,778.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180201979</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180201979</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017299577</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813077</PaymentID>
      <Amount>2,669.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562472431</ExternalID>
      <ExternalName>562472431 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>2,669.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180201212</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180201212</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018164135</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813108</PaymentID>
      <Amount>42,725.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511673485</ExternalID>
      <ExternalName>גרציאני תעשיות (1992) בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>42,725.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180101955</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180101955</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018141380</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813185</PaymentID>
      <Amount>622.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511455685</ExternalID>
      <ExternalName>יפאורה - תבורי בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>622.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187107283</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187107283</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018512598</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813218</PaymentID>
      <Amount>53,945.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520041997</ExternalID>
      <ExternalName>טאואר סמיקונדקטור בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>53,945.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407829</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407829</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018131043</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813247</PaymentID>
      <Amount>96,160.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513716340</ExternalID>
      <ExternalName>אלכסנדרוביץ פלסטיקה הנדסית וגומי -</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>96,160.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407567</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407567</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018078665</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813458</PaymentID>
      <Amount>2,169.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511116634</ExternalID>
      <ExternalName>הרמוני רהיטים בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>2,169.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407479</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407479</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017873702</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813500</PaymentID>
      <Amount>932.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520033374</ExternalID>
      <ExternalName>אלביט מערכות - סאיקלון בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>932.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185102843</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185102843</EntityIdExternalReferenceID>
          <EntityIdKey1>18041014215980</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813508</PaymentID>
      <Amount>1,606.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511430951</ExternalID>
      <ExternalName>511430951</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,606.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407744</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407744</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017855014</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813516</PaymentID>
      <Amount>5,331.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511348716</ExternalID>
      <ExternalName>שמיר הספקה טכנית בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>5,331.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187900558</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187900558</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018100857</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813560</PaymentID>
      <Amount>56,208.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510598923</ExternalID>
      <ExternalName>510598923 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>56,208.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180102038</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180102038</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018121333</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813600</PaymentID>
      <Amount>1,923.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520041997</ExternalID>
      <ExternalName>טאואר סמיקונדקטור בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,923.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407901</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407901</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018501716</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813636</PaymentID>
      <Amount>27,632.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520041997</ExternalID>
      <ExternalName>טאואר סמיקונדקטור בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>27,632.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407903</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407903</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018498004</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813667</PaymentID>
      <Amount>2,092.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520022971</ExternalID>
      <ExternalName>לפידות קפיטל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,092.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407848</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407848</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018111516</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813668</PaymentID>
      <Amount>110,286.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510787930</ExternalID>
      <ExternalName>פקר מתכות איכות בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>110,286.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188500922</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188500922</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018491322</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813672</PaymentID>
      <Amount>19,329.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512764945</ExternalID>
      <ExternalName>אורות העמקים בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>19,329.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402661</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402661</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018166593</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813680</PaymentID>
      <Amount>18,469.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510584808</ExternalID>
      <ExternalName>יהודה יצוא ויבוא בינלאומי (1971) בע</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>18,469.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407493</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407493</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018141562</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813682</PaymentID>
      <Amount>2,030.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512268855</ExternalID>
      <ExternalName>מדיגל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,030.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407742</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407742</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018112183</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813688</PaymentID>
      <Amount>4,948.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562477190</ExternalID>
      <ExternalName>562477190 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>4,948.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180301516</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180301516</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017108000</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813716</PaymentID>
      <Amount>1,485.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510520844</ExternalID>
      <ExternalName>פדקו ישראל בעמ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,485.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407743</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407743</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018085132</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813754</PaymentID>
      <Amount>294.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511604704</ExternalID>
      <ExternalName>פיניקס טכנולוגיות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>294.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403585</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403585</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018494037</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813900</PaymentID>
      <Amount>11,802.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>514998947</ExternalID>
      <ExternalName>יונייטד סיטס בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>11,802.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403112</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403112</EntityIdExternalReferenceID>
          <EntityIdKey1>18021016043984</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813953</PaymentID>
      <Amount>13,528.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510046873</ExternalID>
      <ExternalName>י א מיטווך ובניו בעמ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>13,528.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187406190</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187406190</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017597277</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813971</PaymentID>
      <Amount>47,688.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511429144</ExternalID>
      <ExternalName>ג.יל.ס. - קפה בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>47,688.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106991</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106991</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018129609</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814018</PaymentID>
      <Amount>79,793.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520032442</ExternalID>
      <ExternalName>קליל תעשיות בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>79,793.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402373</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402373</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018137594</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814101</PaymentID>
      <Amount>6,502.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520039934</ExternalID>
      <ExternalName>גן שמואל מזון בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>6,502.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106740</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106740</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017321165</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814106</PaymentID>
      <Amount>550.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510494321</ExternalID>
      <ExternalName>הנקל סוד בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>550.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403659</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403659</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018507077</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814178</PaymentID>
      <Amount>3,704.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>3,704.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104108</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104108</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018018588</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814306</PaymentID>
      <Amount>87,087.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562461756</ExternalID>
      <ExternalName>562461756 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>87,087.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180301415</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180301415</EntityIdExternalReferenceID>
          <EntityIdKey1>18041016250639</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814366</PaymentID>
      <Amount>11,328.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512166059</ExternalID>
      <ExternalName>אומריקס ביופרמצבטיקה בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>11,328.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407791</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407791</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018519064</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814386</PaymentID>
      <Amount>8,476.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511509168</ExternalID>
      <ExternalName>דין דיאגנוסטיקה בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>8,476.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403539</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403539</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018121929</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814396</PaymentID>
      <Amount>28,873.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510669260</ExternalID>
      <ExternalName>פחים לתעשיה המר בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>28,873.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188500999</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188500999</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018528073</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814405</PaymentID>
      <Amount>6,137.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510521560</ExternalID>
      <ExternalName>צח ציוד חקלאי עפולה בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>6,137.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402752</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402752</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018163293</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814437</PaymentID>
      <Amount>38,691.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513777813</ExternalID>
      <ExternalName>ישפאר מוצרי צריכה בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>38,691.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188500983</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188500983</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018525376</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814463</PaymentID>
      <Amount>2,957.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>570038430</ExternalID>
      <ExternalName>תעשיות גומי עין שמר אגודה חקלאית שי</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,957.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407887</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407887</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018125326</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814489</PaymentID>
      <Amount>845.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512310236</ExternalID>
      <ExternalName>הורן - הנדסה בקרה ותהליכים בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>845.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407797</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407797</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018118412</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814513</PaymentID>
      <Amount>28,145.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510914724</ExternalID>
      <ExternalName>רוזדור חברה למסחר בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>28,145.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403533</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403533</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018158715</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814525</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515125763</ExternalID>
      <ExternalName>לוקהיד מרטין ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104272</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104272</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017846401</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814546</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515125763</ExternalID>
      <ExternalName>לוקהיד מרטין ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104257</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104257</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017847466</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814554</PaymentID>
      <Amount>43,359.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>563468685</ExternalID>
      <ExternalName>563468685</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>43,359.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180200920</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180200920</EntityIdExternalReferenceID>
          <EntityIdKey1>18021016550673</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814566</PaymentID>
      <Amount>70,021.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511905929</ExternalID>
      <ExternalName>ויז'יוניקס בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>70,021.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403100</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403100</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017865732</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814599</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515125763</ExternalID>
      <ExternalName>לוקהיד מרטין ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104258</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104258</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017845262</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814607</PaymentID>
      <Amount>34,494.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513789008</ExternalID>
      <ExternalName>513789008 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>34,494.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181401994</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181401994</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017287135</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814614</PaymentID>
      <Amount>28,615.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510970080</ExternalID>
      <ExternalName>מחלבות רמת-הגולן בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>28,615.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001617</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001617</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018522431</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814635</PaymentID>
      <Amount>32,512.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562163006</ExternalID>
      <ExternalName>562163006</ExternalName>
      <CustomsUnit>673</CustomsUnit>
      <PaymentMethodAmount>32,512.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180202143</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180202143</EntityIdExternalReferenceID>
          <EntityIdKey1>18751018525038</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814702</PaymentID>
      <Amount>6,143.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511320160</ExternalID>
      <ExternalName>מובילק - בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>6,143.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403429</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403429</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018494441</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814749</PaymentID>
      <Amount>2,859.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510448392</ExternalID>
      <ExternalName>מנשה ברוך ושות בעמ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,859.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001795</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001795</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018498251</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814769</PaymentID>
      <Amount>18.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562560250</ExternalID>
      <ExternalName>שרכה  לכגרי  לתגארה  אלסיאראת</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>18.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402616</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402616</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018149243</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814789</PaymentID>
      <Amount>18.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562117259</ExternalID>
      <ExternalName>אלהנא  אלחדית'ה  ללתגארה</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>18.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402598</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402598</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018133965</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814799</PaymentID>
      <Amount>50,983.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510584808</ExternalID>
      <ExternalName>יהודה יצוא ויבוא בינלאומי (1971) בע</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>50,983.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407625</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407625</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018134476</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814800</PaymentID>
      <Amount>18.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562490482</ExternalID>
      <ExternalName>שרכה  אלאמירה  לתגארה  אלסיאראת</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>18.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402600</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402600</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018154342</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814815</PaymentID>
      <Amount>59.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562490482</ExternalID>
      <ExternalName>שרכה  אלאמירה  לתגארה  אלסיאראת</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>59.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402605</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402605</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018141661</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814822</PaymentID>
      <Amount>18.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562178806</ExternalID>
      <ExternalName>שרכה  אלהלאל  מותורז  לתגארה  אלסיא</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>18.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402657</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402657</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018156925</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814832</PaymentID>
      <Amount>69,997.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511487761</ExternalID>
      <ExternalName>יוניון מוטורס בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>69,997.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106730</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106730</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018129542</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814853</PaymentID>
      <Amount>135,926.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511487761</ExternalID>
      <ExternalName>יוניון מוטורס בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>135,926.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106731</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106731</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018119188</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814899</PaymentID>
      <Amount>15,059.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512875071</ExternalID>
      <ExternalName>אלביט מערכות אלקטרו-אופטיקה אלאופ ב</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>15,059.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104483</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104483</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018524221</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814915</PaymentID>
      <Amount>36,840.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520032442</ExternalID>
      <ExternalName>קליל תעשיות בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>36,840.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402844</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402844</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018154987</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814921</PaymentID>
      <Amount>866.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562490482</ExternalID>
      <ExternalName>שרכה  אלאמירה  לתגארה  אלסיאראת</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>866.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402857</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402857</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018289635</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814922</PaymentID>
      <Amount>129,090.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511001711</ExternalID>
      <ExternalName>ס. י. ר. א. מ. בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>129,090.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106544</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106544</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017602580</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814931</PaymentID>
      <Amount>114,781.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562437574</ExternalID>
      <ExternalName>562437574 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>673</CustomsUnit>
      <PaymentMethodAmount>114,781.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180301574</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180301574</EntityIdExternalReferenceID>
          <EntityIdKey1>18751018110849</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814936</PaymentID>
      <Amount>143,494.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511487761</ExternalID>
      <ExternalName>יוניון מוטורס בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>143,494.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106732</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106732</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018121747</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814989</PaymentID>
      <Amount>24,287.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562434811</ExternalID>
      <ExternalName>אלמשרובאת  אלוטניה</ExternalName>
      <CustomsUnit>673</CustomsUnit>
      <PaymentMethodAmount>24,287.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180301606</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180301606</EntityIdExternalReferenceID>
          <EntityIdKey1>18751018522043</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814992</PaymentID>
      <Amount>3,431.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510494321</ExternalID>
      <ExternalName>הנקל סוד בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>3,431.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180102266</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180102266</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018530004</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815034</PaymentID>
      <Amount>5,656.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512982257</ExternalID>
      <ExternalName>גליל כימיקלים בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>5,656.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180102088</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180102088</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018521169</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815070</PaymentID>
      <Amount>8,406.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>514093277</ExternalID>
      <ExternalName>514093277</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>8,406.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407738</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407738</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017837053</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815084</PaymentID>
      <Amount>2,708.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>4715348</ExternalID>
      <ExternalName>4715348</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,708.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187900519</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187900519</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018112092</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815127</PaymentID>
      <Amount>1,703.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511455685</ExternalID>
      <ExternalName>יפאורה - תבורי בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,703.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187107117</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187107117</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018167088</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815145</PaymentID>
      <Amount>59.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562475699</ExternalID>
      <ExternalName>שרכה  ורד  לתגארה  אלסיאראת</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>59.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402654</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402654</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018136380</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815152</PaymentID>
      <Amount>9,283.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520019373</ExternalID>
      <ExternalName>מפעלי ים המלח בעמ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>9,283.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106715</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106715</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017602218</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815168</PaymentID>
      <Amount>360,772.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520032442</ExternalID>
      <ExternalName>קליל תעשיות בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>360,772.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402843</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402843</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018151686</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815237</PaymentID>
      <Amount>141.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562600122</ExternalID>
      <ExternalName>שרכה  בירזית  ללאדויה</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>141.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106465</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106465</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018098945</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815252</PaymentID>
      <Amount>12,128.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511142168</ExternalID>
      <ExternalName>511142168</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>12,128.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403353</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403353</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018120640</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815341</PaymentID>
      <Amount>2,324.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510787930</ExternalID>
      <ExternalName>פקר מתכות איכות בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>2,324.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188500901</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188500901</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018517092</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815562</PaymentID>
      <Amount>44,977.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512982257</ExternalID>
      <ExternalName>גליל כימיקלים בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>44,977.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180101820</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180101820</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018529261</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815656</PaymentID>
      <Amount>273.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>273.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104355</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104355</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018550630</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815677</PaymentID>
      <Amount>20,978.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>514072305</ExternalID>
      <ExternalName>514072305</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>20,978.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187900499</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187900499</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017871995</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815759</PaymentID>
      <Amount>6,437.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510704786</ExternalID>
      <ExternalName>ויצנר צרכי בנין וגידור בעמ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>6,437.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180102001</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180102001</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018125920</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815771</PaymentID>
      <Amount>12,199.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>12,199.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104365</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104365</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018545028</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815815</PaymentID>
      <Amount>3,515.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>3,515.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104370</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104370</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018543320</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815824</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510432768</ExternalID>
      <ExternalName>מ אקרמן בעמ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180101093</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180101093</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018536381</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815834</PaymentID>
      <Amount>525.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>525.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104367</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104367</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018541233</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815845</PaymentID>
      <Amount>224.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>224.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104374</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104374</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018539740</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815854</PaymentID>
      <Amount>24,968.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562434811</ExternalID>
      <ExternalName>אלמשרובאת  אלוטניה</ExternalName>
      <CustomsUnit>673</CustomsUnit>
      <PaymentMethodAmount>24,968.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180300597</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180300597</EntityIdExternalReferenceID>
          <EntityIdKey1>18751018501393</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815860</PaymentID>
      <Amount>1,199.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,199.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104354</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104354</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018538536</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815870</PaymentID>
      <Amount>36,516.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>36,516.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104366</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104366</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018537769</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815873</PaymentID>
      <Amount>360.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>360.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104357</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104357</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018536902</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815886</PaymentID>
      <Amount>54,956.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>550216733</ExternalID>
      <ExternalName>יו.טי.אי.- פתרונות ניהול מלאי (איי.</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>54,956.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403321</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403321</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018511202</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815942</PaymentID>
      <Amount>23,072.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562464784</ExternalID>
      <ExternalName>שרכה  אללית'  ללאנמא</ExternalName>
      <CustomsUnit>673</CustomsUnit>
      <PaymentMethodAmount>23,072.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402862</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402862</EntityIdExternalReferenceID>
          <EntityIdKey1>18751018532034</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815949</PaymentID>
      <Amount>7,123.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562434811</ExternalID>
      <ExternalName>אלמשרובאת  אלוטניה</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>7,123.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180301480</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180301480</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017882638</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450815985</PaymentID>
      <Amount>24,689.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510719479</ExternalID>
      <ExternalName>מוסרות בעמ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>24,689.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4186900648</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4186900648</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018503688</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816039</PaymentID>
      <Amount>13,750.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515548907</ExternalID>
      <ExternalName>אייברי דניסון ישראל בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>13,750.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407924</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407924</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018542819</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816059</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511113631</ExternalID>
      <ExternalName>י. ד. עסקים (1986) בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106856</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106856</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017582428</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816060</PaymentID>
      <Amount>801.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562434811</ExternalID>
      <ExternalName>אלמשרובאת  אלוטניה</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>801.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180301563</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180301563</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018135424</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816121</PaymentID>
      <Amount>77.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511235434</ExternalID>
      <ExternalName>קמטק בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>77.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187107183</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187107183</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018156842</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816139</PaymentID>
      <Amount>24,060.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562434811</ExternalID>
      <ExternalName>אלמשרובאת  אלוטניה</ExternalName>
      <CustomsUnit>673</CustomsUnit>
      <PaymentMethodAmount>24,060.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180300888</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180300888</EntityIdExternalReferenceID>
          <EntityIdKey1>18751018501237</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816190</PaymentID>
      <Amount>45,460.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562554568</ExternalID>
      <ExternalName>562554568 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>673</CustomsUnit>
      <PaymentMethodAmount>45,460.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180301598</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180301598</EntityIdExternalReferenceID>
          <EntityIdKey1>18751018505253</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816211</PaymentID>
      <Amount>26,283.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520033374</ExternalID>
      <ExternalName>אלביט מערכות - סאיקלון בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>26,283.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104235</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104235</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018568418</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816227</PaymentID>
      <Amount>42,576.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511898942</ExternalID>
      <ExternalName>אינטרנשיונל פלייבורז אנד פרגרנסז אי</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>42,576.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001766</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001766</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017852078</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816231</PaymentID>
      <Amount>28,828.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513777813</ExternalID>
      <ExternalName>ישפאר מוצרי צריכה בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>28,828.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187106990</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187106990</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017869502</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816289</PaymentID>
      <Amount>11,517.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520004755</ExternalID>
      <ExternalName>פניציה מפעלי זכוכית בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>11,517.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403559</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403559</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018163285</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816297</PaymentID>
      <Amount>2,430.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520033374</ExternalID>
      <ExternalName>אלביט מערכות - סאיקלון בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,430.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104234</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104234</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018567535</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816309</PaymentID>
      <Amount>14,156.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513646430</ExternalID>
      <ExternalName>לנובו (ישראל) בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>14,156.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407942</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407942</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018535029</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816320</PaymentID>
      <Amount>12,625.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511730335</ExternalID>
      <ExternalName>עיסא ובניו - מרכז הדגים הטריים בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>12,625.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403640</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403640</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018488955</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816341</PaymentID>
      <Amount>6,917.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520042326</ExternalID>
      <ExternalName>גטר גרופ בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>6,917.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407109</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407109</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017591577</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816368</PaymentID>
      <Amount>17,034.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>902071380</ExternalID>
      <ExternalName>902071380 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>683</CustomsUnit>
      <PaymentMethodAmount>17,034.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402861</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402861</EntityIdExternalReferenceID>
          <EntityIdKey1>18811018542835</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816383</PaymentID>
      <Amount>95,374.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515144558</ExternalID>
      <ExternalName>לב-ים שיווק דגים בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>95,374.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403639</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403639</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018488583</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816395</PaymentID>
      <Amount>23,794.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515251593</ExternalID>
      <ExternalName>פולירם תעשיות פלסטיק בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>23,794.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001383</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001383</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018541258</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816426</PaymentID>
      <Amount>729,094.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513777813</ExternalID>
      <ExternalName>ישפאר מוצרי צריכה בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>729,094.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188500930</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188500930</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018538825</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816427</PaymentID>
      <Amount>8,265.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562507806</ExternalID>
      <ExternalName>שרכה  יורכס  ללתגארה  ואלשחנ  אלדול</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>8,265.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180202029</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180202029</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018536654</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816473</PaymentID>
      <Amount>7,220.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562143453</ExternalID>
      <ExternalName>562143453</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>7,220.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180202032</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180202032</EntityIdExternalReferenceID>
          <EntityIdKey1>18021017774124</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816494</PaymentID>
      <Amount>133,714.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>511524605</ExternalID>
      <ExternalName>קמהדע בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>133,714.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403664</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403664</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018521557</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816506</PaymentID>
      <Amount>29,339.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515144558</ExternalID>
      <ExternalName>לב-ים שיווק דגים בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>29,339.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403644</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403644</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018487346</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816513</PaymentID>
      <Amount>298,215.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510059744</ExternalID>
      <ExternalName>החברה האמריקאית ישראלית לגז בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>298,215.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407845</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407845</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018163129</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816517</PaymentID>
      <Amount>46,453.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515144558</ExternalID>
      <ExternalName>לב-ים שיווק דגים בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>46,453.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403542</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403542</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018015451</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816547</PaymentID>
      <Amount>26,433.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562527325</ExternalID>
      <ExternalName>562527325</ExternalName>
      <CustomsUnit>673</CustomsUnit>
      <PaymentMethodAmount>26,433.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180301583</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180301583</EntityIdExternalReferenceID>
          <EntityIdKey1>18751018103836</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816594</PaymentID>
      <Amount>21,022.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513602607</ExternalID>
      <ExternalName>אחים זנו דגים בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>21,022.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403633</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403633</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018489615</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816597</PaymentID>
      <Amount>3,350.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>3,350.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104368</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104368</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018570893</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816599</PaymentID>
      <Amount>18,516.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>18,516.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104317</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104317</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018570133</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816609</PaymentID>
      <Amount>2,563.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,563.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104377</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104377</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018569747</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816614</PaymentID>
      <Amount>1,152.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510059744</ExternalID>
      <ExternalName>החברה האמריקאית ישראלית לגז בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,152.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407846</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407846</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018166619</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816619</PaymentID>
      <Amount>3,945.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>3,945.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104373</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104373</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018569036</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816630</PaymentID>
      <Amount>1,213.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,213.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104376</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104376</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018568509</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816640</PaymentID>
      <Amount>26,866.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562600015</ExternalID>
      <ExternalName>562600015 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>26,866.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180201771</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180201771</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018538593</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816645</PaymentID>
      <Amount>1,145.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,145.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104375</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104375</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018567485</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816655</PaymentID>
      <Amount>15,280.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515144558</ExternalID>
      <ExternalName>לב-ים שיווק דגים בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>15,280.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403638</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403638</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018488252</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816657</PaymentID>
      <Amount>1,704.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520043027</ExternalID>
      <ExternalName>אלביט מערכות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>1,704.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4185104465</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4185104465</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018566917</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816721</PaymentID>
      <Amount>866.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520041997</ExternalID>
      <ExternalName>טאואר סמיקונדקטור בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>866.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408100</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408100</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018566198</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816724</PaymentID>
      <Amount>10,656.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513602607</ExternalID>
      <ExternalName>אחים זנו דגים בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>10,656.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403637</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403637</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018487981</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816729</PaymentID>
      <Amount>866.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520041997</ExternalID>
      <ExternalName>טאואר סמיקונדקטור בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>866.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408098</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408098</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018558989</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816748</PaymentID>
      <Amount>4,177.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520041997</ExternalID>
      <ExternalName>טאואר סמיקונדקטור בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>4,177.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408096</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408096</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018551141</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816810</PaymentID>
      <Amount>792.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520041997</ExternalID>
      <ExternalName>טאואר סמיקונדקטור בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>792.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408067</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408067</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018544328</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816814</PaymentID>
      <Amount>9,620.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520041997</ExternalID>
      <ExternalName>טאואר סמיקונדקטור בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>9,620.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408071</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408071</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018540805</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816817</PaymentID>
      <Amount>14,663.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513646430</ExternalID>
      <ExternalName>לנובו (ישראל) בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>14,663.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187407944</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187407944</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018529550</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816825</PaymentID>
      <Amount>2,284.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520041997</ExternalID>
      <ExternalName>טאואר סמיקונדקטור בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,284.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187408086</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187408086</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018536852</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816860</PaymentID>
      <Amount>11,059.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>540175668</ExternalID>
      <ExternalName>ימ""א 1990-ייצור מוצרי אריזה</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>11,059.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180102062</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180102062</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018536589</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816883</PaymentID>
      <Amount>552,643.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520016015</ExternalID>
      <ExternalName>דשנים וחמרים כימיים בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>552,643.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001688</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001688</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018538890</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816938</PaymentID>
      <Amount>1,035.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562178806</ExternalID>
      <ExternalName>שרכה  אלהלאל  מותורז  לתגארה  אלסיא</ExternalName>
      <CustomsUnit>673</CustomsUnit>
      <PaymentMethodAmount>1,035.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402821</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402821</EntityIdExternalReferenceID>
          <EntityIdKey1>18751018159820</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816965</PaymentID>
      <Amount>11,394.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>563103720</ExternalID>
      <ExternalName>שרכה  אלתורידאת  ואלח'דמאת  א</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>11,394.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180301607</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180301607</EntityIdExternalReferenceID>
          <EntityIdKey1>18041018537710</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450817039</PaymentID>
      <Amount>10,396.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>513777813</ExternalID>
      <ExternalName>ישפאר מוצרי צריכה בע""מ</ExternalName>
      <CustomsUnit>2</CustomsUnit>
      <PaymentMethodAmount>10,396.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188500962</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188500962</EntityIdExternalReferenceID>
          <EntityIdKey1>18021018575215</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450817105</PaymentID>
      <Amount>35,346.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515251593</ExternalID>
      <ExternalName>פולירם תעשיות פלסטיק בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>35,346.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001541</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001541</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018543965</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450817114</PaymentID>
      <Amount>63,698.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520016015</ExternalID>
      <ExternalName>דשנים וחמרים כימיים בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>63,698.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4188001864</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4188001864</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018574630</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450817128</PaymentID>
      <Amount>18,171.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>562554568</ExternalID>
      <ExternalName>562554568 יש לשלוף לקוח</ExternalName>
      <CustomsUnit>673</CustomsUnit>
      <PaymentMethodAmount>18,171.00</PaymentMethodAmount>
      <Bank>ה אס בי סי בנק פי אל סי</Bank>
      <Branch>101</Branch>
      <AccountNumber>26755485</AccountNumber>
      <AgentAccountPosessionX>Collapsed</AgentAccountPosessionX>
      <AgentAccountPosessionV>Visible</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180301617</EntityIdExternalReferenceID>
      <BankCode>23</BankCode>
      <AgentMasavPaymentResultHeader>ה אס בי סי בנק פי אל סי, סניף: 101, חשבון: 26755485  -  סה""כ סכום ששולם במס""ב: 6,044,053.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180301617</EntityIdExternalReferenceID>
          <EntityIdKey1>18751018570281</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812254</PaymentID>
      <Amount>774.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>515144194</ExternalID>
      <ExternalName>אנרקון טכנולוגיות בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>774.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>126</Branch>
      <AccountNumber>116122</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180403441</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 126, חשבון: 116122  -  סה""כ סכום ששולם במס""ב: 774.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180403441</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017839372</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813946</PaymentID>
      <Amount>4,738.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>512719303</ExternalID>
      <ExternalName>512719303</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>4,738.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>126</Branch>
      <AccountNumber>409074152</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4187900506</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 126, חשבון: 409074152  -  סה""כ סכום ששולם במס""ב: 4,738.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4187900506</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017378728</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811723</PaymentID>
      <Amount>2,490.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>510515752</ExternalID>
      <ExternalName>דוד לובינסקי בע""מ</ExternalName>
      <CustomsUnit>4</CustomsUnit>
      <PaymentMethodAmount>2,490.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>46</Branch>
      <AccountNumber>409764477</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4180102197</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 46, חשבון: 409764477  -  סה""כ סכום ששולם במס""ב: 2,490.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4180102197</EntityIdExternalReferenceID>
          <EntityIdKey1>18041017854124</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811777</PaymentID>
      <Amount>10,614.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>10,614.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402108</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402108</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018166783</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811791</PaymentID>
      <Amount>23,721.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>23,721.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402579</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402579</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018161362</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450811865</PaymentID>
      <Amount>18,940.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>18,940.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402750</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402750</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018148120</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450812705</PaymentID>
      <Amount>10,614.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>10,614.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402112</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402112</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018167138</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450813103</PaymentID>
      <Amount>27,797.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>27,797.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402127</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402127</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018163087</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814381</PaymentID>
      <Amount>20,480.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>20,480.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402798</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402798</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018117703</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814421</PaymentID>
      <Amount>20,480.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>20,480.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402796</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402796</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018121630</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814446</PaymentID>
      <Amount>20,480.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>20,480.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402827</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402827</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018168045</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814646</PaymentID>
      <Amount>22,442.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>22,442.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402662</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402662</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018527265</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814838</PaymentID>
      <Amount>20,249.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>20,249.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402797</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402797</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018136141</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814910</PaymentID>
      <Amount>20,249.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>20,249.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402826</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402826</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018167740</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450814929</PaymentID>
      <Amount>20,480.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>20,480.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402829</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402829</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018168128</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
    <AgentMasavPaymentResult>
      <PaymentProcess>1</PaymentProcess>
      <PaymentProcessName>הגשת הצהרה</PaymentProcessName>
      <PaymentID>450816345</PaymentID>
      <Amount>5,468.00</Amount>
      <PaymentType>1</PaymentType>
      <PaymentTypeName>מיסים</PaymentTypeName>
      <ExternalID>520038613</ExternalID>
      <ExternalName>טמבור בע""מ</ExternalName>
      <CustomsUnit>1</CustomsUnit>
      <PaymentMethodAmount>5,468.00</PaymentMethodAmount>
      <Bank>הבנק הבינלאומי הראשון לישראל בע""מ</Bank>
      <Branch>6</Branch>
      <AccountNumber>606065</AccountNumber>
      <AgentAccountPosessionX>Visible</AgentAccountPosessionX>
      <AgentAccountPosessionV>Collapsed</AgentAccountPosessionV>
      <EntityID>0</EntityID>
      <EntityIdExternalReferenceID>4181402751</EntityIdExternalReferenceID>
      <BankCode>31</BankCode>
      <AgentMasavPaymentResultHeader>הבנק הבינלאומי הראשון לישראל בע""מ, סניף: 6, חשבון: 606065  -  סה""כ סכום ששולם במס""ב: 242,014.00</AgentMasavPaymentResultHeader>
      <RelatedEntityList>
        <RelatedEntityResult>
          <EntityIdExternalReferenceID>4181402751</EntityIdExternalReferenceID>
          <EntityIdKey1>18011018532315</EntityIdKey1>
          <EntityType>1055</EntityType>
          <EntityTypeName>הצהרת יבוא</EntityTypeName>
        </RelatedEntityResult>
      </RelatedEntityList>
    </AgentMasavPaymentResult>
  </AgentMasavPaymentResultList>
</MasavPaymentsToAgentResponseData>";
        }
    }
}
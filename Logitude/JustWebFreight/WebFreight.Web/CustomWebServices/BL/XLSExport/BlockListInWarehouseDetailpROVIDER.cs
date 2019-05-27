using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.CustomWebServices.BL.XLSExport
{
    public class BlockListInWarehouseDetailProvider : IRequestProvider
    {
        public string GetRequest()
        {


            
            return @"<?xml version=""1.0""?>
<BlockListInWarehouseRequestParams xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <LoggingEnabled>true</LoggingEnabled>
  <LoggingUserId>1-5318</LoggingUserId>
  <IsFakeResponse>false</IsFakeResponse>
  <Tenant>1</Tenant>
  <RequestVIA>WebServiceInteractive</RequestVIA>
  <InterfaceTypeCode>8330</InterfaceTypeCode>
  <CustomsRequestsSheetId>b1cd092e-a8b8-4c89-986d-13d3fca34214</CustomsRequestsSheetId>
  <TransmitionDateTime xsi:nil=""true"" />
  <FutureSendDateTime xsi:nil=""true"" />
  <SuppressSplitWR>false</SuppressSplitWR>
  <PBId>b1cd092e-a8b8-4c89-986d-13d3fca34214</PBId>
  <ForcePersonalSign>false</ForcePersonalSign>
  <IsAngularClient>true</IsAngularClient>
  <DeleteStatus>false</DeleteStatus>
  <FromDate>2018-02-10T00:00:00Z</FromDate>
  <ToDate>2018-02-25T00:00:00Z</ToDate>
  <StorageSiteNumber>2220</StorageSiteNumber>
  <ShowResetBlocks>No</ShowResetBlocks>
</BlockListInWarehouseRequestParams>";
        }

        public string GetResponse()
        {
            return @"<?xml version=""1.0""?>
<BlockListInWarehouseResponseData xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <HasException>false</HasException>
  <UserMessage>לאתר אוברסיז קומרס בעמ קיימים 11גושים </UserMessage>
  <Succeeded>true</Succeeded>
  <ContinueProcessInBackground>false</ContinueProcessInBackground>
  <BlockListInWarehouseResultList>
    <BlockListInWarehouseResult>
      <DeclarationNumber>18021003199864</DeclarationNumber>
      <WarehouseBlockNumber>6418</WarehouseBlockNumber>
      <ImporterNumber>514854124</ImporterNumber>
      <ImporterTitle>סינודן בעמ</ImporterTitle>
      <OriginalOpeningDate>29.01.2018</OriginalOpeningDate>
      <OpeningDate>22.01.2018</OpeningDate>
      <LogicalPackagesQuantityBalance>0.00</LogicalPackagesQuantityBalance>
      <PhysicalPackagesQuantityBalance>124.00</PhysicalPackagesQuantityBalance>
      <Value>624,963.47</Value>
      <SpecialActivityTypeName />
    </BlockListInWarehouseResult>
    <BlockListInWarehouseResult>
      <DeclarationNumber>18021003712161</DeclarationNumber>
      <WarehouseBlockNumber>8618</WarehouseBlockNumber>
      <ImporterNumber>511119778</ImporterNumber>
      <ImporterTitle>ח. י. אלקטרוניקה ורכיבים בע מ</ImporterTitle>
      <OriginalOpeningDate>12.02.2018</OriginalOpeningDate>
      <OpeningDate>24.01.2018</OpeningDate>
      <LogicalPackagesQuantityBalance>0.00</LogicalPackagesQuantityBalance>
      <PhysicalPackagesQuantityBalance>0.00</PhysicalPackagesQuantityBalance>
      <Value>2,659,694.31</Value>
      <SpecialActivityTypeName />
    </BlockListInWarehouseResult>
    <BlockListInWarehouseResult>
      <DeclarationNumber>18021003735907</DeclarationNumber>
      <WarehouseBlockNumber>8818</WarehouseBlockNumber>
      <ImporterNumber>511119778</ImporterNumber>
      <ImporterTitle>ח. י. אלקטרוניקה ורכיבים בע מ</ImporterTitle>
      <OriginalOpeningDate>12.02.2018</OriginalOpeningDate>
      <OpeningDate>24.01.2018</OpeningDate>
      <LogicalPackagesQuantityBalance>0.00</LogicalPackagesQuantityBalance>
      <PhysicalPackagesQuantityBalance>4.00</PhysicalPackagesQuantityBalance>
      <Value>1,256,286.24</Value>
      <SpecialActivityTypeName />
    </BlockListInWarehouseResult>
    <BlockListInWarehouseResult>
      <DeclarationNumber>18021004824403</DeclarationNumber>
      <WarehouseBlockNumber>9118</WarehouseBlockNumber>
      <ImporterNumber>511119778</ImporterNumber>
      <ImporterTitle>ח. י. אלקטרוניקה ורכיבים בע מ</ImporterTitle>
      <OriginalOpeningDate>18.02.2018</OriginalOpeningDate>
      <OpeningDate>29.01.2018</OpeningDate>
      <LogicalPackagesQuantityBalance>0.00</LogicalPackagesQuantityBalance>
      <PhysicalPackagesQuantityBalance>6.00</PhysicalPackagesQuantityBalance>
      <Value>157,812.30</Value>
      <SpecialActivityTypeName />
    </BlockListInWarehouseResult>
    <BlockListInWarehouseResult>
      <DeclarationNumber>18021005495120</DeclarationNumber>
      <WarehouseBlockNumber>9318</WarehouseBlockNumber>
      <ImporterNumber>562450429</ImporterNumber>
      <ImporterTitle>שרכה  פאלכונ  תובכו</ImporterTitle>
      <OriginalOpeningDate>14.02.2018</OriginalOpeningDate>
      <OpeningDate>31.01.2018</OpeningDate>
      <LogicalPackagesQuantityBalance>280.00</LogicalPackagesQuantityBalance>
      <PhysicalPackagesQuantityBalance>280.00</PhysicalPackagesQuantityBalance>
      <Value>313,325.25</Value>
      <SpecialActivityTypeName />
    </BlockListInWarehouseResult>
    <BlockListInWarehouseResult>
      <DeclarationNumber>18021006450629</DeclarationNumber>
      <WarehouseBlockNumber>7818</WarehouseBlockNumber>
      <ImporterNumber>511119778</ImporterNumber>
      <ImporterTitle>ח. י. אלקטרוניקה ורכיבים בע מ</ImporterTitle>
      <OriginalOpeningDate>06.02.2018</OriginalOpeningDate>
      <OpeningDate>04.02.2018</OpeningDate>
      <LogicalPackagesQuantityBalance>0.00</LogicalPackagesQuantityBalance>
      <PhysicalPackagesQuantityBalance>0.00</PhysicalPackagesQuantityBalance>
      <Value>3,242,354.82</Value>
      <SpecialActivityTypeName />
    </BlockListInWarehouseResult>
    <BlockListInWarehouseResult>
      <DeclarationNumber>18021006547911</DeclarationNumber>
      <WarehouseBlockNumber>8418</WarehouseBlockNumber>
      <ImporterNumber>512193087</ImporterNumber>
      <ImporterTitle>אלקטרודן סחר בע מ</ImporterTitle>
      <OriginalOpeningDate>11.02.2018</OriginalOpeningDate>
      <OpeningDate>08.02.2018</OpeningDate>
      <LogicalPackagesQuantityBalance>0.00</LogicalPackagesQuantityBalance>
      <PhysicalPackagesQuantityBalance>-1.00</PhysicalPackagesQuantityBalance>
      <Value>99,746.36</Value>
      <SpecialActivityTypeName />
    </BlockListInWarehouseResult>
    <BlockListInWarehouseResult>
      <DeclarationNumber>18021008606061</DeclarationNumber>
      <WarehouseBlockNumber>9818</WarehouseBlockNumber>
      <ImporterNumber>514936038</ImporterNumber>
      <ImporterTitle>ח.י. פתרונות חכמים בע מ</ImporterTitle>
      <OriginalOpeningDate>19.02.2018</OriginalOpeningDate>
      <OpeningDate>15.02.2018</OpeningDate>
      <LogicalPackagesQuantityBalance>0.00</LogicalPackagesQuantityBalance>
      <PhysicalPackagesQuantityBalance>0.00</PhysicalPackagesQuantityBalance>
      <Value>72,781.22</Value>
      <SpecialActivityTypeName />
    </BlockListInWarehouseResult>
    <BlockListInWarehouseResult>
      <DeclarationNumber>18021008611541</DeclarationNumber>
      <WarehouseBlockNumber>10018</WarehouseBlockNumber>
      <ImporterNumber>511119778</ImporterNumber>
      <ImporterTitle>ח. י. אלקטרוניקה ורכיבים בע מ</ImporterTitle>
      <OriginalOpeningDate>21.02.2018</OriginalOpeningDate>
      <OpeningDate>20.02.2018</OpeningDate>
      <LogicalPackagesQuantityBalance>0.00</LogicalPackagesQuantityBalance>
      <PhysicalPackagesQuantityBalance>0.00</PhysicalPackagesQuantityBalance>
      <Value>689,543.17</Value>
      <SpecialActivityTypeName />
    </BlockListInWarehouseResult>
    <BlockListInWarehouseResult>
      <DeclarationNumber>18021008894527</DeclarationNumber>
      <WarehouseBlockNumber>9718</WarehouseBlockNumber>
      <ImporterNumber>512193087</ImporterNumber>
      <ImporterTitle>אלקטרודן סחר בע מ</ImporterTitle>
      <OriginalOpeningDate>18.02.2018</OriginalOpeningDate>
      <OpeningDate>14.02.2018</OpeningDate>
      <LogicalPackagesQuantityBalance>0.00</LogicalPackagesQuantityBalance>
      <PhysicalPackagesQuantityBalance>-182.00</PhysicalPackagesQuantityBalance>
      <Value>1,224,079.53</Value>
      <SpecialActivityTypeName />
    </BlockListInWarehouseResult>
    <BlockListInWarehouseResult>
      <DeclarationNumber>18021009319102</DeclarationNumber>
      <WarehouseBlockNumber>10318</WarehouseBlockNumber>
      <ImporterNumber>511119778</ImporterNumber>
      <ImporterTitle>ח. י. אלקטרוניקה ורכיבים בע מ</ImporterTitle>
      <OriginalOpeningDate>22.02.2018</OriginalOpeningDate>
      <OpeningDate>16.02.2018</OpeningDate>
      <LogicalPackagesQuantityBalance>0.00</LogicalPackagesQuantityBalance>
      <PhysicalPackagesQuantityBalance>0.00</PhysicalPackagesQuantityBalance>
      <Value>1,207,909.50</Value>
      <SpecialActivityTypeName />
    </BlockListInWarehouseResult>
  </BlockListInWarehouseResultList>
  <NumberOfBlocksInList>11</NumberOfBlocksInList>
</BlockListInWarehouseResponseData>";
        }
    }
}
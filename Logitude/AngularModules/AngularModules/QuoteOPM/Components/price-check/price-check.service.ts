import { Injectable, isDevMode } from '@angular/core';
import { Xml2jsonService } from 'Infrastructure/Services/xml2json/xml2json.service';
import { AmitalGatewayUtil, UnifreightMessageM } from 'Infrastructure/Utilities/AmitalGatewayUtil';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { QuoteOPPropertiesPM } from 'QuoteOPM/EntityPMs/QuoteOPPropertiesPM';
import { Subscription } from 'rxjs';
import { PriceCheckComponent } from './price-check.component';

@Injectable()
export class PriceCheckService {

  constructor(
    private dialogService: DialogService,
    private xml2Json: Xml2jsonService,
  ) { }

  async open(quote: QuoteOPPM): Promise<DynamicDialogRef> {
    const config: DynamicDialogConfig = {}
    config.width = '1440px';
    config.height = '1230px';
    config.showHeader = false;
    config.styleClass = 'price-check';
    // if (isDevMode())
    //   config.data = await this.getPricesTest(quote);
    // else
      config.data = await this.getPrices(quote);

    return this.dialogService.open(PriceCheckComponent, config);
  }

  private async getPrices(quote: QuoteOPPM): Promise<PriceChekRootResponse> {
    const logitudeEntity = 'QuoteOP';
    const logitudeViewModel = 'QuotesComponent';
    var unifreightMessageM = new UnifreightMessageM();
    unifreightMessageM.UnifreightEntity = 'GPRHEAD';
    unifreightMessageM.UnifreightEntityNumber = '-1';
    unifreightMessageM.LogitudeEntity = logitudeEntity;
    unifreightMessageM.LogitudeEntityNumber = quote.Id;
    unifreightMessageM.LogitudeViewModel = logitudeViewModel;
    unifreightMessageM.Requset = [];
    unifreightMessageM.Requset.push(["PriceCheckRequest", this.createRequestXml(quote)]);

    const responsePromise = new Promise<PriceChekRootResponse>((resolve, reject) => {
      const sub: Subscription = AmitalGatewayUtil.Instance.UnifaceRequestArrived
        .subscribe(
          (mess: UnifreightMessageM) => {
            const IsMatchUnifreightCallbackCommand: boolean = (
              mess.LogitudeEntity == logitudeEntity &&
              mess.LogitudeEntityNumber == quote.Id &&
              mess.LogitudeViewModel == logitudeViewModel);

            if (IsMatchUnifreightCallbackCommand) {
              sub.unsubscribe();
              SessionLocator.SelectedSession.StopBusyIndicator();
              let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
              if (sBool) {
                const xmlString: string = mess.Requset.find(x => x[0] === 'Response.PriceCheckResponse')[1];
                const prices: PriceChekRootResponse = this.xml2Json.decodeXmlStr2Json(xmlString);
                this.fixData(prices);
                resolve(prices as any)
              } else
                reject(mess as any);

            }
          }, (err) => {
            reject(err);
            SessionLocator.SelectedSession.StopBusyIndicator();
          }
        );
    });

    SessionLocator.SelectedSession.StartBusyIndicator("RunPriceCheckQuery...");

    AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
      "QuotesComponent.SendRequestToUnifreightAsync",
      "GPRHMAIN.LogitudeTask",
      "RunPriceCheckQuery",
      unifreightMessageM,
      "Feature 148708: פניה ליוניפרייט לקבלת מחירים");

    return responsePromise;
  }

  // private async getPricesTest(quote: any): Promise<PriceChekRootResponse> {
  //   return await new Promise<PriceChekRootResponse>((resolve) => {
  //     const prices: PriceChekRootResponse = this.xml2Json.decodeXmlStr2Json(xmlPriceString)
  //     this.fixData(prices)
  //     resolve(prices);
  //   })
  // }

  private calculateProducteCode(direction: string, transport: string) {
    if (direction === 'E') {
      if (transport === 'A')
        return 'AE'
      else if (transport === 'O')
        return 'OE'
    } else if (direction === 'I') {
      if (transport === 'A')
        return 'AI'
      else if (transport === 'O')
        return 'OI'
    }
  }

  private createRequestXml(quote: QuoteOPPM): string {
    let xml: string = '';

    quote.QuoteProperties.forEach((property: QuoteOPPropertiesPM, i: number) => {
      xml +=
        `
      <PriceChekRequest>
        <QuoteProperties>
          <Order>???</Order>
          <Client>${quote.CustomerId}</Client>
          <From>${property.FromPortId}</From>
          <To>${property.ToPortId}</To>
          <CarrierCode>${property.MainCarriageCarrierId}LH</CarrierCode>
          <ProductCode>${this.calculateProducteCode(quote.DirectionId, quote.TransportModeId)}</ProductCode>
          <CostTariffUseCodes>0000CR,CRE,AG</CostTariffUseCodes>
          <SaleTariffUseCodes>0000EX,EXD,AG</SaleTariffUseCodes>
          <StartDate>${this.fixDateFormat(quote.StartDate as any)}</StartDate>
          <GrossWeightAmount>${quote.GrossWeight}</GrossWeightAmount>
          <GrossWeightUOM>${quote.GrossWeightUnitCode}</GrossWeightUOM>
          <VolumeAmount>${quote.Volume}</VolumeAmount>
          <VolumeUOM>${quote.VolumeUnitCode}</VolumeUOM>
          <ChargeableWeightAmount>$${quote.ChargeableWeight}</ChargeableWeightAmount>
          <ChargeableWeightUOM>${quote.ChargeableWeightUnitCode}</ChargeableWeightUOM>
          <QuoteType>${quote.QuoteTypeCode}SpotRate</QuoteType>
          <Incoterms>${property.IncotermId}</Incoterms>
          <Currency>${quote.SaleCurrencyId}</Currency>
          <SpecialService>${property.SpecialServiceID}</SpecialService>
          <MaxOffers>3</MaxOffers>
          <Cheapest>True</Cheapest>
          <Fastest>True</Fastest>
        </QuoteProperties>
      </PriceChekRequest>
    `
    })
    return xml;

  }

  private fixData(prices: PriceChekRootResponse) {
    prices.PriceChekResponse.Offers.Offer.reduce((previousValue: any[], offer: Offer) => {
      const haveMuchResult: boolean = !!(<any>offer.Result).length;
      const offers: Offer[] = [];
      if (haveMuchResult) {
        const results: Result[] = offer.Result as any;
        results.forEach(result => {
          const copyOffer: Offer = JSON.parse(JSON.stringify(offer));
          copyOffer.Result = result
          offers.push(copyOffer)
        })
      }

      return previousValue.concat([offers]);
    }, []);

    prices.PriceChekResponse.Offers.Offer.forEach(x => {
      if ((<any>x.Result).length)
        x.Result = (<any>x.Result)[0];
    })
  }

  private fixDateFormat(date: string): string {
    return new Date("2021-11-18T00:00:00Z").toJSON().slice(0, 10).split('-').reverse().join('/');
  }
}


export interface QuoteProperties {
  Order: string;
  Client: string;
  From: string;
  To: string;
  CarrierCode: string;
  ProductCode: string;
  CostTariffUseCodes: string;
  SaleTariffUseCodes: string;
  StartDate: string;
  GrossWeightAmount: string;
  GrossWeightUOM: string;
  VolumeAmount: string;
  VolumeUOM: string;
  ChargeableWeightAmount: string;
  ChargeableWeightUOM: string;
  QuoteType: string;
  Incoterms: string;
  Currency: string;
  SpecialService: string;
  MaxOffers: string;
  Cheapest: string;
  Fastest: string;
}

export interface Offer {
  QuoteProperties: QuoteProperties;
  Result: Result;
}

export interface Offers {
  Offer: Offer[];
}

export interface PriceChekResponse {
  Offers: Offers;
}

export interface PriceChekRootResponse {
  PriceChekResponse: PriceChekResponse;
}


export interface Summary {
  Currency: string;
  CarrierCode: string;
  TotalCost: string;
  IsCostAllIn: string;
  TotalSale: string;
  IsSaleAllIn: string;
  EstimatedProfit: string;
  HasRemarks: string;
  DirectFlight: string;
  Incoterms: string;
  SpecialService: string;
  Frequency: string;
  Cheapest: string;
  Fastest: string;
  TransitTime: string;
  [key: string]: any;
}

export interface Tariff {
  Amount: string;
  Rate: string;
  Currency: string;
  ValidDate: string;
  Remark: string;
  OwnerName: string;
  TariffNumber: string;
  Incoterms: string;
  CarrierCode: string;
  LastUsed: string;
  UpdatedDate: string;
  TariffType: string;
  CalcBreakCode: string;
  CalcBreakName: string;
  StepBreakCode: string;
  StepBreakName: string;
  Measurement: string;
  WeightUnit: string;
  Minimum: string;
  Maximum: string;
  Step: Step[];
}

export interface Service {
  ServiceCode: string;
  Name: string;
  CostTariff: Tariff;
  SaleTariff: Tariff;
}

export interface Result {
  Order: string;
  Summary: Summary;
  Service: Service[];
}

export interface Step {
  From: string;
  To: string;
  Rate: string;
  IsAllin: string;
}

export const xmlPriceString: string = `&lt;PriceChekResponse&gt;	&lt;Offers&gt;		&lt;Offer&gt;			&lt;QuoteProperties&gt;				&lt;Order&gt;1&lt;/Order&gt;				&lt;Client&gt;10000046&lt;/Client&gt;				&lt;From&gt;TLV&lt;/From&gt;				&lt;To&gt;FRA&lt;/To&gt;				&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;				&lt;ProductCode&gt;AE&lt;/ProductCode&gt;				&lt;CostTariffUseCodes&gt;CR,CRE,AG&lt;/CostTariffUseCodes&gt;				&lt;SaleTariffUseCodes&gt;EX,EXD,AG&lt;/SaleTariffUseCodes&gt;				&lt;StartDate&gt;06/05/2021&lt;/StartDate&gt;				&lt;GrossWeightAmount&gt;100&lt;/GrossWeightAmount&gt;				&lt;GrossWeightUOM&gt;KG&lt;/GrossWeightUOM&gt;				&lt;VolumeAmount&gt;0.3&lt;/VolumeAmount&gt;				&lt;VolumeUOM&gt;CBM&lt;/VolumeUOM&gt;				&lt;ChargeableWeightAmount&gt;100&lt;/ChargeableWeightAmount&gt;				&lt;ChargeableWeightUOM&gt;KG&lt;/ChargeableWeightUOM&gt;				&lt;QuoteType&gt;SpotRate&lt;/QuoteType&gt;				&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;				&lt;Currency&gt;USD&lt;/Currency&gt;				&lt;SpecialService&gt;&lt;/SpecialService&gt;				&lt;MaxOffers&gt;3&lt;/MaxOffers&gt;				&lt;Cheapest&gt;True&lt;/Cheapest&gt;				&lt;Fastest&gt;False&lt;/Fastest&gt;			&lt;/QuoteProperties&gt;			&lt;Result&gt;				&lt;Order&gt;1&lt;/Order&gt;				&lt;Summary&gt;					&lt;Currency&gt;USD&lt;/Currency&gt;					&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;					&lt;TotalCost&gt;450.00&lt;/TotalCost&gt;					&lt;IsCostAllIn&gt;True&lt;/IsCostAllIn&gt;					&lt;TotalSale&gt;575.00&lt;/TotalSale&gt;					&lt;IsSaleAllIn&gt;True&lt;/IsSaleAllIn&gt;					&lt;EstimatedProfit&gt;125.00&lt;/EstimatedProfit&gt;					&lt;HasRemarks&gt;&lt;/HasRemarks&gt;					&lt;DirectFlight&gt;True&lt;/DirectFlight&gt;					&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;					&lt;SpecialService&gt;Regular&lt;/SpecialService&gt;					&lt;Frequency&gt;Daily&lt;/Frequency&gt;					&lt;Cheapest&gt;True&lt;/Cheapest&gt;					&lt;Fastest&gt;False&lt;/Fastest&gt;					&lt;TransitTime&gt;1&lt;/TransitTime&gt;								&lt;/Summary&gt;				&lt;Service&gt;					&lt;ServiceCode&gt;F&lt;/ServiceCode&gt;					&lt;Name&gt;Freight&lt;/Name&gt;					&lt;CostTariff&gt;						&lt;Amount&gt;400.00&lt;/Amount&gt;						&lt;Rate&gt;4.0000&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;LH&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;CR&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;M&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Minimum Price&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;C&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Chargable Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;CHWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;45.00&lt;/To&gt;							&lt;Rate&gt;5.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;50.00&lt;/From&gt;							&lt;To&gt;150.00&lt;/To&gt;							&lt;Rate&gt;4.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;150.00&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;3.0000&lt;/Rate&gt;							&lt;IsAllin&gt;True&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/CostTariff&gt;					&lt;SaleTariff&gt;						&lt;Amount&gt;500.00&lt;/Amount&gt;						&lt;Rate&gt;5.0000&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;REG1&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;EXD&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;S&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Only Appropriate Level&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;C&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Chargable Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;CHWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;45.00&lt;/To&gt;							&lt;Rate&gt;6.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;50.00&lt;/From&gt;							&lt;To&gt;150.00&lt;/To&gt;							&lt;Rate&gt;5.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;150.00&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;4.0000&lt;/Rate&gt;							&lt;IsAllin&gt;True&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/SaleTariff&gt;				&lt;/Service&gt;				&lt;Service&gt;					&lt;ServiceCode&gt;FU&lt;/ServiceCode&gt;					&lt;Name&gt;Fuel&lt;/Name&gt;					&lt;CostTariff&gt;						&lt;Amount&gt;50.00&lt;/Amount&gt;						&lt;Rate&gt;0.5000&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;please note the pre covid price&lt;/Remark&gt;						&lt;OwnerName&gt;LH&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;CR&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;M&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Minimum Price&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;G&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Gross Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;GRWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;0.5000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/CostTariff&gt;					&lt;SaleTariff&gt;						&lt;Amount&gt;50.00&lt;/Amount&gt;						&lt;Rate&gt;0.5000&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;REG1&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;EXD&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;S&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Only Appropriate Level&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;C&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Chargable Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;CHWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;50.00&lt;/To&gt;							&lt;Rate&gt;0.5000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/SaleTariff&gt;				&lt;/Service&gt;				&lt;Service&gt;					&lt;ServiceCode&gt;AW&lt;/ServiceCode&gt;					&lt;Name&gt;AWB Fee&lt;/Name&gt;					&lt;CostTariff&gt;					&lt;/CostTariff&gt;					&lt;SaleTariff&gt;						&lt;Amount&gt;25.00&lt;/Amount&gt;						&lt;Rate&gt;25.00&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;REG1&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;&lt;/Incoterms&gt;						&lt;CarrierCode&gt;&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;EXD&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;F&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Fix&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;N&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;None&lt;/StepBreakName&gt;						&lt;Measurement&gt;FIXD&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;25&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/SaleTariff&gt;				&lt;/Service&gt;			&lt;/Result&gt;			&lt;Result&gt;				&lt;Order&gt;1&lt;/Order&gt;				&lt;Summary&gt;					&lt;Currency&gt;USD&lt;/Currency&gt;					&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;					&lt;TotalCost&gt;900.00&lt;/TotalCost&gt;					&lt;IsCostAllIn&gt;True&lt;/IsCostAllIn&gt;					&lt;TotalSale&gt;1150.00&lt;/TotalSale&gt;					&lt;IsSaleAllIn&gt;True&lt;/IsSaleAllIn&gt;					&lt;EstimatedProfit&gt;250.00&lt;/EstimatedProfit&gt;					&lt;HasRemarks&gt;&lt;/HasRemarks&gt;					&lt;DirectFlight&gt;True&lt;/DirectFlight&gt;					&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;					&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;					&lt;SpecialService&gt;Flash&lt;/SpecialService&gt;					&lt;Frequency&gt;Daily&lt;/Frequency&gt;					&lt;Cheapest&gt;True&lt;/Cheapest&gt;					&lt;Fastest&gt;False&lt;/Fastest&gt;					&lt;TransitTime&gt;1&lt;/TransitTime&gt;				&lt;/Summary&gt;				&lt;Service&gt;					&lt;ServiceCode&gt;F&lt;/ServiceCode&gt;					&lt;Name&gt;Freight&lt;/Name&gt;					&lt;CostTariff&gt;						&lt;Amount&gt;800.00&lt;/Amount&gt;						&lt;Rate&gt;8.0000&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;LH&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;CR&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;M&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Minimum Price&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;C&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Chargable Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;CHWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;45.00&lt;/To&gt;							&lt;Rate&gt;10.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;50.00&lt;/From&gt;							&lt;To&gt;150.00&lt;/To&gt;							&lt;Rate&gt;8.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;150.00&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;6.0000&lt;/Rate&gt;							&lt;IsAllin&gt;True&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/CostTariff&gt;					&lt;SaleTariff&gt;						&lt;Amount&gt;1000.00&lt;/Amount&gt;						&lt;Rate&gt;10.0000&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;REG1&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;EXD&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;S&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Only Appropriate Level&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;C&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Chargable Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;CHWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;45.00&lt;/To&gt;							&lt;Rate&gt;12.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;50.00&lt;/From&gt;							&lt;To&gt;150.00&lt;/To&gt;							&lt;Rate&gt;10.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;150.00&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;8.0000&lt;/Rate&gt;							&lt;IsAllin&gt;True&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/SaleTariff&gt;				&lt;/Service&gt;				&lt;Service&gt;					&lt;ServiceCode&gt;FU&lt;/ServiceCode&gt;					&lt;Name&gt;Fuel&lt;/Name&gt;					&lt;CostTariff&gt;						&lt;Amount&gt;100.00&lt;/Amount&gt;						&lt;Rate&gt;1.0000&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;please note the pre covid price&lt;/Remark&gt;						&lt;OwnerName&gt;LH&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;CR&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;M&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Minimum Price&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;G&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Gross Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;GRWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;1.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/CostTariff&gt;					&lt;SaleTariff&gt;						&lt;Amount&gt;100.00&lt;/Amount&gt;						&lt;Rate&gt;1.0000&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;REG1&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LH&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;EXD&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;S&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Only Appropriate Level&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;C&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Chargable Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;CHWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;50.00&lt;/To&gt;							&lt;Rate&gt;1.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/SaleTariff&gt;				&lt;/Service&gt;				&lt;Service&gt;					&lt;ServiceCode&gt;AW&lt;/ServiceCode&gt;					&lt;Name&gt;AWB Fee&lt;/Name&gt;					&lt;CostTariff&gt;					&lt;/CostTariff&gt;					&lt;SaleTariff&gt;						&lt;Amount&gt;50.00&lt;/Amount&gt;						&lt;Rate&gt;50.00&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;REG1&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;&lt;/Incoterms&gt;						&lt;CarrierCode&gt;&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;EXD&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;F&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Fix&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;N&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;None&lt;/StepBreakName&gt;						&lt;Measurement&gt;FIXD&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;50&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/SaleTariff&gt;				&lt;/Service&gt;			&lt;/Result&gt;		&lt;/Offer&gt;		&lt;Offer&gt;			&lt;QuoteProperties&gt;				&lt;Order&gt;2&lt;/Order&gt;				&lt;Client&gt;10000046&lt;/Client&gt;				&lt;From&gt;TLV&lt;/From&gt;				&lt;To&gt;FRA&lt;/To&gt;				&lt;CarrierCode&gt;LY&lt;/CarrierCode&gt;				&lt;ProductCode&gt;AE&lt;/ProductCode&gt;				&lt;CostTariffUseCodes&gt;CR,CRE,AG&lt;/CostTariffUseCodes&gt;				&lt;SaleTariffUseCodes&gt;EX,EXD,AG&lt;/SaleTariffUseCodes&gt;				&lt;StartDate&gt;06/05/2021&lt;/StartDate&gt;				&lt;GrossWeightAmount&gt;100&lt;/GrossWeightAmount&gt;				&lt;GrossWeightUOM&gt;KG&lt;/GrossWeightUOM&gt;				&lt;VolumeAmount&gt;0.3&lt;/VolumeAmount&gt;				&lt;VolumeUOM&gt;CBM&lt;/VolumeUOM&gt;				&lt;ChargeableWeightAmount&gt;100&lt;/ChargeableWeightAmount&gt;				&lt;ChargeableWeightUOM&gt;KG&lt;/ChargeableWeightUOM&gt;				&lt;QuoteType&gt;SpotRate&lt;/QuoteType&gt;				&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;				&lt;Currency&gt;USD&lt;/Currency&gt;				&lt;SpecialService&gt;&lt;/SpecialService&gt;				&lt;MaxOffers&gt;3&lt;/MaxOffers&gt;				&lt;Cheapest&gt;True&lt;/Cheapest&gt;				&lt;Fastest&gt;False&lt;/Fastest&gt;			&lt;/QuoteProperties&gt;			&lt;Result&gt;				&lt;Order&gt;2&lt;/Order&gt;				&lt;Summary&gt;					&lt;Currency&gt;USD&lt;/Currency&gt;					&lt;CarrierCode&gt;LY&lt;/CarrierCode&gt;					&lt;TotalCost&gt;575.00&lt;/TotalCost&gt;					&lt;IsCostAllIn&gt;True&lt;/IsCostAllIn&gt;					&lt;TotalSale&gt;700.00&lt;/TotalSale&gt;					&lt;IsSaleAllIn&gt;True&lt;/IsSaleAllIn&gt;					&lt;EstimatedProfit&gt;125.00&lt;/EstimatedProfit&gt;					&lt;HasRemarks&gt;&lt;/HasRemarks&gt;					&lt;DirectFlight&gt;True&lt;/DirectFlight&gt;					&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;					&lt;SpecialService&gt;Regular&lt;/SpecialService&gt;					&lt;Frequency&gt;Daily&lt;/Frequency&gt;					&lt;Cheapest&gt;True&lt;/Cheapest&gt;					&lt;Fastest&gt;False&lt;/Fastest&gt;					&lt;TransitTime&gt;1&lt;/TransitTime&gt;				&lt;/Summary&gt;				&lt;Service&gt;					&lt;ServiceCode&gt;F&lt;/ServiceCode&gt;					&lt;Name&gt;Freight&lt;/Name&gt;					&lt;CostTariff&gt;						&lt;Amount&gt;500.00&lt;/Amount&gt;						&lt;Rate&gt;5.0000&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;LY&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LY&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;CR&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;M&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Minimum Price&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;C&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Chargable Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;CHWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;45.00&lt;/To&gt;							&lt;Rate&gt;6.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;50.00&lt;/From&gt;							&lt;To&gt;150.00&lt;/To&gt;							&lt;Rate&gt;5.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;150.00&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;4.0000&lt;/Rate&gt;							&lt;IsAllin&gt;True&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/CostTariff&gt;					&lt;SaleTariff&gt;						&lt;Amount&gt;600.00&lt;/Amount&gt;						&lt;Rate&gt;6.0000&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;REG1&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LY&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;EXD&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;S&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Only Appropriate Level&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;C&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Chargable Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;CHWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;45.00&lt;/To&gt;							&lt;Rate&gt;7.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;50.00&lt;/From&gt;							&lt;To&gt;150.00&lt;/To&gt;							&lt;Rate&gt;6.0000&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;						&lt;Step&gt;							&lt;From&gt;150.00&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;5.0000&lt;/Rate&gt;							&lt;IsAllin&gt;True&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/SaleTariff&gt;				&lt;/Service&gt;				&lt;Service&gt;					&lt;ServiceCode&gt;FU&lt;/ServiceCode&gt;					&lt;Name&gt;Fuel&lt;/Name&gt;					&lt;CostTariff&gt;						&lt;Amount&gt;75.00&lt;/Amount&gt;						&lt;Rate&gt;0.7500&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;please note the pre covid price&lt;/Remark&gt;						&lt;OwnerName&gt;LY&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LY&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;CR&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;M&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Minimum Price&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;G&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Gross Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;GRWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;0.7500&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/CostTariff&gt;					&lt;SaleTariff&gt;						&lt;Amount&gt;75.00&lt;/Amount&gt;						&lt;Rate&gt;0.7500&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;REG1&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;CIF&lt;/Incoterms&gt;						&lt;CarrierCode&gt;LY&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;EXD&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;S&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Only Appropriate Level&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;C&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;Chargable Weight&lt;/StepBreakName&gt;						&lt;Measurement&gt;CHWT&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;0.00&lt;/From&gt;							&lt;To&gt;50.00&lt;/To&gt;							&lt;Rate&gt;0.7500&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/SaleTariff&gt;				&lt;/Service&gt;				&lt;Service&gt;					&lt;ServiceCode&gt;AW&lt;/ServiceCode&gt;					&lt;Name&gt;AWB Fee&lt;/Name&gt;					&lt;CostTariff&gt;					&lt;/CostTariff&gt;					&lt;SaleTariff&gt;						&lt;Amount&gt;25.00&lt;/Amount&gt;						&lt;Rate&gt;25.00&lt;/Rate&gt;						&lt;Currency&gt;USD&lt;/Currency&gt;						&lt;ValidDate&gt;15/05/2021&lt;/ValidDate&gt;						&lt;Remark&gt;&lt;/Remark&gt;						&lt;OwnerName&gt;REG1&lt;/OwnerName&gt;						&lt;TariffNumber&gt;840350&lt;/TariffNumber&gt;						&lt;Incoterms&gt;&lt;/Incoterms&gt;						&lt;CarrierCode&gt;&lt;/CarrierCode&gt;						&lt;LastUsed&gt;01/05/2020&lt;/LastUsed&gt;						&lt;UpdatedDate&gt;01/07/2018&lt;/UpdatedDate&gt;						&lt;TariffType&gt;EXD&lt;/TariffType&gt;						&lt;CalcBreakCode&gt;F&lt;/CalcBreakCode&gt;						&lt;CalcBreakName&gt;Fix&lt;/CalcBreakName&gt;						&lt;StepBreakCode&gt;N&lt;/StepBreakCode&gt;						&lt;StepBreakName&gt;None&lt;/StepBreakName&gt;						&lt;Measurement&gt;FIXD&lt;/Measurement&gt;						&lt;WeightUnit&gt;KG&lt;/WeightUnit&gt;						&lt;Minimum&gt;20&lt;/Minimum&gt;						&lt;Maximum&gt;&lt;/Maximum&gt;						&lt;Step&gt;							&lt;From&gt;&lt;/From&gt;							&lt;To&gt;&lt;/To&gt;							&lt;Rate&gt;25&lt;/Rate&gt;							&lt;IsAllin&gt;False&lt;/IsAllin&gt;						&lt;/Step&gt;					&lt;/SaleTariff&gt;				&lt;/Service&gt;			&lt;/Result&gt;		&lt;/Offer&gt;	&lt;/Offers&gt;&lt;/PriceChekResponse&gt;`
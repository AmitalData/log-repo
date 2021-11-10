import { Injectable } from '@angular/core';
import { Xml2jsonService } from 'Infrastructure/Services/xml2json/xml2json.service';
import { AmitalGatewayUtil, UnifreightMessageM } from 'Infrastructure/Utilities/AmitalGatewayUtil';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';
import { PriceCheckComponent } from './price-check.component';

@Injectable()
export class PriceCheckService {

  constructor(
    private dialogService: DialogService,
    private xml2Json: Xml2jsonService,
  ) { }

  async open(quoteId: string): Promise<DynamicDialogRef> {
    const config: DynamicDialogConfig = {}
    config.width = '800px';
    config.height = '600px';
    config.showHeader = false;
    config.styleClass = 'price-check';
    config.data = await this.getPrices(quoteId);

    return this.dialogService.open(PriceCheckComponent, config);
  }

  private async getPrices(quoteId: string): Promise<PriceChekRootResponse> {
    const logitudeEntity = 'QuoteOP';
    const logitudeViewModel = 'QuotesComponent';
    var unifreightMessageM = new UnifreightMessageM();
    unifreightMessageM.UnifreightEntity = 'GPRHEAD';
    unifreightMessageM.UnifreightEntityNumber = '-1';
    unifreightMessageM.LogitudeEntity = logitudeEntity;
    unifreightMessageM.LogitudeEntityNumber = quoteId;
    unifreightMessageM.LogitudeViewModel = logitudeViewModel;
    unifreightMessageM.Requset = [];
    unifreightMessageM.Requset.push(["PriceCheckRequest", "<root><key>value 1111</key></root>"]);

    const responsePromise = new Promise<PriceChekRootResponse>((resolve, reject) => {
      const sub: Subscription = AmitalGatewayUtil.Instance.UnifaceRequestArrived
        .subscribe(
          (mess: UnifreightMessageM) => {
            const IsMatchUnifreightCallbackCommand: boolean = (
              mess.LogitudeEntity == logitudeEntity &&
              mess.LogitudeEntityNumber == quoteId &&
              mess.LogitudeViewModel == logitudeViewModel);

            if (IsMatchUnifreightCallbackCommand) {
              sub.unsubscribe();
              SessionLocator.SelectedSession.StopBusyIndicator();
              let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
              if (sBool) {
                const prices: PriceChekRootResponse = this.xml2Json.decodeXmlStr2Json(mess as any);
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

  // private async getPrices(quoteId: string): Promise<PriceChekRootResponse> {
  //   return await new Promise<PriceChekRootResponse>((resolve) => {
  //     const prices: PriceChekRootResponse = this.xml2Json.decodeXmlStr2Json(xmlPriceString)      
  //     this.fixData(prices)
  //     resolve(prices);
  //   })
  // }

  private fixData(prices: PriceChekRootResponse) {
    prices.PriceChekResponse.Offers.Offer.forEach(x => {
      if ((<any>x.Result).length)
        x.Result = (<any>x.Result)[0];
    })
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


import { Injectable } from '@angular/core';
import { DialogService, DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { PriceCheckComponent } from './price-check.component';

@Injectable()
export class PriceCheckService {
  
  constructor(
    private dialogService: DialogService,
  ) { }

  async open(quoteId: string): Promise<DynamicDialogRef> {
    const config: DynamicDialogConfig = {}
    config.width = '800px';
    config.height = '600px';
    config.showHeader = false;
    config.styleClass = 'price-check';
    config.data = await this.getData(quoteId);

    return this.dialogService.open(PriceCheckComponent, config);
  }

  private async getData(quoteId: string): Promise<PriceCheck> {
    return await new Promise<PriceCheck>((resolve) => {      
      resolve(JSON.parse(priceCheckJsonMock) as PriceCheck);
    })    
  }
}


export interface SpecialService {
}

export interface QuoteProperty {
  Order: string;
  Client: string;
  From: string;
  To: string;
  CarrierCodes: string;
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
  SpecialService: SpecialService;
  MaxOffers: string;
  Cheapest: string;
  Fastest: string;
  HasRemarks?: string
  IsCostAllIn?: string
  IsSaleAllIn?: string
  Frequency?: string
  TransitTime?: string
  TotalCost?: string
  TotalSale?: string
  EstimatedProfit?: string
  [key: string]: any  
}

export interface PriceChekRequest {
  QuoteProperties: QuoteProperty[];
}

export type PriceCheck = {
  PriceChekRequest: PriceChekRequest;
}

// #omit-xml-declaration: string;
export const priceCheckJsonMock: string = `{
  "PriceChekRequest": {
    "QuoteProperties": [
      {
        "Order": "1",
        "Client": "10000046",
        "From": "TLV",
        "To": "FRA",
        "CarrierCodes": "LH",
        "ProductCode": "AE",
        "CostTariffUseCodes": "CR,CRE,AG",
        "SaleTariffUseCodes": "EX,EXD,AG",
        "StartDate": "06/05/2021",
        "GrossWeightAmount": "100",
        "GrossWeightUOM": "KG",
        "VolumeAmount": "0.3",
        "VolumeUOM": "CBM",
        "ChargeableWeightAmount": "100",
        "ChargeableWeightUOM": "KG",
        "QuoteType": "SpotRate",
        "Incoterms": "CIF",
        "Currency": "USD",
        "SpecialService": {
        },
        "MaxOffers": "3",
        "Cheapest": "True",
        "Fastest": "False"
      },
      {
        "Order": "2",
        "Client": "10000046",
        "From": "TLV",
        "To": "FRA",
        "CarrierCodes": "LY",
        "ProductCode": "AE",
        "CostTariffUseCodes": "CR,CRE,AG",
        "SaleTariffUseCodes": "EX,EXD,AG",
        "StartDate": "06/05/2021",
        "GrossWeightAmount": "100",
        "GrossWeightUOM": "KG",
        "VolumeAmount": "0.3",
        "VolumeUOM": "CBM",
        "ChargeableWeightAmount": "100",
        "ChargeableWeightUOM": "KG",
        "QuoteType": "SpotRate",
        "Incoterms": "CIF",
        "Currency": "USD",
        "SpecialService": {
        },
        "MaxOffers": "3",
        "Cheapest": "True",
        "Fastest": "False"
      }
    ]
  },
  "#omit-xml-declaration": "yes"
}`
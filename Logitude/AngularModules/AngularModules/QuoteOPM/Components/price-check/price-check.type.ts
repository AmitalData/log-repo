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

export type PriceCheckFilters = {
    Quickest: boolean,
    Cheapest: boolean,
    Direct: boolean
}

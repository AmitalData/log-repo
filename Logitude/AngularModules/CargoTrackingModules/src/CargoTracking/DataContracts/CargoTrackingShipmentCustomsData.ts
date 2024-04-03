import { CargoTrackingShipmentCustomTaxDetails } from "./CargoTrackingShipmentCustomTaxDetails";
export class CargoTrackingShipmentCustomsData
{
    DeclarationNumber: string;
    DeclarationStatus: string;
    CurrencySign: string;
    ImporterId: string;
    CurrencyCode: string;
    CurrencyName: string;
    GoodsDescription: string;
    ImporterVatAmount: number;
    TotalValueInNIS: number;
    TotalValueInForeignCurrency: number;
    TotalTax: number;
    CargoIdentifier1: string;
    CargoIdentifier2: string;
    CargoIdentifier3: string;
    TaxDetails:  CargoTrackingShipmentCustomTaxDetails[] = [];
}

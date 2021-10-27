import { CargoTrackingShipmentCustomTaxDetails } from "./CargoTrackingShipmentCustomTaxDetails";

export class ShipmentCustomsData
{
    DeclarationNumber: string;
    DeclarationStatus: string;
    CurrencySign: string;
    CurrencyCode: string;
    CurrencyName: string;
    GoodsDescription: string;
    ImporterVatAmount: number;
    TotalValueInNIS: number;
    TotalValueInForeignCurrency: number;
    TotalTax: number;
    ImporterId: string;
    CargoIdentifier1: string;
    CargoIdentifier2: string;
    CargoIdentifier3: string;
    TaxDetails: CargoTrackingShipmentCustomTaxDetails[] = [];
}

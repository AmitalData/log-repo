import { CargoTrackingShipmentCustomTaxDetails } from "./CargoTrackingShipmentCustomTaxDetails";
export class CargoTrackingShipmentCustomsData
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

    TaxDetails:  CargoTrackingShipmentCustomTaxDetails[] = [];
}

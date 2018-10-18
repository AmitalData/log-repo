export class SupplierInvoiceItemsTaxList {

    DeclarationId: string;


    CounterKey: number;


    LineNumber: number;

    TaxTypeCode: string;

    Tenant: number;



    TradeAgreementTypeCode: string;

    TaxRate?: number;;

    TaxBaseAmount?: number;;
    TaxAmount?: number;
    DeferedTaxAmount?: number;
    DefinedPerUnitMeasure?: number;

    AlternateDefinedPerUnitMeasure?: number;
    DefinedPerUnitQuantity?: number;
    AlternateDefinedPerUnitQuant?: number;
    MeasurementUnitCode?: number;
    AlternateMeasurementUnitCode: string;
    TradeLevyNumber: string;
    TotalBtlCoverageNIS?: number;
    AlternateRate?: number;

    TaxTypeName: string;
    TradeAgreementTypeName: string;
    MeasurementUnitName: string;
    AlternateMeasurementUnitName: string;
    InvoiceNumber: string;
    ClassificationCode: string;
    
}
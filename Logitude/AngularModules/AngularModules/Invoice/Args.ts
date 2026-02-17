


export class InvoiceTotalsClass {
    public Id: string;
    public VatTypeId: string;
    public VatTypePercentage: number;
    public RowLabel: string;
    public LocalCurrencyAmount: number;
    public InvoiceCurrencyAmount: number;
    public ProfitCurrencyAmount: number;
    public ExternalVatCard: string;
    public ExternalTAXItemId: string;
    public VatTypeCell: string;
}

export class SummaryItem {
    public Label: string = null;
    public Value: any = null;
}

export class InvoiceStockInputArgs {
    public Stock: any;
    public IsEditMode: boolean; 
}

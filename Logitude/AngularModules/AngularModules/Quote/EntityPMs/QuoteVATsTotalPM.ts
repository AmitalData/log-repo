export class QuoteVATsTotalPM {

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { if (this.id != newValue) { this.id = newValue; } }

    private vatTypeId: string;
    public get VatTypeId() { return this.vatTypeId; }
    public set VatTypeId(newValue: string) { if (this.vatTypeId != newValue) { this.vatTypeId = newValue; } }

    private vatPercentage: number;
    public get VatPercentage() { return this.vatPercentage; }
    public set VatPercentage(newValue: number) { if (this.vatPercentage != newValue) { this.vatPercentage = newValue; } }

    private vatTypeName: string;
    public get VatTypeName() { return this.vatTypeName; }
    public set VatTypeName(newValue: string) { if (this.vatTypeName != newValue) { this.vatTypeName = newValue; } }

    private amountInSaleCurrency: number;
    public get AmountInSaleCurrency() { return this.amountInSaleCurrency; }
    public set AmountInSaleCurrency(newValue: number) { if (this.amountInSaleCurrency != newValue) { this.amountInSaleCurrency = newValue; } }

    private amountInLocalCurrency: number;
    public get AmountInLocalCurrency() { return this.amountInLocalCurrency; }
    public set AmountInLocalCurrency(newValue: number) { if (this.amountInLocalCurrency != newValue) { this.amountInLocalCurrency = newValue; } }

    public IsDirty: boolean;
}
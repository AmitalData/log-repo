
export class TariffLineExpirationDatePM {

    private tariffLineId: string;
    public get TariffLineId() { return this.tariffLineId; }
    public set TariffLineId(newValue: string) {
        if (this.tariffLineId != newValue) {
            this.tariffLineId = newValue;
        }
    }


    private expirationDate: Date;
    public get ExpirationDate() { return this.expirationDate; }
    public set ExpirationDate(newValue: Date) {
        if (this.expirationDate != newValue) {
            this.expirationDate = newValue;
        }
    }

    public IsDirty: boolean;    
}


export class TariffLineExpirationDatePM {

    private originPortId: string;
    public get OriginPortId() { return this.originPortId; }
    public set OriginPortId(newValue: string) {
        if (this.originPortId != newValue) {
            this.originPortId = newValue;
        }
    }

    private destinationPortId: string;
    public get DestinationPortId() { return this.destinationPortId; }
    public set DestinationPortId(newValue: string) {
        if (this.destinationPortId != newValue) {
            this.destinationPortId = newValue;
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

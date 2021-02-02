export class ShipmentMapping {         

    private static Directions: { [key: string]: string; } = {
        "Export": "E",
        "Import": "I",
        "Domestic": "D",
        "Drop": "R",
        "Customs Import": "C"
    };

    private static TransportModes: { [key: string]: string; } = {
        "Air": "A",
        "Inland": "I",
        "Ocean": "O"
    };

    public static GetDirectionCode(direction:string): string {
        if(Object.keys(this.Directions).map(k => this.Directions[k]).indexOf(direction) > -1){
            return direction;
        }
        return this.Directions[direction];
    }

    public static GetTransportModeCode(transportMode:string): string {
        if(Object.keys(this.TransportModes).map(k => this.TransportModes[k]).indexOf(transportMode) > -1){
            return transportMode;
        }
        return this.TransportModes[transportMode];
    }
}
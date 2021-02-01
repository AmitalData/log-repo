export class ShipmentMapping {         

    private static Directions: { [key: string]: string; } = {
        "export": "E",
        "import": "I",
        "domestic": "D",
        "drop": "R",
        "customs import": "C"
    };

    private static TransportModes: { [key: string]: string; } = {
        "air": "A",
        "inland": "I",
        "ocean": "O"
    };

    public static GetDirectionCode(directionName:string): string {
      return this.Directions[directionName.toLowerCase()];
    }

    public static GetTransportModeCode(transportModeName:string): string {
        return this.TransportModes[transportModeName.toLowerCase()];
    }
}
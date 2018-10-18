export class WarehouseTools {

    public static GetFromPortLabels(transportModeId: string) {
        var fromPortText = "";
        switch (transportModeId) {
            case "A": {
                fromPortText = "Gateway";
                break;
            }
            case "O": {
                fromPortText = "Loading Port";
                break;
            }
            case "I": {
                fromPortText = "From";
                break;
            }
            default: {
                fromPortText = "From";
                break;
            }
        }
        return fromPortText;
    }
    public static GetToPortLabels(transportModeId: string) {
        var toPortText = "";
        switch (transportModeId) {
            case "A": {
                toPortText = "Destination";
                break;
            }
            case "O": {
                toPortText = "Discharge Port";
                break;
            }
            case "I": {
                toPortText = "To";
                break;
            }
            default: {
                toPortText = "To";
                break;
            }
        }
        return toPortText;
    }
    public static IsInlandDomestic(transportModeId: string, directionId:string ) {
        return transportModeId == "I" && directionId == "D" ? true : false;
    }
}
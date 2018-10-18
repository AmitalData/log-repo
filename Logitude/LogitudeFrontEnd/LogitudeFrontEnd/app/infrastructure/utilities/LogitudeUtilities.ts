export class LogitudeUtilities {

    public static IsLCLEntity(myTransportModeId: string, myShipmentTypeId: string) {
        var myResult = false;

        if (myTransportModeId != null) {
            myTransportModeId = myTransportModeId.toUpperCase();
        }

        if (myShipmentTypeId != null) {
            myShipmentTypeId = myShipmentTypeId.toUpperCase()
        }

        if (myTransportModeId == "A") {
            myResult = true;
        }

        else if (myTransportModeId == "O" && myShipmentTypeId == "LCLD") {
            myResult = true;
        }

        else if (myTransportModeId == "I" && myShipmentTypeId == "LTL") {
            myResult = true;
        }

        return myResult;
    }

}
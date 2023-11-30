import { AppTool, DateTool } from '../../Infrastructure/Tools';

export class LastStatusItem {
    public StatusName: string;
    public EventDate: Date;
    public ReceivingDate: Date;
    public LocationCode: string;
    public DepartureDate: Date;
    public ArrivalDate: Date;
    public DepartureArrivalDate: Date;
    public SortingValue: number = 0;
    public VesselName: string;
    public VoyageNumber: string;
    public DepartureDateInfo: string;
    public ArrivalDateInfo: string;
    public DepartureArrivalDateInfo: string;
    public StatusSourceName: string;

    constructor(item: any) {
        if (item) {
            this.StatusName = item.StatusName;
            this.EventDate = item.EventDate;
            this.ReceivingDate = item.ReceivingDate;
            this.LocationCode = item.LocationCode;
            this.DepartureDate = item.DepartureDate;
            this.ArrivalDate = item.ArrivalDate;
            this.VesselName = item.VesselName;
            this.VoyageNumber = item.VoyageNumber;
            this.StatusSourceName = item.StatusSourceName;

            if (this.DepartureDate) {
                this.DepartureDateInfo = item.TimeOfDepartureInfo == "E" ? "ETD" : "ATD";
            }

            if (this.ArrivalDate) {
                this.ArrivalDateInfo = item.TimeOfArrivalInfo == "E" ? "ETA" : "ATA";
            }

            if (this.EventDate) {
                this.SortingValue = DateTool.GetDateParts(this.EventDate).DateTicks;
            }

            var isDeparture = this.GetEventDirection(item.StatusCode);
            if (isDeparture) {
                this.DepartureArrivalDate = this.DepartureDate;
                this.DepartureArrivalDateInfo = this.DepartureDateInfo;
            }

            else {
                this.DepartureArrivalDate = this.ArrivalDate;
                this.DepartureArrivalDateInfo = this.ArrivalDateInfo;
            }
        }
    }

    GetEventDirection(statusCode: string) {
        var isDeparture = false;

        switch (statusCode) {
            case "2":
            case "3":
            case "AA":
            case "AC":
            case "AE":
            case "AF":
            case "AI":
            case "AW":
            case "B":
            case "BE":
            case "BF":
            case "BR":
            case "C":
            case "CA":
            case "CD":
            case "CO":
            case "CS":
            case "EE":
            case "EP":
            case "GI":
            case "I":
            case "VD":
            case "X3":
            case "X4":
            case "X7":
            case "X8":
            case "XA":
                {
                    isDeparture = true;
                    break;
                }
        }

        return isDeparture;
    }

}

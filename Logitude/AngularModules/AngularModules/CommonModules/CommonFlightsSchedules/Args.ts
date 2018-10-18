export class FlightsSchedulesArgs {
    public BookingPM: any;
    public ShipmentPM: any;
    public IsMainLeg: boolean = true;
    public IsFlightSelected: boolean = false;
    public IsClosedFomProgress: boolean = false;
    public IsCancelledFomProgress: boolean = false;
}

export class FVASimulatorWindowArgs {
    public EntityPM: any;
    public AirlineList: any;
    public Recipient: string;
}

export class XMLSimulatorWindowArgs {
    public BookingId: string;
    public ShipmentId: string;
    public FatherComponent: any;
}
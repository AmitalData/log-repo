export class AWBWizardArgs {
    EntityPM: any;
    ShipmentLevelCode: string;
    IsNewEntity: boolean;
    MasterPM: any;
    IsCreatingHouseFromMaster: boolean;
    IsCopyFromShipment: boolean;
    IsBuildFromBooking: boolean;
}
export class NewShipmentComponentArgs {
    public Shipment: any = null;
    public ShipmentLevelCode: string = null;
    public IsShipmentLevelFixed: boolean = false;
    public IsBuildFromQuote: boolean = false;
    public IsCopyFromShipment: boolean = false;
    public IsCreatedFromMasterHouses: boolean = false;
    public IsCreatedFromCustomerOverview: boolean = false;
    public IsMasterCreatedFromHouse: boolean = false;
    public IsStandalone: boolean = false;
    public IsNewStandAlonePickupDelivery: boolean = false;
    public ForwarderStandaloneShipmentId: string = null;
    public ForwarderShipmentPickUpDeliveryTypeCode: string = null;
}
export class FSRWizardArgs {
    EntityPM: any;
    ShipmentLevelCode: string;
}
export class AddEditPartnerArgs {
    public EntityPM: any;
    public IsNewEntity: boolean;
    public PartnerTypeCode: string;
}
export class SendAWBArgs {
    public EnttiyPM: any;
    public IsSendingFHLs: boolean;
    public IsSendingDEXX: boolean;
    public IsSendingCargonaut: boolean;
    public Wizard: any;
}
export class CustomsWizardArgs {
    public EntityPM: any;
}

export class ArtemusWizardArgs {
    public ShipmentId: string;
    public Type: string;
}

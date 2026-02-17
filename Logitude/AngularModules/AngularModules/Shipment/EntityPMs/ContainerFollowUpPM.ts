import {Output, EventEmitter}  from '@angular/core';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';

export class ContainerFollowUpPM {
    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public UIProperties: UIProperties;
    constructor() {
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { if (this.id != newValue) { this.id = newValue; this.MarkAsDirty("Id"); } }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { if (this.tenant != newValue) { this.tenant = newValue; this.MarkAsDirty("Tenant"); } }

    private shipmentId: string;
    public get ShipmentId() { return this.shipmentId; }
    public set ShipmentId(newValue: string) { if (this.shipmentId != newValue) { this.shipmentId = newValue; this.MarkAsDirty("ShipmentId"); } }

    private shipmentNumber: string;
    public get ShipmentNumber() { return this.shipmentNumber; }
    public set ShipmentNumber(newValue: string) { if (this.shipmentNumber != newValue) { this.shipmentNumber = newValue; this.MarkAsDirty("ShipmentNumber"); } }

    private customerName: string;
    public get CustomerName() { return this.customerName; }
    public set CustomerName(newValue: string) { if (this.customerName != newValue) { this.customerName = newValue; this.MarkAsDirty("CustomerName"); } }

    private customerContactName: string;
    public get CustomerContactName() { return this.customerContactName; }
    public set CustomerContactName(newValue: string) { if (this.customerContactName != newValue) { this.customerContactName = newValue; this.MarkAsDirty("CustomerContactName"); } }

    private carrierName: string;
    public get CarrierName() { return this.carrierName; }
    public set CarrierName(newValue: string) { if (this.carrierName != newValue) { this.carrierName = newValue; this.MarkAsDirty("CarrierName"); } }

    private shipperName: string;
    public get ShipperName() { return this.shipperName; }
    public set ShipperName(newValue: string) { if (this.shipperName != newValue) { this.shipperName = newValue; this.MarkAsDirty("ShipperName"); } }

    private consigneeName: string;
    public get ConsigneeName() { return this.consigneeName; }
    public set ConsigneeName(newValue: string) { if (this.consigneeName != newValue) { this.consigneeName = newValue; this.MarkAsDirty("ConsigneeName"); } }

    private consigneeReference: string;
    public get ConsigneeReference() { return this.consigneeReference; }
    public set ConsigneeReference(newValue: string) { if (this.consigneeReference != newValue) { this.consigneeReference = newValue; this.MarkAsDirty("ConsigneeReference"); } }

    private longMaster: string;
    public get LongMaster() { return this.longMaster; }
    public set LongMaster(newValue: string) { if (this.longMaster != newValue) { this.longMaster = newValue; this.MarkAsDirty("LongMaster"); } }

    private house: string;
    public get House() { return this.house; }
    public set House(newValue: string) { if (this.house != newValue) { this.house = newValue; this.MarkAsDirty("House"); } }

    private directionId: string;
    public get DirectionId() { return this.directionId; }
    public set DirectionId(newValue: string) { if (this.directionId != newValue) { this.directionId = newValue; this.MarkAsDirty("DirectionId"); } }

    private directionName: string;
    public get DirectionName() { return this.directionName; }
    public set DirectionName(newValue: string) { if (this.directionName != newValue) { this.directionName = newValue; this.MarkAsDirty("DirectionName"); } }

    private transportModeId: string;
    public get TransportModeId() { return this.transportModeId; }
    public set TransportModeId(newValue: string) { if (this.transportModeId != newValue) { this.transportModeId = newValue; this.MarkAsDirty("TransportModeId"); } }

    private transportModeName: string;
    public get TransportModeName() { return this.transportModeName; }
    public set TransportModeName(newValue: string) { if (this.transportModeName != newValue) { this.transportModeName = newValue; this.MarkAsDirty("TransportModeName"); } }

    private shipmentType: string;
    public get ShipmentType() { return this.shipmentType; }
    public set ShipmentType(newValue: string) { if (this.shipmentType != newValue) { this.shipmentType = newValue; this.MarkAsDirty("ShipmentType"); } }

    private shipmentLevelCode: string;
    public get ShipmentLevelCode() { return this.shipmentLevelCode; }
    public set ShipmentLevelCode(newValue: string) { if (this.shipmentLevelCode != newValue) { this.shipmentLevelCode = newValue; this.MarkAsDirty("ShipmentLevelCode"); } }

    private shipmentLevelName: string;
    public get ShipmentLevelName() { return this.shipmentLevelName; }
    public set ShipmentLevelName(newValue: string) { if (this.shipmentLevelName != newValue) { this.shipmentLevelName = newValue; this.MarkAsDirty("ShipmentLevelName"); } }

    private statusId: string;
    public get StatusId() { return this.statusId; }
    public set StatusId(newValue: string) { if (this.statusId != newValue) { this.statusId = newValue; this.MarkAsDirty("StatusId"); } }

    private containerTypeName: string;
    public get ContainerTypeName() { return this.containerTypeName; }
    public set ContainerTypeName(newValue: string) { if (this.containerTypeName != newValue) { this.containerTypeName = newValue; this.MarkAsDirty("ContainerTypeName"); } }

    private containerNumber: string;
    public get ContainerNumber() { return this.containerNumber; }
    public set ContainerNumber(newValue: string) { if (this.containerNumber != newValue) { this.containerNumber = newValue; this.MarkAsDirty("ContainerNumber"); } }

    private shipperSeal: string;
    public get ShipperSeal() { return this.shipperSeal; }
    public set ShipperSeal(newValue: string) { if (this.shipperSeal != newValue) { this.shipperSeal = newValue; this.MarkAsDirty("ShipperSeal"); } }

    private volume: number;
    public get Volume() { return this.volume; }
    public set Volume(newValue: number) { if (this.volume != newValue) { this.volume = newValue; this.MarkAsDirty("Volume"); } }

    private isDangerous: boolean;
    public get IsDangerous() { return this.isDangerous; }
    public set IsDangerous(newValue: boolean) { if (this.isDangerous != newValue) { this.isDangerous = newValue; this.MarkAsDirty("IsDangerous"); } }

    private description: string;
    public get Description() { return this.description; }
    public set Description(newValue: string) { if (this.description != newValue) { this.description = newValue; this.MarkAsDirty("Description"); } }

    private marksAndNumbers: string;
    public get MarksAndNumbers() { return this.marksAndNumbers; }
    public set MarksAndNumbers(newValue: string) { if (this.marksAndNumbers != newValue) { this.marksAndNumbers = newValue; this.MarkAsDirty("MarksAndNumbers"); } }

    private isDeliveryFU: boolean;
    public get IsDeliveryFU() { return this.isDeliveryFU; }
    public set IsDeliveryFU(newValue: boolean) { if (this.isDeliveryFU != newValue) { this.isDeliveryFU = newValue; this.MarkAsDirty("IsDeliveryFU"); } }

    private deliveryId: string;
    public get DeliveryId() { return this.deliveryId; }
    public set DeliveryId(newValue: string) { if (this.deliveryId != newValue) { this.deliveryId = newValue; this.MarkAsDirty("DeliveryId"); } }

    private deliveryETD: Date;
    public get DeliveryETD() { return this.deliveryETD; }
    public set DeliveryETD(newValue: Date) { if (this.deliveryETD != newValue) { this.deliveryETD = newValue; this.MarkAsDirty("DeliveryETD"); } }

    private deliveryATD: Date;
    public get DeliveryATD() { return this.deliveryATD; }
    public set DeliveryATD(newValue: Date) { if (this.deliveryATD != newValue) { this.deliveryATD = newValue; this.MarkAsDirty("DeliveryATD"); } }

    private deliveryETA: Date;
    public get DeliveryETA() { return this.deliveryETA; }
    public set DeliveryETA(newValue: Date) { if (this.deliveryETA != newValue) { this.deliveryETA = newValue; this.MarkAsDirty("DeliveryETA"); } }

    private deliveryATA: Date;
    public get DeliveryATA() { return this.deliveryATA; }
    public set DeliveryATA(newValue: Date) { if (this.deliveryATA != newValue) { this.deliveryATA = newValue; this.MarkAsDirty("DeliveryATA"); } }

    private deliveryDeparture: Date;
    public get DeliveryDeparture() { return this.deliveryDeparture; }
    public set DeliveryDeparture(newValue: Date) { if (this.deliveryDeparture != newValue) { this.deliveryDeparture = newValue; this.MarkAsDirty("DeliveryDeparture"); } }

    private deliveryArrival: Date;
    public get DeliveryArrival() { return this.deliveryArrival; }
    public set DeliveryArrival(newValue: Date) { if (this.deliveryArrival != newValue) { this.deliveryArrival = newValue; this.MarkAsDirty("DeliveryArrival"); } }

    private deliveryFrom: string;
    public get DeliveryFrom() { return this.deliveryFrom; }
    public set DeliveryFrom(newValue: string) { if (this.deliveryFrom != newValue) { this.deliveryFrom = newValue; this.MarkAsDirty("DeliveryFrom"); } }

    private deliveryTo: string;
    public get DeliveryTo() { return this.deliveryTo; }
    public set DeliveryTo(newValue: string) { if (this.deliveryTo != newValue) { this.deliveryTo = newValue; this.MarkAsDirty("DeliveryTo"); } }

    private isEmptyContainerReturnFU: boolean;
    public get IsEmptyContainerReturnFU() { return this.isEmptyContainerReturnFU; }
    public set IsEmptyContainerReturnFU(newValue: boolean) { if (this.isEmptyContainerReturnFU != newValue) { this.isEmptyContainerReturnFU = newValue; this.MarkAsDirty("IsEmptyContainerReturnFU"); } }

    private emptyContainerReturnId: string;
    public get EmptyContainerReturnId() { return this.emptyContainerReturnId; }
    public set EmptyContainerReturnId(newValue: string) { if (this.emptyContainerReturnId != newValue) { this.emptyContainerReturnId = newValue; this.MarkAsDirty("EmptyContainerReturnId"); } }

    private emptyContainerReturnETD: Date;
    public get EmptyContainerReturnETD() { return this.emptyContainerReturnETD; }
    public set EmptyContainerReturnETD(newValue: Date) { if (this.emptyContainerReturnETD != newValue) { this.emptyContainerReturnETD = newValue; this.MarkAsDirty("EmptyContainerReturnETD"); } }

    private emptyContainerReturnATD: Date;
    public get EmptyContainerReturnATD() { return this.emptyContainerReturnATD; }
    public set EmptyContainerReturnATD(newValue: Date) { if (this.emptyContainerReturnATD != newValue) { this.emptyContainerReturnATD = newValue; this.MarkAsDirty("EmptyContainerReturnATD"); } }

    private emptyContainerReturnETA: Date;
    public get EmptyContainerReturnETA() { return this.emptyContainerReturnETA; }
    public set EmptyContainerReturnETA(newValue: Date) { if (this.emptyContainerReturnETA != newValue) { this.emptyContainerReturnETA = newValue; this.MarkAsDirty("EmptyContainerReturnETA"); } }

    private emptyContainerReturnATA: Date;
    public get EmptyContainerReturnATA() { return this.emptyContainerReturnATA; }
    public set EmptyContainerReturnATA(newValue: Date) { if (this.emptyContainerReturnATA != newValue) { this.emptyContainerReturnATA = newValue; this.MarkAsDirty("EmptyContainerReturnATA"); } }

    private returnDeparture: Date;
    public get ReturnDeparture() { return this.returnDeparture; }
    public set ReturnDeparture(newValue: Date) { if (this.returnDeparture != newValue) { this.returnDeparture = newValue; this.MarkAsDirty("ReturnDeparture"); } }

    private returnArrival: Date;
    public get ReturnArrival() { return this.returnArrival; }
    public set ReturnArrival(newValue: Date) { if (this.returnArrival != newValue) { this.returnArrival = newValue; this.MarkAsDirty("ReturnArrival"); } }

    private emptyContainerReturnFrom: string;
    public get EmptyContainerReturnFrom() { return this.emptyContainerReturnFrom; }
    public set EmptyContainerReturnFrom(newValue: string) { if (this.emptyContainerReturnFrom != newValue) { this.emptyContainerReturnFrom = newValue; this.MarkAsDirty("EmptyContainerReturnFrom"); } }

    private emptyContainerReturnTo: string;
    public get EmptyContainerReturnTo() { return this.emptyContainerReturnTo; }
    public set EmptyContainerReturnTo(newValue: string) { if (this.emptyContainerReturnTo != newValue) { this.emptyContainerReturnTo = newValue; this.MarkAsDirty("EmptyContainerReturnTo"); } }

    private vesselName: string;
    public get VesselName() { return this.vesselName; }
    public set VesselName(newValue: string) { if (this.vesselName != newValue) { this.vesselName = newValue; this.MarkAsDirty("VesselName"); } }

    private shipmentNotes: string;
    public get ShipmentNotes() { return this.shipmentNotes; }
    public set ShipmentNotes(newValue: string) { if (this.shipmentNotes != newValue) { this.shipmentNotes = newValue; this.MarkAsDirty("ShipmentNotes"); } }

    public OldEntityPM: ContainerFollowUpPM;

    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "ContainerFollowUp");
        }
    }
}
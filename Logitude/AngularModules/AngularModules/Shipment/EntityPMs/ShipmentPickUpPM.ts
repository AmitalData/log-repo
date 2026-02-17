import {Output, EventEmitter} from '@angular/core';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPickUpDeliveryPackagePM} from './ShipmentPickUpDeliveryPackagePM';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';

export class ShipmentPickUpPM {
    public UIProperties: UIProperties;
    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    constructor(_entityParentPM: any) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty(); }

    private shipmentId: string;
    public get ShipmentId() { return this.shipmentId; }
    public set ShipmentId(newValue: string) { this.shipmentId = newValue; this.MarkAsDirty(); }

    private pickUpDeliveryNumber: string;
    public get PickUpDeliveryNumber() { return this.pickUpDeliveryNumber; }
    public set PickUpDeliveryNumber(newValue: string) { this.pickUpDeliveryNumber = newValue; this.MarkAsDirty(); }

    private pickUpDeliveryTypeCode: string;
    public get PickUpDeliveryTypeCode() { return this.pickUpDeliveryTypeCode; }
    public set PickUpDeliveryTypeCode(newValue: string) { this.pickUpDeliveryTypeCode = newValue; this.MarkAsDirty(); }

    private fullResponsibility: boolean;
    public get FullResponsibility() { return this.fullResponsibility; }
    public set FullResponsibility(newValue: boolean) { this.fullResponsibility = newValue; this.MarkAsDirty(); }


    private pickUpDeliveryFromTypeCode: string;
    public get PickUpDeliveryFromTypeCode() { return this.pickUpDeliveryFromTypeCode; }
    public set PickUpDeliveryFromTypeCode(newValue: string) { this.pickUpDeliveryFromTypeCode = newValue; this.MarkAsDirty(); }

    private pickUpDeliveryToTypeCode: string;
    public get PickUpDeliveryToTypeCode() { return this.pickUpDeliveryToTypeCode; }
    public set PickUpDeliveryToTypeCode(newValue: string) { this.pickUpDeliveryToTypeCode = newValue; this.MarkAsDirty(); }

    private fromPortId: string;
    public get FromPortId() { return this.fromPortId; }
    public set FromPortId(newValue: string) { this.fromPortId = newValue; this.MarkAsDirty(); }

    private toPortId: string;
    public get ToPortId() { return this.toPortId; }
    public set ToPortId(newValue: string) { this.toPortId = newValue; this.MarkAsDirty(); }

    private fromPartnerCardId: string;
    public get FromPartnerCardId() { return this.fromPartnerCardId; }
    public set FromPartnerCardId(newValue: string) { this.fromPartnerCardId = newValue; this.MarkAsDirty(); }

    private fromAddressId: string;
    public get FromAddressId() { return this.fromAddressId; }
    public set FromAddressId(newValue: string) { this.fromAddressId = newValue; this.MarkAsDirty(); }

    private fromAddress: string;
    public get FromAddress() { return this.fromAddress; }
    public set FromAddress(newValue: string) { this.fromAddress = newValue; this.MarkAsDirty(); }

    private fromAddressCity: string;
    public get FromAddressCity() { return this.fromAddressCity; }
    public set FromAddressCity(newValue: string) { this.fromAddressCity = newValue; this.MarkAsDirty(); }

    private fromAddressZipCode: string;
    public get FromAddressZipCode() { return this.fromAddressZipCode; }
    public set FromAddressZipCode(newValue: string) { this.fromAddressZipCode = newValue; this.MarkAsDirty(); }

    private fromAddressCountryId: string;
    public get FromAddressCountryId() { return this.fromAddressCountryId; }
    public set FromAddressCountryId(newValue: string) { this.fromAddressCountryId = newValue; this.MarkAsDirty(); }

    private toPartnerCardId: string;
    public get ToPartnerCardId() { return this.toPartnerCardId; }
    public set ToPartnerCardId(newValue: string) { this.toPartnerCardId = newValue; this.MarkAsDirty(); }

    private toAddressId: string;
    public get ToAddressId() { return this.toAddressId; }
    public set ToAddressId(newValue: string) { this.toAddressId = newValue; this.MarkAsDirty(); }

    private toAddress: string;
    public get ToAddress() { return this.toAddress; }
    public set ToAddress(newValue: string) { this.toAddress = newValue; this.MarkAsDirty(); }

    private toAddressCity: string;
    public get ToAddressCity() { return this.toAddressCity; }
    public set ToAddressCity(newValue: string) { this.toAddressCity = newValue; this.MarkAsDirty(); }

    private toAddressZipCode: string;
    public get ToAddressZipCode() { return this.toAddressZipCode; }
    public set ToAddressZipCode(newValue: string) { this.toAddressZipCode = newValue; this.MarkAsDirty(); }

    private toAddressCountryId: string;
    public get ToAddressCountryId() { return this.toAddressCountryId; }
    public set ToAddressCountryId(newValue: string) { this.toAddressCountryId = newValue; this.MarkAsDirty(); }

    // Port Dummy fields
    private fromPortCode: string;
    public get FromPortCode() { return this.fromPortCode; }
    public set FromPortCode(newValue: string) { this.fromPortCode = newValue; this.MarkAsDirty(); }

    private fromPortName: string;
    public get FromPortName() { return this.fromPortName; }
    public set FromPortName(newValue: string) { this.fromPortName = newValue; this.MarkAsDirty(); }

    private fromPortCountryCode: string;
    public get FromPortCountryCode() { return this.fromPortCountryCode; }
    public set FromPortCountryCode(newValue: string) { this.fromPortCountryCode = newValue; this.MarkAsDirty(); }

    private fromPortCountryName: string;
    public get FromPortCountryName() { return this.fromPortCountryName; }
    public set FromPortCountryName(newValue: string) { this.fromPortCountryName = newValue; this.MarkAsDirty(); }

    private toPortCode: string;
    public get ToPortCode() { return this.toPortCode; }
    public set ToPortCode(newValue: string) { this.toPortCode = newValue; this.MarkAsDirty(); }

    private toPortName: string;
    public get ToPortName() { return this.toPortName; }
    public set ToPortName(newValue: string) { this.toPortName = newValue; this.MarkAsDirty(); }

    private toPortCountryCode: string;
    public get ToPortCountryCode() { return this.toPortCountryCode; }
    public set ToPortCountryCode(newValue: string) { this.toPortCountryCode = newValue; this.MarkAsDirty(); }

    private toPortCountryName: string;
    public get ToPortCountryName() { return this.toPortCountryName; }
    public set ToPortCountryName(newValue: string) { this.toPortCountryName = newValue; this.MarkAsDirty(); }


    //Address Dummy fields
    private fromAddressCity_Dummy: string;
    public get FromAddressCity_Dummy() { return this.fromAddressCity_Dummy; }
    public set FromAddressCity_Dummy(newValue: string) { this.fromAddressCity_Dummy = newValue; this.MarkAsDirty(); }

    private fromAddressCountryCode: string;
    public get FromAddressCountryCode() { return this.fromAddressCountryCode; }
    public set FromAddressCountryCode(newValue: string) { this.fromAddressCountryCode = newValue; this.MarkAsDirty(); }


    private fromAddressCountryName: string;
    public get FromAddressCountryName() { return this.fromAddressCountryName; }
    public set FromAddressCountryName(newValue: string) { this.fromAddressCountryName = newValue; this.MarkAsDirty(); }


    private fromLocation: string;
    public get FromLocation() { return this.fromLocation; }
    public set FromLocation(newValue: string) { this.fromLocation = newValue; this.MarkAsDirty(); }

    private toAddressCity_Dummy: string;
    public get ToAddressCity_Dummy() { return this.toAddressCity_Dummy; }
    public set ToAddressCity_Dummy(newValue: string) { this.toAddressCity_Dummy = newValue; this.MarkAsDirty(); }


    private toAddressCountryCode: string;
    public get ToAddressCountryCode() { return this.toAddressCountryCode; }
    public set ToAddressCountryCode(newValue: string) { this.toAddressCountryCode = newValue; this.MarkAsDirty(); }


    private toAddressCountryName: string;
    public get ToAddressCountryName() { return this.toAddressCountryName; }
    public set ToAddressCountryName(newValue: string) { this.toAddressCountryName = newValue; this.MarkAsDirty(); }


    private toLocation: string;
    public get ToLocation() { return this.toLocation; }
    public set ToLocation(newValue: string) { this.toLocation = newValue; this.MarkAsDirty(); }


    private aTD: Date;
    public get ATD() { return this.aTD; }
    public set ATD(newValue: Date) { this.aTD = newValue; this.MarkAsDirty("ATD"); }


    private aTA: Date;
    public get ATA() { return this.aTA; }
    public set ATA(newValue: Date) { this.aTA = newValue; this.MarkAsDirty("ATA"); }

    private eTD: Date;
    public get ETD() { return this.eTD; }
    public set ETD(newValue: Date) { this.eTD = newValue; this.MarkAsDirty("ETD"); }

    private eTA: Date;
    public get ETA() { return this.eTA; }
    public set ETA(newValue: Date) { this.eTA = newValue; this.MarkAsDirty("ETA"); }

    private carrierId: string;
    public get CarrierId() { return this.carrierId; }
    public set CarrierId(newValue: string) { this.carrierId = newValue; this.MarkAsDirty(); }

    private carrierCode: string;
    public get CarrierCode() { return this.carrierCode; }
    public set CarrierCode(newValue: string) { this.carrierCode = newValue; this.MarkAsDirty(); }

    private carrierName: string;
    public get CarrierName() { return this.carrierName; }
    public set CarrierName(newValue: string) { this.carrierName = newValue; this.MarkAsDirty(); }

    private carrierNumber: string;
    public get CarrierNumber() { return this.carrierNumber; }
    public set CarrierNumber(newValue: string) { this.carrierNumber = newValue; this.MarkAsDirty(); }

    private carrierWebSite: string;
    public get CarrierWebSite() { return this.carrierWebSite; }
    public set CarrierWebSite(newValue: string) { this.carrierWebSite = newValue; this.MarkAsDirty(); }

    private driver: string;
    public get Driver() { return this.driver; }
    public set Driver(newValue: string) { this.driver = newValue; this.MarkAsDirty(); }

    private truckNumber: string;
    public get TruckNumber() { return this.truckNumber; }
    public set TruckNumber(newValue: string) { this.truckNumber = newValue; this.MarkAsDirty(); }

    private trailerNumber: string;
    public get TrailerNumber() { return this.trailerNumber; }
    public set TrailerNumber(newValue: string) { this.trailerNumber = newValue; this.MarkAsDirty(); }

    private notes: string;
    public get Notes() { return this.notes; }
    public set Notes(newValue: string) { this.notes = newValue; this.MarkAsDirty(); }

    private emptyPickupContainerPartnerId: string;
    public get EmptyPickupContainerPartnerId() { return this.emptyPickupContainerPartnerId; }
    public set EmptyPickupContainerPartnerId(newValue: string) { this.emptyPickupContainerPartnerId = newValue; this.MarkAsDirty(); }

    private emptyDeliveryContainerPartnerId: string;
    public get EmptyDeliveryContainerPartnerId() { return this.emptyDeliveryContainerPartnerId; }
    public set EmptyDeliveryContainerPartnerId(newValue: string) { this.emptyDeliveryContainerPartnerId = newValue; this.MarkAsDirty(); }

    private emptyPickupDepotReference: string;
    public get EmptyPickupDepotReference() { return this.emptyPickupDepotReference; }
    public set EmptyPickupDepotReference(newValue: string) { this.emptyPickupDepotReference = newValue; this.MarkAsDirty(); }

    private emptyDeliveryDepotReference: string;
    public get EmptyDeliveryDepotReference() { return this.emptyDeliveryDepotReference; }
    public set EmptyDeliveryDepotReference(newValue: string) { this.emptyDeliveryDepotReference = newValue; this.MarkAsDirty(); }

    private customerId: string;
    public get CustomerId() { return this.customerId; }
    public set CustomerId(newValue: string) { this.customerId = newValue; this.MarkAsDirty(); }

    private directionId: string;
    public get DirectionId() { return this.directionId; }
    public set DirectionId(newValue: string) { this.directionId = newValue; this.MarkAsDirty(); }

    private masterNumber: string;
    public get MasterNumber() { return this.masterNumber; }
    public set MasterNumber(newValue: string) { this.masterNumber = newValue; this.MarkAsDirty(); }

    private agentName: string;
    public get AgentName() { return this.agentName; }
    public set AgentName(newValue: string) { this.agentName = newValue; this.MarkAsDirty(); }

    private shipmentNumber: string;
    public get ShipmentNumber() { return this.shipmentNumber; }
    public set ShipmentNumber(newValue: string) { this.shipmentNumber = newValue; this.MarkAsDirty(); }

    private shippingLine: string;
    public get ShippingLine() { return this.shippingLine; }
    public set ShippingLine(newValue: string) { this.shippingLine = newValue; this.MarkAsDirty(); }

    private packageTEU: number;
    public get PackageTEU() { return this.packageTEU; }
    public set PackageTEU(newValue: number) { this.packageTEU = newValue; this.MarkAsDirty(); }

    private agentId: string;
    public get AgentId() { return this.agentId; }
    public set AgentId(newValue: string) { this.agentId = newValue; this.MarkAsDirty(); }

    private transportModeCode: string;
    public get TransportModeCode() { return this.transportModeCode; }
    public set TransportModeCode(newValue: string) { this.transportModeCode = newValue; this.MarkAsDirty(); }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }

    private shipmentPickUpDeliveryPackages: ShipmentPickUpDeliveryPackagePM[];
    get ShipmentPickUpDeliveryPackages() {
        if (this.shipmentPickUpDeliveryPackages == null) {
            this.shipmentPickUpDeliveryPackages = [];
        }

        return this.shipmentPickUpDeliveryPackages;
    }
    set ShipmentPickUpDeliveryPackages(newValue: ShipmentPickUpDeliveryPackagePM[]) {
        if (this.shipmentPickUpDeliveryPackages != newValue) {
            this.shipmentPickUpDeliveryPackages = newValue;
        }
    }
    public AddPackage(item: ShipmentPickUpDeliveryPackagePM) {
        if (item != null) {
            var index = this.ShipmentPickUpDeliveryPackages.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.ShipmentPickUpDeliveryPackages.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemovePackage(item: ShipmentPickUpDeliveryPackagePM) {
        if (item != null) {
            var index = this.ShipmentPickUpDeliveryPackages.indexOf(item);
            if (index > -1) {
                this.ShipmentPickUpDeliveryPackages.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private shipmentPickUpPackagesChangeSet: ShipmentPickUpDeliveryPackagePM[];
    get ShipmentPickUpPackagesChangeSet() {
        if (this.shipmentPickUpPackagesChangeSet == null) {
            this.shipmentPickUpPackagesChangeSet = [];
        }

        return this.shipmentPickUpPackagesChangeSet;
    }
    set ShipmentPickUpPackagesChangeSet(newValue: ShipmentPickUpDeliveryPackagePM[]) {
        if (this.shipmentPickUpPackagesChangeSet != newValue) {
            this.shipmentPickUpPackagesChangeSet = newValue;
        }
    }

    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    public OldEntityPM: ShipmentPickUpPM;
    public UniqueKey: string;
    public IsDirty: boolean;

    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }

        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
        }
    }
}
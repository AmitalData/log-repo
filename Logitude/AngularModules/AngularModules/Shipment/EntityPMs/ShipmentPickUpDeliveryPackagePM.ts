
import { UIProperties, UIProperty } from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { ServiceLocator } from '../../Infrastructure/Locators/ServiceLocator';
import { PickUpDeliveryPackageHarmonizePM } from './PickUpDeliveryPackageHarmonizePM';

export class ShipmentPickUpDeliveryPackagePM {

    public UIProperties: UIProperties;

    constructor(_entityParentPM: any) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty(); }

    private containerNumber: string;
    public get ContainerNumber() { return this.containerNumber; }
    public set ContainerNumber(newValue: string) { this.containerNumber = newValue; this.MarkAsDirty(); }

    private packageTypeId: string;
    public get PackageTypeId() { return this.packageTypeId; }
    public set PackageTypeId(newValue: string) { this.packageTypeId = newValue; this.MarkAsDirty(); }

    private packageTypeName: string;
    public get PackageTypeName() { return this.packageTypeName; }
    public set PackageTypeName(newValue: string) { this.packageTypeName = newValue; this.MarkAsDirty(); }

    private packageTypeTEU: number;
    public get PackageTypeTEU() { return this.packageTypeTEU; }
    public set PackageTypeTEU(newValue: number) { this.packageTypeTEU = newValue; this.MarkAsDirty(); }

    private shipmentPickUpDeliveryId: string;
    public get ShipmentPickUpDeliveryId() { return this.shipmentPickUpDeliveryId; }
    public set ShipmentPickUpDeliveryId(newValue: string) { this.shipmentPickUpDeliveryId = newValue; this.MarkAsDirty(); }

    private quantity: number;
    public get Quantity() { return this.quantity; }
    public set Quantity(newValue: number) { this.quantity = newValue; this.MarkAsDirty(); }

    private volume: number;
    public get Volume() { return this.volume; }
    public set Volume(newValue: number) { this.volume = newValue; this.MarkAsDirty(); }

    private weight: number;
    public get Weight() { return this.weight; }
    public set Weight(newValue: number) { this.weight = newValue; this.MarkAsDirty(); }

    private description: string;
    public get Description() { return this.description; }
    public set Description(newValue: string) { this.description = newValue; this.MarkAsDirty(); }

    private harmonize: string;
    public get Harmonize() { return this.harmonize; }
    public set Harmonize(newValue: string) { this.harmonize = newValue; this.MarkAsDirty(); }

    private shipperSeal: string;
    public get ShipperSeal() { return this.shipperSeal; }
    public set ShipperSeal(newValue: string) { this.shipperSeal = newValue; this.MarkAsDirty(); }

    private width: number;
    public get Width() { return this.width; }
    public set Width(newValue: number) { this.width = newValue; this.MarkAsDirty(); }

    private height: number;
    public get Height() { return this.height; }
    public set Height(newValue: number) { this.height = newValue; this.MarkAsDirty(); }

    private length: number;
    public get Length() { return this.length; }
    public set Length(newValue: number) { this.length = newValue; this.MarkAsDirty(); }

    private originalShipmentPackageId: string;
    public get OriginalShipmentPackageId() { return this.originalShipmentPackageId; }
    public set OriginalShipmentPackageId(newValue: string) { this.originalShipmentPackageId = newValue; this.MarkAsDirty(); }

    private isMultiHarmonize: boolean;
    public get IsMultiHarmonize() { return this.isMultiHarmonize; }
    public set IsMultiHarmonize(newValue: boolean) { if (this.isMultiHarmonize != newValue) { this.isMultiHarmonize = newValue; this.MarkAsDirty("IsMultiHarmonize"); } }

    private make: string;
    public get Make() { return this.make; }
    public set Make(newValue: string) { if (this.make != newValue) { this.make = newValue; this.MarkAsDirty("Make"); } }

    private model: string;
    public get Model() { return this.model; }
    public set Model(newValue: string) { if (this.model != newValue) { this.model = newValue; this.MarkAsDirty("Model"); } }

    private year: string;
    public get Year() { return this.year; }
    public set Year(newValue: string) { if (this.year != newValue) { this.year = newValue; this.MarkAsDirty("Year"); } }


    private color: string;
    public get Color() { return this.color; }
    public set Color(newValue: string) { if (this.color != newValue) { this.color = newValue; this.MarkAsDirty("Color"); } }


    private chassisNumber: string;
    public get ChassisNumber() { return this.chassisNumber; }
    public set ChassisNumber(newValue: string) { if (this.chassisNumber != newValue) { this.chassisNumber = newValue; this.MarkAsDirty("ChassisNumber"); } }


    private registrationNumber: string;
    public get RegistrationNumber() { return this.registrationNumber; }
    public set RegistrationNumber(newValue: string) { if (this.registrationNumber != newValue) { this.registrationNumber = newValue; this.MarkAsDirty("RegistrationNumber"); } }

    private countryId: string;
    public get CountryId() { return this.countryId; }
    public set CountryId(newValue: string) { if (this.countryId != newValue) { this.countryId = newValue; this.MarkAsDirty("CountryId"); } }



    private pickUpDeliveryPackageHarmonizes: PickUpDeliveryPackageHarmonizePM[];
    get PickUpDeliveryPackageHarmonizes() {
        if (this.pickUpDeliveryPackageHarmonizes == null) {
            this.pickUpDeliveryPackageHarmonizes = [];
        }

        return this.pickUpDeliveryPackageHarmonizes;
    }
    set PickUpDeliveryPackageHarmonizes(newValue: PickUpDeliveryPackageHarmonizePM[]) {
        if (this.pickUpDeliveryPackageHarmonizes != newValue) {
            this.pickUpDeliveryPackageHarmonizes = newValue;
        }
    }
    public AddPickUpDeliveryPackageHarmonizePM(item: PickUpDeliveryPackageHarmonizePM) {
        if (item != null) {
            var index = this.PickUpDeliveryPackageHarmonizes.indexOf(item);
            if (index == -1) {

                item.EntityParentPM = this;

                this.PickUpDeliveryPackageHarmonizes.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemovePickUpDeliveryPackageHarmonizePM(item: PickUpDeliveryPackageHarmonizePM) {
        if (item != null) {
            var index = this.PickUpDeliveryPackageHarmonizes.indexOf(item);
            if (index > -1) {
                this.PickUpDeliveryPackageHarmonizes.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }

    public PickUpDeliveryPackageHarmonizesChangeSet: Array<PickUpDeliveryPackageHarmonizePM> = [];

    public OldEntityPM: ShipmentPickUpDeliveryPackagePM;

    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    public UniqueKey: string;
    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }

        if (propertyName != null) {
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "ShipmentPickUpDeliveryPackage");

        }
    }

    private MyClone: ShipmentPickUpDeliveryPackagePM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}

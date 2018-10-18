
import {ShipmentPM} from './ShipmentPM'
import {UIProperties, UIProperty} from '../../infrastructure/logitude-components/UIProperties';

export class ShipmentPackagePM {

    public UIProperties: UIProperties;

    constructor(public entityParentPM: ShipmentPM) {
        this.UIProperties = new UIProperties; 
        this.IsDirty = false;
       
    }

    public IsDirty: boolean;

    private _Id: string;
    public get Id() { return this._Id; }
    public set Id(newValue: string) { this._Id = newValue; this.MarkAsDirty(); }

    
    private _Tenant: number;
    public get Tenant() { return this._Tenant; }
    public set Tenant(newValue: number) { this._Tenant = newValue; this.MarkAsDirty();  }

    private _IsContainer: boolean;
    public get IsContainer() { return this._IsContainer; }
    public set IsContainer(newValue: boolean) { this._IsContainer = newValue; this.MarkAsDirty();  }

    private _PackageTypeId: string;
    public get PackageTypeId() { return this._PackageTypeId; }
    public set PackageTypeId(newValue: string) { this._PackageTypeId = newValue; this.MarkAsDirty(); }

    private _PackageTypeCode: string;
    public get PackageTypeCode() { return this._PackageTypeCode; }
    public set PackageTypeCode(newValue: string) { this._PackageTypeCode = newValue; this.MarkAsDirty(); }

    private _PackageTypeName: string;
    public get PackageTypeName() { return this._PackageTypeName; }
    public set PackageTypeName(newValue: string) { this._PackageTypeName = newValue; this.MarkAsDirty(); }

    private _PrintAs: string;
    public get PrintAs() { return this._PrintAs; }
    public set PrintAs(newValue: string) { this._PrintAs = newValue; this.MarkAsDirty(); }

    private _ContainerSize: number;
    public get ContainerSize() { return this._ContainerSize; }
    public set ContainerSize(newValue: number) { this._ContainerSize = newValue; this.MarkAsDirty(); }

    private _TEU: number;
    public get TEU() { return this._TEU; }
    public set TEU(newValue: number) { this._TEU = newValue; this.MarkAsDirty(); }
    
    private _ContainerNumber: string;
    public get ContainerNumber() { return this._ContainerNumber; }
    public set ContainerNumber(newValue: string) { this._ContainerNumber = newValue; this.MarkAsDirty(); }

    private _MaterialDescription: string;
    public get MaterialDescription() { return this._MaterialDescription; }
    public set MaterialDescription(newValue: string) { this._MaterialDescription = newValue; this.MarkAsDirty(); }


    private _Seal: string;
    public get Seal() { return this._Seal; }
    public set Seal(newValue: string) { this._Seal = newValue; this.MarkAsDirty(); }


    private _Tare: number;
    public get Tare() { return this._Tare; }
    public set Tare(newValue: number) { this._Tare = newValue; this.MarkAsDirty(); }
    
    private _Quantity: number;
    public get Quantity() { return this._Quantity; }
    public set Quantity(newValue: number) { this._Quantity = newValue; this.MarkAsDirty(); }

    private _Weight: number;
    public get Weight() { return this._Weight; }
    public set Weight(newValue: number) { this._Weight = newValue; this.MarkAsDirty(); }
    
    private _Volume: number;
    public get Volume() { return this._Volume; }
    public set Volume(newValue: number) { this._Volume = newValue; this.MarkAsDirty(); }

    private _VolumetricWeight: number;
    public get VolumetricWeight() { return this._VolumetricWeight; }
    public set VolumetricWeight(newValue: number) { this._VolumetricWeight = newValue; this.MarkAsDirty(); }

    private _Height: number;
    public get Height() { return this._Height; }
    public set Height(newValue: number) { this._Height = newValue; this.MarkAsDirty(); }

    private _Width: number;
    public get Width() { return this._Width; }
    public set Width(newValue: number) { this._Width = newValue; this.MarkAsDirty(); }

    private _Length: number;
    public get Length() { return this._Length; }
    public set Length(newValue: number) { this._Length = newValue; this.MarkAsDirty(); }

    private _UnNumber: string;
    public get UnNumber() { return this._UnNumber; }
    public set UnNumber(newValue: string) { this._UnNumber = newValue; this.MarkAsDirty(); }

    private _ClassNumber: string;
    public get ClassNumber() { return this._ClassNumber; }
    public set ClassNumber(newValue: string) { this._ClassNumber = newValue; this.MarkAsDirty(); }

    private _Temperature: number;
    public get Temperature() { return this._Temperature; }
    public set Temperature(newValue: number) { this._Temperature = newValue; this.MarkAsDirty(); }

    private _Ventilation: number;
    public get Ventilation() { return this._Ventilation; }
    public set Ventilation(newValue: number) { this._Ventilation = newValue; this.MarkAsDirty(); }

    private _Seal2: string;
    public get Seal2() { return this._Seal2; }
    public set Seal2(newValue: string) { this._Seal2 = newValue; this.MarkAsDirty(); }

    private _SOC: number;
    public get SOC() { return this._SOC; }
    public set SOC(newValue: number) { this._SOC = newValue; this.MarkAsDirty(); }

    private _PackagingGroup: string;
    public get PackagingGroup() { return this._PackagingGroup; }
    public set PackagingGroup(newValue: string) { this._PackagingGroup = newValue; this.MarkAsDirty(); }

    private _IMDGCode: string;
    public get IMDGCode() { return this._IMDGCode; }
    public set IMDGCode(newValue: string) { this._IMDGCode = newValue; this.MarkAsDirty(); }

    private _FlashPoint: string;
    public get FlashPoint() { return this._FlashPoint; }
    public set FlashPoint(newValue: string) { this._FlashPoint = newValue; this.MarkAsDirty(); }

    private _Harmonize: string;
    public get Harmonize() { return this._Harmonize; }
    public set Harmonize(newValue: string) { this._Harmonize = newValue; this.MarkAsDirty(); }

    private _MarksAndNumbers: string;
    public get MarksAndNumbers() { return this._MarksAndNumbers; }
    public set MarksAndNumbers(newValue: string) { this._MarksAndNumbers = newValue; this.MarkAsDirty(); }

    private _Description: string;
    public get Description() { return this._Description; }
    public set Description(newValue: string) { this._Description = newValue; this.MarkAsDirty(); }

    private _ShipmentNumber: string;
    public get ShipmentNumber() { return this._ShipmentNumber; }
    public set ShipmentNumber(newValue: string) { this._ShipmentNumber = newValue; this.MarkAsDirty(); }

    private _ShipmentId: string;
    public get ShipmentId() { return this._ShipmentId; }
    public set ShipmentId(newValue: string) { this._ShipmentId = newValue; this.MarkAsDirty(); }

    private _ShipmentPMId: string;
    public get ShipmentPMId() { return this._ShipmentPMId; }
    public set ShipmentPMId(newValue: string) { this._ShipmentPMId = newValue; this.MarkAsDirty(); }

    private _IsDangerous: boolean;
    public get IsDangerous() { return this._IsDangerous; }
    public set IsDangerous(newValue: boolean) { this._IsDangerous = newValue; this.MarkAsDirty(); }

    private _OriginalShipmentPackageId: string;
    public get OriginalShipmentPackageId() { return this._OriginalShipmentPackageId; }
    public set OriginalShipmentPackageId(newValue: string) { this._OriginalShipmentPackageId = newValue; this.MarkAsDirty(); }

    private _CommodityId: string;
    public get CommodityId() { return this._CommodityId; }
    public set CommodityId(newValue: string) { this._CommodityId = newValue; this.MarkAsDirty(); }

    private _NumberOfInsidePackages: number;
    public get NumberOfInsidePackages() { return this._NumberOfInsidePackages; }
    public set NumberOfInsidePackages(newValue: number) { this._NumberOfInsidePackages = newValue; this.MarkAsDirty(); }

    private _NumberOfInsidePackagesDetails: string;
    public get NumberOfInsidePackagesDetails() { return this._NumberOfInsidePackagesDetails; }
    public set NumberOfInsidePackagesDetails(newValue: string) { this._NumberOfInsidePackagesDetails = newValue; this.MarkAsDirty(); }

    //Dummy
    private _IsAWBWizardDefault: boolean;
    public get IsAWBWizardDefault() { return this._IsAWBWizardDefault; }
    public set IsAWBWizardDefault(newValue: boolean) { this._IsAWBWizardDefault = newValue; this.MarkAsDirty(); }

    public InsideShipmentPackages: Array<any>;
    public ShipmentPackageItems: Array<any>;

    MarkAsDirty() {
        this.IsDirty = true;
        this.entityParentPM.IsDirty = true;
    }
    
}
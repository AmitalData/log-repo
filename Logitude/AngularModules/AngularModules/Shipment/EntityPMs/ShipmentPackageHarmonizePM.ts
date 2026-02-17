import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {Output, EventEmitter}  from '@angular/core';
import { PropertyChangedArgs } from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';

export class ShipmentPackageHarmonizePM {
    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public UIProperties: UIProperties;
    constructor(_entityParentPM: any) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { if (this.id != newValue) { this.id = newValue; this.MarkAsDirty("Id"); } }


    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { if (this.tenant != newValue) { this.tenant = newValue; this.MarkAsDirty("Tenant"); } }

    private packageId: string;
    public get PackageId() { return this.packageId; }
    public set PackageId(newValue: string) { if (this.packageId != newValue) { this.packageId = newValue; this.MarkAsDirty("PackageId"); } }

    private harmonize: string;
    public get Harmonize() { return this.harmonize; }
    public set Harmonize(newValue: string) { if (this.harmonize != newValue) { this.harmonize = newValue; this.MarkAsDirty("Harmonize"); } }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { if (this.changeSetOp != newValue) { this.changeSetOp = newValue; this.MarkAsDirty("ChangeSetOp"); } }

    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    public OldEntityPM: ShipmentPackageHarmonizePM;
    public UniqueKey: string;

    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "ShipmentPackageHarmonize");

        }
    }
    private MyClone: ShipmentPackageHarmonizePM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}

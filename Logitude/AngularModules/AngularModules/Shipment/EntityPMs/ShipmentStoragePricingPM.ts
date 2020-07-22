
import { ShipmentPM } from './ShipmentPM';
import { UIProperties, UIProperty } from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { ServiceLocator } from '../../Infrastructure/Locators/ServiceLocator';
import { Output, EventEmitter } from '@angular/core';
import { PropertyChangedArgs } from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import { CustomFieldClass } from '../../Infrastructure/DataContracts/CustomFieldClass';

export class ShipmentStoragePricingPM{

    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public UIProperties: UIProperties;
    constructor(_entityParentPM: any) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { if (this.changeSetOp != newValue) { this.changeSetOp = newValue; this.MarkAsDirty("ChangeSetOp"); } }



    public OldEntityPM: ShipmentStoragePricingPM;

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
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "ShipmentStoragePricing");

        }
    }
}

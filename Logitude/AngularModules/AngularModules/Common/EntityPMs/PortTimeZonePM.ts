import { UIProperties, UIProperty } from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
import { ServiceLocator } from '../../Infrastructure/Locators/ServiceLocator';
import { Output, EventEmitter } from '@angular/core';
import { PropertyChangedArgs } from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import { CustomFieldClass } from '../../Infrastructure/DataContracts/CustomFieldClass';


export class PortTimeZonePM {

    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public UIProperties: UIProperties;
    constructor() {
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }


    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { if (this.code != newValue) { this.code = newValue; this.MarkAsDirty("Code"); } }


    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { if (this.name != newValue) { this.name = newValue; this.MarkAsDirty("Name"); } }


    private notes: string;
    public get Notes() { return this.notes; }
    public set Notes(newValue: string) { if (this.notes != newValue) { this.notes = newValue; this.MarkAsDirty("Notes"); } }


    private inactive: boolean;
    public get Inactive() { return this.inactive; }
    public set Inactive(newValue: boolean) { if (this.inactive != newValue) { this.inactive = newValue; this.MarkAsDirty("Inactive"); } }


    private searchFields: string;
    public get SearchFields() { return this.searchFields; }
    public set SearchFields(newValue: string) { if (this.searchFields != newValue) { this.searchFields = newValue; this.MarkAsDirty("SearchFields"); } }


    private uTCOffset: string;
    public get UTCOffset() { return this.uTCOffset; }
    public set UTCOffset(newValue: string) { if (this.uTCOffset != newValue) { this.uTCOffset = newValue; this.MarkAsDirty("UTCOffset"); } }


    private uTCDSTOffset: string;
    public get UTCDSTOffset() { return this.uTCDSTOffset; }
    public set UTCDSTOffset(newValue: string) { if (this.uTCDSTOffset != newValue) { this.uTCDSTOffset = newValue; this.MarkAsDirty("UTCDSTOffset"); } }

    public OldEntityPM: PortTimeZonePM;

    public IsDirty: boolean;
    public DisableMarkAsDirty: boolean = false;
    MarkAsDirty(propertyName: string = null) {
        if (!this.DisableMarkAsDirty) {
            this.IsDirty = true;

            if (propertyName != null) {
                this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
                ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "Branch");

            }
        }
    }
    private MyClone: PortTimeZonePM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }

}
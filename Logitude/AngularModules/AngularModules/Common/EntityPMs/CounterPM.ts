
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {Output, EventEmitter}  from '@angular/core';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';

export class CounterPM {
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

    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { if (this.code != newValue) { this.code = newValue; this.MarkAsDirty("Code"); } }

    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { if (this.name != newValue) { this.name = newValue; this.MarkAsDirty("Name"); } }


    private objectTableId: string;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(newValue: string) { if (this.objectTableId != newValue) { this.objectTableId = newValue; this.MarkAsDirty("ObjectTableId"); } }

    private changedByUserId: string;
    public get ChangedByUserId() { return this.changedByUserId; }
    public set ChangedByUserId(newValue: string) { if (this.changedByUserId != newValue) { this.changedByUserId = newValue; this.MarkAsDirty("ChangedByUserId"); } }

    private changedDate: Date;
    public get ChangedDate() { return this.changedDate; }
    public set ChangedDate(newValue: Date) { if (this.changedDate != newValue) { this.changedDate = newValue; this.MarkAsDirty("ChangedDate"); } }

    public OldEntityPM: CounterPM;

    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "Counter");

        }
    }
    private MyClone: CounterPM;
}
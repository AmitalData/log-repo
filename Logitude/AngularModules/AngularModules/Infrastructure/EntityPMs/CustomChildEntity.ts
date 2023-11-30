declare var window: any;
import { UIProperties } from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {Output, EventEmitter}  from '@angular/core';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import { CustomChildObjectPM } from './CustomChildObjectPM';

export class CustomChildEntity {
    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public ParentObjectTableName: string;
    public EntityParentPM: any;
    public UIProperties: UIProperties;
    constructor(entityParentPM: any, parentObjectTableName: string, objectTableName: string) {
        this.EntityParentPM = entityParentPM;
        this.Name = objectTableName;
        this.ParentObjectTableName = parentObjectTableName;
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { if (this.changeSetOp != newValue) { this.changeSetOp = newValue; this.MarkAsDirty("ChangeSetOp"); } }

    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { if (this.name != newValue) { this.name = newValue; this.MarkAsDirty("Name"); } }

    public UniqueKey: string;

    private values: CustomChildObjectPM[];
    get Values() {
        if (this.values == null) {
            this.values = [];
        }
        return this.values;
    }

    set Values(newValue: CustomChildObjectPM[]) {
        if (this.values != newValue) {
            this.values = newValue;
        }
    }

    public AddCustomChildObject(item: CustomChildObjectPM) {
        if (item == null) return;
        let index = this.Values.indexOf(item);
        if (index != -1) return;
        if (this.EntityParentPM == null) return;
        item.ParentEntityId = this.EntityParentPM.Id;
        let parentObjcetTableId = window.ObjectTables.filter(d => d.Name == this.ParentObjectTableName)[0];
        if (parentObjcetTableId == null) return;
        item.ParentObjectTableId = parentObjcetTableId.Id;
        this.Values.push(item);
        this.MarkAsDirty();
    }

    public RemoveCustomChildObject(item: CustomChildObjectPM) {
        if (item == null) return;
        let index = this.Values.indexOf(item);
        if (index <= -1) return;
        this.Values.splice(index, 1);
        this.MarkAsDirty();
    }

    public OldEntityPM: CustomChildEntity;

    public IsDirty: boolean;
    public DisableMarkAsDirty: boolean = false;
    MarkAsDirty(propertyName: string = null) {
        if (this.DisableMarkAsDirty) return;
        this.IsDirty = true;
        if (propertyName == null) return;
        this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
        ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, this.ParentObjectTableName);
    }

    private MyClone: CustomChildEntity;
    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}
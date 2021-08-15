// no lxml 

import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {Output, EventEmitter}  from '@angular/core';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import {CustomFieldClass} from '../../Infrastructure/DataContracts/CustomFieldClass';


export class QuoteOPTemplateDetailsFieldPM {

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


    private quoteTemplateId: string;
    public get QuoteTemplateId() { return this.quoteTemplateId; }
    public set QuoteTemplateId(newValue: string) { if (this.quoteTemplateId != newValue) { this.quoteTemplateId = newValue; this.MarkAsDirty("QuoteTemplateId"); } }


    private fieldCode: string;
    public get FieldCode() { return this.fieldCode; }
    public set FieldCode(newValue: string) { if (this.fieldCode != newValue) { this.fieldCode = newValue; this.MarkAsDirty("FieldCode"); } }


    private row: number;
    public get Row() { return this.row; }
    public set Row(newValue: number) { if (this.row != newValue) { this.row = newValue; this.MarkAsDirty("Row"); } }


    private column: number;
    public get Column() { return this.column; }
    public set Column(newValue: number) { if (this.column != newValue) { this.column = newValue; this.MarkAsDirty("Column"); } }


    private isDelete: boolean;
    public get IsDelete() { return this.isDelete; }
    public set IsDelete(newValue: boolean) { if (this.isDelete != newValue) { this.isDelete = newValue; this.MarkAsDirty("IsDelete"); } }

    private isAdd: boolean;
    public get IsAdd() { return this.isAdd; }
    public set IsAdd(newValue: boolean) { if (this.isAdd != newValue) { this.isAdd = newValue; this.MarkAsDirty("IsAdd"); } }

    private isEdit: boolean;
    public get IsEdit() { return this.isEdit; }
    public set IsEdit(newValue: boolean) { if (this.isEdit != newValue) { this.isEdit = newValue; this.MarkAsDirty("IsEdit"); } }


    public OldEntityPM: QuoteOPTemplateDetailsFieldPM;

    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "QuoteTemplateDetailsField");

        }
    }
    private MyClone: QuoteOPTemplateDetailsFieldPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}

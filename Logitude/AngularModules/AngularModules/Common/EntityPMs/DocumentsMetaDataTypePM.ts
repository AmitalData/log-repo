import { EventEmitter, Output } from "@angular/core";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { PropertyChangedArgs } from "Infrastructure/EventEmitterArgs/PropertyChangedArgs";
import { ServiceLocator } from "Infrastructure/Locators/ServiceLocator";
import { ServiceHelper } from "Infrastructure/Utilities/ServiceHelper";

export class DocumentsMetaDataTypePM {
    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public UIProperties: UIProperties;
    public OldEntityPM: DocumentsMetaDataTypePM;
    public IsDirty: boolean;
    public DisableMarkAsDirty: boolean = false;
        
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

    private englishName: string;
    public get EnglishName() { return this.englishName; }
    public set EnglishName(newValue: string) { if (this.englishName != newValue) { this.englishName = newValue; this.MarkAsDirty("EnglishName"); } }

    private localName: string;
    public get LocalName() { return this.localName; }
    public set LocalName(newValue: string) { if (this.localName != newValue) { this.localName = newValue; this.MarkAsDirty("LocalName"); } }

    private inActive: boolean;
    public get InActive() { return this.inActive; }
    public set InActive(newValue: boolean) { if (this.inActive != newValue) { this.inActive = newValue; this.MarkAsDirty("InActive"); } }

    private customsMetaDataCode: string;
    public get CustomsMetaDataCode() { return this.customsMetaDataCode; }
    public set CustomsMetaDataCode(newValue: string) { if (this.customsMetaDataCode != newValue) { this.customsMetaDataCode = newValue; this.MarkAsDirty("CustomsMetaDataCode"); } }

    private format: string;
    public get Format() { return this.format; }
    public set Format(newValue: string) { if (this.format != newValue) { this.format = newValue; this.MarkAsDirty("Format"); } }

    private isHybrid: boolean;
    public get IsHybrid() { return this.isHybrid; }
    public set IsHybrid(newValue: boolean) { if (this.isHybrid != newValue) { this.isHybrid = newValue; this.MarkAsDirty("IsHybrid"); } }

    MarkAsDirty(propertyName: string = null) {
        if (!this.DisableMarkAsDirty) {
            this.IsDirty = true;

            if (propertyName != null) {
                this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
                ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "DocumentsMetaDataType");
            }
        }
    }
    
    private MyClone: DocumentsMetaDataTypePM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }

}
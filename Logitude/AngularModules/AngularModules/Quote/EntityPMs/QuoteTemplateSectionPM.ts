
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {Output, EventEmitter}  from '@angular/core';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import {CustomFieldClass} from '../../Infrastructure/DataContracts/CustomFieldClass';


export class QuoteTemplateSectionPM {

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




    private sectionDocId: string;
    public get SectionDocId() { return this.sectionDocId; }
    public set SectionDocId(newValue: string) { if (this.sectionDocId != newValue) { this.sectionDocId = newValue; this.MarkAsDirty("SectionDocId"); } }


    private order: number;
    public get Order() { return this.order; }
    public set Order(newValue: number) { if (this.order != newValue) { this.order = newValue; this.MarkAsDirty("Order"); } }


    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { if (this.name != newValue) { this.name = newValue; this.MarkAsDirty("Name"); } }



    private quoteTemplateSectionTypeCode: string;
    public get QuoteTemplateSectionTypeCode() { return this.quoteTemplateSectionTypeCode; }
    public set QuoteTemplateSectionTypeCode(newValue: string) { if (this.quoteTemplateSectionTypeCode != newValue) { this.quoteTemplateSectionTypeCode = newValue; this.MarkAsDirty("QuoteTemplateSectionTypeCode"); } }



    private templatedata: any;
    public get Templatedata() { return this.templatedata; }
    public set Templatedata(newValue: any) { if (this.templatedata != newValue) { this.templatedata = newValue; this.MarkAsDirty("Templatedata"); } }


    private description: string;
    public get Description() { return this.description; }
    public set Description(newValue: string) { if (this.description != newValue) { this.description = newValue; this.MarkAsDirty("Description"); } }

    private quoteId: string;
    public get QuoteId() { return this.quoteId; }
    public set QuoteId(newValue: string) { if (this.quoteId != newValue) { this.quoteId = newValue; this.MarkAsDirty("QuoteId"); } }


    private ischangeBodySection: boolean;
    public get IschangeBodySection() { return this.ischangeBodySection; }
    public set IschangeBodySection(newValue: boolean) { if (this.ischangeBodySection != newValue) { this.ischangeBodySection = newValue; this.MarkAsDirty("IschangeBodySection"); } }

    private isSettingTypeCodeS: boolean;
    public get IsSettingTypeCodeS() { return this.isSettingTypeCodeS; }
    public set IsSettingTypeCodeS(newValue: boolean) { if (this.isSettingTypeCodeS != newValue) { this.isSettingTypeCodeS = newValue; this.MarkAsDirty("IsSettingTypeCodeS"); } }

    private isSettingTypeCodeP: boolean;
    public get IsSettingTypeCodeP() { return this.isSettingTypeCodeP; }
    public set IsSettingTypeCodeP(newValue: boolean) { if (this.isSettingTypeCodeP != newValue) { this.isSettingTypeCodeP = newValue; this.MarkAsDirty("IsSettingTypeCodeP"); } }

    private isCancel: boolean;
    public get IsCancel() { return this.isCancel; }
    public set IsCancel(newValue: boolean) { if (this.isCancel != newValue) { this.isCancel = newValue; this.MarkAsDirty("IsCancel"); } }

    private isQuoteEdited: boolean;
    public get IsQuoteEdited() { return this.isQuoteEdited; }
    public set IsQuoteEdited(newValue: boolean) { if (this.isQuoteEdited != newValue) { this.isQuoteEdited = newValue; this.MarkAsDirty("IsQuoteEdited"); } }


    private isExcluded: boolean;
    public get IsExcluded() { return this.isExcluded; }
    public set IsExcluded(newValue: boolean) { if (this.isExcluded != newValue) { this.isExcluded = newValue; this.MarkAsDirty("IsExcluded"); } }





    public OldEntityPM: QuoteTemplateSectionPM;

    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "QuoteTemplateSection");

        }
    }
    private MyClone: QuoteTemplateSectionPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}

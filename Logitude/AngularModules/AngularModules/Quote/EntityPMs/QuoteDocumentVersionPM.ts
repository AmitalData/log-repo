
import {QuotePM} from './QuotePM';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {Output, EventEmitter}  from '@angular/core';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import {CustomFieldClass} from '../../Infrastructure/DataContracts/CustomFieldClass';

export class QuoteDocumentVersionPM {
    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public UIProperties: UIProperties;
    constructor(_entityParentPM: any) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
        
    }

   
    private quoteId: string;
    public get QuoteId() { return this.quoteId; }
    public set QuoteId(newValue: string) { if (this.quoteId != newValue) { this.quoteId = newValue; this.MarkAsDirty("QuoteId"); } }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { if (this.tenant != newValue) { this.tenant = newValue; this.MarkAsDirty("Tenant"); } }

    private versionNumber: number;
    public get VersionNumber() { return this.versionNumber; }
    public set VersionNumber(newValue: number) { if (this.versionNumber != newValue) { this.versionNumber = newValue; this.MarkAsDirty("VersionNumber"); } }

    private createDate: Date;
    public get CreateDate() { return this.createDate; }
    public set CreateDate(newValue: Date) { if (this.createDate != newValue) { this.createDate = newValue; this.MarkAsDirty("CreateDate"); } }


    private updateDate: Date;
    public get UpdateDate() { return this.updateDate; }
    public set UpdateDate(newValue: Date) { if (this.updateDate != newValue) { this.updateDate = newValue; this.MarkAsDirty("UpdateDate"); } }


    private createdByUserId: string;
    public get CreatedByUserId() { return this.createdByUserId; }
    public set CreatedByUserId(newValue: string) { if (this.createdByUserId != newValue) { this.createdByUserId = newValue; this.MarkAsDirty("CreatedByUserId"); } }


    private updatedByUserId: string;
    public get UpdatedByUserId() { return this.updatedByUserId; }
    public set UpdatedByUserId(newValue: string) { if (this.updatedByUserId != newValue) { this.updatedByUserId = newValue; this.MarkAsDirty("UpdatedByUserId"); } }

    private versionType: string;
    public get VersionType() { return this.versionType; }
    public set VersionType(newValue: string) { if (this.versionType != newValue) { this.versionType = newValue; this.MarkAsDirty("VersionType"); } }

    private documentId: string;
    public get DocumentId() { return this.documentId; }
    public set DocumentId(newValue: string) { if (this.documentId != newValue) { this.documentId = newValue; this.MarkAsDirty("DocumentId"); } }

    private sendDate: Date;
    public get SendDate() { return this.sendDate; }
    public set SendDate(newValue: Date) { if (this.sendDate != newValue) { this.sendDate = newValue; this.MarkAsDirty("SendDate"); } }


    private isSent: boolean;
    public get IsSent() { return this.isSent; }
    public set IsSent(newValue: boolean) { if (this.isSent != newValue) { this.isSent = newValue; this.MarkAsDirty("IsSent"); } }


    private quoteTemplateId: string;
    public get QuoteTemplateId() { return this.quoteTemplateId; }
    public set QuoteTemplateId(newValue: string) { if (this.quoteTemplateId != newValue) { this.quoteTemplateId = newValue; this.MarkAsDirty("QuoteTemplateId"); } }


    private createdByUserName: string;
    public get CreatedByUserName() { return this.createdByUserName; }
    public set CreatedByUserName(newValue: string) { if (this.createdByUserName != newValue) { this.createdByUserName = newValue; this.MarkAsDirty("CreatedByUserName"); } }

    private updateByUserName: string;
    public get UpdateByUserName() { return this.updateByUserName; }
    public set UpdateByUserName(newValue: string) { if (this.updateByUserName != newValue) { this.updateByUserName = newValue; this.MarkAsDirty("UpdateByUserName"); } }


    private versionTypeName: string;
    public get VersionTypeName() { return this.versionTypeName; }
    public set VersionTypeName(newValue: string) { if (this.versionTypeName != newValue) { this.versionTypeName = newValue; this.MarkAsDirty("VersionTypeName"); } }

    private fileSize: number;
    public get FileSize() { return this.fileSize; }
    public set FileSize(newValue: number) { if (this.fileSize != newValue) { this.fileSize = newValue; this.MarkAsDirty("FileSize"); } }

    private fileName: string;
    public get FileName() { return this.fileName; }
    public set FileName(newValue: string) { if (this.fileName != newValue) { this.fileName = newValue; this.MarkAsDirty("FileName"); } }

    private displayVersionTypeName: string;
    public get DisplayVersionTypeName() { return this.displayVersionTypeName; }
    public set DisplayVersionTypeName(newValue: string) { if (this.displayVersionTypeName != newValue) { this.displayVersionTypeName = newValue;  } }


    private extension: string;
    public get Extension() { return this.extension; }
    public set Extension(newValue: string) { if (this.extension != newValue) { this.extension = newValue; } }
    

    public OldEntityPM: QuoteDocumentVersionPM;

    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

   
    public UniqueKey: string;

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { if (this.changeSetOp != newValue) { this.changeSetOp = newValue; this.MarkAsDirty("ChangeSetOp"); } }



    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;
        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "QuoteDocumentVersionPM");

        }
    }
    private MyClone: QuoteDocumentVersionPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}

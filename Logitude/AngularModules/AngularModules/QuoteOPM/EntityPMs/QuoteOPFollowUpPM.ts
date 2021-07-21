import {QuoteOPPM} from './QuoteOPPM';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {Output, EventEmitter}  from '@angular/core';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';

export class QuoteOPFollowUpPM {

    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public UIProperties: UIProperties;
    constructor(_entityParentPM: any) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; this.MarkAsDirty(); }

    private shipmentId: string;
    public get ShipmentId() { return this.shipmentId; }
    public set ShipmentId(newValue: string) { this.shipmentId = newValue; this.MarkAsDirty(); }

    private jobId: string;
    public get JobId() { return this.jobId; }
    public set JobId(newValue: string) { this.jobId = newValue; this.MarkAsDirty(); }

    private quoteId: string;
    public get QuoteId() { return this.quoteId; }
    public set QuoteId(newValue: string) { this.quoteId = newValue; this.MarkAsDirty(); }

    private externalDocumentId: string;
    public get ExternalDocumentId() { return this.externalDocumentId; }
    public set ExternalDocumentId(newValue: string) { this.externalDocumentId = newValue; this.MarkAsDirty(); }

    private date: Date;
    public get Date() { return this.date; }
    public set Date(newValue: Date) { this.date = newValue; this.MarkAsDirty(); }

    private isNew: boolean;
    public get IsNew() { return this.isNew; }
    public set IsNew(newValue: boolean) { this.isNew = newValue; this.MarkAsDirty(); }

    private note: string;
    public get Note() { return this.note; }
    public set Note(newValue: string) { this.note = newValue; this.MarkAsDirty(); }

    private doneNote: string;
    public get DoneNote() { return this.doneNote; }
    public set DoneNote(newValue: string) { this.doneNote = newValue; this.MarkAsDirty(); }

    private doneDateTime: Date;
    public get DoneDateTime() { return this.doneDateTime; }
    public set DoneDateTime(newValue: Date) { this.doneDateTime = newValue; this.MarkAsDirty(); }

    private done: boolean;
    public get Done() { return this.done; }
    public set Done(newValue: boolean) { this.done = newValue; this.MarkAsDirty(); }

    private internalDocumentId: string;
    public get InternalDocumentId() { return this.internalDocumentId; }
    public set InternalDocumentId(newValue: string) { this.internalDocumentId = newValue; this.MarkAsDirty(); }

    private legType: string;
    public get LegType() { return this.legType; }
    public set LegType(newValue: string) { this.legType = newValue; this.MarkAsDirty(); }

    private deleted: boolean;
    public get Deleted() { return this.deleted; }
    public set Deleted(newValue: boolean) { this.deleted = newValue; this.MarkAsDirty(); }

    private entityDateId: string;
    public get EntityDateId() { return this.entityDateId; }
    public set EntityDateId(newValue: string) { this.entityDateId = newValue; this.MarkAsDirty(); }

    private eventTypeId: string;
    public get EventTypeId() { return this.eventTypeId; }
    public set EventTypeId(newValue: string) { this.eventTypeId = newValue; this.MarkAsDirty(); }

    private eventTypeFollowUpName: string;
    public get EventTypeFollowUpName() { return this.eventTypeFollowUpName; }
    public set EventTypeFollowUpName(newValue: string) { this.eventTypeFollowUpName = newValue; this.MarkAsDirty(); }

    private manualActivatedFollowUp: boolean;
    public get ManualActivatedFollowUp() { return this.manualActivatedFollowUp; }
    public set ManualActivatedFollowUp(newValue: boolean) { this.manualActivatedFollowUp = newValue; this.MarkAsDirty(); }

    private ownerUserId: string;
    public get OwnerUserId() { return this.ownerUserId; }
    public set OwnerUserId(newValue: string) { this.ownerUserId = newValue; this.MarkAsDirty(); }

    private ownerUserName: string;
    public get OwnerUserName() { return this.ownerUserName; }
    public set OwnerUserName(newValue: string) { this.ownerUserName = newValue; this.MarkAsDirty(); }


    private documentTypeId: string;
    public get DocumentTypeId() { return this.documentTypeId; }
    public set DocumentTypeId(newValue: string) { this.documentTypeId = newValue; this.MarkAsDirty(); }


    private automationId: string;
    public get AutomationId() { return this.automationId; }
    public set AutomationId(newValue: string) { this.automationId = newValue; this.MarkAsDirty(); }

    private area: string;
    public get Area() { return this.area; }
    public set Area(newValue: string) { this.area = newValue; this.MarkAsDirty(); }


    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { if (this.changeSetOp != newValue) { this.changeSetOp = newValue; this.MarkAsDirty("ChangeSetOp"); } }

    public OldEntityPM: QuoteOPFollowUpPM;
    public UniqueKey: string;
    public IsDirty: boolean;
    public DisableMarkAsDirty: boolean = false;
    MarkAsDirty(propertyName: string = null) {
        if (!this.DisableMarkAsDirty) {
            this.IsDirty = true;

            if (this.EntityParentPM) {
                this.EntityParentPM.MarkAsDirty();
            }

            if (propertyName != null) {
                this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
                ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "ShipmentReceivable");
            }
        }
    }
}

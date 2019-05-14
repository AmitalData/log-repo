import {Component} from '@angular/core';
import {QuotePM} from '../../EntityPMs/QuotePM';
import {QuoteEventNotesArgs} from '../../Args';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './QuoteEventNotesComponent.html',
})

export class QuoteEventNotesComponent extends BaseComponent {
    public EntityPM: QuotePM = null;
    public DataContext: QuoteEventNotesComponent = this;
    public ObjectTableName: string = "Quote";
    public NotesHeader: string = "Notes";
    public ShowClosingReason: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsConvertQuoteType: boolean = false;
    constructor() {
        super();
    }

    SetWindowArgs(args: QuoteEventNotesArgs) {
        this.EntityPM = args.EntityPM;
        this.NotesHeader = args.NotesHeader;
        this.ShowClosingReason = args.ShowClosingReason;
        this.IsConvertQuoteType = args.IsConvertQuoteType;
        this.EventNote = null;
        this.Clone();
    }


    get QuoteClosingReasonCode() { return this.EntityPM.QuoteClosingReasonCode; }
    set QuoteClosingReasonCode(value: string) {
        if (this.EntityPM.QuoteClosingReasonCode != value) {
            this.EntityPM.QuoteClosingReasonCode = value;
        }
    }

    get EventNote() { return this.EntityPM.EventNote; }
    set EventNote(value: string) {
        if (this.EntityPM.EventNote != value) {
            this.EntityPM.EventNote = value;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('EventNote');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}

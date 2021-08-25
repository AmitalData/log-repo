import {Component} from '@angular/core';
import {QuoteOPPM} from '../../EntityPMs/QuoteOPPM';
import {QuoteEventNotesArgs} from '../../Args';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';

@Component({
    
    templateUrl: './QuoteEventNotesComponent.html',
})

export class QuoteEventNotesComponent extends BaseComponent {
    public EntityPM: QuoteOPPM = null;
    public DataContext: QuoteEventNotesComponent = this;
    public ObjectTableName: string = "QuoteOP";
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


    get QuoteClosingReasonId() { return this.EntityPM.QuoteClosingReasonId; }
    set QuoteClosingReasonId(value: string) {
        if (this.EntityPM.QuoteClosingReasonId != value) {
            this.EntityPM.QuoteClosingReasonId = value;
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

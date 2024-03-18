import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { JournalPM } from '../../EntityPMs/JournalPM';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { JournalLinePM } from '../../EntityPMs/JournalLinePM';
import { AppTool } from 'Infrastructure/Tools';


@Component({
    selector: 'CopyJournalComponent',

    templateUrl: './CopyJournalComponent.html',
})

export class CopyJournalComponent extends BaseComponent implements OnInit {
    public EntityPM: JournalPM;
    public DataContext: any = this;
    public ObjectTableName: string = "Journal";
    public ValidationErrorsList: string[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;

    disableDates: boolean = true;
    disableNotes: boolean = true;
    disableRef1: boolean = true;
    disableRef2: boolean = true;
    
    constructor() {
        super();

        this._entityResourceService.getEntityResourceByTableName("Journal").subscribe((response: any) => { });

    }


    SetWindowArgs(args: any) {
        if (args != null) {
            this.EntityPM = args.JournalPM;
        }
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {

        var valid: boolean = true;
        valid = this.ValidateJournalDates();// && this.ValidateJournalNotes() && this.ValidateJournalRef1() && this.ValidateJournalRef2();
        if (valid) {
            var entityPM: JournalPM = new JournalPM();
            this.NewJournalMapping(entityPM);
            this.CopyJournalData(entityPM);
            this.OpenJournalEditScreen(entityPM);
        }

    }
    ValidateJournalDates() {
        this.ValidationErrorsList = [];
        if (!this.Dates && (!this.AccountingDate || !this.DueDate || !this.DocumentDate)) {
            this.FillErrorMessage();
            return false
        }
        else return true;
    }

    ValidateJournalNotes() {
        this.ValidationErrorsList = [];
        if (!this.NotesCB && this.NotesUserEmpty) {
            this.NotesErrorMessage();
            return false
        }
        else return true;
    }

    ValidateJournalRef1() {
        this.ValidationErrorsList = [];
        if (!this.Ref1CB && this.Ref1UserEmpty) {
            this.Ref1ErrorMessage();
            return false
        }
        else return true;
    }

    ValidateJournalRef2() {
        this.ValidationErrorsList = [];
        if (!this.Ref2CB && this.Ref2UserEmpty) {
            this.Ref2ErrorMessage();
            return false
        }
        else return true;
    }



    FillErrorMessage() {
        var FIELD_IS_REQUIERD: string = null;
        FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (!this.AccountingDate) {
            var error: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("JournalLine.F.AccountingDate"));
            this.ValidationErrorsList.push(error);
        }
        if (!this.DueDate) {
            var error: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("JournalLine.F.DueDate"));
            this.ValidationErrorsList.push(error);
        }
        if (!this.DocumentDate) {
            var error: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("JournalLine.F.DocumentDate"));
            this.ValidationErrorsList.push(error);
        }
    }

    NotesErrorMessage() {
        var FIELD_IS_REQUIERD: string = null;
        FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var error: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("JournalLine.F.Notes"));
            this.ValidationErrorsList.push(error);
    }

    Ref1ErrorMessage() {
        var FIELD_IS_REQUIERD: string = null;
        FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var error: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("JournalLine.F.Reference1"));
        this.ValidationErrorsList.push(error);
    }

    Ref2ErrorMessage() {
        var FIELD_IS_REQUIERD: string = null;
        FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var error: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("JournalLine.F.Reference2"));
        this.ValidationErrorsList.push(error);
    }

    public forceFocus: boolean = false;
    ngOnInit() {
        var t = setTimeout(() => { this.forceFocus = true; }, 1);
    }
    NewJournalMapping(journal: JournalPM) {
        journal.IsNew = true;
        journal.TypeCode = "0"; // Manual
        journal.AccountingEntityCode = "1"; // Journal
        journal.CreatedByUserId = SessionLocator.LoggedUserId;
        journal.Tenant = SessionLocator.Tenant;
        journal.AccountingDate = this.AccountingDate ? this.AccountingDate : this.EntityPM.AccountingDate;
        journal.DueDate = this.DueDate ? this.DueDate : this.EntityPM.DueDate;
        journal.DocumentDate = this.DocumentDate ? this.DocumentDate : this.EntityPM.DocumentDate;
        journal.CurrencyId = this.AmountsAndCurrencies ? journal.CurrencyId : null;
        journal.Copied = true;
        journal.CopiedFrom = this.EntityPM.JournalNumber;


    }
    OpenJournalEditScreen(journal: JournalPM) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityPM: journal, ObjectTableName: 'Journal', BackButtonLabel: TextCodeTranslator.Translate("Journal") + " " + this.EntityPM.JournalNumber
                });
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    this.CancelButtonClicked();
                });
            });
    }

    CopyJournalData(entityPM: JournalPM) {
        for (var i = 0; i < this.EntityPM.JournalLines.length; i++) {
            var line: JournalLinePM = this.NewJournalLine(entityPM, this.EntityPM.JournalLines[i]);
            entityPM.JournalLines.push(line);
        }
        return entityPM;

    }
    NewJournalLine(journal: JournalPM, originalJournalLine: JournalLinePM) {
        var journalLine = new JournalLinePM(journal);
        journalLine = originalJournalLine;
        journalLine.Reference1 = this.Ref1CB ? originalJournalLine.Reference1 : this.Reference1UserText;
        journalLine.Reference2 = this.Ref2CB ? originalJournalLine.Reference2 : this.Reference2UserText;
        journalLine.Reference3 = null;
        journalLine.Notes = this.NotesCB ? originalJournalLine.Notes : this.NotesUserText;
        journalLine.AccountingDate = this.Dates ? originalJournalLine.AccountingDate : this.AccountingDate;
        journalLine.DueDate = this.Dates ? originalJournalLine.DueDate : this.DueDate;
        journalLine.DocumentDate = this.Dates ? originalJournalLine.DocumentDate : this.DocumentDate;
        journalLine.CurrencyId = this.AmountsAndCurrencies ? originalJournalLine.CurrencyId : null;
        journalLine.CurrencyCode = this.AmountsAndCurrencies ? originalJournalLine.CurrencyCode : null;
       
        if (AppTool.IsNullOrEmpty(this.coefficientForAmountsAndCurrencies) || this.coefficientForAmountsAndCurrencies.toString() === '0') {
            this.coefficientForAmountsAndCurrencies = 1; // default value = 1
        }

        journalLine.LocalAmount = this.AmountsAndCurrencies ? originalJournalLine.LocalAmount * this.coefficientForAmountsAndCurrencies : null;
        journalLine.ForeignAmount = this.AmountsAndCurrencies ? originalJournalLine.ForeignAmount * this.coefficientForAmountsAndCurrencies : null;

        return journalLine;
    }
    private amountsAndCurrencies: boolean = true;
    get AmountsAndCurrencies() { return this.amountsAndCurrencies; }
    set AmountsAndCurrencies(value: boolean) {
        if (this.amountsAndCurrencies != value) {
            this.amountsAndCurrencies = value;
        }
    }

    private coefficientForAmountsAndCurrencies: number = 1;
    get CoefficientForAmountsAndCurrencies() { return this.coefficientForAmountsAndCurrencies; }
    set CoefficientForAmountsAndCurrencies(value: number) {
        if (this.coefficientForAmountsAndCurrencies != value) {
            this.coefficientForAmountsAndCurrencies = value;
        }
    }




    private notesUserText: string = "";
    get NotesUserText() { return this.notesUserText; }
    set NotesUserText(value: string) {
        if (this.notesUserText != value) {
            this.notesUserText = value;
        }
    }

    get NotesUserEmpty() {
        var rv: boolean = (this.notesUserText.trim().length === 0);
        return (rv);
    }




    private reference1UserText: string = "";
    get Reference1UserText() { return this.reference1UserText; }
    set Reference1UserText(value: string) {
        if (this.reference1UserText != value) {
            this.reference1UserText = value;
        }
    }

    get Ref1UserEmpty() {
        var rv: boolean = (this.reference1UserText.trim().length === 0);
        return (rv);
    }




    private reference2UserText: string = "";
    get Reference2UserText() { return this.reference2UserText; }
    set Reference2UserText(value: string) {
        if (this.reference2UserText != value) {
            this.reference2UserText = value;
        }
    }
    get Ref2UserEmpty() {
        var rv: boolean = (this.reference2UserText.trim().length === 0);
        return (rv);
    }



    private notesCB: boolean = true;
    get NotesCB() { return this.notesCB; }
    set NotesCB(value: boolean) {
        if (this.notesCB != value) {
            this.notesCB = value;
            this.disableNotes = value;

            if (this.disableNotes)
                this.ResetNotes();
        }
    }


    private ref1CB: boolean = true;
    get Ref1CB() { return this.ref1CB; }
    set Ref1CB(value: boolean) {
        if (this.ref1CB != value) {
            this.ref1CB = value;
            this.disableRef1 = value;

            if (this.disableRef1)
                this.ResetRef1();
        }
    }


    private ref2CB: boolean = true;
    get Ref2CB() { return this.ref2CB; }
    set Ref2CB(value: boolean) {
        if (this.ref2CB != value) {
            this.ref2CB = value;
            this.disableRef2 = value;

            if (this.disableRef2)
                this.ResetRef2();
        }
    }


    private dates: boolean = true;
    get Dates() { return this.dates; }
    set Dates(value: boolean) {
        if (this.dates != value) {
            this.dates = value;

            this.disableDates = value;

            if(this.disableDates)
                this.ResetDates();


        }
    }

    private accountingDate: Date;
    get AccountingDate() { return this.accountingDate; }
    set AccountingDate(value: Date) {
        if (this.accountingDate != value) {
            this.accountingDate = value;
        }
    }

    private dueDate: Date;
    get DueDate() { return this.dueDate; }
    set DueDate(value: Date) {
        if (this.dueDate != value) {
            this.dueDate = value;
        }
    }

    private documentDate: Date;
    get DocumentDate() { return this.documentDate; }
    set DocumentDate(value: Date) {
        if (this.documentDate != value) {
            this.documentDate = value;
        }
    }


    ResetDates(){
        this.AccountingDate = null;
        this.DueDate = null;
        this.DocumentDate = null;
    }

    ResetNotes() {
        this.NotesUserText = "";
    }
    ResetRef1() {
        this.Reference1UserText = "";

    }
    ResetRef2() {
        this.Reference2UserText = "";
    }
}

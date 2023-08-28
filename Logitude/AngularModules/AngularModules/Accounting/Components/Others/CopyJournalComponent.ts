import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { JournalPM } from '../../EntityPMs/JournalPM';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { JournalLinePM } from '../../EntityPMs/JournalLinePM';


@Component({
    selector: 'CopyJournalComponent',

    templateUrl: './CopyJournalComponent.html',
})

export class CopyJournalComponent extends BaseComponent implements OnInit  {
    public EntityPM: JournalPM;
    public DataContext: any = this;
    public ObjectTableName: string = "Journal";
    public ValidationErrorsList: string[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;

    disableDates: boolean = true;
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
         valid  = this.ValidateJournalDates();       
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
    NewJournalLine(journal: JournalPM, originalJourbnalLine:JournalLinePM) {
        var journalLine = new JournalLinePM(journal);
        journalLine = originalJourbnalLine;
        journalLine.Reference1 = this.ReferencesAndNotes ? originalJourbnalLine.Reference1 : null;
        journalLine.Reference2 = this.ReferencesAndNotes ? originalJourbnalLine.Reference2 : null;
        journalLine.Reference3 = this.ReferencesAndNotes ? originalJourbnalLine.Reference3 : null;
        journalLine.Notes = this.ReferencesAndNotes ? originalJourbnalLine.Notes : null;
        journalLine.AccountingDate = this.Dates ? originalJourbnalLine.AccountingDate : this.AccountingDate;
        journalLine.DueDate = this.Dates ? originalJourbnalLine.DueDate : this.DueDate;
        journalLine.DocumentDate = this.Dates ? originalJourbnalLine.DocumentDate : this.DocumentDate;
        journalLine.CurrencyId = this.AmountsAndCurrencies ? originalJourbnalLine.CurrencyId : null;
        journalLine.CurrencyCode = this.AmountsAndCurrencies ? originalJourbnalLine.CurrencyCode : null;    
        journalLine.LocalAmount = this.AmountsAndCurrencies ? originalJourbnalLine.LocalAmount * this.coefficientForAmountsAndCurrencies : null;
        journalLine.ForeignAmount = this.AmountsAndCurrencies ? originalJourbnalLine.ForeignAmount * this.coefficientForAmountsAndCurrencies : null;
        
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
            debugger
            this.coefficientForAmountsAndCurrencies = value;
        }
    }

    private referencesAndNotes: boolean = true;
    get ReferencesAndNotes() { return this.referencesAndNotes; }
    set ReferencesAndNotes(value: boolean) {
        if (this.referencesAndNotes != value) {
            this.referencesAndNotes = value;
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
}

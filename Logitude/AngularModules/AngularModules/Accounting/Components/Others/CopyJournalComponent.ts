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

export class CopyJournalComponent extends BaseComponent {
    public EntityPM: JournalPM;
    public DataContext: any = this;
    public ObjectTableName: string = "Journal";
    public ValidationErrorsList: string[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();

        this._entityResourceService.getEntityResourceByTableName("Journal").subscribe((response: any) => { });

    }

    SetUIProperties() {

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
      
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewJournal");


        var entityPM: JournalPM = new JournalPM();
        entityPM.IsNew = true;
        this.CopyJournalData(entityPM);
       
        entityPM.TypeCode = "0"; // Manual
        entityPM.AccountingEntityCode = "1"; // Journal

        entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        entityPM.Tenant = SessionLocator.Tenant;
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityPM: entityPM, ObjectTableName: 'Journal', BackButtonLabel: TextCodeTranslator.Translate("Accounting.General.O.Main")
                   
                });
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    //this.LoadAllScreenData();
                    ////this.isWindowOpened = false;
                });
            });
    }

    CopyJournalData(entityPM: JournalPM) {
        entityPM.JournalLines = this.EntityPM.JournalLines;
        if (!this.AmountsAndCurrencies) {
            for (var i = 0; i < this.EntityPM.JournalLines.length; i++) {
                entityPM.JournalLines.push(this.NewJournalLine(entityPM, this.EntityPM.JournalLines[i]));
            }
        }
        return entityPM;

    }
    NewJournalLine(journal: JournalPM, originalJourbnalLine:JournalLinePM) {
        var journalLine = new JournalLinePM(journal);
        journalLine.Reference1 = this.ReferencesAndNotes ? originalJourbnalLine.Reference1 : null;
        journalLine.Reference2 = this.ReferencesAndNotes ? originalJourbnalLine.Reference2 : null;
        journalLine.Reference3 = this.ReferencesAndNotes ? originalJourbnalLine.Reference3 : null;
        journalLine.Notes = this.ReferencesAndNotes ? originalJourbnalLine.Notes : null;
        journalLine.AccountingDate = this.Dates ? originalJourbnalLine.AccountingDate : null;
        journalLine.DueDate = this.Dates ? originalJourbnalLine.DueDate : null;
        journalLine.DocumentDate = this.Dates ? originalJourbnalLine.DocumentDate : null;
        journalLine.CurrencyId = this.AmountsAndCurrencies ? originalJourbnalLine.CurrencyId : null;
        journalLine.LocalAmount = this.AmountsAndCurrencies ? originalJourbnalLine.LocalAmount : null;
        journalLine.ForeignAmount = this.AmountsAndCurrencies ? originalJourbnalLine.ForeignAmount : null;
        journalLine.Line = originalJourbnalLine.Line;
        journalLine.Tenant = originalJourbnalLine.Tenant;
        journalLine.DebitAccountId = originalJourbnalLine.DebitAccountId;
        journalLine.CreditAccountId = originalJourbnalLine.CreditAccountId;
        journalLine.ActionTypeCode = originalJourbnalLine.ActionTypeCode;
        

        return journalLine;
    }
    private amountsAndCurrencies: boolean = true;
    get AmountsAndCurrencies() { return this.amountsAndCurrencies; }
    set AmountsAndCurrencies(value: boolean) {
        if (this.amountsAndCurrencies != value) {
            this.amountsAndCurrencies = value;
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
        }
    }
}

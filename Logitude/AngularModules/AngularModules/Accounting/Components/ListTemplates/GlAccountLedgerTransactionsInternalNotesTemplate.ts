import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Component, ChangeDetectorRef } from '@angular/core';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { InterestTransactionExtendedListService } from '../../Services/ExtendedLists/InterestTransactionExtendedListService';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { JournalExtendedListService } from '../../Services/ExtendedLists/JournalExtendedListService';
import { ARPaymentExtendedListService } from '../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService';
import { OnInit, Output, EventEmitter, ComponentRef, QueryList } from '@angular/core';
import { LedgerTransactionPM } from '../../EntityPMs/LedgerTransactionPM';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

@Component({

    templateUrl: "./GlAccountLedgerTransactionsInternalNotesTemplate.html"
})
export class GlAccountLedgerTransactionsInternalNotesTemplate {

    public isRTL: boolean = false;
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    private CurrentSession = SessionLocator.SelectedSession;
    public EntityPM: LedgerTransactionPM = null;
    public isUsedOutside: boolean = false; // when view tab inside customer ..
   // public myService: InterestTransactionExtendedListService;
    InterestTransaction: ObservableCollection;
    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public Source: any;

    public TenantCurrencySign: string;

    public journalExtendedListService = new JournalExtendedListService();
    public arPaymentExtendedListService = new ARPaymentExtendedListService();

    @Output() CheckBoxChecked = new EventEmitter();
    @Output() Changed: EventEmitter<boolean> = new EventEmitter<boolean>();


    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef) {

        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;
        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
        }
        this.InterestTransaction = new ObservableCollection([]);
       // this.myService = new InterestTransactionExtendedListService();

    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;

    }

    OpenLedgerTransactionInternalNote(line: any) {

        var logWindow = new LogitudeWindow();
        logWindow.Width = 450;
        logWindow.Height = 350;
        logWindow.Title = TextCodeTranslator.Translate("ARInvoice.F.InternalNotes");
        logWindow.WindowArgs = { ledgerTransaction: line };
        logWindow.Show('./Accounting/Components/Others/LedgerTransactionInternalNotesComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.CD.detectChanges();
            this.CurrentSession.StopBusyIndicator();
        });

    }
}

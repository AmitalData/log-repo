import { SessionLocator } from './../../../Infrastructure/Utilities/SessionLocator';
import {Component,ChangeDetectorRef} from '@angular/core';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import { InterestTransactionExtendedListService } from './../../Services/ExtendedLists/InterestTransactionExtendedListService';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {JournalExtendedListService} from '../../Services/ExtendedLists/JournalExtendedListService';
import {ARPaymentExtendedListService} from '../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService';
import {OnInit, Output, EventEmitter, ComponentRef, QueryList} from '@angular/core';
import {InterestTransactionPM} from '../../EntityPMs/InterestTransactionPM';

@Component({

    templateUrl: "./GlAccountInterestTransactionsNotesTemplate.html"
})
export class GlAccountInterestTransactionsNotesTemplate {

    public isRTL: boolean = false;
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    private CurrentSession = SessionLocator.SelectedSession;
    public EntityPM: InterestTransactionPM = null;
    public isUsedOutside: boolean = false; // when view tab inside customer ..
    public myService: InterestTransactionExtendedListService;
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
     
   

    constructor(private entityArgs: EntityArgs,private CD: ChangeDetectorRef) {
        
        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;
        if (ObjectsLocator.GlobalSetting)
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
            this.InterestTransaction = new ObservableCollection([]);
            this.myService = new InterestTransactionExtendedListService();


            
        // Set Entity
        if(entityArgs)
        {
            this.EntityPM = entityArgs.EntityPM;    
        }
        else
        {
            this.isUsedOutside = true;
        }
       
    }
    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;

    }
    OpenInterestTransactionNote(line: any){

        var logWindow = new LogitudeWindow();
        logWindow.Width = 450;
        logWindow.Height = 350;
        logWindow.Title = TextCodeTranslator.Translate("InterestTransaction Note");
        logWindow.WindowArgs = { interestTransaction: line };
        logWindow.Show('./Accounting/Components/Others/InterestTransactionNoteComponent');
        logWindow.WindowClosed.subscribe(($event: any) => {
            this.GetInterestTransactionNotes();
        });

    }

    GetInterestTransactionNotes(){
        // if(this.EntityPM.CardId){
        //     this.accountingNotesList = [];
        //     this.isNotesLoading = true;
        //     // setTimeout(() => {

        //     this._AccountingNoteExtendedListService.GetNotesByCard(this.EntityPM.CardId)
        //         .subscribe((res:ServiceResponse) =>
        //         {
        //                 this.isNotesLoading = false;


        //             if(res.HasError){
        //                 var msg = new MessageWindow();
        //                 msg.Show("Get Accounting Note error: " + res.ErrorsArray[0]);
        //             }else{
        //                 var notesList = res.Result;
        //                 this.accountingNotesList = notesList;
        //             }
        //         });
        // }
    }

}

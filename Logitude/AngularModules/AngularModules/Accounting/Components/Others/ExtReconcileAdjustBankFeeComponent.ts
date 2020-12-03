
import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { GLAccountPM } from '../../EntityPMs/GLAccountPM';
import { ReconciliationPM } from '../../EntityPMs/ReconciliationPM';
import { JournalPM } from '../../EntityPMs/JournalPM';

import { ReconciliationLinePM } from '../../EntityPMs/ReconciliationLinePM';
import { LedgerTransactionList } from '../../EntityLists/LedgerTransactionList';
import { AutomaticReconcileMethodList } from '../../EntityLists/AutomaticReconcileMethodList';
import { LedgerTransactionPM } from '../../EntityPMs/LedgerTransactionPM';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { ApiQueryFilters, FilterItem } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ReconcileEventManager } from '../../Utilities/ReconcileEventManager';
import { ReconciliationExtendedPMService } from '../../Services/ExtendedPMs/ReconciliationExtendedPMService';
import { LedgerTransactionExtendedListService } from '../../Services/ExtendedLists/LedgerTransactionExtendedListService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { AccountingPeriodListService } from '../../Services/StandardLists/AccountingPeriodListService';
import { AccountingPeriodList } from '../../EntityLists/AccountingPeriodList';
import { FullAccountingSettingListService } from '../../Services/StandardLists/FullAccountingSettingListService';
import { FullAccountingSettingList } from '../../EntityLists/FullAccountingSettingList';
import { GLAccountPMService } from '../../Services/StandardPMs/GLAccountPMService';
import { ExternalReconciliationExtendedPMService } from '../../Services/ExtendedPMs/ExternalReconciliationExtendedPMService';
import { ReconcileExternalPageLinePM } from '../../EntityPMs/ReconcileExternalPageLinePM';



@Component({
    
    templateUrl: './ExtReconcileAdjustBankFeeComponent.html',

})

export class ExtReconcileAdjustBankFeeComponent extends BaseComponent implements OnInit {
    public EntityPM: JournalPM = null;
    public ObjectTableName = "Journal";
    public DataContext = this;
    defaultCurrencyId: string = SessionLocator.TenantPM.CurrencyId;

    creditTotal: number = 0;
    debitTotal: number = 0;
    journalDisabled: boolean = false;
    forceFocus: boolean = false;
    fullAccountingSettingList: FullAccountingSettingList;
    AccountingPeriods: AccountingPeriodList[] = [];
    _AccountingPeriodListService: AccountingPeriodListService = new AccountingPeriodListService();
    _ExternalReconciliationExtendedPMService: ExternalReconciliationExtendedPMService = new ExternalReconciliationExtendedPMService();
    fullAccountingSettingListService: FullAccountingSettingListService = new FullAccountingSettingListService();
    GLAccountsFilterItems: ApiQueryFilters;
    gLAccountPMService: GLAccountPMService = new GLAccountPMService();
    public isRTL: boolean = false;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        super();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.fullAccountingSettingListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var res = myResponse.Result;
                if (res != null && res.length > 0) {
                    var list: FullAccountingSettingList[];
                    list = res;
                    this.fullAccountingSettingList = list[0];
                    if (!AppTool.IsNullOrEmpty(this.fullAccountingSettingList) && !AppTool.IsNullOrEmpty(this.fullAccountingSettingList.DefaultExternalDiffGLAccountId)) {
                        this.GLAccountId = this.fullAccountingSettingList.DefaultExternalDiffGLAccountId;
                        this.gLAccountPMService.get(this.glAcccountId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var res = myResponse.Result;
                                this.GLAccount = res;

                            }
                        });
                    }

                }
            }
        });


        this.GLAccountsFilterItems = new ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("AccountTypeCode", "5,4", null, null, "Exclude", false, false, false, "string", false, true);

        this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, true);

        if (this.AccountingDate == null)
            this.AccountingDate = new Date();

        //this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, true);

    }


    glAcccountId: string;
    get GLAccountId() { return this.glAcccountId; }
    set GLAccountId(value: string) {
        if (this.glAcccountId != value) {
            this.glAcccountId = value;
        }
    }

    glAccount: GLAccountPM;
    get GLAccount() { return this.glAccount; }
    set GLAccount(value: GLAccountPM) {
        if (this.glAccount != value) {
            this.glAccount = value;
        }
        if (!AppTool.IsNullOrEmpty(this.glAccount)) {
            this.UIProperties.SetRequired("GLAccount", this.ObjectTableName, false);
            this.UIProperties.SetRequired("GLAccountId", this.ObjectTableName, false);

        } else {
            this.UIProperties.SetRequired("GLAccount", this.ObjectTableName, true);
            this.UIProperties.SetRequired("GLAccountId", this.ObjectTableName, true);
        }

    }


    txt_Reference: string = TextCodeTranslator.Translate("Accounting.General.O.Reference");
    txt_Amount: string = TextCodeTranslator.Translate("JournalLine.F.LocalAmount");

    ngOnInit() {
        this.CurrentSession.LostFocusEvent.subscribe((res) => {
            if (this.CD) {
                var isDestroyed: boolean = this.CD['destroyed'];
                if (!isDestroyed) {
                    this.CD.detectChanges();
                    //console.log("AfterLostFocus");
                }
            }
        });

        this.GetAccountingPeriods();
        //set focus on accounting date
        var t = setTimeout(() => { this.forceFocus = true; }, 1);
    }

    //#region Properties
    notes: string = "";
    get Notes() { return this.notes; }
    set Notes(value: string) {
        if (this.notes != value) {

            this.notes = value;

        }
    }


    _AccountingDate: Date;
    get AccountingDate() { return this._AccountingDate; }
    set AccountingDate(value: Date) {
        if (this._AccountingDate != value) {


            if (value != null) {

                // Get Accounting Period by year
                var accountingPeriod = this.AccountingPeriods.find(d => d.Year == value.getFullYear());

                if (accountingPeriod) {

                    var month = value.getMonth() + 1;

                    this.ValidationErrorsList = []; // empty errors list
                    // Valid Month => (ClosedMonth < month <= OpenMonth)
                    if (month > accountingPeriod.ClosedMonth && month <= accountingPeriod.OpenMonth) { // valid (open month)

                        this.ValidationErrorsList = []; // empty errors list

                    } else { // invalid (closed month)

                        // push the error to errors list
                        this.ValidationErrorsList.push("Closed Month!");
                        this._AccountingDate = value;
                        return;

                    }
                }

            }

            this._AccountingDate = value;
            if (!AppTool.IsNullOrEmpty(this._AccountingDate)) {
                this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, false);
            } else {
                //this.AccountingDate = null;
                this.UIProperties.SetRequired("AccountingDate", this.ObjectTableName, true);
            }
        }



    }

    accountingPeriod: AccountingPeriodList;
    get AccountingPeriod() { return this.accountingPeriod; }
    set AccountingPeriod(value: AccountingPeriodList) {
        if (this.accountingPeriod != value) {
            this.accountingPeriod = value;
        }
        if (value != null) {

        }
    }


    //#endregion


    DetectChanges() {
        this.CD.detectChanges();
    }



    GetAccountingPeriods() {
        var filters = new ApiQueryFilters(true);
        filters.addAdditionalFilter("PeriodTypeCode", "1", null, null, "Equals", false, false, false, "string"); // 1-Regular

        this._AccountingPeriodListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.AccountingPeriods = myResponse.Result;
                    console.log(">>Accounting Periods: ", myResponse.Result);
                }
            }
        });
    }


    lastDay(year, month) {
        return new Date(year, month + 1, 0).getDate();
    }

    //_SelectedLines: ObservableCollection;//SelectedLines[];
    _SelectedReconcileExternalPageLinePMList: ReconcileExternalPageLinePM[]=[];
    _SelectedLedgerTransactionIdList: [];
    _BankAccountPMId: string;
    TotalDifference:number;
    TotalDifferenceCurrency:  string;
    SetWindowArgs(winArgs) {
        //logitudeWindow.WindowArgs = { "ExtPageSelectedLine": this.ExtPageSelectedLines[0], "LedgerTransactionIdList": LedgerTransactionIdList, "BankAccountPMId": this.BankAccountPM.Id };
        //"ReconcileExternalPageLinePMList": ReconcileExternalPageLinePMList,
        this._BankAccountPMId = winArgs.BankAccountPMId;
        
        this.TotalDifference = winArgs.TotalDifference;
        this.TotalDifferenceCurrency = winArgs.TotalDifferenceCurrency;

        this._SelectedReconcileExternalPageLinePMList = winArgs.ReconcileExternalPageLinePMList;
        if (!AppTool.IsNullOrEmpty(this._SelectedReconcileExternalPageLinePMList)) {
            let firstNote = this._SelectedReconcileExternalPageLinePMList.filter(r => !AppTool.IsNullOrEmpty(r.Notes))[0]
            if (!AppTool.IsNullOrEmpty(firstNote) && !AppTool.IsNullOrEmpty(firstNote.Notes)) {
                this.Notes = firstNote.Notes;
            }

        }

        this.AccountingDate = DateTool.GetDateParts(this._SelectedReconcileExternalPageLinePMList[0].ReferenceDate).DateObject;//ohad  request it 

        this._SelectedLedgerTransactionIdList = winArgs.LedgerTransactionIdList;

        //this._SelectedLines.Collection.forEach(r => {
        //    let myReconcileExternalPageLinePM: ReconcileExternalPageLinePM = r.PageLinePM;
        //    if (AppTool.IsNullOrEmpty(this.Notes)) {
        //        this.Notes = myReconcileExternalPageLinePM.Notes;
        //    }
            
        //});
    }
    FillErrors() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.glAccount)) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList.push("GLAccount is Required");
        }
        else if (AppTool.IsNullOrEmpty(this.AccountingDate)) {

            this.ValidationErrorsList.push("Accounting Date is Required");

        } else {
            this.ValidationErrorsList = [];

        }
    }
    OkButtonClicked() {
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();


        

        //let reconcileExternalPageLineIdList: string[] = [];
        //this._SelectedLines.Collection.forEach(r => {
        //    let myReconcileExternalPageLinePM: ReconcileExternalPageLinePM = r.PageLinePM;
        //    reconcileExternalPageLineIdList.push(myReconcileExternalPageLinePM.Id)

        //});

        //var AdjustAccountId: string = "1-19";

        let SelectedReconcileExternalPageLineIdsList: string[] = [];
        if (!AppTool.IsNullOrEmpty(this._SelectedReconcileExternalPageLinePMList)) {
            this._SelectedReconcileExternalPageLinePMList.forEach(r => SelectedReconcileExternalPageLineIdsList.push(r.Id));
        }
        
        this._ExternalReconciliationExtendedPMService.CreateJournalReconcileAdjustBankFee(
            //this._SelectedReconcileExternalPageLinePMList.Id, //reconcileExternalPageLineIdList,
            SelectedReconcileExternalPageLineIdsList,
            this._SelectedLedgerTransactionIdList,
            this._BankAccountPMId, this.GLAccount.Id, this.AccountingDate.toUTCString(),
            this.Notes)
            .subscribe(
                (res:ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();
                    if (res.HasError) {
                        this.ValidationErrorsList = res.ErrorsArray;

                    } else {
                        var journalPM: JournalPM;
                        journalPM = res.Result;
                        console.log(journalPM);
                        this._NewJournalPM = journalPM;
                    }

                });

    }
    _NewJournalPM: JournalPM;
    OpenJournal() {
        if (!AppTool.IsNullOrEmpty(this._NewJournalPM.Id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this._NewJournalPM.Id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}


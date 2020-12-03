
import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef}  from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {ReconciliationPM} from '../../EntityPMs/ReconciliationPM';
import {JournalPM} from '../../EntityPMs/JournalPM';

import {ReconciliationLinePM} from '../../EntityPMs/ReconciliationLinePM';
import {LedgerTransactionList} from '../../EntityLists/LedgerTransactionList';
import {AutomaticReconcileMethodList} from '../../EntityLists/AutomaticReconcileMethodList';
import {LedgerTransactionPM} from '../../EntityPMs/LedgerTransactionPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {ApiQueryFilters, FilterItem} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {AppTool} from '../../../Infrastructure/Tools';
import {ReconcileEventManager} from '../../Utilities/ReconcileEventManager';
import {ReconciliationExtendedPMService} from '../../Services/ExtendedPMs/ReconciliationExtendedPMService';
import {LedgerTransactionExtendedListService} from '../../Services/ExtendedLists/LedgerTransactionExtendedListService';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {AccountingPeriodListService} from '../../Services/StandardLists/AccountingPeriodListService';
import {AccountingPeriodList} from '../../EntityLists/AccountingPeriodList';
import { FullAccountingSettingListService } from '../../Services/StandardLists/FullAccountingSettingListService';
import { FullAccountingSettingList } from '../../EntityLists/FullAccountingSettingList';
import { GLAccountPMService } from '../../Services/StandardPMs/GLAccountPMService';



@Component({
    
    templateUrl: './JournalReconcileComponent.html',
    
})

export class JournalReconcileComponent extends BaseComponent implements OnInit {
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
    _ReconciliationExtendedPMService: ReconciliationExtendedPMService = new ReconciliationExtendedPMService();
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
                    if (this.fullAccountingSettingList && this.fullAccountingSettingList.DefaultDifferencesGLAccountId) {
                        this.GLAccountId = this.fullAccountingSettingList.DefaultDifferencesGLAccountId;
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
    

    
    public get IsCustomerCare() {
        return SessionLocator.LoggedUserPM.IsCustomerCare;
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
        if (!AppTool.IsNullOrEmpty( this.glAccount)) {
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
    reference1: string="";
    get Reference1() { return this.reference1; }
    set Reference1(value: string) {
        if (this.reference1 != value) {
          
            this.reference1 = value;

        }
    }

    reference2: string="";
    get Reference2() { return this.reference2; }
    set Reference2(value: string) {
        if (this.reference2 != value) {
            
            this.reference2 = value;

        }
    }

    reference3: string="";
    get Reference3() { return this.reference3; }
    set Reference3(value: string) {
        if (this.reference3 != value) {
          
            this.reference3 = value;

        }
    }

    notes: string="";
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

    _SelectedLines: ObservableCollection;//SelectedLines[];
    _GLAccountPMId: string;

    TotalDifference: any;
    SetWindowArgs(winArgs) {
        this._SelectedLines = winArgs.SelectedLines;
        this._GLAccountPMId = winArgs.GLAccountPMId;
        this.TotalDifference = winArgs.TotalDifference;
    }
    FillErrors() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.glAccount)) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList.push("GLAccount is Required");
        }
        else if (AppTool.IsNullOrEmpty( this.AccountingDate )) {

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


        var myReconciliationLines: ReconciliationLinePM[] = [];


        for (var i = 0; i < this._SelectedLines.Length; i++) {
            var selectedTransaction = this._SelectedLines.Collection[i];
            var newLine: any = {};
            newLine.ChangeSetOp = "1";
            newLine.ReconciliationId = "new";
            newLine.Tenant = SessionLocator.Tenant;;
            newLine.Line = i;
            newLine.CurrencyId = selectedTransaction.OpenAmountCurrencyId;
            newLine.TransactionId = selectedTransaction.Id;
            newLine.ReconciliationAmount = selectedTransaction.AmountToReconcile;
            newLine.IsPartial = selectedTransaction.IsPartial;

            //newLine.GroupNumber = selectedTransaction.GroupHash;

            myReconciliationLines.push(newLine);
        }

        //var AdjustAccountId: string = "1-19";

        this._ReconciliationExtendedPMService.CreateJournalReconcile(
            myReconciliationLines,
            this._GLAccountPMId, this.GLAccount.Id, this.AccountingDate.toUTCString(),
            this.reference1, this.reference2, this.reference3, this.Notes)
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


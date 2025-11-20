
import {Component, OnInit, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {JournalPM} from '../../EntityPMs/JournalPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../Infrastructure/Tools';
import {JournalExtendedPMService} from '../../Services/ExtendedPMs/JournalExtendedPMService';
import {AccountingPeriodListService} from '../../Services/StandardLists/AccountingPeriodListService';
import {AccountingPeriodList} from '../../EntityLists/AccountingPeriodList';
import {FullAccountingSettingListService} from '../../Services/StandardLists/FullAccountingSettingListService';
import {FullAccountingSettingList} from '../../EntityLists/FullAccountingSettingList';
import {GLAccountPMService} from '../../Services/StandardPMs/GLAccountPMService';
import { VendorValidator } from 'Common/Validators/VendorValidator';

@Component({

    templateUrl: './JournalRevaluationComponent.html',

})

export class JournalRevaluationComponent extends BaseComponent implements OnInit {
    public EntityPM: JournalPM = null;
    public ObjectTableName = "Journal";
    public DataContext = this;
    defaultCurrencyId: string = SessionLocator.TenantPM.CurrencyId;


    journalDisabled: boolean = false;
    forceFocus: boolean = false;
    fullAccountingSettingList: FullAccountingSettingList;
    AccountingPeriods: AccountingPeriodList[] = [];
    _AccountingPeriodListService: AccountingPeriodListService = new AccountingPeriodListService();
    _JournalExtendedPMService: JournalExtendedPMService = new JournalExtendedPMService();
    fullAccountingSettingListService: FullAccountingSettingListService = new FullAccountingSettingListService();
    GLAccountsFilterItems: ApiQueryFilters;
    gLAccountPMService: GLAccountPMService = new GLAccountPMService();
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];
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
                    if (this.fullAccountingSettingList && this.fullAccountingSettingList.ExchangeRateDiffGLAccountId) {
                        this.RevaluationGLAcccountId = this.fullAccountingSettingList.ExchangeRateDiffGLAccountId;
                        this.gLAccountPMService.get(this.revaluationGLAcccountId).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var res = myResponse.Result;
                                this.RevaluationGLAccount = res;
                                this.RevaluationGLAcccountId = this.RevaluationGLAccount.Id;
                                this.RevaluationsGLAccountName = this.RevaluationGLAccount?.LocalName.trim() ||
                                                                 this.RevaluationGLAccount?.EnglishName || "";  
                            }
                            });
                    }

                }
            }
        });

        this.GLAccountsFilterItems = new ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("IsControlAccount", false, null, null,
            "Equals", false, false, false, "string", false, true);


    }



    public get IsCustomerCare() {
        return SessionLocator.LoggedUserPM.IsCustomerCare;
    }







    revaluationGLAcccountId: string;
    get RevaluationGLAcccountId() { return this.revaluationGLAcccountId; }
    set RevaluationGLAcccountId(value: string) {
        if (this.revaluationGLAcccountId != value) {
            this.revaluationGLAcccountId = value;
        }
    }

    revaluationGLAccount: GLAccountPM;
    get RevaluationGLAccount() { return this.revaluationGLAccount; }
    set RevaluationGLAccount(value: GLAccountPM) {
        if (this.revaluationGLAccount != value) {
            this.revaluationGLAccount = value;
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
                }
            }
        });

        this.AccountName   = this.SourceGLAccountPM?.LocalName.trim() ||
                            this.SourceGLAccountPM?.EnglishName || "";

        this.GetAccountingPeriods();

        const t = setTimeout(() => {
            this.forceFocus = true;
        }, 1);
    }


    //#region Properties

 
    accountName: string="";
    get AccountName() { return this.accountName; }
    set AccountName(value: string) {
        if (this.accountName != value) {

            this.accountName = value;

        }
    }


    revaluationsGLAccountName: string="";
    get RevaluationsGLAccountName() { return this.revaluationsGLAccountName; }
    set RevaluationsGLAccountName(value: string) {
        if (this.revaluationsGLAccountName != value) {

            this.revaluationsGLAccountName = value;

        }
    }







    
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

    dueDate: Date;
    get DueDate() { return this.dueDate; }
    set DueDate(value: Date) {
        if (this.dueDate != value) {

            this.dueDate = value;
        }
    }

    refDate: Date;
    get RefDate() { return this.refDate; }
    set RefDate(value: Date) {
        if (this.refDate != value) {

            this.refDate = value;
        }
    }

    _AccountingDate: Date;
    get AccountingDate() {
        return this._AccountingDate;
    }


    checkClosedMonth(value: Date): boolean {
        if (value != null) {
            const accountingPeriod = this.AccountingPeriods.find(d => d.Year === value.getFullYear());
            if (accountingPeriod) {
                const month = value.getMonth() + 1;

                if (month > accountingPeriod.ClosedMonth && month <= accountingPeriod.OpenMonth) { 
                    return false;
                } else {
                    return true;
                }
            }
        }
    }

    set AccountingDate(value: Date) {
        if (this._AccountingDate != value) {
            if (value != null) {
                var accountingPeriod = this.AccountingPeriods.find(d => d.Year == value.getFullYear());

                if (accountingPeriod) {

                    var month = value.getMonth() + 1;

                    this.ValidationErrorsList = []; 
                    if (month > accountingPeriod.ClosedMonth && month <= accountingPeriod.OpenMonth) { 
                        this.ValidationErrorsList = []; 

                    } else { 
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
        const filters = new ApiQueryFilters(true);
        filters.addAdditionalFilter('PeriodTypeCode', '1', null, null,
            'Equals', false, false, false, 'string');

        this._AccountingPeriodListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.AccountingPeriods = myResponse.Result;
                }
            }
        });
    }



    lastDay(year, month) {
        return new Date(year, month + 1, 0).getDate();
    }

    SourceGLAccountPM: GLAccountPM;



    TotalLocalDifference: number = null;

    SetWindowArgs(winArgs) {
        this.SourceGLAccountPM = winArgs.SourceGLAccountPM;
        this.TotalLocalDifference = winArgs.TotalLocalDifference;

    }
    async FillErrors() {
        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.reference1)) {
            this.ValidationErrorsList.push('Reference 1 is Required');
        }

    }



    async OkButtonClicked() {

        await this.FillErrors();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        else {
            this.CurrentSession.CurrentWindow.Close(this.Reference1);
        }

    }



 

    _NewJournalPM: JournalPM;

    OpenJournal(id: string) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    getScreenHeight() { return self.innerHeight; }
    dropdownDisplay: string = 'none';
    DropdowndisplayToggle() {
        var item = document.getElementById("adjustbutton");
        var itemRect = item.getBoundingClientRect();

        let DDLHeight =22.5;
        let Extra = 22 + 1 + 1; 
        if (itemRect.bottom + DDLHeight < this.getScreenHeight()) {
            document.getElementById("dropdowmenu").style.top = (itemRect.bottom - DDLHeight - Extra) + 'px';
        }

        if (this.dropdownDisplay == 'none') {
            this.dropdownDisplay = 'block';
        }
        else {
            this.dropdownDisplay = 'none';
        }
    }
}


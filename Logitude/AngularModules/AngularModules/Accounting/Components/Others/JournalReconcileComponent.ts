
import {Component, OnInit, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {JournalPM} from '../../EntityPMs/JournalPM';
import {ReconciliationLinePM} from '../../EntityPMs/ReconciliationLinePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters, FilterItem} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {AppTool} from '../../../Infrastructure/Tools';
import {ReconciliationExtendedPMService} from '../../Services/ExtendedPMs/ReconciliationExtendedPMService';
import {AccountingPeriodListService} from '../../Services/StandardLists/AccountingPeriodListService';
import {AccountingPeriodList} from '../../EntityLists/AccountingPeriodList';
import {FullAccountingSettingListService} from '../../Services/StandardLists/FullAccountingSettingListService';
import {FullAccountingSettingList} from '../../EntityLists/FullAccountingSettingList';
import {GLAccountPMService} from '../../Services/StandardPMs/GLAccountPMService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';

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
        this.GLAccountsFilterItems.addAdditionalFilter("AccountTypeCode", "5,4", null, null,
            "Exclude", false, false, false, "string", false, true);
        this.GLAccountsFilterItems.addAdditionalFilter("IsControlAccount", false, null, null,
            "Equals", false, false, false, "string", false, true);


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
                }
            }
        });

        this.GetAccountingPeriods();

        const t = setTimeout(() => {
            this.forceFocus = true;
        }, 1);
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

    checkSelectedLinesClosedMonth()
    {
        let errorMessage = '';

        for (let i = 0; i < this._SelectedLines.Length; i++) {
            const selectedTransaction = this._SelectedLines.Collection[i];
            if (this.checkClosedMonth(new Date(selectedTransaction.AccountingDate))) {

                let lineErrorMessage = TextCodeTranslator.Translate('Journal.RE.ReconcilePeriodClosed');

                let lineDetails =
                    new Date(selectedTransaction.AccountingDate).toDateString() + ', ' +
                    new Date(selectedTransaction.DocumentDate).toDateString() + ', ' +
                    new Date(selectedTransaction.DueDate).toDateString() + ', ' +
                    selectedTransaction.Source + ', ' +
                    selectedTransaction.OriginalAmount + ', ' +
                    selectedTransaction.OpenAmount + ', ';


                if (selectedTransaction.Reference1 != null) {
                    lineDetails += selectedTransaction.Reference1;
                }
                if (selectedTransaction.Reference2 != null) {
                    lineDetails += selectedTransaction.Reference2;
                }
                if (selectedTransaction.Reference3 != null) {
                    lineDetails += selectedTransaction.Reference3;
                }

                lineErrorMessage = lineErrorMessage.replace('(X)' , lineDetails);

                errorMessage += lineErrorMessage + '\n';
            }

        }

        if (errorMessage !== '') {
            const messageWindow = new MessageWindow();
            messageWindow.Width = 600;
            messageWindow.Height = 300;
            messageWindow.IsMessageMultiLine = true;
            messageWindow.RTL = (ObjectsLocator.GlobalSetting.LayoutDirection === 'rtl');
            messageWindow.Show(errorMessage);
        }
    }

    checkClosedMonth(value: Date): boolean {
        if (value != null) {
            const accountingPeriod = this.AccountingPeriods.find(d => d.Year === value.getFullYear());
            if (accountingPeriod) {
                const month = value.getMonth() + 1;

                if (month > accountingPeriod.ClosedMonth && month <= accountingPeriod.OpenMonth) { // valid (open month)
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
        const filters = new ApiQueryFilters(true);
        filters.addAdditionalFilter('PeriodTypeCode', '1', null, null,
            'Equals', false, false, false, 'string'); // 1-Regular

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

    _SelectedLines: ObservableCollection;//SelectedLines[];
    _GLAccountPMId: string;

    TotalDifference: any;
    TotalCredit: any;
    TotalDebit: any;
    SetWindowArgs(winArgs) {
        this._SelectedLines = winArgs.SelectedLines;
        this._GLAccountPMId = winArgs.GLAccountPMId;
        this.TotalDifference = winArgs.TotalDifference;
        this.TotalCredit = winArgs.TotalCredit;
        this.TotalDebit = winArgs.TotalDebit;
    }
    FillErrors(isSplitJournal: boolean) {

        if (AppTool.IsNullOrEmpty(this.glAccount)) {
            this.ValidationErrorsList.push('GLAccount is Required');
        }
        else if (AppTool.IsNullOrEmpty(this.AccountingDate) && !isSplitJournal) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate('Journal.RE.AccountingDateRequired'));
        }
    }

    BuildReconciliationLines(myReconciliationLines: ReconciliationLinePM[]): void {
        for (let i = 0; i < this._SelectedLines.Length; i++) {
            const selectedTransaction = this._SelectedLines.Collection[i];
            const newLine: any = {};
            newLine.ChangeSetOp = '1';
            newLine.ReconciliationId = 'new';
            newLine.Tenant = SessionLocator.Tenant;
            newLine.Line = i;
            newLine.CurrencyId = selectedTransaction.OpenAmountCurrencyId;
            newLine.TransactionId = selectedTransaction.Id;
            newLine.ReconciliationAmount = selectedTransaction.AmountToReconcile;
            newLine.IsPartial = selectedTransaction.IsPartial;
            newLine.Reference1 = selectedTransaction.Reference1;
            newLine.Reference2 = selectedTransaction.Reference2;
            newLine.Reference3 = selectedTransaction.Reference3;
            newLine.Notes = selectedTransaction.Notes;
            newLine.JournalNumber = selectedTransaction.JournalNumber;
            myReconciliationLines.push(newLine);
        }
    }

    OkButtonClicked(isSplitJournal: boolean) {

        this.FillErrors(isSplitJournal);

        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        const reconciliationLines: ReconciliationLinePM[] = [];
        this.BuildReconciliationLines(reconciliationLines);

        if (isSplitJournal) {
            const confirmMsg = TextCodeTranslator.Translate('Journal.RE.AccountingDateConfrimation');
            const confirmWindow = new ConfirmWindow();
            confirmWindow.Width = 400;
            confirmWindow.Left = '25%';
            confirmWindow.YesButtonText = TextCodeTranslator.Translate('Customs.General.B.OK');
            confirmWindow.NoButtonText = TextCodeTranslator.Translate('Customs.General.B.Cancel');
            confirmWindow.Show(confirmMsg);
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.ReconcileSplit(reconciliationLines);
                    confirmWindow.Close();
                }
            });

        } else {
            this.Reconcile(reconciliationLines);
        }
    }

    ReconcileSplit(myReconciliationLines: ReconciliationLinePM[]) {
        this.CurrentSession.StartBusyIndicatorCreating();

        this._ReconciliationExtendedPMService.CreateSplitJournalReconcile(
            myReconciliationLines,
            this._GLAccountPMId, this.GLAccount.Id,
            this.AccountingDate ? this.AccountingDate.toUTCString() : null,
            this.DueDate ? this.DueDate.toUTCString() : null,
            this.RefDate ? this.RefDate.toUTCString() : null,
            this.reference1, this.reference2, this.reference3, this.Notes)
            .subscribe(
                (res: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();
                    if(!this.AccountingDate)
                        this.checkSelectedLinesClosedMonth();

                    if (res.HasError) {
                        this.ValidationErrorsList = res.ErrorsArray;

                    } else {
                        let journalsArray: JournalPM[];
                        journalsArray = res.Result;
                        this._NewJournals = journalsArray;
                    }

                });
    }

    Reconcile(myReconciliationLines: ReconciliationLinePM[]) {
        this.CurrentSession.StartBusyIndicatorCreating();

        this._ReconciliationExtendedPMService.CreateJournalReconcile(
            myReconciliationLines,
            this._GLAccountPMId, this.GLAccount.Id, this.AccountingDate.toUTCString(),
            this.DueDate ? this.DueDate.toUTCString() : null, this.RefDate ? this.RefDate.toUTCString() : null,
            this.reference1, this.reference2, this.reference3, this.Notes)
            .subscribe(
                (res: ServiceResponse) => {

                    this.CurrentSession.StopBusyIndicator();


                    if (res.HasError) {
                        this.ValidationErrorsList = res.ErrorsArray;

                    } else {
                        let journalPM: JournalPM;
                        journalPM = res.Result;
                        this._NewJournalPM = journalPM;
                    }

                });
    }

    _NewJournalPM: JournalPM;
    _NewJournals: JournalPM[];
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

        let DDLHeight =22.5;//    height: 22px; * 3 +30
        let Extra = 22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN
        if (itemRect.bottom + DDLHeight < this.getScreenHeight()) {//this.PaintTop = true
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


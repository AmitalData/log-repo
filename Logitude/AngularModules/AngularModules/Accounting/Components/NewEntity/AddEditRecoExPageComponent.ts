import {Component, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ReconcileExternalPagePM} from '../../EntityPMs/ReconcileExternalPagePM';
import {ReconcileExternalPageLinePM} from '../../EntityPMs/ReconcileExternalPageLinePM';
import {BankAccountPM} from '../../EntityPMs/BankAccountPM';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ReconcileExternalPagePMService} from '../../Services/StandardPMs/ReconcileExternalPagePMService';
import {CurrencyPMService} from '../../../Common/Services/StandardPMs/CurrencyPMService';
import {CurrencyListService} from '../../../Common/Services/StandardLists/CurrencyListService';
import {ReconcileExternalPageExtendedPMService} from '../../Services/ExtendedPMs/ReconcileExternalPageExtendedPMService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    selector: 'AddEditRecoExPageComponent',
    moduleId: module.id,
    providers: [EntityListService],
    templateUrl: './AddEditRecoExPageComponent.html',
})

export class AddEditRecoExPageComponent extends BaseComponent{
    public ReconcileExternalPagePM: ReconcileExternalPagePM;
    public BankAccountPM: BankAccountPM;
    public PrevBankPagePM: ReconcileExternalPagePM;
    public DataContext: AddEditRecoExPageComponent = this;
    public ObjectTableName: string = "ReconcileExternalPage";
    public ValidationErrorsList: string[] = [];
    isNewEntity: boolean = false;
    IsCancelApprovedEnabled: boolean = false;
    public IsDisplayOnly: boolean = false;
    public IsMultiCurrency: boolean = false;
    currency: any;
    AMOUNT_TEXT = TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Amount");
    AmountColHeader: string;
    PageLinesList: ObservableCollection;
    public isRTL: boolean = false;
    public TotalSum: number = 0.0;
    public Difference: number = 0.0;

    _entityResourceService: EntityResourceService = new EntityResourceService();
    _ReconcileExternalPagePMService: ReconcileExternalPagePMService = new ReconcileExternalPagePMService();
    _ReconcileExternalPageExtendedPMService: ReconcileExternalPageExtendedPMService = new ReconcileExternalPageExtendedPMService();
    _CurrencyPMService: CurrencyPMService = new CurrencyPMService();
    currencyListService: CurrencyListService = new CurrencyListService();

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef, public entityListService: EntityListService) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.PageLinesList = new ObservableCollection([]);

        this.SetUIProperties();
    }

    SetWindowArgs(args) {
        if (args != null) {
            var prevPageNo;
            this.BankAccountPM = args.BankAccount;
            this.GetDefaultValues();
            if (args.entity)
            {
                // EDIT Mode
                this.ReconcileExternalPagePM = args.entity;
                this.isNewEntity = false;
                this.GetPrevPage(this.ReconcileExternalPagePM.PageNo);
                if (this.ReconcileExternalPagePM.StatusCode == "2" || this.ReconcileExternalPagePM.StatusCode == "3")
                    this.IsDisplayOnly = true;
                this.SetUIProperties();

            }
            else
            {
                // NEW Entity

                //get prev page
                prevPageNo = this.BankAccountPM.LastPageNumber;
                this.GetPage(prevPageNo);

                this.isNewEntity = true;
                var newEntity = new ReconcileExternalPagePM();
                newEntity.Tenant = SessionLocator.Tenant;
                //newEntity.CreateDate = new Date();
                //if (SessionLocator.LoggedUserPM)
                //{
                //    newEntity.CreatedByUserId = SessionLocator.LoggedUserPM.Id;
                //    newEntity.CreatedByUserName = SessionLocator.LoggedUserPM.LocalName;
                //}

                if (args.BankAccount.LastPageEndDate)
                {
                    var lastDate: Date = new Date(args.BankAccount.LastPageEndDate);
                    var lastDatePlusOne = new Date(lastDate.setDate(lastDate.getDate() + 1));
                    var date = DateTool.GetDate(lastDatePlusOne.getFullYear(), lastDatePlusOne.getMonth(), lastDatePlusOne.getDate(), 0, 0, 0);
                    newEntity.FromDate = date;
                }
                newEntity.StatusCode = "1"; // 1- Draft
                newEntity.PageNo = 0;
                newEntity.BankAccountId = args.BankAccountId;
                newEntity.GLAccountId = args.GLAccountId;
                newEntity.EntryTypeCode = "1"; // 1- Manual

                this.ReconcileExternalPagePM = newEntity;
            }

            if (this.isNewEntity) {
                this.IsCancelApprovedEnabled = false;
            } else if (this.ReconcileExternalPagePM.StatusCode == "3") { // 3- Cancelled
                this.IsCancelApprovedEnabled = false;
            }
            else if (this.ReconcileExternalPagePM.StatusCode == "2" || this.ReconcileExternalPagePM.StatusCode == "1" ) { // 1- Draft, 2- Approved
                this.IsCancelApprovedEnabled = true;
            }

            this.CalculateTotals();
            this.FillGridsData();





        }
    }

    //#region Properties
    get FromDate() { return this.ReconcileExternalPagePM.FromDate; }
    set FromDate(value: Date) {
        if (this.ReconcileExternalPagePM.FromDate != value) {
            this.ReconcileExternalPagePM.FromDate = value;
            var todayDate = DateTool.GetCurrentDateTimeAsUtc();
            if (value > todayDate) {
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
            } else {
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, "ok");

            }
        }
    }


    get ToDate() { return this.ReconcileExternalPagePM.ToDate; }
    set ToDate(value: Date) {
        if (this.ReconcileExternalPagePM.ToDate != value) {
            this.ReconcileExternalPagePM.ToDate = value;
            var todayDate = DateTool.GetCurrentDateTimeAsUtc();
            if (value > todayDate) {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
            } else {
                this.UIProperties.SetValidity("ToDate", this.ObjectTableName, true, "ok");

            }
        }
    }


    get StartBalance() { return this.ReconcileExternalPagePM.StartBalance; }
    set StartBalance(value: number) {
        if (this.ReconcileExternalPagePM.StartBalance != value) {
            this.ReconcileExternalPagePM.StartBalance = value;
            this.CalculateTotals();
        }
    }


    get CloseBalance() { return this.ReconcileExternalPagePM.CloseBalance; }
    set CloseBalance(value: number) {
        if (this.ReconcileExternalPagePM.CloseBalance != value) {
            this.ReconcileExternalPagePM.CloseBalance = value;
            this.CalculateTotals();
        }
    }

    get StatusCode() { return this.ReconcileExternalPagePM.StatusCode; }
    set StatusCode(value: string) {
        if (this.ReconcileExternalPagePM.StatusCode != value) {
            this.ReconcileExternalPagePM.StatusCode = value;
        }
    }


    tenantCurrency: any;
    get TenantCurrency() { return this.tenantCurrency }
    set TenantCurrency(value: any) {
        if (this.tenantCurrency != value) {
            this.tenantCurrency = value;

        }
    }
    //#endregion

    //#region Buttons Handlers
    SaveAsDraftButtonClicked() {
        this.ReconcileExternalPagePM.StatusCode = '1' // 1- Draft
        this.SaveEntity();
    }
    isApprovedButtonClicked: boolean = false;
    ApproveButtonClicked() {

        if (this.PageLinesList.Length == 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.O.Nolineswereenteredonbank"));
            return;
        }

        this.isApprovedButtonClicked = true;
        this.ReconcileExternalPagePM.StatusCode = '2' // 2- Approved
        this.SaveEntity();
    }
    CancelApprovalButtonClicked() {
        var errors: string[] = [];

        if (this.ReconcileExternalPagePM.StatusCode == "2") {  // 2- Approved

            // * Check last approved page
            var isLastApprovedPage = this.BankAccountPM.LastPageNumber == this.ReconcileExternalPagePM.PageNo+"";
            if (isLastApprovedPage)
            {
                // continue...
            }
            else
            {
                errors.push(TextCodeTranslator.Translate("BankAccounts.O.CantCancelItsNotLastApproved"));
                this.ValidationErrorsList = errors;
                return;
            }


            // * Check reconciled page lines
            var hasReconciledLines: boolean = false;
            var lines = this.PageLinesList.Collection;
            if (lines.length > 0) {
                lines.forEach((line) => {
                    if (line.IsReconciled) {
                        hasReconciledLines = true;
                    }
                });
            }
            if (hasReconciledLines)
            {
                errors.push(TextCodeTranslator.Translate("BankAccounts.O.CanCancelItsTransactionsReconciled"));
                this.ValidationErrorsList = errors;
                return;
            }
        }

        this.ReconcileExternalPagePM.StatusCode = "3"; // 3-Canceled
        this.SaveEntity();
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    //* grid handlers in seperate region

    //#endregion

    SaveEntity() {
        var errors: string[] = [];
        var todayDate = DateTool.GetCurrentDateTimeAsUtc();
        if (this.ToDate > todayDate || this.FromDate > todayDate) {
            errors.push(TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
        }

        if (this.isNewEntity) {
            this.ReconcileExternalPagePM.CreateDate = new Date();
            if (SessionLocator.LoggedUserPM) {
                this.ReconcileExternalPagePM.CreatedByUserId = SessionLocator.LoggedUserPM.Id;
                this.ReconcileExternalPagePM.CreatedByUserName = SessionLocator.LoggedUserPM.LocalName;
            }
        }

        // Class Validator
        Validator.TryValidateObject(this.ReconcileExternalPagePM, this.ObjectTableName, errors);

        // validate empty lines
        //if (this.ReconcileExternalPagePM.StatusCode != "1") {
            var lines = this.ReconcileExternalPagePM.ReconcileExternalPageLines;
            if (lines.length > 0) {
                lines.forEach((line) => {
                    if (!line.ReferenceDate) {
                        var error = "";
                        error = TextCodeTranslator.Translate("Accounting.General.O.Line");
                        error += (line.LineNumber + ": ");
                        error += TextCodeTranslator.Translate("Accounting.O.ReferenceDateIsRequired");
                        errors.push(error);
                    }
                    if (AppTool.IsNullOrEmpty(line.Amount)) {
                        var error = "";
                        error = TextCodeTranslator.Translate("Accounting.General.O.Line");
                        error += (line.LineNumber + ": ");
                        error += TextCodeTranslator.Translate("Accounting.O.AmountIsRequired");
                        errors.push(error);
                    }
                });
            }
        //}

        //// Custom Validation
        //if (DateTool.IsDateSmaller(this.FromDate,this.PrevBankPagePM.ToDate)) {
        ////if (this.FromDate < this.PrevBankPagePM.ToDate) {
        //    errors.push(TextCodeTranslator.Translate("ReconcileExternalPage.O.FromDateShouldBiggerPrevToDate"));
        //}
        //if (DateTool.IsDateSmaller(this.ToDate,this.FromDate)) {
        ////if (this.ToDate < this.FromDate) {
        //    errors.push(TextCodeTranslator.Translate("ReconcileExternalPage.O.ToDateShouldBiggerFromDate"));
        //}
        //if (Number(this.StartBalance) != Number(this.PrevBankPagePM.CloseBalance)) {
        //    errors.push(TextCodeTranslator.Translate("ReconcileExternalPage.O.StartBalanceShouldEqualCloseBalance"));
        //}

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        } else {
            if (this.isApprovedButtonClicked) {
                this.isApprovedButtonClicked = false;
                this.ReconcileExternalPagePM.StatusCode = '1' // 1- Draft
            }
        }
    }

    SubmitChanges() {
        this.CurrentSession.StartBusyIndicatorSaving();
        if (this.isNewEntity) {
            this._ReconcileExternalPagePMService.insert(this.ReconcileExternalPagePM).subscribe(myResult => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }

                else {

                    if (this.isApprovedButtonClicked) {
                        this.isApprovedButtonClicked = false;
                        this.ReconcileExternalPagePM.StatusCode = '1' // 1- Draft
                    }
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        } else {

            this._ReconcileExternalPagePMService.update(this.ReconcileExternalPagePM).subscribe(myResult => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }

                else {

                    if (this.isApprovedButtonClicked) {
                        this.isApprovedButtonClicked = false;
                        this.ReconcileExternalPagePM.StatusCode = '1' // 1- Draft
                    }

                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });

        }
    }

    GetCurrency() {
        if (this.BankAccountPM.GLAccountCurrencyId && this.BankAccountPM.GLAccountCurrencyId == "multi")
        {
            this.IsMultiCurrency = true;
            this.AmountColHeader = this.AMOUNT_TEXT + " (" + this.TenantCurrency.Code + ")";
        }
        else if (this.BankAccountPM.GLAccountCurrencyId)
        {
            this._CurrencyPMService.get(this.BankAccountPM.GLAccountCurrencyId).subscribe((myResult) =>
            {
                var currency = myResult.Result;
                if (!AppTool.IsNullOrEmpty(currency))
                {
                    this.currency = currency;
                    this.AmountColHeader = this.AMOUNT_TEXT + " (" + this.currency.Code +")"
                }
                else {
                    console.log("[!] Cannot find the currency !!");
                    this.AmountColHeader = this.AMOUNT_TEXT;
                }
            });
        }
    }

    //#region Prev Bank Page
    OpenPrevPage() {
        console.log(this.PrevBankPagePM);
        this.OpenBankPageWindow(this.PrevBankPagePM);
    }
    OpenBankPageWindow(entity: any = null) {

        var windowTitle = entity ? (TextCodeTranslator.Translate("ReconcileExternalPage.F.PageNo") + " " + entity.PageNo) : TextCodeTranslator.Translate("Accounting.General.O.NewPage");

        var windowArgs: any = {};
        windowArgs.entity = entity;
        windowArgs.BankAccountId = this.BankAccountPM.Id;
        windowArgs.GLAccountId = this.BankAccountPM.GLAccountId;
        windowArgs.BankAccount = this.BankAccountPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) =>
        {

        });
        logWindow.Show('./Accounting/Components/NewEntity/AddEditRecoExPageComponent');

    }
    GetPage(pageNo: string) {

        if (pageNo) {
            //get last page
            this._ReconcileExternalPageExtendedPMService.GetBankPageByPageNo(pageNo, this.BankAccountPM.Id).subscribe((myResult) => {
                var bankPage = myResult.Result;
                if (!AppTool.IsNullOrEmpty(bankPage)) {
                    this.PrevBankPagePM = bankPage;
                }
                else {
                    console.log("[!] Cannot find the Prev Bank Page !!");
                }
            });
        }
        else {
            console.log("[!!!] No pageNo provided");
        }
    }

    GetPrevPage(prevPageNo: number) {

        if (prevPageNo) {
            //get last page
            this._ReconcileExternalPageExtendedPMService.GetPrevPageByPageNo(prevPageNo, this.BankAccountPM.Id).subscribe((myResult) => {
                var bankPage = myResult.Result;
                if (!AppTool.IsNullOrEmpty(bankPage)) {
                    this.PrevBankPagePM = bankPage;
                }
                else {
                    console.log("[!] Cannot find the Prev Bank Page !!");
                }
            });
        }
        else {
            console.log("[!!!] No prevPageNo provided");
        }
    }
    //#endregion

    SetUIProperties() {
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StartBalance", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CloseBalance", this.ObjectTableName, !this.IsDisplayOnly);
        //this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
    }

    //#region Lines Grid

    FillGridsData() {

        this.PageLinesList = new ObservableCollection([]);
        if (!AppTool.IsNullOrEmpty(this.ReconcileExternalPagePM)) {
            for (let item of this.ReconcileExternalPagePM.ReconcileExternalPageLines.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 })) {
                this.PageLinesList.Insert(new PageLineModel(item, this));
            }
            this.CalculateTotals();

        }
    }

    AddButtonClicked() {
        if (this.IsDisplayOnly) return;

        var line = 0;

        if (this.ReconcileExternalPagePM.ReconcileExternalPageLines.length > 0) {
            var line = this.ReconcileExternalPagePM.ReconcileExternalPageLines.reduce(function (prev, current) { return (prev.LineNumber > current.LineNumber) ? prev : current }).LineNumber;
        }

        line++; // last no.

        var pageLine: ReconcileExternalPageLinePM = new ReconcileExternalPageLinePM(this.ReconcileExternalPagePM);
        //pageLine.Id = "new";
        //pageLine.ChangeSetOp = "insert";
        pageLine.Tenant = SessionLocator.Tenant;
        pageLine.ReconcileExternalPageId = this.isNewEntity ? "new" : this.ReconcileExternalPagePM.Id;
        pageLine.LineNumber = line;
        pageLine.IsReconciled = false;

        this.ReconcileExternalPagePM.AddReconcileExternalPageLine(pageLine);

        var item = new PageLineModel(pageLine, this);
        this.PageLinesList.Insert(item);

    }

    RemoveLineClicked(item) {
        if (item) {
            this.ReconcileExternalPagePM.RemoveReconcileExternalPageLine(item.pageLinePM);
            this.PageLinesList.Remove(item);
        }

        //resequence consignments
        for (var i = 0; i < this.ReconcileExternalPagePM.ReconcileExternalPageLines.length; i++) {
            var line = this.ReconcileExternalPagePM.ReconcileExternalPageLines[i];
            line.LineNumber = i + 1;
        }

        this.CalculateTotals();
    }

    OnRowEnded($event) {
        if (this.StatusCode == "2") return;
        if (($event) == this.PageLinesList.Length) {
            this.AddButtonClicked();
        }
    }

    OnFocus() {
        if (this.StatusCode == "2") return;
        if (this.PageLinesList.Length == 0) {
            this.AddButtonClicked();
        }
    }

    //#endregion

    GetDefaultValues() {
        // Tenant currency
        var defaultCurrencyId: string = SessionLocator.TenantPM.CurrencyId;
        this.currencyListService.getSingle(defaultCurrencyId).subscribe((myResponse: ServiceResponse) => {

            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.TenantCurrency = myResponse.Result;
                    console.log(">>Tenant Currency: ", myResponse.Result);
                    this.GetCurrency();

                } else
                    this.GetCurrency();

            } else
                this.GetCurrency();

        });
    }

    CalculateTotals() {
        var sum = 0.0;

        if (this.PageLinesList.Length > 0) {
            for (var line of this.PageLinesList.Collection) {
                sum += (line.Amount == null || line.Amount == undefined) ? 0 : line.Amount;
            }
        }

        this.TotalSum = sum;

        if (this.StartBalance) {
            var GrandTotal = this.TotalSum + +this.StartBalance;
            this.Difference = this.CloseBalance - GrandTotal;
        }
    }


}


export class PageLineModel extends BaseComponent {
    public ObjectTableName = "ReconcileExternalPageLine";
    public DataContext = this;
    constructor(public pageLinePM: ReconcileExternalPageLinePM, public parent: AddEditRecoExPageComponent) {
        super();
    }

    //#region Properties

    get LineNumber() { return this.pageLinePM.LineNumber; }
    set LineNumber(value: number) {
        if (this.pageLinePM.LineNumber != value) {
            this.pageLinePM.LineNumber = value;
        }
    }

    get ReferenceDate() { return this.pageLinePM.ReferenceDate; }
    set ReferenceDate(value: Date) {
        if (this.pageLinePM.ReferenceDate != value) {
            this.pageLinePM.ReferenceDate = value;
        }
    }

    get Amount() { return this.pageLinePM.Amount; }
    set Amount(value: number) {
        if (this.pageLinePM.Amount != value) {
            this.pageLinePM.Amount = value;
            this.parent.CalculateTotals();
        }
    }

    get Reference() { return this.pageLinePM.Reference; }
    set Reference(value: string) {
        if (this.pageLinePM.Reference != value) {
            this.pageLinePM.Reference = value;
        }
    }

    get IsReconciled() { return this.pageLinePM.IsReconciled; }
    set IsReconciled(value: boolean) {
        if (this.pageLinePM.IsReconciled != value) {
            this.pageLinePM.IsReconciled = value;
        }
    }

    get ReconciliationNumber() { return this.pageLinePM.ReconciliationNumber; }
    set ReconciliationNumber(value: string) {
        if (this.pageLinePM.ReconciliationNumber != value) {
            this.pageLinePM.ReconciliationNumber = value;
        }
    }

    get Notes() { return this.pageLinePM.Notes; }
    set Notes(value: string) {
        if (this.pageLinePM.Notes != value) {
            this.pageLinePM.Notes = value;
        }
    }

    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
}

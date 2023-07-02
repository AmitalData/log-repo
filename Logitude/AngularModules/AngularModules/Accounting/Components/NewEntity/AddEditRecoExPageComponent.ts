declare var window: any;
import { Component, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { ReconcileExternalPagePM } from '../../EntityPMs/ReconcileExternalPagePM';
import { ReconcileExternalPageLinePM } from '../../EntityPMs/ReconcileExternalPageLinePM';
import { BankAccountPM } from '../../EntityPMs/BankAccountPM';
import { GLAccountPM } from '../../EntityPMs/GLAccountPM';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ReconcileExternalPagePMService } from '../../Services/StandardPMs/ReconcileExternalPagePMService';
import { CurrencyPMService } from '../../../Common/Services/StandardPMs/CurrencyPMService';
import { CurrencyListService } from '../../../Common/Services/StandardLists/CurrencyListService';
import { ReconcileExternalPageExtendedPMService } from '../../Services/ExtendedPMs/ReconcileExternalPageExtendedPMService';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ExternalPageAdditionalDataPMService } from '../../Services/StandardPMs/ExternalPageAdditionalDataPMService';
import { ExternalPageAdditionalDataPM } from '../../EntityPMs/ExternalPageAdditionalDataPM';
import { reject } from 'q';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { ImageParameter } from 'Infrastructure/DataContracts/ImageParameter';
import { Guid } from 'Infrastructure/Utilities/Guid';
import * as moment from 'moment';
import { PartnersUploadExcelParameter } from 'Common/Services/CommonDomainService';
import { DocumentsFilingExtendedPMService } from 'Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
declare var attachmentUploader, ResultAsArray, resultToUnitArray: any;
@Component({
    selector: 'AddEditRecoExPageComponent',

    providers: [EntityListService],
    templateUrl: './AddEditRecoExPageComponent.html',
})

export class AddEditRecoExPageComponent extends BaseComponent {
    public PageObjectTableName: string;
    public ReconcileExternalPagePM: ReconcileExternalPagePM;
    public EntityPM: any;
    public PreviousPagePM: ReconcileExternalPagePM;
    public DataContext: AddEditRecoExPageComponent = this;
    public ExternalPageTable: string = "ReconcileExternalPage";

    AMOUNT_TEXT = TextCodeTranslator.Translate("ReconcileExternalPageLine.F.Amount");
    CreditAMOUNT_TEXT = TextCodeTranslator.Translate("ReconcileExternalPageLine.F.CreditAmount");
    DebitAMOUNT_TEXT = TextCodeTranslator.Translate("ReconcileExternalPageLine.F.DebitAmount");
    _entityResourceService: EntityResourceService = new EntityResourceService();
    _ReconcileExternalPagePMService: ReconcileExternalPagePMService = new ReconcileExternalPagePMService();
    _ReconcileExternalPageExtendedPMService: ReconcileExternalPageExtendedPMService = new ReconcileExternalPageExtendedPMService();
    _ExternalPageAdditionalDataPMService: ExternalPageAdditionalDataPMService = new ExternalPageAdditionalDataPMService();
    _CurrencyPMService: CurrencyPMService = new CurrencyPMService();
    currencyListService: CurrencyListService = new CurrencyListService();

    public ValidationErrorsList: string[] = [];
    isNewEntity: boolean = false;
    IsCancelApprovedEnabled: boolean = false;
    IsRestoreButtonVisibile: boolean = false;
    public IsDisplayOnly: boolean = false;
    public IsMultiCurrency: boolean = false;
    RestoreToolTipMessage: string;
    currency: any;
    AmountColHeader: string;
    CreditAmountColHeader: string;
    DebitAmountColHeader: string;
    PageLinesList: ObservableCollection;
    public isRTL: boolean = false;
    public TotalSum: number = 0.0;
    public Difference: number = 0.0;
    private CurrentSession = SessionLocator.SelectedSession;
    IsEditButtonDisabled: boolean = false;
    EditWindowToolTip: string = null;
    IsRestoreButtonEnabled: boolean;
    UploadButtonIsEnabled: boolean = true;




    constructor(private CD: ChangeDetectorRef, public entityListService: EntityListService) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.PageLinesList = new ObservableCollection([]);

        this.SetUIProperties();
    }




    SetWindowArgs(args) {
        if (args != null) {
            this.PageObjectTableName = args.PageObjectTableName;
            this.EntityPM = args.EntityPM;
            this.IsRestoreButtonEnabled = args.IsRestoreButtonEnabled;
            this.IsEditButtonDisabled = !args.EnableReconcileEditButton;
            this.IsRestoreButtonVisibile = args.IsRestoreButtonVisibile;
            this.EditWindowToolTip = args.message;
            this.RestoreToolTipMessage = args.RestoreToolTipMessage;
            this.additionalDataPM = args.AdditionalDataPM;


            this.GetDefaultValues();

            if (args.externalPage)
                this.SetEditMode(args);
            else
                this.SetNewEntityMode(args);

            this.SetCancelApprovalEditablilty();
            this.CalculateTotals();
            this.FillGridsData();


        }
    }


    additionalDataPM: ExternalPageAdditionalDataPM;
    GetAdditionalData(objectTableId: string, entityId: string) {
        return new Promise(resolve => {
            this._ExternalPageAdditionalDataPMService.get(objectTableId, entityId)
                .subscribe((response: any) => {
                    console.log("[GetAdditionalData]", response);

                    if (!response.HasError) {
                        resolve(this.additionalDataPM);
                    }
                    else {
                        console.error(response.ErrorsArray.toString());
                        reject();

                    }
                });
        });
    }

    private SetCancelApprovalEditablilty() {
        if (this.isNewEntity) {
            this.IsCancelApprovedEnabled = false;
        }
        else if (this.ReconcileExternalPagePM.StatusCode == "3") { // 3- Cancelled
            this.IsCancelApprovedEnabled = false;
        }
        else if (this.ReconcileExternalPagePM.StatusCode == "2" || this.ReconcileExternalPagePM.StatusCode == "1") { // 1- Draft, 2- Approved
            this.IsCancelApprovedEnabled = true;
        }
    }

    private SetNewEntityMode(args: any) {
        this.isNewEntity = true;

        if (this.additionalDataPM)
            this.GetPreviousPageByNumber(this.additionalDataPM.LastPageNumber);


        this.ReconcileExternalPagePM = this.InitializeNewPage(args);
    }

    private InitializeNewPage(args: any) {
        var newEntity = new ReconcileExternalPagePM();
        var objectTableId = window.ObjectTables.filter(d => d.Name === this.PageObjectTableName)[0];
        newEntity.Tenant = SessionLocator.Tenant;
        newEntity.StatusCode = "1"; // 1- Draft
        newEntity.PageNo = 0;
        newEntity.EntityId = args.BankAccountId;
        newEntity.ObjectTableId = objectTableId.Id;
        newEntity.GLAccountId = args.GLAccountId;
        newEntity.EntryTypeCode = "1"; // 1- Manual

        if (this.additionalDataPM && this.additionalDataPM.LastPageEndDate) {
            newEntity.FromDate = this.GetLastPageDatePlusOneDay(args);
            newEntity.StartBalance = this.additionalDataPM.LastPageCloseBalance;
        }

        return newEntity;
    }

    private GetLastPageDatePlusOneDay(args: any) {
        var lastDate: Date = new Date(this.additionalDataPM.LastPageEndDate);
        var lastDatePlusOne = new Date(lastDate.setDate(lastDate.getDate() + 1));
        var date = DateTool.GetDate(lastDatePlusOne.getFullYear(), lastDatePlusOne.getMonth(), lastDatePlusOne.getDate(), 0, 0, 0);
        return date;
    }

    private SetEditMode(args: any) {
        this.ReconcileExternalPagePM = args.externalPage;


        this.isNewEntity = false;
        this.GetPreviousPageForExternalPage(this.ReconcileExternalPagePM.PageNo);
        this.SetComponentEditablity();

        this.SetUIProperties();
    }

    private SetComponentEditablity() {
        if (this.ReconcileExternalPagePM.StatusCode == "2" || this.ReconcileExternalPagePM.StatusCode == "3")
            this.IsDisplayOnly = true;
        else
            this.IsDisplayOnly = false;
    }

    //#region Properties
    get FromDate() { return this.ReconcileExternalPagePM.FromDate; }
    set FromDate(value: Date) {
        if (this.ReconcileExternalPagePM.FromDate != value) {
            this.ReconcileExternalPagePM.FromDate = value;
            var todayDate = DateTool.GetCurrentDateTimeAsUtc();
            if (value > todayDate) {
                this.UIProperties.SetValidity("FromDate", this.ExternalPageTable, false, TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
            } else {
                this.UIProperties.SetValidity("FromDate", this.ExternalPageTable, true, "ok");

            }
        }
    }


    get ToDate() { return this.ReconcileExternalPagePM.ToDate; }
    set ToDate(value: Date) {
        if (this.ReconcileExternalPagePM.ToDate != value) {
            this.ReconcileExternalPagePM.ToDate = value;
            var todayDate = DateTool.GetCurrentDateTimeAsUtc();
            if (value > todayDate) {
                this.UIProperties.SetValidity("ToDate", this.ExternalPageTable, false, TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
            } else {
                this.UIProperties.SetValidity("ToDate", this.ExternalPageTable, true, "ok");

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
            if (hasReconciledLines) {
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
    closeScreen: boolean = true;
    EditButtonClicked() {
        this.ReconcileExternalPagePM.StatusCode = "1";

        this.SetComponentEditablity();

        this.SetUIProperties();
        this.closeScreen = false;

        this.SaveEntity();

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
        Validator.TryValidateObject(this.ReconcileExternalPagePM, this.ExternalPageTable, errors);

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
                if (AppTool.IsNullOrEmpty(line.CreditAmount) && AppTool.IsNullOrEmpty(line.DebitAmount)) {
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

    RestoreButtonClicked() {
        this.IsCancelApprovedEnabled = true;
        this.EditButtonClicked();
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

            this._ReconcileExternalPagePMService.update(this.ReconcileExternalPagePM).subscribe((myResult: any) => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError && this.closeScreen) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }

                else {
                    if (!this.closeScreen) {
                        this.CurrentSession.StartBusyIndicatorSaving();
                        this._ReconcileExternalPagePMService.get(mm.Result.Id).subscribe((myResult: any) => {

                            var result: ServiceResponse = myResult;
                            if (!result.HasError) {
                                this.ReconcileExternalPagePM = result.Result;
                                if (this.IsRestoreButtonVisibile) {
                                    this.IsRestoreButtonVisibile = false;
                                }

                                this.FillGridsData();
                            }
                            this.CurrentSession.StopBusyIndicator();

                        });
                    }
                    else if (this.isApprovedButtonClicked) {
                        this.isApprovedButtonClicked = false;
                        this.ReconcileExternalPagePM.StatusCode = '1' // 1- Draft
                    }

                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }


                this.closeScreen = true;
                this.ReconcileExternalPagePM.StatusName = mm.Result.StatusName;
            });

        }
    }

    SetCurrency() {
        if (this.PageObjectTableName == "BankAccount") {
            if (this.EntityPM.GLAccountCurrencyId && this.EntityPM.GLAccountCurrencyId == "multi")
                this.SetMultiCurrencyAmountHeader();
            else if (this.EntityPM.GLAccountCurrencyId)
                this.GetCurrency(this.EntityPM.GLAccountCurrencyId);
        }
        else if (this.PageObjectTableName == "GLAccount") {
            if (this.EntityPM.IsMultiCurrency == true)
                this.SetMultiCurrencyAmountHeader();
            else
                this.GetCurrency(this.EntityPM.CurrencyId);
        }
    }

    private SetMultiCurrencyAmountHeader() {
        this.IsMultiCurrency = true;
        this.AmountColHeader = this.AMOUNT_TEXT + " (" + this.TenantCurrency.Code + ")";
        this.CreditAmountColHeader = this.CreditAMOUNT_TEXT + " (" + this.TenantCurrency.Code + ")";
        this.DebitAmountColHeader = this.DebitAMOUNT_TEXT + " (" + this.TenantCurrency.Code + ")";
    }

    private GetCurrency(currencyId: any) {
        this._CurrencyPMService.get(currencyId).subscribe((myResult: any) => {
            var currency = myResult.Result;
            if (!AppTool.IsNullOrEmpty(currency)) {
                this.currency = currency;
                this.AmountColHeader = this.AMOUNT_TEXT + " (" + this.currency.Code + ")";
                this.CreditAmountColHeader = this.CreditAMOUNT_TEXT + " (" + this.currency.Code + ")";
                this.DebitAmountColHeader = this.DebitAMOUNT_TEXT + " (" + this.currency.Code + ")";
            }
            else {
                console.log("[!] Cannot find the currency !!");
                this.AmountColHeader = this.AMOUNT_TEXT;
                this.CreditAmountColHeader = this.CreditAMOUNT_TEXT;
                this.DebitAmountColHeader = this.DebitAMOUNT_TEXT;
            }
        });
    }

    //#region Prev Bank Page
    OpenPrevPage() {
        console.log(this.PreviousPagePM);
        this.OpenExternalPageWindow(this.PreviousPagePM);
    }
    OpenExternalPageWindow(externalPage: any = null) {

        var windowTitle = externalPage ? (TextCodeTranslator.Translate("ReconcileExternalPage.F.PageNo") + " " + externalPage.PageNo) : TextCodeTranslator.Translate("Accounting.General.O.NewPage");

        var windowArgs: any = {};
        windowArgs.PageObjectTableName = this.PageObjectTableName;
        windowArgs.externalPage = externalPage;
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.BankAccountId = this.EntityPM.Id;
        windowArgs.GLAccountId = this.EntityPM.GLAccountId;
        windowArgs.BankAccount = this.EntityPM;
        windowArgs.AdditionalDataPM = this.additionalDataPM;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => {

        });
        logWindow.Show('./Accounting/Components/NewEntity/AddEditRecoExPageComponent');

    }
    GetPreviousPageByNumber(previousPageNo: string) {

        if (previousPageNo) {
            //get last page
            this._ReconcileExternalPageExtendedPMService.GetPageByNumber(previousPageNo, this.EntityPM.Id, this.PageObjectTableName).subscribe((myResult: ServiceResponse) => {
                var bankPage = myResult.Result;
                if (!AppTool.IsNullOrEmpty(bankPage)) {
                    this.PreviousPagePM = bankPage;
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

    GetPreviousPageForExternalPage(extPageNumber: number) {

        if (extPageNumber) {
            //get last page
            this._ReconcileExternalPageExtendedPMService.GetPreviousPageByNumber(extPageNumber, this.EntityPM.Id, this.PageObjectTableName).subscribe((myResult: ServiceResponse) => {
                var bankPage = myResult.Result;
                if (!AppTool.IsNullOrEmpty(bankPage)) {
                    this.PreviousPagePM = bankPage;
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
        this.UIProperties.SetEnabled("FromDate", this.ExternalPageTable, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ToDate", this.ExternalPageTable, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StartBalance", this.ExternalPageTable, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CloseBalance", this.ExternalPageTable, !this.IsDisplayOnly);

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


    public ShowMessage(message: string) {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);
    }

    private fileName: string;
    private fileExtension: string;
    OnFileChanged(fileEvent) {
        var file = fileEvent.target.files[0];

        if (file) {
            var extension: string = file.name.split('.')[1];

            if (extension.includes("xls")) {
                var file = fileEvent.target.files[0];
                this.UploadExcel(file);
            }

            else {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("You have to upload excel files only");
            }
        }
    }

    UploadExcel(file: any) {
        this.CurrentSession.StartBusyIndicator("Uploading...");
        this.UploadButtonIsEnabled = false;

        this.fileName = null;
        this.fileExtension = null;

        if (!AppTool.IsNullOrEmpty(file.name)) {
            var name = file.name.split('.');
            if (name.length == 2) {
                this.fileName = name[0];
                this.fileExtension = name[1];
            }
        }
        if (file && file.size > 0) {
            var documentExtendedService = new DocumentsFilingExtendedPMService();
            documentExtendedService.GetFileSizeAndUnit(file.size).subscribe((response: ServiceResponse) => {
                if (!response.HasError) {
                    var myResult = response.Result;
                    if (myResult) {
                        this.StartUploadingExcelFile(file);
                    }
                }
            });
        }
    }

    StartUploadingExcelFile(file: any) {
        if (file && file.size > 0) {
            var filebuffer = file.slice(0, file.size);
            this.ConvertArrayBufferToBase64(filebuffer, this);
        }
    }

    public partnersUploadExcelParameter: PartnersUploadExcelParameter;
    ConvertArrayBufferToBase64(file: any, viewmodel: any) {
        return new Promise((resolve, reject) => {
            var reader: FileReader = new FileReader();
            var reader = new FileReader();
            reader.onload = function (e) {
                var binary = '';
                var bytes = new Uint8Array(ResultAsArray(e));
                var len = bytes.byteLength;

                for (var i = 0; i < len; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }

                viewmodel.partnersUploadExcelParameter = new PartnersUploadExcelParameter();
                viewmodel.partnersUploadExcelParameter.FileData = window.btoa(binary);
                viewmodel.partnersUploadExcelParameter.FileName = viewmodel.FileName;
                viewmodel.partnersUploadExcelParameter.ComputingPartnerCode = viewmodel.ComputingPartnerCode;
                viewmodel.SendExcelToServer(viewmodel.partnersUploadExcelParameter);
            };
            reader.onerror = function (e) {
                console.log(e);
            };
            reader.readAsArrayBuffer(file);
        });
    }

    SendExcelToServer(filters: ReconcileExternalPageLineParameters) {
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this._ReconcileExternalPageExtendedPMService.ImportReconcileExternalPageLineFromExcel(filters,this.EntityPM.bankId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                filters = response.Result;
                this.FillReconcileExternalPageLines(filters);
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            }

            else {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = response.ErrorsArray;
            }
        });
    }

    FillReconcileExternalPageLines(filters: ReconcileExternalPageLineParameters) {
        var line = 0;
        if (this.ReconcileExternalPagePM.ReconcileExternalPageLines.length > 0) {
            var line = this.ReconcileExternalPagePM.ReconcileExternalPageLines.reduce(function (prev, current) { return (prev.LineNumber > current.LineNumber) ? prev : current }).LineNumber;
        }
        line++; // last no.
        if (filters.ExcelReconcileExternalPageLines.length > 0) {
            this.ReconcileExternalPagePM.EntryTypeCode = "2";
            filters.ExcelReconcileExternalPageLines.forEach(element => {
                var pageLine: ReconcileExternalPageLinePM = new ReconcileExternalPageLinePM(this.ReconcileExternalPagePM);
                pageLine.Tenant = SessionLocator.Tenant;
                pageLine.ReconcileExternalPageId = this.isNewEntity ? "new" : this.ReconcileExternalPagePM.Id;
                var datemomentobject = moment.utc(element.ReferenceDate, "YYYY-MM-DD")
                pageLine.ReferenceDate = datemomentobject.toDate();
                pageLine.DebitAmount = element.DebitAmount;
                pageLine.CreditAmount = element.CreditAmount;
                pageLine.Reference = element.Reference;
                pageLine.Notes = element.Notes;
                pageLine.LineNumber = line++;
                pageLine.IsReconciled = false;
                this.ReconcileExternalPagePM.AddReconcileExternalPageLine(pageLine);
                var item = new PageLineModel(pageLine, this);
                this.PageLinesList.Insert(item);
            });
            if (this.PageLinesList.Length == 0) {
                this.UploadButtonIsEnabled = true
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
        if (this.PageLinesList.Length == 0) {
            this.UploadButtonIsEnabled = true
        }
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
                    this.SetCurrency();

                } else
                    this.SetCurrency();

            } else
                this.SetCurrency();

        });
    }

    CalculateTotals() {
        var sum = 0.0;

        if (this.PageLinesList.Length > 0) {
            for (var line of this.PageLinesList.Collection) {

                //debit
                sum -= !line.DebitAmount ? 0 : line.DebitAmount;
                sum += !line.CreditAmount ? 0 : line.CreditAmount;

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

        if (AppTool.IsNullOrEmpty(this.CreditAmount))
            this.CreditAmount = 0;
        if (AppTool.IsNullOrEmpty(this.DebitAmount))
            this.DebitAmount = 0;
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

    get CreditAmount() { return this.pageLinePM.CreditAmount; }
    set CreditAmount(value: number) {
        if (this.pageLinePM.CreditAmount != value) {
            this.pageLinePM.CreditAmount = value;
            this.parent.CalculateTotals();

            if (AppTool.IsNullOrEmpty(value))
                this.CreditAmount = 0;
            // if(value != 0)
            //     this.DebitAmount = 0;
        }
    }

    get DebitAmount() { return this.pageLinePM.DebitAmount; }
    set DebitAmount(value: number) {
        if (this.pageLinePM.DebitAmount != value) {
            this.pageLinePM.DebitAmount = value;
            this.parent.CalculateTotals();

            if (AppTool.IsNullOrEmpty(value))
                this.DebitAmount = 0;
            // if(value != 0)
            //     this.CreditAmount = 0;
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
export class ReconcileExternalPageLineParameters {
    Tenant: number;
    FileData: string;
    CarrierAreaId: string;
    FileName: string;
    FileExtension: string;
    TransportMode: string;
    RowsCount: number;
    ExcelReconcileExternalPageLines: ExcelReconcileExternalPageLine[];
}
export class ExcelReconcileExternalPageLine {
    ReferenceDate: Date;
    DebitAmount: number;
    CreditAmount: number;
    Reference: string;
    Notes: string;
    HasError: boolean;
}



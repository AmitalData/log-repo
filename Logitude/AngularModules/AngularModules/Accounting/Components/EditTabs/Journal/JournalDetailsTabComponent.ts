import {Component, OnInit,ChangeDetectorRef}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {JournalPM} from '../../../EntityPMs/JournalPM';
import {JournalLinePM} from '../../../EntityPMs/JournalLinePM';
import {JournalActionTypePM} from '../../../EntityPMs/JournalActionTypePM';
import {AccountingPeriodList} from '../../../EntityLists/AccountingPeriodList';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {AccountingPeriodExtendedListService} from '../../../Services/ExtendedLists/AccountingPeriodExtendedListService';
import {GLAccountExtendedListService} from '../../../Services/ExtendedLists/GLAccountExtendedListService';
import {AccountingPeriodListService} from '../../../Services/StandardLists/AccountingPeriodListService';
import {RatesTableExtendedListService} from '../../../../Infrastructure/Services/ExtendedLists/RatesTableExtendedListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {CurrencyPM} from '../../../../Common/EntityPMs/CurrencyPM';
import {CurrencyList} from '../../../../Common/EntityLists/CurrencyList';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {JournalValidator} from '../../../Validators/JournalValidator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {GLAccountListService} from '../../../Services/StandardLists/GLAccountListService'
import { APInvoicePMService } from '../../../../Invoice/Services/StandardPMs/APInvoicePMService';
import { APInvoicePM } from '../../../../Invoice/EntityPMs/APInvoicePM';
import { GLAccountSecurityLevelService } from 'Accounting/Utilities/GLAccountSecurityLevelService';
import { FullAccountingSettingListService } from 'Accounting/Services/StandardLists/FullAccountingSettingListService';
declare var window: any;

@Component({

    templateUrl: './JournalDetailsTabComponent.html',
    providers:
        [CurrencyListService,
        AccountingPeriodExtendedListService,
        RatesTableExtendedListService,
        APInvoicePMService
        ]
})

export class JournalDetailsTabComponent extends BaseComponent implements OnInit {
  public ActionId: any;

    public EntityPM: JournalPM = null;
    public ObjectTableName = "Journal";
    public DataContext = this;
    defaultCurrencyId: string = SessionLocator.TenantPM.CurrencyId;
    public  TenantCurrency = SessionLocator.TenantPM.CurrencyCode;
    JournalLines: ObservableCollection;//JournalLineModel[];
    creditTotal: number = 0;
    debitTotal: number = 0;
    difference: number = 0;
    differenceColor = '#9e4300';
    journalDisabled: boolean = false;
    forceFocus: boolean = false;
    PointerEvents: string = 'auto';
    Opacity: string = "1";
    referencesDivHeight: number;
    Approved: boolean = false;
    IsJournalEditableAfterApproval: boolean = false;
    APInvoice: APInvoicePM;
    Voided: boolean = false;
    private CancelledStatusCode: string = "5";
    private fullAccountingSettingListService: FullAccountingSettingListService;
    public IsSecurityLevelVisible: boolean = false;
    IsJournalSecurityManaged: boolean = false;
    public IsSecurityLevelOK: boolean = true;
    public UserSecurityLevel: number = SessionLocator.LoggedUserPM.SecurityLevel;
    AccountingPeriods: AccountingPeriodList[] = [];
    _AccountingPeriodListService: AccountingPeriodListService = new AccountingPeriodListService();
    ratesTableExtendedListService: RatesTableExtendedListService = new RatesTableExtendedListService();
    public EntityWarningsList: string[] = [];

    OnRowEnded($event) {
        console.log("this.JournalLines.Length : " + this.JournalLines.Length);
        if (($event) == this.JournalLines.Length) {
            this.AddLine();
            //this.CurrentSession.ResetRowIndex();
        }
    }
    OnFocus() {
        if (this.JournalLines.Length == 0) {
            this.AddLine();
        }
    }

    getHeadercurrencyRate(CurrencyId:string){
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        if(this.defaultCurrencyId == CurrencyId){
            this.HeadercurrencyRate = 1;
            return ;
        }
        this.ratesTableExtendedListService.getExchageRateByValueAndDate(this.defaultCurrencyId,CurrencyId, this.AccountingDate).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    if (myResponse.Result != undefined && myResponse.Result != null) {
                        var rate = myResponse.Result;
                        this.HeadercurrencyRate = rate.Rate;
                        this.UpdateLinesExchangeRate();
                        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];

                    } else {
                        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                        this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(TextCodeTranslator.Translate("Journal.O.ExchangeRateValidation"));
                    }
                }

            }


        });
    }
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(
        private entityArgs: EntityArgs,
        private currencyListService: CurrencyListService,
        private accountingPeriodListService: AccountingPeriodExtendedListService,
        private CD: ChangeDetectorRef,
        private apInvoicePMService :APInvoicePMService
    ) {
        super();

        this.fullAccountingSettingListService = new FullAccountingSettingListService();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.JournalLines = new ObservableCollection([]);
      
        this.EntityPM = entityArgs.EntityPM;

        this.CheckFeatures();
        this.getAccountingSettingSecurityLevelField();
        if (this.IsSecurityLevelOK != undefined && !this.IsSecurityLevelOK) {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(TextCodeTranslator.Translate("Journal.O.ViewingNotAuthorized"));

        }


        this.SetDatesDefaultValues();

       // this.FillGrid();
        this.SetUIProperties();

        // redraw
        this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(isSuccess => {
            if (isSuccess) {
                this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                this.FillGrid();
                this.SetUIProperties();

            }

        });

        this.Listen();
    }
    private SetDatesDefaultValues() {
        if (this.AccountingDate == null)
            this.AccountingDate = new Date();


    }

    CheckFeatures() {
     // if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        var table = window.ObjectTables.filter(d => d.Name === 'Journal')[0];
        var journalSecurityManagedFeature = FeatureLocator.Features.filter(f => (f.Code == "Journal.Feature.ManageSecurity") && f.ObjectTableId == table.Id)[0];
        if (journalSecurityManagedFeature) {
            this.IsJournalSecurityManaged = true;
        }
    }

    getAccountingSettingSecurityLevelField() {
        if (this.IsJournalSecurityManaged) {
            this.fullAccountingSettingListService.getSingle(SessionLocator.Tenant.toString()).subscribe((response: any) => {
                this.CurrentSession.StopBusyIndicator();
                var userSecurityLevel: number = 0;
                if (this.UserSecurityLevel != undefined) {
                    userSecurityLevel = this.UserSecurityLevel;
                }
                var journalSecurityLevel: number = 0;
                if (this.EntityPM.SecurityLevel != undefined) {
                    journalSecurityLevel = this.EntityPM.SecurityLevel;
                }
                this.IsSecurityLevelOK = true;

                if (response != null) {
                    var response = response.Result;
                    if (response.IsSecurityLevelActivated && userSecurityLevel >= 1) {
                        this.IsSecurityLevelVisible = true;
                        if (journalSecurityLevel > userSecurityLevel) {
                            this.IsSecurityLevelOK = false;
                            this.IsSecurityLevelVisible = false;
                        }
                    }
                    else if (response.IsSecurityLevelActivated && userSecurityLevel == 0 && journalSecurityLevel > 0) {
                        this.IsSecurityLevelOK = false;
                        this.IsSecurityLevelVisible = false;
                    }
                    else {
                        this.IsSecurityLevelVisible = false;
                    }
                }
            });
        } else {
            this.IsSecurityLevelVisible = false;
        }
    }





    public CurrentEditComponentId: string;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private TabSelectedEvent: any = null;
    Listen() {


        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            //
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                        this.FillGrid();
                        this.SetUIProperties();
                    }
                });
            }

            //
            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                        this.FillGrid();
                        this.SetUIProperties();
                        console.log("Entity Reloaded");
                    }
                });
            }
        }

        this.EntityPM.PropertyChanged.subscribe(changes=>{
          console.log("JournalPM changed",changes);

        });
    }

    SetUIProperties() {
        //Display only
        if (this.IsSecurityLevelOK != undefined && !this.IsSecurityLevelOK) {
            //disable controls
            this.journalDisabled = true;
            this.PointerEvents = 'none';
            this.Opacity = "1";
            this.referencesDivHeight = 0;
            this.UIProperties.SetVisibility("Reference1", "Journal", false);
            this.UIProperties.SetVisibility("Reference2", "Journal", false);
            this.UIProperties.SetVisibility("Reference3", "Journal", false);
            this.UIProperties.SetVisibility("Notes", "Journal", false);
        }
        else if (this.EntityPM.StatusCode == "3" || this.EntityPM.StatusCode == this.CancelledStatusCode) { // 3-Voided and 2-Approved
            //disable controls
            this.journalDisabled = true;
            this.PointerEvents = 'none';
            this.Opacity = "1";
            this.Voided = true;
        }
        else if (this.EntityPM.StatusCode == "2") { // 3-Voided and 2-Approved
            //disable controls
            this.journalDisabled = true;
            this.PointerEvents = 'none';
            this.Opacity = "1";
            this.referencesDivHeight = 0;
            this.Approved = true;
            this.UIProperties.SetVisibility("Reference1", "Journal", false);
            this.UIProperties.SetVisibility("Reference2", "Journal", false);
            this.UIProperties.SetVisibility("Reference3", "Journal", false);
            this.UIProperties.SetVisibility("Notes", "Journal", false);
        }
        else{
            this.DueDate=new Date();
            this.DocumentDate=new Date();
        }

        this.CheckIfJournalEditableAfterApproval();
    }


    private CheckIfJournalEditableAfterApproval() {
        if (this.Approved && this.EntityPM.IsLedgerCreated) {

            this.CheckIfJournalManuallyCreated();

            this.CheckIfJournalFromAPInvoiceAndCreatedExternally();
        }
    }

    private CheckIfJournalManuallyCreated() {
        const allowedAccountingEntityCodes = [
            '1', //Journal
            '10', //Adjustment
            '12' //Bank Adjustment
        ];

        const isJournalManuallyCreated =
            this.EntityPM.ExternalSystem == null &&
            this.EntityPM.ExternalNo == null &&
            allowedAccountingEntityCodes.includes(this.EntityPM.AccountingEntityCode);

        if (isJournalManuallyCreated) {
            this.IsJournalEditableAfterApproval = true;
        }
    }

    private CheckIfJournalFromAPInvoiceAndCreatedExternally() {
        var isJournalCreatedFromAPInvoice = this.CheckIfJournalFromAPInvoice();

        if (isJournalCreatedFromAPInvoice) {
            this.CheckIfAPInvoiceCreatedExternally(this.EntityPM.AccountingEntityId);
        }
    }

    private CheckIfJournalFromAPInvoice() {
        const AccountingEntityCode_APInvoice = "4";
        var isJournalCreatedFromAPInvoice = this.EntityPM.AccountingEntityCode == AccountingEntityCode_APInvoice;
        return isJournalCreatedFromAPInvoice;
    }

    CheckIfAPInvoiceCreatedExternally(id: string) {
        this.apInvoicePMService.get(id).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.APInvoice = myResponse.Result;
                    if (this.APInvoice.IsExternalEntity == false) {
                        this.IsJournalEditableAfterApproval = true;
                    }
                }
            }
        });
    }

    FillGrid() {
        if (this.IsSecurityLevelOK == undefined || this.IsSecurityLevelOK) {
            // if entity in edit mode
            if (this.EntityPM.Id != undefined || this.EntityPM.JournalLines.length > 0) {
                var tempItemSource: JournalLineModel[] = [];
                if (this.EntityPM.JournalLines != null) {
                    for (var i = 0; i < this.EntityPM.JournalLines.length; i++) {
                        var line = new JournalLineModel(this.EntityPM.JournalLines[i], this);
                        tempItemSource.push(line);
                        //this.JournalLines.Insert(line);
                    }
                    this.JournalLines.InsertCollection(tempItemSource);
                    //for (let item of this.EntityPM.JournalLines) {
                    //    var line = new JournalLineModel(item, this);
                    //    this.JournalLines.Insert(line);
                    //}
                }
                this.CalculateTotals();

            }
            else {
                this.EntityPM.StatusCode = "0"; // Draft
                this.EntityPM.TypeCode = "0"; // Manual
                this.EntityPM.AccountingEntityCode = "1"; // Journal

                this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
                this.EntityPM.Tenant = SessionLocator.Tenant;

                var journalLine: JournalLinePM = new JournalLinePM(this.EntityPM);
                journalLine.Line = 1;
                journalLine.Tenant = this.EntityPM.Tenant;
                journalLine.DocumentDate = this.DocumentDate;
                journalLine.DueDate = this.DueDate;
                journalLine.CurrencyId = this.Currency.Id;
                this.EntityPM.AddJournalLine(journalLine);
                var line = new JournalLineModel(journalLine, this);
                line.Currency = this.Currency;
                this.JournalLines.Insert(line);

            }
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
        this.GetDefaultValues();
        //set focus on accounting date
        var t = setTimeout(() => { this.forceFocus = true; }, 1);
    }

    //#region Properties
    reference1: string;
    get Reference1() { return this.reference1; }
    set Reference1(value: string) {
        if (this.reference1 != value) {
            this.reference1 = value;
            this.UpdateFirstLineReferencesNotesAndCurrency();


        }
    }

    reference2: string;
    get Reference2() { return this.reference2; }
    set Reference2(value: string) {
        if (this.reference2 != value) {
            this.reference2 = value;
            this.UpdateFirstLineReferencesNotesAndCurrency();
        }
    }

    reference3: string;
    get Reference3() { return this.reference3; }
    set Reference3(value: string) {
        if (this.reference3 != value) {
            this.reference3 = value;
            this.UpdateFirstLineReferencesNotesAndCurrency();

        }
    }

    notes: string;
    get Notes() { return this.notes; }
    set Notes(value: string) {
        if (this.notes != value) {
            this.notes = value;
            this.UpdateFirstLineReferencesNotesAndCurrency();

        }
    }

    currency: CurrencyList;
    get Currency() { return this.currency; }
    set Currency(value: CurrencyList) {
        if (this.currency != value) {
            this.currency = value;
            this.CurrencyId =this.EntityPM.IsNew ? value? value.Id: null:this.EntityPM.CurrencyId;
            this.UpdateFirstLineReferencesNotesAndCurrency();
        }
    }

    headerCurrency: CurrencyList;
    get HeaderCurrency() { return this.headerCurrency; }
    set HeaderCurrency(value: CurrencyList) {
        if (this.headerCurrency != value) {
            this.headerCurrency = value;
         }
    }

    headercurrencyRate: number;
    get HeadercurrencyRate() { return this.headercurrencyRate; }
    set HeadercurrencyRate(value: number) {
        if (this.headercurrencyRate!= value) {
            this.headercurrencyRate = value;
        }
    }


    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (value && this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
            this.getHeaderCurrency(value);
          //  this.getHeadercurrencyRate(value);

        }
        if (!value) {
            this.HeaderCurrency = null;
        }
    }


    public get SecurityLevel() { return this.EntityPM.SecurityLevel; }
    public set SecurityLevel(value: number) {
        if (this.EntityPM.SecurityLevel != value) {
            this.EntityPM.SecurityLevel = value;
        }
    }


    get DocumentDate() { return this.EntityPM.DocumentDate; }
    set DocumentDate(value: Date) {
        if (this.EntityPM.DocumentDate != value) {
            if (value != null) {
                this.ValidateDates(value, "DocumentDate");
            }

          this.EntityPM.DocumentDate = value;
          this.UpdateLinesDates();
        }
    }

    get DueDate() { return this.EntityPM.DueDate; }
    set DueDate(value: Date) {
        if (this.EntityPM.DueDate != value) {
              if (value != null) {
                  this.ValidateDates(value, "DueDate");
            }
          this.EntityPM.DueDate = value;
          this.UpdateLinesDates();
        }
    }
    get AccountingDate() { return this.EntityPM.AccountingDate; }
    set AccountingDate(value: Date) {
        if (this.EntityPM.AccountingDate != value) {

            if (value != null) {
                this.ValidateDates(value,"AccountingDate");

            }

            this.EntityPM.AccountingDate = value;
            this.UpdateLinesDates();
            this.UpdateLinesAccountingDates();
            if (this.CurrencyId) this.getHeadercurrencyRate(this.CurrencyId);
        }



    }
    private async ValidateDates(value:Date, fieldName:string) {
        if (fieldName == "AccountingDate") {
            this.AccountingPeriods=null;
            await this.GetAccountingPeriods();
            var accountingPeriod = this.AccountingPeriods.find(d => d.Year == value.getFullYear());
            if (accountingPeriod) {

                var month = value.getMonth() + 1;

                // Valid Month => (ClosedMonth < month <= OpenMonth)
                if (month > accountingPeriod.ClosedMonth && month <= accountingPeriod.OpenMonth) { // valid (open month)

                    JournalValidator.SetAccountingDateInValid=false;

                } else { // invalid (closed month)

                    // push the error to errors list
                    JournalValidator.SetAccountingDateInValid=true;

                    this.EntityPM.AccountingDate = value;

                    return;
                }
            }
        }
          else  if (fieldName != "DueDate") {
                //FUTURE DATE VALIDATION
                if (value > DateTool.GetCurrentDateTimeAsUtc()) {
                    var msg = TextCodeTranslator.Translate("Journal.M.FutureDateForbidden");
                    this.UIProperties.SetValidity(fieldName, this.ObjectTableName, false, msg);

                    // push the error to errors list
                   // this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(msg);
                    if (fieldName == "AccountingDate") this.EntityPM.AccountingDate = value;
                    return;
                } else {
                  //  this.CurrentSession.CurrentEditComponent.ValidationErrorsList = []; // empty errors list
                    this.UIProperties.SetValidity(fieldName, this.ObjectTableName, true, "OK");
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

    localAmountHeader: string;
    get LocalAmountHeader() { return this.localAmountHeader; }
    set LocalAmountHeader(value: string) {
        if (this.localAmountHeader != value) {
            this.localAmountHeader = value;
        }
    }
    //#endregion

    AddLine() {

        if (this.journalDisabled) return;

        var errors = [];

        if (this.JournalLines.Collection.length > 0) {

            // Validation
            var lastRow = this.JournalLines.Collection[this.JournalLines.Collection.length - 1];
            errors = JournalValidator.ValidateJournalLines(lastRow);

            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
            if (errors.length > 0) {
                return;
            }

            //lastRow.SplittedCheck();


        }

        //
        // Adding New Line
        var journalLine: JournalLinePM = new JournalLinePM(this.EntityPM);
        journalLine.Tenant = this.EntityPM.Tenant;
        this.EntityPM.AddJournalLine(journalLine);
        journalLine.Line = this.JournalLines.Collection.length > 0 ? (lastRow.Line + 1) : 1;

        if (!AppTool.IsNullOrEmpty(this.EntityPM)) {
            journalLine.JournalId = this.EntityPM.Id;
        }

        var line = new JournalLineModel(journalLine, this);
        line.Reference1 = this.reference1;
        line.Reference2 = this.reference2;
        line.Reference3 = this.reference3;
        line.Currency = this.HeaderCurrency;
        line.currencyRate = this.HeadercurrencyRate;
        line.DocumentDate = this.DocumentDate != null ? this.DocumentDate : null;
        line.DueDate = this.DueDate != null ? this.DueDate : null;
        line.Notes = this.notes;
        this.JournalLines.Insert(line);
    }

    DetectChanges() {
        this.CD.detectChanges();
    }

    RemoveLine(line: any) {
        if (this.journalDisabled) return;

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Accounting.General.O.Areyousuredeleteline") + " " + line.Line + " ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                this.JournalLines.Remove(line);
                this.EntityPM.JournalLines.splice(line.Line - 1, 1);
                //var ItemsSource = [];

                // Recalculate line numbers
                for (var i = 0; i < this.JournalLines.Collection.length; i++) {
                    var oldItem = this.JournalLines.Collection[i];
                    var updatedItem = this.JournalLines.Collection[i];
                    updatedItem.Line = i + 1;
                    this.JournalLines.Update(oldItem, updatedItem);
                }
                //this.JournalLines.Collection.forEach((item) => {
                //    ItemsSource.push(item);
                //});
                //this.JournalLines = ItemsSource;

                this.CalculateTotals();
            }
        });

    }

getHeaderCurrency(CurrencyId:string){
    this.currencyListService.getSingle(this.CurrencyId).subscribe((myResponse: ServiceResponse) => {
        if (myResponse != null) {
            if (!myResponse.HasError) {
                this.HeaderCurrency = myResponse.Result;
            }
        }
    });
}

    TextChanged(searchtext) {
        //console.log(this.JournalLines);
        //console.log(this.EntityPM.JournalLines);
    }

    GetDefaultValues() {
        // Tenant currency
        this.currencyListService.getSingle(this.defaultCurrencyId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.Currency = myResponse.Result;
                    this.FillGrid();
                    this.localAmountHeader = "Amount (" + myResponse.Result.Code + ")";
                    console.log(">>Tenant Currency: ", myResponse.Result);

                }
            }
        });

        //this.GetAccountingPeriods();
        // Current Accouting Period
        //var periodTypeCode = "1" // 1-Regular
        //this.accountingPeriodListService.getByYear(new Date().getFullYear(), periodTypeCode).subscribe((myResponse: ServiceResponse) => {
        //    if (myResponse != null) {
        //        if (!myResponse.HasError) {
        //            this.AccountingPeriod = myResponse.Result;
        //            console.log(">>Current Accounting Period: ", myResponse.Result);
        //        }
        //    }
        //});
    }

    CalculateTotals() {
        this.creditTotal = this.debitTotal = this.difference = 0;
        for (let line of this.JournalLines.Collection) {

            if (!AppTool.IsNullOrEmpty(line.LocalAmount)) {
                if (line.ActionCode == "1")
                    this.creditTotal += line.LocalAmount;
                else if (line.ActionCode == "2")
                    this.debitTotal += line.LocalAmount;
                else if (line.ActionCode == "3") {
                    this.creditTotal += line.LocalAmount;
                    this.debitTotal += line.LocalAmount;
                }
                else if (line.ActionCode == "4") {
                    this.creditTotal += line.LocalAmount;
                    this.debitTotal += line.LocalAmount;
                }
            }


        }
        if(this.debitTotal - this.creditTotal > 0)
            this.differenceColor ='#9e4300'
        else if (this.debitTotal - this.creditTotal == 0)
            this.differenceColor = 'black'
        else
            this.differenceColor = '#008c7a'        

        this.difference = Math.abs(this.debitTotal - this.creditTotal);
    }

    async GetAccountingPeriods() {
          
            var filters = new ApiQueryFilters(true);
            filters.addAdditionalFilter("PeriodTypeCode", "1", null, null, "Equals", false, false, false, "string"); // 1-Regular   
             
        const res = await new Promise<boolean>((resolve, reject) => {   
            
            this._AccountingPeriodListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        this.AccountingPeriods = myResponse.Result;
                        console.log(">>Accounting Periods: ", myResponse.Result);
                        resolve(true);
                    }
                }
            });
        })
       return res;

    }

    header_year: number;
    header_month: number;
    header_day: number;
    GetHeaderDateParts() {
        this.header_year = this.headerDate.getFullYear();
        this.header_month = this.headerDate.getMonth() + 1;
        this.header_day = this.headerDate.getDate();
    }
   line_year:number;
   line_month:number;
   line_day:number;

    GetLineDateParts() {
        this.line_year = this.lineDate.getFullYear();
        this.line_month = this.lineDate.getMonth() + 1;
        this.line_day = this.lineDate.getDate();
    }
    SetLineDateParts() {
        if (this.line_year != this.header_year)
            this.line_year = this.header_year;

        if (this.line_month != this.header_month)
            this.line_month = this.header_month;

        if (this.line_day != this.header_day)
            this.line_day = this.header_day;
    }

    ValidateDayAccordingToMonth(line: JournalLineModel) {
        if (this.line_day > this.lastDay(this.line_year, this.line_month - 1)) {
            return false;
        } else {
         return true
        }
    }

    SetLineDate(line: JournalLineModel) {
        this.lineDate.setFullYear(this.line_year);
        this.lineDate.setMonth(this.line_month - 1);
        this.lineDate.setDate(this.line_day);

        if (!line.ActionCode) line.accDay = this.line_day;
        line.AccountingDate = new Date(this.line_year, this.line_month-1, line.accDay);
    }
    lineDate: Date;
    headerDate: Date;
    UpdateLinesAccountingDates() {
        var lines = this.JournalLines.Collection;
        if (lines) {
            lines.forEach((line: JournalLineModel) => {
                this.headerDate = this.AccountingDate;
                if (this.headerDate) {
                    this.GetHeaderDateParts();
                    this.lineDate = this.AccountingDate;
                    if (line.AccountingDate) {
                         this.lineDate = new Date(line.AccountingDate.toString());
                        this.GetLineDateParts();
                    }
                   this.SetLineDateParts();
                    if (!this.ValidateDayAccordingToMonth(line)) {
                        this.lineDate = null;
                        line.AccDay = null;
                    }
                    else {
                        this.SetLineDate(line);

                    }
                }
            });
        }
    }

    lastDay(year, month) {
        return new Date(year, month + 1, 0).getDate();
    }

    Test() {
        var entity = this.EntityPM;
        var lines = this.JournalLines;
        console.log("[TEST] ", entity, this.JournalLines);
    }
    UpdateLinesDates() {
        var lines = this.JournalLines.Collection;
        if (lines) {
            lines.forEach((line: JournalLineModel) => {
                if (!line.ActionCode) {
                    if (line.AccountingDate != this.AccountingDate ) line.AccountingDate = this.AccountingDate;
                    if (line.DueDate != this.DueDate) line.DueDate = this.DueDate;
                    if (line.DocumentDate != this.DocumentDate) line.DocumentDate = this.DocumentDate;
                }
            });


        }
    }
    UpdateLinesExchangeRate() {
        var lines = this.JournalLines.Collection;
        if (lines) {
            lines.forEach((line: JournalLineModel) => {
                if (!line.ActionCode) {
                    if (line.CurrencyId == this.CurrencyId) line.currencyRate = this.headercurrencyRate;
                }
            });


        }
    }
    UpdateFirstLineReferencesNotesAndCurrency() {
        var lines = this.JournalLines.Collection;
        if (lines) {
            lines.forEach((line: JournalLineModel) => {
                if ( !line.ActionCode) {
                    if (line.Reference1 != this.Reference1 ) line.Reference1 = this.Reference1;
                    if (line.Reference2 != this.Reference2) line.Reference2 = this.Reference2;
                    if (line.Reference3 != this.Reference3) line.Reference3 = this.Reference3;
                    if (line.Notes != this.Notes) line.Notes = this.Notes;
                    if (line.Currency != this.Currency) line.Currency = this.Currency;
                }
            });


        }
    }

    EditJournalLine(line: any) {
        var logWindow = new LogitudeWindow();
        logWindow.Width = 450;
        logWindow.Height = 350;
        logWindow.Title = TextCodeTranslator.Translate("Journal.M.EditJournalLine");
        logWindow.WindowArgs = { journalLine: line };
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.JournalLines.Collection.filter(line => line.Line == line.Line)[0].Note = comp.Note;
                }
            });
        });
        logWindow.Show('./Accounting/Components/EditTabs/Journal//UpdateJournalLineComponent');
    }
}


class JournalLineModel extends BaseComponent {
    public JournalLinePM: JournalLinePM = null;
    public ObjectTableName = "JournalLine";
    public DataContext = this;
    ratesTableExtendedListService: RatesTableExtendedListService;
    _GLAccountExtendedListService: GLAccountExtendedListService;
    private glaccountListService:GLAccountListService;
    // private CD: ChangeDetectorRef

    public CreditAccountFilterItems: ApiQueryFilters;
    public DebitAccountFilterItems: ApiQueryFilters;
    accountingDayMustBeInRange: string = TextCodeTranslator.Translate("Journal.O.TheAccountingDayMustBeInRange");
    public IsCurrencyEnabled :boolean =true;
    public isValid: boolean = true;
    public SessionIndex: number;

    public __UserCanSetRateManually: boolean = false; // user can set rate manually by insert forign amount with local amount empty (see WI 24999)
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(
        private journalLine: JournalLinePM,
        private parent: JournalDetailsTabComponent
    ) {
        super();
        this.EntityPM = this.parent.EntityPM;
        this.JournalLinePM = journalLine;
       // this.Currency = this.parent.Currency;
        if (this.JournalLinePM.AccountingDate) {

        } else {
            this.AccountingDate = this.parent.AccountingDate;
        }

        // Set Acc. Day from journalLine.AccountingDay
        if (this.AccountingDate) {
            var date = new Date(this.AccountingDate.toString());
            this.accDay = date.getDate();
        }

        setTimeout(() => {
            this.enableForeighAmountField = this.JournalLinePM.CurrencyId !=SessionLocator.TenantPM.CurrencyId;
        }, 2000);

        //if (this.Currency.Id == SessionLocator.TenantPM.CurrencyId) {
        //    this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, false);
        //}
        //else {
        //    this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, true);
        //}
        this.ratesTableExtendedListService = new RatesTableExtendedListService();
        this._GLAccountExtendedListService = new GLAccountExtendedListService();

        //#region initialize query filters for Accounts LOV
        this.CreditAccountFilterItems = new ApiQueryFilters();
        this.CreditAccountFilterItems.addAdditionalFilter("AccountTypeCode", "5,4", null, null, "Exclude", false, false, false, "string", false, true);

        this.DebitAccountFilterItems = new ApiQueryFilters();
        this.DebitAccountFilterItems.addAdditionalFilter("AccountTypeCode", "4,5", null, null, "Exclude", false, false, false, "string", false, true);
        //#endregion
        this.glaccountListService = new GLAccountListService();
        this.SetCurrencyFieldEnabilityForCopyJournal();
        this.SessionIndex  = SessionLocator.Index;
    }
    SetCurrencyFieldEnabilityForCopyJournal() {

        if (this.JournalLinePM.CurrencyId != "Multi") {
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);
        }
    }

    get AccountingDate() { return this.JournalLinePM.AccountingDate; }
    set AccountingDate(value: Date) {
        if (this.JournalLinePM.AccountingDate != value) {
            this.JournalLinePM.AccountingDate = value;
            if (this.CurrencyId)  this.GetExchangeRate(this.CurrencyId);

        }
    }

    get currencyRate() { return this.JournalLinePM.ExchangeRate; }
    set currencyRate(value: number) {
        if (this.JournalLinePM.ExchangeRate != value) {
            this.JournalLinePM.ExchangeRate = value;
        }
    }

    //#region Line Original Properties
    get Line() { return this.JournalLinePM.Line; }
    set Line(value: number) {
        if (this.JournalLinePM.Line != value) {
            this.JournalLinePM.Line = value;
        }
    }

  OnSelectedItemChanged($event) {
    console.log($event);
  }

    get ActionId() { return this.JournalLinePM.ActionId; }
    set ActionId(value: string) {

        if (this.JournalLinePM.ActionId != value) {
            this.JournalLinePM.ActionId = value;
            this.parent.CalculateTotals();
        }
       // if (value != null) {
        //    this.CurrencyId = null;
         //   this.Currency = null;
       // }
    }

    get ActionCode() { return this.JournalLinePM.ActionCode; }
    set ActionCode(value: string) {

        if (this.JournalLinePM.ActionCode != value) {
            this.JournalLinePM.ActionCode = value;
        }
    }

    // get ActionCode() {
    //     if(this.journalActionType)
    //         return this.journalActionType.Code;
    //     else
    //         return this.JournalLinePM.ActionCode;
    // }
    // set ActionCode(value: string) {

    //     if (this.journalActionType && this.journalActionType.Code != value) {
    //         this.journalActionType.Code = value;
    //     }
    // }

    get ActionName() { return this.JournalLinePM.ActionName == null ? "" : this.JournalLinePM.ActionName }
    set ActionName(value: string) {
        if (this.JournalLinePM.ActionName != value) {
            this.JournalLinePM.ActionName = value;
            //alert(value);
        }

    }

    journalActionType: JournalActionTypePM;
    get JournalActionType() { return this.journalActionType; }
    set JournalActionType(value: JournalActionTypePM) {
        if (this.journalActionType != value) {
            this.journalActionType = value;
            if (value != null) {
                this.ActionCode = value.Code;
                this.ActionName = value.LocalName;
            }
        }

    }

    get DocumentDate() { return this.JournalLinePM.DocumentDate; }
    set DocumentDate(value: Date) {
        if (this.JournalLinePM.DocumentDate != value) {
            this.JournalLinePM.DocumentDate = value;
        }
    }

    get DueDate() { return this.JournalLinePM.DueDate; }
    set DueDate(value: Date) {
        if (this.JournalLinePM.DueDate != value) {
            this.JournalLinePM.DueDate = value;
        }
    }

    get CreditAccountId() { return this.JournalLinePM.CreditAccountId; }
    set CreditAccountId(value: string) {
        if (this.JournalLinePM.CreditAccountId != value) {
            this.JournalLinePM.CreditAccountId = value;
            this.glaccountListService.getSingle(value).subscribe((result:ServiceResponse)=>{
                var entity=result.Result;
                if(entity){
                    this.CreditAccount=entity;
                    this.CreditAccountName=this.CreditAccount.LocalName;
                }
            });
        }
    }

    get DebitAccountId() { return this.JournalLinePM.DebitAccountId; }
    set DebitAccountId(value: string) {
        if (this.JournalLinePM.DebitAccountId != value) {
            this.JournalLinePM.DebitAccountId = value;
            this.glaccountListService.getSingle(value).subscribe((result:ServiceResponse)=>{
                var entity=result.Result;
                if(entity){
                    this.DebitAccount=entity;
                    this.DebitAccountName=this.DebitAccount.LocalName;
                }
            });
        }
    }

    // ساحة المعركة
    get CurrencyId() { return this.JournalLinePM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.JournalLinePM.CurrencyId != value) {
            this.JournalLinePM.CurrencyId = value;
            this.ClearAmounts();
            if (!AppTool.IsNullOrEmpty(value) && !AppTool.IsNullOrEmpty(this.parent.currency)) {
                if (value != SessionLocator.TenantPM.CurrencyId) {
                    this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, true);
                    if (this.parent.defaultCurrencyId != value) {
                        this.GetExchangeRate(value);
                    }
                    else this.currencyRate = 1;
                }
                else {
                    // Local Currency
                    this.isRateManualy = false;
                    this.currencyRate = 1;
                    this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, false);
                    if (this.LocalAmount) {
                        this.isRateCoverted = true;
                        this.ForeignAmount = (this.LocalAmount / this.currencyRate);
                    }
                    else if (this.ForeignAmount) {
                        this.isRateCoverted = true;
                        this.LocalAmount = (this.ForeignAmount * this.currencyRate);
                    }
                }
            }
            else {
                //this.CurrencyCode = null;
            }

        }
    }
    ClearAmounts() {
        this.LocalAmount = null;
        this.ForeignAmount = null;

    }
    SetAmountsWhenChangingAccDay() {
        this.IsAccDayChanged = false;
        this.LocalAmount = null;
        this.ForeignAmount = null;
    }
    GetExchangeRate(value: string) {
        if (this.parent.defaultCurrencyId != value) {
            this.ratesTableExtendedListService.getExchageRateByValueAndDate(this.parent.defaultCurrencyId, value, this.AccountingDate).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        if (myResponse.Result != undefined && myResponse.Result != null) {
                            this.isRateManualy = false;

                            var rate = myResponse.Result;


                            if (this.IsAccDayChanged && (this.currencyRate != rate.Rate)) {
                                this.SetAmountsWhenChangingAccDay();
                                this.currencyRate = rate.Rate;
                            }
                            else {
                                this.currencyRate = rate.Rate;
                                // Recalculate local amount
                                this.isRateCoverted = true;
                                if (this.LocalAmount) {
                                    this.CalculateForeignAmount();
                                }
                                else if (this.ForeignAmount) {
                                    this.CalculateLocalAmount();
                                }
                            }
                            console.log(">Ex. Rate: ", this.currencyRate);
                            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];

                        }
                        else {
                            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                            this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(TextCodeTranslator.Translate("Journal.O.ExchangeRateValidation") + " " + this.Line);

                            this.LocalAmount = null;
                            this.ForeignAmount = null;
                        }
                    }
                }
            });
        }
        else this.currencyRate = 1;
    }
    CalculateForeignAmount() {
        this.ForeignAmount = (this.LocalAmount / this.currencyRate);
    }
    CalculateLocalAmount() {
        this.LocalAmount = (this.ForeignAmount * this.currencyRate);
    }
    isRateCoverted: boolean = false;
    isRateManualy: boolean = false;
    isLocalEntered: boolean = false;
    isForeignEntered: boolean = false;

    // [!]
    // [!] Warning!! Any changes in one function must done in another function
    // [!]

    // SYNCRONIZED CODE WITH ForeignAmount
    get LocalAmount() { return this.JournalLinePM.LocalAmount; }
    set LocalAmount(value: number) {
        if (this.JournalLinePM.LocalAmount != value) {

            // set value
            this.JournalLinePM.LocalAmount = value;
            this.parent.CalculateTotals();

            if(this.CurrencyId ==SessionLocator.TenantPM.CurrencyId)  this.ForeignAmount= this.LocalAmount;

            if (!this.ForeignAmount && this.CurrencyId) this.GetExchangeRate(this.CurrencyId);


        }
    }

    // SYNCRONIZED CODE WITH LocalAmount
    get ForeignAmount() { return this.JournalLinePM.ForeignAmount; }
    set ForeignAmount(value: number) {
        if (this.JournalLinePM.ForeignAmount != value) {

            // set value
            this.JournalLinePM.ForeignAmount = value;
            this.parent.CalculateTotals();
            if (!this.LocalAmount && this.CurrencyId) this.GetExchangeRate(this.CurrencyId);

        }

    }


    AmountChanged(type,localAmount,foreignAmount){
        console.log("[AmountChanged] local: ", localAmount, ", foreign: ", foreignAmount);


        if (this.CurrencyId) {

            if (type == 'local')
                this.isLocalEntered = !AppTool.IsNullOrEmpty(localAmount);

            if (type == 'foreign')
                this.isForeignEntered = !AppTool.IsNullOrEmpty(foreignAmount);

            // local amount entered
            if (type == 'local' && AppTool.IsNullOrEmpty(localAmount)) {
                this.LocalAmount = AppTool.IsNullOrEmpty(localAmount) ? null : localAmount;
            }

            // foreign amount entered
            if (type == 'foreign' && AppTool.IsNullOrEmpty(foreignAmount)) {
                this.ForeignAmount = AppTool.IsNullOrEmpty(foreignAmount) ? null : foreignAmount;
            }

            // local amount entered and foreign is null
            if (type == 'local' && !this.ForeignAmount && this.currencyRate) {
                this.LocalAmount = localAmount;
                this.ForeignAmount = localAmount / this.currencyRate;
                this.isRateManualy = false;

            }
            else {
                if (this.ForeignAmount && this.LocalAmount && !this.CheckIfCurrencyIsSameAsTenantCurrency()) {
                    this.SetExchangeRateMnualy();
                }
            }
            //// foreign amount entered and local is null
            //if (type == 'foreign' && !this.isLocalEntered && this.currencyRate) {
            //    this.ForeignAmount = foreignAmount;
            //    this.LocalAmount = foreignAmount * this.currencyRate;
            //}

            // if two amounts are entered, recalculate rate
            //if (!AppTool.IsNullOrEmpty(this.ForeignAmount) && !AppTool.IsNullOrEmpty(this.LocalAmount)) {
            //    this.currencyRate = this.LocalAmount / this.ForeignAmount;
            //    this.isRateManualy = true;
            //}
        }
        this.isForeignEntered = false;
        this.isLocalEntered = false;
    //    this.isRateManualy = false;
    }
    CheckIfCurrencyIsSameAsTenantCurrency() {
        if (this.CurrencyId == SessionLocator.TenantPM.CurrencyId) return true;
    }
    SetExchangeRateMnualy() {
        //this.currencyRate = this.LocalAmount / this.ForeignAmount;
        this.isRateManualy = true;
    }
    // [!]
    // [!]

    get Reference1() { return this.JournalLinePM.Reference1; }
    set Reference1(value: string) {
        if (this.JournalLinePM.Reference1 != value) {
            this.JournalLinePM.Reference1 = value;
        }
    }

    get Reference2() { return this.JournalLinePM.Reference2; }
    set Reference2(value: string) {
        if (this.JournalLinePM.Reference2 != value) {
            this.JournalLinePM.Reference2 = value;
        }
    }

    get Reference3() { return this.JournalLinePM.Reference3; }
    set Reference3(value: string) {
        if (this.JournalLinePM.Reference3 != value) {
            this.JournalLinePM.Reference3 = value;
        }
    }

    get Notes() { return this.JournalLinePM.Notes; }
    set Notes(value: string) {
        if (this.JournalLinePM.Notes != value) {
            this.JournalLinePM.Notes = value;
        }
    }
    //#endregion

    get CreditAccountName() { return this.JournalLinePM.CreditAccountName; }
    set CreditAccountName(value: string) {
        if (this.JournalLinePM.CreditAccountName != value) {
            this.JournalLinePM.CreditAccountName = value;
        }
    }

    get DebitAccountName() { return this.JournalLinePM.DebitAccountName; }
    set DebitAccountName(value: string) {
        if (this.JournalLinePM.DebitAccountName != value) {
            this.JournalLinePM.DebitAccountName = value;
        }
    }

    get DebitAccountNumber() { return this.JournalLinePM.DebitAccountNumber; }
    set DebitAccountNumber(value: string) {
        if (this.JournalLinePM.DebitAccountNumber != value) {
            this.JournalLinePM.DebitAccountNumber = value;
        }
    }
    get DebitAccountEnglishName() { return this.JournalLinePM.DebitAccountEnglishName; }
    set DebitAccountEnglishName(value: string) {
        if (this.JournalLinePM.DebitAccountEnglishName != value) {
            this.JournalLinePM.DebitAccountEnglishName = value;
        }
    }
    get CreditAccountEnglishName() { return this.JournalLinePM.CreditAccountEnglishName; }
    set CreditAccountEnglishName(value: string) {
        if (this.JournalLinePM.CreditAccountEnglishName != value) {
            this.JournalLinePM.CreditAccountEnglishName = value;
        }
    }
    get CreditAccountNumber() { return this.JournalLinePM.CreditAccountNumber; }
    set CreditAccountNumber(value: string) {
        if (this.JournalLinePM.CreditAccountNumber != value) {
            this.JournalLinePM.CreditAccountNumber = value;
        }
    }


    get CurrencyCode() { return this.JournalLinePM.CurrencyCode; }
    set CurrencyCode(value: string) {
        if (this.JournalLinePM.CurrencyCode != value) {
            this.JournalLinePM.CurrencyCode = value;
        }
    }

    creditAccount: GLAccountPM;
    get CreditAccount() { return this.creditAccount; }
    set CreditAccount(value: GLAccountPM) {
        //console.log("-creditAccount-");
        if (this.creditAccount != value) {
            this.creditAccount = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.CreditAccountName = value.LocalName;

            this.SetCurrencyForSingleAccount(value, ActionCode.Credit.toString(), ActionCode.DebitAndCredit.toString());
            if (!AppTool.IsNullOrEmpty(this.Currency)) {
                this.SplittedCheck();
            }


        } else {
            this.CreditAccountName = null;
            this.CreditAccountId = null;
        }
        this.SetForeignAmountEnabilityForSingleCurrencyAccount();
    }

    debitAccount: GLAccountPM;
    get DebitAccount() { return this.debitAccount; }
    set DebitAccount(value: GLAccountPM) {
        //console.log("-debitAccount-");
        if (this.debitAccount != value) {
            this.debitAccount = value;
        }
        if (!AppTool.IsNullOrEmpty(value)) {
            this.DebitAccountName = value.LocalName;
            this.SetCurrencyForSingleAccount(value, ActionCode.Debit.toString(), ActionCode.DebitAndCredit.toString());

            if (!AppTool.IsNullOrEmpty(this.Currency)) {
                this.SplittedCheck();
            }
        } else {
            this.DebitAccountName = null;
            this.DebitAccountId = null;
        }
        this.SetForeignAmountEnabilityForSingleCurrencyAccount();
    }
    SetCurrencyForSingleAccount(account:GLAccountPM,actionCode1:string ,actionCode2:string) {
        if (!account.IsMultiCurrency) {
            if (this.ActionCode == actionCode1 || this.ActionCode == actionCode2) {
                this.CurrencyId = account.CurrencyId;
                this.CurrencyCode = account.CurrencyCode;
            }
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, false);

        } else {
            this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);

        }
    }
    SetForeignAmountEnabilityForSingleCurrencyAccount() {
        if (this.CurrencyId != SessionLocator.TenantPM.CurrencyId) {
            this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, true);
            this.enableForeighAmountField = true;
        }
        else {
            this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, false);
            this.enableForeighAmountField = false;
        }
    }
    accDay: number;
    get AccDay() {
        return this.accDay
    }
    set AccDay(value: number) {
        if (this.accDay != value) {
            this.accDay = value;

            //1- check parent accounting date if changed?
            if (this.parent.AccountingDate != this.AccountingDate) {
              //  this.AccountingDate = this.parent.AccountingDate;
            }


        }
    }
    ValidateAccDay() {
        this.IsAccDayValid(this.date, this.AccDay);
        var valid = JournalValidator.IsAccDayValid(this.date, this.AccDay);

        this.isValid = valid;
        if (valid) {
            this.UIProperties.SetValidity("AccDay", this.ObjectTableName, true, "valid");
        } else {
            this.UIProperties.SetValidity("AccDay", this.ObjectTableName, false, this.accountingDayMustBeInRange);
        }
    }
    date: Date;
    SetAccountingDate() {
        this.date = new Date(this.AccountingDate.toString()); // somtimes this.AccountingDate contains string date o.O

        var newDate: Date = new Date();
        newDate.setUTCFullYear(this.date.getFullYear());
        newDate.setUTCMonth(this.date.getMonth());
        newDate.setUTCDate(this.date.getDate());
        newDate.setUTCHours(0);
        newDate.setUTCMinutes(0);
        newDate.setUTCSeconds(0);
        newDate.setUTCMilliseconds(0);
        this.date = newDate;
        this.ValidateAccDay();
        this.AccountingDate.setUTCDate(this.date.getDate());
    }
    AccDay_LostFocus(date: Date) {

        this.IsAccDayChanged = true;
        if (this.CurrencyId)
            this.GetExchangeRate(this.CurrencyId);
        this.SetAccountingDate();

    }
    IsAccDayChanged: boolean;
    IsAccDayValid(date: Date, day: number) {
        if (day > 0 && day < 32) {
            var lastDayOfMonth = this.lastDay(date.getFullYear(), date.getMonth());
            if (day > lastDayOfMonth) {
                //error
                this.UIProperties.SetValidity("AccDay", this.ObjectTableName, false, this.accountingDayMustBeInRange);
                this.isValid = false;
                return false;
                //var t = setTimeout(() => {
                //    this.AccDay = value;
                //});
            } else {
                this.UIProperties.SetValidity("AccDay", this.ObjectTableName, true, "valid");
                this.AccountingDate = new Date(date.setDate(day));
                this.isValid = true;

                return true;

            }
        } else {
            //error
            this.UIProperties.SetValidity("AccDay", this.ObjectTableName, false, this.accountingDayMustBeInRange);
            this.isValid = false;
            //var t = setTimeout(() => {
            //    this.AccDay = value;
            //});
            return false;
        }

    }



    private timerToken: any;
    private isMouseIn: boolean = false;
    OnMouseOver() {
        this.isMouseIn = true;
        if (this.currencyRate) {
            this.timerToken = setTimeout(() => {
                var item = document.getElementById("tooltip-" + this.SessionIndex + this.Line);
                if (AppTool.IsNullOrEmpty(item))
                    return;
                var itemRect = item.getBoundingClientRect();
                if (this.isMouseIn) {
                    document.getElementById("tooltip-body-" + this.SessionIndex + this.Line).style.position = "fixed";
                    document.getElementById("tooltip-body-" + this.SessionIndex + this.Line).style.top = (itemRect.top - 35) + 'px';
                    document.getElementById("tooltip-body-" + this.SessionIndex + this.Line).style.left = (itemRect.left + 60) + 'px';
                    document.getElementById("tooltip-body-" + this.SessionIndex + this.Line).style.visibility = "visible";

                    this.timerToken = setTimeout(() => {
                        document.getElementById("tooltip-body-" + this.SessionIndex + this.Line).style.visibility = "hidden";

                    }, 2500);
                }

            }, 700);
        }
    }
    OnMouseLeave() {
        this.isMouseIn = false;
        if (this.currencyRate) {

            this.timerToken = setTimeout(() => {
                document.getElementById("tooltip-body-" + this.SessionIndex + this.Line).style.visibility = "hidden";

            }, 400);

        }

    }

    GetManualyRate() {
        if (this.LocalAmount && this.ForeignAmount) {

            var myResult = (this.LocalAmount / this.ForeignAmount).toFixed(2).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

            return myResult;
        }
        else {
            return null;
        }
    }
    isRateMoreThanFivePerc() {
        this.parent.EntityWarningsList = [];
        if (this.LocalAmount && this.ForeignAmount) {

            var userExchageRate = (this.LocalAmount / this.ForeignAmount);
            this.parent.JournalLines.Collection.map((line)=> {
                if(!this.parent.Approved && (line.currencyRate - (line.LocalAmount / line.ForeignAmount)) > 0.05) {
                    this.parent.EntityWarningsList.push(TextCodeTranslator.Translate("Journal.O.DifferenceExchangeRate"));
                }
            })
            return userExchageRate && Math.abs(this.currencyRate - userExchageRate) > 0.05;
        }
        else {
            return null;
        }
    }
    enableForeighAmountField: boolean = true;
    currency: CurrencyList;
    get Currency() { return this.currency; }
    set Currency(value: CurrencyList) {
        if (this.currency != value) {
            this.currency = value;
            if (!AppTool.IsNullOrEmpty(value)) {
                this.CurrencyCode = value.Code;
                this.CurrencyId = value.Id;
                this.enableForeighAmountField = this.Currency.Id !=SessionLocator.TenantPM.CurrencyId;
                 if(this.Currency.Id ==SessionLocator.TenantPM.CurrencyId) {
                     this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, false);
                    this.ForeignAmount = this.LocalAmount;
                    }
                 else this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, true);

            }
            else {
                this.SetCurrencyNull();
            }

            this.SplittedCheck();
        }
    }
    SetCurrencyNull() {
        this.CurrencyCode = null;
        this.CurrencyId = null;
        this.enableForeighAmountField = true;
        this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, true);
    }
    GetGLAccountCurency(isCredit: boolean, isDebit: boolean, currencyId) {

        // credit and debit
        if (isCredit && isDebit) {

            this.GetGLAccountCurency(true, false, currencyId);
            this.GetGLAccountCurency(false, true, currencyId);

            // credit or debit
        } else {
            this._GLAccountExtendedListService.GetAccountCurrencies(isCredit ? this.CreditAccountId : this.DebitAccountId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (myResponse.HasError) {
                        console.error(myResponse.ErrorsArray);
                    }
                    else {
                        var currencies = myResponse.Result;
                        if (!AppTool.IsNullOrEmpty(currencies)) {
                            var currency = currencies.find(d => d.CurrencyId == currencyId);
                            if (currency != null) {

                                //
                                // Task 41399: Journal Line--> No need for the validation in case the user chose Multi currency GLAccount
                                //
                                ////// Show prompt
                                ////var confirmWindow = new ConfirmWindow();
                                ////confirmWindow.YesButtonText = TextCodeTranslator.Translate("Accounting.General.B.OK");//"Ok";
                                ////confirmWindow.NoButtonText = TextCodeTranslator.Translate("Accounting.General.B.Cancel");//"Cancel";
                                ////confirmWindow.Width = 500;
                                //////confirmWindow.Show("There is a splitted GLAccounts for the chosen multi currency " + (isCredit ? 'Credit' : 'Debit')
                                //////    + " Account, the transactions will be registered in the Splitted By Currency GLAccount ");
                                ////if (isCredit)
                                ////    confirmWindow.Show(TextCodeTranslator.Translate("Accounting.General.O.SplittedAccountMsgCredit"));
                                ////else
                                ////    confirmWindow.Show(TextCodeTranslator.Translate("Accounting.General.O.SplittedAccountMsgDebit"));

                                ////console.log("DetectChanges");
                                //////this.parent.DetectChanges();
                                ////confirmWindow.WindowClosed.subscribe((event: any) => {
                                ////    if (confirmWindow.Yes) {
                                ////        if (isCredit) {
                                ////            this.CreditAccountId = currency.GLAccountId;
                                ////            this.CreditAccountName = currency.GLAccountName;
                                ////        }
                                ////        if (isDebit) {
                                ////            this.DebitAccountId = currency.GLAccountId;
                                ////            this.DebitAccountName = currency.GLAccountName;
                                ////        }
                                ////    } else if (confirmWindow.No) {
                                ////        if (isCredit) this.CreditAccount = null;
                                ////        if (isDebit) this.DebitAccount = null;
                                ////    }
                                ////});

                            } else {
                                return null;
                            }

                        }
                    }
                }
            });
        }


    }

    ShowPrompt() {

    }

    public SplittedCheck() {

        var currency = this.Currency;
        if (!AppTool.IsNullOrEmpty(currency)) {
            var account;
            var checkTwoAccount = false;
            if (this.ActionCode == "1") { // Credit
                if (AppTool.IsNullOrEmpty(this.CreditAccount) || !this.CreditAccount.IsMultiCurrency) {
                    return;
                }
                this.GetGLAccountCurency(true, false, currency.Id);

            } else if (this.ActionCode == "2") { // Debit
                if (AppTool.IsNullOrEmpty(this.DebitAccount) || !this.DebitAccount.IsMultiCurrency) {
                    return;
                }
                this.GetGLAccountCurency(false, true, currency.Id);

            } else if (this.ActionCode == "3" || this.ActionCode == "4") { // Credit and Debit
                if (AppTool.IsNullOrEmpty(this.CreditAccount) || !this.CreditAccount.IsMultiCurrency) {
                    return;
                }
                if (AppTool.IsNullOrEmpty(this.DebitAccount) || !this.DebitAccount.IsMultiCurrency) {
                    return;
                }
                this.GetGLAccountCurency(true, true, currency.Id);
            }
        } else {
            //console.warn("SplittedCheck: no currency!");
        }

    }

    lastDay(year, month) {
        return new Date(year, month + 1, 0).getDate();
    }

    //#region GLAccount HyberLink

    GLAccountHyperlinkClicked() {
        GLAccountSecurityLevelService.OpenGLAccountEditWindow(this.CreditAccountId);
    }

    DebitAccountHyperlinkClicked() {
        GLAccountSecurityLevelService.OpenGLAccountEditWindow(this.DebitAccountId);
    }

    public EditEntity(objectTableName: string, entityId: string, windowTitle: string, defaultSelectedTabCode: string) {


        var editWindow = new LogitudeWindow();

        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;

        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe((res:any) => {



        });

    }


    //#endregion
}
enum ActionCode {
    Debit = 2,
    Credit = 1,
    DebitAndCredit=3
}

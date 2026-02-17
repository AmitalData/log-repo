import { BankDepositLineListService } from './../../../Services/StandardLists/BankDepositLineListService';
import { CashBookExtendedPMService } from './../../../Services/ExtendedPMs/CashBookExtendedPMService';
import { filter } from 'rxjs/operators';
import { CashBookLineListService } from './../../../Services/StandardLists/CashBookLineListService';
import { CashBookListService } from './../../../Services/StandardLists/CashBookListService';
import { ObservableCollection } from './../../../../Infrastructure/Utilities/ObservableCollection';
import { ARPaymentChequeList } from './../../../EntityLists/ARPaymentChequeList';
import { ARPaymentChequeListService } from './../../../Services/StandardLists/ARPaymentChequeListService';
import {ChangeDetectorRef, Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {BankDepositPM} from '../../../EntityPMs/BankDepositPM';
import {BankDepositLinePM} from '../../../EntityPMs/BankDepositLinePM';
import {CashBookLinePM} from '../../../EntityPMs/CashBookLinePM';
import {CashBookPM} from '../../../EntityPMs/CashBookPM';
import {CurrencyPM} from '../../../../Common/EntityPMs/CurrencyPM';
import {CashBookPMService} from '../../../Services/StandardPMs/CashBookPMService';
import {RatesTableExtendedListService } from '../../../../Infrastructure/Services/ExtendedLists/RatesTableExtendedListService';
import {CurrencyListService} from '../../../../Common/Services/StandardLists/CurrencyListService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {BankDepositExtendedPMService } from '../../../Services/ExtendedPMs/BankDepositExtendedPMService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { BankDepositPMService } from '../../../Services/StandardPMs/BankDepositPMService';
import { CashbookChequesCounter } from '../../../DataContracts/CashbookChequesCounter';
import { CashBookList } from '../../../EntityLists/CashBookList';
import { QueryColumnPM } from 'Infrastructure/EntityPMs/QueryColumnPM';
import { LogitudeGridExportToExcelComponent } from 'Common/Components/LogitudeGridExportToExcel/LogitudeGridExportToExcelComponent';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';

@Component({

    templateUrl: './BankDepositDetailsTabComponent.html',
})

export class BankDepositDetailsTabComponent extends BaseComponent {
  public showLocal: any;

    public EntityPM: BankDepositPM = null;
    public ObjectTableName = "BankDeposit";
    public DataContext = this;
    public CashBookTotal = 0;
    public SelectedTotal = 0;
    public NoCashBookRows: boolean = false;
    public IsLinesSelection: boolean = false;
    public IsNewDepositMode: boolean = false;
    public EnableExportChequeDepositsToExcell: boolean = false;
    searchText: string = "";
    private ChequePairValidationDays = 183;
    private PeriodForChequePairValidation = this.ChequePairValidationDays * 24 * 60 * 60 * 1000;

    CashBookPM: CashBookPM;
    // BankDepositLines: BankDepositLinePM[];
    OriginalCashbookLines: ObservableCollection = new ObservableCollection([]);
    CashbookLines: ObservableCollection = new ObservableCollection([]);
    SelectedCashbookLines: ObservableCollection = new ObservableCollection([]);
    public IsOriginalCashbookLinesNeedeToFill: boolean = true;

    BankDepositLines: ObservableCollection = new ObservableCollection([]);
    public isRTL: boolean = false;
    public IsReturnChequeEnabled: boolean = false;
    txt_NewDeposit: string = TextCodeTranslator.Translate("Accounting.General.O.NewDeposit");
    txt_DepositDetails: string = TextCodeTranslator.Translate("Accounting.General.O.DepositDetails");
    private CurrentSession = SessionLocator.SelectedSession;

    public LogitudeGridExportToExcelComponent:LogitudeGridExportToExcelComponent= new LogitudeGridExportToExcelComponent();

    public showLocals: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    public QueryColumns: QueryColumnPM[] = [];

    BankDepositPMService: BankDepositPMService = new BankDepositPMService();
    cashBookPMService: CashBookPMService = new CashBookPMService();
    cashBookListService: CashBookListService = new CashBookListService();
    cashBookLineListService: CashBookLineListService = new CashBookLineListService();
    bankDepositLineListService: BankDepositLineListService = new BankDepositLineListService();
    ratesTableExtendedListService: RatesTableExtendedListService = new RatesTableExtendedListService();
    currencyListService: CurrencyListService = new CurrencyListService();
    bankDepositExtendedPMService: BankDepositExtendedPMService = new BankDepositExtendedPMService();
    arPaymentChequeListService: ARPaymentChequeListService = new ARPaymentChequeListService();
    cashBookExtendedPMService: CashBookExtendedPMService = new CashBookExtendedPMService();

    constructor(private entityArgs: EntityArgs,private cdr: ChangeDetectorRef) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.EntityPM = entityArgs.EntityPM;

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id))
            this.SetSelectionMode();
        else
            this.SetViewMode();


        this.Listen();
        this.GetDefaultValues();
        this.buildQueryColumns();
        if (FeatureLocator.HasFeaturePermession("BankDeposit", "ExpChDepToExcell")) {
            this.EnableExportChequeDepositsToExcell = true;
        }
    }

    private SetViewMode()
    {
        this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = false;
        this.GetDepositLines();
        // this.BankDepositLines = new ObservableCollection(this.EntityPM.BankDepositLines);
        this.SetUIProperty();
        this.CalculateDepositLinesTotal();
        console.log("Deposit: ", this.EntityPM);
    }

    private SetSelectionMode()
    {
        this.IsLinesSelection = true;
        this.IsNewDepositMode = true;
        this.GetCashBook();
        this.GetChequesCounter();


        this.BankDepositLines = new ObservableCollection([]);
    }

    GetDepositLines(){

        this.CurrentSession.StartBusyIndicator("Load Deposit Lines ...");
        this.bankDepositLineListService.getByFilters(this.GetDeposiutAPIFilters()).subscribe((response: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            console.log("[bankDepositLineListService]", response);

            if (!response.HasError) {
                this.BankDepositLines = new ObservableCollection(response.Result);

            }
            else {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = response.ErrorsArray;
            }
        });
    }

    private GetDeposiutAPIFilters()
    {
        var depositAPIFilters = new ApiQueryFilters();
        depositAPIFilters.GetAll = true;
        depositAPIFilters.addAdditionalFilter("DepositId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");

        if(this.searchText)
            depositAPIFilters.addAdditionalFilter("SearchFields", this.searchText, null, null, "Contains", false, false, false, "string");


        return depositAPIFilters;
    }

    private CalculateDepositLinesTotal()
    {
        for (let line2 of this.BankDepositLines.Collection) {
            this.SelectedTotal += line2.ForeignAmount == null ? 0 : line2.ForeignAmount;
        }
    }

    Listen(){
        // Save
        this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            if (isSaveSuccess) {
                console.log("Deposited Success", this.EntityPM);
                this.RefreshEntity();
            }
        });

        // Reload
        this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
            if (isLoadSuccess) {
                this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                 console.log("Entity Reloaded");
                 console.log("Deposited Success", this.EntityPM);
                 this.RedrawScreen(false);
            }
            else{
                this.MarkRelatedCheques();              
                

            }
        });
    }
    MarkRelatedCheques() {    
        const selectedLines = this.CashbookLines.Collection.filter(line => line.IsSelected);
    
        for (const selected of selectedLines) {
            const { Bank, Branch, AccountNumber, ChequeNumber } = selected;
            const selectedDueDate = new Date(selected.DueDate).getTime();
            const startRange = selectedDueDate - this.PeriodForChequePairValidation;
            const endRange = selectedDueDate + this.PeriodForChequePairValidation;
        
            const relatedLines = this.CashbookLines.Collection.filter(line =>
                !line.IsSelected &&
                line.Bank === Bank &&
                line.Branch === Branch &&
                line.AccountNumber === AccountNumber &&
                line.ChequeNumber === ChequeNumber &&
                new Date(line.DueDate).getTime() >= startRange &&
                new Date(line.DueDate).getTime() <= endRange
            );
    
            for (const line of relatedLines) {
                this.LineSelection(line, true, false);
            }
        }
    
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    }    
    RefreshEntity(){
        this.CurrentSession.CurrentEditComponent.EntityId = this.EntityPM.Id; // Set entity id in edit component to reload
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM(); // reloading
    }

    RedrawScreen(getDepositLines = true) {

        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        console.log("Deposit Reloaded: ", this.EntityPM);

        this.IsLinesSelection = !this.EntityPM.Id;


        // Redraw UI
        this.IsLinesSelection = false;
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        if(getDepositLines) {
            this.GetDepositLines();
        }

        this.CashbookLines = new ObservableCollection([]);
        this.SelectedCashbookLines = new ObservableCollection([]);
        this.OriginalCashbookLines = new ObservableCollection([]);

        // this.BankDepositLines = new ObservableCollection(this.EntityPM.BankDepositLines);

        this.CalculateTotals();

        this.GetCashBook();
        this.GetChequesCounter();
        this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = false;
        this.SetUIProperty();
    }

    //#region Alert
    public showAlert: boolean = false;
    ShowAlert() {
        this.showAlert = true;
        //this.timerToken = setTimeout(() => { // Turn off after 5 second
        //    this.showAlert = false;
        //}, 5000);
    }
    CloseAlert() {
        this.showAlert = false;
    }
    //#endregion

    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    this.showAlert = false;
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal' });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                    });
                });
        }
    }

    OpenARPayment(id) {
        // open ARPayment screen
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'ARPayment' });
                });
        } else {
            console.warn("No ID for this payment!", id);
        }
    }

    //#region Properties
    get DepositNumber() { return this.EntityPM.DepositNumber; }
    set DepositNumber(value: number) {
        if (this.EntityPM.DepositNumber != value) {
            this.EntityPM.DepositNumber = value;
        }
    }

    get DepositDate() { return this.EntityPM.DepositDate; }
    set DepositDate(value: Date) {
        if (this.EntityPM.DepositDate != value) {
            this.EntityPM.DepositDate = value;
        }
    }

    get DepositCurrencyId() { return this.EntityPM.DepositCurrencyId; }
    set DepositCurrencyId(value: string) {
        if (this.EntityPM.DepositCurrencyId != value) {
            this.EntityPM.DepositCurrencyId = value;

            this.GetCurrencyRate();
        }
    }

    get LocalDepositAmount() { return this.EntityPM.LocalDepositAmount; }
    set LocalDepositAmount(value: number) {
        if (this.EntityPM.LocalDepositAmount != value) {
            this.EntityPM.LocalDepositAmount = value;

            //if (this.EntityPM.IsCashDeposit && value != null) {
            //    if (value > this.CashBookPM.TotalAmount) {
            //        this.UIProperties.SetValidity("LocalDepositAmount", this.ObjectTableName, false, "Amount must be less than Cashbook total");
            //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push("Deposit Amount must be less than Cashbook total");
            //        this.CurrentSession.CurrentEditComponent.IsEditValid = false;
            //    } else {
            //        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            //        this.UIProperties.SetValidity("LocalDepositAmount", this.ObjectTableName, true, "");
            //        this.CurrentSession.CurrentEditComponent.IsEditValid = true;
            //        this.CalculateForeign(value);
            //    }
            //}
            //if (this.IsLinesSelection && value != null) {
            //    this.UIProperties.SetRequired("LocalDepositAmount", this.ObjectTableName, false);
            //}
        }
    }

    get ForeignAmount() { return this.EntityPM.ForeignAmount; }
    set ForeignAmount(value: number) {
        if (this.EntityPM.ForeignAmount != value) {
            this.EntityPM.ForeignAmount = value;

            //
            if (this.EntityPM.IsCashDeposit && value != null) {
                // canceled validation
                // if (value > this.CashBookPM.TotalAmount) {
                    // this.UIProperties.SetValidity("ForeignAmount", this.ObjectTableName, false, "Amount must be less than Cashbook total");
                    //this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                    //this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push("Deposit Amount must be less than Cashbook total");
                    //this.CurrentSession.CurrentEditComponent.IsEditValid = false;
                // } else {
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                    this.UIProperties.SetValidity("ForeignAmount", this.ObjectTableName, true, "");
                    //this.CurrentSession.CurrentEditComponent.IsEditValid = true;
                    this.CalculateLocal(value);
                // }
            }
            if (this.IsLinesSelection && value != null) {
                this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, false);
            }
        }
    }

    get CashBookId() { return this.EntityPM.CashBookId; }
    set CashBookId(value: string) {
        if (this.EntityPM.CashBookId != value) {
            this.EntityPM.CashBookId = value;

        }
    }

    get DepositBankAccountId() { return this.EntityPM.DepositBankAccountId; }
    set DepositBankAccountId(value: string) {
        if (this.EntityPM.DepositBankAccountId != value) {
            this.EntityPM.DepositBankAccountId = value;
        }
    }

    get _CashbookTotal() { return this.CashBookTotal }
    set _CashbookTotal(value: number) {
        if (this.CashBookTotal != value) {
            this.CashBookTotal = value;

        }
    }


    isAllSelected: boolean = false;
    get IsAllSelected() { return this.isAllSelected; }
    set IsAllSelected(value: boolean) {
        if (this.isAllSelected != value) {
            this.isAllSelected = value;

        }
    }

    tenantCurrency: CurrencyPM;
    get TenantCurrency() { return this.tenantCurrency }
    set TenantCurrency(value: CurrencyPM) {
        if (this.tenantCurrency != value) {
            this.tenantCurrency = value;

        }
    }
    //#endregion

    SetUIProperty() {
        if (this.EntityPM.IsCashDeposit) {
            this.UIProperties.SetRequired("LocalDepositAmount", this.ObjectTableName, this.IsLinesSelection ? true : false);
            this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, this.IsLinesSelection ? true : false);

            this.UIProperties.SetEnabled("DepositBankAccountId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CashBookId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("_CashbookTotal", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("LocalDepositAmount", this.ObjectTableName, this.IsLinesSelection ? true : false);
            this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, this.IsLinesSelection ? true : false);

        } else {
            this.UIProperties.SetRequired("LocalDepositAmount", this.ObjectTableName, false);
            this.UIProperties.SetRequired("ForeignAmount", this.ObjectTableName, false);

            this.UIProperties.SetEnabled("LocalDepositAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ForeignAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CashBookId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DepositBankAccountId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("_CashbookTotal", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DepositCurrencyId", this.ObjectTableName, false);
        }
    }
    GetAmountHeader(){
        var msg = TextCodeTranslator.Translate("CashBookLine.F.LocalAmount");
        return msg + " (" + this.tenantCurrencyCode + ")";
    }
    //#region Filter Methods
    public FilterSelectedValue: string = 'cash';
    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue != itemValue) {
            
            // this.FilterLines();
            this.GetCashbookLinesAccordingToFilter(itemValue);
        }
    }

    private GetCashbookLinesAccordingToFilter(newValue: string) {
        this.IsOriginalCashbookLinesNeedeToFill = true;
        if (this.GetNumberOfSelectedLines() > 0) {
            this.ShowConfirmMessageToToggleBetweenCashAndPostdated(newValue);
        }
        else {
            this.FilterSelectedValue = newValue;
            this.GetCashbookLines();
        }
    }

    private GetNumberOfSelectedLines(): number {
        var numberOfSelectedLines = 0;
        for (let line of this.CashbookLines.Collection) {
            if (line.IsSelected)
                numberOfSelectedLines++;
        }
        return numberOfSelectedLines;
    }

    ShowConfirmMessageToToggleBetweenCashAndPostdated(newValue: string) {
        var myConfirmWindow = new ConfirmWindow();
        myConfirmWindow.Width = 400;
        myConfirmWindow.Show(TextCodeTranslator.Translate("Cashbook.O.ConfirmUncheckelines"));
        myConfirmWindow.WindowClosed.subscribe(event => {
            if (myConfirmWindow.Yes) {
                this.FilterSelectedValue = newValue;
                this.GetCashbookLines();
                this.IsAllSelected = false;
                this.SelectedCashbookLines.Clear();
            }
     
        });
    }

    private timerToken: any;
    TextChanged(searchtext) {

        // Deposited cheque
        if (!this.EntityPM.IsCashDeposit) {
            this.timerToken = setTimeout(() => {
                this.searchText = searchtext;
                this.FilterChequeDeposits();
            }, 500);
        }

    }
    FilterChequeDeposits() {
        if (!this.IsNewDepositMode) {
            this.GetDepositLines();
        }
        else {
            this.GetCashbookLines();
        }
        // if (AppTool.IsNullOrEmpty(this.searchText)) {
        //     this.BankDepositLines = new ObservableCollection(this.EntityPM.BankDepositLines);


        // } else {
        //     var filteredDepositedCheques = [];
        //     filteredDepositedCheques = this.EntityPM.BankDepositLines.filter(d => d.SearchFields.toLowerCase().includes(this.searchText.toLowerCase()));
        //     // this.BankDepositLines = filteredDepositedCheques;

        //     this.BankDepositLines.Clear();
        //     this.BankDepositLines = new ObservableCollection(filteredDepositedCheques);

        // }


    }
    //#endregion

    //#region Get Methods
    GetCashBook()
    {
        this.CurrentSession.StartBusyIndicator("Loading Cashbook Data ...");

        //this._CashBookPMService.get(this.EntityPM.CashBookId)
        this.cashBookListService.getSingle(this.EntityPM.CashBookId)
            .subscribe((myResult:any) =>
            {
                this.CurrentSession.StopBusyIndicator();

                var myResponse: ServiceResponse = myResult;

                if (!myResponse.HasError) {
                    this.CashBookPM = myResponse.Result;
                    console.log("Cashbook: ", this.CashBookPM);

                    if (this.IsLinesSelection) {
                        this.EntityPM.IsCashDeposit = this.CashBookPM.CashBookTypeCode == "1";


                        if (this.CashBookPM.CashBookTypeCode == "1") // 1- Cash
                        {
                            this.SetUIProperty();

                            this.CalculateTotals();

                            if (this.isCurrencyRateLoaded)
                                this.CalculateLocal(this.ForeignAmount);

                            this.UIProperties.SetValidity("LocalDepositAmount", this.ObjectTableName, true, "");
                            this.UIProperties.SetValidity("ForeignAmount", this.ObjectTableName, true, "");
                        }
                        else {  // Cheque

                            this.GetCashbookLines();
                           
                            this.SetUIProperty();
                        }


                    } else {
                        this._CashbookTotal = this.CashBookPM.TotalAmount;
                        this.CalculateTotals();

                    }
                }
            }
                , error =>
                {
                });


    }

    tenantCurrencyCode: string = "";
    private GetCashbookLines()
    {
        
        // this.BankDepositLines.Clear();
        // this.EntityPM.BankDepositLines = [];


        this.cashBookLineListService.getByFilters(this.GetCashbookLinesAPIFilters()).subscribe((response: ServiceResponse) => {
            var result = response.Result;
            console.log("CashBookLineListService", result);

            this.CashbookLines = new ObservableCollection([]);
            this.CashbookLines.InsertCollection(result);

            // this.CashBookLines = result;

            // this.UpdateFiltersCounts();

            if (this.CashbookLines.Length > 0) {
                this.NoCashBookRows = false;
            } else {
                this.NoCashBookRows = true;
            }

            if (this.IsOriginalCashbookLinesNeedeToFill || this.OriginalCashbookLines.Length == 0) {
                this.OriginalCashbookLines = this.CashbookLines;
                this.IsOriginalCashbookLinesNeedeToFill = false;
            }

            this.SetSelectedCashbookLines();
            this.CheckIfOneOfTheOriginalLineUnChecked();
            this.CalculateTotals();

        });
    }

    private SetSelectedCashbookLines() {
        for (let line of this.SelectedCashbookLines.Collection) {
            var CashbookLine = this.CashbookLines.Collection.filter(a => a.CashBookId == line.CashBookId && a.ARPChequeId == line.ARPChequeId)[0];
            if (CashbookLine != null) {
                this.CashbookLines.Collection.filter(a => a.CashBookId == line.CashBookId && a.ARPChequeId == line.ARPChequeId)[0].IsSelected = true;
                this.PushBankDeposit(CashbookLine);
            }
        }
    }

    private CheckIfOneOfTheOriginalLineUnChecked() {
        for (let line of this.CashbookLines.Collection) {
            var CashbookLine = this.OriginalCashbookLines.Collection.filter(a => a.CashBookId == line.CashBookId && a.ARPChequeId == line.ARPChequeId)[0];
            if (CashbookLine.IsSelected == false || CashbookLine.IsSelected == null)
                this.isAllSelected = false;
            if (CashbookLine.IsSelected != line.IsSelected && line.IsSelected != null ) {
                line.IsSelected = CashbookLine.IsSelected;
                if (CashbookLine.IsSelected == true)
                    this.SelectedCashbookLines.Collection.push(line);
            }
        }
    }

 
    private GetCashbookLinesAPIFilters()
    {
        var linesQueryFilters = new ApiQueryFilters();
        linesQueryFilters.GetAll = true;
        linesQueryFilters.addAdditionalFilter("CashBookId", this.EntityPM.CashBookId, null, null, "Equals", false, false, false, "string");
        linesQueryFilters.addAdditionalFilter("IsDeposited", false, null, null, "Equals", true, false, false, "boolean");
        linesQueryFilters.addAdditionalFilter("ARPChequeStatusCode", "1,4", null, null, "InListExact",false , false, false, "string");
        // linesQueryFilters.addAdditionalFilter("ARPChequeStatusCode", "5", null, null, "NotEqual",false , false, false, "string");

        if (this.searchText)
            linesQueryFilters.addAdditionalFilter("SearchFields", this.searchText, null, null, "Contains", false, false, false, "string");

        linesQueryFilters.AdditionalFilters.push(this.GetDueDateFilter());


        return linesQueryFilters;
    }

    private GetDueDateFilter()
    {
        var todayDate = DateTool.GetCurrentDateTimeAsUtc();

        if (this.FilterSelectedValue == 'postdated')
            var filter: FilterItem = new FilterItem("DueDate", todayDate, null, null, "LargerThan", false, false, false, "date", false);
        else if (this.FilterSelectedValue == 'cash')
            var filter: FilterItem = new FilterItem("DueDate", todayDate, null, null, "LessThanOrEqual", false, false, false, "date", false);
        return filter;
    }

    GetDefaultValues() {
        // Tenant currency
        var defaultCurrencyId: string = SessionLocator.TenantPM.CurrencyId;
        this.tenantCurrencyCode = SessionLocator.TenantPM.CurrencyCode
        this.currencyListService.getSingle(defaultCurrencyId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.TenantCurrency = myResponse.Result;
                    console.log(">>Tenant Currency: ", myResponse.Result);

                    this.GetCurrencyRate();
                }
            }
        });
    }

    public currencyRate: number = 1;
    isCurrencyRateLoaded: boolean = false;
    GetCurrencyRate() {
        if (this.IsLinesSelection) {
            if (this.TenantCurrency.Id == this.DepositCurrencyId)
            {
                // local currecny
                this.currencyRate = 1;
                this.CalculateLocal(this.EntityPM.ForeignAmount);
                this.isCurrencyRateLoaded = true;

            }
            else
            {
                this.ratesTableExtendedListService.getClosestRate(this.TenantCurrency.Id, this.DepositCurrencyId).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            if (myResponse.Result != undefined && myResponse.Result != null) {

                                var rate = myResponse.Result;
                                this.currencyRate = rate.Rate;
                                this.isCurrencyRateLoaded = true;
                                //this.EntityPM.ForeignAmount = (this.SelectedTotal * this.currencyRate);
                                //this.CalculateForeign(this.EntityPM.LocalDepositAmount);
                                this.CalculateLocal(this.EntityPM.ForeignAmount);

                                console.log(">Exchange Rate for " + this.TenantCurrency.Code + " : ", this.currencyRate);

                            } else {

                                this.CurrentSession.CurrentEditComponent.IsEditValid = false;
                                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                                this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.O.NoExchangeRateForLocalCurrency"));
                                //console.warn("The selected currency does not have Exchange Rate!");
                            }
                        }
                    }
                });
            }
        }

    }
    //#endregion

    CalculateForeign(local) {
        if (!AppTool.IsNullOrEmpty(local) && !AppTool.IsNullOrEmpty(this.currencyRate)) {
            this.EntityPM.ForeignAmount = +(local / this.currencyRate).toFixed(2);
        } else {
            console.warn("No value or rate to caclulate forigen amount!");
        }
    }

    CalculateLocal(foriegn) {
        if (!AppTool.IsNullOrEmpty(foriegn) && !AppTool.IsNullOrEmpty(this.currencyRate)) {
            this.EntityPM.LocalDepositAmount = +(foriegn * this.currencyRate).toFixed(2);
        } else {
            console.warn("No value or rate to caclulate local amount!");
        }
    }

    CalculateTotals() {
        this.CashBookTotal = 0;

        if (this.CashBookPM.CashBookTypeCode == "1")
        {
            this.CashBookTotal = this.CashBookPM.TotalAmount;
            this._CashbookTotal = this.CashBookPM.TotalAmount;
            // this.EntityPM.ForeignAmount = this.CashBookPM.TotalAmount;


        }
        else
        {
            if (!AppTool.IsNullOrEmpty(this.CashbookLines.Collection))
            {
                for (let line of this.CashbookLines.Collection) {
                    this.CashBookTotal += line.ForeignAmount == null ? 0 : line.ForeignAmount;
                }
            }
        }



        this.SelectedTotal = 0;
        var localSum = 0.0;
        if (!AppTool.IsNullOrEmpty(this.BankDepositLines.Collection)) {
            for (let line2 of this.BankDepositLines.Collection) {
                localSum += line2.LocalAmount;
                this.SelectedTotal += line2.ForeignAmount == null ? 0 : line2.ForeignAmount;
            }

            if (this.IsLinesSelection) {
                if (this.IsLinesSelection && !this.EntityPM.IsCashDeposit) {
                    this.CalculateTotalsForNewChequeDeposite();

                } else if (this.IsLinesSelection) {
                    this.EntityPM.ForeignAmount = this.SelectedTotal;
                    //this.CalculateForeign(this.SelectedTotal);
                    this.CalculateLocal(this.SelectedTotal);

                }
            }
        }
    }

    private CalculateTotalsForNewChequeDeposite() {
        this.SelectedTotal = 0;
        var localSum = 0.0;
        for (let CashbookLine of this.SelectedCashbookLines.Collection) {
            localSum += CashbookLine.LocalAmount;
            this.SelectedTotal += CashbookLine.ForeignAmount == null ? 0 : CashbookLine.ForeignAmount;
        }
        this.EntityPM.LocalDepositAmount = AppTool.Round(localSum, 2);
        this.EntityPM.ForeignAmount = AppTool.Round(this.SelectedTotal, 2);
    }

    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }

    // RemoveDepositedLines() { // remove deposited lines from cashbook lines
    //     if (!AppTool.IsNullOrEmpty(this.CashBookLines)) {


    //         var nonDepositedlines = [];
    //         for (var i = 0; i < this.CashBookLines.length; i++) {

    //             if (this.CashBookLines[i].IsDeposited == false) {
    //                 nonDepositedlines.push(this.CashBookLines[i]);
    //             }

    //         }

    //         // Filtering
    //         nonDepositedlines = nonDepositedlines.filter((el) => {
    //             if (el.ARPChequeStatusCode == "5")
    //                 return false;
    //             else
    //                 return true;
    //         }); //// 5- Returned to Customer

    //         this.CashBookLines = nonDepositedlines;
    //         this.CashBookLines2.InsertCollection(this.CashBookLines);
    //     }

    // }

    SelectAll(event) {

        this.IsAllSelected = event;

        this.BankDepositLines.Clear();
        this.EntityPM.BankDepositLines = [];
        this.SelectedCashbookLines = new ObservableCollection([]);

        // this.BankDepositLines = [];
        if (event == true) {
            for (let line of this.CashbookLines.Collection) {
                this.OriginalCashbookLines.Collection.filter(a => a.CashBookId == line.CashBookId && a.ARPChequeId == line.ARPChequeId)[0].IsSelected = true;
                this.SelectedCashbookLines.Collection.push(line);
                this.PushBankDeposit(line);
                line.IsSelected = true;
            }
        }
        else {
            for (let line of this.CashbookLines.Collection) {
                this.OriginalCashbookLines.Collection.filter(a => a.CashBookId == line.CashBookId && a.ARPChequeId == line.ARPChequeId)[0].IsSelected = false;
                line.IsSelected = false;
          }
        }

        this.CalculateTotals();
    }
    
    LineSelection(cashbookLine, event, validatePairs = true) {
        this.CashbookLines.Collection.filter(a => a.CashBookId == cashbookLine.CashBookId && a.ARPChequeId == cashbookLine.ARPChequeId)[0].IsSelected = event;
        this.OriginalCashbookLines.Collection.filter(a => a.CashBookId == cashbookLine.CashBookId && a.ARPChequeId == cashbookLine.ARPChequeId)[0].IsSelected = event;

        if (event) {
            this.PushBankDeposit(cashbookLine);
            this.SelectedCashbookLines.Collection.push(cashbookLine);
        } else {
            this.DeleteUnSelectedLine(cashbookLine);
            this.PopBankDeposit(cashbookLine);
            this.IsAllSelected = false;
        }
        this.CalculateTotals();
        if (validatePairs) {
            this.CurrentSession.CurrentEditComponent.EditComponentArgument = this.CurrentSession.CurrentEditComponent.EditComponentArgument || {};
            this.CurrentSession.CurrentEditComponent.EditComponentArgument.ChequePairsValid = this.CheckAllSelectedChequesHaveTheirPairs();
        }
    }
    CheckAllSelectedChequesHaveTheirPairs() {
        const selectedLines = this.CashbookLines.Collection.filter(line => line.IsSelected);
    
        for (const selected of selectedLines) {
            const { BankNumber, BranchNumber, AccountNumber, ChequeNumber } = selected;
            const selectedDueDate = new Date(selected.DueDate).getTime();

            const startRange = selectedDueDate - this.PeriodForChequePairValidation;
            const endRange = selectedDueDate + this.PeriodForChequePairValidation;
        
            const matchingGroup = this.CashbookLines.Collection.filter(line => 
                line.BankNumber === BankNumber &&
                line.BranchNumber === BranchNumber &&
                line.AccountNumber === AccountNumber &&
                line.ChequeNumber === ChequeNumber &&
                new Date(line.DueDate).getTime() >= startRange &&
                new Date(line.DueDate).getTime() <= endRange
            );
           
            const allGroupSelected = matchingGroup.every(line => line.IsSelected);
            if (!allGroupSelected) {
                return false;
            }
        }  
        return true;
    }
      
    private DeleteUnSelectedLine(line: any) {
        var index = this.SelectedCashbookLines.Collection.findIndex(d => d.ChequeNumber === line.ChequeNumber);
        this.SelectedCashbookLines.Collection.splice(index, 1);
    }

    PushBankDeposit(cashbookLine: CashBookLinePM) {
        const isChequeNotExists = this.EntityPM.BankDepositLines.filter(x => x.ARPaymentChequeId == cashbookLine.ARPChequeId).length == 0;
        if (!AppTool.IsNullOrEmpty(cashbookLine) && isChequeNotExists) {
            // Get Counter
            var lastRow = this.BankDepositLines.Collection[this.BankDepositLines.Length - 1];
            // var lastRow = this.BankDepositLines[this.BankDepositLines.length - 1];
            if (!AppTool.IsNullOrEmpty(lastRow)) {
                var lineCounter = Number(this.BankDepositLines.Collection[this.BankDepositLines.Length - 1].Line + 1);
            } else {
                var lineCounter = 1;
            }

            // New Row
            var depositLine = new BankDepositLinePM(this.EntityPM);

            depositLine.CompositId = cashbookLine.CashBookId + ',' + cashbookLine.ARPChequeId;

            depositLine.Line = lineCounter;
            depositLine.Tenant = this.EntityPM.Tenant;
            depositLine.ARPaymentChequeId = cashbookLine.ARPChequeId;
            depositLine.AccountNumber = cashbookLine.AccountNumber;
            depositLine.IsOutOfDeposit = false;
            depositLine.LocalAmount = cashbookLine.LocalAmount;
            depositLine.ForeignAmount = cashbookLine.ForeignAmount;
            depositLine.DueDate = cashbookLine.DueDate;
            depositLine.Bank = cashbookLine.Bank;
            depositLine.Branch = cashbookLine.Branch;
            depositLine.ARPaymentNumber = cashbookLine.ARPaymentNumber;
            //depositLine.ChequeStatusCode = cashbookLine.ARPChequeStatusCode;
            //depositLine.ChequeStatusName = cashbookLine.ARPChequeStatusName;
            depositLine.ChequeNumber = cashbookLine.ChequeNumber;
            depositLine.Currency = cashbookLine.Currency;
            depositLine.ARPaymentId = cashbookLine.ARPaymentId;
            this.EntityPM.AddBankDepositLine(depositLine);

            this.BankDepositLines.Insert(depositLine);
            // this.BankDepositLines.push(depositLine);
        }
    }

    PopBankDeposit(cashbookLine: CashBookLinePM) {
        if (!AppTool.IsNullOrEmpty(cashbookLine)) {

            var id = cashbookLine.CashBookId + ',' + cashbookLine.ARPChequeId;

            var item = this.BankDepositLines.Collection.find(d => d.CompositId == id);
            var index = this.BankDepositLines.Collection.findIndex(d => d.CompositId == id);
            this.BankDepositLines.RemoveFromIndex(index);
            // this.BankDepositLines.splice(index, 1)

            this.EntityPM.RemoveBankDepositLine(item);
        }
    }

    //#region Out of Deposit
    ReturnChequeButtonClicked(line: BankDepositLinePM) {

        // validate redeemed cheque
       //CANCELLED BY TASK  112261
        // if (line.ChequeStatusCode == "6") { // 6- Redeemed
        //     var msg = new MessageWindow();
        //     msg.Show(TextCodeTranslator.Translate("Accounting.O.RedeemedChequeMSG"));
        //     this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //     this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.O.RedeemedChequeMSG"));

        //     return;
        // } 
       
        if (line.ChequeStatusCode == "6"){
              this.showRedeemedWarningWindow(line);
        }
        else{
            this.showReturnChequeWindow(line);
        }
        

       

    }
    showRedeemedWarningWindow(line){
        var myConfirmWindow = new ConfirmWindow();
        myConfirmWindow.Width = 400;
        myConfirmWindow.ShowNoButton = true;
        
        myConfirmWindow.Show(TextCodeTranslator.Translate("Accounting.O.AlreadyRedeemed"));
        myConfirmWindow.WindowClosed.subscribe(event => {
            if (myConfirmWindow.Yes) {
                this.showReturnChequeWindow(line);
            }
     
        });
    }
    showReturnChequeWindow(line){

        // new code
        var windowArgs: any = {};
        //windowArgs.ReconciliationPM = entity;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 215;
        logitudeWindow.Title = TextCodeTranslator.Translate("Accounting.O.OutOfDeposit");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Accounting/Components/Others/OutOfDepositMessage');
        logitudeWindow.WindowClosed.subscribe((event: string) =>
        {
            if (event && event != "Cancel")
            {
                var sp = event.split(";");
                var type = sp[0];
                var notes = sp[1];
                this.ReturnCheque(line.ARPaymentChequeId, type, notes);


            }
        });

    }
    ReturnCheque(chequeId:string ,returnType:string, notes: string) {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.bankDepositExtendedPMService.returnCheque(this.EntityPM.Id, chequeId, returnType, notes).subscribe((myResult:ServiceResponse) => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.GetEntityPMAndRedrawScreen();

            }
            else {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
    GetEntityPMAndRedrawScreen() {
        this.BankDepositPMService.get(this.EntityPM.Id).subscribe((Result:any) => {

            var result: ServiceResponse = Result;
            if (!result.HasError) {
                //this.EntityPM = Result.Result;
                this.CurrentSession.CurrentEditComponent.EntityPM = Result.Result;
                this.RedrawScreen();
            }
        });

    }
    //#endregion

    CashCount: number = 0;
    PostdatesCount: number = 0;


    public ChequesCounter: CashbookChequesCounter = new CashbookChequesCounter();
    GetChequesCounter(){
        this.ChequesCounter = new CashbookChequesCounter();
        this.cashBookExtendedPMService.GetCashbookChequesCounter(this.EntityPM.CashBookId)
            .subscribe((response: ServiceResponse) =>
            {
                console.log("[GetCashbookChequesCounter]", response);

                if (!response.HasError) {
                    this.ChequesCounter = response.Result;
                }
                else {
                    console.error(response.ErrorsArray);
                }
            });

    }

    // private UpdateFiltersCounts()
    // {
    //     var todayDate = DateTool.GetCurrentDateTimeAsUtc();

    //     this.CashCount = this.CashbookLines.Collection.filter((el) =>
    //     {
    //         if (el.DueDate != null) {
    //             var date = new Date(el.DueDate.toString());
    //             if (date <= todayDate) {
    //                 return true;
    //             }
    //             return false;
    //         }
    //         return false;
    //     }).length;
    //     this.PostdatesCount = this.CashbookLines.Collection.filter((el) =>
    //     {
    //         if (el.DueDate != null) {
    //             var date = new Date(el.DueDate.toString());
    //             if (date > todayDate) {
    //                 return true;
    //             }
    //             return false;
    //         }
    //         return false;
    //     }).length;
    // }

    RefreshButtonClicked() {

        if (!this.IsNewDepositMode) {
            this.GetDepositLines();
        }
        else {
            this.GetCashbookLines();
        }
    }

    buildQueryColumns(){
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ChequeNumber",'Text',TextCodeTranslator.Translate("BankDepositLine.F.ChequeNumber")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("DueDate",'DateTime',TextCodeTranslator.Translate("BankDepositLine.F.DueDate")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("LocalAmount",'Number',this.GetAmountHeader()));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Currency",'Text',TextCodeTranslator.Translate("CashBookLine.F.Currency")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ForeignAmount",'Number',TextCodeTranslator.Translate("CashBookLine.F.ForeignAmount")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("AccountNumber",'Text',TextCodeTranslator.Translate("CashBookLine.F.AccountNumber")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Bank",'Text',TextCodeTranslator.Translate("CashBookLine.F.Bank")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("Branch",'Text',TextCodeTranslator.Translate("CashBookLine.F.Branch")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.GetQueryColumn("ARPaymentNumber",'Text',TextCodeTranslator.Translate("CashBookLine.F.ARPaymentNumber")));
        this.QueryColumns.push(this.LogitudeGridExportToExcelComponent.
            GetQueryColumn(SessionLocator.LoggedUserPM.DontShowLocal ? 'ChequeStatusName' : 'ChequeStatusLocalName','Text',TextCodeTranslator.Translate("ARPaymentCheque.F.StatusCode")));
    }

    public ExportToExcel(){
        this.LogitudeGridExportToExcelComponent.ExportToExcelExcute('BankDepositLine',this.GetDeposiutAPIFilters(),this.QueryColumns,"SaveToMicrosoftExcel2007",true);
    }
}

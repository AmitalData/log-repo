import { ARPaymentChequeList } from './../../../EntityLists/ARPaymentChequeList';
import { ARPaymentChequeListService } from './../../../Services/StandardLists/ARPaymentChequeListService';
import {Component}  from '@angular/core';
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

@Component({
    moduleId: module.id,
    templateUrl: './BankDepositDetailsTabComponent.html',
})

export class BankDepositDetailsTabComponent extends BaseComponent {
    public EntityPM: BankDepositPM = null;
    public ObjectTableName = "BankDeposit";
    public DataContext = this;
    public CashBookTotal = 0;
    public SelectedTotal = 0;
    public NoCashBookRows: boolean = false;
    public IsLinesSelection: boolean = false;
    searchText: string = "";

    CashBookPM: CashBookPM;
    BankDepositLines: BankDepositLinePM[];
    CashBookLines: CashBookLinePM[];

    _CashBookPMService: CashBookPMService = new CashBookPMService();
    ratesTableExtendedListService: RatesTableExtendedListService = new RatesTableExtendedListService();
    currencyListService: CurrencyListService = new CurrencyListService();
    _BankDepositExtendedPMService: BankDepositExtendedPMService = new BankDepositExtendedPMService();
    _ARPaymentChequeListService: ARPaymentChequeListService = new ARPaymentChequeListService();

    public isRTL: boolean = false;
    public IsReturnChequeEnabled: boolean = false;

    txt_NewDeposit: string = TextCodeTranslator.Translate("Accounting.General.O.NewDeposit");
    txt_DepositDetails: string = TextCodeTranslator.Translate("Accounting.General.O.DepositDetails");
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityArgs.EntityPM;
        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) { // selection mode
            this.IsLinesSelection = true;
            this.GetCashBook();
            this.BankDepositLines = this.EntityPM.BankDepositLines;
            this.BankDepositLines = [];


        } else { // view mode
            this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = false;
            this.GetCashBook();
            this.BankDepositLines = this.EntityPM.BankDepositLines;
            this.SetUIProperty();

            console.log("Deposit: ", this.EntityPM);

        }

        this.Listen();


        // Dim fields
        //this.SetUIProperty(); // do it after getting cashbook (isCashDeposit)


        // Get Default Value
        this.GetDefaultValues();
    }

    Listen(){
        // Save
        this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            if (isSaveSuccess) {
                console.log("Deposited Success", this.EntityPM);
                this.RedrawScreen();
                this.ShowAlert();
            }
        });

        // Reload
        this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
            if (isLoadSuccess) {
                this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                console.log("Entity Reloaded");
                console.log("Deposited Success", this.EntityPM);
                this.RedrawScreen();
            }
        });
    }

    RefreshEntity(){
        this.CurrentSession.CurrentEditComponent.EntityId = this.EntityPM.Id; // Set entity id in edit component to reload
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM(); // reloading
    }

    RedrawScreen() {

        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        console.log("Deposit Reloaded: ", this.EntityPM);

        this.IsLinesSelection = !this.EntityPM.Id;


        // Redraw UI
        this.IsLinesSelection = false;
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.CashBookLines = [];
        this.BankDepositLines = this.EntityPM.BankDepositLines;
        this.CalculateTotals();

        this.GetCashBook();
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
                if (value > this.CashBookPM.TotalAmount) {
                    this.UIProperties.SetValidity("ForeignAmount", this.ObjectTableName, false, "Amount must be less than Cashbook total");
                    //this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                    //this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push("Deposit Amount must be less than Cashbook total");
                    //this.CurrentSession.CurrentEditComponent.IsEditValid = false;
                } else {
                    this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
                    this.UIProperties.SetValidity("ForeignAmount", this.ObjectTableName, true, "");
                    //this.CurrentSession.CurrentEditComponent.IsEditValid = true;
                    this.CalculateLocal(value);
                }
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

    //#region Filter Methods
    public FilterSelectedValue: string = 'cash';
    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            this.FilterLines();
            this.IsAllSelected = false;
        }
    }
    FilterLines() {

        this.BankDepositLines = [];
        this.EntityPM.BankDepositLines = [];

        var todayDate = DateTool.GetCurrentDateTimeAsUtc();

        var lines = this.CashBookPM.CashBookLines;

        // Filtering
        if (!AppTool.IsNullOrEmpty(this.FilterSelectedValue)) {
            lines = lines.filter((el) => {

                if (el.DueDate != null) {
                    var date = new Date(el.DueDate.toString());
                    if (this.FilterSelectedValue == 'postdated') {
                        if (date > todayDate) {
                            return true; //postdated
                        }
                    } else if (this.FilterSelectedValue == 'cash') {
                        if (date <= todayDate) {
                            return true;
                        }
                    }
                }
                return false;
            });
        }

        this.CashBookLines = lines;

        // remove deposited lines
        this.RemoveDepositedLines();


        if (this.CashBookLines.length > 0) {
            this.NoCashBookRows = false;
        } else {
            this.NoCashBookRows = true;
        }

        this.CalculateTotals();

    }
    private timerToken: any;
    TextChanged(searchtext) {

        // Deposited cheque
        if (!this.IsLinesSelection && !this.EntityPM.IsCashDeposit) {
            this.timerToken = setTimeout(() => {
                this.searchText = searchtext;
                this.FilterChequeDeposits();
            }, 500);
        }

    }
    FilterChequeDeposits() {
        if (AppTool.IsNullOrEmpty(this.searchText)) {
            this.BankDepositLines = this.EntityPM.BankDepositLines;

        } else {
            var filteredDepositedCheques = [];
            //filteredDepositedCheques = this.EntityPM.BankDepositLines.filter(d => d.ChequeNumber.toLowerCase().includes(this.searchText.toLowerCase()));
            filteredDepositedCheques = this.EntityPM.BankDepositLines.filter(d => d.SearchFields.toLowerCase().includes(this.searchText.toLowerCase()));
            this.BankDepositLines = filteredDepositedCheques;
        }


    }
    //#endregion

    //#region Get Methods
    GetCashBook() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._CashBookPMService.get(this.EntityPM.CashBookId).subscribe(myResult => {
            this.CurrentSession.StopBusyIndicator();

            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {
                this.CashBookPM = myResponse.Result;
                console.log("Cashbook: ", this.CashBookPM);

                if (this.IsLinesSelection)
                {
                    if (this.CashBookPM.CashBookTypeCode == "1") // 1- Cash
                    {
                        this.SetUIProperty();
                        this.EntityPM.IsCashDeposit = true;

                        this._CashbookTotal = this.CashBookPM.TotalAmount;

                        //copy amount
                        //this.EntityPM.LocalDepositAmount = this.CashBookPM.TotalAmount;
                        this.EntityPM.ForeignAmount = this.CashBookPM.TotalAmount;

                        if (this.isCurrencyRateLoaded)
                            this.CalculateLocal(this.ForeignAmount);

                        this.UIProperties.SetValidity("LocalDepositAmount", this.ObjectTableName, true, "");
                        this.UIProperties.SetValidity("ForeignAmount", this.ObjectTableName, true, "");
                    }
                    else
                    {  // Cheque

                        this.CashBookLines = this.CashBookPM.CashBookLines;
                        this.ComputeTotals();

                        this.RemoveDepositedLines();
                        this.CalculateTotals();
                        this.FilterLines();
                        if (this.CashBookLines.length > 0) {
                            this.NoCashBookRows = false;
                        } else {
                            this.NoCashBookRows = true;
                        }

                        this.SetUIProperty();
                        this.EntityPM.IsCashDeposit = false;
                    }


                } else {
                    this._CashbookTotal = this.CashBookPM.TotalAmount;
                    this.CalculateTotals();

                }
            }
        }
            , error => {
            });
    }

    tenantCurrencyCode: string = "";
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
        }
        else
        {
            if (!AppTool.IsNullOrEmpty(this.CashBookLines))
            {
                for (let line of this.CashBookLines) {
                    this.CashBookTotal += line.ForeignAmount == null ? 0 : line.ForeignAmount;
                }
            }
        }



        this.SelectedTotal = 0;
        var localSum = 0.0;
        if (!AppTool.IsNullOrEmpty(this.BankDepositLines)) {
            for (let line2 of this.BankDepositLines) {
                localSum += line2.LocalAmount;
                this.SelectedTotal += line2.ForeignAmount == null ? 0 : line2.ForeignAmount;
            }

            if (this.IsLinesSelection) {
                if (this.IsLinesSelection && !this.EntityPM.IsCashDeposit) {
                    this.EntityPM.LocalDepositAmount = localSum;
                    this.EntityPM.ForeignAmount = this.SelectedTotal;

                } else if (this.IsLinesSelection) {
                    this.EntityPM.ForeignAmount = this.SelectedTotal;
                    //this.CalculateForeign(this.SelectedTotal);
                    this.CalculateLocal(this.SelectedTotal);

                }
            }
        }
    }

    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }

    RemoveDepositedLines() { // remove deposited lines from cashbook lines
        if (!AppTool.IsNullOrEmpty(this.CashBookLines)) {


            var nonDepositedlines = [];
            for (var i = 0; i < this.CashBookLines.length; i++) {

                if (this.CashBookLines[i].IsDeposited == false) {
                    nonDepositedlines.push(this.CashBookLines[i]);
                }

            }

            // Filtering
            nonDepositedlines = nonDepositedlines.filter((el) => {
                if (el.ARPChequeStatusCode == "5")
                    return false;
                else
                    return true;
            }); //// 5- Returned to Customer

            this.CashBookLines = nonDepositedlines;
        }

    }

    SelectAll(event) {

        this.IsAllSelected = event;

        this.BankDepositLines = [];
        this.EntityPM.BankDepositLines = [];
        if (event == true) {
            for (let line of this.CashBookLines) {
                this.PushBankDeposit(line);
            }
        }
        this.CalculateTotals();
    }

    LineSelection(cashbookLine, event) {
        if (event == true) {
            this.PushBankDeposit(cashbookLine);
        } else if (event == false) {
            this.PopBankDeposit(cashbookLine);
        }
        this.CalculateTotals();
    }

    PushBankDeposit(cashbookLine: CashBookLinePM) {
        if (!AppTool.IsNullOrEmpty(cashbookLine)) {

            // Get Counter
            var lastRow = this.BankDepositLines[this.BankDepositLines.length - 1];
            if (!AppTool.IsNullOrEmpty(lastRow)) {
                var lineCounter = this.BankDepositLines[this.BankDepositLines.length - 1].Line + 1;
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

            this.EntityPM.AddBankDepositLine(depositLine);
            this.BankDepositLines.push(depositLine);
        }
    }

    PopBankDeposit(cashbookLine: CashBookLinePM) {
        if (!AppTool.IsNullOrEmpty(cashbookLine)) {

            var id = cashbookLine.CashBookId + ',' + cashbookLine.ARPChequeId;

            var item = this.BankDepositLines.find(d => d.CompositId == id);
            var index = this.BankDepositLines.findIndex(d => d.CompositId == id);
            this.BankDepositLines.splice(index, 1)
            this.EntityPM.RemoveBankDepositLine(item);
        }
    }

    //#region Out of Deposit
    ReturnChequeButtonClicked(line: BankDepositLinePM) {

        // validate redeemed cheque
        if (line.ChequeStatusCode == "6") { // 6- Redeemed
            // var msg = new MessageWindow();
            // msg.Show(TextCodeTranslator.Translate("Accounting.O.RedeemedChequeMSG"));
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.O.RedeemedChequeMSG"));

            return;
        } else {
            this.showReturnChequeWindow(line);
        }

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
        //
        // returnType: Customer / Cashbook
        //
        this.CurrentSession.StartBusyIndicatorLoading();
        this._BankDepositExtendedPMService.returnCheque(this.EntityPM.Id, chequeId, returnType, notes).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.RedrawScreen();
            }
            else {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
    //#endregion

    CashCount: number = 0;
    PostdatesCount: number = 0;
    ComputeTotals() {

        this.RemoveDepositedLines();

        var todayDate = DateTool.GetCurrentDateTimeAsUtc();
        this.CashCount = this.CashBookLines.filter((el) => {

            if (el.DueDate != null) {
                var date = new Date(el.DueDate.toString());
                if (date <= todayDate) {
                    return true;
                }
                return false;

            }
            return false;
        }).length;
        this.PostdatesCount = this.CashBookLines.filter((el) => {

            if (el.DueDate != null) {
                var date = new Date(el.DueDate.toString());
                if (date > todayDate) {
                    return true;
                }
                return false;

            }
            return false;
        }).length;
    }

}

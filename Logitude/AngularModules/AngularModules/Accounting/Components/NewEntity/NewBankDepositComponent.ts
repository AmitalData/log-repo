import {Component, ChangeDetectorRef, OnInit} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {BankDepositPM} from '../../EntityPMs/BankDepositPM';
import {CashBookPM} from '../../EntityPMs/CashBookPM';
import {BankAccountPM} from '../../EntityPMs/BankAccountPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {CashBookPMService} from '../../Services/StandardPMs/CashBookPMService';
import {AccountingPeriodExtendedListService} from '../../Services/ExtendedLists/AccountingPeriodExtendedListService';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {RatesTableExtendedListService } from '../../../Infrastructure/Services/ExtendedLists/RatesTableExtendedListService';

@Component({
    selector: 'NewBankDepositComponent',
    moduleId: module.id,
    templateUrl: './NewBankDepositComponent.html',
})

export class NewBankDepositComponent extends BaseComponent implements OnInit {
    public EntityPM: BankDepositPM;
    public DataContext: NewBankDepositComponent = this;
    public ObjectTableName: string = "BankDeposit";
    public ValidationErrorsList: string[] = [];
    public CashBookTotal: number = 0;
    private cashBookPMService = new CashBookPMService();
    private myAccountingPeriodListService: AccountingPeriodExtendedListService = new AccountingPeriodExtendedListService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private ratesTableExtendedListService: RatesTableExtendedListService = new RatesTableExtendedListService();


    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this._entityResourceService.getEntityResourceByTableName("AccountingPeriod").subscribe(res => { });

        this.EntityPM = new BankDepositPM();
        this.EntityPM.AccountingDate = new Date();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.DepositDate = new Date();
        this.EntityPM.DepositNumber = 0;

        this.GetClosedMonth();

              
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            if (!AppTool.IsNullOrEmpty(args.CashBookId)) {
                this.CashBookId = args.CashBookId;
                this.UIProperties.SetEnabled("CashBookId", this.ObjectTableName, false); // lock field
            }
        }
    }

    ngOnInit() {

    }

    //#region Properties

    get AccountingDate() { return this.EntityPM.AccountingDate; }
    set AccountingDate(value: Date) {
        if (this.EntityPM.AccountingDate != value) {
            this.EntityPM.AccountingDate = value;
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

    cashbook: CashBookPM;
    get CashBook() { return this.cashbook; }
    set CashBook(value: CashBookPM) {
        if (this.cashbook != value) {
            this.cashbook = value;
            if (this.cashbook) {
                this.EntityPM.DepositCurrencyId = this.cashbook.CurrencyId;
                this.CashBookTotal = this.cashbook.TotalAmount == null ? 0 : this.cashbook.TotalAmount;
            }
        }
    }

    bankAccount: BankAccountPM;
    get DepositBankAccount() { return this.bankAccount; }
    set DepositBankAccount(value: BankAccountPM) {
        if (this.bankAccount != value) {
            this.bankAccount = value;
        }
    }
    //#endregion

    OkButtonClicked() {
        var errors: string[] = [];
        

        // Required check
        if (AppTool.IsNullOrEmpty(this.DepositBankAccountId) || AppTool.IsNullOrEmpty(this.CashBookId)) {
            errors.push(TextCodeTranslator.Translate("Accounting.General.O.AllFieldsRequired"));
        }
        if (this.EntityPM.DepositCurrencyId != SessionLocator.TenantPM.CurrencyId) {
            //check rate 
            this.CurrentSession.CurrentWindow.StartBusyIndicator("...");
            this.ratesTableExtendedListService.getClosestRate(SessionLocator.TenantPM.CurrencyId, this.EntityPM.DepositCurrencyId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        if (myResponse.Result != undefined && myResponse.Result != null) {
                            //continu saving
                            this.CheckIfThereIsCheques(this.CashBookId);

                        }
                        else {
                            this.ValidationErrorsList = [];
                            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.O.NoExchangeRateForLocalCurrency"));
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }
                    }
                }
            });

            if (errors.length > 0) {
                this.ValidationErrorsList = errors;
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
            }
        } else {
            //continu saving
            this.CheckIfThereIsCheques(this.CashBookId);
        }

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    AccountingDateLostFocus() {
        this.validateAccountingPeriod();
    }

    CheckIfThereIsCheques(id) {

        // 1-get cashbook pm
        this.cashBookPMService.get(id).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;

            if (!myResponse.HasError) {
                var entityPm: CashBookPM;

                entityPm = myResponse.Result;
                if (!AppTool.IsNullOrEmpty(entityPm)) {

                    //2-check
                    if (entityPm.CashBookTypeCode == "2") { // 2-cheques
                        var exist = entityPm.CashBookLines.find(d => d.IsDeposited == false);
                        if (exist) {
                            this.SubmitChanges();
                        } else {
                            this.ValidationErrorsList = [];
                            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.noChequesinCashbook"));
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }
                    } else if (entityPm.CashBookTypeCode == "1") { // 1-cash
                        if (AppTool.IsNullOrZero(entityPm.TotalAmount)) {
                            this.ValidationErrorsList = [];
                            this.ValidationErrorsList.push(TextCodeTranslator.Translate("Accounting.General.O.noCashICashbook"));
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        } else {
                            this.SubmitChanges();
                        }
                    }

                }

            } else {
                console.log("error");
                this.CurrentSession.CurrentWindow.StopBusyIndicator();

            }
        });

        
    }

    SubmitChanges() {

        var errors: string[] = [];

        
        //#region currency check
        if (this.bankAccount.GLAccountCurrencyId != null && this.bankAccount.GLAccountCurrencyId != "multi") {
            if (this.bankAccount.GLAccountCurrencyId != this.cashbook.CurrencyId) {
                errors.push(TextCodeTranslator.Translate("Accounting.General.O.Currencydifferent"));
            }
        } else {
            console.warn("maybe the glaccount of bank account is multi or null!");
        }
        this.EntityPM.IsCashDeposit = this.CashBook.CashBookTypeCode == "1";
        //#endregion 

        //closed month check
        var isValid = this.IsMonthOpenForAccountingDate();
        if (!isValid) errors.push(TextCodeTranslator.Translate("AccountingPeriods.O.ClosedMonth")); // closed month
        

        if (errors.length == 0) {

            this.EntityPM.CashBookName = this.CashBook.LocalName;
            this.EntityPM.BankAccountNumber = this.DepositBankAccount.AccountNumber;

            if (this.EntityPM != null) {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();

                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityPM: this.EntityPM, ObjectTableName: 'BankDeposit' });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.CancelButtonClicked();
                        });
                    });
            }

        } else {
            this.ValidationErrorsList = errors;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
        }
    }

    //#region closed month
    validateAccountingPeriod()
    {
        var isValid = this.IsMonthOpenForAccountingDate();
        if (!isValid) {
            //this.ValidationErrorsList = [];
            //this.ValidationErrorsList.push("חודש סגור!"); // closed month
            this.UIProperties.SetValidity("AccountingDate", this.ObjectTableName, false, TextCodeTranslator.Translate("AccountingPeriods.O.ClosedMonth"));
        } else {
            this.UIProperties.SetValidity("AccountingDate", this.ObjectTableName, true, "");
        }
        return isValid;
    }
    IsMonthOpenForAccountingDate()
    {
        var valid = true;
        if (this.accountingPeriod == null) {
            valid = false;
            //errorsList.Add(transText);
        }
        else {
            var accountingDateMonth = this.EntityPM.AccountingDate.getMonth() + 1; 

            if (accountingDateMonth > this.accountingPeriod.ClosedMonth) {
                //Valid ... AccountingDateMonth must be greater than close Mounth
            }
            else {
                //Not Valid ... AccountingDateMonth must be greater than close Mounth
                //not valid  8>=8 
                //not valid  0>=1 - Must Open mounth before work on year !!
                valid = false;
                //errorsList.Add(transText); //ClosedMonth Must B
            }
            if (accountingDateMonth == this.accountingPeriod.OpenMonth) {
                //valid ... accountingDateMonth can be  equal to OpenMonth
            }
            else if (accountingDateMonth < this.accountingPeriod.OpenMonth) {
                //valid ... accountingDateMonth can be  less than OpenMonth
            }
            else {
                valid = false;
                //errorsList.Add(transText);
            }
        }
        return valid;
    }

    private accountingPeriod: any;
    private GetClosedMonth() {
        var periodTypeCode = "1" // 1-Regular
        this.myAccountingPeriodListService.getByYear(new Date().getFullYear(), periodTypeCode).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.accountingPeriod = myResponse.Result;
                }
            }
        });
    }
    //#endregion
}

import {Component,OnInit} from '@angular/core';
import {CustomerPM} from '../../../../Common/EntityPMs/CustomerPM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {GetAccountingSystemWindowArgs} from '../../../../Common/Args';
import {GlobalDomainService} from "../../../../Common/Services/GlobalDomainService"

@Component({
    moduleId: module.id,
    templateUrl: './QuickBooksComponent.html',
})

export class QuickBooksComponent implements OnInit {
    public EntityPM: CustomerPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = entityArgs.EntityPM;
        this.GlobalDomainService = new GlobalDomainService();
    }

    public GlobalDomainService: GlobalDomainService;
    public MyCardsLabel: string = "My Cards";
    private isWindowOpened: boolean = false;
    public CustomersListFilterd: Array<any> = [];
    public searchText: string = "";
    public HelpText: string = "";
    public isCustomer: boolean = false;
    public isPaymentMethod: boolean = false;
    public isVatType: boolean = false;
    public isReceivablesChargesType: boolean = false;
    public isPayablesChargesType: boolean = false;
    public isPaymentTerm: boolean = false;
    public isVendor: boolean = false;
    public isAgent: boolean = false;
    public isCurrency: boolean = false;
    public LoadedForTheFirstTime: boolean = true;
    private QuantityLabel: string = "My Cards";
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        if (this.searchText != newValue) {
            this.searchText = newValue;
            this.OnSearchTextChanged();
        }
        
    }
    public args: GetAccountingSystemWindowArgs;

    SetWindowArgs(args: GetAccountingSystemWindowArgs) {
        this.args = args;
    }

    private timerToken: any;

    public selectedItem = null;

    public get SelectedItem() { return this.selectedItem; }

    public set SelectedItem(item: any) { this.selectedItem = item; }

    OnSearchTextChanged() {
    
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }

            this.timerToken = setTimeout(() => this.GetCustomersBySearch(), 500);


    }
    OkButtonClicked() {
        if (this.SelectedItem != null)
            this.args.SelectedEntity = this.SelectedItem;
        this.CurrentSession.CloseCurrentWindow();

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();

    }

    public TempList: Array<any> = [];


    ngOnInit() {
        if (this.args.CardName == "Account" || this.args.CardName == "AccountBankCredit") {
            this.isPayablesChargesType = true;
            this.QuantityLabel = "Payables Charges Types";
            if (this.args.CardName == "AccountBankCredit")
                this.QuantityLabel = "Payment Account Types";
        }

        else if (this.args.LogitudeCardName == "Customer")
            this.isCustomer = true;
        else if (this.args.LogitudeCardName == "VatType") {
            this.isVatType = true;
            this.QuantityLabel = "VAT Types";
        }

        else if (this.args.LogitudeCardName == "ChargesType") {
            this.isReceivablesChargesType = true;
            this.QuantityLabel = "Receivables Charges Types";
        }

        else if (this.args.LogitudeCardName == "PaymentTerm") {
            this.isPaymentTerm = true;
            this.QuantityLabel = "Payment Terms";
        }

        else if (this.args.LogitudeCardName == "Currency") {
            this.isCurrency = true;
            this.QuantityLabel = "Currencies";
        }

        else if (this.args.LogitudeCardName == "Vendor")
            this.isVendor = true;

        else if (this.args.LogitudeCardName == "Agent")
            this.isAgent = true;

        else if (this.args.CardName == "PaymentMethod") {
            this.isPaymentMethod = true;
            this.QuantityLabel = "Payment Methods";
        }

      

        setTimeout(() => this.GetCustomersBySearch(), 1); 

    }

    GetCustomersBySearch() {
        if (!this.LoadedForTheFirstTime && !this.args.ReceivableCard && !this.args.PayableCard && !this.isReceivablesChargesType && !this.isPayablesChargesType && !this.isPaymentMethod ) {
            if (this.searchText == null)
                this.searchText = "";
            this.CustomersListFilterd = this.TempList.filter(f => f.nameField.toLowerCase().indexOf(this.SearchText.toLowerCase()) > -1);
            if (this.args.LogitudeCardName == "VatType")
                this.MyCardsLabel = "VAT Types (" + this.CustomersListFilterd.length + ")";
            else if (this.args.LogitudeCardName == "ChargesType")
                this.MyCardsLabel = "Charges Types (" + this.CustomersListFilterd.length + ")";
            else if (this.args.LogitudeCardName == "PaymentTerm")
                this.MyCardsLabel = "Payment Terms (" + this.CustomersListFilterd.length + ")";
            else if (this.args.LogitudeCardName == "Currency")
                this.MyCardsLabel = "Currencies (" + this.CustomersListFilterd.length + ")";
        }
        else {
            if (this.searchText == null)
                this.searchText = "";
            this.CurrentSession.StartBusyIndicator("Searching ..");           
            this.GlobalDomainService.GetQuickBooksQueries(this.args, this.SearchText).subscribe(myResult => {
                this.TempList = [];
                this.CustomersListFilterd = [];
                var list: Array<any> = myResult.Result;
                if (list != null)
                    if (list.length > 20) {
                        this.HelpText = "Please, use the search tool to find more " + SessionLocator.AccountingSystemPM.Name + " " + this.args.LogitudeCardName + "s.";
                        list.pop();
                    }
                if (list != null)
                    list.forEach(d => {
                        if (this.isPaymentTerm) {
                            if (d.nameField != null && d.nameField != "")
                                if (d.itemsField["0"] == null)
                                    d.itemsField["0"] = "";
                            this.TempList.push(d);
                        }
                        else if (this.isCurrency || this.isPaymentMethod) {
                            if (d.nameField != null && d.nameField != "")
                                this.TempList.push(d);
                        }
                        else if (this.isReceivablesChargesType) {
                            if (d.incomeAccountRefField != null)
                                this.TempList.push(d);
                        }
                        else if (this.isPayablesChargesType) {
                            if (d.nameField != null && d.nameField != "" && d.accountTypeField != "Accounts Payable" && d.accountSubTypeField != "AccountsPayable")
                                this.TempList.push(d);
                        }
                        else {
                            this.TempList.push(d);
                        }
                    });
                this.CustomersListFilterd = this.TempList;
                this.MyCardsLabel = this.QuantityLabel + " (" + this.CustomersListFilterd.length + ")";
                this.CurrentSession.StopBusyIndicator();
            });

        }



            this.LoadedForTheFirstTime = false;
        }
    }
   





    

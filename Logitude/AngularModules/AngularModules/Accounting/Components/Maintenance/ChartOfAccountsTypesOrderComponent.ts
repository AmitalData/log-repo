import { Component, OnInit } from '@angular/core';
import { ChartOfAccountsTypesExtendedPMService } from 'Accounting/Services/ExtendedPMs/ChartOfAccountsTypesExtendedPMService';
import { ChartOfAccountsTypeListService } from 'Accounting/Services/StandardLists/ChartOfAccountsTypeListService';
import { TenantPM } from 'Common/EntityPMs/TenantPM';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from 'Infrastructure/Tools';


@Component({
    selector: 'ChartOfAccountsTypesOrderComponent',
    
    templateUrl: './ChartOfAccountsTypesOrderComponent.html',   
})

export class ChartOfAccountsTypesOrderComponent extends BaseComponent implements OnInit {
    public DataContext: ChartOfAccountsTypesOrderComponent = this;
    public ObjectTableName: string = "ChartOfAccountsType";
    public TenantPm: TenantPM = new TenantPM();
    chartOfAccountsTypes: any[] = [];
    public demoMessageVisibility: boolean = false;
    public ChartOfAccountsTypesOrders: number[] = [1,2,3,4,5,6,7];
    private CurrentSession = SessionLocator.SelectedSession;
    _chartOfAccountsTypesExtendedPMService: ChartOfAccountsTypesExtendedPMService = new ChartOfAccountsTypesExtendedPMService();
    _chartOfAccountsTypeListService: ChartOfAccountsTypeListService = new ChartOfAccountsTypeListService();
    constructor() {
        super();
    }

    ngOnInit() {
        // this.SetUIProperties();
        this.loadChartOfAccountTypes();
    }
    // SetUIProperties() {
    //     this.UIProperties.SetRequired("RevenuesSelectedItem", this.ObjectTableName, AppTool.IsNullOrEmpty(this.RevenuesSelectedItem));
        
    // }
    private loadChartOfAccountTypes()
    {
        let apiQueryFilters = new ApiQueryFilters(true);

        this._chartOfAccountsTypeListService.getByFilters(apiQueryFilters)
            .subscribe((arg: any) =>
            {
                this.chartOfAccountsTypes = arg.Result;
                this.RevenuesSelectedItem = this.chartOfAccountsTypes[0].Order;
                this.ExpensesSelectedItem = this.chartOfAccountsTypes[1].Order;
                this.CustomersSelectedItem = this.chartOfAccountsTypes[2].Order;
                this.VendorsSelectedItem = this.chartOfAccountsTypes[3].Order;
                this.BanksSelectedItem = this.chartOfAccountsTypes[4].Order;
                this.WorksSelectedItem = this.chartOfAccountsTypes[5].Order;
                this.DebtorsAndCreditorsSelectedItem = this.chartOfAccountsTypes[6].Order;
            });
    }

    RevenuesSelectedChange(item) {
        this.RevenuesSelectedItem = item;
    }

    ExpensesSelectedChange(item) {
        this.ExpensesSelectedItem = item;
    }

    CustomersSelectedChange(item) {
        this.CustomersSelectedItem = item;
    }

    VendorsSelectedChange(item) {
        this.VendorsSelectedItem = item;
    }

    BanksSelectedChange(item) {
        this.BanksSelectedItem = item;
    }

    WorksSelectedChange(item) {
        this.WorksSelectedItem = item;
    }

    DebtorsAndCreditorsSelectedChange(item) {
        this.DebtorsAndCreditorsSelectedItem = item;
    }



    private revenuesSelectedItem: number;
    get RevenuesSelectedItem() { return this.revenuesSelectedItem; }
    set RevenuesSelectedItem(value: number) {
        if (this.revenuesSelectedItem != value) {
            this.revenuesSelectedItem = value;
        }
    }

    private expensesSelectedItem: number;
    get ExpensesSelectedItem() { return this.expensesSelectedItem; }
    set ExpensesSelectedItem(value: number) {
        if (this.expensesSelectedItem != value) {
            this.expensesSelectedItem = value;
        }
    }

    private customersSelectedItem: number;
    get CustomersSelectedItem() { return this.customersSelectedItem; }
    set CustomersSelectedItem(value: number) {
        if (this.customersSelectedItem != value) {
            this.customersSelectedItem = value;
        }
    }
    
    private vendorsSelectedItem: number;
    get VendorsSelectedItem() { return this.vendorsSelectedItem; }
    set VendorsSelectedItem(value: number) {
        if (this.vendorsSelectedItem != value) {
            this.vendorsSelectedItem = value;
        }
    }

    private banksSelectedItem: number;
    get BanksSelectedItem() { return this.banksSelectedItem; }
    set BanksSelectedItem(value: number) {
        if (this.banksSelectedItem != value) {
            this.banksSelectedItem = value;
        }
    }

    private worksSelectedItem: number;
    get WorksSelectedItem() { return this.worksSelectedItem; }
    set WorksSelectedItem(value: number) {
        if (this.worksSelectedItem != value) {
            this.worksSelectedItem = value;
        }
    }

    private debtorsAndCreditorsSelectedItem: number;
    get DebtorsAndCreditorsSelectedItem() { return this.debtorsAndCreditorsSelectedItem; }
    set DebtorsAndCreditorsSelectedItem(value: number) {
        if (this.debtorsAndCreditorsSelectedItem != value) {
            this.debtorsAndCreditorsSelectedItem = value;
        }
    }




    // Commands 
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
 
    public ValidationErrorsList: string[];
    public reloadingTranslation: boolean;

    OkButtonClicked() {
        var errors: string[] = [];
        var orders  = [this.RevenuesSelectedItem, this.ExpensesSelectedItem, this.CustomersSelectedItem, this.VendorsSelectedItem,
            this.BanksSelectedItem,this.WorksSelectedItem, this.DebtorsAndCreditorsSelectedItem]
        // Validator.TryValidateObject(this.DataContext.TenantPm, this.DataContext.ObjectTableName, errors);
        // if (AppTool.IsNullOrEmpty(this.RevenuesSelectedItem)) {
        //     errors.push("Revenues field is required");
        // }
        if(this.hasDuplicates(orders)){
            errors.push("Order can't be duplicated");
        }
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges(orders);
        }
    }
    private hasDuplicates(array) {
        return (new Set(array)).size !== array.length;
    }
    SubmitChanges(orders: number[]) {
        this.CurrentSession.StartBusyIndicator("Saving...");
        this._chartOfAccountsTypesExtendedPMService
        .UpdateChartOfAccountsTypesOrder(orders)
        .subscribe((myResponse: ServiceResponse) => {
            if (myResponse) {
                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
}

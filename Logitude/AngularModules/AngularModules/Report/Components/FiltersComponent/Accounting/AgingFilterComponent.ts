import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../../Components/Filters/QueryFilterItem';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './AgingFilterComponent.html',
})

export class AgingFilterComponent extends BaseComponent implements OnInit {
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    isReady: boolean = false;
    public SalesmanFilterItems: ApiQueryFilters;

    entityResourceService: EntityResourceService = new EntityResourceService();
    public isRTL: boolean = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");


    constructor() {
        super();

        // get requierd resources
        this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(response => { this.isReady = true; });

        // salesman lov field filtera
        this.SalesmanFilterItems = new ApiQueryFilters();
        this.SalesmanFilterItems.addAdditionalFilter("IsSalesman", true, null, null, "Equals", false, false, false, "boolean", false, false);

        // set default value for no of months
        var newDate = new Date();
        var currentMonth = newDate.getMonth()+1;
        //this.NumberOfMonths = currentMonth - 6; // 6 backward
        this.NumberOfMonths = 6; // 6 backward

    }

    ngOnInit() {
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("AgingForDate", "GLAccount", true);
        //this.UIProperties.SetRequired("Customer", "GLAccount", true);
        //this.UIProperties.SetRequired("NumberOfMonths", "GLAccount", true);

        if(this.filterSelectedValue == "filter_vendor"){
            this.UIProperties.SetEnabled("Salesman","GLAccount",false);
            this.UIProperties.SetEnabled("Collector","GLAccount",false);
        }else{
            this.UIProperties.SetEnabled("Salesman","GLAccount",true);
            this.UIProperties.SetEnabled("Collector","GLAccount",true);
        }

    }

    //#region Filters

    //row 1
    private agingForDate: Date = null;
    public get AgingForDate() { return this.agingForDate; }
    public set AgingForDate(value: Date) {
        if (this.agingForDate != value) {
            this.agingForDate = value;

            this.ValidationErrorsList = [];
            this.ValidateDate();
        }
    }

    private customer: string;
    public get Customer() { return this.customer; }
    public set Customer(value: string) {
        if (this.customer != value) {
            this.customer = value;

            if (value)
                this.IsCategoryDisabled = true;
            else
                this.IsCategoryDisabled = false;
        }
    }

    ValidateDate() {
        if (this.AgingForDate) {
            var newDate = new Date();
            var currentDate = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate()+1, 0, 0, 0); // +1 is to include today date to allowed values

            if (this.AgingForDate > currentDate) {
                this.UIProperties.SetValidity("AgingForDate", "GLAccount", false, TextCodeTranslator.Translate("AgingReport.O.FutureDate"));
                return false;
            } else {
                this.UIProperties.SetValidity("AgingForDate", "GLAccount", true, "valid");
                this.UIProperties.SetRequired("AgingForDate", "GLAccount", false);
                return true;
            }
        }
        return true;
    }

    //private chartOfAccount: string;
    //public get ChartOfAccount() { return this.chartOfAccount; }
    //public set ChartOfAccount(value: string) {
    //    if (this.chartOfAccount != value) {
    //        this.chartOfAccount = value;
    //    }
    //}

    private numberOfMonths: number;
    public get NumberOfMonths() { return this.numberOfMonths; }
    public set NumberOfMonths(value: number) {
        if (this.numberOfMonths != value) {
            this.numberOfMonths = value;
        }
    }


    //row 2

    private collector: string;
    public get Collector() { return this.collector; }
    public set Collector(value: string) {
        if (this.collector != value) {
            this.collector = value;
        }
    }

    private salesman: string;
    public get Salesman() { return this.salesman; }
    public set Salesman(value: string) {
        if (this.salesman != value) {
            this.salesman = value;
        }
    }

    //row 3

    private category1: string;
    public get Category1() { return this.category1; }
    public set Category1(value: string) {
        if (this.category1 != value) {
            this.category1 = value;
        }
    }


    private category2: string;
    public get Category2() { return this.category2; }
    public set Category2(value: string) {
        if (this.category2 != value) {
            this.category2 = value;
        }
    }

    private category3: string;
    public get Category3() { return this.category3; }
    public set Category3(value: string) {
        if (this.category3 != value) {
            this.category3 = value;
        }
    }

    private category4: string;
    public get Category4() { return this.category4; }
    public set Category4(value: string) {
        if (this.category4 != value) {
            this.category4 = value;
        }
    }

    //row 4
    private category5: string;
    public get Category5() { return this.category5; }
    public set Category5(value: string) {
        if (this.category5 != value) {
            this.category5 = value;
        }
    }

    private currenciesDetailed: boolean;
    public get CurrenciesDetailed() { return this.currenciesDetailed; }
    public set CurrenciesDetailed(value: boolean) {
        if (this.currenciesDetailed != value) {
            this.currenciesDetailed = value;
        }
    }

    private balance: number;
    public get Balance() { return this.balance; }
    public set Balance(value: number) {
        if (this.balance != value) {
            this.balance = value;
        }
    }


    //#endregion

    RunButtonClicked() {
        this.SetUIProperties();

        var errors: string[] = [];
        var categoryValue = null;
        var categoryIndex = null;
        this.ValidationErrorsList = [];

        //#region requierd fields
        if (!this.AgingForDate) { errors.push("Aging for date field is requierd"); }
        //if (!this.Customer) { errors.push("Customer field is requierd"); }
        if (!this.NumberOfMonths) { errors.push("Number of months field is requierd"); }
        //#endregion

        //#region Date validation
        var isDateValid = this.ValidateDate();
        if (!isDateValid)
            errors.push(TextCodeTranslator.Translate("AgingReport.O.FutureDate"));
        //#endregion

        if (errors.length == 0) {


            // Selecting category
            if (this.SelectedCategory) {
                categoryIndex = this.SelectedCategory.replace(' ', ''); // remove space from selected category

                if (categoryIndex)
                    categoryValue = this.DataContext[categoryIndex]; // select the value from the context
            }

            var myFilterItems: QueryFilterItem[] = [];
            myFilterItems.push(new QueryFilterItem("AgingForDate", this.AgingForDate, "Date"));
            myFilterItems.push(new QueryFilterItem("GLAccountType", this.AccountTypeCode));
            myFilterItems.push(new QueryFilterItem("CustomerId", this.Customer ? this.Customer : null));
            myFilterItems.push(new QueryFilterItem("NumberOfMonths", this.NumberOfMonths, "Number"));
            myFilterItems.push(new QueryFilterItem("CollectorId", this.Collector));
            myFilterItems.push(new QueryFilterItem("SalesmanId", this.Salesman));
            myFilterItems.push(new QueryFilterItem("Detailed", this.CurrenciesDetailed));

            myFilterItems.push(new QueryFilterItem("CategoryIndex", categoryIndex)); // 'Category1' , 'Category2' , ...
            myFilterItems.push(new QueryFilterItem("CategoryValue", categoryValue));

            myFilterItems.push(new QueryFilterItem("BalanceFilter", this.balanceFilterSelectedValue.replace("filter_","")));
            myFilterItems.push(new QueryFilterItem("BalanceFilterValue", this.balance||0));

            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = myFilterItems;

            this.RunReportEvent.emit(myReportFliter);

        } else {
            this.ValidationErrorsList = errors;
        }
    }

    //#region Category fields
    IsCategoryDisabled: boolean = false;
    CategoriesList: string[] = [
        'Category 1',
        'Category 2',
        'Category 3',
        'Category 4',
        'Category 5'
    ];
    SelectedCategory: string;
    SelectedItemChanged(item) {
        this.SelectedCategory = item;
    }
    //#endregion

    //#region Filter Methods
    public filterSelectedValue: string = 'filter_customer';
    public AccountTypeCode: string = '2';
    FilterItemClicked(itemValue: string)
    {
        if (this.filterSelectedValue != itemValue) {
            this.filterSelectedValue = itemValue;
            this.FilterChanged();
        }
    }
    FilterChanged()
    {

        this.Customer = null;
        this.Salesman = null;
        this.Collector = null;
        switch (this.filterSelectedValue) {
            case 'filter_customer':
                this.AccountTypeCode = '2';
                break;
            case 'filter_vendor':
                this.AccountTypeCode = '3';
                break;
            default:
                break;
        }

        this.SetUIProperties();
        this.ValidateDate();

    }
      //#endregion

      public balanceFilterSelectedValue: string = 'filter_All';
      BalanceFilterItemClicked(itemValue: string)
      {
          if (this.balanceFilterSelectedValue != itemValue) {
              this.balanceFilterSelectedValue = itemValue;
              this.BalanceFilterChanged();
          }
      }
      BalanceFilterChanged()
      {

        //   switch (this.balanceFilterSelectedValue) {
        //       case 'filter_All':
        //           this.AccountTypeCode = '2';
        //           break;
        //       case 'filter_Debtors':
        //           this.AccountTypeCode = '2';
        //           break;
        //       case 'filter_DebtAbove':
        //           this.AccountTypeCode = '3';
        //           break;
        //       default:
        //           break;
        //   }

          this.SetUIProperties();

      }

}

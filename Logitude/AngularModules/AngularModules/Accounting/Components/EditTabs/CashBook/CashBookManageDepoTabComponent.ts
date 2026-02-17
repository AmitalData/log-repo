import {Component}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {CashBookPM} from '../../../EntityPMs/CashBookPM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {BankDepositList} from '../../../EntityLists/BankDepositList';
import {BankDepositListService} from '../../../Services/StandardLists/BankDepositListService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './CashBookManageDepoTabComponent.html',
})

export class CashBookManageDepoTabComponent extends BaseComponent {
    public EntityPM: CashBookPM = null;
    public ObjectTableName = "CashBook";
    public DataContext = this;
    dateFilter: FilterItem;
    searchFieldFilter: FilterItem;
    _BankDepositListService: BankDepositListService = new BankDepositListService();
    ItemsSource: BankDepositList[] = [];
    TotalSum: number = 0;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        var today = new Date();
        this.ToDate = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1)
        this.FromDate = new Date(lastmonth);

        this.tenantCurrency = SessionLocator.TenantPM.CurrencyCode;

    }

    // Properties
    private fromDate: Date;
    get FromDate() { return this.fromDate; }
    set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;

            if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
                this.dateFilter = new FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                this.GetDeposits();
            }
        }
    }

    toDate: Date;
    get ToDate() { return this.toDate; }
    set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;

            if (!AppTool.IsNullOrEmpty(this.ToDate) && !AppTool.IsNullOrEmpty(this.FromDate)) {
                this.dateFilter = new FilterItem("CreateDate", new Date(this.FromDate.getFullYear(), this.FromDate.getMonth(), this.FromDate.getDate(), 0, 0, 0), new Date(this.ToDate.setHours(23, 59, 59, 59)), null, "Between", false, false, false, "Date", false);
                this.GetDeposits();
            }
        }
    }

    public tenantCurrency: string = "";


    GetDeposits() {
        var filters = new ApiQueryFilters;
        if (this.dateFilter) {
            filters.AdditionalFilters.push(this.dateFilter);
        }
        if (this.searchFieldFilter) {
            filters.AdditionalFilters.push(this.searchFieldFilter);
        }
        filters.GetAll = true;
        filters.SortBy = "CreateDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("CashBookId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");

        this._BankDepositListService.getByFilters(filters).subscribe(myResult => {
            console.log("Response: ", myResult);
            if (myResult == null) {
                this.ItemsSource = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.ItemsSource = myResponse.Result;

                    // Calculate commulative sums 
                    this.TotalSum = 0;
                    for (var i = 0; i < this.ItemsSource.length; i++) {
                        this.TotalSum += this.ItemsSource[i].ForeignAmount;
                    }
                }
            }
        });
    }

    RowClicked(item) {
        if (!AppTool.IsNullOrEmpty(item)) {

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({
                        EntityId: item.Id,
                        ObjectTableName: 'BankDeposit',
                        BackButtonLabel: 'Cashbook'
                    });
                });
        }
    }

    private timerToken: any;
    TextChanged(searchtext) {
        if (searchtext != null || searchtext != undefined) {

            this.timerToken = setTimeout(() => {
                this.searchFieldFilter = new FilterItem("SearchFields", searchtext, null, null, "Contains", false, false, false, "string", false);
                this.GetDeposits();
            }, 700);

        } else {
            this.searchFieldFilter = null;
            this.GetDeposits();
        }
    }

}

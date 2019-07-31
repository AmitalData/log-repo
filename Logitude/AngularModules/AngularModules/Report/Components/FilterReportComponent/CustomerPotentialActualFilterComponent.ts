import {Component}  from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {CodeNameClass} from '../../../Infrastructure/DataContracts/CodeNameClass';
import {AppTool} from '../../../Infrastructure/Tools';
import {ProductTypeListService} from '../../../Common/Services/StandardLists/ProductTypeListService';
import {ProductTypeList} from '../../../Common/EntityLists/ProductTypeList';
import {BusinessUnitListService} from '../../../Common/Services/StandardLists/BusinessUnitListService';
import {BusinessUnitList} from '../../../Common/EntityLists/BusinessUnitList';
import {UserListService} from '../../../Common/Services/StandardLists/UserListService';
import {UserList} from '../../../Common/EntityLists/UserList';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ReportService, CustomersDataProvider, CustomersData} from '../../../Common/Services/ExtendedLists/ReportService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    selector: 'CustomerPotentialActualFilterComponent',
    templateUrl: './CustomerPotentialActualFilterComponent.html',
})

export class CustomerPotentialActualFilterComponent extends BaseComponent {
    public DataContext: CustomerPotentialActualFilterComponent = this;
    public ItemsSource: CustomerSummary[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.UpdateColumns();
        this.BuildFilters();
    } 
    
    public AD_IsVisible: boolean = false;
    public AR_IsVisible: boolean = false;
    public AE_IsVisible: boolean = false;
    public AI_IsVisible: boolean = false;
    public CI_IsVisible: boolean = false;
    public DL_IsVisible: boolean = false;
    public ID_IsVisible: boolean = false;
    public IR_IsVisible: boolean = false;
    public IE_IsVisible: boolean = false;
    public II_IsVisible: boolean = false;
    public IN_IsVisible: boolean = false;
    public OD_IsVisible: boolean = false;
    public OR_IsVisible: boolean = false;
    public OE_IsVisible: boolean = false;
    public OI_IsVisible: boolean = false;

    private UpdateColumns() {
        var ad_IsVisible: boolean = false;
        var ar_IsVisible: boolean = false;
        var ae_IsVisible: boolean = false;
        var ai_IsVisible: boolean = false;
        var ci_IsVisible: boolean = false;
        var dl_IsVisible: boolean = false;
        var id_IsVisible: boolean = false;
        var ir_IsVisible: boolean = false;
        var ie_IsVisible: boolean = false;
        var ii_IsVisible: boolean = false;
        var in_IsVisible: boolean = false;
        var od_IsVisible: boolean = false;
        var or_IsVisible: boolean = false;
        var oe_IsVisible: boolean = false;
        var oi_IsVisible: boolean = false;
        
        var proeductTypeListService: ProductTypeListService = new ProductTypeListService();
        proeductTypeListService.getAllFromCache().subscribe((response: ServiceResponse) => {
            var list: ProductTypeList[] = response.Result;

            list.filter(d => !d.InActive).forEach((item) => {
                if (this.SelectedProdustTypeFilter == "All") {
                    if (item.Code == "AD") {
                        ad_IsVisible = true;
                    }

                    else if (item.Code == "AR") {
                        ar_IsVisible = true;
                    }

                    else if (item.Code == "AE") {
                        ae_IsVisible = true;
                    }

                    else if (item.Code == "AI") {
                        ai_IsVisible = true;
                    }

                    else if (item.Code == "CI") {
                        ci_IsVisible = true;
                    }

                    else if (item.Code == "DL") {
                        dl_IsVisible = true;
                    }

                    else if (item.Code == "ID") {
                        id_IsVisible = true;
                    }

                    else if (item.Code == "IR") {
                        ir_IsVisible = true;
                    }

                    else if (item.Code == "IE") {
                        ie_IsVisible = true;
                    }

                    else if (item.Code == "II") {
                        ii_IsVisible = true;
                    }

                    else if (item.Code == "IN") {
                        in_IsVisible = true;
                    }

                    else if (item.Code == "OD") {
                        od_IsVisible = true;
                    }

                    else if (item.Code == "OR") {
                        or_IsVisible = true;
                    }

                    else if (item.Code == "OE") {
                        oe_IsVisible = true;
                    }

                    else if (item.Code == "OI") {
                        oi_IsVisible = true;
                    }
                }

                else {
                    if (this.mySelectedProductsList.indexOf(item.Code) > -1) {
                        if (item.Code == "AD") {
                            ad_IsVisible = true;
                        }

                        else if (item.Code == "AR") {
                            ar_IsVisible = true;
                        }

                        else if (item.Code == "AE") {
                            ae_IsVisible = true;
                        }

                        else if (item.Code == "AI") {
                            ai_IsVisible = true;
                        }

                        else if (item.Code == "CI") {
                            ci_IsVisible = true;
                        }

                        else if (item.Code == "DL") {
                            dl_IsVisible = true;
                        }

                        else if (item.Code == "ID") {
                            id_IsVisible = true;
                        }

                        else if (item.Code == "IR") {
                            ir_IsVisible = true;
                        }

                        else if (item.Code == "IE") {
                            ie_IsVisible = true;
                        }

                        else if (item.Code == "II") {
                            ii_IsVisible = true;
                        }

                        else if (item.Code == "IN") {
                            in_IsVisible = true;
                        }

                        else if (item.Code == "OD") {
                            od_IsVisible = true;
                        }

                        else if (item.Code == "OR") {
                            or_IsVisible = true;
                        }

                        else if (item.Code == "OE") {
                            oe_IsVisible = true;
                        }

                        else if (item.Code == "OI") {
                            oi_IsVisible = true;
                        }
                    }
                }
            });
        });

        this.AD_IsVisible = ad_IsVisible;
        this.AR_IsVisible = ar_IsVisible;
        this.AE_IsVisible = ae_IsVisible;
        this.AI_IsVisible = ai_IsVisible;
        this.CI_IsVisible = ci_IsVisible;
        this.DL_IsVisible = dl_IsVisible;
        this.ID_IsVisible = id_IsVisible;
        this.IR_IsVisible = ir_IsVisible;
        this.IE_IsVisible = ie_IsVisible;
        this.II_IsVisible = ii_IsVisible;
        this.IN_IsVisible = in_IsVisible;
        this.OD_IsVisible = od_IsVisible;
        this.OR_IsVisible = or_IsVisible;
        this.OE_IsVisible = oe_IsVisible;
        this.OI_IsVisible = oi_IsVisible;
    }

    private BuildFilters() {
        this.BuildTimeRangeFilters();
        this.BuildProductTypesFilters();
        this.BuildBusinessUnitFilter();
        this.BuildViewByFilters();
        this.BuildProductsFilter();
    }

    private GetMonthName(monthNumber: number) {
        var monthName: string = null;

        switch (monthNumber) {
            case 1: { monthName = "January"; break; }
            case 2: { monthName = "February"; break; }
            case 3: { monthName = "March"; break; }
            case 4: { monthName = "April"; break; }
            case 5: { monthName = "May"; break; }
            case 6: { monthName = "June"; break; }
            case 7: { monthName = "July"; break; }
            case 8: { monthName = "August"; break; }
            case 9: { monthName = "September"; break; }
            case 10: { monthName = "October"; break; }
            case 11: { monthName = "November"; break; }
            case 12: { monthName = "December"; break; }
        }

        return monthName;
    }

    private countryId: string;
    get CountryId() { return this.countryId; }
    set CountryId(value: string) {
        if (this.countryId != value) {
            this.countryId = value;
        }
    }

    //Time Ranges
    public TimeRangeComboList: CodeNameClass[];
    private BuildTimeRangeFilters() {
        this.TimeRangeComboList = [];
        
        var lastMonth: Date = new Date();

        this.TimeRangeComboList.push(new CodeNameClass("L1M", this.GetMonthName(lastMonth.getMonth()) + " " + lastMonth.getFullYear()));
        this.TimeRangeComboList.push(new CodeNameClass("L3M", "Average of last 3 months"));
        this.TimeRangeComboList.push(new CodeNameClass("L12M", "Average of last 12 months"));
        
        this.selectedTimeRangeFilter = this.TimeRangeComboList.filter(d => d.Code == "L1M")[0];
    }

    private selectedTimeRangeFilter: CodeNameClass;
    get SelectedTimeRangeFilter() { return this.selectedTimeRangeFilter; }
    set SelectedTimeRangeFilter(value: CodeNameClass) {
        if (this.selectedTimeRangeFilter != value) {
            this.selectedTimeRangeFilter = value;
        }
    }

    //Product Types
    public ProductTypeComboList: ProductTypeItemClass[];
    private BuildProductTypesFilters() {
        this.ProductTypeComboList = [];

        var proeductTypeListService: ProductTypeListService = new ProductTypeListService();
        proeductTypeListService.getAllFromCache().subscribe((response: ServiceResponse) => {
            var list: ProductTypeList[] = response.Result;

            list.filter(d => !d.InActive).sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 }).forEach((item) => {
                this.ProductTypeComboList.push(new ProductTypeItemClass(item));
            });
        });
    }
    
    public SelectedProdustTypeFilter: any = "All";

    EditedItemSource(newSource: any) {
        this.ProductTypeComboList = newSource;

    }
    // Business Units
    public OwnerId: string;
    public BusinessUnitId: string;
    public UsersFilterList: CodeNameClass[] = [];
    public BusinessUnitFilterList: CodeNameClass[] = [];
    private BuildBusinessUnitFilter() {
        var myBusinessUnitListService: BusinessUnitListService = new BusinessUnitListService();

        myBusinessUnitListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: BusinessUnitList[] = myResponse.Result;

                this.BusinessUnitFilterList = [];
                this.BusinessUnitFilterList.push(new CodeNameClass("M", "My Records"));

                if (list) {
                    list.filter(d => d.Id != SessionLocator.Tenant.toString()).forEach((item) => {
                        this.BusinessUnitFilterList.push(new CodeNameClass(item.Id, item.Name));
                    });
                }

                this.BusinessUnitFilterList.push(new CodeNameClass("A", "All Records"));
                
                this.selectedBusinessUnitFilter = this.BusinessUnitFilterList.filter(d => d.Code == "M")[0];
                this.GetSelectedBusinessUnitId();
                this.BuildUsersFilters();
            }
        });
    }
    GetSelectedBusinessUnitId() {
        var myResult: string = null;

        if (this.SelectedBusinessUnitFilter) {
            switch (this.SelectedBusinessUnitFilter.Code) {
                case "M": {
                    myResult = SessionLocator.LoggedUserPM.BusinessUnitId;
                    break;
                }

                case "A": {
                    myResult = null;
                    break;
                }

                default: {
                    myResult = this.SelectedBusinessUnitFilter.Code;
                    break;
                }
            }
        }

        this.BusinessUnitId = myResult;
    }
    BuildUsersFilters() {
        this.UsersFilterList = [];

        if (this.SelectedBusinessUnitFilter == null) {
            this.selectedUserFilter = null;
            this.GetSelectedOwnerId();
        }

        else {
            switch (this.SelectedBusinessUnitFilter.Code) {
                case "M":
                    {
                        var item = new CodeNameClass(SessionLocator.LoggedUserId, SessionLocator.LoggedUserPM.EnglishName);
                        this.UsersFilterList.push(item);
                        this.selectedUserFilter = item;
                        this.GetSelectedOwnerId();
                        break;
                    }

                case "A":
                    {
                        var item = new CodeNameClass("A", "All Owners");
                        this.UsersFilterList.push(item);

                        this.OwnerId = null;
                        this.listOfValuesUserId = null;
                        this.selectedUserFilter = null;
                        this.GetSelectedOwnerId();

                        break;
                    }

                default:
                    {
                        var item = new CodeNameClass("A", "All " + this.SelectedBusinessUnitFilter.Name + " Owners");
                        this.UsersFilterList.push(item);

                        var filters = new ApiQueryFilters();
                        filters.PageIndex = 0;
                        filters.PageSize = 100;
                        filters.Filter1Name = "BusinessUnitId";
                        filters.Filter1Value = this.BusinessUnitId;
                        filters.Filter1Operator = "Equals";

                        var myUserListService: UserListService = new UserListService();
                        myUserListService.getAllFromCache(filters).subscribe((myResponse: ServiceResponse) => {
                            if (!myResponse.HasError) {
                                var loadedUsers: UserList[] = myResponse.Result;

                                if (loadedUsers != null) {
                                    loadedUsers.forEach((list) => {
                                        this.UsersFilterList.push(new CodeNameClass(list.Id, list.EnglishName));
                                    });
                                }
                            }

                            this.OwnerId = null;
                            this.listOfValuesUserId = null;
                            this.selectedUserFilter = item;
                            this.listOfValuesUserId = this.OwnerId;

                            if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                                this.selectedUserFilter = this.UsersFilterList.filter(d => d.Code == this.OwnerId)[0];
                            }

                            if (this.selectedUserFilter == null) {
                                this.selectedUserFilter = this.UsersFilterList[0];
                            }
                        });

                        break;
                    }
            }
        }
    }
    GetSelectedOwnerId() {
        var myResult = null;
        this.listOfValuesUserId = null;

        if (this.SelectedUserFilter) {
            switch (this.SelectedUserFilter.Code) {
                case "A": {
                    this.listOfValuesUserId = null;
                    break;
                }

                default: {
                    myResult = this.SelectedUserFilter.Code;
                    this.listOfValuesUserId = myResult;
                    break;
                }
            }
        }

        this.OwnerId = myResult;
    }

    private selectedBusinessUnitFilter: CodeNameClass;
    get SelectedBusinessUnitFilter() { return this.selectedBusinessUnitFilter; }
    set SelectedBusinessUnitFilter(value: CodeNameClass) {
        if (this.selectedBusinessUnitFilter != value) {
            this.selectedBusinessUnitFilter = value;

            this.GetSelectedBusinessUnitId();
            this.BuildUsersFilters();
        }
    }

    private selectedUserFilter: CodeNameClass;
    get SelectedUserFilter() { return this.selectedUserFilter; }
    set SelectedUserFilter(value: CodeNameClass) {
        if (this.selectedUserFilter != value) {
            this.selectedUserFilter = value;

            if (value == null) {
                this.OwnerId = null;
            }

            else if (value.Code == "A") {
                this.OwnerId = null;
            }

            else {
                this.OwnerId = value.Code;
            }
        }
    }

    private listOfValuesUserId: string;
    get ListOfValuesUserId() { return this.listOfValuesUserId; }
    set ListOfValuesUserId(value: string) {
        if (this.listOfValuesUserId != value) {

            this.OwnerId = value;
            this.listOfValuesUserId = value;
        }
    }
    
    //View By
    public ViewByComboList: CodeNameClass[];
    private BuildViewByFilters() {
        this.ViewByComboList = [];
        
        this.ViewByComboList.push(new CodeNameClass("NSH", "No. Of Shipments"));
        this.ViewByComboList.push(new CodeNameClass("TEU", "TEU"));
        this.ViewByComboList.push(new CodeNameClass("CHW", "Chargeable Weight"));
        this.ViewByComboList.push(new CodeNameClass("REV", "Revenue"));

        this.selectedViewByFilter = this.ViewByComboList.filter(d => d.Code == "NSH")[0];
    }
    
    private selectedViewByFilter: CodeNameClass;
    get SelectedViewByFilter() { return this.selectedViewByFilter; }
    set SelectedViewByFilter(value: CodeNameClass) {
        if (this.selectedViewByFilter != value) {
            this.selectedViewByFilter = value;
        }
    }
    
    //Product
    public ProductFilterList: CodeNameClass[];
    private BuildProductsFilter() {
        this.ProductFilterList = [];

        this.ProductFilterList.push(new CodeNameClass("ALL", "All"));
        this.ProductFilterList.push(new CodeNameClass("POT", "Potential Only"));
        this.ProductFilterList.push(new CodeNameClass("ACT", "Actual Only"));
        this.ProductFilterList.push(new CodeNameClass("NON", "None"));

        this.selectedProductFilter = this.ProductFilterList.filter(d => d.Code == "ALL")[0];
    }

    private selectedProductFilter: CodeNameClass;
    get SelectedProductFilter() { return this.selectedProductFilter; }
    set SelectedProductFilter(value: CodeNameClass) {
        if (this.selectedProductFilter != value) {
            this.selectedProductFilter = value;
        }
    }

    private reportFliter: ReportFliter;
    private queryFilterItems: QueryFilterItem[];
    private mySelectedProductsList: string[];
    public ValidationErrorsList: string[];
    RunReport() {
        this.ValidationErrorsList = [];

        if (this.SelectedProdustTypeFilter != "All") {
            if (this.ProductTypeComboList.filter(d => d.Checked).length == 0) {
                this.ValidationErrorsList.push("Please select product type");
            }
        }

        if (!this.SelectedTimeRangeFilter) {
            this.ValidationErrorsList.push("Time Range field is required");
        }

        if (!this.SelectedViewByFilter) {
            this.ValidationErrorsList.push("View by field is required");
        }

        if (!this.SelectedProductFilter) {
            this.ValidationErrorsList.push("Product field is required");
        }

        if (!this.SelectedBusinessUnitFilter) {
            this.ValidationErrorsList.push("Business unit field is required");
        }

        if (this.SelectedBusinessUnitFilter) {
            if (this.SelectedBusinessUnitFilter.Code != "A") {
                if (!this.SelectedUserFilter) {
                    this.ValidationErrorsList.push("User field is required");
                }
            }
        }

        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();

            var myProductTypes: string = "";

            if (this.SelectedProdustTypeFilter == "All") {
                myProductTypes = "All";
            }

            else {
                this.ProductTypeComboList.forEach((i) => {
                    if (i.Checked) {
                        myProductTypes += i.Code + ",";
                    }
                });
            }

            var myProductsText: string = myProductTypes.trim();
            this.mySelectedProductsList = myProductsText.split(',');
            this.UpdateColumns();

            var queryFilterItem1 = new QueryFilterItem();
            queryFilterItem1.DisplayInList = false;
            queryFilterItem1.FieldName = "DataTypeCode";
            queryFilterItem1.FieldValue = this.SelectedViewByFilter.Code;
            queryFilterItem1.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem1);

            var queryFilterItem2 = new QueryFilterItem();
            queryFilterItem2.DisplayInList = false;
            queryFilterItem2.FieldName = "TimeRange";
            queryFilterItem2.FieldValue = this.SelectedTimeRangeFilter.Code;
            queryFilterItem2.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem2);

            var queryFilterItem3 = new QueryFilterItem();
            queryFilterItem3.DisplayInList = false;
            queryFilterItem3.FieldName = "ProductsTypes";
            queryFilterItem3.FieldValue = myProductTypes;
            queryFilterItem3.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem3);

            var queryFilterItem4 = new QueryFilterItem();
            queryFilterItem4.DisplayInList = false;
            queryFilterItem4.FieldName = "BusinessUnitId";
            queryFilterItem4.FieldValue = this.BusinessUnitId;
            queryFilterItem4.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem4);

            var queryFilterItem5 = new QueryFilterItem();
            queryFilterItem5.DisplayInList = false;
            queryFilterItem5.FieldName = "OwnerId";
            queryFilterItem5.FieldValue = this.OwnerId;
            queryFilterItem5.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem5);

            var queryFilterItem6 = new QueryFilterItem();
            queryFilterItem6.DisplayInList = false;
            queryFilterItem6.FieldName = "CountryId";
            queryFilterItem6.FieldValue = this.CountryId;
            queryFilterItem6.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem6);

            var queryFilterItem7 = new QueryFilterItem();
            queryFilterItem7.DisplayInList = false;
            queryFilterItem7.FieldName = "ProductCode";
            queryFilterItem7.FieldValue = this.SelectedProductFilter.Code;
            queryFilterItem7.Operator = "Equals";
            this.queryFilterItems.push(queryFilterItem7);

            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = "CustomerPotentialActualFilterControl";
            this.reportFliter.ReportCode = "CUPA";
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";

            this.GenerateReport(this.reportFliter);
        }
    }

    private GenerateReport(filter: ReportFliter) {
            this.CurrentSession.StartBusyIndicatorLoading();
        
            var reportService: ReportService = new ReportService();
            reportService.GenerateReportForCustomerPotentialActual(filter).subscribe((myResponse: ServiceResponse) => {                
                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show(myResponse.ErrorsArray[0]);
                }

                else {
                    var myResult: CustomersDataProvider = myResponse.Result;
                    if (myResult != null) {
                        this.BuildItemsSource(myResult);
                    }
                }
            });
    }

    private BuildItemsSource(myResult: CustomersDataProvider) {
        this.ItemsSource = [];
        myResult.Customers.forEach((i) => {
            this.ItemsSource.push(new CustomerSummary(i, this));
        });
    }
}

export class ProductTypeItemClass {
    public entityList: ProductTypeList;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityList: ProductTypeList) {
        this.entityList = entityList;
    }

    get Code() { return this.entityList.Code; }
    get Name() { return this.entityList.Name; }  

    private checked: boolean;
    public get Checked() { return this.checked; }
    public set Checked(value: boolean) { this.checked = value; }
}

export class CustomerSummary {
    private entity: CustomersData;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(myEntity: CustomersData, public fatherComponent: CustomerPotentialActualFilterComponent) {
        this.entity = myEntity;
    }

    get CustomerId() { return this.entity.CustomerId; }
    get CustomerName() { return this.entity.CustomerName; }
    get PrimaryContactName() { return this.entity.PrimaryContactName; }
    get PrimaryContactEmail() { return this.entity.PrimaryContactEmail; }
    get Salesman() { return this.entity.Salesman; }

    get AD_POT() { return this.entity.AD_POT; }
    get AD_ACT() { return this.entity.AD_ACT; }

    get AR_POT() { return this.entity.AR_POT; }
    get AR_ACT() { return this.entity.AR_ACT; }

    get AE_POT() { return this.entity.AE_POT; }
    get AE_ACT() { return this.entity.AE_ACT; }

    get AI_POT() { return this.entity.AI_POT; }
    get AI_ACT() { return this.entity.AI_ACT; }

    get CI_POT() { return this.entity.CI_POT; }
    get CI_ACT() { return this.entity.CI_ACT; }

    get DL_POT() { return this.entity.DL_POT; }
    get DL_ACT() { return this.entity.DL_ACT; }

    get ID_POT() { return this.entity.ID_POT; }
    get ID_ACT() { return this.entity.ID_ACT; }

    get IR_POT() { return this.entity.IR_POT; }
    get IR_ACT() { return this.entity.IR_ACT; }

    get IE_POT() { return this.entity.IE_POT; }
    get IE_ACT() { return this.entity.IE_ACT; }

    get II_POT() { return this.entity.II_POT; }
    get II_ACT() { return this.entity.II_ACT; }

    get IN_POT() { return this.entity.IN_POT; }
    get IN_ACT() { return this.entity.IN_ACT; }

    get OD_POT() { return this.entity.OD_POT; }
    get OD_ACT() { return this.entity.OD_ACT; }

    get OR_POT() { return this.entity.OR_POT; }
    get OR_ACT() { return this.entity.OR_ACT; }

    get OE_POT() { return this.entity.OE_POT; }
    get OE_ACT() { return this.entity.OE_ACT; }

    get OI_POT() { return this.entity.OI_POT; }
    get OI_ACT() { return this.entity.OI_ACT; }    

    ViewCustomerClicked() {
        if (!AppTool.IsNullOrEmpty(this.CustomerId)) {            
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.CustomerId, ObjectTableName: "Customer", BackButtonLabel: "Reports" });

                    let isEditComponentSaved = false;
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        if (isEditComponentSaved) {
                            this.fatherComponent.RunReport();
                        }
                    });

                    cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });
                });
        }
    }   
}

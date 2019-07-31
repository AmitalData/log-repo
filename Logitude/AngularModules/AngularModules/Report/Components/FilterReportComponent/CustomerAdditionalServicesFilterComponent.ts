import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ReportsDomainService} from '../../Services/ReportsDomainService';
import {UserListService} from '../../../Common/Services/StandardLists/UserListService';
import {CodeNameClass} from './CodeNameClass';

@Component({
    moduleId: module.id,
    selector: 'CustomerAdditionalServicesFilterComponent',
    templateUrl: './CustomerAdditionalServicesFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class CustomerAdditionalServicesFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    public FilterdAdditionalService: any;
    private listOfValuesUserId: string = null;    
    private reportDomainService: ReportsDomainService;
    private userListService: UserListService;

    public IsAllId: string;
    public IsPotentialId: string;
    public IsInUseId: string;
    public TypeRadio: string;

    public TenantPM: TenantPM;

    queryFilterItems: QueryFilterItem[];
    public CustomerId = null;
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    public DataContext: CustomerAdditionalServicesFilterComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.IsAllId = "_IsAllId" + this.CurrentSession.GetNewId("_IsAllId");
        this.IsPotentialId = "_IsPotentialId" + this.CurrentSession.GetNewId("_IsPotentialId");
        this.IsInUseId = "_IsInUseId" + this.CurrentSession.GetNewId("_IsInUseId");
        this.TypeRadio = "_TypeRadio" + this.CurrentSession.GetNewId("_TypeRadio");
    }

    public CustomerStatusList: CodeNameClass[];
    private BuildCustomerStatusFilters() {
        this.CustomerStatusList = [];
        this.CustomerStatusList.push(new CodeNameClass("ALL", "All"));
        this.CustomerStatusList.push(new CodeNameClass("ACT", "Active"));
        this.CustomerStatusList.push(new CodeNameClass("POT", "Potential"));

        this.selectedCustomerStatus = this.CustomerStatusList.filter(d => d.Code == "ALL")[0];
    }

    private selectedCustomerStatus: CodeNameClass;
    get SelectedCustomerStatus() { return this.selectedCustomerStatus; }
    set SelectedCustomerStatus(value: CodeNameClass) {
        if (this.selectedCustomerStatus != value) {
            this.selectedCustomerStatus = value;
        }
    }

    public get ListOfValuesUserId() {        
        if (this.listOfValuesUserId != null) {
            return this.listOfValuesUserId;
        }

        else {
            return null;
        }
    }
    public set ListOfValuesUserId(value: any) {
        this.listOfValuesUserId = value;

        if (value == "" || value == null) {
            this.SalesmanUserId = null;
            this.BusinessUnitId = null;
        }

        else {
            this.UsersCachedList.forEach((i) => {
                if (i.Id == value + "") {
                    this.SalesmanUserId = i.Id;
                    this.BusinessUnitId = i.BusinessUnitId;
                    return;
                }
            });
        }
    }
       
    public IsListOfValuesVisible() {
        var visible: boolean = false;

        if (this.SelectedItemBusniessFilterd != null) {
            if (this.SelectedItemBusniessFilterd.Code == "A") {
                visible = true;
            }

            else {
                visible = false;
            }
        }

        else {
            return visible;
        }
    }
        
    public SalesmanUserId: string = SessionLocator.LoggedUserPM.Id;
    public BusinessUnitId: string = SessionLocator.LoggedUserPM.BusinessUnitId;
    public UsersCachedList: any;
    
    public selectedItemBusniessFilterd: any;
    public IsVisibale: boolean = false;

    public get SelectedItemBusniessFilterd() {
        return this.selectedItemBusniessFilterd;
    }
    public set SelectedItemBusniessFilterd(value: any) {
        this.selectedItemBusniessFilterd = value;    }

    public UsersFilterList: Array<CodeNameClass>;
    public BusniessItemSource: Array<CodeNameClass>;

    public get ServiceType() {
        if (this.IsAll)
            return "All";

        else if (this.IsInUse)
            return "In Use";

        else
            return "Potential";
    }
    
    CheckUsersEnabled() {
        if (this.SelectedItemBusniessFilterd != null)
            if (this.SelectedItemBusniessFilterd.Code == 'M')
                return true;

        if (this.UsersFilterList != null)
            if (this.UsersFilterList.length == 0)
                return true;

        return false;
    }

    SelectedBusniessUnitChanged(item1) {
        this.SelectedItemBusniessFilterd = item1;
        this.UsersFilterList = [];

        if (this.SelectedItemBusniessFilterd != null) {
            switch (this.SelectedItemBusniessFilterd.Code) {

                case "M": {
                    var item = new CodeNameClass();
                    item.Code = SessionLocator.LoggedUserPM.Id;
                    item.Name = SessionLocator.LoggedUserPM.EnglishName;
                    this.UsersFilterList.push(item);
                    this.ListOfValuesUserId = this.TenantPM.Id;
                    this.SalesmanUserId = SessionLocator.LoggedUserPM.Id;
                    this.BusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;

                    break;
                }

                case "A": {
                    var item = new CodeNameClass();
                    item.Code = "A";
                    item.Name = "All Owners";
                    this.UsersFilterList.push(item);
                    this.SalesmanUserId = null;
                    this.ListOfValuesUserId = null;

                    break;
                }

                default: {
                    var item = new CodeNameClass();
                    item.Code = "A";
                    item.Name = "All " + this.SelectedItemBusniessFilterd.Name + " Owners";
                    this.UsersFilterList.push(item);
                    this.SalesmanUserId = null;
                    this.ListOfValuesUserId = null;
                    this.getFromCachedList(this.UsersCachedList);

                    break;
                }
            }
        }
    }

    getFromCachedList(myResult: any) {
        this.UsersCachedList = myResult;
        myResult.forEach((i) => {
            if (i.BusinessUnitId == this.SelectedItemBusniessFilterd.Code + "") {
                var item = new CodeNameClass();
                item.Code = i.Id;
                item.Name = i.EnglishName;
                this.UsersFilterList.push(item);
            }
        });

        if (this.UsersFilterList.length > 0) {
            this.UsersFilterList.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });
        }
    }  

    BuildBusinessUnitFilterList(myResult: any) {
        var item: CodeNameClass = new CodeNameClass();

        item.Code = "M";
        item.Name = "My Records";
        this.BusniessItemSource.push(item);
        myResult.forEach((i) => {
            if (i.Id != this.TenantPM.Id + "") {
                var item = new CodeNameClass();
                item.Code = i.Id;
                item.Name = i.Name;
                this.BusniessItemSource.push(item);
            }
        });

        item = new CodeNameClass();
        item.Code = "A";
        item.Name = "All Records";
        this.BusniessItemSource.push(item);
        this.SelectedBusniessUnitChanged( this.BusniessItemSource[0]);
    }
     
    fillcombo(arr: any) {
        this.FilterdAdditionalService = [];

        arr.forEach((i) => {
            if (!i.InActive) {
                var item = new CodeNameClass();
                item.Code = i.Id;
                item.Name = i.Name;
                item.Checked = false;
                this.FilterdAdditionalService.push(i);
            }

            else {

            }
        });

        this.FilterdAdditionalService.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });    
    }

    public IsAll: boolean = true;
    public IsInUse: boolean = false;
    public IsPotential: boolean = false;

    IsAllClicked() {
        this.IsAll = true;
        this.IsPotential = false;
        this.IsInUse = false;
    }
    IsPotentialClicked() {
        this.IsAll = false;
        this.IsPotential = true;
        this.IsInUse = false;
    }
    IsInUseClicked() {
        this.IsAll = false;
        this.IsPotential = false;
        this.IsInUse = true;
    }
    
    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.userListService = new UserListService();
        this.reportDomainService = new ReportsDomainService();
        this.TenantPM = SessionLocator.TenantPM;

        this.BuildCustomerStatusFilters();

        this.reportDomainService.GetBusinessUnitLists(this.TenantPM.Id).subscribe((myResult: any) => {
            this.BusniessItemSource = new Array<CodeNameClass>();
            this.BuildBusinessUnitFilterList(myResult);
        });

        this.reportDomainService.GetAdditionalServicesByTenant(this.TenantPM.Id).subscribe((myResult: any) => {
            this.fillcombo(myResult);
        });

        this.userListService.getAllFromCache().subscribe((myResult: any) => {
            this.UsersCachedList = myResult.Result;
        });
    }
    
    ngOnInit() {
        
    }            

    EditedItemSource(newSource: any) {
        this.FilterdAdditionalService = newSource;
    }

    public SelectedItem: string = "All";
    SelectedAdditionalServiceChanged(item) {
        this.SelectedItem = item;
    }    
    public ValidationErrorsList: string[];

    RunReport(isloading: boolean) {
        this.ValidationErrorsList = [];

        if (!this.SelectedCustomerStatus) {
            this.ValidationErrorsList.push("Customer field is required");
        }


        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();

            var myAdditionalServices: string = "";

            if (this.SelectedItem == "All") {
                myAdditionalServices = "All";
            }

            else {
                this.FilterdAdditionalService.forEach((i) => {
                    if (i.Checked) {
                        myAdditionalServices += i.Id + ",";
                    }
                });
            }

            if (this.SelectedCustomerStatus.Code == "ALL") {
                this.SelectedCustomerStatus.Code = "";
            }

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "AdditionalServices";
            this.queryFilterItem.FieldValue = myAdditionalServices;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "BusinessUnitId";
            this.queryFilterItem.FieldValue = this.BusinessUnitId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "SalesmanUserId";
            this.queryFilterItem.FieldValue = this.SalesmanUserId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ServiceType";
            this.queryFilterItem.FieldValue = this.ServiceType;
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerStatus";
            this.queryFilterItem.FieldValue = this.SelectedCustomerStatus.Code;
            this.queryFilterItems.push(this.queryFilterItem);

            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    }
}



declare var JSZip: any;

declare var System: any;
declare var window: any;
import {Component, OnInit, ElementRef}  from '@angular/core';

import {ListComponentArgs} from '../../Infrastructure/Args';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ServiceArgs} from '../../Infrastructure/DataContracts/ServiceArgs';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {WindowArgs} from '../../Infrastructure/DataContracts/WindowArgs';
import {SharedLogisticsSummary} from '../DataContracts/SharedLogisticsSummary';
import {CustomerTenantAccessRequestStatusCount} from '../DataContracts/CustomerTenantAccessRequestStatusCount';

import {CustomerTenantAccessList} from '../../Common/EntityLists/CustomerTenantAccessList';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';

import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';

import {CustomerPMService} from '../../Common/Services/StandardPMs/CustomerPMService';
import {TenantPMService} from '../../Common/Services/StandardPMs/TenantPMService';
import {TenantPM} from '../../Common/EntityPMs/TenantPM';
import {SharedLogisticsService} from '../Services/Others/SharedLogisticsService';
import {DocumentTypeListService} from '../../Common/Services/StandardLists/DocumentTypeListService';



import {ApiQueryFilters} from '../../Infrastructure/DataContracts/ApiQueryFilters';

import {CustomerPM} from '../../Common/EntityPMs/CustomerPM';
import {ObservableCollection} from '../../Infrastructure/Utilities/ObservableCollection';

@Component({
    moduleId: module.id,
    templateUrl: './CutsomerTenantAccessManagementComponent.html',
    providers: [SharedLogisticsService, DocumentTypeListService],
})


export class CutsomerTenantAccessManagementComponent implements OnInit {
    filterAgrs: ApiQueryFilters;

    private _entityResourceService: EntityResourceService = new EntityResourceService();

    private _customerPMService: CustomerPMService = new CustomerPMService();

    EnableAccess: boolean = true;
    TitleSettings: string = "Access Management";
    TitleStatus: string = "Requests Status";

    
    EAWBQueryGroupVisibility: boolean = true;
    MobileActivatedEnabled: boolean = false;

    WaitingCount: number = 0;
    InProgressCount: number = 0;
    AcceptedCount: number = 0;
    InactiveCount: number = 0;
    WaitingCountEnabled: boolean;
    AcceptedCountEnabled: boolean;
    InProgressCountEnabled: boolean;
    InactiveCountEnabled: boolean;
    public ItemsSource: ObservableCollection;
   

    public LastCustomerRequestList: CustomerTenantAccessList[]; 
    myTenantPM: TenantPM;
    public tenantPMService: TenantPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _sharedLogisticsService: SharedLogisticsService, public _documentTypeListService: DocumentTypeListService) {
        this.ItemsSource = new ObservableCollection([]);
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService();

        }
    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName("CustomerTenantAccess", 0).subscribe(response => {
            this.LoadData();
        });
    }



    LoadData() {
        this.LoadCurrentTenant();
        this.LoadLastCustomerRequest();
    }



    LoadCurrentTenant() {
        this.tenantPMService.get(SessionInfo.LoggedUserTenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.myTenantPM = myResult;
                    this.RefreshTenantScreenData();
                }
            }
        });
    }

    LoadLastCustomerRequest() {
        this._sharedLogisticsService.GetLastCustomerRequest(SessionInfo.LoggedUserTenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.LastCustomerRequestList = pmResponse.Result;
                this.LastCustomerRequestList.forEach((item) => {
                    this.ItemsSource.Insert(item);
                });

            }
        });

    }

    OnRowSelected(itemComponent: any) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: itemComponent.Id, ObjectTableName: 'CustomerTenantAccess', BackButtonLabel: "Back" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.ItemsSource.Clear();
                    this.LoadData();
                });
            });
    }



    RefreshTenantScreenData() {

        if (this.myTenantPM != null && !this.myTenantPM.IsCustomerTenantShare) {
            this.EnableAccess = false;
        }

        this.loadCustomerRequestStatusData();
        //this.LoadSharedLogisticsSummary();
    }


    loadCustomerRequestStatusData() {
        this._sharedLogisticsService.getCustomerTenantAccessRequestStatusCount(SessionInfo.LoggedUserTenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    var data: CustomerTenantAccessRequestStatusCount = myResult;

                    if (data != null) {
                        this.WaitingCount = data.WaitingCount;
                        this.InProgressCount = data.InProgressCount;
                        this.AcceptedCount = data.AcceptedCount;
                        this.InactiveCount = data.InactiveCount;
                    }
                    this.WaitingCountEnabled = this.WaitingCount == 0 ? false : true;
                    this.InProgressCountEnabled = this.InProgressCount == 0 ? false : true;
                    this.AcceptedCountEnabled = this.AcceptedCount == 0 ? false : true;
                    this.InactiveCountEnabled = this.InactiveCount == 0 ? false : true;
                }

            }


        });
    }




    


   

    SettingsLinkClick() {
        var windowArgs: any = {};
        windowArgs = { EntityPM: this.myTenantPM, Parent: this}
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.Title = "LogBox Access Settings";
        logWindow.Show("./SharedLogistics/Components/TenantAccessSettingsComponent");



    }







    InviteLinkClick(code: string) {
        var backButtonTitle = "Shared Logistics";
        var queryCode = "";
        var displayTitle = "";
        var objectTableName = "";
        if (code != null) {

            switch (code) {
                case "Customers":
                    {
                        displayTitle = "Customers";
                        objectTableName = "Customer";
                        queryCode = "Shared Logistics Customers";
                        break;
                    }

                case "Agents":
                    {
                        displayTitle = "Agents";
                        objectTableName = "Agent";
                        queryCode = "Shared Logistics Agents";
                        break;
                    }
                default: { break; }
            }
            this.filterAgrs = new ApiQueryFilters();
            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            //listArgs.ShowViews = false;
            //this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
            //});

        }
    }


    StatusZoomCommand(code: string) {

        this.filterAgrs = new ApiQueryFilters();
        //this.filterAgrs.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, true, false, "string");
        //this.filterAgrs.addAdditionalFilter("PartnerTypeId", "CS", null, null, "Equals", false, true, false, "string");


        var backButtonTitle = "Shared Logistics";
        var queryCode = "";
        var displayTitle = "";
        var showViews = false;
        var objectTableName = "CustomerTenantAccess";
        var navigate: boolean = true;
        switch (code) {
            case "W":
                {
                    if (this.WaitingCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "RequestDateTime";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("Status", "W", null, null, "Equals", false, true, false, "string"); 
                        displayTitle = "Request";
                        queryCode = "CustomerTenantAccesses";

                    } 
                    break;
                }
            case "IP":
                {
                    if (this.InProgressCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "RequestDateTime";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("Status", "IP", null, null, "Equals", false, true, false, "string");
                        displayTitle = "Request"; 
                        queryCode = "CustomerTenantAccesses";

                    } 
                    break;
                }
            case "A":
                {
                    if (this.AcceptedCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "RequestDateTime";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("Status", "A", null, null, "Equals", false, true, false, "string");
                        displayTitle = "Request";
                        queryCode = "CustomerTenantAccesses";

                    }
                    break; 
                }

            case "IA":
                {
                    if (this.InactiveCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "RequestDateTime";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("Status", "IA", null, null, "Equals", false, true, false, "string");
                        displayTitle = "Request";
                        queryCode = "CustomerTenantAccesses";

                    }
                    break;
                }
            case "All":
                {
                    displayTitle = "Request";
                    this.filterAgrs.SortBy = "RequestDateTime";
                    this.filterAgrs.SortDirection = "Descending";
                    queryCode = "CustomerTenantAccesses";
                    showViews = true;
                    break;
                }
        }

        if (navigate) {  
            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            if (queryCode != "") {
                listArgs.QueryCode = queryCode;
            } 
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            listArgs.ShowViews = showViews;
           
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });

        }
    }



    ActivityZoomLinkClick(m: string) {


        var windowArgs: any = {};
        windowArgs.TenantPM = this.myTenantPM;


        switch (m) {
            case "Today Customers":
                {
                    windowArgs.PartnerTypeId = "CS";
                    windowArgs.DateParameter = "T";
                    windowArgs.DataContext = this;
                    // model = new ActivityZoomViewModel("CS", "T", myPartnersContext);
                    //control.DataContext = model;
                    break;
                }
            case "Last Week Customers":
                {
                    windowArgs.PartnerTypeId = "CS";
                    windowArgs.DateParameter = "W";
                    windowArgs.DataContext = this;
                    break;
                }
            case "Last Month Customers":
                {
                    windowArgs.PartnerTypeId = "CS";
                    windowArgs.DateParameter = "M";
                    windowArgs.DataContext = this;
                    break;
                }

            case "Today Agents":
                {
                    windowArgs.PartnerTypeId = "AG";
                    windowArgs.DateParameter = "T";
                    windowArgs.DataContext = this;
                    break;
                }
            case "Last Week Agents":
                {
                    windowArgs.PartnerTypeId = "AG";
                    windowArgs.DateParameter = "W";
                    windowArgs.DataContext = this;
                    break;
                }
            case "Last Month Agents":
                {
                    windowArgs.PartnerTypeId = "AG";
                    windowArgs.DateParameter = "M";
                    windowArgs.DataContext = this;

                    break;
                }
        }

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Title = "Activity Log";
        logitudeWindow.Height = 600;
        logitudeWindow.Width = 1000;
        logitudeWindow.Show("./SharedLogistics/Components/ActivityZoomComponent");

    }






    ShowDetailsButtonclick(item: any) {

        if (item.PartnerTypeName == "Customer") {
            item.IsEnabledShowDetailsButton = false;
            this._customerPMService.get(item.CardId).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                item.IsEnabledShowDetailsButton = true;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        if (!myResult.IsCustomerAllowed) this.ViewBlocedEntity(myResult);
                        else {
                            var logWindow = new LogitudeWindow();
                            logWindow.Title = "Customer" + " Edit";
                            if (window.innerHeight > 700 && window.innerWidth > 1200) {
                                logWindow.Width = 1200;
                                logWindow.Height = 700;
                                logWindow.ShowEditComponent(myResult.Id, "Customer", null, false);
                            }
                            else logWindow.ShowEditComponent(myResult.Id, "Customer");
                        }

                    }
                }


            });

        }


    }



    ViewBlocedEntity(item: CustomerPM) {

        var windowArgs: any = {};
        windowArgs.CustomerPM = item;
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Title = "View Customer";
        logitudeWindow.Height = 500;
        logitudeWindow.Width = 800;
        logitudeWindow.Show("./SharedLogistics/Components/ViewBlocedCustomerComponent");
    }
}

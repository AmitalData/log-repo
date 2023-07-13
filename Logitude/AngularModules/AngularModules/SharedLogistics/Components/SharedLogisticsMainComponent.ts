

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
import {SharedLogisticsStatusStatistics} from '../DataContracts/SharedLogisticsStatusStatistics';
import {LastLoginPartners} from '../DataContracts/LastLoginPartners';
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
import { AppTool } from '../../Infrastructure/Tools';

@Component({
    
    templateUrl: './SharedLogisticsMainComponent.html',
    providers: [SharedLogisticsService, DocumentTypeListService],
})

export class SharedLogisticsMainComponent implements OnInit {
    filterAgrs: ApiQueryFilters;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _customerPMService: CustomerPMService = new CustomerPMService();
    IsDisplayAreaDocument: boolean = false;
    IsShowActivationWizardLink: boolean = false;
    IsShowSettingLink: boolean = false;
    SharedLogisticsActivatedEnabled: boolean = false;
    EAWBQueryGroupVisibility: boolean = true;
    MobileActivatedEnabled: boolean = false;
    InvitedCustomersCount: number = 0;
    NotInvitedCustomersCount: number = 0;
    ActivatedCustomersCount: number = 0;
    ActivatedCustomersForMobileCount: number = 0;
    InvitedAgentsCount: number = 0;
    NotInvitedAgentsCount: number = 0;
    ActivatedAgentsCount: number = 0;
    InvitedCustomersCountIsEnabled: boolean;
    NotInvitedCustomersCountIsEnabled: boolean;
    ActivatedCustomersCountIsEnabled: boolean;
    InvitedAgentsCountIsEnabled: boolean;
    NotInvitedAgentsCountIsEnabled: boolean;
    ActivatedAgentsCountIsEnabled: boolean;
    ActivatedCustomersForMobileCountIsEnabled: boolean;
    TodayCustomerLogsCount: string = "0";
    LastWeekCustomerLogsCount: string = "0";
    LastMonthCustomerLogsCount: string = "0";
    TodayAgentLogsCount: string = "0";
    LastWeekAgentLogsCount: string = "0";
    LastMonthAgentLogsCount: string = "0";
    TodayCustomerIsEnabled: boolean;
    LastWeekCustomerIsEnabled: boolean;
    LastMonthCustomerIsEnabled: boolean;
    TodayAgentIsEnabled: boolean;
    LastWeekAgentIsEnabled: boolean;
    LastMonthAgentIsEnabled: boolean;

    TitleSettings: string;
    SharedTitleType: string;
    public LastPartnersList: LastLoginPartners[];
    public SelectLastPartnersList: LastLoginPartners;
    public SupportManagement: any;
    public ResetUserPassword: any;
    sharedLogisticsSummary: SharedLogisticsSummary;
    myTenantPM: TenantPM;
    public tenantPMService: TenantPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public LastMonthAccessVisibility: boolean = false;
    public LastActivityVisibility: boolean = false;
    public InviteLinkLable: string = "Invite Customers";
    public IsCtoolSetting: boolean = false;
    private ObjectTableInviteName: string = "Customer";
    public DisplayObjectTableInviteName: string = "Customers";
    private InviteQueryCode: string = "Shared Logistics Customers";
    public IsShowDisplaySetting: boolean = false;
    public IsShowAgentStatisticsArea: boolean = false;
    public HasCustomizedInvitationDocumentFeature: boolean = false;
    public HasCustomizedResetPasswordEmailFeature: boolean = false;

    constructor(public _sharedLogisticsService: SharedLogisticsService, public _documentTypeListService: DocumentTypeListService) {
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService();
        }
    }

    ngOnInit() {
        this.SetFeatures();
        this.SetVisibility();
        this.SetSettings();
        this.SetTitles();
        this.LoadData();

    }

    SetFeatures() {
        this.HasCustomizedInvitationDocumentFeature = FeatureLocator.HasFeaturePermession("General", "DocumentType.CustomizedInvitationDocument");
        this.HasCustomizedResetPasswordEmailFeature = FeatureLocator.HasFeaturePermession("General", "DocumentType.CustomizedResetPasswordEmail");
        this.IsDisplayAreaDocument = this.HasCustomizedInvitationDocumentFeature || this.HasCustomizedResetPasswordEmailFeature;
    }

    SetSettings() {

        if (this.SharedTitleType != "CTool") return;
        this.ObjectTableInviteName = "Card";
        this.DisplayObjectTableInviteName = "Partners";
        this.InviteQueryCode = "Ctool Partners";
        this.IsCtoolSetting = true

    }


    SetVisibility() {

        if (this.SharedTitleType == "CTool") return;
        this.LastMonthAccessVisibility = this.LastActivityVisibility = true;
        this.IsShowDisplaySetting = true;
        this.IsShowAgentStatisticsArea = true;
        

    }

    GetInviteToolTipMessage() {
        return this.IsCtoolSetting ? "View shows all partners except customers" : "";
    }

    public get IsCargoTracking(): boolean {
        return this.SharedTitleType == "CargoTracking";
    }

    public get IsShowActivatedMobileArea(): boolean {
        if (this.SharedTitleType == "CTool" || this.SharedTitleType == "CargoTracking") return false;
        return true;
    }

    public get TitleStatus(): string {
        if (this.SharedTitleType == "CTool") return "CTool Status";
        if (this.SharedTitleType == "CargoTracking") return "Cargo Tracking";
        if (FeatureLocator.HasFeaturePermession("General", "MOBILE") && FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICS")) return "Shared Logistics & Mobile Status";
        if (FeatureLocator.HasFeaturePermession("General", "MOBILE")) return "Mobile Status";
        if (FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICS")) return "Shared Logistics Status";
        return "Shared Logistics Status";
    }

    public get ActivatedLabel(): string {
        if (this.SharedTitleType == "CTool") return "Activated Ctool";
        if (this.SharedTitleType == "CargoTracking") return "Activated Cargo Tracking";
        return "Activated Shared Logistics";
    }

    SetTitles() {


          if (this.SharedTitleType == "CargoTracking")
        {
            this.TitleSettings = "Cargo Tracking Settings";
        }
        else {
            if (FeatureLocator.HasFeaturePermession("General", "MOBILE") && FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICS"))
                this.TitleSettings = "Shared Logistics & Mobile Settings";
            else if (FeatureLocator.HasFeaturePermession("General", "MOBILE"))
                this.TitleSettings = "Mobile Settings";
            else if (FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICS"))
                this.TitleSettings = "Shared Logistics Settings";
        }

        if (this.SharedTitleType != "CTool") return;
        this.TitleSettings = "CTool Settings";
        this.InviteLinkLable = "Invite CTool Partners";

    }


    SetSharedTitleType(titleType) {
        this.SharedTitleType = titleType;
        this.InviteQueryCode = this.IsCargoTracking ? "Cargo Tracking Customers" : this.InviteQueryCode;
    }

    LoadData() {
        this.LoadCurrentTenant();
        if (this.LastMonthAccessVisibility) {
            this.LoadLastLoginPartners();
        }
    }

    LoadCurrentTenant() {
        this.tenantPMService.get(SessionInfo.LoggedUserTenant).subscribe((res:any) => {
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

    RefreshTenantScreenData() {
        if (this.myTenantPM.IsSharedLogisticsActivated || this.myTenantPM.IsMobileActivated) {
            this.SharedLogisticsActivatedEnabled = true;
            this.IsShowActivationWizardLink = false;
            this.IsShowSettingLink = true;
        }

        else {
            this.SharedLogisticsActivatedEnabled = false;
            this.IsShowActivationWizardLink = true;
            this.IsShowSettingLink = false;
        }

        if (this.myTenantPM.IsMobileActivated) {
            this.MobileActivatedEnabled = true;
            this.IsShowActivationWizardLink = false;
            this.IsShowSettingLink = true;
        }

        else {
            this.MobileActivatedEnabled = false;
        }

        //this.MobileActivatedEnabled = false;
        this.LoadCardData();

        if (this.LastActivityVisibility || this.LastMonthAccessVisibility) {
            this.LoadSharedLogisticsSummary();
        }
    }

    LoadCardData() {
        let invitationStatusType = this.IsCargoTracking ? "CargoTracking" : "";
        this._sharedLogisticsService.getSharedLogisticsStatistics(SessionInfo.LoggedUserTenant, invitationStatusType).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    var data: SharedLogisticsStatusStatistics = myResult;
                  
                    if (data != null) {
                        this.InvitedCustomersCount = !this.IsCtoolSetting ? data.InvitedCustomersCount : data.InvitedCToolPartnersCount;
                        this.NotInvitedCustomersCount = !this.IsCtoolSetting ? data.NotInvitedCustomersCount : data.NotInvitedCToolPartnersCount;

                        this.ActivatedCustomersCount = !this.IsCtoolSetting ? data.ActivatedCustomersCount : data.ActivatedCToolPartnersCount;
                        this.ActivatedCustomersForMobileCount = data.ActivatedCustomersForMobileCount;

                        this.InvitedAgentsCount = data.InvitedAgentsCount;
                        this.NotInvitedAgentsCount = data.NotInvitedAgentsCount;
                        this.ActivatedAgentsCount = data.ActivatedAgentsCount;
                    }

                    if (this.SharedLogisticsActivatedEnabled) {
                        this.InvitedCustomersCountIsEnabled = this.InvitedCustomersCount == 0 ? false : true;
                        this.NotInvitedCustomersCountIsEnabled = this.NotInvitedCustomersCount == 0 ? false : true;
                        this.ActivatedCustomersCountIsEnabled = this.ActivatedCustomersCount == 0 ? false : true;

                        this.InvitedAgentsCountIsEnabled = this.InvitedAgentsCount == 0 ? false : true;
                        this.NotInvitedAgentsCountIsEnabled = this.NotInvitedAgentsCount == 0 ? false : true;
                        this.ActivatedAgentsCountIsEnabled = this.ActivatedAgentsCount == 0 ? false : true;
                    }

                    else {
                        this.InvitedCustomersCountIsEnabled = false;
                        this.NotInvitedCustomersCountIsEnabled = false;
                        this.ActivatedCustomersCountIsEnabled = false;

                        this.InvitedAgentsCountIsEnabled = false;
                        this.NotInvitedAgentsCountIsEnabled = false;
                        this.ActivatedAgentsCountIsEnabled = false;
                    }

                    if (this.MobileActivatedEnabled) {
                        this.ActivatedCustomersForMobileCountIsEnabled = this.ActivatedCustomersForMobileCount == 0 ? false : true;
                    }
                    else {
                        this.ActivatedCustomersForMobileCountIsEnabled = false;
                    }


                }

            }


        });
    }

    LoadSharedLogisticsSummary() {
        this._sharedLogisticsService.getSharedLogisticsSummaryData(SessionInfo.LoggedUserTenant).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.sharedLogisticsSummary = pmResponse.Result;

                if (this.sharedLogisticsSummary != null) {
                    this.TodayCustomerLogsCount = this.sharedLogisticsSummary.TodayCustomersCount.toString();
                    this.LastWeekCustomerLogsCount = this.sharedLogisticsSummary.LastWeekCustomersCount.toString();
                    this.LastMonthCustomerLogsCount = this.sharedLogisticsSummary.LastMonthCustomersCount.toString();

                    this.TodayAgentLogsCount = this.sharedLogisticsSummary.TodayAgentsCount.toString();
                    this.LastWeekAgentLogsCount = this.sharedLogisticsSummary.LastWeekAgentsCount.toString();
                    this.LastMonthAgentLogsCount = this.sharedLogisticsSummary.LastMonthAgentsCount.toString();

                    if (this.SharedLogisticsActivatedEnabled) {
                        this.TodayCustomerIsEnabled = this.TodayCustomerLogsCount == "0" ? false : true;
                        this.LastWeekCustomerIsEnabled = this.LastWeekCustomerLogsCount == "0" ? false : true;
                        this.LastMonthCustomerIsEnabled = this.LastMonthCustomerLogsCount == "0" ? false : true;

                        this.TodayAgentIsEnabled = this.TodayAgentLogsCount == "0" ? false : true;
                        this.LastWeekAgentIsEnabled = this.LastWeekAgentLogsCount == "0" ? false : true;
                        this.LastMonthAgentIsEnabled = this.LastMonthAgentLogsCount == "0" ? false : true;
                    }

                    else {
                        this.TodayCustomerIsEnabled = false;
                        this.LastWeekCustomerIsEnabled = false;
                        this.LastMonthCustomerIsEnabled = false;

                        this.TodayAgentIsEnabled = false;
                        this.LastWeekAgentIsEnabled = false;
                        this.LastMonthAgentIsEnabled = false;
                    }
                }
            }
        });
    }

    LoadLastLoginPartners() {
        this._sharedLogisticsService.getLastLoginPartners(SessionInfo.LoggedUserTenant).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.LastPartnersList = pmResponse.Result;
                this.LastPartnersList.forEach((item) => {

                    item.IsEnabledShowDetailsButton = true;
                });
            }
        });
    }

    EventsPermissionsLinkClick() {
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 520;
        logWindow.Title = "Events Permissions";
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsEventPermissiosComponent");
    }

    MilestonesPermissionsLinkClick() {
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 520;
        logWindow.Title = "Milestones Permissions";
        logWindow.Show("./SharedLogistics/Components/CargoTrackingMilestonesPermissiosComponent");
    }

    DocumentsPermissionsLinkClick() {
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 520;
        logWindow.Title = "Documents Permissions";
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsDocumentPermissiosComponent");
    }

    MoneyPermissionsLinkClick() {
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 520;
        logWindow.Title = "Money Permissions";
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsMoneyPermissiosComponent");
    }

    PartnersPermissionsLinkClick() {
        var windowArgs: any = {};
        windowArgs.IsCargoTracking = this.SharedTitleType == "CargoTracking";
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 570;
        logWindow.Title = "Partners Permissions";
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsPartnersPermissiosComponent");
    }

    ActivationWizardLinkClick() {
        var logWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.TenantPM = this.myTenantPM;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 910;
        logWindow.Height = 550;
        logWindow.Title = this.IsCargoTracking ? "Cargo Tracking Wizard" : "Shared Logistics Wizard";
        logWindow.DataContext = this.myTenantPM;
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsWizardComponent");
    }

    SettingsLinkClick() {
        var windowArgs: any = {};
        windowArgs.TenantPM = this.myTenantPM;
        windowArgs.SharedTitleType = this.SharedTitleType;
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 900;
        logWindow.Height = 550;
        logWindow.Title = this.SharedTitleType == "CargoTracking" ? "Cargo Tracking Settings": "Shared Logistics Settings";
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsSettingComponent");
    }

    InviteLinkClick(code: string) {

        var backButtonTitle = this.IsCargoTracking ? "Cargo Tracking" : (this.IsCtoolSetting ? "Ctool" : "Shared Logistics");
        var queryCode = "";
        var displayTitle = "";
        var objectTableName = "";
        if (code != null) {

            switch (code) {
                case "Customers":
                    {
                        displayTitle = this.DisplayObjectTableInviteName;
                        objectTableName = this.ObjectTableInviteName;
                        queryCode = this.InviteQueryCode;
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
            if (this.IsCtoolSetting) {
                this.filterAgrs.addAdditionalFilter("InActive", false, null, null, "Equals", true, false, false, "boolean");
            }
            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            listArgs.IsCargoTrackingMenuClicked = this.SharedTitleType == "CargoTracking";
            listArgs.DontCheckQueryFeature = this.IsCtoolSetting ? true : false;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response:any) => {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        this.CurrentSession.AddMenuReference(cmpRef);
                    });
            });
        }
    }

    CustomersZoomLinkClcik(code: string) {
        this.filterAgrs = new ApiQueryFilters();
        this.SetAddAdditionalFilters();
        var backButtonTitle = this.IsCargoTracking ? "Cargo Tracking" : (!this.IsCtoolSetting ? "Shared Logistics" : "Ctool");
        var queryCode = this.InviteQueryCode;
        var displayTitle = "";
        var displayObjectTableName = this.DisplayObjectTableInviteName;
        var navigate: boolean = true;
        switch (code) {
            case "invited":
                {
                    if (this.InvitedCustomersCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = this.IsCargoTracking ? "CargoTrackingInvitationDate" : "InvitationDate";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter(this.IsCargoTracking ? "CargoTrackingInvitationStatusCode" : "SharedLogisticsInvitationStatusCode", 2, null, null, "Equals", false, true, false, "string");
                        displayTitle = "Invited " + displayObjectTableName;
                    }
                    break;
                }
            case "not invited":
                {
                    if (this.NotInvitedCustomersCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = this.IsCtoolSetting ? "InvitationDate" : "LastShipmentDate";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter(this.IsCargoTracking ? "CargoTrackingInvitationStatusCode" : "SharedLogisticsInvitationStatusCode", 1, null, null, "Equals", false, true, false, "string");
                        displayTitle = "Not Invited " + displayObjectTableName;

                    }
                    break;
                }
            case "activated":
                {
                    if (this.ActivatedCustomersCount == 0) {
                        navigate = false;
                    }
                    else {

                        this.filterAgrs.SortBy = "LastLoginDate";
                        this.filterAgrs.SortDirection = "Descending";

                        this.filterAgrs.addAdditionalFilter(this.IsCargoTracking ? "CargoTrackingInvitationStatusCode" : "SharedLogisticsInvitationStatusCode", 3, null, null, "Equals", false, true, false, "string");

                        displayTitle = "Activated " + displayObjectTableName;


                    }
                    break;
                }

            case "activatedmobile":
                {
                    if (this.ActivatedCustomersForMobileCount == 0) {
                        navigate = false;
                    }
                    else {

                        this.filterAgrs.SortBy = "LastLoginDate";
                        this.filterAgrs.SortDirection = "Descending";

                        this.filterAgrs.addAdditionalFilter(this.IsCargoTracking ? "CargoTrackingInvitationStatusCode" : "SharedLogisticsInvitationStatusCode", 3, null, null, "Equals", false, true, false, "string");
                        this.filterAgrs.addAdditionalFilter("IsActiveForMobile", true, null, null, "Equals", false, true, false, "boolen");
                        displayTitle = "Activated Mobile Customers";

                    }
                    break;
                }
        }

        if (navigate) {
            var listArgs = new ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = this.ObjectTableInviteName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            listArgs.ShowViews = false;
            listArgs.DontCheckQueryFeature = this.IsCtoolSetting ? true : false;
            listArgs.IsCargoTrackingMenuClicked = this.SharedTitleType == "CargoTracking";

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        }
    }





    private SetAddAdditionalFilters() {
        if (this.IsCtoolSetting) {
            this.filterAgrs.addAdditionalFilter("InActive", false, null, null, "Equals", true, false, false, "boolean");
            return;
        }
        this.filterAgrs.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, true, false, "string");
        this.filterAgrs.addAdditionalFilter("PartnerTypeId", "CS", null, null, "Equals", false, true, false, "string");
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

    ShowDetailsButtonclick(item: LastLoginPartners) {
        if (item.PartnerTypeName == "Customer") {
            item.IsEnabledShowDetailsButton = false;
            this._customerPMService.get(item.CardId).subscribe((res:any) => {
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

    AgentsZoomLinkClcik(arg: any) {

    }

    ShowDocumentTypeEditButtonclick(documentcode: string) {
        var apiQueryFilters: ApiQueryFilters = new ApiQueryFilters();

        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionInfo.LoggedUserTenant;
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe((res:any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;

                var item = myResult.filter(d => d.Code == documentcode)[0];

                if (item) {
                    this.OpenDocumentTypeEdit(item.Id);
                }
            }
        });
    }

    OpenDocumentTypeEdit(documentId: string) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Document Type" + " Edit";
        logWindow.ShowEditComponent(documentId, "DocumentType");
    }
}

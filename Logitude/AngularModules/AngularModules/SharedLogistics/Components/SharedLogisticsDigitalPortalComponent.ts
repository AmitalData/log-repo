declare var window: any;
import { Component, OnInit, ElementRef } from '@angular/core';
import { ListComponentArgs } from '../../Infrastructure/Args';
import { FeatureLocator } from '../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ServiceArgs } from '../../Infrastructure/DataContracts/ServiceArgs';
import { EntityResourceService } from '../../Infrastructure/Services/EntityResourceService';
import { WindowArgs } from '../../Infrastructure/DataContracts/WindowArgs';
import { SharedLogisticsSummary } from '../DataContracts/SharedLogisticsSummary';
import { SharedLogisticsStatusStatistics } from '../DataContracts/SharedLogisticsStatusStatistics';
import { LastLoginPartners } from '../DataContracts/LastLoginPartners';
import { SessionInfo } from '../../Infrastructure/Utilities/SessionInfo';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../Controls/Windows/LogitudeWindow';
import { CustomerPMService } from '../../Common/Services/StandardPMs/CustomerPMService';
import { TenantPMService } from '../../Common/Services/StandardPMs/TenantPMService';
import { TenantPM } from '../../Common/EntityPMs/TenantPM';
import { ApiQueryFilters } from '../../Infrastructure/DataContracts/ApiQueryFilters';
import { CustomerPM } from '../../Common/EntityPMs/CustomerPM';
import { SharedLogisticsService } from '../Services/Others/SharedLogisticsService';
import { AppTool } from '../../Infrastructure/Tools';

@Component({
    templateUrl: './SharedLogisticsDigitalPortalComponent.html',
    providers: [SharedLogisticsService],
})

export class SharedLogisticsDigitalPortalComponent implements OnInit {
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
    private ObjectTableInviteName: string = "Customer";
    public DisplayObjectTableInviteName: string = "Customers";
    private InviteQueryCode: string = "Shared Logistics Customers";
    public IsShowDisplaySetting: boolean = false;
    public IsShowAgentStatisticsArea: boolean = false;
    public ValidationWarningsMessage: string = null;
    public IsValidationWarningsVisible: boolean = false;
    public IsCustomerCareUser = false;
    IsTenantZero: boolean = false;

    constructor(public _sharedLogisticsService: SharedLogisticsService) {
        if (SessionLocator.TenantPM.Id == 0) {
            this.IsTenantZero = true;
        }

        this.InitalizeServices();
    }

    InitalizeServices() {
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService();
        }
    }

    SetIsCustomerCareUser() {
        this.IsCustomerCareUser = SessionLocator?.LoggedUserPM?.IsCustomerCare;
    }

    ngOnInit() {
        this.SetVisibility();
        this.SetTitles();
        this.LoadData();
        this.ValidateDomainSettings();
        this.SetIsCustomerCareUser();
    }

    private ValidateDomainSettings() {
        this.IsValidationWarningsVisible = false;
        this.ValidationWarningsMessage = null;
        if (AppTool.IsNullOrEmpty(SessionLocator.TenantManagementJS.CustomerURL)) {
            this.IsValidationWarningsVisible = true;
            this.ValidationWarningsMessage = "Please contact your Administrator to define your Digital Portal domain!";
        }
    }

    InviteLinkClick() {
        var backButtonTitle = "Digital Portal";
        var displayTitle = "Customers";
        var objectTableName = "Customer";
        var queryCode = "Shared Logistics Customers";
        this.filterAgrs = new ApiQueryFilters();
        var listArgs = new ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = backButtonTitle;
        listArgs.IsDigitalPortalMenuClicked = true;
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe((response: any) => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        });
    }

    SetVisibility() {
        this.LastMonthAccessVisibility = this.LastActivityVisibility = true;
        this.IsShowDisplaySetting = true;
        this.IsShowAgentStatisticsArea = true;
    }

    GetInviteToolTipMessage() {
        return  "";
    }

    public get IsShowActivatedMobileArea(): boolean {
         return false;
    }

    public get TitleStatus(): string {
        return "Digital Portal Status";
    }

    public get ActivatedLabel(): string {
        return "Activate Digital Portal";
    }

    SetTitles() {
        this.TitleSettings = "Digital Portal Settings";
    }


    SetSharedTitleType(titleType) {
        this.SharedTitleType = titleType;
    }

    LoadData() {
        this.LoadCurrentTenant();
        if (this.LastMonthAccessVisibility) {
            this.LoadLastLoginPartners();
        }
    }

    LoadCurrentTenant() {
        this.tenantPMService.get(SessionInfo.LoggedUserTenant).subscribe((res: any) => {
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

        this.LoadCardData();

        if (this.LastActivityVisibility || this.LastMonthAccessVisibility) {
            this.LoadSharedLogisticsSummary();
        }
    }

    LoadCardData() {
        this._sharedLogisticsService.getSharedLogisticsStatistics(SessionInfo.LoggedUserTenant, "DigitalPortal").subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    var data: SharedLogisticsStatusStatistics = myResult;

                    if (data != null) {
                        this.InvitedCustomersCount =  data.InvitedCustomersCount ;
                        this.NotInvitedCustomersCount = data.NotInvitedCustomersCount ;

                        this.ActivatedCustomersCount =  data.ActivatedCustomersCount ;
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
        this._sharedLogisticsService.getDigitalSharedLogisticsSummaryData(SessionInfo.LoggedUserTenant).subscribe((res: any) => {
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

    DocumentsPermissionsLinkClick() {
        var windowArgs: any = {};
        windowArgs.IsDigitalPortal = true;
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

    CustomizationLinkClick() {
        var logWindow = new LogitudeWindow();
        logWindow.IsFillScreen = true;
        logWindow.IsShowCloseButton = false;
        logWindow.Title = "Customization ";
        logWindow.Show('./SharedLogistics/Components/DigitalPortal/DigitalPortalCustomizationMainComponent');
    }

    LanguageLinkClick() {
        var windowArgs: any = {};
        windowArgs.IsDigitalPortal = true;
        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 520;
        logWindow.Title = "Language Display Settings";

        logWindow.Show("./SharedLogistics/Components/DigitalPortal/DigitalPortalLanguageSettingsComponent");
    }

    PartnersPermissionsLinkClick() {
        var windowArgs: any = {};
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
        logWindow.Title = "Shared Logistics Wizard";
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
        logWindow.Title = "Digital Portal Settings";
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsSettingComponent");
    }

    CustomersZoomLinkClcik(code: string) {
        this.filterAgrs = new ApiQueryFilters();
        this.SetAddAdditionalFilters();
        var backButtonTitle = "Digital Portal";
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
                        this.filterAgrs.SortBy = "InvitationDate";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("SharedLogisticsInvitationStatusCode", 2, null, null, "Equals", false, true, false, "string");
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
                        this.filterAgrs.SortBy = "LastShipmentDate";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("SharedLogisticsInvitationStatusCode", 1, null, null, "Equals", false, true, false, "string");
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

                        this.filterAgrs.addAdditionalFilter("SharedLogisticsInvitationStatusCode", 3, null, null, "Equals", false, true, false, "string");

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

                        this.filterAgrs.addAdditionalFilter("SharedLogisticsInvitationStatusCode", 3, null, null, "Equals", false, true, false, "string");
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
            listArgs.DontCheckQueryFeature = false;

            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    this.CurrentSession.AddMenuReference(cmpRef);
                });
        }
    }

    private SetAddAdditionalFilters() {
        this.filterAgrs.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, true, false, "string");
        this.filterAgrs.addAdditionalFilter("PartnerTypeId", "CS", null, null, "Equals", false, true, false, "string");
    }

    ActivityZoomLinkClick(m: string) {
        var windowArgs: any = {};
        windowArgs.TenantPM = this.myTenantPM;
        windowArgs.IsDigital = true;

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
            this._customerPMService.get(item.CardId).subscribe((res: any) => {
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
}

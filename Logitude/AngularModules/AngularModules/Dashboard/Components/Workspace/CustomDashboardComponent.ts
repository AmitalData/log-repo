import { Component, OnDestroy, ViewEncapsulation, HostListener, ElementRef } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { BehaviorSubject, Subject } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool, ArrayTool, DateTool } from '../../../Infrastructure/Tools';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DashboardDataBinding } from 'logitude-dashboard-library/dist/types/DashboardDataBinding';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';
import { DashboardListService } from '../../../DashboardModule/Services/StandardLists/DashboardListService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DashboardList } from '../../../DashboardModule/EntityLists/DashboardList';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { DashboardPMService } from '../../../DashboardModule/Services/StandardPMs/DashboardPMService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { DashboardPMExtendedService, PinnedDashboard } from '../../../DashboardModule/Services/ExtendedPMs/DashboardPMExtendedService';
import { UserPinnedDashboardPM } from '../../../DashboardModule/EntityPMs/UserPinnedDashboardPM';
import { DashboardListExtendedService } from '../../../DashboardModule/Services/ExtendedLists/DashboardListExtendedService';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';

@Component({
    templateUrl: 'CustomDashboardComponent.html',
    styleUrls: ['CustomDashboardCSS.scss'],
    selector: 'custom-dashboard',
    encapsulation: ViewEncapsulation.None,
})

export class CustomDashboardComponent extends BaseComponent implements OnDestroy {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;
    private DashboardListService: DashboardListService;
    public DashboardEntity: DashboardPM = null;
    public HasChanges: boolean = false;
    public OpenEditLayout: boolean = false;
    public dashbaordCount = -1;
    public ItemsSource: DashboardList[] = [];
    public DashboardDropdownLoading: boolean = true;
    public DashboardsTabs: DashboardTab[] = [];
    private TabsCount: number = 10;
    private dashboardPMEstendedService: DashboardPMExtendedService;
    public ComponentId: string = null;
    public ComponentContentId: string = null;
    public SectionsItemsSource: CodeNameClass[];
    constructor(private eRef: ElementRef) {
        super();
        var idIndex = this.CurrentSession.GetNewId("Meu");
        this.ComponentId = "Menu_" + idIndex;
        this.ComponentContentId = "MeuContent_" + idIndex;
        this.DashboardListService = new DashboardListService();
        this.dashboardPMEstendedService = new DashboardPMExtendedService();
        this.GetData();
        this.BuildSectionsItemsSource();
    }
    BuildSectionsItemsSource() {
        this.SectionsItemsSource = [];

        this.SectionsItemsSource.push(new CodeNameClass("ONM", "My Dashboards"));
        this.SectionsItemsSource.push(new CodeNameClass("SPF", "Shared Dashboards"));
        this.SectionsItemsSource.push(new CodeNameClass("SYS" ,"System Dashboards"));
    }

    DashboardDataBinding: DashboardDataBinding = {
        isOnEditLayout: new Subject(),
        onGetLayouts: new BehaviorSubject({ lg: [] }),
        widgetUpdated: new Subject(),
        onAddWidget: new Subject(),
        onEditWidget: new Subject(),
    }

    public ShowDashboardTab: boolean = false;
    private selectedDashboard: DashboardList;
    public get SelectedDashboard() { return this.selectedDashboard; }
    public set SelectedDashboard(value: DashboardList) {
        if (value?.Id == this.selectedDashboard?.Id) return;
        this.selectedDashboard = value;
        this.ShowDashboardTab = false;
        setTimeout(() => {
            this.ShowDashboardTab = true
        }, 100);

    }

    GetData() {
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("Dashboard").subscribe((res1: any) => {
            this._entityResourceService.getEntityResourceByTableName("Widget").subscribe((res2: any) => {
                this._entityResourceService.getEntityResourceByTableName("WidgetMeasure").subscribe((res2: any) => {
                    this._entityResourceService.getEntityResourceByTableName("AnalyticsFactsMetaData").subscribe((res3: any) => {
                        this._entityResourceService.getEntityResourceByTableName("AnalyticsFactsFieldsMetaData").subscribe((res4: any) => {
                            setTimeout(e => {
                                this.CheckIsLoggedUserHasPinnedDashboards();
                                this.LoadDashboardsForDropDown();
                            }, 70);
                        });
                    });
                });
            });
        });
    }

    private SessionEvent: any = null;
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    public UserPinnedDashboards: UserPinnedDashboardPM;
    CheckIsLoggedUserHasPinnedDashboards() {
        this.dashboardPMEstendedService.GetoggedUserPinnedDashboards(SessionLocator.LoggedUserId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.UserPinnedDashboards = myResponse.Result;
                if (this.UserPinnedDashboards == null) {
                    this.LoadPredefinedDashboardsFromTenantZero();
                }

                else {
                    if (AppTool.IsNullOrEmpty(this.UserPinnedDashboards.Dashboards)) {
                        this.dashbaordCount = 0;
                    }

                    else {
                        this.DrawPinnedDashboards(this.UserPinnedDashboards.Dashboards);
                    }
                }
            }
        });
    }

    private LoadPredefinedDashboardsFromTenantZero() {
        this.dashboardPMEstendedService.GetPredefinedDashboardsFromTenantZero().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                SessionLocator.SelectedSession.StopBusyIndicator();
                this.loadedDashboards = myResponse.Result?.slice(0, this.TabsCount);
                this.BuildTabs();
            }
        });
    }

    pinnedDashboards: PinnedDashboard[] = [];
    pinnedDashboardsCount: number = 0;
    isPinnedDashboardsList:boolean = false;
    private DrawPinnedDashboards(pinnedDashboardsJson: string) {
        this.pinnedDashboards = JSON.parse(pinnedDashboardsJson);
        this.pinnedDashboardsCount = this.pinnedDashboards.length;
        this.isPinnedDashboardsList = true;

        this.dashboardPMEstendedService.GetPinnedDashboards(pinnedDashboardsJson).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                SessionLocator.SelectedSession.StopBusyIndicator();
                this.loadedDashboards = myResponse.Result?.slice(0, this.TabsCount);
                this.BuildTabs();
            }
        });
    }

    private LoadDashboardsForDropDown() {
        var service: DashboardListExtendedService = new DashboardListExtendedService();
        service.GetDashboardsForDropDown().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ItemsSource = myResponse.Result ?? [];
                this.DashboardDropdownLoading = false;

                if (SessionLocator.Tenant == 0) {
                    this.loadedDashboards = myResponse.Result?.slice(0, this.TabsCount);
                    this.dashbaordCount = this.loadedDashboards.length;
                    this.BuildTabs();
                }
            }
        });
    }

    private selectedDashboardsFromLOV: DashboardList[] = [];
    private loadedDashboards: DashboardList[] = [];
    private unpinnedDashboards: DashboardList[] = [];
    BuildTabs() {
        this.DashboardsTabs = [];
        var list: DashboardTab[] = [];
        var index: number = 0;

        if (this.selectedDashboardsFromLOV.length > 0) {
            this.selectedDashboardsFromLOV.forEach(item => {
                list.push(new DashboardTab(item, index, this));
                index++;
            });
        }

        if (this.unpinnedDashboards.length > 0) {
            this.unpinnedDashboards.forEach(item => {
                if (!list.find(d => d.Id == item.Id)) {
                    list.push(new DashboardTab(item, index, this));
                    index++;
                }
            });
        }

        if (list.length < this.TabsCount) {
            if (this.isPinnedDashboardsList) {
                this.pinnedDashboards.forEach(pinnedItem => {
                    if (!list.find(d => d.Id == pinnedItem.Id)) {
                        if (index != this.TabsCount) {
                            var pinnedDashboard: DashboardList = this.loadedDashboards.find(d => d.Id == pinnedItem.Id);
                            list.push(new DashboardTab(pinnedDashboard, index, this));
                            index++;
                        }
                    }
                });
            }

            else {
                this.loadedDashboards.forEach(item => {
                    if (!list.find(d => d.Id == item.Id)) {
                        if (index != this.TabsCount) {
                            list.push(new DashboardTab(item, index, this));
                            index++;
                        }
                    }
                });
            }
        }

        var pinList: DashboardTab[] = list.filter(d => d.IsPinned).sort(function (a, b) { return a.Order == b.Order ? 0 : a.Order > b.Order ? 1 : -1; });
        var unpinList: DashboardTab[] = list.filter(d => !d.IsPinned).sort(d => d.Order);

        pinList.forEach((item) => {
            this.DashboardsTabs.push(item);
        });

        unpinList.forEach((item) => {
            this.DashboardsTabs.push(item);
        });

        this.dashbaordCount = this.DashboardsTabs.length;

        this.SelectFirstDashboard();
    }

    SelectFirstDashboard() {
        if (this.DashboardsTabs && this.DashboardsTabs.length > 0) {
            if (this.isSelectedDashboardDeleted || !this.SelectedDashboard) {
                this.isSelectedDashboardDeleted = false;
                this.SelectedDashboard = this.DashboardsTabs[0]?.Dashboard;
            }
        }
    }

    AddDashboardClicked() {
        MixPanelLocator.PostDashboardAction({ ActionName: "Open Dashboard add page" });
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Add Dashboard";

        var dashboardPM: DashboardPM = new DashboardPM();
        dashboardPM.Tenant = SessionInfo.LoggedUserTenant;
        dashboardPM.CreatedByUserId = SessionInfo.LoggedUserId;
        dashboardPM.UpdatedByUserId = SessionInfo.LoggedUserId;
        dashboardPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        dashboardPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
        dashboardPM.PermissionLevelCode = "ONM";

        logitudeWindow.WindowArgs = { EntityPM: dashboardPM, };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditDashboardComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (!s) return;
                this.GetAddedDashboard(comp.EntityPM.Id);
            });
        });
    }

    GetAddedDashboard(dashboardId: any) {
        this.DashboardListService.getSingle(dashboardId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse || myResponse.HasError) return;
            var addedDasboard = myResponse.Result;
            this.ItemsSource.unshift(addedDasboard);
            this.ChangeDashboard(addedDasboard, true, true);
            this.dashbaordCount++;
        });
    }

    TabSelectionChanged(clickdTab: DashboardTab) {
        if (!clickdTab) return;
        this.ChangeDashboard(clickdTab.Dashboard, false);
    }

    DropDwonSelectionChanged(dashboard: DashboardList) {
        MixPanelLocator.PostDashboardAction({ ActionName: "Dashboard drop down", DashboardId: dashboard.Id });
        this.ChangeDashboard(dashboard, true);
    }

    private ChangeDashboard(dashboard: DashboardList, dropDownClicked: boolean, isNew: boolean = false) {
        if (this.SelectedDashboard?.Id == dashboard.Id) return;

        this.OpenEditLayout = isNew;
        if (this.HasChanges) {
            this.ConfirmSave(dashboard);
            return;
        }
        if (!dropDownClicked) {
            this.SelectedDashboard = dashboard;
            return;
        }

        var dashboardTab: DashboardTab = this.DashboardsTabs.find(d => d.Dashboard.Id == dashboard.Id);
        if (dashboardTab) {
            if (dashboard.Id != this.SelectedDashboard.Id) this.SelectedDashboard = dashboard;
            return;
        }
        this.selectedDashboardsFromLOV.push(dashboard);
        this.selectedDashboardsFromLOV.reverse();
        this.BuildTabs();
        this.SelectedDashboard = dashboard;
    }

    private ConfirmSave(clickedDashboard: DashboardList) {
        var confirmWindow = new ConfirmWindow();
                confirmWindow.Width = 450;
                confirmWindow.Height = 190;
                confirmWindow.ShowCancelButton = true;
                confirmWindow.NoButtonText = "Don't Save";
                confirmWindow.YesButtonText = "Save ";
                confirmWindow.CancelButtonText = "Cancel";
                confirmWindow.Title = TextCodeTranslator.Translate("General.O.UnSavedChanges");
                confirmWindow.Show("This Dashboard has unsaved changes. Do you want to save it?");
                confirmWindow.WindowClosed.subscribe((event: any) => {
                    if (confirmWindow.Yes) {
                        MixPanelLocator.PostDashboardAction({ ActionName: "Confirm Window Yes Click", DashboardId: this.SelectedDashboard?.Id });
                        this.SaveDashboard(clickedDashboard);
                    }
                    else if (confirmWindow.No) {
                        MixPanelLocator.PostDashboardAction({ ActionName: "Confirm Window No Click", DashboardId: this.SelectedDashboard?.Id });
                        this.HasChanges = false;
                        this.SelectedDashboard = clickedDashboard;
                    } 
                });
    }

    SaveDashboard(clickedDashboard: DashboardList) {
        var dashboardPMService: DashboardPMService = new DashboardPMService();
        this.CurrentSession.StartBusyIndicatorSaving();
        dashboardPMService.update(this.DashboardEntity).subscribe((myResponse: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) return;
            this.HasChanges = false;
            this.ChangeDashboard(clickedDashboard, false);
        });
    }

    private isSelectedDashboardDeleted: boolean = false;
    public RefreshTabsAfterDelete(deletedDashboardId: string) {
        var deletedTab: DashboardTab = this.DashboardsTabs.find(d => d.Dashboard.Id == deletedDashboardId);
        if (deletedTab) {
            this.isSelectedDashboardDeleted = true;
            this.CheckIsLoggedUserHasPinnedDashboards();
        }
    }

    public DashboardChanged(dashboard: DashboardPM) {
        this.DashboardEntity = dashboard;
        this.ItemsSource?.forEach(item => { this.UpdateDashboardListItem(item, dashboard) });
        this.loadedDashboards?.forEach(item => { this.UpdateDashboardListItem(item, dashboard) });
        this.selectedDashboardsFromLOV?.forEach(item => { this.UpdateDashboardListItem(item, dashboard) });
        this.DashboardsTabs?.forEach(item => { this.UpdateDashboardListItem(item.Dashboard, dashboard) });
    }
    UpdateDashboardListItem(oldDashboard: DashboardList, newDashboard: DashboardPM) {
        if (oldDashboard.Id != newDashboard.Id) return;
        oldDashboard.Name = newDashboard.Name
    }

    private isClosingPinnedTab: boolean = false;
    CloseDashboardTabClicked(item: DashboardTab) {
        console.log("close tab");

        if (this.HasChanges) {
            this.ConfirmSave(item.Dashboard);
            return;
        }

        if (item.IsPinned) {
            this.UnpinThenClose(item);
        }

        else {
            this.CloseTab(item);
        }
    }
    UnpinThenClose(item: DashboardTab) {
        this.isClosingPinnedTab = true;
        this.UnpinDashboardTabClicked(item);
    }
    CloseTab(item: DashboardTab) {
        var tabIndex = this.selectedDashboardsFromLOV.indexOf(item.Dashboard);
        if (tabIndex > -1) {
            this.selectedDashboardsFromLOV.splice(tabIndex, 1);
        }

        tabIndex = this.loadedDashboards.indexOf(item.Dashboard);
        if (tabIndex > -1) {
            this.loadedDashboards.splice(tabIndex, 1);
        }

        if (item.Id == this.SelectedDashboard.Id)
            this.SelectedDashboard = null;

        this.BuildTabs();
        this.dashbaordCount = this.DashboardsTabs.length;        
    }

    PinDashboardTabClicked(item: DashboardTab) {
        console.log("pin tab");

        var tabIndex = this.selectedDashboardsFromLOV.indexOf(item.Dashboard);
        if (tabIndex > -1) {
            this.selectedDashboardsFromLOV.splice(tabIndex, 1);
        }

        var pinnedDashboard: PinnedDashboard = new PinnedDashboard();
        pinnedDashboard.Id = item.Dashboard.Id;
        pinnedDashboard.IsPredefined = item.Dashboard.Tenant == 0;
        pinnedDashboard.Order = ArrayTool.Max(this.pinnedDashboards, "Order");

        this.dashboardPMEstendedService.PinDashboard(pinnedDashboard).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.CheckIsLoggedUserHasPinnedDashboards();
            }
        });
    }

    UnpinDashboardTabClicked(item: DashboardTab) {
        if (!this.isClosingPinnedTab) {
            this.unpinnedDashboards.push(item.Dashboard);
            this.unpinnedDashboards.reverse();
        }

        console.log("unpin tab");
        this.dashboardPMEstendedService.UnpinDashboard(this.UserPinnedDashboards.Id, item.Dashboard.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                if (this.isClosingPinnedTab) {
                    this.isClosingPinnedTab = false;
                    this.CloseTab(item);
                }

                else
                    this.CheckIsLoggedUserHasPinnedDashboards();
            }
        });
    }

    @HostListener('document:click', ['$event'])
    clickout(event) {
        if (event == this.SelectedDashboard?.Id) {
            this.DashboardsTabs.filter(d => d.Id != event).forEach(item => {
                if (item.IsTabMenuOpened)
                    item.IsTabMenuOpened = false;
            });
        }

        else {
            this.DashboardsTabs.forEach(item => {
                if (item.IsTabMenuOpened)
                    item.IsTabMenuOpened = false;
            });
        }
    }
}

class DashboardTab {
    public Dashboard: DashboardList;
    public Order: number = 0;
    public DropdownId: string = null;
    constructor(dashboard: DashboardList, order:number, public fatherComponent: CustomDashboardComponent) {
        this.Dashboard = dashboard;
        this.Order = order;
        this.DropdownId = "DahboardDropdownId" + dashboard.Id;
    }

    get Id(): string {
        return this.Dashboard.Id;
    }

    get Name(): string {
        return this.Dashboard.Name;
    }

    get IsPinned() {
        if (this.fatherComponent.pinnedDashboards.find(d => d.Id == this.Dashboard.Id))
            return true;

        return false;
    }

    private isTabMenuOpened: boolean = false;
    get IsTabMenuOpened() { return this.isTabMenuOpened; }
    set IsTabMenuOpened(value: boolean) {
        if (value != undefined) {
            if (this.isTabMenuOpened != value) {
                this.isTabMenuOpened = value;
            }
        }
    }

    onRightClick(event) {
        console.log("right cick");
        event.preventDefault();

        this.fatherComponent.clickout(this.Id);

        if (this.IsTabMenuOpened) {
            this.IsTabMenuOpened = false;
        }

        else {
            this.IsTabMenuOpened = true;
        }
    }
}

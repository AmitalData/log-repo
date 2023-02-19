import { Component, ViewEncapsulation, HostListener } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BehaviorSubject, Subject } from 'rxjs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DateTool } from '../../../../Infrastructure/Tools';
import { DashboardPM } from '../../../../DashboardModule/EntityPMs/DashboardPM';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DashboardDataBinding } from 'logitude-dashboard-library/dist/types/DashboardDataBinding';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';
import { DashboardListService } from '../../../../DashboardModule/Services/StandardLists/DashboardListService';
import { DashboardList } from '../../../../DashboardModule/EntityLists/DashboardList';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { DashboardPMService } from '../../../../DashboardModule/Services/StandardPMs/DashboardPMService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DashboardPMExtendedService, PinnedDashboard } from '../../../../DashboardModule/Services/ExtendedPMs/DashboardPMExtendedService';
import { DashboardListExtendedService } from '../../../../DashboardModule/Services/ExtendedLists/DashboardListExtendedService';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { DashboardsUserSettingPM } from 'DashboardModule/EntityPMs/DashboardsUserSettingPM';
import { AnalyticsFactsFieldsMetaDataPMExtendedService } from 'DashboardModule/Services/ExtendedPMs/AnalyticsFactsFieldsMetaDataExtendedService';
import { AnalyticsFactsFieldsMetaDataPM } from 'DashboardModule/EntityPMs/AnalyticsFactsFieldsMetaDataPM';

@Component({
    templateUrl: 'CustomDashboardComponent.html',
    styleUrls: ['CustomDashboardCSS.scss'],
    selector: 'custom-dashboard',
    encapsulation: ViewEncapsulation.None,
})

export class CustomDashboardComponent extends BaseComponent {
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
    private MaxTabsCount: number = 10;
    private MaxPinnedTabsCount: number = 8;
    private dashboardPMEstendedService: DashboardPMExtendedService;
    public SectionsItemsSource: CodeNameClass[];
    public SelectedFromDashboardDropDown: boolean = false;
    public PinnedDashboards: DashboardList[];
    public DashboardsUserSetting: DashboardsUserSettingPM;
    private DashboardListExtendedService: DashboardListExtendedService;
    private AnalyticsFactsFieldsMetaDataPMExtendedService: AnalyticsFactsFieldsMetaDataPMExtendedService;
    public PresetFilters: AnalyticsFactsFieldsMetaDataPM[];
    public IsPredfineds: boolean;

    constructor() {
        super();
        this.DashboardListService = new DashboardListService();
        this.dashboardPMEstendedService = new DashboardPMExtendedService();
        this.DashboardListExtendedService = new DashboardListExtendedService();
        this.AnalyticsFactsFieldsMetaDataPMExtendedService = new AnalyticsFactsFieldsMetaDataPMExtendedService();
        this.GetData();
        this.BuildSectionsItemsSource();
    }

    BuildSectionsItemsSource() {
        this.SectionsItemsSource = [];
        this.SectionsItemsSource.push(new CodeNameClass("MYS", "My Dashboards"));
        this.SectionsItemsSource.push(new CodeNameClass("SHR", "Shared Dashboards"));
        this.SectionsItemsSource.push(new CodeNameClass("SYS", "System Dashboards"));
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
                                this.GetPresetFilterFields();
                            }, 70);
                        });
                    });
                });
            });
        });
    }

    GetPresetFilterFields() {
        this.AnalyticsFactsFieldsMetaDataPMExtendedService.GetPresetFilters().subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) return
            this.PresetFilters = myResponse.Result ?? [];
            this.LoadDashboardsForDropDown();
        });
    }

    private LoadDashboardsForDropDown() {
        this.DashboardListExtendedService.GetDashboardsForDropDown().subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) return
            this.ItemsSource = myResponse.Result ?? [];
            this.dashbaordCount = this.ItemsSource.length;
            this.DashboardDropdownLoading = false;
            this.GetPinnedDashboards();
        });
    }


    GetPinnedDashboards() {
        this.dashboardPMEstendedService.GetDashboardsUserSettings(SessionLocator.LoggedUserId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) return;
            SessionLocator.SelectedSession.StopBusyIndicator();
            this.DashboardsUserSetting = myResponse.Result;
            if (myResponse.Result) this.BuildPinnedDashboards(myResponse);
            this.BuildTabs();
        });
    }


    private BuildPinnedDashboards(myResponse: ServiceResponse) {
        this.PinnedDashboards = [];
        var pinnedDashboards = (JSON.parse(myResponse.Result.PinnedDashboards) as any[]) ?? [];
        pinnedDashboards.forEach(item => {
            var pinned = this.ItemsSource.find(x => x.Id == item.Id);
            if (pinned) this.PinnedDashboards.push(pinned);
        });
    }

    BuildTabs() {
        if (!this.ItemsSource || this.ItemsSource.length == 0) return;
        this.DashboardsTabs = [];

        if (!this.PinnedDashboards && SessionLocator.Tenant != 0) {
            this.ShowPredfinedDashboards();
            this.SelectFirstDashboard();
            this.IsPredfineds = true;
            return;
        }
        if (this.PinnedDashboards.length == 0) return;

        this.ShowPinnedDashboards();
        this.SelectFirstDashboard();
    }

    ShowPinnedDashboards() {
        let pinneds = this.PinnedDashboards.slice(0, this.MaxPinnedTabsCount);
        pinneds.forEach((dashboard) => {
            this.DashboardsTabs.push(new DashboardTab(dashboard, this, true));
        });
    }

    ShowPredfinedDashboards() {
        var predineds = this.ItemsSource.filter(x => x.PinnedByDefault && x.Tenant == 0).sort(function (a, b) {
            return (a.PredefinedOrder ?? 1000) - (b.PredefinedOrder ?? 1000) || a.CreateDate.valueOf() - b.CreateDate.valueOf();
        });
        if (predineds.length == 0) return;

        predineds = predineds.slice(0, this.MaxTabsCount);
        predineds.forEach((dashboard) => {
            this.DashboardsTabs.push(new DashboardTab(dashboard, this, true));
        });
    }

    SelectFirstDashboard() {
        if (this.DashboardsTabs && this.DashboardsTabs.length > 0) {
            this.SelectedDashboard = this.DashboardsTabs[0]?.Dashboard;
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
        logitudeWindow.Show('./DashboardModule/Components/Windows/AddEditDashboard/AddEditDashboardComponent');
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
            this.dashbaordCount++;
            this.AppendClickedDashboard(addedDasboard);
            this.ChangeDashboard(addedDasboard, false, true);
        });
    }

    TabSelectionChanged(clickdTab: DashboardTab) {
        if (!clickdTab) return;
        this.SelectedFromDashboardDropDown = false;
        this.ChangeDashboard(clickdTab.Dashboard, false);
    }

    DropDwonSelectionChanged(dashboard: DashboardList) {
        MixPanelLocator.PostDashboardAction({ ActionName: "Dashboard drop down", DashboardId: dashboard.Id });
        this.SelectedFromDashboardDropDown = true;
        this.ChangeDashboard(dashboard, true);
    }

    private ChangeDashboard(dashboard: DashboardList, dropDownClicked: boolean, isNew: boolean = false) {
        if (dashboard == null) {
            this.RemoveTab(this.SelectedDashboard.Id);
            return;
        }

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
        this.AppendClickedDashboard(dashboard);
        this.SelectedDashboard = dashboard;
    }

    AppendClickedDashboard(dashboard: DashboardList) {
        var tab = new DashboardTab(dashboard, this, false);

        if (this.DashboardsTabs.length == 0) {
            this.DashboardsTabs.push(tab);
            return;
        }
        var appendItemTo = this.DashboardsTabs.indexOf(this.DashboardsTabs.find(x => !x.IsPinned));
        if (appendItemTo == -1) this.DashboardsTabs.push(tab)
        else this.DashboardsTabs.splice(appendItemTo, 0, tab);

        if (this.DashboardsTabs.length > this.MaxTabsCount) {
            this.DashboardsTabs.pop();
        }
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
        confirmWindow.Show("This Dashboard has unsaved changes do you want to save it?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                MixPanelLocator.PostDashboardAction({ ActionName: "Confirm Window Yes Click", DashboardId: this.SelectedDashboard?.Id });
                this.SaveDashboard(clickedDashboard);
            }
            else if (confirmWindow.No) {
                MixPanelLocator.PostDashboardAction({ ActionName: "Confirm Window No Click", DashboardId: this.SelectedDashboard?.Id });
                this.HasChanges = false;
                this.ChangeDashboard(clickedDashboard, this.SelectedFromDashboardDropDown);
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
            this.ChangeDashboard(clickedDashboard, this.SelectedFromDashboardDropDown);
        });
    }

    public RefreshTabsAfterDelete(deletedDashboardId: string) {
        this.DeleteDashboardFromList(deletedDashboardId);
        this.RemoveTab(deletedDashboardId);
    }

    DeleteDashboardFromList(dashboardId: string) {
        var deletedDashboard = this.ItemsSource.find(d => d.Id == dashboardId);
        const index = this.ItemsSource.indexOf(deletedDashboard, 0);
        this.ItemsSource.splice(index, 1);
        this.dashbaordCount = this.dashbaordCount - 1;
    }

    RemoveTab(dashboardId: string) {
        var deletedTab: DashboardTab = this.DashboardsTabs.find(d => d.Dashboard.Id == dashboardId);
        if (!deletedTab) return;

        const index = this.DashboardsTabs.indexOf(deletedTab, 0);
        this.DashboardsTabs.splice(index, 1);

        if (this.DashboardsTabs.length == 0) this.SelectedDashboard = null;
        else this.SelectedDashboard = this.DashboardsTabs[0].Dashboard;
    }

    public DashboardChanged(dashboard: DashboardPM) {
        this.DashboardEntity = dashboard;
        this.ItemsSource?.forEach(item => { this.UpdateDashboardListItem(item, dashboard) });
        this.DashboardsTabs?.forEach(item => { this.UpdateDashboardListItem(item.Dashboard, dashboard) });
    }

    public RefreshAfterCopy(copiedDashboard: DashboardPM) {
        this.DashboardListService.getSingle(copiedDashboard.Id).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse || myResponse.HasError) return;
            this.ItemsSource.unshift(myResponse.Result);
            this.ChangeDashboard(myResponse.Result, true, true);
            this.dashbaordCount++;
        });
    }

    UpdateDashboardListItem(oldDashboard: DashboardList, newDashboard: DashboardPM) {
        if (oldDashboard.Id != newDashboard.Id) return;
        oldDashboard.Name = newDashboard.Name
    }

    CloseDashboardTabClicked(item: DashboardTab) {
        if (this.HasChanges) {
            this.ConfirmSave(null);
            return;
        }
        if (item.IsPinned) this.UnpinDashboardTabClicked(item, true);
        else this.RemoveTab(item.Dashboard.Id);
    }

    UnpinDashboardTabClicked(item: DashboardTab, close: boolean = false) {
        if (this.IsPredfineds) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.PinPredfinedDashboard(item, 0, close);
            return;
        }
        this.UnpinDashboard(item, close);
    }

    private UnpinDashboard(item: DashboardTab, close: boolean) {
        this.dashboardPMEstendedService.UnpinDashboard(this.DashboardsUserSetting.Id, item.Dashboard.Id).subscribe((myResponse: ServiceResponse) => {
            SessionLocator.SelectedSession.StopBusyIndicator();
            if (myResponse.HasError) return;
            const oldIndex = this.DashboardsTabs.indexOf(item, 0);
            let newIndex = this.DashboardsTabs.indexOf(this.DashboardsTabs.find(x => !x.IsPinned));
            if (newIndex == -1) newIndex = this.DashboardsTabs.length - 1;
            else newIndex = newIndex - 1;

            item.IsPinned = false;
            if (close) this.RemoveTab(item.Id);
            else this.arraymove(this.DashboardsTabs, oldIndex, newIndex);
        });
    }

    PinDashboardTabClicked(item: DashboardTab) {
        if (this.IsPredfineds) {
            SessionLocator.SelectedSession.StartBusyIndicatorLoading();
            this.PinPredfinedDashboard(null, 0);
            this.SubmitPinDashboard(item);
            return;
        }
        this.PinDashboard(item);
    }

    PinDashboard(pinnedDashboardTab: DashboardTab) {
        var pinnedDashboard: PinnedDashboard = new PinnedDashboard();
        pinnedDashboard.Id = pinnedDashboardTab.Dashboard.Id;

        this.dashboardPMEstendedService.PinDashboard(pinnedDashboard).subscribe((myResponse: ServiceResponse) => {
            SessionLocator.SelectedSession.StopBusyIndicator();
            if (myResponse.HasError) return;
            this.DashboardsUserSetting = myResponse.Result;
            this.SubmitPinDashboard(pinnedDashboardTab);
        });
    }

    private SubmitPinDashboard(pinnedDashboardTab: DashboardTab) {
        const oldIndex = this.DashboardsTabs.indexOf(pinnedDashboardTab, 0);
        let newIndex = this.DashboardsTabs.indexOf(this.DashboardsTabs.find(x => !x.IsPinned));
        if (newIndex == -1) newIndex = this.DashboardsTabs.length - 1;
        pinnedDashboardTab.IsPinned = true;
        this.arraymove(this.DashboardsTabs, oldIndex, newIndex);
    }

    PinPredfinedDashboard(dashboardTab: DashboardTab, index: number, close: boolean = false) {
        if (this.DashboardsTabs == null || this.DashboardsTabs.length == 0) return;
        if (!this.DashboardsTabs[index]) return this.PinPredfinedDashboardFinished(dashboardTab, close);

        var pinnedDashboard: PinnedDashboard = new PinnedDashboard();
        pinnedDashboard.Id = this.DashboardsTabs[index].Dashboard.Id;

        this.dashboardPMEstendedService.PinDashboard(pinnedDashboard).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) return;
            this.PinPredfinedDashboard(dashboardTab, index + 1, close);
        });
    }

    private PinPredfinedDashboardFinished(unpinnedDashboardTab: DashboardTab, close: boolean) {
        this.IsPredfineds = false;
        if (unpinnedDashboardTab) return this.GetDashboardsUserSettings(unpinnedDashboardTab, close, true);

        this.GetDashboardsUserSettings(unpinnedDashboardTab, close, false);
        SessionLocator.SelectedSession.StopBusyIndicator();
    }

    GetDashboardsUserSettings(dashboardTab: DashboardTab, close: boolean, unpin: boolean) {
        this.dashboardPMEstendedService.GetDashboardsUserSettings(SessionLocator.LoggedUserId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) return;
            this.DashboardsUserSetting = myResponse.Result;
            if (unpin) this.UnpinDashboard(dashboardTab, close);
        });
    }

    private arraymove(arr: any, fromIndex: number, toIndex: number) {
        var element = arr[fromIndex];
        arr.splice(fromIndex, 1);
        arr.splice(toIndex, 0, element);
    }

    public get MainMessage(): string {
        if (this.dashbaordCount == 0) return "There are currently no dashboards.";
        else return "There are currently no dashboards open.";
    }

    public get MainSubMessage(): string {
        if (this.dashbaordCount == 0) return "It is time to add a new one";
        else return "It's time to search for an existing dashboard or add a new one";
    }

    public get CanPinn(): boolean {
        return this.DashboardsTabs.filter(x => x.IsPinned).length < this.MaxPinnedTabsCount;
    }
}

class DashboardTab {
    public Dashboard: DashboardList;
    public DropdownId: string = null;
    public IsPinned: boolean = false;
    public FatherComponent: CustomDashboardComponent

    constructor(dashboard: DashboardList, fatherComponent: CustomDashboardComponent, isPinned: boolean) {
        this.Dashboard = dashboard;
        this.DropdownId = "DahboardDropdownId" + dashboard.Id;
        this.IsPinned = isPinned
        this.FatherComponent = fatherComponent
    }

    get Id(): string {
        return this.Dashboard.Id;
    }

    get Name(): string {
        return this.Dashboard.Name;
    }

}

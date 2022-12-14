import { Component, OnDestroy, ViewEncapsulation, Output, EventEmitter, ViewChild } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { BehaviorSubject, Subject } from 'rxjs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
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
import { DashboardTabComponent } from './DashboardTabComponent';

@Component({
    templateUrl: 'CustomDashboardComponent.html',
    styleUrls: ['CustomDashboardCSS.scss'],
    selector: 'custom-dashboard',
    encapsulation: ViewEncapsulation.None,
})

export class CustomDashboardComponent extends BaseComponent implements OnDestroy {
    @Output() SaveDashboardCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() EditLayoutChanged: EventEmitter<boolean> = new EventEmitter<boolean>();
    @ViewChild(DashboardTabComponent) child: DashboardTabComponent;

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

    constructor() {
        super();
        this.DashboardListService = new DashboardListService();
        this.GetData();
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
                                SessionLocator.SelectedSession.StopBusyIndicator();
                                this.LoadDefaultDashboards(300);
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

    private LoadDefaultDashboards(numberOfDashboard: number) {
        var filters: ApiQueryFilters = new ApiQueryFilters();
        filters.PageSize = numberOfDashboard;
        filters.SortBy = "CreateDate";
        filters.SortDirection = "Descending";

        this.DashboardListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.loadedDashboards = myResponse.Result?.slice(0, this.TabsCount);
                this.ItemsSource = myResponse.Result ?? [];
                this.dashbaordCount = this.ItemsSource.length;
                this.DashboardDropdownLoading = false;
                this.BuildTabs();
            }
        });
    }

    private selectedDashboardsFromLOV: DashboardList[] = [];
    private loadedDashboards: DashboardList[] = [];

    BuildTabs() {
        this.DashboardsTabs = [];
        var list: DashboardTab[] = [];
        var index: number = 0;

        if (this.selectedDashboardsFromLOV.length > 0) {
            this.selectedDashboardsFromLOV.forEach(item => {
                list.push(new DashboardTab(item));
                index++;
            });
        }

        if (list.length < this.TabsCount) {
            this.loadedDashboards.forEach(item => {
                if (index != this.TabsCount) {
                    list.push(new DashboardTab(item));
                    index++;
                }
            });
        }

        list.forEach((item) => {
            this.DashboardsTabs.push(item);
        })

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
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Confirm";
        confirmWindow.Show("This Dashboard has unsaved changes do you want to save it?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.SaveDashboard(clickedDashboard);
                MixPanelLocator.PostDashboardAction({ ActionName: "Confirm Window Yes Click", DashboardId: this.SelectedDashboard?.Id });
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
            if (!myResponse.HasError) return;
            this.SaveDashboardCompleted.emit(true);
            this.HasChanges = false;
            this.SelectedDashboard = clickedDashboard;
        });
    }

    private isSelectedDashboardDeleted: boolean = false;
    public RefreshTabsAfterDelete(deletedDashboardId: string) {
        var deletedTab: DashboardTab = this.DashboardsTabs.find(d => d.Dashboard.Id == deletedDashboardId);
        if (deletedTab) {
            this.isSelectedDashboardDeleted = true;
            this.LoadDefaultDashboards(300);
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
}

class DashboardTab {
    public Dashboard: DashboardList;
    get Name(): string {
        return this.Dashboard.Name;
    }
    constructor(dashboard: DashboardList) {
        this.Dashboard = dashboard;
    }
}

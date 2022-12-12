import { Component, OnInit, OnDestroy, ViewEncapsulation, ViewChildren, QueryList, Output, EventEmitter } from '@angular/core';
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
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { DashboardPMService } from '../../../DashboardModule/Services/StandardPMs/DashboardPMService';

@Component({
    templateUrl: 'CustomDashboardComponent.html',
    styleUrls: ['CustomDashboardCSS.scss'],
    selector: 'custom-dashboard',
    encapsulation: ViewEncapsulation.None,
})

export class CustomDashboardComponent extends BaseComponent implements OnInit, OnDestroy {
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;
    public DashboardsTabs: DashboardTab[] = [];
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    public HasChanges: boolean = false;
    public IsEditLayoutModeActive: boolean = false;
    public SavedDashboard: DashboardPM = null;
    @Output() SaveDashboardCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() EditLayoutChanged: EventEmitter<boolean> = new EventEmitter<boolean>();
    public dashbaordCount = -1;
    public ItemsSource: DashboardList[] = [];
    public DashboardDropdownLoading: boolean = true;
    private loadedDashboards: DashboardList[] = [];
    private DashboardListService: DashboardListService;

    constructor() {
        super();
        this.DashboardListService = new DashboardListService();
    }

    DashboardDataBinding: DashboardDataBinding = {
        isOnEditLayout: new Subject(),
        onGetLayouts: new BehaviorSubject({ lg: [] }),
        widgetUpdated: new Subject(),
        onAddWidget: new Subject(),
        onEditWidget: new Subject(),
        onApplyGlobalFilters: new Subject(),
    }

    ngOnInit(): void {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName("Dashboard").subscribe((res1: any) => {
            this._entityResourceService.getEntityResourceByTableName("Widget").subscribe((res2: any) => {
                this._entityResourceService.getEntityResourceByTableName("WidgetMeasure").subscribe((res2: any) => {
                    this._entityResourceService.getEntityResourceByTableName("AnalyticsFactsMetaData").subscribe((res3: any) => {
                        this._entityResourceService.getEntityResourceByTableName("AnalyticsFactsFieldsMetaData").subscribe((res4: any) => {
                            setTimeout(e => {
                                this.CurrentSession.StopBusyIndicator();
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
                this.loadedDashboards = myResponse.Result?.slice(0, 6);
                this.ItemsSource = myResponse.Result ?? [];
                this.dashbaordCount = this.ItemsSource.length;
                this.DashboardDropdownLoading = false;
                this.BuildTabs();
            }
        });
    }

    BuildTabs() {
        this.DashboardsTabs = [];
        var list: DashboardTab[] = [];
        var index: number = 0;

        if (this.selectedDashboardsFromLOV.length > 0) {
            this.selectedDashboardsFromLOV.forEach(item => {
                list.push(new DashboardTab(index, item));
                index++;
            });
        }

        if (list.length < 6) {
            this.loadedDashboards.forEach(item => {
                if (index != 6) {
                    list.push(new DashboardTab(index, item));
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

    private selectedDashboard: DashboardList;
    get SelectedDashboard() { return this.selectedDashboard; }
    set SelectedDashboard(value: DashboardList) {
        if (this.selectedDashboard == value) return;
        this.selectedDashboard = value;
        if (!value) return;
        MixPanelLocator.PostDashboardAction({ ActionName: "Dashboard drop down", DashboardId: value.Id });

        var addedDashboardTab: DashboardTab = this.DashboardsTabs.find(d => d.Dashboard.Id == this.SelectedDashboard.Id);
        if (addedDashboardTab) {
            if (!addedDashboardTab.IsSelected) this.SelectionChanged(addedDashboardTab);
            return;
        }
        this.selectedDashboardsFromLOV.push(value);
        this.selectedDashboardsFromLOV.reverse();
        this.BuildTabs();
        this.SelectionChanged(this.DashboardsTabs[0]);
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
            this.SelectedDashboard = addedDasboard;
            this.openEditLayout = true;
            this.dashbaordCount++;
        });
    }

    private openEditLayout: boolean = false;
    private selectedDashboardsFromLOV: DashboardList[] = [];

    public SelectedTabItem: DashboardTab;
    private clickdTab: DashboardTab;
    SelectionChanged(clickdTab: DashboardTab, fromScreen: boolean = false) {
        if (!clickdTab) return;

        this.clickdTab = clickdTab;
        if (this.HasChanges) {
            this.HasChanges = false;
            this.ConfirmSave();
        }
        else if (this.IsEditLayoutModeActive) {
            this.EditLayoutChanged.emit(true);
        }
        else {
            if (fromScreen) this.selectedDashboard = clickdTab.Dashboard;
            this.NavigateToSelectedTab();
        }
    }

    private ConfirmSave() {
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "Confirm";
        confirmWindow.Show("This Dashboard has unsaved changes do you want to save it?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.SaveDashboard();
                MixPanelLocator.PostDashboardAction({ ActionName: "Confirm Window Yes Click", DashboardId: this.SelectedDashboard?.Id });
            }

            else if (confirmWindow.No) {
                MixPanelLocator.PostDashboardAction({ ActionName: "Confirm Window No Click", DashboardId: this.SelectedDashboard?.Id });
                this.NavigateToSelectedTab();
            }
        });
    }

    SaveDashboard() {
        var dashboardPMService: DashboardPMService = new DashboardPMService();
        this.CurrentSession.StartBusyIndicatorSaving();
        dashboardPMService.update(this.SavedDashboard).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.SaveDashboardCompleted.emit(true);
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    public NavigateToSelectedTab() {
        if (this.SelectedTabItem != this.clickdTab) {
            this.SelectedTabItem = this.clickdTab;
            this.DashboardsTabs.forEach((item) => {
                item.IsSelected = false;
            });
            this.SelectedTabItem.IsSelected = true;
        }
        if (this.SelectedTabItem.IsTabLoaded || !this.SelectedTabItem.ComponentPath) return;

        let locs = this.AllLocations.toArray().filter(f => f.Code == 'DashboardTabLocation');
        let location: LocationDirective = locs.filter(f => f.ItemCode == this.SelectedTabItem.Dashboard.Id)[0];
        if (location == null) {
            this.Retries = 0;
            this.RunComponentTimer();
            return;
        }

        SessionLocator.DynamicLoader.Load(this.SelectedTabItem.ComponentPath, location.viewContainerRef).then(cmpRef => {
            this.SelectedTabItem.IsTabLoaded = true;
            if (this.DashboardsTabs.filter(p => p.Dashboard.Id == this.SelectedTabItem.Dashboard.Id)[0]) {
                this.DashboardsTabs.filter(p => p.Dashboard.Id == this.SelectedTabItem.Dashboard.Id)[0].IsTabLoaded = true;
            }

            if (this.SelectedTabItem.Dashboard.Id) {
                cmpRef.instance.Intialize({
                    SelectedDashboardId: this.SelectedTabItem.Dashboard.Id,
                    OpenEditLayout: this.openEditLayout,
                    FatherComponent: this,
                });
            }
        });
    }

    private Retries: number = 0;
    private timerToken: any;

    RunComponentTimer() {
        this.Retries++;
        if (this.timerToken) clearTimeout(this.timerToken);
        if (this.Retries < 10) this.timerToken = setTimeout(() => this.NavigateToSelectedTab(), 1);
    }

    public RefereshTabAfterEdit(dashboard: DashboardPM) {
        this.SelectedTabItem.Name = dashboard.Name;
    }

    private isSelectedDashboardDeleted: boolean = false;
    public RefreshTabsAfterDelete(deletedDashboardId: string) {
        var deletedTab: DashboardTab = this.DashboardsTabs.find(d => d.Dashboard.Id == deletedDashboardId);
        if (deletedTab) {
            this.isSelectedDashboardDeleted = true;
            this.LoadDefaultDashboards(300);
        }
    }
}

class DashboardTab {
    public Index: number;
    public Dashboard: DashboardList;
    public Name: string;
    public ComponentPath: string;
    public IsSelected: boolean = false;
    public IsTabLoaded: boolean = false;
    constructor(index: number, dashboard: DashboardList) {
        this.Index = index;
        this.Dashboard = dashboard;
        this.Name = dashboard.Name;
        this.ComponentPath = "./Dashboard/Components/Workspace/DashboardTabComponent";
    }
}

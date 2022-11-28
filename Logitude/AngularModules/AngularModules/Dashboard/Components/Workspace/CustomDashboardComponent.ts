import { Component, AfterViewInit, OnInit, Input, OnDestroy, ViewEncapsulation, ViewChildren, QueryList, Output, EventEmitter } from '@angular/core';
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
import { LastFilterClass } from '../../../Infrastructure/Utilities/LastFilterClass';
import { DashboardListService } from '../../../DashboardModule/Services/StandardLists/DashboardListService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DashboardList } from '../../../DashboardModule/EntityLists/DashboardList';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { DashboardPMExtendedService } from '../../../DashboardModule/Services/ExtendedPMs/DashboardPMExtendedService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { DashboardPMService } from '../../../DashboardModule/Services/StandardPMs/DashboardPMService';
import { Cloner } from '../../../Infrastructure/Utilities/Cloner';

@Component({
    templateUrl: 'CustomDashboardComponent.html',
    styleUrls: ['CustomDashboardCSS.scss'],
    selector: 'custom-dashboard',
    encapsulation: ViewEncapsulation.None,
})

export class CustomDashboardComponent extends BaseComponent implements OnInit, AfterViewInit, OnDestroy {    
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;        
    @Input('Show') Show = false;
    private filterName_SelectedDashboard: string = "SelectedDashboard";
    private filterControlNameSpace: string = "Workspace.CustomDashboard";    
    public DashboardsTabs: DashboardTab[] = [];
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private dashboardExtendedService: DashboardPMExtendedService;
    public HasChanges: boolean = false;
    public IsEditLayoutModeActive: boolean = false;
    public SavedDashboard: DashboardPM = null;
    @Output() SaveDashboardCompleted: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() EditLayoutChanged: EventEmitter<boolean> = new EventEmitter<boolean>();
    constructor() {
        super();
        this.dashboardExtendedService = new DashboardPMExtendedService();
        this.RunComponent();
    }

    private Retries: number = 0;
    private timerToken: any;
    RunComponent() {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }

            else {
                this.selectedDashboardId = this.DashboardsTabs[0]?.DashboardId;
                this.SelectionChanged(this.DashboardsTabs[0]);
            }
        }

        else {
            this.RunComponentTimer();
        }
    }
    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 10) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    InitComponent() {
        //this.Show = true;
    }

    DashboardDataBinding: DashboardDataBinding = {
        isOnEditLayout: new Subject(),
        onGetLayouts: new BehaviorSubject({ lg: [] }),
        widgetUpdated: new Subject(),
        onAddWidget: new Subject(),
        onEditWidget: new Subject(),
    }

    ngOnInit(): void {
        this._entityResourceService.getEntityResourceByTableName("Dashboard").subscribe((res1: any) => {
            this._entityResourceService.getEntityResourceByTableName("Widget").subscribe((res2: any) => {
                this._entityResourceService.getEntityResourceByTableName("WidgetMeasure").subscribe((res2: any) => {
                    this._entityResourceService.getEntityResourceByTableName("AnalyticsFactsMetaData").subscribe((res3: any) => {
                        this._entityResourceService.getEntityResourceByTableName("AnalyticsFactsFieldsMetaData").subscribe((res4: any) => {
                            setTimeout(e => {
                                this.LoadDefaultDashboards(6);
                            }, 70);
                        });
                    });
                });
            });
        });
    }

    ngAfterViewInit(): void {
        this.Show = true;
    }

    private SessionEvent: any = null;
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    private loadedDashboards: DashboardList[] = [];
    private previousSelectedDashboards: DashboardList[] = [];
    private GetDashboardsForTabs() {
        this.CurrentSession.StartBusyIndicatorLoading();

        var rememberedDashboards: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_SelectedDashboard);
        if (!AppTool.IsNullOrEmpty(rememberedDashboards)) {
            this.dashboardExtendedService.GetDashboardsFromIds(rememberedDashboards).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {
                    this.previousSelectedDashboards = myResponse.Result;
                    if (this.previousSelectedDashboards.length == 6) {
                        this.BuildTabs(true);
                    }

                    else if (this.previousSelectedDashboards.length < 6) {
                        this.LoadDefaultDashboards(6 - this.previousSelectedDashboards.length);
                    }
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }

        else {
            this.LoadDefaultDashboards(6);
        }
    }
    private LoadDefaultDashboards(numberOfDashboard: number) {
        var filters: ApiQueryFilters = new ApiQueryFilters();
        filters.PageSize = numberOfDashboard;
        filters.SortBy = "CreateDate";
        filters.SortDirection = "Descending";

        var dashboardListService: DashboardListService = new DashboardListService();
        dashboardListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.loadedDashboards = myResponse.Result;
                this.BuildTabs();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    BuildTabs(isAllPrevious: boolean = false) {
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
            if (isAllPrevious) {
                this.previousSelectedDashboards.forEach(item => {
                    if (index != 6) {
                        list.push(new DashboardTab(index, item));
                        index++;
                    }
                });
            }

            else {
                this.loadedDashboards.forEach(item => {
                    if (index != 6) {
                        list.push(new DashboardTab(index, item));
                        index++;
                    }
                });
            }
        }

        list.forEach((item) => {
            this.DashboardsTabs.push(item);
        })

        this.selectedDashboardId = this.DashboardsTabs[0]?.DashboardId;
        this.SelectionChanged(this.DashboardsTabs[0]);
    }

    private selectedDashboardId: string;
    get SelectedDashboardId() { return this.selectedDashboardId; }
    set SelectedDashboardId(value: string) {
        if (this.selectedDashboardId != value) {
            this.selectedDashboardId = value;

            if (value) {
                MixPanelLocator.PostDashboardAction({ ActionName: "Dashboard drop down", DashboardId: value });
            }
        }
    }

    private UpdateSelectedDashboards() {
        //var selectedDashboards: string = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_SelectedDashboard);
        //if (AppTool.IsNullOrEmpty(selectedDashboards)) {
        //    selectedDashboards = this.SelectedDashboardId;
        //}

        //else {
        //    selectedDashboards = selectedDashboards + "," + this.SelectedDashboardId;
        //}

        //LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_SelectedDashboard, selectedDashboards);
    }

    private selectedDashboard: DashboardList;
    get SelectedDashboard() { return this.selectedDashboard; }
    set SelectedDashboard(value: DashboardList) {
        if (this.selectedDashboard != value) {
            this.selectedDashboard = value;

            if (value) {
                var addedDashboardTab: DashboardTab = this.DashboardsTabs.find(d => d.DashboardId == this.SelectedDashboard.Id);
                if (addedDashboardTab) {
                    if (!addedDashboardTab.IsSelected)
                        this.SelectionChanged(addedDashboardTab);
                }

                else {
                    this.selectedDashboardsFromLOV.push(value);
                    this.selectedDashboardsFromLOV.reverse();
                    this.UpdateSelectedDashboards();
                    this.BuildTabs();
                }
            }
        }
    }

    CheckIfDashboardTabAdded(): boolean {
        var isTabAdded: boolean = false;
        if (this.DashboardsTabs.find(d => d.DashboardId == this.SelectedDashboard.Id)) {
            isTabAdded = true;
        }

        return isTabAdded;
    }

    AdDashboardClicked() {
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
                if (s) {
                    this.SelectedDashboardId = comp.EntityPM.Id;
                    this.openEditLayout = true;
                }
            });
        });
    }    

    private openEditLayout: boolean = false;
    private selectedDashboardsFromLOV: DashboardList[] = [];
    
    public SelectedTabItem: DashboardTab;
    private clickdTab: DashboardTab;
    SelectionChanged(clickdTab: DashboardTab) {
        if (clickdTab != null) {
            this.clickdTab = clickdTab;
            if (this.HasChanges) {
                this.HasChanges = false;
                this.ConfirmSave();
            }

            else if (this.IsEditLayoutModeActive) {
                this.EditLayoutChanged.emit(true);
            }

            else {
                this.NavigateToSelectedTab();
            }
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
                this.RejectChanges();
                this.NavigateToSelectedTab();
            }
        });
    }
    SaveDashboard() {
        //this.CheckDeletedWidgets(dashboard);

        var dashboardPMService: DashboardPMService = new DashboardPMService();
        this.CurrentSession.StartBusyIndicatorSaving();
        dashboardPMService.update(this.SavedDashboard).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.SaveDashboardCompleted.emit(true);
                //this.NavigateToSelectedTab(clickdTab);
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

        if (this.SelectedTabItem.IsTabLoaded) {

        }

        else {
            let locs = this.AllLocations.toArray().filter(f => f.Code == 'DashboardTabLocation');

            let location: LocationDirective = locs.filter(f => f.ItemCode == this.SelectedTabItem.DashboardId)[0];
            if (location == null) {
                this.Retries = 0;
                this.RunComponentTimer();
            }

            if (location) {
                if (this.SelectedTabItem.IsTabLoaded) {

                }

                else if (this.SelectedTabItem.ComponentPath) {
                    SessionLocator.DynamicLoader.Load(this.SelectedTabItem.ComponentPath, location.viewContainerRef).then(cmpRef => {
                        this.SelectedTabItem.IsTabLoaded = true;
                        if (this.DashboardsTabs.filter(p => p.DashboardId == this.SelectedTabItem.DashboardId)[0]) {
                            this.DashboardsTabs.filter(p => p.DashboardId == this.SelectedTabItem.DashboardId)[0].IsTabLoaded = true;
                        }

                        if (this.SelectedTabItem.DashboardId) {
                            cmpRef.instance.Intialize({
                                SelectedDashboardId: this.SelectedTabItem.DashboardId,
                                OpenEditLayout: this.openEditLayout,
                                FatherComponent: this,
                            });
                        }
                    });
                }
            }
        }
    }

    public RefereshTabAfterEdit(dashboard: DashboardPM) {
        this.SelectedTabItem.Name = dashboard.Name;
    }

    public RefreshTabsAfterDelete(deletedDashboardId: string) {
        var deletedTab: DashboardTab = this.DashboardsTabs.find(d => d.DashboardId == deletedDashboardId);
        if (deletedTab) {
            var index = this.DashboardsTabs.indexOf(deletedTab);
            if (index != -1) {
                this.DashboardsTabs.splice(index, 1);
                this.BuildTabs();

                //this.selectedDashboardId = this.DashboardsTabs[0]?.DashboardId;
                //this.SelectionChanged(this.DashboardsTabs[0]);
            }
        }
    }

    private myCloner: Cloner;
    private Clone() {
        //this.myCloner = new Cloner(this.SelectedDashboard);
        //this.myCloner.AddField('Name');
        //this.myCloner.AddField('Description');
        //this.myCloner.AddField('PermissionLevelCode');

        //this.myCloner.AddEntity(this.SelectedDashboard);
    }
    private RejectChanges() {
        //this.SavedDashboard.Widgets = this.CloneDashboardLayout;
        //this.reactWidgetsLayout = this.BindReactWidgets(this.SelectedDashboard.Widgets);
        //this.DashboardDataBinding.onGetLayouts.next(DashboardMapping.deepClone(this.reactWidgetsLayout));
    }
}

class DashboardTab {
    public Index: number;
    public DashboardId: string;
    public Name: string;
    public ComponentPath: string;
    public IsSelected: boolean = false;
    public IsTabLoaded: boolean = false;
    constructor(index: number, dashboard: DashboardList) {
        this.Index = index;
        this.DashboardId = dashboard.Id;
        this.Name = dashboard.Name;
        this.ComponentPath = "./Dashboard/Components/Workspace/DashboardTabComponent";
    }
}

import { Component, AfterViewInit, OnInit, Input, OnDestroy, ViewEncapsulation, ViewChildren, QueryList } from '@angular/core';
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

@Component({
    templateUrl: 'CustomDashboardComponent.html',
    styleUrls: ['CustomDashboardComponent.css', 'dashboard.scss'],
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
    constructor() {
        super();
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
                                this.GetDefaultDashboards();                                
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
    private GetDefaultDashboards() {
        this.CurrentSession.StartBusyIndicatorLoading();

        var filters: ApiQueryFilters = new ApiQueryFilters();
        filters.PageSize = 6;
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

        this.SelectionChanged(this.DashboardsTabs[0]);
    }

    private selectedDashboardId: string;
    get SelectedDashboardId() { return this.selectedDashboardId; }
    set SelectedDashboardId(value: string) {
        if (this.selectedDashboardId != value) {
            this.selectedDashboardId = value;

            if (value)
                MixPanelLocator.PostDashboardAction({ ActionName: "Dashboard drop down", DashboardId: value });            

            LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_SelectedDashboard, value);
        }
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
    SelectionChanged(clickdTab: DashboardTab) {
        if (clickdTab != null) {
            if (this.SelectedTabItem != clickdTab) {
                this.SelectedTabItem = clickdTab;

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
                                });
                            }
                        });
                    }
                }
            }
        }
    }

    //private myCloner: Cloner;
    //private Clone() {
    //    this.myCloner = new Cloner(this.SelectedDashboard);
    //    this.myCloner.AddField('Name');
    //    this.myCloner.AddField('Description');
    //    this.myCloner.AddField('PermissionLevelCode');

    //    this.myCloner.AddEntity(this.SelectedDashboard);
    //}
    //private RejectChanges() {
    //    this.SelectedDashboard.Widgets = this.CloneDashboardLayout;
    //    this.reactWidgetsLayout = this.BindReactWidgets(this.SelectedDashboard.Widgets);
    //    this.DashboardDataBinding.onGetLayouts.next(DashboardMapping.deepClone(this.reactWidgetsLayout));
    //}
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

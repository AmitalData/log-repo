import { Component, ViewChild, ElementRef, AfterViewInit, ViewEncapsulation, OnInit,Input } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import * as React from 'react';
import Dashboard from 'logitude-dashboard-library';
import * as ReactDOM from 'react-dom';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ReactDashboardPM, ReactDashboardSharedUserPM } from 'logitude-dashboard-library/dist/types/Dashboard';
import { DashboardDataBinding } from 'logitude-dashboard-library/dist/types/DashboardDataBinding';
import { BehaviorSubject, forkJoin } from 'rxjs';
import { ReactWidgetPM } from 'logitude-dashboard-library/dist/types/widget';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';
import { WidgetPM } from '../../../DashboardModule/EntityPMs/WidgetPM';
import { DashboardPMService } from '../../../DashboardModule/Services/StandardPMs/DashboardPMService';
import { DashboardPMExtendedService } from '../../../DashboardModule/Services/ExtendedPMs/DashboardPMExtendedService';
import { WidgetMeasurePM } from '../../../DashboardModule/EntityPMs/WidgetMeasurePM';
import { ReactWidgetMeasurePM } from 'logitude-dashboard-library/dist/types/ReactWidgetMeasurePM';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { DataPointSelection } from 'logitude-dashboard-library/dist/types/SeriesMeasure';
import { DashboardSharedUserPM } from '../../../DashboardModule/EntityPMs/DashboardSharedUserPM';

@Component({
    templateUrl:'CustomDashboardComponent.html',

    styleUrls:['CustomDashboardComponent.css'],
    selector:'custom-dashboard',
    encapsulation:ViewEncapsulation.ShadowDom
})

export class CustomDashboardComponent implements OnInit,  AfterViewInit {
    _show:boolean = false;
    @Input('show') set show(value){
        console.log('CustomDashboardComponent show= ',value);
        if(value && !this._show){
            this.ShowDashboard();
        }
        this._show = value;
        
    } 
    get show(){
        return this._show;
    }
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    private CurrentSession = SessionLocator.SelectedSession;    
    @ViewChild('reactDashboradContainer') reactDashboradContainer: ElementRef;
    private dashboardPMService: DashboardPMService;
    private dashboardPMExtendedService: DashboardPMExtendedService;
    private myDashboardPM: DashboardPM;
    constructor() {
        this.dashboardPMService = new DashboardPMService();
        this.dashboardPMExtendedService = new DashboardPMExtendedService();
        this.myDashboardPM = new DashboardPM();
    }
    ngOnInit(): void {
        this.GetDashboards();
    }

    ngAfterViewInit(): void {
        
    }
    ShowDashboard(){
        this._entityResourceService.getEntityResourceByTableName("Dashboard").subscribe((res1: any) => {
            this._entityResourceService.getEntityResourceByTableName("Widget").subscribe((res2: any) => {
                this._entityResourceService.getEntityResourceByTableName("WidgetMeasure").subscribe((res2: any) => {
                    this._entityResourceService.getEntityResourceByTableName("AnalyticsFactsMetaData").subscribe((res3: any) => {
                        this._entityResourceService.getEntityResourceByTableName("AnalyticsFactsFieldsMetaData").subscribe((res4: any) => {
                            setTimeout(e=>{
                                this.renderNewDashboard();
                            },70);
                        });
                    });
                });
            });
        });
    }

    private selectedDashboard: ReactDashboardPM = {} as ReactDashboardPM;
    private dashboardDataBinding: DashboardDataBinding =
        {
            onGetAllDashboards: new BehaviorSubject<ReactDashboardPM[]>([]),
            onGetDashboard: new BehaviorSubject<ReactDashboardPM>({} as ReactDashboardPM),
            onAddUpdateWidget: new BehaviorSubject<boolean>(false),
        };

    public AllDashboards: DashboardPM[] = [];
    private GetDashboards() {        
        this.dashboardPMExtendedService.GetDashboardPMs().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllDashboards = myResponse.Result;
            }

            var reactDashboards: ReactDashboardPM[] = [];
            this.AllDashboards.forEach(element => {
                reactDashboards.push(this.GetReactDashboard(element));
            });

            this.dashboardDataBinding.onGetAllDashboards.next(reactDashboards);

            if (this.AllDashboards) {
                this.GetSingleDashboardWithWidgets(this.AllDashboards[0].Id);                
            }
        });
    }
    GetSingleDashboardWithWidgets(dashboardId: string) {
        this.dashboardPMService.get(dashboardId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.myDashboardPM = myResponse.Result;
                if (this.myDashboardPM) {
                    var myReactDashboard: ReactDashboardPM = this.GetReactDashboard(this.myDashboardPM)
                    this.dashboardDataBinding.onGetDashboard.next(myReactDashboard);
                    this.selectedDashboard = myReactDashboard;
                }
            }           
        });        
    }

    renderNewDashboard() {
        ReactDOM.render(React.createElement(Dashboard, {
            token: SessionInfo.Token,
            tenant: SessionInfo.LoggedUserTenant,
            userId: SessionInfo.LoggedUserId,
            dataBinding: this.dashboardDataBinding,
            openAddEditWidget: this.OpenDashboardWidgetWindow.bind(this),
            openAddEditDashboard: this.OpenDashboardWindow.bind(this),
            onChangeDashboard: this.OnChangeDashboard.bind(this),
            onSaveDashboard: this.OnSaveDashboard.bind(this),
            onSelectDataPoint:this.onSelectDataPoint.bind(this),
        }),
            this.reactDashboradContainer.nativeElement);
    }
    private onSelectDataPoint(dataPointSelection: DataPointSelection){

    }
    private OpenDashboardWindow(dashboard: ReactDashboardPM) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = dashboard != null ? "Edit Dashboard" : "Add Dashboard";

        var dashboardPM: DashboardPM = null;
        if (dashboard != null) {
            dashboardPM = this.myDashboardPM;
        }

        else {
            dashboardPM = new DashboardPM();
            dashboardPM.Tenant = SessionInfo.LoggedUserTenant;
            dashboardPM.CreatedByUserId = SessionInfo.LoggedUserId;
            dashboardPM.UpdatedByUserId = SessionInfo.LoggedUserId;
            dashboardPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            dashboardPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            dashboardPM.PermissionLevelCode = "ONM";
        }

        logitudeWindow.WindowArgs = { EntityPM: dashboardPM, };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditDashboardComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.GetDashboards();
                }
            });
        });
    }    
    private OpenDashboardWidgetWindow(widget: ReactWidgetPM) {
        var myWidget: WidgetPM = this.myDashboardPM.Widgets.filter(d => d.Id == widget.Id)[0];
        if (myWidget == null) {
            myWidget = new WidgetPM(this.myDashboardPM);
            myWidget.TypeCode = widget.TypeCode;
            myWidget.Tenant = SessionInfo.LoggedUserTenant;
            myWidget.StartPotistion = widget.StartPotistion;
            myWidget.EndPosition = widget.EndPosition;
            myWidget.DateGroupCode = widget.DateGroupCode;
            myWidget.SortDirection = widget.SortDirection;
            myWidget.SortBy = widget.SortBy;
            myWidget.MaximumGrouping = widget.MaximumGrouping;
        }

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = !AppTool.IsNullOrEmpty(widget.Id) ? "Edit Widget" : "Add Widget";
        logitudeWindow.WindowArgs = { EntityPM: myWidget, IsNew: widget.ChangeSetOp == "Insert", DashboardPM: this.myDashboardPM };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditWidgetComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.selectedDashboard = this.GetReactDashboard(comp.DashboardPM);
                    this.myDashboardPM = comp.DashboardPM;
                    this.dashboardDataBinding.onGetDashboard.next(this.selectedDashboard);
                    this.dashboardDataBinding.onAddUpdateWidget.next(true);
                }
            });
        });
    }

    GetReactDashboard(dashboard: DashboardPM): ReactDashboardPM {
        var myDashboard: ReactDashboardPM = {} as ReactDashboardPM;

        if (dashboard) {
            myDashboard.Id = dashboard.Id;
            myDashboard.Tenant = dashboard.Tenant;
            myDashboard.Name = dashboard.Name;
            myDashboard.Description = dashboard.Description;
            myDashboard.CreateDate = dashboard.CreateDate;
            myDashboard.CreatedByUserId = dashboard.CreatedByUserId;
            myDashboard.UpdateDate = dashboard.UpdateDate;
            myDashboard.UpdatedByUserId = dashboard.UpdatedByUserId;
            myDashboard.PermissionLevelCode = dashboard.PermissionLevelCode;
            myDashboard.Widgets = [];
            myDashboard.DashboardSharedUsers = [];

            dashboard.Widgets.forEach(item => {
                myDashboard.Widgets.push(this.GetReactWidget(item));
            });

            dashboard.DashboardSharedUsers.forEach(item => {
                myDashboard.DashboardSharedUsers.push(this.GetReactSharedUser(item));
            });
        }

        return myDashboard;
    }
    GetReactWidget(widget: WidgetPM): ReactWidgetPM {
        var myWidget: ReactWidgetPM = {} as ReactWidgetPM;

        if (widget) {
            myWidget.Id = widget.Id;
            myWidget.Tenant = widget.Tenant;
            myWidget.Title = widget.Title;
            myWidget.GroupById = widget.GroupById;
            myWidget.DashboardId = widget.DashboardId;
            myWidget.StartPotistion = widget.StartPotistion;
            myWidget.EndPosition = widget.EndPosition;
            myWidget.EntityId = widget.EntityId;
            myWidget.TypeCode = widget.TypeCode as "line" | "area" | "bar" | "histogram" | "pie" | "donut" | "radialBar" | "scatter" | "bubble" | "heatmap" | "treemap" | "boxPlot" | "candlestick" | "radar" | "polarArea" | "rangeBar";
            myWidget.WidgetMeasures = [];
            myWidget.Filters = widget.Filters;
            myWidget.DateGroupCode = widget.DateGroupCode;
            myWidget.SortBy = widget.SortBy;
            myWidget.SortDirection = widget.SortDirection;
            myWidget.MaximumGrouping = widget.MaximumGrouping;

            widget.WidgetMeasures.forEach(item => {
                myWidget.WidgetMeasures.push(this.GetReactWidgetMeasure(item));
            });
        }

        return myWidget;
    }
    GetReactSharedUser(user: DashboardSharedUserPM): ReactDashboardSharedUserPM {
        var myUser: ReactDashboardSharedUserPM = {} as ReactDashboardSharedUserPM;

        if (user) {
            myUser.Id = user.Id;
            myUser.Tenant = user.Tenant;
            myUser.UserId = user.UserId;
            myUser.UserName = user.UserName;
            myUser.DashboardId = user.DashboardId;
        }

        return myUser;
    }
    GetReactWidgetMeasure(widgetMeasure: WidgetMeasurePM): ReactWidgetMeasurePM {
        var myWidgetMeasuer: ReactWidgetMeasurePM = {} as ReactWidgetMeasurePM;

        if (widgetMeasure) {
            myWidgetMeasuer.Id = widgetMeasure.Id;
            myWidgetMeasuer.Tenant = widgetMeasure.Tenant;
            myWidgetMeasuer.WidgetId = widgetMeasure.WidgetId;
            myWidgetMeasuer.MeasureCode = widgetMeasure.MeasureCode;
            myWidgetMeasuer.MeasureFieldId = widgetMeasure.MeasureFieldId;
        }

        return myWidgetMeasuer;
    }

    private OnChangeDashboard(dashboard: ReactDashboardPM) {
        this.GetSingleDashboardWithWidgets(dashboard.Id);
    }
    private OnSaveDashboard(dashboard: ReactDashboardPM) {
        this.CheckDeletedWidgets(dashboard);
        this.MapWidgetsPositions(dashboard);

        this.dashboardPMService.update(this.myDashboardPM).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.myDashboardPM = myResponse.Result;
                this.selectedDashboard = this.GetReactDashboard(this.myDashboardPM);
                this.dashboardDataBinding.onGetDashboard.next(this.selectedDashboard);
                this.dashboardDataBinding.onAddUpdateWidget.next(true);
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private CheckDeletedWidgets(dashboard: ReactDashboardPM) {
        var deletedWidgets: WidgetPM[] = [];

        this.myDashboardPM.Widgets.forEach(item => {
            if (dashboard.Widgets.filter(d => d.Id == item.Id)[0] == null) {
                deletedWidgets.push(item);
            }
        });

        deletedWidgets.forEach(item => {
            this.myDashboardPM.RemoveWidget(item);
        });
    }
    private MapWidgetsPositions(dashboard: ReactDashboardPM) {
        dashboard.Widgets.forEach(item => {
            var myWidgetPm: WidgetPM = this.myDashboardPM.Widgets.filter(d => d.Id == item.Id)[0];
            if (myWidgetPm) {
                myWidgetPm.EndPosition = item.EndPosition;
                myWidgetPm.StartPotistion = item.StartPotistion;
            }
        });
    }
}




import { Component, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import * as React from 'react';
import Dashboard from 'logitude-dashboard-library';
import * as ReactDOM from 'react-dom';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ReactDashboardPM } from 'logitude-dashboard-library/dist/types/Dashboard';
import { DashboardDataBinding } from 'logitude-dashboard-library/dist/types/DashboardDataBinding';
import { BehaviorSubject } from 'rxjs';
import { ReactWidgetPM } from 'logitude-dashboard-library/dist/types/widget';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';
import { WidgetPM } from '../../../DashboardModule/EntityPMs/WidgetPM';
import { DashboardPMService } from '../../../DashboardModule/Services/StandardPMs/DashboardPMService';
import { DashboardPMExtendedService } from '../../../DashboardModule/Services/ExtendedPMs/DashboardPMExtendedService';
import { WidgetMeasurePM } from '../../../DashboardModule/EntityPMs/WidgetMeasurePM';
import { ReactWidgetMeasurePM } from 'logitude-dashboard-library/dist/types/ReactWidgetMeasurePM';

@Component({
    template:
        `
        <div class="new-dashboard" #reactDashboradContainer>
        </div>
    `,

    styles:
        [`
    .new-dashboard{
        width: 100%;
        height: 100%;
        padding: 10px 0px 0px 0px;
     }
    `],
})

export class CustomDashboardComponent implements  AfterViewInit {
    private CurrentSession = SessionLocator.SelectedSession;    
    private isNewDashboardRendered: boolean = false;
    @ViewChild('reactDashboradContainer') reactDashboradContainer: ElementRef;
    private dashboardPMService: DashboardPMService;
    private dashboardPMExtendedService: DashboardPMExtendedService;
    constructor() {
        this.dashboardPMService = new DashboardPMService();
        this.dashboardPMExtendedService = new DashboardPMExtendedService();
    }

    ngAfterViewInit(): void {
        this.renderNewDashboard();
    }

    public InitComponent() {
        this.GetDashboards();
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
                var myDashbord: DashboardPM = myResponse.Result;
                if (myDashbord) {
                    var myReactDashboard: ReactDashboardPM = this.GetReactDashboard(myDashbord)
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
        }),
            this.reactDashboradContainer.nativeElement);
    }
    private OpenDashboardWindow(dashboard: ReactDashboardPM) {
        var myDashboard: DashboardPM = this.GetDashboardEntity(dashboard);
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = dashboard != null ? "Edit Dashboard" : "Add Dashboard";
        logitudeWindow.WindowArgs = { EntityPM: myDashboard, };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditDashboardComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.GetDashboards();
                    //this.dashboardDataBinding.onGetDashboard.next(this.GetReactDashboard(myDashboard));
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
            myDashboard.Widgets = [];

            dashboard.Widgets.forEach(item => {
                myDashboard.Widgets.push(this.GetReactWidget(item));
            });
        }

        return myDashboard;
    }
    GetDashboardEntity(dashboard: ReactDashboardPM): DashboardPM {
        var myDashboard: DashboardPM = new DashboardPM();

        if (dashboard) {
            myDashboard.Id = dashboard.Id;
            myDashboard.Tenant = dashboard.Tenant;
            myDashboard.Name = dashboard.Name;
            myDashboard.Description = dashboard.Description;
            myDashboard.CreateDate = dashboard.CreateDate;
            myDashboard.CreatedByUserId = dashboard.CreatedByUserId;
            myDashboard.UpdateDate = dashboard.UpdateDate;
            myDashboard.UpdatedByUserId = dashboard.UpdatedByUserId;
            myDashboard.Widgets = [];

            dashboard.Widgets.forEach(item => {
                myDashboard.Widgets.push(this.GetWidgetEntity(item));
            });
        }

        return myDashboard;
    }
    private OpenDashboardWidgetWindow(widget: ReactWidgetPM) {
        var myWidget: WidgetPM = this.GetWidgetEntity(widget);
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = !AppTool.IsNullOrEmpty(widget.Id) ? "Edit Widget" : "Add Widget";
        logitudeWindow.WindowArgs = { EntityPM: myWidget, DashboardPM: this.GetDashboardEntity(this.selectedDashboard) };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditWidgetComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.selectedDashboard = this.GetReactDashboard(comp.DashboardPM);
                    this.dashboardDataBinding.onGetDashboard.next(this.selectedDashboard);
                    this.dashboardDataBinding.onAddUpdateWidget.next(true);
                }
            });
        });
    }
    GetWidgetEntity(widget: ReactWidgetPM): WidgetPM {
        var myWidget: WidgetPM = new WidgetPM(null);

        if (widget) {
            myWidget.Id = widget.Id;
            myWidget.Tenant = widget.Tenant;
            myWidget.Title = widget.Title;
            myWidget.GroupById = widget.GroupById;
            myWidget.DashboardId = widget.DashboardId;
            myWidget.StartPotistion = widget.StartPotistion;
            myWidget.EndPosition = widget.EndPosition;
            myWidget.TypeCode = widget.TypeCode;
            myWidget.EntityId = widget.EntityId;
            myWidget.Filters = widget.Filters;
            myWidget.WidgetMeasures = [];

            if (widget.WidgetMeasures) {
                widget.WidgetMeasures.forEach(item => {
                    myWidget.WidgetMeasures.push(this.GetWidgetMeasureEntity(item));
                });
            }
        }

        return myWidget;
    }
    GetReactWidget(widget: WidgetPM): ReactWidgetPM {
        var myWidget: ReactWidgetPM = {} as ReactWidgetPM;

        if (widget) {
            myWidget.Id = widget.Id;
            myWidget.Tenant = widget.Tenant;
            myWidget.Title = widget.Title;
            myWidget.GroupBy = widget.GroupById;
            myWidget.DashboardId = widget.DashboardId;
            myWidget.StartPotistion = widget.StartPotistion;
            myWidget.EndPosition = widget.EndPosition;
            myWidget.EntityId = widget.EntityId;
            myWidget.TypeCode = widget.TypeCode as "line" | "area" | "bar" | "histogram" | "pie" | "donut" | "radialBar" | "scatter" | "bubble" | "heatmap" | "treemap" | "boxPlot" | "candlestick" | "radar" | "polarArea" | "rangeBar";
            myWidget.WidgetMeasures = [];
            myWidget.Filters = widget.Filters;

            widget.WidgetMeasures.forEach(item => {
                myWidget.WidgetMeasures.push(this.GetReactWidgetMeasure(item));
            });
        }

        return myWidget;
    }
    GetWidgetMeasureEntity(widgetMeasure: ReactWidgetMeasurePM): WidgetMeasurePM {
        var myWidgetMeasuer: WidgetMeasurePM = new WidgetMeasurePM(null);

        if (widgetMeasure) {
            myWidgetMeasuer.Id = widgetMeasure.Id;
            myWidgetMeasuer.Tenant = widgetMeasure.Tenant;
            myWidgetMeasuer.WidgetId = widgetMeasure.WidgetId;
            myWidgetMeasuer.MeasureCode = widgetMeasure.MeasureCode;
            myWidgetMeasuer.MeasureFieldId = widgetMeasure.MeasureFieldId;
        }

        return myWidgetMeasuer;
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
        this.selectedDashboard = dashboard;
    }
    private OnSaveDashboard(dashboard: ReactDashboardPM) {        
        var savedEntity: DashboardPM = this.GetDashboardEntity(dashboard);

        this.dashboardPMService.update(savedEntity).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                //this.EntityPM = myResponse.Result;
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }


}

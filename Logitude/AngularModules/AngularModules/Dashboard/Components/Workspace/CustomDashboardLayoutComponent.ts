import { Component, ViewChild, ElementRef, AfterViewInit, ViewEncapsulation, Input } from '@angular/core';
import * as React from 'react';
import Dashboard from 'logitude-dashboard-library';
import * as ReactDOM from 'react-dom';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
//import { ReactDashboardPM } from 'logitude-dashboard-library/dist/types/Dashboard';
import { DashboardDataBinding } from 'logitude-dashboard-library/dist/types/DashboardDataBinding';
import { BehaviorSubject, forkJoin, Subject } from 'rxjs';
import { ReactWidgetPM } from 'logitude-dashboard-library/dist/types/widget';
import { DataPointSelection } from 'logitude-dashboard-library/dist/types/SeriesMeasure';
import { DashboardPM } from '../../../DashboardModule/EntityPMs/DashboardPM';
import { WidgetPM } from '../../../DashboardModule/EntityPMs/WidgetPM';
import { WidgetMeasurePM } from '../../../DashboardModule/EntityPMs/WidgetMeasurePM';
import { ReactWidgetMeasurePM } from 'logitude-dashboard-library/dist/types/ReactWidgetMeasurePM';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { DashboardMapping } from 'Dashboard/Services/DashboardMapping';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';
//import { ReactWidgetPM } from 'logitude-dashboard-library/dist/types/widget';
//import { DataPointSelection } from 'logitude-dashboard-library/dist/types/SeriesMeasure';

@Component({
    template:
        `
        <div class="new-dashboard" #reactDashboradContainer></div>`,

    styles:
        [`
        .new-dashboard {
            width: 100%;
            height: 100%;
            padding: 10px 0px 0px 0px;
        }`],
    selector: 'custom-layout',
    styleUrls: ['CustomDashboardComponent.css'],
    inputs: ['IsEditLayout', 'SelectedDashboard'],
    encapsulation: ViewEncapsulation.ShadowDom
})

export class CustomDashboardLayoutComponent implements AfterViewInit {   
    @ViewChild('reactDashboradContainer') reactDashboradContainer: ElementRef;    
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    _show: boolean = false;
    @Input('Show') set Show(value) {
        console.log('CustomDashboardLayoutComponent show= ', value);
        if (value && !this._show) {
            this.ShowDashboard();
        }
        this._show = value;

    }
    get Show() {
        return this._show;
    }

    

    private isEditLayout: boolean;
    get IsEditLayout() { return this.isEditLayout; }
    set IsEditLayout(value: boolean) {
        if (this.isEditLayout != value) {
            this.isEditLayout = value;

            this.DashboardDataBinding?.isOnEditLayout.next(value);
        }
    }

    private selectedDashboard: DashboardPM;
    get SelectedDashboard() { return this.selectedDashboard; }
    set SelectedDashboard(value: DashboardPM) {
        if (this.selectedDashboard != value) {
            this.selectedDashboard = value;
        }
    }

    private _DashboardDataBinding: DashboardDataBinding;
    @Input('DashboardDataBinding') set DashboardDataBinding(value: DashboardDataBinding) {
        if (this._DashboardDataBinding != value) {
            this._DashboardDataBinding = value;
        }
    }
    get DashboardDataBinding() { return this._DashboardDataBinding; }
    ShowDashboard() {
        this.renderNewDashboard();
    }

    ngAfterViewInit(): void {
        this.renderNewDashboard();
    }

    onChangeLayouts(layouts: { lg: ReactWidgetPM[]; }) {
        layouts.lg.forEach(item => {
            var myWidget: WidgetPM = this.SelectedDashboard.Widgets.filter(d => d.Id == item.Id)[0];
            if (myWidget) {
                myWidget.EndPosition = item.EndPosition;
                myWidget.StartPotistion = item.StartPotistion;
            }
        });

        this.CurrentSession.FireEvent("WidgetEdited");
    }

    openEditWidget(widget: ReactWidgetPM){
        MixPanelLocator.PostDashboardAction({ ActionName: "Open Widget edit page" });
        var myWidget: WidgetPM = this.SelectedDashboard.Widgets.filter(d => d.Id == widget.Id)[0];
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = "Edit Widget";
        logitudeWindow.WindowArgs = { EntityPM: myWidget, IsNew: false, DashboardPM: this.SelectedDashboard };
        logitudeWindow.Show('./Dashboard/Components/Windows/AddEditWidgetComponent');
        logitudeWindow.ComponentLoaded.subscribe(comp => {
            logitudeWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.DashboardDataBinding.widgetUpdated.next(DashboardMapping.GetReactWidget(myWidget));
                    this.CurrentSession.FireEvent("WidgetEdited");
                }
            });
        });
    }

    private onSelectDataPoint(dataPointSelection: DataPointSelection){
        if(!dataPointSelection) return;
        SessionLocator.DynamicLoader.Load('./DashboardModule/Components/Workspace/DashboardLists/DashboardListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        .then((cmpRef : any) => {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(dataPointSelection);
        });
    }

    renderNewDashboard() {
        ReactDOM.render(React.createElement(Dashboard, {
            token: SessionInfo.Token,
            tenant: SessionInfo.LoggedUserTenant,
            userId: SessionInfo.LoggedUserId,
            dataBinding: this.DashboardDataBinding,
            onChangeLayouts: this.onChangeLayouts.bind(this),
            openEditWidget: this.openEditWidget.bind(this),
            onSelectDataPoint: this.onSelectDataPoint.bind(this),

        }),
            this.reactDashboradContainer.nativeElement);
    }
}

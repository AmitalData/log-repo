import { Component, ViewChild, ElementRef, AfterViewInit, ViewEncapsulation, Input, Output,EventEmitter } from '@angular/core';
import * as React from 'react';
import Dashboard from 'logitude-dashboard-library';
import * as ReactDOM from 'react-dom';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { DashboardDataBinding } from 'logitude-dashboard-library/dist/types/DashboardDataBinding';
import { ReactWidgetPM } from 'logitude-dashboard-library/dist/types/widget';
import { DataPointSelection } from 'logitude-dashboard-library/dist/types/SeriesMeasure';
import { DashboardPM } from '../../../../DashboardModule/EntityPMs/DashboardPM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { OnDestroy } from '@angular/core';

@Component({
    template:
        `
        <div class="new-dashboard" #reactDashboradContainer></div>`,

    styles:
        [`
        .new-dashboard {
            width: 100%;
            height: calc(100% - 50px);
            padding: 0px 0px 0px 0px;
        }`],
    selector: 'custom-layout',
    inputs: ['IsEditLayout', 'SelectedDashboard']
})

export class CustomDashboardLayoutComponent implements AfterViewInit, OnDestroy {
    @ViewChild('reactDashboradContainer') reactDashboradContainer: ElementRef;    
    private CurrentSession = SessionLocator.SelectedSession;
    @Output() openEditWidget: EventEmitter<ReactWidgetPM> = new EventEmitter<ReactWidgetPM>();
    @Output() onReactChangeLayouts: EventEmitter<{ lg: ReactWidgetPM[]; }> = new EventEmitter<{ lg: ReactWidgetPM[]; }>();

    constructor() {

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

    ngAfterViewInit(): void {
        this.renderNewDashboard();
    }

    ngOnDestroy() {
        ReactDOM.unmountComponentAtNode(this.reactDashboradContainer.nativeElement);
    }

    onChangeLayouts(layouts: { lg: ReactWidgetPM[]; }) {
        this.onReactChangeLayouts.emit(layouts);
    }
    

    private onSelectDataPoint(dataPointSelection: DataPointSelection){
        if(!dataPointSelection || this.IsEditLayout) return;
        SessionLocator.DynamicLoader.Load('./DashboardModule/Components/Workspace/DashboardLists/DashboardListComponent', this.CurrentSession.SessionLocation.viewContainerRef)
        .then((cmpRef : any) => {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Dashboard = this.SelectedDashboard;
            cmpRef.instance.Run(dataPointSelection);
        });
    }
    private openReactEditWidget(Widget: ReactWidgetPM){
        this.openEditWidget.emit(Widget)
    }

    renderNewDashboard() {
        const props: any = {
            token: SessionInfo.Token,
            tenant: SessionInfo.LoggedUserTenant,
            userId: SessionInfo.LoggedUserId,
            dataBinding: this.DashboardDataBinding,
            onChangeLayouts: this.onChangeLayouts.bind(this),
            openEditWidget: this.openReactEditWidget.bind(this),
            onSelectDataPoint: this.onSelectDataPoint.bind(this),
        };

        ReactDOM.render(React.createElement(Dashboard, props),this.reactDashboradContainer.nativeElement);
    }
}

import { Component, ViewChild, ElementRef, AfterViewInit, ViewEncapsulation } from '@angular/core';
import * as React from 'react';
import Dashboard from 'logitude-dashboard-library';
import * as ReactDOM from 'react-dom';
import { SessionInfo } from 'Infrastructure/Utilities/SessionInfo';
import { ReactDashboardPM } from 'logitude-dashboard-library/dist/types/Dashboard';
import { DashboardDataBinding } from 'logitude-dashboard-library/dist/types/DashboardDataBinding';
import { BehaviorSubject, forkJoin } from 'rxjs';
import { ReactWidgetPM } from 'logitude-dashboard-library/dist/types/widget';
import { DataPointSelection } from 'logitude-dashboard-library/dist/types/SeriesMeasure';

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
    selector: 'Custom-Layout',
    styleUrls: ['CustomDashboardComponent.css'],
    encapsulation: ViewEncapsulation.ShadowDom
})

export class CustomDashboardLayoutComponent implements AfterViewInit {
    @ViewChild('reactDashboradContainer') reactDashboradContainer: ElementRef;

    constructor() {

    }

    ngAfterViewInit(): void {
        this.renderNewDashboard();
    }

    renderNewDashboard() {
        ReactDOM.render(React.createElement(Dashboard, {
            token: SessionInfo.Token,
            tenant: SessionInfo.LoggedUserTenant,
            userId: SessionInfo.LoggedUserId,
            dataBinding: this.dashboardDataBinding,
            openAddEditWidget: (widget: ReactWidgetPM | undefined) => { },
            openAddEditDashboard: (dashboard: ReactDashboardPM | undefined) => { },
            onChangeDashboard: (dashboard: ReactDashboardPM | undefined) => { },
            onSaveDashboard: (dashboard: ReactDashboardPM | undefined) => { },
            onSelectDataPoint: (dataPointSelection: DataPointSelection) => { },
        }),
            this.reactDashboradContainer.nativeElement);
    }

    private dashboardDataBinding: DashboardDataBinding =
        {
            onGetAllDashboards: new BehaviorSubject<ReactDashboardPM[]>([]),
            onGetDashboard: new BehaviorSubject<ReactDashboardPM>({} as ReactDashboardPM),
            onAddUpdateWidget: new BehaviorSubject<boolean>(false),
        };
}

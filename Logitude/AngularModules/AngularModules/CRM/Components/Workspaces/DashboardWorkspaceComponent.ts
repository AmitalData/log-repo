import {Component, ViewChildren, QueryList, AfterViewInit} from '@angular/core';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';


@Component({

    templateUrl: './DashboardWorkspaceComponent.html',
})

export class DashboardWorkspaceComponent extends BaseComponent implements AfterViewInit {

    public HasQuoteDashboardFeature = false;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    constructor() {
        super();

        this.selectedTabCode = "BCD";

        if (FeatureLocator.HasFeaturePermession("General", "QPDB")) {
            this.HasQuoteDashboardFeature = true;
        }
    }

    ngAfterViewInit() {
        this.SelectionChanged();
    }

    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }
    private PageChild_BCD: any = null;
    private PageChild_INP: any = null;
    private PageChild_COP: any = null;
    private PageChild_QOT: any = null;

    SelectionChanged() {
        if (this.SelectedTabCode != null) {

            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {

                switch (this.SelectedTabCode) {

                    case "BCD": {
                        if (this.PageChild_BCD == null) {
                            SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/DashboardTabComponents/ByCreateDateComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_BCD = cmpRef.instance;
                                    this.PageChild_BCD.InitTab(this);
                                });
                        }

                        else {
                            this.PageChild_BCD.RefreshTab();
                        }

                        break;
                    }

                    case "INP": {
                        if (this.PageChild_INP == null) {
                            SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/DashboardTabComponents/ByInProgressComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_INP = cmpRef.instance;
                                    this.PageChild_INP.InitTab(this);
                                });
                        }

                        else {
                            this.PageChild_INP.RefreshTab();
                        }

                        break;
                    }

                    case "COP": {
                        if (this.PageChild_COP == null) {
                            SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/DashboardTabComponents/CompanyPerformanceComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_COP = cmpRef.instance;
                                    this.PageChild_COP.InitTab(this);
                                });
                        }

                        else {
                            this.PageChild_COP.RefreshTab();
                        }

                        break;
                    }

                    case "QOT": {
                        if (this.PageChild_QOT == null) {
                            SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/QuoteDashboardTabComponent/QuoteDashboardComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_QOT = cmpRef.instance;
                                    this.PageChild_QOT.InitTab(this);
                                });
                        }

                        else {
                            this.PageChild_QOT.RefreshTab();
                        }

                        break;
                    }
                }
            }
        }
    }
}

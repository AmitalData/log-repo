import { Component, ViewChildren, QueryList, AfterViewInit} from '@angular/core';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';


@Component({

    templateUrl: './TicketDashboardComponent.html',
})

export class TicketDashboardComponent extends BaseComponent implements AfterViewInit {


    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;

    constructor() {
        super();
        this.selectedTabCode = "BOT";
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
    private PageChild_BOT: any = null;
    private PageChild_BFR: any = null;

    SelectionChanged() {
        if (this.SelectedTabCode != null) {

            let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
            if (myLocation != null) {

                switch (this.SelectedTabCode) {

                    case "BOT": {
                        if (this.PageChild_BOT == null) {
                            SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/TicketDashboardTabComponents/ByOpenedTicketComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_BOT = cmpRef.instance;
                                    this.PageChild_BOT.InitTab(this);
                                });
                        }

                        else {
                            this.PageChild_BOT.RefreshTab();
                        }

                        break;
                    }

                    case "BFR": {
                        if (this.PageChild_BFR == null) {
                            SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/TicketDashboardTabComponents/ByFirstResolveTicketComponent', myLocation.viewContainerRef)
                                .then(cmpRef => {
                                    this.PageChild_BFR = cmpRef.instance;
                                    this.PageChild_BFR.InitTab(this);
                                });
                        }

                        else {
                            this.PageChild_BFR.RefreshTab();
                        }

                        break;
                    }





                }
            }
        }

    }
}

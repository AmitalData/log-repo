import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {Component, OnInit, QueryList, ViewChildren}  from '@angular/core';

@Component({
    moduleId: module.id,
    templateUrl: './MainReportSchedulerComponent.html',
})

export class MainReportSchedulerComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private PageChild_RTASK: any = null;
    
    constructor() {
    }

    ngOnInit() {
    }

    ngAfterViewInit() {
        this.RunComponent();
    }

    RunComponent() {
        if (this.AllLocations) {

            if (this.AllLocations.length != 0) {
                var tabCode = "RTASK";

                this.SetSelectedItem(tabCode);
            }
        }
    }

    SetSelectedItem(tabCode: string) {
        this.SelectedTabCode = tabCode;

    }


    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }

    SelectionChanged() {
        let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.SelectedTabCode)[0];
        if (myLocation != null) {

            switch (this.SelectedTabCode) {
                //Report Task
                case "RTASK": {
                    if (this.PageChild_RTASK == null) {
                        SessionLocator.DynamicLoader.Load('./Report/Components/Scheduler/TaskReportSchedulerComponent', myLocation.viewContainerRef)
                            .then(cmpRef => {
                                this.PageChild_RTASK = cmpRef.instance;
                            });
                    }

                    break;
                }
            }
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    NextButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}

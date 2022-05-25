import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {Component, OnInit, QueryList, ViewChildren, ViewChild, ViewContainerRef}  from '@angular/core';
import { ReportGroupList } from '../../EntityLists/ReportGroupList';
import { ReportList } from '../../EntityLists/ReportList';

@Component({
    
    templateUrl: './MainReportSchedulerComponent.html',
})

export class MainReportSchedulerComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    private PageChild_RETASK: any = null;
    public ShowPreviewReport: boolean = false;
    public ReportGroupList: ReportGroupList;
    public ReportList: ReportList;
    public BIReportEntity: any;
    constructor() {
    }

    ngOnInit() {
    }

    SetWindowArgs(windowArgs) {
        this.ReportGroupList = windowArgs.ReportGroupList;
        this.ReportList = windowArgs.ReportList;
        this.BIReportEntity = windowArgs.BIReportEntity;
        this.RunComponent();
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    RunComponent() {
        if (this.AllLocations) {
            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }
            else {
                var tabCode = "RETASK";
                this.SetSelectedItem(tabCode);
            }
        }
        else this.RunComponentTimer();
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
                case "RETASK": {
                    if (this.PageChild_RETASK == null) {
                        SessionLocator.DynamicLoader.Load('./Report/Components/Scheduler/TaskReportSchedulerComponent', myLocation.viewContainerRef)
                            .then(cmpRef => {
                                this.PageChild_RETASK = cmpRef.instance;
                                this.PageChild_RETASK.SetWindowArgs({ ReportGroupList: this.ReportGroupList, ReportList: this.ReportList, BIReportEntity: this.BIReportEntity });
                            });
                    }
                    break;
                }
            }
        }
    }

    CloseButtonClicked() {
        this.PageChild_RETASK.IsEditReportSchedulerEventAlreadyExist = true; //To avoid multiple events of edit report schedule.
        this.CurrentSession.CloseCurrentWindow();
    }
}

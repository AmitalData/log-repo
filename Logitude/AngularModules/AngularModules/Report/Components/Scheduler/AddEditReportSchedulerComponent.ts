import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LocationDirective } from '../../../Infrastructure/Utilities/LocationDirective';
import { Component, OnInit, QueryList, ViewChildren, ViewContainerRef, ViewChild } from '@angular/core';
import { TaskReportSchedulerItemClass } from '../../../Report/Components/Scheduler/TaskReportSchedulerComponent';
import { TasksSchedulerPM } from '../../../Infrastructure/EntityPMs/TasksSchedulerPM';
import { ReportGroupList } from '../../EntityLists/ReportGroupList';
import { ReportList } from '../../EntityLists/ReportList';
import { ReportsTemplateListExtendedService } from '../../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
@Component({
    moduleId: module.id,
    templateUrl: './AddEditReportSchedulerComponent.html',
})

export class AddEditReportSchedulerComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    @ViewChild('CustomerChild', { read: ViewContainerRef }) customerViewContainerRef: ViewContainerRef;
    private PageChild_RETASK: any = null;
    private PageChild_PRREP: any = null;
    public IsNextButtonClicked: boolean = false;
    public ReportGroupList: ReportGroupList;
    public ReportList: ReportList;
    reportsTemplateListExtendedService: ReportsTemplateListExtendedService;
    constructor() {
        this.reportsTemplateListExtendedService = new ReportsTemplateListExtendedService();
    }

    ngOnInit() {
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

    DataContext;
    SetDataContext(DataContext: TaskReportSchedulerItemClass) {
        this.DataContext = DataContext;
        this.DataContext.EntityPM = DataContext.EntityPM;
    }

    SetWindowArgs(windowArgs) {
        this.ReportGroupList = windowArgs.ReportGroupList;
        this.ReportList = windowArgs.ReportList;
        this.RunComponent();
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
        else {
            this.RunComponentTimer();
        }
    }

    SetSelectedItem(tabCode: string) {
        this.SelectedTabCode = tabCode;

    }
    ReportTemplates:any = [];
    LoadReportTemplate(reportList: ReportList) {
        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(reportList.Id, "R").subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                this.ReportTemplates = myResponse.Result;

            }

        });
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
                        SessionLocator.DynamicLoader.Load('./Report/Components/Scheduler/AddEditReportTaskSchedulerComponent', myLocation.viewContainerRef)
                            .then(cmpRef => {
                                this.PageChild_RETASK = cmpRef.instance;
                                this.PageChild_RETASK.SetDataContext({ DataContext: this.DataContext });
                            });
                    }

                    break;
                }
                //Preview Report
                case "PRREP": {
                    if (this.PageChild_PRREP == null) {
                        this.LoadReportTemplate(this.ReportList);
                        SessionLocator.DynamicLoader.Load('./Report/Components/ReportsPreviewComponent', myLocation.viewContainerRef)
                            .then(cmpRef => {
                                this.PageChild_PRREP = cmpRef.instance;
                                this.PageChild_PRREP.PreviewSchedulerReport();
                                this.PageChild_PRREP.ReportsPreview(this.ReportGroupList, this.ReportList, this.ReportTemplates);
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
        if (this.PageChild_RETASK.NextButtonClicked()) {
            this.IsNextButtonClicked = true;
            this.SetSelectedItem("PRREP");
        }
    }

    SaveButtonClicked() {
        this.PageChild_RETASK.SaveButtonClicked();
        this.CurrentSession.CloseCurrentWindow();
    }

    BackButtonClicked() {
        this.IsNextButtonClicked = false;
        this.SetSelectedItem("RETASK");
    }
}

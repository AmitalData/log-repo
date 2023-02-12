import { Component, EventEmitter, Output, QueryList, ViewChild, ViewChildren, ViewContainerRef } from '@angular/core';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { AppTool } from 'Infrastructure/Tools';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { ListComponentArgs } from 'Infrastructure/Args';

@Component({
    templateUrl: './RunHistoryWorkflowComponent.html',
})

export class RunHistoryWorkflowComponent extends BaseComponent {
    public EntityPM: WorkFlowPM;
    public ObjectTableName: string = "WorkFlowInstance";

    private timerToken: any;
    private Retries: number = 0;

    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    @ViewChild("WFInstanceContainer", { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    public filterAgrs: ApiQueryFilters;
    private _entityResourceService: EntityResourceService = new EntityResourceService();

    private TabSelectedEvent: any = null;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();
    }

    ngOnInit() {
        this.RunComponent();
    }

    RunComponent() {
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.AllLocations) {

            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }

            else {
                let locs = this.AllLocations.toArray().filter(f => f.Code == 'WFInstanceContainer');
                let myLocation: LocationDirective = locs[0];
                this.CurrentSession.SessionWorkflowInstanceLocation = myLocation;

                this.loadComponentList();
            }
        }

        else {
            this.RunComponentTimer();
        }
    }

    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    private loadComponentList() {
        this.filterAgrs = new ApiQueryFilters();

        var versionsIdList = this.EntityPM.WorkFlowVersions.map(v => v.Id);
        this.filterAgrs.addAdditionalFilter("WorkFlowVersionId", versionsIdList.join(','), null, null, "InListExact", false, false, false, "Text");

        var listArgs = new ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = "All WorkFlow Instance";
        listArgs.ObjectTableName = "WorkFlowInstance";
        listArgs.DisplayTitle = "Workflow Instance";
        listArgs.HideBackButton = true;

        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, this.EntityPM.Tenant).subscribe((response: any) => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionWorkflowInstanceLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);

                    cmpRef.instance.BackCompleted.subscribe(event => {
                        this.onRowSelected(event)
                    });
                });
        });
        this.CurrentSession.StopBusyIndicator();
    }


    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "WFRH") {
                    this.CurrentSession.FireEvent("ReloadAllList");
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }

    onRowSelected(event: any) {
        if (event !== null && event.rowData !== null && (event.rowData.StatusCode === "COED" || event.rowData.StatusCode === "FAED")) {
            let isVariableHasPermission = FeatureLocator.HasFeaturePermession("WorkFlowInstance", "WorkFlowInstance.ShowVariables");
            let logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = isVariableHasPermission ? 690 : 570;
            logWindow.Title = "Instance Activities" + (isVariableHasPermission ? " And Variables" : '');
            logWindow.IsShowCloseButton = true
            let windowArgs: any = {};
            windowArgs.EntityId = event.rowData.Id;
            windowArgs.ObjectTableName = "WorkFlowInstanceActivity";
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./Workflow/Components/CreateEditWorkflow/WorkflowInstanceDetailsComponent');
        }
    }
}
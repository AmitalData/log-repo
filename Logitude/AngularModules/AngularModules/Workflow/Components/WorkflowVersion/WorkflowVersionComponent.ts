import { Component, QueryList, ViewChild, ViewChildren, ViewContainerRef } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters, FilterItem } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { AppTool } from 'Infrastructure/Tools';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { WorkFlowPM } from 'Workflow/EntityPMs/WorkFlowPM';
import { ListComponentArgs } from 'Infrastructure/Args';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';

@Component({
    templateUrl: './WorkflowVersionComponent.html',
})

export class WorkflowVersionComponent extends BaseComponent {
    public EntityPM: WorkFlowPM;
    public ObjectTableName: string = "WorkFlowVersion";

    private timerToken: any;
    private Retries: number = 0;

    private CurrentSession = SessionLocator.SelectedSession;
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    @ViewChild("WFVersionContainer", { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
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
        if (this.AllLocations) {

            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }

            else {
                let locs = this.AllLocations.toArray().filter(f => f.Code == 'WFVersionContainer');
                let myLocation: LocationDirective = locs[0];
                this.CurrentSession.SessionWorkflowVersionLocation = myLocation;

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
        let filterItem = new FilterItem("WorkflowId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");

        var listArgs = new ListComponentArgs();
        listArgs.DefaultFilterItems = [filterItem];
        listArgs.QueryCode = "All Workflow Versions";
        listArgs.ObjectTableName = "WorkFlowVersion";
        listArgs.DisplayTitle = "Workflow Versions";
        listArgs.HideBackButton = true;

        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, this.EntityPM.Tenant).subscribe((response: any) => {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionWorkflowVersionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.RowClicked.subscribe((event: any) => {
                        this.onRowSelected(event.rowData.Id)
                    });
                });
        });
    }

    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe((tabCode: string) => {
                if (tabCode == "WFVR") {
                    this.CurrentSession.FireEvent("ReloadAllList" + this.ObjectTableName);
                }
            });
        }

        if (this.entityArgs.EditComponent) {
            this.entityArgs.EntityArgEventEmitter.subscribe(
                theMessage => {
                    if (theMessage == "WorkflowVersionsUpdated") {
                        this.CurrentSession.FireEvent("ReloadAllList" + this.ObjectTableName);
                    }
                }
            );
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.TabSelectedEvent);
    }

    onRowSelected(id: string) {
        this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, UpdatedVersion: null }
        this.entityArgs.EditComponentArgument = { ...this.entityArgs.EditComponentArgument, ClickedVersionRow: id }
        this.CurrentSession.CurrentEditComponent.SetSelectedTabByCode("WFFB");
    }
}
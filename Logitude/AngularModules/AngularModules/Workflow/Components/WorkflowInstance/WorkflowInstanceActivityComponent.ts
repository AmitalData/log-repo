import { Component } from '@angular/core';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { WorkFlowInstanceActivityList } from 'Workflow/EntityLists/WorkFlowInstanceActivityList';
import { ApiQueryFiltersBuilder } from 'Workflow/Utilities/ApiQueryFiltersBuilder';
import { WorkFlowInstanceActivityListService } from 'Workflow/Services/StandardLists/WorkFlowInstanceActivityListService';
import { WorkFlowInstanceVariableListService } from 'Workflow/Services/StandardLists/WorkFlowInstanceVariableListService';

@Component({
    templateUrl: './WorkflowInstanceActivityComponent.html',
})

export class WorkflowInstanceActivityComponent extends BaseComponent {
    public EntityId: string;
    public IsActivityLoading: boolean = false;
    public IsVariableLoading: boolean = false;
    public IsVariablesHasPermission: boolean = false;
    public ObjectTableName: string = "WorkFlowInstanceActivity";
    public ActivityItemsSource: ObservableCollection;
    public VariableItemsSource: ObservableCollection;
    public WorkFlowInstanceActivityListService = new WorkFlowInstanceActivityListService()
    public WorkFlowInstanceVariableListService = new WorkFlowInstanceVariableListService()

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ActivityItemsSource = new ObservableCollection([]);
        this.VariableItemsSource = new ObservableCollection([]);
        this.IsVariablesHasPermission = FeatureLocator.HasFeaturePermession("WorkFlowInstance", "WorkFlowInstance.ShowVariables")
    }

    SetWindowArgs(args: any) {
        this.EntityId = args.EntityId;
        this.ObjectTableName = args.ObjectTableName;
    }

    ngOnInit() {
        this.IsActivityLoading = true
        this.LoadActivityData()
        this.LoadVariables()
    }

    RefreshButtonClicked() {
        this.IsActivityLoading = true
        this.LoadActivityData()
        this.LoadVariables()
    }

    LoadVariables(){
        if(this.IsVariablesHasPermission){
            this.IsVariableLoading = true
            this.LoadVariablesData()
        }
    }

    LoadActivityData() {
        let filters = ApiQueryFiltersBuilder.getWorkflowInstanceActivities(this.EntityId, true);
        
        this.WorkFlowInstanceActivityListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var result = myResponse.Result;
                    this.ActivityItemsSource.InsertCollection(result);
                    this.IsActivityLoading = false
                }
            }
        });
    }

    LoadVariablesData() {
        let filters = ApiQueryFiltersBuilder.getWorkflowInstanceActivities(this.EntityId, true);
        
        this.WorkFlowInstanceVariableListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var result = myResponse.Result;
                    this.VariableItemsSource.InsertCollection(result);
                    this.IsVariableLoading = false
                }
            }
        });
    }

    OnRowLoaded(myRow: any) {
        if (myRow) {
            var isExpandaple = false;

            var item: WorkFlowInstanceActivityList = myRow.rowData;
            if (item.StatusCode == 'FAED') {
                isExpandaple = true;
            }

            myRow.SetExpandaple(isExpandaple);
        }
    }

    DisplayValueObject(item) {
        if (item != null) {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height =  400;
            logWindow.Title = item.Type + " Value Body";
            logWindow.IsShowCloseButton = true
            var windowArgs: any = {};
            windowArgs.Value = item.Value;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./Workflow/Components/CreateEditWorkflow/WorkflowInstanceVariableObject');
        }
    }
}
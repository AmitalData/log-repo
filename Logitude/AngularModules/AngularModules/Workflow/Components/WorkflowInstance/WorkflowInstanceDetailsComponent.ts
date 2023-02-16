import { Component, OnInit } from '@angular/core';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { FeatureLocator } from 'Infrastructure/Utilities/FeatureLocator';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { WorkFlowInstanceActivityList } from 'Workflow/EntityLists/WorkFlowInstanceActivityList';
import { WorkflowInstanceExtendedService } from 'Workflow/Services/Extended/WorkflowInstanceExtendedService';
import { BlobDownloader } from 'Workflow/Utilities/BlobDownloader';

@Component({
    templateUrl: './WorkflowInstanceDetailsComponent.html',
})

export class WorkflowInstanceDetailsComponent extends BaseComponent implements OnInit {
    public EntityId: string;
    public IsActivitiesLoading: boolean = false;
    public IsVariablesLoading: boolean = false;
    public IsVariablesHasPermission: boolean = false;
    public ObjectTableName: string = "WorkFlowInstanceActivity";
    public ActivityItemsSource: ObservableCollection;
    public VariableItemsSource: ObservableCollection;
    public WorkflowInstanceExtendedService = new WorkflowInstanceExtendedService();

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ActivityItemsSource = new ObservableCollection([]);
        this.VariableItemsSource = new ObservableCollection([]);
        this.IsVariablesHasPermission = FeatureLocator.HasFeaturePermession("WorkFlowInstance", "WorkFlowInstance.ShowVariables");
    }

    SetWindowArgs(args: any) {
        this.EntityId = args.EntityId;
        this.ObjectTableName = args.ObjectTableName;
    }

    ngOnInit() {
        this.Load();
    }

    RefreshButtonClicked() {
        this.Load();
    }

    Load() {
        this.LoadActivities();
        this.LoadVariables();
    }

    LoadActivities() {
        this.IsActivitiesLoading = true;
        this.WorkflowInstanceExtendedService.getActivities(this.EntityId).subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                this.ActivityItemsSource.InsertCollection(serviceResponse.Result);
            }
            this.IsActivitiesLoading = false;
        });
    }

    LoadVariables() {
        if (this.IsVariablesHasPermission) {
            this.IsVariablesLoading = true;
            this.WorkflowInstanceExtendedService.getVariables(this.EntityId).subscribe((serviceResponse: ServiceResponse) => {
                if (!serviceResponse.HasError) {
                    this.VariableItemsSource.InsertCollection(serviceResponse.Result);
                }
                this.IsVariablesLoading = false;
            });
        }
    }

    OnRowLoaded(row: any) {
        if (row) {
            let isExpandaple = false;
            let item: WorkFlowInstanceActivityList = row.rowData;
            if (item.StatusCode == 'FAED') {
                isExpandaple = true;
            }
            row.SetExpandaple(isExpandaple);
        }
    }

    DisplayObjectValue(item: any) {
        if (item !== null) {
            let logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 400;
            logWindow.Title = item.Type + " Value Body";
            logWindow.IsShowCloseButton = true
            let windowArgs: any = {};
            windowArgs.Value = item.Value;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./Workflow/Components/CreateEditWorkflow/ObjectVariableComponent');
        }
    }

    DownloadDetails() {
        let details = {
            Id: this.EntityId,
            Activities: this.ActivityItemsSource.Collection,
            Variables: this.VariableItemsSource.Collection
        };
        BlobDownloader.downloadJson(details, this.EntityId);
    }
}
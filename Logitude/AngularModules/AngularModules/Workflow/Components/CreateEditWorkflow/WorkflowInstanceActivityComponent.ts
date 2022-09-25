import { Component } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { ObservableCollection } from 'Infrastructure/Utilities/ObservableCollection';
import { WorkFlowInstanceActivityList } from 'Workflow/EntityLists/WorkFlowInstanceActivityList';
import { WorkFlowInstanceActivityListService } from 'Workflow/Services/StandardLists/WorkFlowInstanceActivityListService';

@Component({
    templateUrl: './WorkflowInstanceActivityComponent.html',
})

export class WorkflowInstanceActivityComponent extends BaseComponent {
    public EntityId: string;
    public IsLoading: boolean = false;
    public ObjectTableName: string = "WorkFlowInstanceActivity";
    public ItemsSource: ObservableCollection;
    public WorkFlowInstanceActivityListService = new WorkFlowInstanceActivityListService()

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.ItemsSource = new ObservableCollection([]);
    }

    SetWindowArgs(args: any) {
        this.EntityId = args.EntityId;
        this.ObjectTableName = args.ObjectTableName;
    }

    ngOnInit() {
        this.IsLoading = true
        this.LoadData()
    }

    RefreshButtonClicked() {
        this.IsLoading = true
        this.LoadData()
    }

    LoadData() {
        var filters = new ApiQueryFilters(true);
        filters.addAdditionalFilter("WorkflowInstanceId", this.EntityId, null, null, "Equals", false, false, false, "string");

        this.WorkFlowInstanceActivityListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var result = myResponse.Result;
                    this.ItemsSource.InsertCollection(result);
                    this.IsLoading = false
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
}
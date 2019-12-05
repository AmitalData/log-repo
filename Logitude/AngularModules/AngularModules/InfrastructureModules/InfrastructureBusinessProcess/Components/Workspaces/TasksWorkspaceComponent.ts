import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {QueueData, BusinessProcessDomainService} from '../../../../Infrastructure/Services/BusinessProcessDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ListComponentArgs} from '../../../../Infrastructure/Args';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../Infrastructure/Tools';
import {TeamPM} from '../../../../Infrastructure/EntityPMs/TeamPM';

@Component({
    moduleId: module.id,
    templateUrl: './TasksWorkspaceComponent.html',
})

export class TasksWorkspaceComponent {
    private businessProcessDomainService: BusinessProcessDomainService;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    private _entityResourceService: EntityResourceService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {         
        this.businessProcessDomainService = new BusinessProcessDomainService();
        this._entityResourceService = new EntityResourceService();

        this.GetTeamsForLoggedUser();
        this.InitListArgs();
        this.SelectedFilterValue = "My";     
        this.RunComponent();
    }

    private teamsIdsList: string;
    private GetTeamsForLoggedUser() {
        this.businessProcessDomainService.GetTeamsForLoggedUser(SessionLocator.LoggedUserId).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                this.teamsIdsList = myResponse.Result;
            }
        }); 
    }

    private filterAgrs: ApiQueryFilters;
    private listArgs: ListComponentArgs;
    private InitListArgs() {
        this.filterAgrs = new ApiQueryFilters();
        this.listArgs = new ListComponentArgs();
        
        this.listArgs.QueryCode = "All Activities";
        this.listArgs.ObjectTableName = "Activity";
        this.listArgs.IsTasksMenuClicked = true;
    }

    public QueuesItemsSource: QueueItem[] = [];
    public NoQueuesVisibility: boolean = false;
    private LoadQueues(myFilter: string) {    
        this.QueuesItemsSource = [];
            
        this.businessProcessDomainService.GetQueuesWithCounts(myFilter).subscribe(myResult => {
            var myResponse: ServiceResponse = myResult;
            if (!myResponse.HasError) {

                var myData: QueueData[] = myResponse.Result;
                if (myData != null) {
                    myData.forEach(item => {
                        this.QueuesItemsSource.push(new QueueItem(item, this));
                    });                    

                    if (this.QueuesItemsSource.length == 0) {
                        this.NoQueuesVisibility = true;
                    }

                    else {
                        this.selectedQueue = this.QueuesItemsSource[0];
                        this.SelectQueue(this.selectedQueue);
                    }
                }
            }
        });
    }

    private selectedQueue: QueueItem;
    get SelectedQueue() { return this.selectedQueue; }
    set SelectedQueue(value: QueueItem) {
        if (this.selectedQueue != value) {
            this.selectedQueue = value;
        }
    }
    
    SelectQueue(entity: any) {
        this.SelectedQueue = entity;

        this.filterAgrs = new ApiQueryFilters();
        this.filterAgrs.addAdditionalFilter("ActivityTypeCode", "TX", null, null, "Equals", false, false, false, "String");

        if (this.SelectedFilterValue == "My") {
            this.filterAgrs.addAdditionalFilter("OwnerId", SessionLocator.LoggedUserId, null, null, "Equals", false, false, false, "String");
        }

        else if (this.SelectedFilterValue == "Team") {
            if (!AppTool.IsNullOrEmpty(this.teamsIdsList)) {
                this.filterAgrs.addAdditionalFilter("TeamId", this.teamsIdsList, null, null, "InList", false, true, false, "string");
            }

            else {
                this.filterAgrs.addAdditionalFilter("TeamId", "XXX", null, null, "InList", false, true, false, "string");
            } 
        }

        if (!AppTool.IsNullOrEmpty(this.SelectedQueue.QueueId)) {
            this.filterAgrs.addAdditionalFilter("BusinessProcessQueueId", this.SelectedQueue.QueueId, null, null, "Equals", false, false, false, "String");
        }

        this.listArgs.Filters = this.filterAgrs;
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(this.listArgs);
                this.CurrentSession.AddMenuReference(cmpRef);
            });

    }    
    
    private selectedFilterValue: string = "";
    public get SelectedFilterValue() { return this.selectedFilterValue;}
    public set SelectedFilterValue(value: string) {
        if (this.selectedFilterValue != value) {
            this.selectedFilterValue = value;
            
            this.LoadQueues(value);
        }
    }

    FilterItemClicked(myArgs: string) {
        this.SelectedFilterValue = myArgs;
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
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

    LoadChildComponent() {
        this.listArgs.Filters = this.filterAgrs;        
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(this.listArgs);
            });
    }
}

export class QueueItem {
    public MyEntity: QueueData;
    constructor(entity: QueueData, public fatherComponent: TasksWorkspaceComponent) {       
        this.MyEntity = entity;
    }

    get QueueId() { return this.MyEntity.QueueId; }
    get QueueName() { return this.MyEntity.QueueName; }
    get Count() { return this.MyEntity.Count; }
    get Title() { return this.MyEntity.QueueName + " (" + this.MyEntity.Count + ")"; }
}

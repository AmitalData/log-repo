import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {GlobalDomainService} from '../../../../Common/Services/GlobalDomainService';
import {InfrastructureDomainService} from '../../../../Infrastructure/Services/InfrastructureDomainService';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {CodeNameClass} from '../../../../Infrastructure/DataContracts/CodeNameClass';
import {BatchServicesDefinitionPM} from '../../../../Infrastructure/EntityPMs/BatchServicesDefinitionPM';
import {BatchServicesLogList} from '../../../../Infrastructure/EntityLists/BatchServicesLogList';

@Component({
    moduleId: module.id,
    templateUrl: './BatchServicesComponent.html',
})

export class BatchServicesComponent {
    public ItemsSource: BatchServiceItemClass[] = [];
    public LogsItemsSource: BatchServicesLogList[] = [];
    private loadedDataList: BatchServicesDefinitionPM[] = [];
    private globalDomainService: GlobalDomainService;  
    private infraDomainService: InfrastructureDomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.globalDomainService = new GlobalDomainService();
        this.infraDomainService = new InfrastructureDomainService();

        this.FillBatchFilterList();
        this.GetBatchServicesDefinitions();
    }
    
    public BatchFilterList: CodeNameClass[] = [];
    private FillBatchFilterList() {
        this.BatchFilterList = [];
        this.BatchFilterList.push(new CodeNameClass("L1D", "Last Day"));
        this.BatchFilterList.push(new CodeNameClass("L2D", "Last Two Days"));
        this.BatchFilterList.push(new CodeNameClass("L1H", "Last Hour"));
        this.BatchFilterList.push(new CodeNameClass("L2H", "Last Two Hours"));
        this.BatchFilterList.push(new CodeNameClass("L1Y", "Last Year"));

        this.selectedBatchFilter = this.BatchFilterList.filter(d => d.Code == "L1D")[0];
        this.GetBatchServicesDefinitions();
    }

    private selectedBatchFilter: CodeNameClass;
    get SelectedBatchFilter() { return this.selectedBatchFilter; }
    set SelectedBatchFilter(value: CodeNameClass) {
        if (this.selectedBatchFilter != value) {
            this.selectedBatchFilter = value;

            this.GetBatchServicesDefinitions();
        }
    }

    private filterTypeCode: string = "AL";
    public get FilterTypeCode() { return this.filterTypeCode; }
    public set FilterTypeCode(value: string) {
        if (this.filterTypeCode != value) {
            this.filterTypeCode = value;

            this.IsLogsGridVsisible = false;
            this.BuildItemsSource();
        }
    }

    public IsLogsGridVsisible = false;
    public SelectedRow: BatchServiceItemClass;
    Selecting(item: BatchServiceItemClass) {
        this.SelectedRow = item;

        if (item == null) {
            this.IsLogsGridVsisible = false;
        }

        else {
            this.LoadBatchServicesLogs();            
        }
    }

    private GetBatchServicesDefinitions() {
        this.CurrentSession.StartBusyIndicatorLoading();

        this.IsLogsGridVsisible = false;
        
        this.globalDomainService.GetAllBatchServicesDefinitionsPMs(this.SelectedBatchFilter.Code).subscribe(myResult => {
            if (myResult == null) {
                this.ItemsSource = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.loadedDataList = myResponse.Result;

                    this.BuildItemsSource();
                }
            }
        });
    }

    BuildItemsSource() {
        this.ItemsSource = [];

        var items: BatchServicesDefinitionPM[] = [];

        if (this.FilterTypeCode == "AC") {
            items = this.loadedDataList.filter(d => d.InActive == false);
        }

        else if (this.FilterTypeCode == "IN") {
            items = this.loadedDataList.filter(d => d.InActive == true);
        }

        else {
            items = this.loadedDataList;
        } 

        items.forEach(item => {
            this.ItemsSource.push(new BatchServiceItemClass(item));
        });

        this.CurrentSession.StopBusyIndicator();
    }

    private LoadBatchServicesLogs() {  
        this.CurrentSession.StartBusyIndicatorLoading();
              
        this.infraDomainService.GetBatchServicesLogs(this.SelectedRow.Code, this.SelectedBatchFilter.Code).subscribe(myResult => {
            if (myResult == null) {
                this.LogsItemsSource = [];
            }

            else {
                var myResponse: ServiceResponse = myResult;
                if (!myResponse.HasError) {
                    this.LogsItemsSource = myResponse.Result;
                    this.IsLogsGridVsisible = true;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }

    EditClicked(item: BatchServiceItemClass) {        
            var logWindow = new LogitudeWindow();
            logWindow.Title = "Edit Batch Service";
            logWindow.DataContext = item;
            logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/BatchService/EditBatchServiceComponent');
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.RefreshButtonClicked();
                }
            });        
    }

    RefreshButtonClicked() {
        this.GetBatchServicesDefinitions();
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

export class BatchServiceItemClass extends BaseComponent {
    public EntityPM: BatchServicesDefinitionPM;
    public ObjectTableName: string = "BatchServicesDefinition";
    constructor(item: BatchServicesDefinitionPM) {
        super();
        this.EntityPM = item;
    }

    get Code() { return this.EntityPM.Code; }
    get LastActivity() { return this.EntityPM.LastActivity; }
    get NumberOfDoneItems() { return this.EntityPM.NumberOfDoneItems; }
    get DoneItemsInOneMinute() { return this.EntityPM.DoneItemsInOneMinute; }
    get DoneItemsInFiveMinutes() { return this.EntityPM.DoneItemsInFiveMinutes; }
    get DoneItemsInOneHour() { return this.EntityPM.DoneItemsInOneHour; }
    get WaitingItems() { return this.EntityPM.WaitingItems; }
    get FailedItems() { return this.EntityPM.FailedItems; }

    get NumberOfThreads() { return this.EntityPM.NumberOfThreads; }
    set NumberOfThreads(newValue: number) {
        if (this.EntityPM.NumberOfThreads != newValue) {
            this.EntityPM.NumberOfThreads = newValue;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(newValue: boolean) {
        if (this.EntityPM.InActive != newValue) {
            this.EntityPM.InActive = newValue;
        }
    }
}

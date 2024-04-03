import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ShipmentDomainService, ShipmentsForAutomaticRequest } from '../../../../Shipment/Services/ShipmentDomainService';
import { BatchTaskExecutionListService } from '../../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { BatchTaskExecutionList } from '../../../../Infrastructure/EntityLists/BatchTaskExecutionList';

@Component({
    templateUrl: './VizionAutomaticRequestComponent.html',
})

export class VizionAutomaticRequestComponent extends BaseComponent implements OnInit {    
    public DataContext: VizionAutomaticRequestComponent = this;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public ShipmentsItemsSource: ShipmentsForAutomaticRequest[];
    public LogsItemsSource: ShipmentsForAutomaticRequest[];
    private vizionService: ShipmentDomainService;
    constructor() {
        super();
        this.ShipmentsItemsSource = [];
        this.LogsItemsSource = [];
        this.vizionService = new ShipmentDomainService();
    }

    ngOnInit() {
        this.LoadShipments();
    }

    private LoadShipments() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.vizionService.GetShipmentsForAutomaticRequest().subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                this.ShipmentsItemsSource = response.Result;
                this.BuildShipmentsNumbersText();
            }

            this.CurrentSession.StopBusyIndicator();
        });
    }

    private BuildShipmentsNumbersText() {
        var myText: string = "";
        this.ShipmentsItemsSource.forEach(item => {
            if (AppTool.IsNullOrEmpty(myText)) {
                myText = item.ShipmentNumber;
            }

            else {
                myText += ", " +  item.ShipmentNumber;
            }
        });

        this.ShipmentsNumbersText = myText;
    }

    private shipmentsNumbersText: string = "";
    get ShipmentsNumbersText() { return this.shipmentsNumbersText; }
    set ShipmentsNumbersText(value: string) {
        if (this.shipmentsNumbersText != value) {
            this.shipmentsNumbersText = value;
        }
    }

    private shipmentsNumbersRequestText: string = "";
    get ShipmentsNumbersRequestText() { return this.shipmentsNumbersRequestText; }
    set ShipmentsNumbersRequestText(value: string) {
        if (this.shipmentsNumbersRequestText != value) {
            this.shipmentsNumbersRequestText = value;
        }
    }

    private selectedTabCode: string = "REQ";
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
        }
    }    

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SendRequestClicked() {
        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.ShipmentsNumbersRequestText)) {
            errors.push("Request text box is empty");
        }

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("Sending...");
            this.vizionService.SendVizionAutomaticRequests(this.ShipmentsNumbersRequestText).subscribe((response: ServiceResponse) => {
                //if (!response.HasError) {
                //    this.LogsItemsSource = response.Result;
                //}

                if (!response.HasError) {
                    var batchTaskExecutionId: string = response.Result;

                    this.StopTimer();

                    this.timer = setInterval(() => {
                        this.CheckBatchTaskExecution(batchTaskExecutionId);
                    }, this.timerInterval);
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }
    }

    timer: any;
    timerInterval: number = 1000;
    StopTimer() {
        if (this.timer) {
            clearInterval(this.timer);
        }
    }

    CheckBatchTaskExecution(BatchTaskExecutionId: string) {
        var iBatchService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
        iBatchService.getSingle(BatchTaskExecutionId).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var list: BatchTaskExecutionList = myResponse.Result;

                if (list.StatusCode == "D") {
                    this.StopTimer();                    
                    this.CurrentSession.StopBusyIndicator();

                    this.LogsItemsSource = JSON.parse(list.PrametersXml);
                }

                else if (list.StatusCode == "F") {
                    this.StopTimer();
                    this.CurrentSession.StopBusyIndicator();

                    var errors: string[] = [];
                    errors.push(list.ErrorLog);
                    this.ValidationErrorsList = errors;
                }

                else {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.StartBusyIndicator("Sending...");
                }
            }

            else {
                this.StopTimer();
                this.CurrentSession.StopBusyIndicator();
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    }
}

import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ShipmentDomainService, ShipmentsForAutomaticRequest } from '../../../../Shipment/Services/ShipmentDomainService';

@Component({
    templateUrl: './VizionAutomaticRequestComponent.html',
})

export class VizionAutomaticRequestComponent extends BaseComponent implements OnInit {    
    public DataContext: VizionAutomaticRequestComponent = this;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public ShipmentsItemsSource: ShipmentsForAutomaticRequest[];
    private vizionService: ShipmentDomainService;
    constructor() {
        super();
        this.ShipmentsItemsSource = [];
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
                if (!response.HasError) {
                    
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }
    }
}

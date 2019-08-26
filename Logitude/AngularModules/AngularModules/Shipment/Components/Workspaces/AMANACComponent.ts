import { Component, OnInit} from '@angular/core';
import { SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ShipmentDomainService } from '../../../Shipment/Services/ShipmentDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './AMANACComponent.html',
})

export class AMANACComponent implements OnInit {
    private CurrentSession = SessionLocator.SelectedSession;
    private shipmentDomainService: ShipmentDomainService;
    constructor() {
        this.shipmentDomainService = new ShipmentDomainService();
        this.LoadDataCount();
    }

    ngOnInit() {

    }

    NewTransferClicked(args) {
        var logWindowTitle: string = null;
        var transferTypeCode: string = null;
        if (args == "Air") {
            logWindowTitle = "New Air Shipment AMANAC Transfer";
            transferTypeCode = "Air";
        }
        else {
            logWindowTitle = "New Ocean Shipment AMANAC Transfer";
            transferTypeCode = "Ocean";
        }

        var logWindow = new LogitudeWindow();
        logWindow.Width = 980;
        logWindow.Height = 570;
        logWindow.Title = logWindowTitle;
        logWindow.WindowArgs = transferTypeCode;
        logWindow.Show('./ShipmentModules/ShipmentAMANAC/Components/NewEntity/NewTransferComponent');
    }

    public BlockedOceanShipmentsCount: string = "0";
    public BlockedAirShipmentsCount: string = "0";
    LoadDataCount() {
        this.shipmentDomainService.GetShipmentsTransferSummary().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                if (myResponse.Result) {
                    this.BlockedOceanShipmentsCount = myResponse.Result.BlockedOceanShipmentsCount > 1000 ? "1000+" : myResponse.Result.BlockedOceanShipmentsCount + "";
                    this.BlockedAirShipmentsCount = myResponse.Result.BlockedAirShipmentsCount > 1000 ? "1000+" : myResponse.Result.BlockedAirShipmentsCount + "";
                }
            }
        });
    }

    ViewBlockedShipmentsClicked(transportMode: string) {
        var logWindowTitle: string;

        if (transportMode == "Air") {
            logWindowTitle = "Marked as Blocked Air Shipments";
        }
        else {
            logWindowTitle = "Marked as Blocked Ocean Shipments";
        }

        var logWindow = new LogitudeWindow();
        logWindow.Width = 980;
        logWindow.Height = 570;
        logWindow.Title = logWindowTitle;
        logWindow.WindowArgs = transportMode;
        logWindow.Show('./ShipmentModules/ShipmentAMANAC/Components/BlockedShipmentsComponent');
        logWindow.WindowClosed.subscribe(s => {
            this.LoadDataCount();
        });
    }
}

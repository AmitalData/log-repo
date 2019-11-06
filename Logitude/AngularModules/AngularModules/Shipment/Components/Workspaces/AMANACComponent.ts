import { Component, OnInit} from '@angular/core';
import { SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { ShipmentDomainService } from '../../../Shipment/Services/ShipmentDomainService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ListComponentArgs } from '../../../Infrastructure/Args';

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
            transferTypeCode = "AMAS";
        }
        else {
            logWindowTitle = "New Ocean Shipment AMANAC Transfer";
            transferTypeCode = "AMOS";
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

    ViewQueryClicked(args: string) {
        var filterAgrs = new ApiQueryFilters();
        filterAgrs.addAdditionalFilter("CustomsTransferTypeCode", args, null, null, "Equals", false, false, false, "String");
        
        var listArgs = new ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = "ShipmentsTransferHistory";
        listArgs.ObjectTableName = "CustomsTransferHeader";
        listArgs.BackButtonTitle = "Operations";
        listArgs.DisplayTitle = "Transfer History";

        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(cmpRef => {
                this.CurrentSession.AddMenuReference(cmpRef);

                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);

                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    this.LoadDataCount();
                });
            });
    }
}  


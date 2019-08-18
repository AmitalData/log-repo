import { Component, OnInit} from '@angular/core';
import { SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';


@Component({
    moduleId: module.id,
    templateUrl: './AMANACComponent.html',
})

export class AMANACComponent implements OnInit {

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

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

}

import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPM} from '../../../../Shipment/EntityPMs/ShipmentPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from  '../../../../Controls/Windows/LogitudeWindow';

@Component({
    selector: 'SentToCustomLinkComponent',
    moduleId: module.id,
    templateUrl: './SentToCustomLinkComponent.html',
})

export class SentToCustomLinkComponent extends BaseComponent {
    public EntityPM: ShipmentPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    SetWindowArgs(windowArgs: ShipmentPM) {
        this.EntityPM = windowArgs;
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    ViewCustomsSettings() {
        this.CurrentSession.CloseCurrentWindow();
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Customs Settings";
        logWindow.Show('./Common/Components/Maintenance/CustomsInterface/CustomsInterfaceSettingsComponent');
        logWindow.WindowClosed.subscribe(comp => {
            if (comp == "OK"){
                var logWindow = new LogitudeWindow();
                logWindow.Title = "Customs Transmissions";
                logWindow.Height = 600;
                logWindow.WindowArgs = this.EntityPM;
                logWindow.Show('./ShipmentModules/ShipmentOthers/Components/SentToCustomComponent/SentToCustomComponent');
            }
        });
    }
}

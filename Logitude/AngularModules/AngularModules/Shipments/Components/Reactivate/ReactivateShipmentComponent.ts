import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ShipmentPM} from '../../EntityPMs/ShipmentPM';
import {ShipmentPMService} from '../../Services/StandardPMs/ShipmentPMService';

@Component({
    
    templateUrl: './ReactivateShipmentComponent.html',
})

export class ReactivateShipmentComponent {
    public EntityPM: ShipmentPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetWindowArgs(entityPM: ShipmentPM) {
        this.EntityPM = entityPM;        
    }

    private notes: string;
    get Notes() { return this.notes; }
    set Notes(newValue: string) {
        this.notes = newValue;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();

        this.EntityPM.EventNote = this.Notes;
        this.EntityPM.IsCancelled = false;

        // save
        var myService: ShipmentPMService = new ShipmentPMService();

        myService.update(this.EntityPM).subscribe((myResult:any) => {
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindowEmit('OK');
        });        
    }
}

import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ApproveBuildShipmentComponent.html',
})

export class ApproveBuildShipmentComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetWindowArgs() {
        
    }

    public Approving: boolean = false;

    CancelButtonClicked() {
        this.Approving = false;
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    }

    OkButtonClicked() {
        this.Approving = true;
        this.CurrentSession.CloseCurrentWindow();        
    }
}

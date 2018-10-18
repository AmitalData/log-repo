import {Component} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './ApproveBuildShipmentComponent.html',
})

export class ApproveBuildShipmentComponent {
    constructor() {

    }

    SetWindowArgs() {
        
    }

    public Approving: boolean = false;

    CancelButtonClicked() {
        this.Approving = false;
        SessionLocator.CurrentSession.CloseCurrentWindowEmit("ok");
    }

    OkButtonClicked() {
        this.Approving = true;
        SessionLocator.CurrentSession.CloseCurrentWindow();        
    }
}
import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './FeaturesEventChangesComponent.html',
})

export class FeaturesEventChangesComponent {
    public Notes: string;
    constructor() {
        
    }

    SetWindowArgs(myChanges: string) {
        this.Notes = myChanges;
    }

    CloseClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
}
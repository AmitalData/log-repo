import {Component} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './FeaturesEventChangesComponent.html',
})

export class FeaturesEventChangesComponent {
    public Notes: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        
    }

    SetWindowArgs(myChanges: string) {
        this.Notes = myChanges;
    }

    CloseClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

import { Component } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './OccasionWorkspaceComponent.html',
})

export class OccasionWorkspaceComponent  {
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {

    }

    NewOccasionClicked() {

    }
}

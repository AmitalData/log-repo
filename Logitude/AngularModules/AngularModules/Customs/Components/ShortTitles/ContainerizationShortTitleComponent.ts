
import { Component, ChangeDetectorRef } from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    
    templateUrl: "ContainerizationShortTitleComponent.html",
})

export class ContainerizationShortTitleComponent {
     private CurrentSession = SessionLocator.SelectedSession;
  
    RefreshButtonClicked() {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    }



 }

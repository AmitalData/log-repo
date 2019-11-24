import {Component} from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: "./InterestBasesTypeShortTitleComponent.html",
})

export class InterestBasesTypeShortTitleComponent {
    public EntityPM: GLAccountPM;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();
    }

    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                }
            });

            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

                }
            });
        }
    } 
}

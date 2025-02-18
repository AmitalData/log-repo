import { Component } from '@angular/core';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { ContainerPM } from '../../EntityPMs/ContainerPM';
import { AppTool } from '../../../Infrastructure/Tools';

@Component({

    templateUrl: "ContainerShortTitleComponent.html",
})

export class ContainerShortTitleComponent {

    public EntityPM: ContainerPM;
    constructor(public entityArgs: EntityArgs) {
        this.EntityPM = this.entityArgs.EntityPM;

        if (this.EntityPM != null) {
            this.BuildComponent();
        }

        this.Listen();
    }

    private LoadCompletedEvent: any = null;
    private SessionEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.BuildComponent();
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    private BuildComponent() {

    }

    get IsCancelled() { return this.EntityPM.IsCancelled; }
}

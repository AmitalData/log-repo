import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    templateUrl: './CheckAllInviteeCheckBoxComponent.html',
})

export class CheckAllInviteeCheckBoxComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {

    }
    
    public IsEnabled: boolean = true;
    setVariables() {

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    private isChecked: boolean;
    get IsChecked() { return this.isChecked; }
    set IsChecked(newValue: boolean) {
        if (this.isChecked != newValue) {
            this.isChecked = newValue;

            if (newValue) {
                this.CurrentSession.PseventRowSelectEvent.emit({ Name: "AddAll"});
            }

            else {
                this.CurrentSession.PseventRowSelectEvent.emit({ Name: "RemoveAll"});
            }
        }
    }
}


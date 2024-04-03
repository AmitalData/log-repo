import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './TestMultiHarmonizeComponent.html',
})

export class TestMultiHarmonizeComponent {
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        
    }

    private isChecked: boolean = false;
    get IsChecked() { return this.isChecked; }
    set IsChecked(value: boolean) {
        if (this.isChecked != value) {
            this.isChecked = value;
        }
    }

    OkClicked() {
        var checked: string = this.IsChecked ? "multi" : null
        this.CurrentSession.CloseCurrentWindowEmit(checked);
    }
}

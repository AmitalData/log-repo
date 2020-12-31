import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    templateUrl: './UploadedExcelsComponent.html',
})

export class UploadedExcelsComponent {
    public TariffId: string;
    public Version: number;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetWindowArgs(args: any) {
        this.TariffId = args['TariffId'];
        this.Version = args['Version'];
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}


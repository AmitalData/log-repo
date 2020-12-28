import { Component } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { CustomsTransferLinePM } from '../../../../Shipment/EntityPMs/CustomsTransferLinePM';

@Component({

    templateUrl: './AMANACValidationComponent.html',
})

export class AMANACValidationComponent {
    public TransferTypeCode: string = null;
    public ItemsSource: CustomsTransferLinePM[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        
    }

    SetWindowArgs(lines: CustomsTransferLinePM[]) {
        this.ItemsSource = lines.filter(d => d.HasError);
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}

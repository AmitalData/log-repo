import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceLocator } from '../../../../Infrastructure/Locators/ServiceLocator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { Component } from '@angular/core';

@Component({
    
    templateUrl: './WarningApprovePaymentComponent.html'
})

export class WarningApprovePaymentComponent {
    public RTL: boolean = true;
    public WarningMessage;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }
    
    SetWindowArgs(args: any) {
        if (args) {
            this.RTL = args.RTL;
            this.SetWarningMessage(args.WarningCode);
        }
    }

    SetWarningMessage(WarningCode: string) {
        switch (WarningCode) {
            case '17':
                this.WarningMessage = '"נדרש אישור לתשלום על סעיף מכס בסך <br/>(tax_details-tax_amount) ש"ח, האם אתה מאשר?';
                break;
            case '16':
                this.WarningMessage = '"נדרש אישור לתשלום על סעיף מס קניה בסך <br/>(tax_details - tax_amount) ש"ח, האם אתה מאשר?';
                break;
            case '1':
                this.WarningMessage = 'נדרש אישור לתשלום(tax_details-tax_amount) <br/>ש”ח בגין סעיף מכס וכן מס קניה(tax_details - tax_amount)<br/>ש"ח, האם אתה מאשר?';
                break;
            default:
                this.WarningMessage = '';
        }
    }

    ApproveButtonClicked() {
        this.CurrentSession.FireEvent("Approved");
        this.CurrentSession.CloseCurrentWindow();
    }

    DennyButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}

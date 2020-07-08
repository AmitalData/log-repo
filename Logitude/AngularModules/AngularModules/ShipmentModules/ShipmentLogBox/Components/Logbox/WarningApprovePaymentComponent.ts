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
    private tax1Amount: number = 0;
    private tax16Amount: number = 0;
    constructor() {
    }
    
    SetWindowArgs(args: any) {
        if (args) {
            this.RTL = args.RTL;
            this.tax1Amount = args.tax1Amount;
            this.tax16Amount = args.tax16Amount;
            this.SetWarningMessage(args.WarningCode);
        }
    }

    SetWarningMessage(WarningCode: string) {
        switch (WarningCode) {
            case '16':
                this.WarningMessage = 'נדרש אישור לתשלום על סעיף מס קניה בסך  ' + this.tax16Amount + '  ש"ח, <br/>האם אתה מאשר ?';
                break;
            case '1':
                this.WarningMessage = 'נדרש אישור לתשלום על סעיף מכס בסך  ' + this.tax1Amount + '  ש"ח, <br/>האם אתה מאשר ?';
                break;
            case '17':
                this.WarningMessage = 'נדרש אישור לתשלום ' + this.tax1Amount + '  ש”ח <br/>בגין סעיף מכס וכן מס קניה בסך ' + this.tax16Amount + '  ש"ח, <br/>האם אתה מאשר ?';
                break;
            default:
                this.WarningMessage = '';
        }
    }

    ApproveButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Approved");
    }

    DennyButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}

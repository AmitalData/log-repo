import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceLocator } from '../../../../Infrastructure/Locators/ServiceLocator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { Component } from '@angular/core';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';

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
            //if (ObjectsLocator.GlobalSetting) this.RTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
            this.tax1Amount = args.tax1Amount;
            this.tax16Amount = args.tax16Amount;
            if (this.RTL) this.SetWarningMessage(args.WarningCode);
            else this.SetWarningMessageInEnglish(args.WarningCode);
        }
    }

    SetWarningMessage(WarningCode: string) {
        switch (WarningCode) {
            case '16':
                this.WarningMessage = 'אנא תשומת ליבך כי בנוסף לתשלום המע"מ, <br/> יש לנו צורך באישורך לשלם: <br/> מס קנייה בסך של ' + this.tax16Amount + ' ₪<br/> האם אתה מאשר?';
                break;
            case '1':
                this.WarningMessage = 'אנא תשומת ליבך כי בנוסף לתשלום המע"מ, <br/> יש לנו צורך באישורך לשלם: <br/> מכס בסך של ' + this.tax1Amount + ' ₪<br/> האם אתה מאשר?';
                break;
            case '17':
                this.WarningMessage = 'אנא תשומת ליבך כי בנוסף לתשלום המע"מ, <br/> יש לנו צורך באישורך לשלם: <br/> מכס בסך של ' + this.tax1Amount + ' ₪<br/> מס קנייה בסך של ' + this.tax16Amount + ' ₪<br/> האם אתה מאשר?';
                break;
            default:
                this.WarningMessage = '';
        }
    }

    SetWarningMessageInEnglish(WarningCode: string) {
        switch (WarningCode) {
            case '16':
                this.WarningMessage = 'Please note that in addition to paying VAT, <br/> We need your approval to pay: <br/> Purchase tax in the amount of ' + this.tax16Amount + ' ₪<br/> Do you confirm?';
                break;
            case '1':
                this.WarningMessage = 'Please note that in addition to paying VAT, <br/> We need your approval to pay: <br/> Customs in the amount of ' + this.tax1Amount + ' ₪<br/> Do you confirm?';
                break;
            case '17':
                this.WarningMessage = 'Please note that in addition to paying VAT, <br/> We need your approval to pay: <br/> Customs in the amount of ' + this.tax1Amount + ' ₪<br/> Purchase tax in the amount of ' + this.tax16Amount + ' ₪<br/> Do you confirm?';
                break;
            default:
                this.WarningMessage = '';
        }
    }

    ApproveButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Approved");
    }

    DenyButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("Deny");
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}

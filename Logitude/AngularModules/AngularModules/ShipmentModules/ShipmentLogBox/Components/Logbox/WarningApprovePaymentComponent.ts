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
            this.SetWarningMessage(args.WarningCode);
            //if (this.RTL) this.SetWarningMessage(args.WarningCode);
            //else this.SetWarningMessageInEnglish(args.WarningCode);
        }
    }

    SetWarningMessage(WarningCode: string) {
        var approvalPaymentWarning: string = TextCodeTranslator.Translate("General.M.ApprovalPaymentWarning");
        approvalPaymentWarning = approvalPaymentWarning.split("%br").join("<br/>");
        
        switch (WarningCode) {
            case '16':
                var approvalPaymentWarningSplit1: string = approvalPaymentWarning.split("%PurchaseTax")[0];
                var approvalPaymentWarningSplit2: string = approvalPaymentWarning.split("%CustomsTax")[1]?.split("<br/>")[1];
                approvalPaymentWarning = approvalPaymentWarningSplit1 + this.tax16Amount + " ₪ <br>" + approvalPaymentWarningSplit2;
                this.WarningMessage = approvalPaymentWarning;
                break;
            case '1':
                var approvalPaymentWarningSplit3 = approvalPaymentWarning.split(":");
                var approvalPaymentWarningSplit4 = approvalPaymentWarningSplit3[1]?.split("%PurchaseTax")[1];
                approvalPaymentWarning = approvalPaymentWarningSplit3[0] + ":" + approvalPaymentWarningSplit4.substring(2);
                approvalPaymentWarning = approvalPaymentWarning.replace("%CustomsTax", this.tax1Amount?.toString());
                this.WarningMessage = approvalPaymentWarning;
                break;
            case '17':
                approvalPaymentWarning = approvalPaymentWarning.replace("%PurchaseTax", this.tax16Amount?.toString());
                approvalPaymentWarning = approvalPaymentWarning.replace("%CustomsTax", this.tax1Amount?.toString());
                this.WarningMessage = approvalPaymentWarning;
                break;
            default:
                this.WarningMessage = '';
        }
    }

    //SetWarningMessage(WarningCode: string) {
    //    switch (WarningCode) {
    //        case '16':
    //            this.WarningMessage = 'אנא תשומת ליבך כי בנוסף לתשלום המע"מ, <br/> יש לנו צורך באישורך לשלם: <br/> מס קנייה בסך של ' + this.tax16Amount + ' ₪<br/> האם אתה מאשר?';
    //            break;
    //        case '1':
    //            this.WarningMessage = 'אנא תשומת ליבך כי בנוסף לתשלום המע"מ, <br/> יש לנו צורך באישורך לשלם: <br/> מכס בסך של ' + this.tax1Amount + ' ₪<br/> האם אתה מאשר?';
    //            break;
    //        case '17':
    //            this.WarningMessage = 'אנא תשומת ליבך כי בנוסף לתשלום המע"מ, <br/> יש לנו צורך באישורך לשלם: <br/> מכס בסך של ' + this.tax1Amount + ' ₪<br/> מס קנייה בסך של ' + this.tax16Amount + ' ₪<br/> האם אתה מאשר?';
    //            break;
    //        default:
    //            this.WarningMessage = '';
    //    }
    //}

    //SetWarningMessageInEnglish(WarningCode: string) {
    //    switch (WarningCode) {
    //        case '16':
    //            this.WarningMessage = 'Please note that in addition to paying VAT, <br/> We need your approval to pay: <br/> Purchase tax in the amount of ' + this.tax16Amount + ' ₪<br/> Do you confirm?';
    //            break;
    //        case '1':
    //            this.WarningMessage = 'Please note that in addition to paying VAT, <br/> We need your approval to pay: <br/> Customs in the amount of ' + this.tax1Amount + ' ₪<br/> Do you confirm?';
    //            break;
    //        case '17':
    //            this.WarningMessage = 'Please note that in addition to paying VAT, <br/> We need your approval to pay: <br/> Customs in the amount of ' + this.tax1Amount + ' ₪<br/> Purchase tax in the amount of ' + this.tax16Amount + ' ₪<br/> Do you confirm?';
    //            break;
    //        default:
    //            this.WarningMessage = '';
    //    }
    //}

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

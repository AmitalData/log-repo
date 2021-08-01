import { ChangeDetectorRef, Component } from "@angular/core";
import { BaseComponent } from "../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { SessionLocator } from "../../../../../Infrastructure/Utilities/SessionLocator";
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { CustomsItemPM } from "../../../../../Customs/EntityPMs/CustomsItemPM";
import { AppTool } from "../../../../../Infrastructure/Tools";
import { TextCodeTranslator } from "../../../../../Infrastructure/Utilities/TextCodeTranslator";
import { LuhnAlgorithm } from "../../../../../Customs/Utilities/LuhnAlgorithm";
import { ConfirmWindow } from "../../../../../Controls/Windows/ConfirmWindow";
import { SendMultiUpdateRequestParams } from "../../../../../Customs/DataContract/RequestParams/SendMultiUpdateRequestParams";
import { SupplierInvoiceService } from "../../../../../Customs/Services/Others/SupplierInvoiceService";
import { MessageWindow } from "../../../../../Controls/Windows/MessageWindow";

@Component({

    templateUrl: './MultiUpdateComponent.html',
})

export class MultiUpdateComponent extends BaseComponent {
    DataContext: any = this;
    public ValidationErrorsList: string[] = [];
    IsDisplayOnly: boolean = false;
    IsReady: boolean = false;
    CustomItemErrorMessage: string;
    private CurrentSession = SessionLocator.SelectedSession;
    _SupplierInvoiceService: SupplierInvoiceService = new SupplierInvoiceService();
    constructor(private EntityResourceService: EntityResourceService, private cd: ChangeDetectorRef) {
        super();
        this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemProcesType").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsItem").subscribe((response: any) => {
                    this.IsReady = true;
                });
            });
        });

        this.SetUIProperties();
    }
    SetUIProperties() {
    }

    SetWindowArgs(args: any) {
    }

    customsItem: string;
    get CustomsItem() { return this.customsItem; }
    set CustomsItem(value: string) {
        this.customsItem = value;
    }

    CustomsItemClicked(item: CustomsItemPM) {
        if (item) {
            this.CustomsItem = null;
            this.cd.detectChanges();
            this.CustomsItem = item.FullClassification;
        }
    }

    customsItemTextValue: string;
    get CustomsItemTextValue() { return this.customsItemTextValue; }
    set CustomsItemTextValue(value: string) {
        this.customsItemTextValue = value;
    }
    CustomsItemTextChanged(text: string) {
        this.CustomsItemTextValue = text;
    }


    CustomsItemLostFocus(text: string) {
        if (!AppTool.IsNullOrEmpty(text))
            this.ValidateCustomsItemField(text);
        this.TaxExemptCode = this.CustomsItemTextValue;
        this.CustomsItem = this.CustomsItemTextValue;
    }

    updateAll: boolean;
    get UpdateAll() { return this.updateAll }
    set UpdateAll(value: boolean) {
        this.updateAll = value;
        if (value) {
            this.UpdateSelected = false;
            this.UIProperties.SetEnabled("ClassificationCode", null, false);
        }
    }

    updateSelected: boolean;
    get UpdateSelected() { return this.updateSelected }
    set UpdateSelected(value: boolean) {
        this.updateSelected = value;
        if (value) {
            this.UpdateAll = false;
            this.UIProperties.SetEnabled("ClassificationCode", null, true);
        }
    }

    UpdateAllRadio(newValue: boolean) {
        this.UpdateAll = newValue;
    }

    UpdateSelectedRadio(newValue: boolean) {
        this.UpdateSelected = newValue;
    }

    hasDash: boolean = false;
    digit: string = null;
    checkDigit: number = 0;

    ValidateCustomsItemField(text: string = null) {

        this.CustomItemErrorMessage = null;

        var valid: boolean = true;

        if (!this.CustomsItemTextValue && this.CustomsItem) // set the value when entering the window
            this.CustomsItemTextValue = this.CustomsItem;

        if (text) {
            this.CustomsItemTextValue = text;
            this.CustomsItem = text;
        }

        var value: string = this.CustomsItemTextValue;
        if (value) {
            this.hasDash = value.includes("-");
            value = value.replace("-", "");
        }



        if (AppTool.IsNullOrEmpty(value)) {
            this.CustomItemErrorMessage = null;
        }
        else if (value.toString().length > 12) {
            this.CustomItemErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong");
            valid = false;
        }
        else if (value.toString().length < 8) {
            this.CustomItemErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort");
            valid = false;
        }
        else if (value.toString().length == 8) {
            value = value + "00";

            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(value);
            if (this.checkDigit)
                value = value + this.checkDigit;
        }
        else if (value.toString().length == 9) {
            this.digit = value.toString().substring(8);

            value = value.toString().substring(0, 8) + "00" + value.toString().substring(8);
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(value.substring(0, 10));

            if (this.digit != this.checkDigit.toString()) {
                this.CustomItemErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString();
                valid = false;
            }
            else {
            }

        }
        else if (value.toString().length == 10) {
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(value);
            if (this.checkDigit)
                value = value + "" + this.checkDigit;

        }
        else if (value.toString().length == 11) {
            this.digit = value.toString().substring(10);
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(value.toString().substring(0, 10));
            if (this.digit != this.checkDigit.toString()) {
                this.CustomItemErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString();
                valid = false;
            }
            else {

            }


        }
        else {
            this.CustomItemErrorMessage = null;
        }

        this.CustomsItemTextValue = value;

        if (this.hasDash)
            this.CustomsItemTextValue = "-" + this.CustomsItemTextValue;

        return valid;
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var errors = [];
        if (!this.UpdateAll && !this.UpdateSelected) {
            errors.push("בחר פריטים לעדכון");
        }
        if (this.ProcessTypeCode == null || this.TaxExemptCode ) {
            errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.ProcessTypeRequired"));
        }
        if (this.UpdateSelected ) {
            errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.SelectItems"));
        }
        this.ValidationErrorsList = errors;

      /*  var multiUpdateValues: MultiUpdateValues;
        debugger;
        multiUpdateValues.ProcessTypeCode = this.ProcessTypeCode;
        multiUpdateValues.TaxExemptCode = this.TaxExemptCode;
        multiUpdateValues.ClassificationCode = this.ClassificationCode;
        multiUpdateValues.tenant = this.CurrentSession.CurrentEditComponent.EntityPM.Tenant;
        multiUpdateValues.Declarationid = this.CurrentSession.CurrentEditComponent.EntityPM.Id;*/
        if (errors.length == 0) {
            var confirm = new ConfirmWindow();
            confirm.Width = 320;
            confirm.Height = 180;
            confirm.Title = "עדכון קוד תהליך/הנחה פטור";
            confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
            confirm.ShowNoButton = true;
            confirm.NoButtonText = TextCodeTranslator.Translate("Customs.General.B.Cancel");
            confirm.Show(" שינוי יבצע עדכון קוד התהליך/הנחה פטור באופן גורף לכל שורות פרטי המכס או לחלקן");
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    this.SendMultiUpdate();
                }
                confirm.Close();
            });
        }
    }
    SendMultiUpdate() {
        var currRequestParams = new SendMultiUpdateRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.ProcessTypeCode = this.ProcessTypeCode;
        currRequestParams.ClassificationCode = this.ClassificationCode;
        currRequestParams.TaxExemptCode = this.TaxExemptCode;
        currRequestParams.Declarationid = this.CurrentSession.CurrentEditComponent.EntityPM.Id;


        this._SupplierInvoiceService.PostSendMultiUpdate(currRequestParams)
            .subscribe((res: any) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                myMessageWindow.Show(res.Result);
                myMessageWindow.WindowClosed.subscribe(s => {
                    this.CancelButtonClicked();
                });
            });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("");
    }

    processTypeCode: string;
    get ProcessTypeCode() { return this.processTypeCode; }
    set ProcessTypeCode(value: string) {
        this.processTypeCode = value;
    }

    taxExemptCode: string;
    get TaxExemptCode() { return this.taxExemptCode; }
    set TaxExemptCode(value: string) {
        this.taxExemptCode = value;
    }

    classificationCode: string;
    get ClassificationCode() { return this.classificationCode }
    set ClassificationCode(value: string) { this.classificationCode = value; }
}

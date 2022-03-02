import { ChangeDetectorRef, Component } from "@angular/core";
import { BaseComponent } from "../../../../Infrastructure/Components/LogitudeComponents/BaseComponent";
import { SessionLocator } from "../../../../Infrastructure/Utilities/SessionLocator";
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { CustomsItemPM } from "../../../../Customs/EntityPMs/CustomsItemPM";
import { AppTool } from "../../../../Infrastructure/Tools";
import { TextCodeTranslator } from "../../../../Infrastructure/Utilities/TextCodeTranslator";
import { LuhnAlgorithm } from "../../../../Customs/Utilities/LuhnAlgorithm";
import { ConfirmWindow } from "../../../../Controls/Windows/ConfirmWindow";
import { SendMultiUpdateRequestParams } from "../../../../Customs/DataContract/RequestParams/SendMultiUpdateRequestParams";
import { SupplierInvoiceService } from "../../../../Customs/Services/Others/SupplierInvoiceService";
import { MessageWindow } from "../../../../Controls/Windows/MessageWindow";
import { PendingWebService } from "../../../../Customs/Services/WebServices/PendingWebService";
import { ApiQueryFilters } from "../../../../Infrastructure/DataContracts/ApiQueryFilters";

@Component({

    templateUrl: './MultiUpdateDecComponent.html',
})

export class MultiUpdateDecComponent extends BaseComponent {
    DataContext: any = this;
    public ValidationErrorsList: string[] = [];
    IsDisplayOnly: boolean = false;
    notUpdateSelf: boolean = false;
    declarationIdsList: string[] = [];
    allWithoutdeclarationIdsList: string[] = [];
    filter: ApiQueryFilters = null as any;

    checkboxAll: boolean;
    courierMasterId: string;
    IsReady: boolean = false;
    CustomItemErrorMessage: string;
    private CurrentSession = SessionLocator.SelectedSession;
    _SupplierInvoiceService: SupplierInvoiceService = new SupplierInvoiceService();
    
    constructor(private EntityResourceService: EntityResourceService, private cd: ChangeDetectorRef, private pendingWebService: PendingWebService) {
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
        this.UIProperties.SetEnabled("ClassificationCode", "Customs.SupplierInvoiceItem", false);
    }

    SetWindowArgs(args: any) {

        this.notUpdateSelf = args.notUpdateSelf;
        this.declarationIdsList = args.declarationIdsList;
        this.allWithoutdeclarationIdsList = args.allWithoutdeclarationIdsList;
        this.checkboxAll = args.checkboxAll;
        this.filter = args.filter;
        this.courierMasterId = args.courierMasterId;
        this.IsDisplayOnly = args.IsDisplayOnly;
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
            this.UIProperties.SetEnabled("ClassificationCode", "Customs.SupplierInvoiceItem", false);
        }
    }

    updateSelected: boolean;
    get UpdateSelected() { return this.updateSelected }
    set UpdateSelected(value: boolean) {
        this.updateSelected = value;
        if (value) {
            this.UpdateAll = false;
            this.UIProperties.SetEnabled("ClassificationCode", "Customs.SupplierInvoiceItem", true);
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

    ClassificationKeyUp(event,classificationTextBox: any) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnClassificationLostFocus(classificationTextBox);
        }
    }

    valid: boolean = true;
    OnClassificationLostFocus(classificationTextBox: any) {
        var newValue = this.ClassificationCode;
        this.valid = true;
        this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");

        if (AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");
        }
        else if (newValue.toString().length > 11) {
            this.valid = false;
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong"));

        }
        else if (newValue.toString().length < 8) {
            this.valid = false;
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort"));

        }
        else if (newValue.toString().length == 8) {
            newValue = newValue + "00";
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + this.checkDigit;
            this.valid = true;;

        }
        else if (newValue.toString().length == 9) {
            this.digit = newValue.toString().substring(8);

            newValue = newValue.toString().substring(0, 8) + "00" + newValue.toString().substring(8);
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.substring(0, 10));

            if (this.digit != this.checkDigit.toString()) {
                this.valid = false;
                this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString());

            }
            else {
                this.valid = true;
            }

        }
        else if (newValue.toString().length == 10) {
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + "" + this.checkDigit;
            this.valid = true;

        }
        else if (newValue.toString().length == 11) {
            this.digit = newValue.toString().substring(10);
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.toString().substring(0, 10));
            if (this.digit != this.checkDigit.toString()) {
                this.valid = false;
                this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString());

            }
            else {
                this.valid = true;
            }


        }

        else {
            this.valid = true;
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");
        }

        this.ClassificationCode = newValue;
        classificationTextBox.TextValue = newValue;
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var errors = [];
        if (this.ProcessTypeCode == null && this.TaxExemptCode == null ) {
            errors.push("חובה להזין שדה קוד");
        }
        if (!this.UpdateAll && !this.UpdateSelected) {
            errors.push("בחר פריטים לעדכון");
        }
        this.ValidationErrorsList = errors;
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
    async SendMultiUpdate() {
        var currRequestParams = new SendMultiUpdateRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.ProcessTypeCode = this.ProcessTypeCode;
        currRequestParams.ClassificationCode = this.ClassificationCode;
        currRequestParams.TaxExemptCode = this.TaxExemptCode;
        //currRequestParams.Declarationid = this.CurrentSession.CurrentEditComponent.EntityPM.Id;
        currRequestParams.DeclarationIds = this.declarationIdsList;
        currRequestParams.CourierMasterId = this.courierMasterId;
        currRequestParams.checkboxAll = this.checkboxAll;
        currRequestParams.allWithoutdeclarationIdsList = this.allWithoutdeclarationIdsList;
        SessionLocator.SelectedSession.StartBusyIndicator("");
        //this._SupplierInvoiceService.PostSendMultiUpdate(currRequestParams)
        //    .subscribe((res: any) => {
        //        SessionLocator.SelectedSession.StopBusyIndicator();
        //        var myMessageWindow = new MessageWindow();
        //        myMessageWindow.Show(res.Result);
        //        myMessageWindow.WindowClosed.subscribe(s => {
        //            //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        //            this.CancelButtonClicked();
        //        });
        //    });

        const msg: string = await this.pendingWebService.PostSendMultiUpdate(currRequestParams,this.filter)
        SessionLocator.SelectedSession.StopBusyIndicator();
        var myMessageWindow = new MessageWindow();
        myMessageWindow.Show(msg);
        myMessageWindow.WindowClosed.subscribe(s => {
            this.CancelButtonClicked();
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

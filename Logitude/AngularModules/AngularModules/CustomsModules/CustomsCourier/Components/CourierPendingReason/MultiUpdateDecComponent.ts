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
import { customsItemsService } from "QuoteOPM/Utilities/customsItems.service";
import { combineLatest, forkJoin } from "rxjs";
import { DeclarationsBulkFeedWebService } from "Customs/Services/WebServices/DeclarationsBulkFeedWebService";
import { EntityArgs } from "Infrastructure/DataContracts/EntityArgs";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";

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
    taxExemptCodeTypesFilter: ApiQueryFilters = customsItemsService.initTaxExemptCodeTypesFilter(false);
    
    constructor(private EntityResourceService: EntityResourceService, private cd: ChangeDetectorRef, private pendingWebService: PendingWebService) {
        super();

        combineLatest([
            this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemProcesType"),
            this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem"),
            this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice"),
            this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsItem"),
            this.EntityResourceService.getEntityResourceByTableName("Customs.CurrencyType"),
            this.EntityResourceService.getEntityResourceByTableName("Customs.MeasurmentUnit"),
            this.EntityResourceService.getEntityResourceByTableName("Customs.CourierMaster"),
        ]).subscribe((response: any) => this.IsReady = true);
     
        this.SetUIProperties();
    }

    SetUIProperties() {
        //this.UIProperties.SetEnabled("ClassificationCode", "Customs.SupplierInvoiceItem", false);
    }

    async SetWindowArgs(args: any) {
        this.notUpdateSelf = args.notUpdateSelf;
        this.declarationIdsList = args.declarationIdsList;
        this.allWithoutdeclarationIdsList = args.allWithoutdeclarationIdsList;
        this.checkboxAll = args.checkboxAll;
        this.filter = args.filter;
        this.courierMasterId = args.courierMasterId;
        this.IsDisplayOnly = args.IsDisplayOnly;
        
        this.checkDeclarationsInDisplayOnly();
    }

    declarationsDisplayOnly: string[];
    declarationsDisplayOnlyPromise: Promise<any>;
    customsItemTextValue: string;
    procedureCurrentCode: string;
    taxExemptCode: string;
    classificationCode: string;
    customsItem: string;
    invoiceAmount: string;
    invoiceCurrencyTypeCode: string;
    invoiceQuantity: string;
    invoiceQuantityType: string;
    grossMassMeasure: string;

    get CustomsItemTextValue() { return this.customsItemTextValue; }
    set CustomsItemTextValue(value: string) { this.customsItemTextValue = value;}

    get ProcedureCurrentCode() { return this.procedureCurrentCode; }
    set ProcedureCurrentCode(value: string) { this.procedureCurrentCode = value; }

    get TaxExemptCode() { return this.taxExemptCode; }
    set TaxExemptCode(value: string) { this.taxExemptCode = value; }

    get ClassificationCode() { return this.classificationCode }
    set ClassificationCode(value: string) { this.classificationCode = value; }
    
    get CustomsItem() { return this.customsItem; }
    set CustomsItem(value: string) { this.customsItem = value; }
    
    get InvoiceAmount() { return this.invoiceAmount; }
    set InvoiceAmount(newValue: string) { this.invoiceAmount = newValue; }
    
    get InvoiceCurrencyTypeCode() { return this.invoiceCurrencyTypeCode; }
    set InvoiceCurrencyTypeCode(newValue: string) { this.invoiceCurrencyTypeCode = newValue; }
    
    get InvoiceQuantity() { return this.invoiceQuantity; }
    set InvoiceQuantity(newValue: string) { this.invoiceQuantity = newValue; }
    
    get InvoiceQuantityType() { return this.invoiceQuantityType; }
    set InvoiceQuantityType(newValue: string) { this.invoiceQuantityType = newValue; }
    
    get GrossMassMeasure() { return this.grossMassMeasure; }
    set GrossMassMeasure(newValue: string) { this.grossMassMeasure = newValue; }

    CustomsItemClicked(item: CustomsItemPM) {
        if (item) {
            this.CustomsItem = null;
            this.cd.detectChanges();
            this.CustomsItem = item.FullClassification;
        }
    }

    async checkDeclarationsInDisplayOnly() {        
        this.declarationsDisplayOnlyPromise = new DeclarationsBulkFeedWebService().checkDeclarationsInDisplayOnly(this.declarationIdsList, this.allWithoutdeclarationIdsList, this.checkboxAll, this.filter);
        this.declarationsDisplayOnly = await this.declarationsDisplayOnlyPromise;
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

    OnInvoiceNumberLostFocus(invoiceNumberTextBox: any) {

        // if (this.declarationPM != null && this.declarationPM.SupplierInvoices.length >= 0) {
        //     if (this.EntityPM.InvoiceNumber) {

        //         //client method
        //         var invoices: SupplierInvoicePM[] = [];
        //         invoices = this.declarationPM.SupplierInvoices;
        //         invoices = invoices.concat(this.Parent.NewInvoices);
        //         var exist = invoices.find(d => d.InvoiceNumber == this.EntityPM.InvoiceNumber);
        //         if (exist) {
        //             //show confirm window
        //             var newValue = this.InvoiceNumber;
        //             var confirm = new ConfirmWindow();
        //             confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
        //             confirm.ShowNoButton = true;
        //             confirm.Show(" קיים כבר חשבון ספק עם מספר חשבון זהה - שורה" + exist.SequenceNumeric + "- הםם להמשיך ?");
        //             confirm.WindowClosed.subscribe((event: any) => {
        //                 confirm.Close();
        //                 this.InvoiceNumber = newValue;
        //                 if (confirm.Yes) {
        //                     SessionLocator.SustainFocusOnCell = false;
        //                 }
        //                 else {
        //                     SessionLocator.SustainFocusOnCell = true;
        //                     console.log(invoiceNumberTextBox.InputId);
        //                     var element = document.getElementById(invoiceNumberTextBox.InputId);
        //                     if (element) {
        //                         element.focus();
        //                     }
        //                 }
        //             });
        //         }
        //     }
        // }
    }

    InvoiceAmountBlur(text) {
        // if (this.old_amount != this.EntityPM.InvoiceAmount ||
        //     this.old_currency != this.EntityPM.InvoiceCurrencyTypeCode ||
        //     this.old_vendor != this.EntityPM.VendorId)
        //     this.Parent.CalculateCommissionPercentage();

        // this.old_amount = this.EntityPM.InvoiceAmount;
        // this.old_currency = this.EntityPM.InvoiceCurrencyTypeCode;
        // this.old_vendor = this.EntityPM.VendorId;
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var errors = [];
        if ([
            this.ProcedureCurrentCode,
            this.TaxExemptCode,
            this.classificationCode,
            this.invoiceAmount,
            this.invoiceCurrencyTypeCode,
            this.invoiceQuantity,
            this.invoiceQuantityType,
            this.grossMassMeasure,
        ].every(x => x == null)
        ) {
            errors.push("חובה להזין אחד מהשדות לעדכון");
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
            confirm.Show("שינוי יבצע עדכון גורף של קוד תהליך בהצהרות,\n ועדכון קוד הנחה פטור לכל שורות פרטי המכס");
            confirm.WindowClosed.subscribe((event: any) => {
                if (confirm.Yes) {
                    this.SendMultiUpdate();
                }
                confirm.Close();
            });
        }
    }

    async showMassageExistDeclarationsDisplayOnly() {
        await this.declarationsDisplayOnlyPromise;
        if(!this.declarationsDisplayOnly?.length) return;

        await this.showMessageDisplayOnly();

        if(this.checkboxAll)
            this.allWithoutdeclarationIdsList = this.allWithoutdeclarationIdsList.concat(this.declarationsDisplayOnly)
        else
            this.declarationIdsList = this.declarationIdsList.filter(x => !this.declarationsDisplayOnly.includes(x));
    }

    private async showMessageDisplayOnly() {
        await new Promise<void>((resolve) => {
            const msg: MessageWindow = new MessageWindow();
            msg.Show(TextCodeTranslator.Translate("Customs.CourierMaster.O.DisplayOnly"));
            msg.WindowClosed.subscribe(() => resolve());
        });
    }

    async SendMultiUpdate() {
        await this.showMassageExistDeclarationsDisplayOnly();

        var currRequestParams = new SendMultiUpdateRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator.Tenant;
        currRequestParams.ProcessTypeCode = this.ProcedureCurrentCode;
        currRequestParams.ClassificationCode = this.ClassificationCode;
        currRequestParams.TaxExemptCode = this.TaxExemptCode;
        //currRequestParams.Declarationid = this.CurrentSession.CurrentEditComponent.EntityPM.Id;
        currRequestParams.DeclarationIds = this.declarationIdsList;
        currRequestParams.CourierMasterId = this.courierMasterId;
        currRequestParams.checkboxAll = this.checkboxAll;
        currRequestParams.allWithoutdeclarationIdsList = this.allWithoutdeclarationIdsList;
        currRequestParams.InvoiceAmount = this.invoiceAmount;
        currRequestParams.InvoiceCurrencyTypeCode = this.InvoiceCurrencyTypeCode;
        currRequestParams.InvoiceQuantity = this.InvoiceQuantity;
        currRequestParams.InvoiceQuantityType = this.InvoiceQuantityType;
        currRequestParams.GrossMassMeasure = this.GrossMassMeasure;

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


        this.pendingWebService.PostSendMultiUpdate(currRequestParams, this.filter)
            .subscribe((res: any) => {
                SessionLocator.SelectedSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow();
                if(!AppTool.IsNullOrEmpty(res.RequestInProgressList)){
                    myMessageWindow.ShowEventButton=true;
                    myMessageWindow.EventButtonText=TextCodeTranslator.Translate("Customs.Declaration.TH.RequestSheet");
                }  
                myMessageWindow.Show(res.Message);
                myMessageWindow.WindowClosed.subscribe(s => {
                    //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.CancelButtonClicked();
                });
                myMessageWindow.SendEvent.subscribe(s=>{
                    if(s){
                        this.LoadCustomsRequestSheetsScreen(res.RequestInProgressList)
                    }
                });
            });

        //const msg: string = await this.pendingWebService.PostSendMultiUpdate(currRequestParams,this.filter)
        //SessionLocator.SelectedSession.StopBusyIndicator();
        //var myMessageWindow = new MessageWindow();
        //myMessageWindow.Show(msg);
        //myMessageWindow.WindowClosed.subscribe(s => {
        //    this.CancelButtonClicked();
        //});
    }
    LoadCustomsRequestSheetsScreen(RequestInProgressList:string){
       
        var entityArgs=new EntityArgs();
       // entityArgs.EntityPM = this.entityPM;
        entityArgs.ObjectTableName="Customs.CourierMaster";
        entityArgs.OriginEntity=RequestInProgressList;
        let windowTitle = TextCodeTranslator.Translate("TextCodeTranslator");
        let logWindow = new LogitudeWindow();
        logWindow.Width = 1300;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = true;
        logWindow.WindowArgs = entityArgs;
        
        logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent');


 
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("");
    }
}
